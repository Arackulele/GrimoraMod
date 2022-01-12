using System;
using System.Collections;
using System.Collections.Generic;
using DiskCardGame;
using HarmonyLib;
using UnityEngine;

namespace GrimoraMod
{
	[HarmonyPatch(typeof(ChessboardEnemyBattleSequencer))]
	public class EnemyBattleSequencerPatches
	{

		public static ChessboardEnemyPiece activeEnemyPiece;
		
		[HarmonyPrefix, HarmonyPatch(nameof(ChessboardEnemyBattleSequencer.PreCleanUp))]
		public static void Prefix(ref ChessboardEnemyBattleSequencer __instance,
			out ChessboardEnemyBattleSequencer __state)
		{
			__state = __instance;
		}

		[HarmonyPostfix, HarmonyPatch(nameof(ChessboardEnemyBattleSequencer.PreCleanUp))]
		public static IEnumerator Postfix(IEnumerator enumeratorz, ChessboardEnemyBattleSequencer __state)
		{
			
			GrimoraPlugin.Log.LogDebug($"[ChessboardEnemyBattleSequencer.PreCleanUp] PreCleanup postfix called");

			if (!TurnManager.Instance.PlayerWon)
			{
				GrimoraPlugin.Log.LogDebug($"[ChessboardEnemyBattleSequencer.PreCleanUp][Postfix] Player did not win...");
				AudioController.Instance.FadeOutLoop(3f, Array.Empty<int>());

				ViewManager.Instance.SwitchToView(View.Default, false, true);

				PlayerHand.Instance.PlayingLocked = true;

				InteractionCursor.Instance.InteractionDisabled = true;

				__state.StartCoroutine(CardDrawPiles.Instance.CleanUp());
				__state.StartCoroutine(TurnManager.Instance.Opponent.CleanUp());

				yield return (BoardManager.Instance as BoardManager3D).HideSlots();
				foreach (PlayableCard c in BoardManager.Instance.CardsOnBoard)
				{
					__state.GlitchOutCard(c);
					yield return new WaitForSeconds(0.1f);
				}

				List<PlayableCard>.Enumerator enumerator = default(List<PlayableCard>.Enumerator);
				foreach (PlayableCard c2 in PlayerHand.Instance.CardsInHand)
				{
					__state.GlitchOutCard(c2);
					yield return new WaitForSeconds(0.1f);
				}

				PlayerHand.Instance.SetShown(false, false);

				yield return new WaitForSeconds(0.75f);

				TableRuleBook.Instance.enabled = false;

				GlitchOutAssetEffect.GlitchModel(TableRuleBook.Instance.transform, false, true);

				yield return new WaitForSeconds(0.75f);

				yield return TextDisplayer.Instance.ShowUntilInput("Let the circle reset");

				yield return new WaitForSeconds(0.5f);

				InteractionCursor.Instance.InteractionDisabled = false;

				///SaveManager.saveFile.ResetRun();

				SaveManager.saveFile.grimoraData.Initialize();
				SaveManager.SaveToFile();

				SceneLoader.Load("Start");
			}
			
			GrimoraPlugin.Log.LogDebug($"[ChessboardEnemyBattleSequencer.PreCleanUp] " +
			                           $"Adding enemy [{activeEnemyPiece.name}] to config removed pieces");
			ChessboardMapExt.Instance.AddPieceToRemovedPiecesConfig(activeEnemyPiece.name);
			
			// else if (!DialogueEventsData.EventIsPlayed("FinaleGrimoraBattleWon"))
			// {
			// 	GrimoraPlugin.Log.LogDebug($"[ChessboardEnemyBattleSequencer.PreCleanUp][Postfix]" +
			// 	                           $" FinaleGrimoraBattleWon has not played yet, playing now.");
			// 	
			// 	ViewManager.Instance.Controller.LockState = ViewLockState.Locked;
			// 	yield return new WaitForSeconds(0.5f);
			// 	yield return TextDisplayer.Instance.PlayDialogueEvent(
			// 		"FinaleGrimoraBattleWon", TextDisplayer.MessageAdvanceMode.Input
			// 	);
			// }

			yield break;
		}
	}
}