using DiskCardGame;
using HarmonyLib;
using UnityEngine;
using static GrimoraMod.GrimoraPlugin;

namespace GrimoraMod;

// [HarmonyPatch]
public class CardAppearanceBehaviourPatch
{
	public static Texture2D GravestoneGold = GravestoneTexture();
	public static readonly int Albedo = Shader.PropertyToID("_Albedo");

	internal static Texture2D GravestoneTexture()
	{
		Texture2D texture = new Texture2D(1, 1) { filterMode = FilterMode.Point };
		texture.LoadImage(FileUtils.ReadFileAsBytes("GravestoneCard_Base_Lowered2", true));

		return texture;
	}

	[HarmonyPrefix, HarmonyPatch(typeof(RareCardBackground), nameof(RareCardBackground.ApplyAppearance))]
	public static bool CorrectBehaviourForGrimora(ref RareCardBackground __instance)
	{
		if (GrimoraSaveUtil.isNotGrimora)
		{
			return true;
		}

		var renderer = __instance.Card.GetComponentInChildren<GravestoneRenderStatsLayer>();
		renderer.Material.SetTexture(Albedo, GravestoneGold);
		Log.LogDebug($"[RareCardBackground] {renderer} Set new gravestone layer for rare cards");

		return false;
	}

	[HarmonyPrefix, HarmonyPatch(typeof(GiantAnimatedPortrait), nameof(GiantAnimatedPortrait.ApplyAppearance))]
	public static bool GiantTesting(GiantAnimatedPortrait __instance)
	{
		if (GrimoraSaveUtil.isNotGrimora)
		{
			return true;
		}
		__instance.ApplyAppearance();

		var renderer = __instance.Card.GetComponentInChildren<GravestoneRenderStatsLayer>();
		renderer.Material.SetTexture(Albedo, GravestoneGold);
		Log.LogDebug($"[RareCardBackground] {renderer} Set new gravestone layer for rare cards");

		return false;
	}
}

// [HarmonyPatch(typeof(CardAbilityIcons))]
public class ChangeLogicInCardAbilityIcons
{
	// [HarmonyTranspiler]
	// private static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
	// {
	// 	Log.LogDebug($"Setting CardAbilityIcons.UpdateAbilityIcons");
	//
	// 	bool InstructionIsFieldInfo(CodeInstruction ins)
	// 		=> ins.opcode == OpCodes.Stfld && ins.operand is FieldInfo { Name: "debugTransition" };
	//
	// 	return new CodeMatcher(instructions)
	// 		.Start()
	// 		.MatchForward(false,
	// 			new CodeMatch(OpCodes.Ldarg_0),
	// 			new CodeMatch(OpCodes.Ldfld),
	// 			new CodeMatch(OpCodes.Ldloc_S),
	// 			new CodeMatch(OpCodes.Callvirt),
	// 			new CodeMatch(OpCodes.Ldc_I4_1),
	// 			new CodeMatch(OpCodes.Sub),
	// 			new CodeMatch(OpCodes.Callvirt)
	// 		)
	// 		.Advance(2)
	// 		.RemoveInstruction() // ldloc.s
	// 		.RemoveInstruction() // callvirt
	// 		.SetOpcodeAndAdvance(OpCodes.Ldc_I4_0)
	// 		.RemoveInstruction()
	// 		.InstructionEnumeration();
	// }

	[HarmonyPrefix, HarmonyPatch(nameof(CardAbilityIcons.SetColorOfDefaultIcons))]
	public static bool SetColorOfDefaultIconsChange(CardAbilityIcons __instance, Color color, bool inConduitCircuit)
	{
		if (GrimoraSaveUtil.isNotGrimora)
		{
			return true;
		}
		
		AbilityIconInteractable[] componentsInChildren = __instance.defaultIconGroups[3].GetComponentsInChildren<AbilityIconInteractable>();
		foreach (AbilityIconInteractable abilityIconInteractable in componentsInChildren)
		{
			AbilityInfo info = AbilitiesUtil.GetInfo(abilityIconInteractable.Ability);
			if (info is null) continue;
			if (info.hasColorOverride && !(info.conduitCell && inConduitCircuit))
			{
				abilityIconInteractable.SetColor(info.colorOverride);
			}
			else
			{
				abilityIconInteractable.SetColor(color);
			}
		}

		return false;
	}

