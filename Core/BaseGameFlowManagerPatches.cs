using System.Collections;
using DiskCardGame;
using HarmonyLib;
using UnityEngine;
using static GrimoraMod.GrimoraPlugin;

namespace GrimoraMod;

[HarmonyPatch(typeof(GameFlowManager))]
public class BaseGameFlowManagerPatches
{
	public static readonly GameObject PrefabGrimoraSelectableCard =
		ResourceBank.Get<GameObject>("Prefabs/Cards/SelectableCard_Grimora");

	public static readonly GameObject PrefabGrimoraCardBack =
		ResourceBank.Get<GameObject>("Prefabs/Cards/CardBack_Grimora");

	public static SpecialNodeHandler SpecialNodeHandler;

	private static readonly int EmissionColor = Shader.PropertyToID("_EmissionColor");

	private static GameObject SetupSelectableCard()
	{
		GameObject selectable = ResourceBank.Get<GameObject>("Prefabs/Cards/SelectableCard_Grimora");
		RuntimeAnimatorController controller
			= ResourceBank.Get<RuntimeAnimatorController>("Animation/Cards/PaperCards/Card");

		selectable.GetComponent<Animator>().runtimeAnimatorController = controller;

		return selectable;
	}

	[HarmonyPrefix, HarmonyPatch(nameof(GameFlowManager.Start))]
	public static void PrefixStart(GameFlowManager __instance)
	{
		GameObject boardObj = GameObject.Find("ChessboardGameMap");
		Log.LogDebug($"[GameFlowManager.Start] Instance is [{__instance.GetType()}]" +
		             $" Board Already exists? [{boardObj is not null}]");
		if (SaveManager.SaveFile.IsGrimora && boardObj is not null)
		{
			SpecialNodeHandler = UnityEngine.Object.FindObjectOfType<SpecialNodeHandler>();

			ChangeChessboardToExtendedClass();

			AddRareCardSequencerToScene();

			AddDeckReviewSequencerToScene();

			ChangeStartDeckIfNotAlreadyChanged();

			AddEnergyDrone();

			AddCardRemoveSequencer();
		}
	}

	private static void AddCardRemoveSequencer()
	{
		// TODO: This will work, but it doesn't show the boon art correctly.

		Log.LogDebug($"Creating card remove sequencer");
		GameObject cardRemoveSequencerObj = UnityEngine.Object.Instantiate(
			ResourceBank.Get<GameObject>("Prefabs/SpecialNodeSequences/CardRemoveSequencer"),
			SpecialNodeHandler.transform
		);

		var oldRemoveSequencer = cardRemoveSequencerObj.GetComponent<CardRemoveSequencer>();

		Log.LogDebug($"Adding new card remove sequencer component");
		var cardRemoveSequencer = cardRemoveSequencerObj.gameObject.AddComponent<GrimoraCardRemoveSequencer>();

		Log.LogDebug($"Setting prefabs");
		cardRemoveSequencer.gamepadGrid = oldRemoveSequencer.gamepadGrid;
		cardRemoveSequencer.selectableCardPrefab = PrefabGrimoraSelectableCard;
		cardRemoveSequencer.confirmStone = oldRemoveSequencer.confirmStone;
		cardRemoveSequencer.sacrificeSlot = oldRemoveSequencer.sacrificeSlot;
		cardRemoveSequencer.sacrificeSlot.cardSelector.selectableCardPrefab = PrefabGrimoraSelectableCard;
		cardRemoveSequencer.sacrificeSlot.pile.cardbackPrefab = PrefabGrimoraCardBack;
		cardRemoveSequencer.skullEyes = oldRemoveSequencer.skullEyes;
		cardRemoveSequencer.stoneCircleAnim = oldRemoveSequencer.stoneCircleAnim;

		cardRemoveSequencer.GetComponentInChildren<SelectableCardArray>().selectableCardPrefab =
			PrefabGrimoraSelectableCard;

		Log.LogDebug($"Setting card backs");
		cardRemoveSequencer.deckPile = oldRemoveSequencer.deckPile;
		cardRemoveSequencer.deckPile.cardbackPrefab = PrefabGrimoraCardBack;

		Log.LogDebug($"Destroying old sequencer");
		UnityEngine.Object.Destroy(oldRemoveSequencer);

		SpecialNodeHandler.cardRemoveSequencer = cardRemoveSequencer;

		// TODO: HOW DO WE GET THOSE DECALS TO SHOW UP
		CardDisplayer3D displayer3D = ResourceBank.Get<CardDisplayer3D>("Prefabs/Cards/CardElements");

		CardDisplayer3D graveDisplayer = UnityEngine.Object.FindObjectOfType<CardDisplayer3D>();

		BoonIconInteractable cardAbilityIcons = UnityEngine.Object.Instantiate(
			ResourceBank.Get<BoonIconInteractable>("Prefabs/Cards/CardSurfaceInteraction/BoonIcon"),
			graveDisplayer.GetComponentInChildren<CardAbilityIcons>().transform
		);
		graveDisplayer.GetComponentInChildren<CardAbilityIcons>().boonIcon = cardAbilityIcons;

		GameObject cardDecals = UnityEngine.Object.Instantiate(
			displayer3D.transform.GetChild(9).gameObject,
			graveDisplayer.transform, true
		);

		graveDisplayer.decalRenderers.Clear();
		for (int i = 0; i < cardDecals.transform.childCount; i++)
		{
			graveDisplayer.decalRenderers.Add(cardDecals.transform.GetChild(i).GetComponent<MeshRenderer>());
		}
	}

