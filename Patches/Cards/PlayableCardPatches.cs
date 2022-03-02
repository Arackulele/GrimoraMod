using DiskCardGame;
using HarmonyLib;
using static GrimoraMod.GrimoraPlugin;

namespace GrimoraMod;

[HarmonyPatch(typeof(PlayableCard))]
public class PlayableCardPatches
{
	// 

	[HarmonyPostfix, HarmonyPatch(nameof(PlayableCard.GetOpposingSlots))]
	public static void AreaOfEffectStrikeGetOpposingSlotsPatch(PlayableCard __instance, ref List<CardSlot> __result)
	{
		bool hasRaider = __instance.HasAbility(Raider.ability);
		bool hasAOEStrike = __instance.HasAbility(AreaOfEffectStrike.ability);
		bool hasInvertedStrike = __instance.HasAbility(InvertedStrike.ability);
		bool hasAlternatingStrike = __instance.HasAbility(AlternatingStrike.ability);

		if (hasAOEStrike || hasRaider)
		{
			var toLeftSlot = BoardManager.Instance.GetAdjacent(__instance.Slot, true);
			var toRightSlot = BoardManager.Instance.GetAdjacent(__instance.Slot, false);

			// insert at beginning
			if (toLeftSlot is not null)
			{
				__result.Insert(0, toLeftSlot);
			}

			// insert at end
			if (toRightSlot is not null)
			{
				__result.Insert(__result.Count, toRightSlot);
			}

			if (hasInvertedStrike)
			{
				// this will make it so that instead of attacking the slots in a clockwise manner, left to right,
				// it will now be right to left, counter-clockwise
				__result.Reverse();
			}

			if (hasAlternatingStrike && hasAOEStrike)
			{
				List<CardSlot> alternatedResult = new List<CardSlot>()
				{
					__result[4], // right adj
					__result[0], // left adj
					__result[3], // right opposing
					__result[1], // left opposing
					__result[2], // center
				};

				__result = alternatedResult;
			}
		}
		else if (hasInvertedStrike)
		{
			List<CardSlot> slotsToCheck = __instance.OpponentCard
				? BoardManager.Instance.PlayerSlotsCopy
				: BoardManager.Instance.OpponentSlotsCopy;

			int slotIndex = __instance.Slot.Index;
			// 3 - 0 (card slot) == 3 (opposing slot)
			// 3 - 1 (card slot) == 2 (opposing slot)
			// 3 - 2 (card slot) == 1 (opposing slot)
			// 3 - 3 (card slot) == 0 (opposing slot)
			// if for whatever reason we increase the number of card slots in the mod, don't hardcode to 3
			int slotToAttack = (BoardManager.Instance.playerSlots.Count - 1) - slotIndex;

			__result = new List<CardSlot>() { slotsToCheck[slotToAttack] };
		}
		else if (hasAlternatingStrike)
		{
			__result.Clear();
			bool isAttackingLeft = __instance.GetComponent<AlternatingStrike>().isAttackingLeft;
			CardSlot slotToAttack = BoardManager.Instance.GetAdjacent(__instance.Slot.opposingSlot, isAttackingLeft);
			if (slotToAttack is null)
			{
				// if in far left slot, adj slot left will be null
				// if in far left slot and attacked right last, need to keep attack to the right slot
				slotToAttack = BoardManager.Instance.GetAdjacent(__instance.Slot.opposingSlot, !isAttackingLeft);
			}

			__result.Add(slotToAttack);
			// Log.LogDebug($"[AlternatingStrike.Patch] Attacking slot [{slotToAttack.Index}]");
		}
	}

	[HarmonyPostfix, HarmonyPatch(nameof(PlayableCard.HasTriStrike))]
	public static void AreaOfEffectHasTriStrikePatches(PlayableCard __instance, ref bool __result)
	{
		if (__instance.HasAbility(AreaOfEffectStrike.ability))
		{
			__result = true;
		}
	}

	[HarmonyPostfix, HarmonyPatch(nameof(PlayableCard.GetOpposingSlots))]
	public static void PossessiveGetOpposingSlotsPatch(PlayableCard __instance, ref List<CardSlot> __result)
	{
		if (__instance.Slot.opposingSlot.Card is not null
		    && __instance.Slot.opposingSlot.Card.HasAbility(Possessive.ability))
		{
			var adjSlots = BoardManager.Instance
				.GetAdjacentSlots(__instance.Slot)
				.Where(_ => _.Card is not null)
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

	private static bool SlotHasCardAndIsOpposingGiant(PlayableCard giant, CardSlot cardSlot)
	{
		return cardSlot.Card is not null && (cardSlot == giant.Slot.opposingSlot || cardSlot.Index == giant.Slot.Index - 1);
	}

	[HarmonyPostfix, HarmonyPatch(nameof(PlayableCard.GetOpposingSlots))]
	public static void GrimoraGiantPatch(PlayableCard __instance, ref List<CardSlot> __result)
	{
		if (__instance.HasAbility(GiantStrike.ability))
		{
			__result = new List<CardSlot>();
			List<CardSlot> slotsToTarget = __instance.OpponentCard
				? BoardManager.Instance.PlayerSlotsCopy
				: BoardManager.Instance.OpponentSlotsCopy;
			if (slotsToTarget.Exists(slot => SlotHasCardAndIsOpposingGiant(__instance, slot)))
			{
				List<CardSlot> filteredList =
					slotsToTarget.Where(slot => SlotHasCardAndIsOpposingGiant(__instance, slot)).ToList();
				if (filteredList.Count == 1)
				{
					__result.Add(filteredList[0]);
					// single card has health greater than current attack, then attack twice 
					if (filteredList[0].Card.Health > __instance.Attack)
					{
						__result.Add(filteredList[0]);
					}
				}
				else
				{
					// add both items in the list
					__result.AddRange(filteredList);
				}
			}
			else
			{
				__result.Add(slotsToTarget[0]);
			}

			Log.LogDebug($"[GiantStrike] Opposing slots is now [{string.Join(",", __result.Select(_ => _.Index))}]");
		}
		else if (__instance.HasAbility(GiantStrikeEnraged.ability))
		{
			__result = (__instance.OpponentCard
					? BoardManager.Instance.PlayerSlotsCopy
					: BoardManager.Instance.OpponentSlotsCopy)
				.Where(slot => slot.opposingSlot.Card == __instance)
				.ToList();

			Log.LogDebug($"[GiantStrikeEnraged] Opposing slots is now [{string.Join(",", __result.Select(_ => _.Index))}]");
		}
	}
}
