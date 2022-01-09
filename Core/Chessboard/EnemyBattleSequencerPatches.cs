using System;
using System.Collections;
using System.Collections.Generic;
using DiskCardGame;
using HarmonyLib;
using UnityEngine;

namespace GrimoraMod.Chessboard
{
	[HarmonyPatch(typeof(ChessboardEnemyBattleSequencer), nameof(ChessboardEnemyBattleSequencer.PreCleanUp))]
	public class EnemyBattleSequencerPatches
	{
		public static void Prefix(ref ChessboardEnemyBattleSequencer __instance,
			out ChessboardEnemyBattleSequencer __state)
		{
			__state = __instance;
		}

		public static IEnumerator Postfix(IEnumerator enumeratorz, ChessboardEnemyBattleSequencer __state)
		{
			if (!Singleton<TurnManager>.Instance.PlayerWon)
			{
				AudioController.Instance.FadeOutLoop(3f, Array.Empty<int>());
				Singleton<ViewManager>.Instance.SwitchToView(View.Default, false, true);
				Singleton<PlayerHand>.Instance.PlayingLocked = true;
				Singleton<InteractionCursor>.Instance.InteractionDisabled = true;
				__state.StartCoroutine(Singleton<CardDrawPiles>.Instance.CleanUp());
				__state.StartCoroutine(Singleton<TurnManager>.Instance.Opponent.CleanUp());
				yield return (Singleton<BoardManager>.Instance as BoardManager3D).HideSlots();
				foreach (PlayableCard c in Singleton<BoardManager>.Instance.CardsOnBoard)
				{
					__state.GlitchOutCard(c);
					yield return new WaitForSeconds(0.1f);
				}

				List<PlayableCard>.Enumerator enumerator = default(List<PlayableCard>.Enumerator);
				foreach (PlayableCard c2 in Singleton<PlayerHand>.Instance.CardsInHand)
				{
					__state.GlitchOutCard(c2);
					yield return new WaitForSeconds(0.1f);
				}

				enumerator = default(List<PlayableCard>.Enumerator);
				Singleton<PlayerHand>.Instance.SetShown(false, false);
				yield return new WaitForSeconds(0.75f);
				Singleton<TableRuleBook>.Instance.enabled = false;
				GlitchOutAssetEffect.GlitchModel(Singleton<TableRuleBook>.Instance.transform, false, true);
				yield return new WaitForSeconds(0.75f);
				yield return Singleton<TextDisplayer>.Instance.ShowUntilInput("Let the circle reset");
				yield return new WaitForSeconds(0.5f);
				Singleton<InteractionCursor>.Instance.InteractionDisabled = false;
				///SaveManager.saveFile.ResetRun();
				SaveManager.saveFile.grimoraData.Initialize();
				SaveManager.SaveToFile();
				StoryEventsData.EraseEvent(StoryEvent.PlayerDeletedArchivistFile);
				StoryEventsData.EraseEvent(StoryEvent.FactoryConveyorBeltMoved);
				StoryEventsData.EraseEvent(StoryEvent.Part3PurchasedHoloBrush);
				StoryEventsData.EraseEvent(StoryEvent.FactoryCuckooClockAppeared);
				SceneLoader.Load("Start");
			}
			else if (!DialogueEventsData.EventIsPlayed("FinaleGrimoraBattleWon"))
			{
				Singleton<ViewManager>.Instance.Controller.LockState = ViewLockState.Locked;
				yield return new WaitForSeconds(0.5f);
				yield return Singleton<TextDisplayer>.Instance.PlayDialogueEvent("FinaleGrimoraBattleWon",
					TextDisplayer.MessageAdvanceMode.Input, TextDisplayer.EventIntersectMode.Wait, null, null);
			}

			yield break;
		}
	}
}