	[HarmonyPrefix, HarmonyPatch(nameof(CardAbilityIcons.UpdateAbilityIcons))]
	public static bool PrefixTest(
		CardAbilityIcons __instance,
		CardInfo info,
		List<CardModificationInfo> mods,
		PlayableCard playableCard,
		List<Ability> hiddenAbilities)
	{
		if (GrimoraSaveUtil.isNotGrimora)
		{
			return true;
		}
		
		Log.LogDebug($"[CardAbilityIcons] DefaultIconGroups [{__instance.defaultIconGroups.Count}]");

		__instance.abilityIcons.Clear();
		List<Ability> distinctShownAbilities = __instance.GetDistinctShownAbilities(info, mods, hiddenAbilities);
		List<CardModificationInfo> modInfoCopy = new List<CardModificationInfo>(info.Mods);
		modInfoCopy.AddRange(mods);
		List<Ability> cardMergeAbilities = new List<Ability>();
		List<Ability> cardTotemAbilities = new List<Ability>();
		Ability ability = Ability.None;
		foreach (CardModificationInfo item in modInfoCopy)
		{
			foreach (Ability ability2 in item.abilities)
			{
				if (distinctShownAbilities.Contains(ability2))
				{
					if (item.fromCardMerge)
					{
						cardMergeAbilities.Add(ability2);
						distinctShownAbilities.Remove(ability2);
					}
					else if (item.fromTotem)
					{
						cardTotemAbilities.Add(ability2);
						distinctShownAbilities.Remove(ability2);
					}
				}

				if (item.fromLatch)
				{
					if (distinctShownAbilities.Contains(ability2))
					{
						distinctShownAbilities.Remove(ability2);
					}

					ability = ability2;
				}

				if (item.sideDeckMod && ability2 == Ability.ConduitNull && distinctShownAbilities.Contains(ability2))
				{
					distinctShownAbilities.Remove(ability2);
				}
			}
		}

		List<Ability> distinctAbilitiesCopy = new List<Ability>(distinctShownAbilities);
		if (ability != 0 && __instance.latchIcon != null)
		{
			__instance.latchIcon.AssignAbility(ability, info, playableCard);
		}

		__instance.ApplyAbilitiesToIcons(cardMergeAbilities, __instance.mergeIcons, __instance.emissiveIconMat, info,
			playableCard);
		bool isOpponentCard = playableCard != null && playableCard.OpponentCard;
		foreach (AbilityIconInteractable emissiveTotemIcon in __instance.emissiveTotemIcons)
		{
			emissiveTotemIcon.gameObject.SetActive(isOpponentCard);
		}

		foreach (AbilityIconInteractable totemIcon in __instance.totemIcons)
		{
			totemIcon.gameObject.SetActive(!isOpponentCard);
		}

		if (isOpponentCard)
		{
			__instance.ApplyAbilitiesToIcons(cardTotemAbilities, __instance.emissiveTotemIcons, __instance.emissiveIconMat,
				info, playableCard);
		}
		else
		{
			__instance.ApplyAbilitiesToIcons(cardTotemAbilities, __instance.totemIcons, __instance.totemIconMat, info,
				playableCard);
		}

		__instance.PositionModIcons(
			distinctAbilitiesCopy,
			cardMergeAbilities,
			__instance.mergeIcons, cardTotemAbilities,
			isOpponentCard ? __instance.emissiveTotemIcons : __instance.totemIcons);
		foreach (GameObject defaultIconGroup in __instance.defaultIconGroups)
		{
			defaultIconGroup.SetActive(value: false);
		}

		if (distinctAbilitiesCopy.Count > 0 && __instance.defaultIconGroups.Count >= distinctAbilitiesCopy.Count)
		{
			int indexToStartAt = 1;
			if (distinctAbilitiesCopy.Count == 4)
			{
				indexToStartAt = 0;
			}
			
			GameObject obj = __instance.defaultIconGroups[3];
			Log.LogDebug($"[CardAbilityIcons] IconGroup {__instance.defaultIconGroups[0]} Distinct abilities count [{distinctAbilitiesCopy.Count}]");
			Log.LogDebug($"[CardAbilityIcons] Setting defaultIconGroup zero active");
			obj.SetActive(value: true);
			Log.LogDebug($"[CardAbilityIcons] Getting AbilityIconInteractable in children");
			AbilityIconInteractable[] componentsInChildren = obj.GetComponentsInChildren<AbilityIconInteractable>();
			Log.LogDebug($"[CardAbilityIcons] Components in children [{componentsInChildren.Length}]");
			for (int i = indexToStartAt; i <= distinctAbilitiesCopy.Count; i++)
			{
				if (i > distinctAbilitiesCopy.Count) break;
				Log.LogDebug($"[CardAbilityIcons] [{i}] iter");
				Log.LogDebug($"[CardAbilityIcons] [{i}] Component{componentsInChildren[i]} Setting material");
				componentsInChildren[i].SetMaterial(__instance.defaultIconMat);
				Log.LogDebug($"[CardAbilityIcons] [{i}] Assigning ability");
				componentsInChildren[i].AssignAbility(distinctAbilitiesCopy[indexToStartAt == 1 ? i - 1 : i], info, playableCard);
				Log.LogDebug($"[CardAbilityIcons] [{i}] Adding to ability icons list");
				__instance.abilityIcons.Add(componentsInChildren[i]);
			}
		}

		Log.LogDebug($"[CardAbilityIcons] Checking if card has single ability");
		bool hasSingleAbility = distinctAbilitiesCopy.Count == 1 && cardMergeAbilities.Count == 0 &&
		                        cardTotemAbilities.Count == 0 && info.SpecialStatIcon == SpecialStatIcon.None;
		Log.LogDebug($"[CardAbilityIcons] __instance.SetSingleDefaultAbilityColliderSize");
		__instance.SetSingleDefaultAbilityColliderSize(hasSingleAbility &&
		                                               !ProgressionData.LearnedMechanic(MechanicsConcept.Rulebook));
		Log.LogDebug($"[CardAbilityIcons] __instance.DisplayBoonIcon");
		__instance.DisplayBoonIcon(info);

		return false;
	}
}
