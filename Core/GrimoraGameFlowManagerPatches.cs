using System.Collections.Generic;
using DiskCardGame;
using HarmonyLib;
using UnityEngine;

namespace GrimoraMod
{
	[HarmonyPatch(typeof(GrimoraGameFlowManager))]
	public class GrimoraGameFlowManagerPatches
	{
		private static GameObject PrefabGrimoraSelectableCard =
			ResourceBank.Get<GameObject>("Prefabs/Cards/SelectableCard_Grimora");

		[HarmonyPrefix, HarmonyPatch(nameof(GrimoraGameFlowManager.SceneSpecificInitialization))]
		public static bool PrefixAddMultipleSequencersDuringLoad(GrimoraGameFlowManager __instance)
		{
			if (SaveManager.SaveFile.IsGrimora)
			{
				ChangeChessboardToExtendedClass();

				AddRareCardSequencerToScene();

				AddDeckReviewSequencerToScene();

				bool skipIntro = false;
				bool skipTombstone = false;

				if (FinaleDeletionWindowManager.instance != null)
				{
					// GrimoraPlugin.Log.LogDebug($"[SceneSpecificInitialization] Destroying FinaleDeletionWindowManager as it exists");
					Object.Destroy(FinaleDeletionWindowManager.instance.gameObject);
				}

				ViewManager.Instance.SwitchToView(View.Default, immediate: true);

				if (!StoryEventsData.EventCompleted(StoryEvent.GrimoraReachedTable))
				{
					// GrimoraPlugin.Log.LogDebug($"[SceneSpecificInitialization] GrimoraReachedTable is false");

					if (GameMap.Instance != null)
					{
						// GrimoraPlugin.Log.LogDebug($"[SceneSpecificInitialization] Calling GameMap.HideMapImmediate as it exists");
						GameMap.Instance.HideMapImmediate();
					}

					// GrimoraPlugin.Log.LogDebug($"[SceneSpecificInitialization] Setting __instance.CurrentGameState to GameState.FirstPerson3D");
					__instance.CurrentGameState = GameState.FirstPerson3D;

					// GrimoraPlugin.Log.LogDebug($"[SceneSpecificInitialization] Transitioning to FirstPerson3D");
					__instance.StartCoroutine(__instance.TransitionTo(GameState.FirstPerson3D, null, immediate: true));

					ExplorableAreaManager.Instance.HangingLight.gameObject.SetActive(skipTombstone);
					ExplorableAreaManager.Instance.HandLight.gameObject.SetActive(skipTombstone);

					__instance.gameTableCandlesParent.SetActive(skipTombstone);
					__instance.gravestoneNavZone.SetActive(skipTombstone);

					if (!skipIntro)
					{
						// GrimoraPlugin.Log.LogDebug($"[SceneSpecificInitialization] Intro is not being skipped");
						__instance.StartCoroutine(__instance.StartSceneSequence());
					}
				}
				else
				{
					// GrimoraPlugin.Log.LogDebug($"[SceneSpecificInitialization] GrimoraReachedTable is true, playing finalegrimora_ambience");
					AudioController.Instance.SetLoopAndPlay("finalegrimora_ambience");
					if (GameMap.Instance != null)
					{
						// GrimoraPlugin.Log.LogDebug($"[SceneSpecificInitialization] Setting CurrentGameState to GameState.Map");
						__instance.CurrentGameState = GameState.Map;
						// GrimoraPlugin.Log.LogDebug($"[SceneSpecificInitialization] Transitioning to GameState.Map");
						__instance.StartCoroutine(__instance.TransitionTo(GameState.Map, null, immediate: true));
					}
				}

				return false;
			}

			return true;
		}

		private static void ChangeChessboardToExtendedClass()
		{
			if (Object.FindObjectOfType<ChessboardMapExt>() is null)
			{
				// GrimoraPlugin.Log.LogDebug($"Adding MapExt to ChessboardMapGameObject");
				GameObject boardObj = GameObject.Find("ChessboardGameMap");
				ChessboardMap boardComp = boardObj.GetComponent<ChessboardMap>();

				ChessboardMapExt ext = boardObj.gameObject.AddComponent<ChessboardMapExt>();
				ext.dynamicElementsParent = boardComp.dynamicElementsParent;
				ext.mapAnim = boardComp.mapAnim;
				ext.navGrid = boardComp.navGrid;
				ext.pieces = new List<ChessboardPiece>();
				ext.defaultPosition = boardComp.defaultPosition;

				// GrimoraPlugin.Log.LogDebug($"Destroying old chessboard component");
				Object.Destroy(boardComp);

				var allPieces = Object.FindObjectsOfType<ChessboardPiece>();
				// GrimoraPlugin.Log.LogDebug($"[ChangeChessboardToExtendedClass] Resetting initial pieces" +
				//                            $" {string.Join(", ", allPieces.Select(_ => _.name))}");
				foreach (var piece in allPieces)
				{
					ext.pieces.Remove(piece);
					piece.MapNode.OccupyingPiece = null;
					piece.gameObject.SetActive(false);
					Object.Destroy(piece.gameObject);
				}
			}
		}

		private static void AddDeckReviewSequencerToScene()
		{
			DeckReviewSequencer deckReviewSequencer = Object.FindObjectOfType<DeckReviewSequencer>();

			if (deckReviewSequencer is not null)
			{
				// DeckReviewSequencer reviewSequencer = deckReviewSequencerObj.GetComponent<DeckReviewSequencer>();
				SelectableCardArray cardArray = deckReviewSequencer.GetComponentInChildren<SelectableCardArray>();
				cardArray.selectableCardPrefab = PrefabGrimoraSelectableCard;
			}
		}

		private static void AddRareCardSequencerToScene()
		{
			SpecialNodeHandler specialNodeHandler = Object.FindObjectOfType<SpecialNodeHandler>();

			// GrimoraPlugin.Log.LogDebug($"Creating RareCardChoiceSelector");

			GameObject rareCardChoicesSelector = Object.Instantiate(
				ResourceBank.Get<GameObject>("Prefabs/SpecialNodeSequences/RareCardChoiceSelector"),
				specialNodeHandler.transform
			);

			RareCardChoicesSequencer sequencer = rareCardChoicesSelector.GetComponent<RareCardChoicesSequencer>();

			// GrimoraPlugin.Log.LogDebug($"-> Setting RareCardChoicesSequencer choice generator to Part1RareChoiceGenerator");
			sequencer.choiceGenerator = rareCardChoicesSelector.AddComponent<Part1RareChoiceGenerator>();

			// GrimoraPlugin.Log.LogDebug($"-> Setting RareCardChoicesSequencer selectableCardPrefab to SelectableCard_Grimora");
			sequencer.selectableCardPrefab = PrefabGrimoraSelectableCard;

			// GrimoraPlugin.Log.LogDebug($"-> Setting SpecialNodeHandler rareCardChoiceSequencer to sequencer");
			specialNodeHandler.rareCardChoiceSequencer = sequencer;
		}
	}
}