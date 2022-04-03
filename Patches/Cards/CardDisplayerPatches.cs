using DiskCardGame;
using HarmonyLib;
using UnityEngine;

namespace GrimoraMod;

[HarmonyPatch(typeof(CardDisplayer))]
public class CardDisplayerPatches
{
	[HarmonyPrefix, HarmonyPatch(nameof(CardDisplayer.SetTextColors))]
	public static bool PrefixChangeRenderColors(ref CardDisplayer __instance,
	                                            CardRenderInfo renderInfo,
	                                            PlayableCard playableCard)
	{
		if (GrimoraSaveUtil.isNotGrimora)
		{
			return true;
		}

		if (playableCard)
		{
			int maxHealth = playableCard.MaxHealth;
		}
		else
		{
			int health = renderInfo.baseInfo.Health;
		}

		__instance.SetHealthTextColor(
			renderInfo.health >= renderInfo.baseInfo.Health
				? GameColors.Instance.glowSeafoam
				: GameColors.Instance.darkLimeGreen
		);

		__instance.SetAttackTextColor(GameColors.Instance.glowSeafoam);
		__instance.SetNameTextColor(GameColors.Instance.glowSeafoam);

		return false;
	}
}

[HarmonyPatch(typeof(CardDisplayer3D))]
public class CardDisplayer3DPatches
{
	[HarmonyPrefix, HarmonyPatch(nameof(CardDisplayer3D.EmissionEnabledForCard))]
	public static bool PrefixChangeRenderColors(CardRenderInfo renderInfo, PlayableCard playableCard, ref bool __result)
	{
		bool renderInfoIsNull = renderInfo == null || renderInfo.baseInfo == null;
		bool cardDoesNotImbuedWithAttack = playableCard && playableCard.HasAbility(Imbued.ability) && !playableCard.TemporaryMods.Exists(mod => mod.singletonId == Imbued.ModIdImbued);
		bool renderInfoDoesNotHaveImbuedMod = !renderInfoIsNull && renderInfo.baseInfo.HasAbility(Imbued.ability) && !renderInfo.baseInfo.Mods.Exists(mod => mod.singletonId == Imbued.ModIdImbued);
		// GrimoraPlugin.Log.LogDebug($"RenderInfo is null [{renderInfoIsNull}] PlayableCard is imbued? [{cardDoesNotImbuedWithAttack}] RenderInfo? [{renderInfoDoesNotHaveImbuedMod}]");
		if (renderInfoIsNull || renderInfoDoesNotHaveImbuedMod || cardDoesNotImbuedWithAttack)
		{
			__result = false;
			return false;
		}

		bool cardIsOpponentAndHasModFromTotem = playableCard && playableCard.OpponentCard && playableCard.HasModFromTotem();
		bool hasModFromCardMerge = renderInfo.baseInfo.HasModFromCardMerge();
		__result = renderInfo.forceEmissivePortrait || cardIsOpponentAndHasModFromTotem || hasModFromCardMerge;
		return false;
	}
}

[HarmonyPatch(typeof(GravestoneCardDisplayer))]
public class GravestoneCardDisplayerPatches
{
	[HarmonyPostfix, HarmonyPatch(nameof(GravestoneCardDisplayer.DisplayInfo))]
	public static void PrefixChangeRenderColors(GravestoneCardDisplayer __instance, CardRenderInfo renderInfo, PlayableCard playableCard)
	{
		string bonesCost = Mathf.Max(0, renderInfo.baseInfo.BonesCost).ToString();
		__instance.costShadow.text = bonesCost;
		__instance.costText.text = bonesCost;
	}
}
