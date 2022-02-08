using DiskCardGame;
using HarmonyLib;

namespace GrimoraMod;

[HarmonyPatch(typeof(ChessboardEnemyPiece))]
public class EnemyPatches
{
	[HarmonyPostfix, HarmonyPatch(nameof(ChessboardEnemyPiece.Start))]
	public static void ChangeBlueprintAfter(ref ChessboardEnemyPiece __instance)
	{
		if (__instance.specialEncounterId.Contains("Boss"))
		{
			__instance.blueprint = BaseBossExt.OpponentTupleBySpecialId[__instance.specialEncounterId].Item3;
		}
		else
		{
			__instance.blueprint = GetBlueprint();
		}
	}

	private static EncounterBlueprintData GetBlueprint()
	{
		var blueprints
			= BlueprintUtils.RegionWithBlueprints[ChessboardMapExt.Instance.ActiveChessboard.ActiveBossType];
		return blueprints[UnityEngine.Random.RandomRangeInt(0, blueprints.Count)];
	}
}
