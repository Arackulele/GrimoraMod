using DiskCardGame;
using HarmonyLib;

namespace GrimoraMod;

[HarmonyPatch(typeof(BoardManager))]
public class BoardManagerPatches
{
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
	}
}
