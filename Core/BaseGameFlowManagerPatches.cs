using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DiskCardGame;
using HarmonyLib;
using UnityEngine;

namespace GrimoraMod
{
	[HarmonyPatch(typeof(GameFlowManager))]
	public class BaseGameFlowManagerPatches
	{
		private static GameObject PrefabGrimoraSelectableCard =
			ResourceBank.Get<GameObject>("Prefabs/Cards/SelectableCard_Grimora");

		[HarmonyPrefix, HarmonyPatch(nameof(GameFlowManager.Start))]
		public static void PrefixStart(GameFlowManager __instance)
		{
			GameObject boardObj = GameObject.Find("ChessboardGameMap");
			GrimoraPlugin.Log.LogDebug($"[GameFlowManager.Start] Instance is [{__instance.GetType()}]" +
			                           $" Board Already exists? [{boardObj is not null}]");
			if (SaveManager.SaveFile.IsGrimora && boardObj is not null)
			{
				ChangeChessboardToExtendedClass();

				AddRareCardSequencerToScene();

				AddDeckReviewSequencerToScene();
			}
		}

		private static void ChangeChessboardToExtendedClass()
		{
			GrimoraPlugin.Log.LogDebug($"Adding MapExt to ChessboardMapGameObject");
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
			GrimoraPlugin.Log.LogDebug($"[ChangeChessboardToExtendedClass] Resetting initial pieces" +
			                           $" {string.Join(", ", allPieces.Select(_ => _.name))}");
			foreach (var piece in allPieces)
			{
				ext.pieces.Remove(piece);
				piece.MapNode.OccupyingPiece = null;
				piece.gameObject.SetActive(false);
				Object.Destroy(piece.gameObject);
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

		[HarmonyPostfix, HarmonyPatch(nameof(GameFlowManager.TransitionTo))]
		public static IEnumerator PostfixGameLogicPatch(
			IEnumerator enumerator,
			GameFlowManager __instance,
			GameState gameState,
			NodeData triggeringNodeData = null,
			bool immediate = false,
			bool unlockViewAfterTransition = true
		)
		{
			// GrimoraPlugin.Log.LogDebug($"[GameFlowManager.TransitionTo] GameState is [{gameState}]");
			GrimoraPlugin.Log.LogDebug($"[GameFlowManager.TransitionTo] yield return the enumerator");
			yield return enumerator;

			if (SaveManager.SaveFile.IsGrimora && gameState == GameState.Map)
			{
				bool isBossDefeated = ChessboardMapExt.Instance.BossDefeated;
				bool piecesExist = ChessboardMapExt.Instance.pieces.Count > 0;

				// GrimoraPlugin.Log.LogDebug($"[GameFlowManager.TransitionTo] IsBossDefeated? [{isBossDefeated}] Pieces exist? [{piecesExist}]");
				if (piecesExist && isBossDefeated)
				{
					// GrimoraPlugin.Log.LogDebug($"[GameFlowManager.TransitionTo] ChessboardMapExt is not null");
					ChessboardMapExt.Instance.BossDefeated = false;
					// GrimoraPlugin.Log.LogDebug($"[GameFlowManager.TransitionTo] Calling CompleteRegionSequence");
					yield return ChessboardMapExt.Instance.CompleteRegionSequence();
				}
			}
		}
	}
}