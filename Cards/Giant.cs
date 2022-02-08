using System.Collections;
using APIPlugin;
using DiskCardGame;
using HarmonyLib;
using UnityEngine;
using static GrimoraMod.GrimoraPlugin;

namespace GrimoraMod;

public partial class GrimoraPlugin
{
	public const string NameGiant = "ara_Giant";

	private void AddAra_Giant()
	{
		NewCard.Add(CardBuilder.Builder
				.SetAsNormalCard()
				.SetAbilities(Ability.QuadrupleBones, Ability.SplitStrike)
				.SetBaseAttackAndHealth(2, 7)
				.SetBoneCost(15)
				.SetNames(NameGiant, "Giant")
				.SetTraits(Trait.Giant, Trait.Uncuttable)
				// .SetDescription("A vicious pile of bones. You can have it...")
				.Build()
			// , specialAbilitiesIdsParam: new List<SpecialAbilityIdentifier> { sbIds.id }
		);
	}
}

public class GrimoraGiant : SpecialCardBehaviour
{
	public static readonly NewSpecialAbility NewSpecialAbility = Create();

	public static NewSpecialAbility Create()
	{
		var sId = SpecialAbilityIdentifier.GetID(GUID, "!GRIMORA_GIANT");

		return new NewSpecialAbility(typeof(GrimoraGiant), sId);
	}

	public override bool RespondsToResolveOnBoard()
	{
		return true;
	}

	public override IEnumerator OnResolveOnBoard()
	{
		BoardManager.Instance.OpponentSlotsCopy[base.PlayableCard.Slot.Index - 1].Card = base.PlayableCard;
		BoardManager.Instance.OpponentSlotsCopy[base.PlayableCard.Slot.Index].Card = base.PlayableCard;
		yield break;
	}

	public override bool RespondsToDie(bool wasSacrifice, PlayableCard killer)
	{
		return true;
	}

	public override IEnumerator OnDie(bool wasSacrifice, PlayableCard killer)
	{
		BoardManager.Instance.OpponentSlotsCopy[base.PlayableCard.Slot.Index - 1].Card = null;
		BoardManager.Instance.OpponentSlotsCopy[base.PlayableCard.Slot.Index].Card = null;
		yield break;
	}
}

[HarmonyPatch]
public class ModifyLocalPositionsOfTableObjects
{
	[HarmonyPostfix,
	 HarmonyPatch(typeof(BoardManager3D), nameof(BoardManager3D.TransitionAndResolveCreatedCard))
	]
	public static IEnumerator ChangeScaleOfMoonCardToFitAcrossAllSlots(
		IEnumerator enumerator, PlayableCard card, CardSlot slot,
		float transitionLength, bool resolveTriggers = true)
	{
		if (SaveManager.SaveFile.IsGrimora 
		    && card.Info.HasTrait(Trait.Giant) 
		    && card.Info.SpecialAbilities.Contains(GrimoraGiant.NewSpecialAbility.specialTriggeredAbility))
		{
			Log.LogDebug($"Setting new scaling and position of [{card.Info.name}]");
			// Card -> RotatingParent -> TombstoneParent -> Cardbase_StatsLayer
			UnityEngine.Transform rotatingParent = card.transform.GetChild(0);
			Log.LogDebug($"Transforming [{rotatingParent.name}]");

			rotatingParent.localPosition = new Vector3(-0.7f, 1.05f, 0f);
			// GrimoraPlugin.Log.LogDebug($"Successfully set new localPosition for the giant");

			rotatingParent.localScale = new UnityEngine.Vector3(2.1f, 2.1f, 1f);
			// GrimoraPlugin.Log.LogDebug($"Successfully set new scaling for the giant");
		}

		yield return enumerator;
	}
}

[HarmonyPatch]
public class KayceeModLogicForDeathTouchPrevention
{
	[HarmonyPostfix, HarmonyPatch(typeof(Deathtouch), nameof(Deathtouch.RespondsToDealDamage))]
	public static void AddLogicForDeathTouchToNotKillGiants(int amount, PlayableCard target, ref bool __result)
	{
		__result = __result && !target.Info.SpecialAbilities.Contains(GrimoraGiant.NewSpecialAbility.specialTriggeredAbility);
	}
}


[HarmonyPatch]
public class CorrectLogicForAllStrikeAbility
{
	[HarmonyPostfix, HarmonyPatch(typeof(PlayableCard), nameof(PlayableCard.GetOpposingSlots))]
	public static void FixSlotsToAttackForGiantAllStrike(PlayableCard __instance, ref List<CardSlot> __result)
	{
		if (__instance.Info.HasTrait(Trait.Giant)
		    && __instance.HasAbility(Ability.AllStrike)
		    && __instance.Info.SpecialAbilities.Contains(GrimoraGiant.NewSpecialAbility.specialTriggeredAbility))
		{
			List<CardSlot> slotsToTarget = __instance.OpponentCard
				? BoardManager.Instance.PlayerSlotsCopy
				: BoardManager.Instance.OpponentSlotsCopy;

			__result = new List<CardSlot>(slotsToTarget.Where(slot => slot.opposingSlot.Card == __instance));
			Log.LogDebug($"[AllStrikePatch] Opposing slots is now [{string.Join(",", __result.Select(_ => _.Index))}]");
		}
	}
}
