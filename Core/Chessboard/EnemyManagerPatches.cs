using DiskCardGame;
using HarmonyLib;

namespace GrimoraMod.Chessboard
{
	[HarmonyPatch(typeof(ChessboardEnemyManager))]
	public class EnemyManagerPatches
	{
		[HarmonyPrefix, HarmonyPatch(nameof(ChessboardEnemyManager.MoveOpponentPieces))]
		public static bool DisableMovingOpponentPiecesPrefix()
		{
			return false;
		}
	}
}