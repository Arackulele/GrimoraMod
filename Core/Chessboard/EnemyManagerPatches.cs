using DiskCardGame;
using HarmonyLib;

namespace GrimoraMod
{
	[HarmonyPatch(typeof(ChessboardEnemyManager))]
	public class EnemyManagerPatches
	{
		[HarmonyPrefix, HarmonyPatch(nameof(ChessboardEnemyManager.MoveOpponentPieces))]
		public static bool DisableMovingOpponentPiecesPrefix()
		{
			return false;
		}
		
		[HarmonyPrefix, HarmonyPatch(nameof(ChessboardEnemyManager.StartCombatWithEnemy))]
		public static bool ModifyPieceToBeAddedToConfigListPrefix(
			ChessboardEnemyManager __instance, ChessboardEnemyPiece enemy, bool playerStarted)
		{
			GrimoraPlugin.Log.LogDebug($"[ChessboardEnemyManager.StartCombatWithEnemy] " +
			                           $"Adding enemy [{enemy.name}] to config removed pieces");
			// TODO: this allows for someone to enter a fight but they can just leave to despawn the piece
			ChessUtils.AddPieceToRemovedPiecesConfig(enemy.name);
			__instance.enemyPieces.Remove(enemy);
			enemy.MapNode.OccupyingPiece = null;
			Singleton<MapNodeManager>.Instance.SetAllNodesInteractable(nodesInteractable: false);
			__instance.StartCoroutine(__instance.StartCombatSequence(enemy, playerStarted));
			
			return false;
		}
	}
}