	private static void AddEnergyDrone()
	{
		BoardManager3D boardManager = UnityEngine.Object.FindObjectOfType<BoardManager3D>();

		ResourceDrone drone = UnityEngine.Object.Instantiate(
			ResourceBank.Get<ResourceDrone>("Prefabs/CardBattle/ResourceModules"),
			new Vector3(5.3f, 5.5f, 1.92f),
			Quaternion.Euler(270f, 0f, -146.804f),
			boardManager.transform
		);

		Color grimoraTextColor = new Color(0.420f, 1f, 0.63f);
		drone.name = "Grimora Resource Drone";
		drone.baseCellColor = grimoraTextColor;
		drone.highlightedCellColor = new Color(1, 1, 0.23f);

		Log.LogDebug($"[AddEnergyDrone] Disabling animation");
		Animator animator = drone.GetComponentInChildren<Animator>();
		animator.enabled = false;

		Transform moduleEnergy = animator.transform.GetChild(0);
		Log.LogDebug($"[AddEnergyDrone] Getting module energy and setting mesh to null");
		moduleEnergy.gameObject.GetComponent<MeshFilter>().mesh = null;

		int modulesChildren = moduleEnergy.childCount;
		for (int i = 1; i < 7; i++)
		{
			Transform energyCell = moduleEnergy.GetChild(i);
			Log.LogDebug($"[AddEnergyDrone] Energy cell [{energyCell.name}]");
			energyCell.gameObject.GetComponent<MeshFilter>().mesh = null;
			var energyCellCase = energyCell.GetChild(0);
			energyCellCase.GetChild(0).GetComponent<MeshRenderer>().material.SetColor(EmissionColor, grimoraTextColor);
			energyCellCase.GetChild(1).GetComponent<MeshFilter>().mesh = null;
			energyCellCase.GetChild(2).GetComponent<MeshFilter>().mesh = null;
		}

		Log.LogDebug($"[AddEnergyDrone] Setting Connector inactive");
		moduleEnergy.Find("Connector").gameObject.SetActive(false);
		Log.LogDebug($"[AddEnergyDrone] Setting Propellers inactive");
		moduleEnergy.Find("Propellers").gameObject.SetActive(false);
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
		UnityEngine.Object.Destroy(boardComp);

		var initialStartingPieces = UnityEngine.Object.FindObjectsOfType<ChessboardPiece>();
		Log.LogDebug($"[ChangeChessboardToExtendedClass] Resetting initial pieces" +
		             $" {string.Join(", ", initialStartingPieces.Select(_ => _.name))}");

		foreach (var piece in initialStartingPieces)
		{
			ext.pieces.Remove(piece);
			piece.MapNode.OccupyingPiece = null;
			piece.gameObject.SetActive(false);
			UnityEngine.Object.Destroy(piece.gameObject);
		}
	}

	private static void AddDeckReviewSequencerToScene()
	{
		DeckReviewSequencer deckReviewSequencer = UnityEngine.Object.FindObjectOfType<DeckReviewSequencer>();

		if (deckReviewSequencer is not null)
		{
			// DeckReviewSequencer reviewSequencer = deckReviewSequencerObj.GetComponent<DeckReviewSequencer>();
			SelectableCardArray cardArray = deckReviewSequencer.GetComponentInChildren<SelectableCardArray>();
			cardArray.selectableCardPrefab = PrefabGrimoraSelectableCard;
		}
	}

	private static void AddRareCardSequencerToScene()
	{
		// GrimoraPlugin.Log.LogDebug($"Creating RareCardChoiceSelector");

		GameObject rareCardChoicesSelector = UnityEngine.Object.Instantiate(
			ResourceBank.Get<GameObject>("Prefabs/SpecialNodeSequences/RareCardChoiceSelector"),
			SpecialNodeHandler.transform
		);

		RareCardChoicesSequencer sequencer = rareCardChoicesSelector.GetComponent<RareCardChoicesSequencer>();
		sequencer.deckPile.cardbackPrefab = PrefabGrimoraCardBack;

		// GrimoraPlugin.Log.LogDebug($"-> Setting RareCardChoicesSequencer choice generator to Part1RareChoiceGenerator");
		sequencer.choiceGenerator = rareCardChoicesSelector.AddComponent<GrimoraRareChoiceGenerator>();

		// GrimoraPlugin.Log.LogDebug($"-> Setting RareCardChoicesSequencer selectableCardPrefab to SelectableCard_Grimora");
		sequencer.selectableCardPrefab = PrefabGrimoraSelectableCard;

		// GrimoraPlugin.Log.LogDebug($"-> Setting SpecialNodeHandler rareCardChoiceSequencer to sequencer");
		SpecialNodeHandler.rareCardChoiceSequencer = sequencer;
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
