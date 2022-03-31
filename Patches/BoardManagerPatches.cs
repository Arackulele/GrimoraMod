using System.Collections;
using DiskCardGame;
using HarmonyLib;

namespace GrimoraMod;

[HarmonyPatch(typeof(BoardManager))]
public class BoardManagerPatches
{
	[HarmonyPostfix, HarmonyPatch(nameof(BoardManager.CleanUp))]
	public static IEnumerator CheckForNonCardTriggersOnTheBoardStill(IEnumerator enumerator, BoardManager __instance)
	{
		yield return enumerator;

		if (GrimoraSaveUtil.isGrimora)
		{
			foreach (var slot in __instance.AllSlotsCopy)
			{
				var nonCardReceivers = slot.GetComponentsInChildren<NonCardTriggerReceiver>();
				foreach (var nonCardTriggerReceiver in nonCardReceivers)
				{
					GrimoraPlugin.Log.LogWarning($"[CleanUp] Destroying NonCardTriggerReceiver [{nonCardTriggerReceiver}] from slot [{slot}]");
					UnityObject.Destroy(nonCardTriggerReceiver);
				}
			}
		}
	}

	[HarmonyPostfix, HarmonyPatch(nameof(BoardManager.QueueCardForSlot))]
	public static void PostfixAssignAsChildForQueuedCard(
		PlayableCard card,
		CardSlot slot,
		float tweenLength = 0.1f,
		bool doTween = true,
		bool setStartPosition = true
	)
	{
		card.transform.SetParent(BoardManager.Instance.OpponentQueueSlots[card.QueuedSlot.Index].transform);
		if (TurnManager.Instance.Opponent is RoyalBossOpponentExt && card.OpponentCard && !card.HasAbility(Anchored.ability))
		{
			CardInfo copyInfo = card.Info.Clone() as CardInfo;
			copyInfo.Mods.Add(new CardModificationInfo(Anchored.ability));

			card.SetInfo(copyInfo);
		}
	}

	[HarmonyPrefix, HarmonyPatch(nameof(BoardManager.ResolveCardOnBoard))]
	public static void AddSeaLegsToCardsThatDontHaveItForRoyal(
		PlayableCard card,
		CardSlot slot,
		float tweenLength = 0.1f,
		Action landOnBoardCallback = null,
		bool resolveTriggers = true
	)
	{
		if (TurnManager.Instance.Opponent is RoyalBossOpponentExt && card.OpponentCard && !card.HasAbility(Anchored.ability))
		{
			CardInfo copyInfo = card.Info.Clone() as CardInfo;
			copyInfo.Mods.Add(new CardModificationInfo(Anchored.ability));

			card.SetInfo(copyInfo);
		}
	}

	// [HarmonyPrefix, HarmonyPatch(nameof(BoardManager.GetAdjacentSlots))]
	public static bool GetCorrectAdjacentSlotsForGiantCards(BoardManager __instance, CardSlot slot, ref List<CardSlot> __result)
	{
		if (slot.Card && slot.Card.Info.SpecialAbilities.Contains(GrimoraGiant.FullSpecial.Id) && slot.Card.OnBoard)
		{
			__result = new List<CardSlot>();
			List<CardSlot> slotsWithGiant = __instance.opponentSlots
				.Where(x => x.Card && x.Card.InfoName() == slot.Card.InfoName())
				.ToList();

			if (slotsWithGiant.Count < 2)
			{
				GrimoraPlugin.Log.LogDebug($"Giant card slot count is not 2, returning empty");
				return false;
			}

			// Log.LogDebug($"Slots that contain [{slot.Card.GetNameAndSlot()}] = {secondSlot.Join(x => x.Index.ToString())}");
				
			int leftAdjSlotIndex = slotsWithGiant[0].Index - 1;
			int rightAdjSlotIndex = slotsWithGiant[1].Index + 1;
			// Log.LogDebug($"Adjacent slot indexes to check left-[{leftAdjSlotIndex}] right-[{rightAdjSlotIndex}]");
			if (__instance.opponentSlots.Exists(x => x.Index == leftAdjSlotIndex && x.Card != slot.Card))
			{
				__result.Add(__instance.opponentSlots[leftAdjSlotIndex]);
			}
			if (__instance.opponentSlots.Exists(x => x.Index == rightAdjSlotIndex && x.Card != slot.Card))
			{
				__result.Add(__instance.opponentSlots[rightAdjSlotIndex]);
			}

			// Log.LogDebug($"Adjacent Slots for [{slot.Card.GetNameAndSlot()}] = {__result.Join(x => x.Index.ToString())}");

			return false;
		}

		return true;
	}
}
