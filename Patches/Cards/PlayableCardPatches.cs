using System.Collections;
using DiskCardGame;
using HarmonyLib;
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
		if (__instance.Slot.opposingSlot.CardIsNotNullAndHasAbility(Possessive.ability))
		{
			var adjSlots = BoardManager.Instance
				.GetAdjacentSlots(__instance.Slot)
				.Where(_ => _.Card)
				.ToList();

			__result = new List<CardSlot>();
			if (adjSlots.IsNotEmpty())
			{
				CardSlot slotToTarget = adjSlots[UnityEngine.Random.RandomRangeInt(0, adjSlots.Count)];
				// Log.LogDebug($"[OpposingPatches.Possessive] Slot targeted for attack [{slotToTarget.Index}]");
				__result.Add(slotToTarget);
			}
		}
	}


	// [HarmonyPostfix, HarmonyPatch(typeof(PlayableCard), nameof(PlayableCard.GetPassiveAttackBuffs))]
	public static void CorrectDebuffEnemiesLogicForGiants(PlayableCard __instance, ref int __result)
	{
		if (__instance.OnBoard && __instance.Info.HasTrait(Trait.Giant) && !__instance.HasAbility(Ability.MadeOfStone))
		{
			List<CardSlot> slotsToTarget = BoardManager.Instance.GetSlots(__instance.OpponentCard);

			foreach (var slot in slotsToTarget.Where(slot => slot.Card))
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
