using System.Collections;
using DiskCardGame;
using HarmonyLib;
using UnityEngine;

namespace GrimoraMod.Chessboard.Pieces
{
	[HarmonyPatch(typeof(ChessboardChestPiece))]
	public class ChestPatches
	{

		[HarmonyPrefix, HarmonyPatch(nameof(ChessboardChestPiece.Start))]
			public static void StartPrefix(ref ChessboardChestPiece __instance)
			{
				
				GrimoraPlugin.Log.LogDebug($"[ChessboardChestPiece.Start] Setting piece [{__instance.name}] to nav grid position x[{__instance.gridXPos}] y[{__instance.gridYPos}]");
				
				// this position code exists only in ChessboardEnemyPiece, which is why we need a patch for it
					__instance.gameObject.transform.position = ChessboardNavGrid.instance
						.zones[__instance.gridXPos, __instance.gridYPos].transform.position;
			}
			
			[HarmonyPostfix, HarmonyPatch(nameof(ChessboardChestPiece.Start))]
			public static void StartPostfix(ref ChessboardChestPiece __instance)
			{
				GrimoraPlugin.Log.LogDebug($"[ChessboardChestPiece.Start][Postfix] Piece [{__instance.name}] " +
				                           $"node data [{__instance.NodeData.GetType()}] " +
				                           $"Occupying piece [{ChessboardNavGrid.instance.zones[__instance.gridXPos, __instance.gridYPos].GetComponent<ChessboardMapNode>().OccupyingPiece.name}]"
				                          );
			}

			[HarmonyPostfix, HarmonyPatch(nameof(ChessboardChestPiece.OpenSequence))]
			public static IEnumerator OpenSequencePrefix(IEnumerator enumerator, ChessboardChestPiece __instance)
			{
				if (!SaveManager.SaveFile.IsGrimora)
				{
					yield return enumerator;
					yield break;
				}

				// Log.LogInfo(__instance.MapNode.OccupyingPiece);
				GrimoraSaveData.Data.removedPieces.Add(__instance.saveId);
				
				Singleton<MapNodeManager>.Instance.SetAllNodesInteractable(false);
				Singleton<ViewManager>.Instance.Controller.LockState = ViewLockState.Locked;
				
				PlayerMarker.Instance.Anim.Play("knock against", 0, 0f);
				yield return new WaitForSeconds(0.05f);
				
				__instance.anim.Play("open", 0, 0f);
				yield return new WaitForSeconds(0.25f);
				
				Singleton<ViewManager>.Instance.Controller.LockState = ViewLockState.Unlocked;
				
				Singleton<GameFlowManager>.Instance.TransitionToGameState(GameState.SpecialCardSequence, __instance.NodeData);
				
				ChessboardNavGrid.instance
					.zones[__instance.gridXPos, __instance.gridYPos]
					.GetComponent<ChessboardMapNode>()
					.OccupyingPiece = null;
				
				__instance.MapNode.OccupyingPiece = null;
				
				// Log.LogInfo(__instance.MapNode.OccupyingPiece);
				__instance.MapNode.nodeId = __instance.saveId;
			}
		
	}
}