using System.Collections;
using DiskCardGame;
using HarmonyLib;
using UnityEngine;
using static GrimoraMod.GrimoraPlugin;

namespace GrimoraMod;

public partial class GrimoraPlugin
{
	public const string NameGiant = $"{GUID}_Giant";

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
		IEnumerator enumerator,
		PlayableCard card,
		CardSlot slot,
		float transitionLength,
		bool resolveTriggers = true
	)
	{
		if (GrimoraSaveUtil.isGrimora
		    && card.Info.HasTrait(Trait.Giant)
		    && card.Info.SpecialAbilities.Contains(GrimoraGiant.FullAbility.Id))
		{
			bool isBonelord = card.InfoName().Equals(NameBonelord);
			// Card -> RotatingParent (child zero) -> TombstoneParent -> Cardbase_StatsLayer
			Transform rotatingParent = card.transform.GetChild(0);

			float xValPosition = -0.7f;
			float xValScale = 2.1f;
			if (ConfigHelper.HasIncreaseSlotsMod && isBonelord)
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
			!target.Info.SpecialAbilities.Contains(GrimoraGiant.FullAbility.Id);
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
			List<CardSlot> slotsToTarget = BoardManager.Instance.GetSlots(__instance.OpponentCard);

			foreach (var slot in slotsToTarget.Where(slot => slot.Card.IsNotNull()))
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

			// should return farthest left slot
			CardSlot firstSlotOfGiant = BoardManager.Instance.GetSlots(!__instance.OpponentCard)
				.First(slot => slot.Card.IsNotNull() && slot.Card == __instance);

			if (BoardManager.Instance.GetAdjacentSlots(firstSlotOfGiant)
			    .Exists(slot => slot.IsNotNull() && slot.Card.IsNotNull() && slot.Card.HasAbility(Ability.BuffNeighbours)))
			{
				__result++;
			}
		}
	}
}
