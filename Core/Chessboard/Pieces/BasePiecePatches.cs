using System.Collections.Generic;
using System.Linq;
using DiskCardGame;
using HarmonyLib;

namespace GrimoraMod
{
	[HarmonyPatch(typeof(ChessboardPiece))]
	public class ChessboardPiecePatches
	{
		
		[HarmonyPrefix, HarmonyPatch(nameof(ChessboardPiece.UpdateSaveState))]
		public static bool UpdatePrefix(ChessboardPiece __instance)
		{
			// if(GrimoraSaveData.Data.removedPieces.Count != 0)
			// {
			// 	GrimoraPlugin.Log.LogDebug($"-> Current removed pieces [{string.Join(", ", GrimoraSaveData.Data.removedPieces)}]");
			// }
			
			// GrimoraPlugin.Log.LogDebug($"[ChessboardPiece.UpdateSaveState]" +
			//                            $"for piece [{__instance.name}] saveId [{__instance.saveId}]");
			
			if (GrimoraPlugin.ConfigCurrentRemovedPieces.Value.Split(',').Contains(__instance.name))
			{
				GrimoraPlugin.Log.LogDebug($"[ChessboardPiece.UpdateSaveState] Setting piece [{__instance.name}] to destroy");
				__instance.MapNode.OccupyingPiece = null;
				__instance.gameObject.SetActive(value: false);
				// UnityEngine.Object.Destroy(__instance.gameObject);
			}
			else
			{
				GrimoraPlugin.Log.LogDebug($"[ChessboardPiece.UpdateSaveState] Setting piece [{__instance.name}] to active");
				__instance.gameObject.SetActive(value: true);
			}

			return false;
		}
	}
}