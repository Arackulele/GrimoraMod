using DiskCardGame;
using GrimoraMod.Saving;
using HarmonyLib;
using UnityEngine;

namespace GrimoraMod;

[HarmonyPatch(typeof(ChessboardEnemyManager))]
public class EnemyManagerPatches
{

	public static List<EncounterData.StartCondition> OneCondition = new List<EncounterData.StartCondition>();

	[HarmonyPrefix, HarmonyPatch(nameof(ChessboardEnemyManager.MoveOpponentPieces))]
	public static bool PrefixDisableMovingOpponentPieces()
	{
		return false;
	}

	[HarmonyPrefix, HarmonyPatch(nameof(ChessboardEnemyManager.StartCombatWithEnemy))]
	public static bool PrefixModifyPieceToBeAddedToConfigList(
		ChessboardEnemyManager __instance, ChessboardEnemyPiece enemy, bool playerStarted)
	{

		//Dynamically set Blueprint data here, which happens right before the battle starts
		List<EncounterData.StartCondition> CompoundStartCon = new List<EncounterData.StartCondition>();
		CompoundStartCon.Clear();

		switch (GrimoraRunState.CurrentRun.regionTier)
		{
			case 0:
				CompoundStartCon.AddRange(GrimoraModBattleSequencer.KayceeStarts);
				Debug.Log("Adding Kaycee starter Range to start conditions");
				break;
			case 1:
				CompoundStartCon.AddRange(GrimoraModBattleSequencer.SawyerStarts);
				Debug.Log("Adding Sawyer starter Range to start conditions");
				break;
			case 2:
				CompoundStartCon.AddRange(GrimoraModBattleSequencer.RoyalStarts);
				break;
			case 3:
				//CompoundStartCon.AddRange(GrimoraModBattleSequencer.GrimoraStarts);
				Debug.Log("Adding Royal starter Range to start conditions");
				break;

		}
		CompoundStartCon.AddRange(GrimoraModBattleSequencer.GlobalStartCon);

		OneCondition.Clear();
		if (UnityEngine.Random.Range(0, 10) > 3)OneCondition.Add(CompoundStartCon.GetRandomItem());

		// this will correctly place the pieces back if they aren't defeated.
		// e.g. from quitting mid match
		if (enemy is ChessboardGoatEyePiece) { 
			//GrimoraRunState.CurrentRun.PiecesRemovedFromBoard.Add(enemy.name);
			AnkhGuardCombatSequencer.ActiveEnemyPiece = enemy;
		}
		else GrimoraModBattleSequencer.ActiveEnemyPiece = enemy;

		MapNodeManager.Instance.SetAllNodesInteractable(false);
		__instance.StartCoroutine(__instance.StartCombatSequence(enemy, playerStarted));

		return false;
	}



}
