using DiskCardGame;
using HarmonyLib;

namespace GrimoraMod.Chessboard.Pieces
{
	[HarmonyPatch(typeof(ChessboardChestPiece))]
	public class Chest
	{

		[HarmonyPrefix, HarmonyPatch(nameof(ChessboardChestPiece.Start))]
			public static bool StartPrefix(ref ChessboardChestPiece __instance)
			{
				//if (__instance.gameObject.name.Contains("SpecialPiece"))
				{
					__instance.gameObject.transform.position = ChessboardNavGrid.instance
						.zones[__instance.gridXPos, __instance.gridYPos].transform.position;
					ChessboardNavGrid.instance.zones[__instance.gridXPos, __instance.gridYPos].GetComponent<ChessboardMapNode>()
						.OccupyingPiece = __instance;

					return false;
				}

				return true;
			}
		

			[HarmonyPrefix, HarmonyPatch(nameof(ChessboardChestPiece.OpenSequence))]
			public static void OpenSequencePrefix(ref ChessboardChestPiece __instance)
			{
				//if (__instance.gameObject.name.Contains("SpecialPiece"))
				{
					// Log.LogInfo(__instance.MapNode.OccupyingPiece);
					SaveManager.saveFile.grimoraData.removedPieces.Add(__instance.saveId);
					ChessboardNavGrid.instance.zones[__instance.gridXPos, __instance.gridYPos].GetComponent<ChessboardMapNode>()
						.OccupyingPiece = null;
					Singleton<MapNodeManager>.Instance.SetAllNodesInteractable(false);
					__instance.MapNode.OccupyingPiece = null;
					// Log.LogInfo(__instance.MapNode.OccupyingPiece);
					__instance.MapNode.nodeId = __instance.saveId;
				}
			}
		
	}
}