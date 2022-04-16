using System.Collections;
using DiskCardGame;
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
		CardSlot opposingSlot = __instance.Slot.opposingSlot;
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
		CardSlot oppositeSlot = __instance.Slot.opposingSlot;
		bool oppositeSlotHasPossessive = oppositeSlot.Card && oppositeSlot.Card.HasAbility(Possessive.ability);
		__result &= !oppositeSlotHasPossessive;
		Log.LogDebug($"[Possessive.CanAttackDirectly] Result after [{__result}]");
	}

	[HarmonyPrefix, HarmonyPatch(typeof(PlayableCard), nameof(PlayableCard.GetPassiveAttackBuffs))]
	public static bool CorrectBuffsAndDebuffsForGrimoraGiants(PlayableCard __instance, ref int __result)
	{
		bool isGrimoraGiant = __instance.HasTrait(Trait.Giant) && __instance.HasSpecialAbility(GrimoraGiant.FullSpecial.Id);
		if (__instance.OnBoard && isGrimoraGiant)
		{
			int finalAttackNum = 0;
			List<PlayableCard> opposingSlots = BoardManager.Instance.GetSlots(__instance.OpponentCard).GetCards();
			foreach (var opposingCard in opposingSlots)
			{
				if (opposingCard.HasAbility(Ability.BuffEnemy))
				{
					finalAttackNum++;
				}

				if (!__instance.HasAbility(Ability.MadeOfStone) && opposingCard.HasAbility(Ability.DebuffEnemy))
				{
					finalAttackNum--;
				}
			}

			List<PlayableCard> giantCards = BoardManager.Instance.GetSlots(__instance.IsPlayerCard()).GetCards(pCard => pCard == __instance);
			foreach (var giant in giantCards)
			{
				List<PlayableCard> adjCards = giant.Slot.GetAdjacentSlots(true).GetCards(pCard => pCard != __instance);
				if (adjCards.Exists(pCard => pCard.HasAbility(Ability.BuffNeighbours)))
				{
					finalAttackNum++;
				}
			}

			__result = finalAttackNum;
			return false;
		}

		return true;
	}
}
