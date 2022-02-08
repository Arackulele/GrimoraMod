using DiskCardGame;
using HarmonyLib;

namespace GrimoraMod;

[HarmonyPatch(typeof(ChessboardEnemyPiece))]
public class EnemyPatches
{
	[HarmonyPostfix, HarmonyPatch(nameof(ChessboardEnemyPiece.Start))]
	public static void ChangeBlueprintAfter(ref ChessboardEnemyPiece __instance)
	{
		__instance.blueprint = GetBlueprint();
	}
	
	private static EncounterBlueprintData GetBlueprint()
	{
		// Log.LogDebug($"[GetBlueprint] ActiveBoss [{ActiveBossType}]");
		var blueprints 
			= BlueprintUtils.RegionWithBlueprints[ChessboardMapExt.Instance.ActiveChessboard.ActiveBossType];
		return blueprints[UnityEngine.Random.RandomRangeInt(0, blueprints.Count)];
	}

}
