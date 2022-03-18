using DiskCardGame;
using HarmonyLib;
using static GrimoraMod.GrimoraPlugin;

namespace GrimoraMod;

[HarmonyPatch(typeof(PlayableCard))]
public class PlayableCardPatches
{
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

			if (hasRaider)
			{
				// by default result will have at least 1 slot, which is the opposing slot
				__result.Clear();
			}

			// insert at beginning
			if (toLeftSlot.IsNotNull())
			{
				if (!hasRaider || toLeftSlot.Card.IsNull() || !toLeftSlot.Card.HasAbility(Raider.ability))
				{
					__result.Insert(0, toLeftSlot);
				}
			}

			// insert at end
			if (toRightSlot.IsNotNull())
			{
				if (!hasRaider || toRightSlot.Card.IsNull() || !toRightSlot.Card.HasAbility(Raider.ability))
				{
					__result.Insert(__result.Count, toRightSlot);
				}
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
			if (slotToAttack.IsNull())
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
		if (__instance.Slot.opposingSlot.CardIsNotNullAndHasAbility(Possessive.ability))
		{
			var adjSlots = BoardManager.Instance
				.GetAdjacentSlots(__instance.Slot)
				.Where(_ => _.Card.IsNotNull())
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


	private static List<CardSlot> GetTwinGiantOpposingSlots(PlayableCard giant)
	{
		return BoardManager.Instance.PlayerSlotsCopy
			.Where(slot => slot.opposingSlot.Card == giant)
			.ToList();
	}

	[HarmonyPostfix, HarmonyPatch(nameof(PlayableCard.GetOpposingSlots))]
	public static void GrimoraGiantAttackSlotsPatch(PlayableCard __instance, ref List<CardSlot> __result)
	{
		if (__instance.HasAbility(GiantStrike.ability))
		{
			__result = new List<CardSlot>();
			List<CardSlot> slotsToTarget = GetTwinGiantOpposingSlots(__instance);
			if (slotsToTarget.Exists(slot => slot.Card.IsNotNull()))
			{
				List<CardSlot> slotsWithCards = slotsToTarget
					.Where(slot => slot.Card.IsNotNull())
					.ToList();
				if (slotsWithCards.Count == 1)
				{
					__result.Add(slotsWithCards[0]);
					// single card has health greater than current attack, then attack twice 
					if (slotsWithCards[0].Card.Health > __instance.Attack)
					{
						__result.Add(slotsWithCards[0]);
					}
				}
				else
				{
					__result.AddRange(slotsWithCards);
				}
			}
			else
			{
				__result.AddRange(slotsToTarget);
			}

			Log.LogInfo($"[GiantStrike] Opposing slots is now [{__result.Join(slot => slot.Index.ToString())}]");
		}
		else if (__instance.HasAbility(GiantStrikeEnraged.ability))
		{
			__result = GetTwinGiantOpposingSlots(__instance);

			Log.LogInfo($"[GiantStrikeEnraged] Opposing slots is now [{string.Join(",", __result.Select(_ => _.Index))}]");
		}
	}

	[HarmonyPostfix, HarmonyPatch(typeof(PlayableCard), nameof(PlayableCard.GetPassiveAttackBuffs))]
	public static void CorrectDebuffEnemiesLogicForGiants(PlayableCard __instance, ref int __result)
	{
		if (!__instance.Dead && __instance.OnBoard && __instance.Info.HasTrait(Trait.Giant))
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
