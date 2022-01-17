using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DiskCardGame;
using HarmonyLib;
using UnityEngine;
using static GrimoraMod.GrimoraPlugin;

namespace GrimoraMod;

[HarmonyPatch(typeof(GameFlowManager))]
public class BaseGameFlowManagerPatches
{
	private static GameObject PrefabGrimoraSelectableCard =
		ResourceBank.Get<GameObject>("Prefabs/Cards/SelectableCard_Grimora");

	[HarmonyPrefix, HarmonyPatch(nameof(GameFlowManager.Start))]
	public static void PrefixStart(GameFlowManager __instance)
	{
		GameObject boardObj = GameObject.Find("ChessboardGameMap");
		Log.LogDebug($"[GameFlowManager.Start] Instance is [{__instance.GetType()}]" +
		             $" Board Already exists? [{boardObj is not null}]");
		if (SaveManager.SaveFile.IsGrimora && boardObj is not null)
		{
			ChangeChessboardToExtendedClass();

			AddRareCardSequencerToScene();

			AddDeckReviewSequencerToScene();

			// ResizeArtworkForVanillaBoneCards();

			ChangeStartDeckIfNotAlreadyChanged();
		}
	}

	private static void ChangeStartDeckIfNotAlreadyChanged()
	{
		List<CardInfo> grimoraDeck = GrimoraSaveData.Data.deck.Cards;
		int graveDiggerCount = grimoraDeck.Count(info => info.name == "Gravedigger");
		int frankNSteinCount = grimoraDeck.Count(info => info.name == "FrankNStein");
		if (grimoraDeck.Count == 5 && graveDiggerCount == 3 && frankNSteinCount == 2)
		{
			Log.LogDebug($"[ChangeStartDeckIfNotAlreadyChanged] Starter deck needs reset");
			GrimoraSaveData.Data.Initialize();
		}
	}

	private static void ChangeChessboardToExtendedClass()
	{
		// Log.LogDebug($"[ChangeChessboardToExtendedClass] Adding MapExt to ChessboardMapGameObject");
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

		var initialStartingPieces = Object.FindObjectsOfType<ChessboardPiece>();
		Log.LogDebug($"[ChangeChessboardToExtendedClass] Resetting initial pieces" +
		             $" {string.Join(", ", initialStartingPieces.Select(_ => _.name))}");

		foreach (var piece in initialStartingPieces)
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

	private static void ResizeArtworkForVanillaBoneCards()
	{
		List<string> cardsToResizeArtwork = new List<string>
		{
			"Amoeba", "Bat", "Maggots", "Rattler", "Vulture",
		};

		var newPivot = new Vector2(0.5f, 0.65f);

		CardLoader.AllData.ForEach(info =>
		{
			if (cardsToResizeArtwork.Contains(info.name))
			{
				Sprite spriteCopy = info.portraitTex;

				// Log.LogDebug($"[{info.name}] Rect {spriteCopy.rect} Pivot [{spriteCopy.pivot}] PPU [{spriteCopy.pixelsPerUnit}]");
				info.portraitTex = Sprite.Create(
					spriteCopy.texture, spriteCopy.rect, newPivot, 125f
				);
			}
		});
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
		// GrimoraPlugin.Log.LogDebug($"[GameFlowManager.TransitionTo] Current state is [{__instance.CurrentGameState}]");

		if (SaveManager.SaveFile.IsGrimora)
		{
			bool isBossDefeated = ChessboardMapExt.Instance.BossDefeated;
			bool piecesExist = ChessboardMapExt.Instance.pieces.Count > 0;

			// GrimoraPlugin.Log.LogDebug($"[TransitionTo] IsBossDefeated [{isBossDefeated}] Pieces exist [{piecesExist}]");

			// FOR ENUMS IN POSTFIX CALLS, THE OPERATOR TO USE IS 'IS' NOT '==' 
			// CORRECT  : gameState is GameState.Map
			// INCORRECT: gameState == GameState.Map
			if (gameState is GameState.Map && piecesExist && isBossDefeated)
			{
				yield return ChessboardMapExt.Instance.CompleteRegionSequence();

				__instance.CurrentGameState = gameState;
			}
			else
			{
				// GrimoraPlugin.Log.LogDebug($"[TransitionTo] yield return the enumerator inside SaveFile");
				yield return enumerator;
			}

			// GrimoraPlugin.Log.LogDebug($"[TransitionTo] yield breaking");
			yield break;
		}

		// GrimoraPlugin.Log.LogDebug($"[GameFlowManager.TransitionTo] GameState is [{gameState}]");
		// GrimoraPlugin.Log.LogDebug($"[TransitionTo] yield return the enumerator");
		yield return enumerator;
	}
}