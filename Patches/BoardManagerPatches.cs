using System.Collections;
using DiskCardGame;
using HarmonyLib;
using UnityEngine;

namespace GrimoraMod;

[HarmonyPatch(typeof(BoardManager))]
public class BoardManagerPatches
{
	// [HarmonyPostfix, HarmonyPatch(nameof(BoardManager.ClearBoard))]
	// public static IEnumerator CheckForNonCardTriggersOnTheBoardStill(IEnumerator enumerator, BoardManager __instance)
	// {
	// 	yield return enumerator;
	// }
}

[HarmonyPatch(typeof(BoardManager3D))]
public class BoardManager3DPatches
{
	[HarmonyPostfix, HarmonyPatch(nameof(BoardManager3D.MoveQueuedCardToSlot))]
	public static void CheckForNonCardTriggersOnTheBoardStill(
		BoardManager3D __instance,
		PlayableCard card,
		CardSlot slot,
		float transitionDuration = 0.1f,
		bool doTween = true,
		bool setStartPosition = true
	)
	{
		card.transform.SetParent(BoardManager.Instance.OpponentQueueSlots[card.Slot.Index].transform);
	}
}
