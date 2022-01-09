using System.Linq;
using DiskCardGame;
using HarmonyLib;

namespace GrimoraMod.Chessboard
{
	[HarmonyPatch(typeof(DiskCardGame.ChessboardPiece))]
	public class ChessboardPiecePatches
	{
		[HarmonyPrefix, HarmonyPatch(nameof(ChessboardPiece.UpdateSaveState))]
		public static bool UpdatePrefix(ChessboardPiece __instance)
		{
			// if(GrimoraSaveData.Data.removedPieces.Count != 0)
			// {
			// 	GrimoraPlugin.Log.LogDebug($"-> Current removed pieces [{string.Join(", ", GrimoraSaveData.Data.removedPieces)}]");
			// }
			
			if (GrimoraSaveData.Data.removedPieces.Contains(__instance.saveId))
			{
				GrimoraPlugin.Log.LogDebug($"--> Setting piece [{__instance.name}] to inactive");
				__instance.gameObject.SetActive(value: false);
				__instance.MapNode.OccupyingPiece = null;
			}
			else
			{
				GrimoraPlugin.Log.LogDebug($"--> Setting piece [{__instance.name}] to active");
				__instance.gameObject.SetActive(value: true);
			}

			return false;
		}
	}
}