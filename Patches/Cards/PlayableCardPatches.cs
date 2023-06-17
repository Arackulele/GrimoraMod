using System.Collections;
using DiskCardGame;
using GrimoraMod.Extensions;
using HarmonyLib;
using InscryptionAPI.Card;
using InscryptionAPI.Helpers.Extensions;
using static GrimoraMod.GrimoraPlugin;

namespace GrimoraMod;

[HarmonyPatch(typeof(PlayableCard))]
public class PlayableCardPatches
{
	[HarmonyPostfix, HarmonyPatch(nameof(PlayableCard.Die))]
	public static IEnumerator ExtendDieMethod(
		IEnumerator enumerator,
		PlayableCard __instance,
		bool wasSacrifice,
		PlayableCard killer = null,
		bool playSound = true
	)
	{
		yield return __instance.DieCustom(wasSacrifice, killer, playSound);
	}

	[HarmonyPostfix, HarmonyPatch(nameof(PlayableCard.GetOpposingSlots))]
	public static void PossessiveGetOpposingSlotsPatch(PlayableCard __instance, ref List<CardSlot> __result)
	{
		CardSlot opposingSlot = __instance.OpposingSlot();
		if (opposingSlot.Card && opposingSlot.Card.HasAbility(Possessive.ability))
		{
			var adjCards = __instance.Slot.GetAdjacentSlots(true).GetCards();

			__result = new List<CardSlot>();
			if (adjCards.Any())
			{
				// Log.LogDebug($"[OpposingPatches.Possessive] Slot targeted for attack [{slotToTarget.Index}]");
				__result.Add(adjCards.GetRandomItem().Slot);
			}
		}
	}

	[HarmonyPostfix, HarmonyPatch(nameof(PlayableCard.CanAttackDirectly))]
	public static void PossessiveCanAttackDirectlyPatch(PlayableCard __instance, CardSlot opposingSlot, ref bool __result)
	{
		Log.LogDebug($"[Possessive.CanAttackDirectly] Result before [{__result}]");
		CardSlot oppositeSlot = __instance.OpposingSlot();
		bool oppositeSlotHasPossessive = oppositeSlot.Card && oppositeSlot.Card.HasAbility(Possessive.ability) && __instance.LacksTrait(Trait.Giant);
		__result &= !oppositeSlotHasPossessive;
		Log.LogDebug($"[Possessive.CanAttackDirectly] Result after  [{__result}]");
	}


	[HarmonyAfter(InscryptionAPI.InscryptionAPIPlugin.ModGUID)]
	[HarmonyPostfix, HarmonyPatch(typeof(PlayableCard), nameof(PlayableCard.GetPassiveAttackBuffs))]
	public static void CorrectBuffsAndDebuffsForGrimoraGiants(PlayableCard __instance, ref int __result)
	{
		bool isGrimoraGiant = GrimoraSaveUtil.IsGrimoraModRun && __instance.IsGrimoraGiant();
		if (__instance.OnBoard && isGrimoraGiant)
		{
			int finalAttackNum = 0;
			List<PlayableCard> opposingCards = BoardManager.Instance.GetSlots(__instance.OpponentCard).GetCards();
			foreach (var card in opposingCards)
			{
				if (card.HasAbility(Ability.BuffEnemy))
				{
					finalAttackNum++;
				}

				if (__instance.LacksAbility(Ability.MadeOfStone) && card.HasAbility(Ability.DebuffEnemy))
				{
					finalAttackNum--;
				}
			}

			finalAttackNum += GetAdjacentLeaderBuffs(__instance);

			__result = finalAttackNum;
		}
	}

	private static int GetAdjacentLeaderBuffs(PlayableCard giantCard)
	{
		return BoardManager.Instance.opponentSlots
		 .Where(slot => slot.Card == giantCard)
		 .Count(slot => slot.GetAdjacentSlots(true)
						 .GetCards(pCard => pCard != giantCard)
						 .Exists(pCard => pCard.HasAbility(Ability.BuffNeighbours))
			);
	}
}
