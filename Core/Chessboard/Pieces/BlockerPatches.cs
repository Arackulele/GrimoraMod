using DiskCardGame;
using HarmonyLib;

namespace GrimoraMod;

[HarmonyPatch(typeof(DiskCardGame.ChessboardBlockerPiece))]
public class BlockerPatches
{
	[HarmonyPrefix, HarmonyPatch(nameof(ChessboardBlockerPiece.Start))]
	public static void UpdateLogicAtStartPrefix(ChessboardBlockerPiece __instance)
	{
		// this position code exists only in ChessboardEnemyPiece, which is why we need a patch for it
		__instance.gameObject.transform.position
			= ChessboardNavGrid.instance.zones[__instance.gridXPos, __instance.gridYPos].transform.position;

		ChessboardNavGrid
			.instance
			.zones[__instance.gridXPos, __instance.gridYPos]
			.GetComponent<ChessboardMapNode>()
			.OccupyingPiece = __instance;
	}
}