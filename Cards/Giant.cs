using System.Collections;
using DiskCardGame;
using HarmonyLib;
using UnityEngine;
using static GrimoraMod.GrimoraPlugin;

namespace GrimoraMod;

public partial class GrimoraPlugin
{
	public const string NameGiant = "GrimoraMod_Giant";

	private void Add_Giant()
	{
		CardBuilder.Builder
				.SetAsNormalCard()
				.SetAbilities(Ability.QuadrupleBones, Ability.SplitStrike)
				.SetBaseAttackAndHealth(2, 7)
				.SetBoneCost(15)
				.SetNames(NameGiant, "Giant")
				.SetTraits(Trait.Giant, Trait.Uncuttable)
				.SetDescription("TRULY A SIGHT TO BEHOLD.")
				.Build()
			// , specialAbilitiesIdsParam: new List<SpecialAbilityIdentifier> { sbIds.id }
			;
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
		if (GrimoraSaveUtil.isGrimora
		    && card.Info.HasTrait(Trait.Giant)
		    && card.Info.SpecialAbilities.Contains(GrimoraGiant.NewSpecialAbility.specialTriggeredAbility))
		{
			bool isBonelord = card.InfoName().Equals(NameBonelord);
			// Card -> RotatingParent (child zero) -> TombstoneParent -> Cardbase_StatsLayer
			Transform rotatingParent = card.transform.GetChild(0);

			float xValPosition = -0.7f;
			float xValScale = 2.1f;
			if (ConfigHelper.Instance.HasIncreaseSlotsMod && isBonelord)
			{
				xValPosition = -1.4f;
				xValScale = 3.3f;
			}

			rotatingParent.localPosition = new Vector3(xValPosition, 1.05f, 0);
			rotatingParent.localScale = new Vector3(xValScale, 2.1f, 1);
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
		bool targetIsNotGrimoraGiant =
			!target.Info.SpecialAbilities.Contains(GrimoraGiant.NewSpecialAbility.specialTriggeredAbility);
		__result = __result && targetIsNotGrimoraGiant;
	}
}

[HarmonyPatch]
public class PlayableCardPatchesForGiant
{
	[HarmonyPostfix, HarmonyPatch(typeof(PlayableCard), nameof(PlayableCard.GetPassiveAttackBuffs))]
	public static void CorrectDebuffEnemiesLogicForGiants(PlayableCard __instance, ref int __result)
	{
		if (__instance.OnBoard && __instance.Info.HasTrait(Trait.Giant))
		{
			List<CardSlot> slotsToTarget =
				__instance.OpponentCard
					? BoardManager.Instance.PlayerSlotsCopy
					: BoardManager.Instance.OpponentSlotsCopy;

			foreach (var slot in slotsToTarget.Where(slot => slot.Card is not null))
			{
				// if(!hasPrinted)
				// 	Log.LogDebug($"[Giant PlayableCard Patch] Slot [{__instance.Slot.Index}] for stinky");

				if (slot.Card.HasAbility(Ability.DebuffEnemy) && slot.opposingSlot.Card != __instance)
				{
					// __result is -1 right now
					// G1 IS FIRST GIANT, G2 IS SECOND GIANT
					// D IS THE CARD WITH STINKY
					// G1 G1 G2 G2
					//  x  x  D  X

					// G1 SHOULD HAVE THE -1 REVERSED, BUT G2 SHOULD STILL HAVE -1 APPLIED TO ATTACK
					__result += 1;
				}
			}
		}
	}
}

[HarmonyPatch]
public class AddLogicForGiantStrikeAbility
{
	[HarmonyPostfix, HarmonyPatch(typeof(PlayableCard), nameof(PlayableCard.GetOpposingSlots))]
	public static void FixSlotsToAttackForGiantStrike(PlayableCard __instance, ref List<CardSlot> __result)
	{
		if (__instance.Info.HasTrait(Trait.Giant)
		    && __instance.HasAbility(GiantStrike.ability)
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
