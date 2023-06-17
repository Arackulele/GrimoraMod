using System.Collections;
using DiskCardGame;
using GrimoraMod.Saving;
using HarmonyLib;
using UnityEngine;

namespace GrimoraMod;

[HarmonyPatch(typeof(ChessboardChestPiece))]
public class ChestPatches
{
	[HarmonyPrefix, HarmonyPatch(nameof(ChessboardChestPiece.Start))]
	public static bool StartPrefix(ref ChessboardChestPiece __instance)
	{
		// this position code exists only in ChessboardEnemyPiece, which is why we need a patch for it
		__instance.gameObject.transform.position
			= ChessboardNavGrid
				.instance
				.zones[__instance.gridXPos, __instance.gridYPos]
				.transform
				.position;

		ChessboardNavGrid.instance
			.zones[__instance.gridXPos, __instance.gridYPos]
			.GetComponent<ChessboardMapNode>()
			.OccupyingPiece = __instance;

		// if we place chests with a different type of node data during creation, the Start method will overwrite it 
		__instance.NodeData ??= new CardChoicesNodeData();

		return false;
	}

	[HarmonyPostfix, HarmonyPatch(nameof(ChessboardChestPiece.OpenSequence))]
	public static IEnumerator OpenSequencePostfix(IEnumerator enumerator, ChessboardChestPiece __instance)
	{
		if (!SaveManager.SaveFile.IsGrimora)
		{
			yield return enumerator;
			yield break;
		}

		GrimoraPlugin.Log.LogDebug($"[ChessboardChestPiece.OpenSequence] Piece [{__instance.name}]");
		GrimoraRunState.CurrentRun.PiecesRemovedFromBoard.Add(__instance.name);

		MapNodeManager.Instance.SetAllNodesInteractable(false);

		ViewManager.Instance.Controller.LockState = ViewLockState.Locked;

		PlayerMarker.Instance.Anim.Play("knock against", 0, 0f);
		yield return new WaitForSeconds(0.05f);

		__instance.anim.Play("open", 0, 0f);
		yield return new WaitForSeconds(0.25f);

		ViewManager.Instance.Controller.LockState = ViewLockState.Unlocked;

		GameFlowManager.Instance.TransitionToGameState(GameState.SpecialCardSequence, __instance.NodeData);

	}
}
