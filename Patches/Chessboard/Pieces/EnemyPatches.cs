using DiskCardGame;
using HarmonyLib;

namespace GrimoraMod;

[HarmonyPatch(typeof(ChessboardEnemyPiece))]
public class EnemyPatches
{
	[HarmonyPostfix, HarmonyPatch(nameof(ChessboardEnemyPiece.Start))]
	public static void StartPrefix(ChessboardEnemyPiece __instance)
	{
		// __instance.TurnToFacePoint(ChessboardNavGrid.instance.zones[3, 3].transform.position, 0.1f);
	}

}
