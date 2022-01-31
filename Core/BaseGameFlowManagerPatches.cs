using System.Collections;
using DiskCardGame;
using GrimoraMod.Consumables;
using HarmonyLib;
using Sirenix.Serialization;
using Sirenix.Utilities;
using UnityEngine;
using static GrimoraMod.GrimoraPlugin;

namespace GrimoraMod;

[HarmonyPatch(typeof(GameFlowManager))]
public class BaseGameFlowManagerPatches
{
	private static GameObject PrefabGrimoraSelectableCard =>
		ResourceBank.Get<GameObject>("Prefabs/Cards/SelectableCard_Grimora");

	private static GameObject PrefabGrimoraPlayableCard =>
		ResourceBank.Get<GameObject>("Prefabs/Cards/PlayableCard_Grimora");

	private static GameObject PrefabGrimoraCardBack =>
		ResourceBank.Get<GameObject>("Prefabs/Cards/CardBack_Grimora");

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
		if (!SaveManager.SaveFile.IsGrimora)
		{
			return;
		}
		// GameObject giantPrefab = ResourceBank.Get<GameObject>("Prefabs/Cards/PlayableCard_Grimora");
		// giantPrefab.name += "_Giant";
		//
		// giantPrefab.GetComponent<Animator>().runtimeAnimatorController 
		// 	= ResourceBank.Get<GameObject>("Prefabs/Cards/PlayableCard_Giant")
		// 		.GetComponent<Animator>().runtimeAnimatorController;

		// giantPrefab.transform.GetChild(0).GetChild(0).GetChild(0).localPosition = new Vector3(-0.65f, 1.25f, 0f);
		// giantPrefab.transform.GetChild(0).GetChild(0).GetChild(0).localScale = new Vector3(1f, 1.175f, 0.2f);
		CardSpawner.Instance.giantPlayableCardPrefab = PrefabGrimoraPlayableCard;

		Log.LogDebug($"[GameFlowManager.Start] Instance is [{__instance.GetType()}] GameMap.Instance [{GameMap.Instance}]");

		AddCardRemoveSequencer();

		AddDeckReviewSequencerToScene();

		AddEnergyDrone();

		AddHammer();

		AddRareCardSequencerToScene();

		ChangeChessboardToExtendedClass();

		ChangeStartDeckIfNotAlreadyChanged();

		// AddBoonLordBoonConsumable();

		// AddCustomEnergy();
	}

	public static void AddBoonLordBoonConsumable()
	{
		Log.LogDebug($"Adding Boon Lord Consumable");
		GameObject ramSkull = Object.Instantiate(
			ResourceBank.Get<GameObject>("Art/Assets3D/NodeSequences/GoatSkull/RamSkull_NoHorn"),
			new Vector3(4.59f, 4.8f, 0),
			Quaternion.Euler(270, 235, 0)
		);
		Log.LogDebug($"Setting consumable name");
		ramSkull.name = "BoneLordBoon_Consumable";
		Log.LogDebug($"Setting consumable scale");
		ramSkull.transform.localScale = new Vector3(0.35f, 0.35f, 0.35f);

		Log.LogDebug($"Setting runtime controller");
		ramSkull.AddComponent<Animator>().runtimeAnimatorController =
			ResourceBank.Get<RuntimeAnimatorController>("Animation/Items/ItemAnim");

		Log.LogDebug($"Adding BoneLordSkull class");
		ramSkull.AddComponent<BoneLordSkull>();

		Log.LogDebug($"Creating scriptable object");
		ConsumableItemData itemData = ScriptableObject.CreateInstance<ConsumableItemData>();
		itemData.notRandomlyGiven = true;
		itemData.powerLevel = 1;
		itemData.rulebookCategory = AbilityMetaCategory.Part1Modular;
		itemData.rulebookName = "Bone Lord Boon of Bones";
		itemData.rulebookDescription = "How gracious of the Bone Lord to give you 8 starting bones.";
		itemData.rulebookSprite = Sprite.Create(Rect.zero, Vector2.zero, float.Epsilon);
		itemData.regionSpecific = false;

		if (!ItemsUtil.allData.Exists(x => (x as ConsumableItemData).rulebookName == itemData.rulebookName))
		{
			Log.LogDebug($"Adding consumable in ItemsUtil.allData");
			ItemsUtil.allData.Add(itemData);
		}

		// Log.LogDebug($"Updating items");
		// GrimoraItemsManagerExt.Instance.UpdateItems();
	}

	// ONLY USE THIS IF YOU COMPILED THE ASSET BUNDLE WITH A VERSION OF UNITY THAT IS NOT 2019.4.24F
	public static void FixShaders(GameObject go)
	{
		// Reset shader if this object has a MeshRenderer
		if (go.GetComponent<MeshRenderer>() != null)
		{
			go.GetComponent<MeshRenderer>().material.shader = Shader.Find("Standard");
		}

		// Do the same for all child game objects
		for (int i = 0; i < go.transform.childCount; i++)
		{
			FixShaders(go.transform.GetChild(i).gameObject);
		}
	}

	private static void AddCustomEnergy()
	{
		// Log.LogDebug($"Starting load of custom energy object");
		// var prefab = AllPrefabAssets.LoadAssetWithSubAssets("Hexalantern")[0];
		//
		// Log.LogDebug($"Creating custom energy object [{prefab}]");
		// GameObject energyObj = (GameObject)Object.Instantiate(
		// 	prefab,
		// 	new Vector3(-2.69f, 5.82f, -0.48f),
		// 	Quaternion.Euler(0, 0, 0f),
		// 	Object.FindObjectOfType<BoardManager3D>().transform
		// );

		// FixShaders(energyObj);
	}

	private static void AddCardRemoveSequencer()
	{
		// TODO: This will work, but it doesn't show the boon art correctly.

		if (SpecialNodeHandler.Instance is null || SpecialNodeHandler.Instance.cardRemoveSequencer is not null)
		{
			return;
		}

		Log.LogDebug($"[AddCardRemoveSequencer] Creating card remove sequencer");
		GameObject cardRemoveSequencerObj = Object.Instantiate(
			ResourceBank.Get<GameObject>("Prefabs/SpecialNodeSequences/CardRemoveSequencer"),
			SpecialNodeHandler.Instance.transform
		);
		cardRemoveSequencerObj.name = cardRemoveSequencerObj.name.Replace("(Clone)", "_Grimora");

		var oldRemoveSequencer = cardRemoveSequencerObj.GetComponent<CardRemoveSequencer>();

		Log.LogDebug($"[AddCardRemoveSequencer] Adding new card remove sequencer component");
		var cardRemoveSequencer = cardRemoveSequencerObj.AddComponent<GrimoraCardRemoveSequencer>();

		Log.LogDebug($"[AddCardRemoveSequencer] Setting prefabs");
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

		Log.LogDebug($"[AddCardRemoveSequencer] Setting card backs");
		cardRemoveSequencer.deckPile = oldRemoveSequencer.deckPile;
		cardRemoveSequencer.deckPile.cardbackPrefab = PrefabGrimoraCardBack;

		Log.LogDebug($"[AddCardRemoveSequencer] Destroying old sequencer");
		Object.Destroy(oldRemoveSequencer);


		// TODO: HOW DO WE GET THOSE DECALS TO SHOW UP
		CardDisplayer3D displayer3D = ResourceBank.Get<CardDisplayer3D>("Prefabs/Cards/CardElements");

		CardDisplayer3D graveDisplayer = Object.FindObjectOfType<CardDisplayer3D>();

		BoonIconInteractable cardAbilityIcons = Object.Instantiate(
			ResourceBank.Get<BoonIconInteractable>("Prefabs/Cards/CardSurfaceInteraction/BoonIcon"),
			graveDisplayer.GetComponentInChildren<CardAbilityIcons>().transform
		);
		graveDisplayer.GetComponentInChildren<CardAbilityIcons>().boonIcon = cardAbilityIcons;

		GameObject cardDecals = Object.Instantiate(
			displayer3D.transform.GetChild(9).gameObject,
			graveDisplayer.transform, true
		);

		graveDisplayer.decalRenderers.Clear();
		for (int i = 0; i < cardDecals.transform.childCount; i++)
		{
			graveDisplayer.decalRenderers.Add(cardDecals.transform.GetChild(i).GetComponent<MeshRenderer>());
		}

		SpecialNodeHandler.Instance.cardRemoveSequencer = cardRemoveSequencer;
		Log.LogDebug($"[AddCardRemoveSequencer] Finished adding AddCardRemoveSequencer");
	}

	internal static void AddHammer()
	{
		GrimoraItemsManager currentItemsManager = GrimoraItemsManager.Instance.GetComponent<GrimoraItemsManager>();

		GrimoraItemsManagerExt ext = GrimoraItemsManager.Instance.GetComponent<GrimoraItemsManagerExt>();

		if (ext is null)
		{
			Log.LogDebug($"[AddHammer] Creating hammer and GrimoraItemsManagerExt");

			ext = GrimoraItemsManager.Instance.gameObject.AddComponent<GrimoraItemsManagerExt>();
			ext.consumableSlots = currentItemsManager.consumableSlots;

			Part3ItemsManager part3ItemsManager = Object.Instantiate(
				ResourceBank.Get<Part3ItemsManager>("Prefabs/Items/ItemsManager_Part3")
			);

			part3ItemsManager.hammerSlot.transform.SetParent(ext.transform);

			ext.HammerSlot = part3ItemsManager.hammerSlot;

			float xVal = Harmony.HasAnyPatches("julianperge.inscryption.act1.increaseCardSlots") ? -8.75f : -7.5f;
			ext.HammerSlot.gameObject.transform.localPosition = new Vector3(xVal, 0.81f, -0.48f);
			ext.HammerSlot.gameObject.transform.rotation = Quaternion.Euler(270f, 315f, 0f);

			Log.LogDebug($"[AddHammer] Destroying old part3ItemsManager");
			Object.Destroy(part3ItemsManager);
		}

		if (GameObject.Find("ItemsManager_Part3(Clone)"))
		{
			Log.LogDebug($"[AddHammer] Destroying existing part3ItemsManager");
			Object.Destroy(GameObject.Find("ItemsManager_Part3(Clone)"));
		}

		Log.LogDebug($"[AddHammer] Finished adding hammer");
	}

	private static void AddEnergyDrone()
	{
		ResourceDrone resourceEnergy = Object.FindObjectOfType<ResourceDrone>();

		if (BoardManager3D.Instance is not null && resourceEnergy is null)
		{
			resourceEnergy = Object.Instantiate(
				ResourceBank.Get<ResourceDrone>("Prefabs/CardBattle/ResourceModules"),
				new Vector3(5.3f, 5.5f, 1.92f),
				Quaternion.Euler(270f, 0f, -146.804f),
				BoardManager3D.Instance.gameObject.transform
			);

			Color grimoraTextColor = new Color(0.420f, 1f, 0.63f);
			resourceEnergy.name = "Grimora Resource Modules";
			resourceEnergy.baseCellColor = grimoraTextColor;
			resourceEnergy.highlightedCellColor = new Color(1, 1, 0.23f);

			// Log.LogDebug($"[AddEnergyDrone] Disabling animation");
			Animator animator = resourceEnergy.GetComponentInChildren<Animator>();
			animator.enabled = false;

			Transform moduleEnergy = animator.transform.GetChild(0);
			// Log.LogDebug($"[AddEnergyDrone] Getting module energy and setting mesh to null");
			moduleEnergy.gameObject.GetComponent<MeshFilter>().mesh = null;

			int modulesChildren = moduleEnergy.childCount;
			for (int i = 1; i < 7; i++)
			{
				Transform energyCell = moduleEnergy.GetChild(i);
				// Log.LogDebug($"[AddEnergyDrone] Energy cell [{energyCell.name}]");
				energyCell.gameObject.GetComponent<MeshFilter>().mesh = null;
				var energyCellCase = energyCell.GetChild(0);
				energyCellCase.GetChild(0).GetComponent<MeshRenderer>().material.SetColor(EmissionColor, grimoraTextColor);
				energyCellCase.GetChild(1).GetComponent<MeshFilter>().mesh = null;
				energyCellCase.GetChild(2).GetComponent<MeshFilter>().mesh = null;
			}

			// Log.LogDebug($"[AddEnergyDrone] Setting Connector inactive");
			moduleEnergy.Find("Connector").gameObject.SetActive(false);
			// Log.LogDebug($"[AddEnergyDrone] Setting Propellers inactive");
			moduleEnergy.Find("Propellers").gameObject.SetActive(false);
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
		ChessboardMapExt ext = ChessboardMap.Instance.gameObject.GetComponent<ChessboardMapExt>();

		Log.LogDebug($"[ChangeChessboardToExtendedClass] Adding MapExt to ChessboardMapGameObject");

		if (ext is null)
		{
			Log.LogDebug($"[ChangeChessboardToExtendedClass] ChessboardMapExt is null");
			ChessboardMap boardComp = ChessboardMap.Instance.gameObject.GetComponent<ChessboardMap>();

			ext = ChessboardMap.Instance.gameObject.AddComponent<ChessboardMapExt>();

			Log.LogDebug($"[ChangeChessboardToExtendedClass] Transferring over fields to new extension class");
			ext.dynamicElementsParent = boardComp.dynamicElementsParent;
			ext.mapAnim = boardComp.mapAnim;
			ext.navGrid = boardComp.navGrid;
			ext.pieces = new List<ChessboardPiece>();
			ext.defaultPosition = boardComp.defaultPosition;

			Log.LogDebug($"[ChangeChessboardToExtendedClass] Destroying old chessboard component");
			Object.Destroy(boardComp);

			Log.LogDebug($"[ChangeChessboardToExtendedClass] Getting initial starting pieces");
			var initialStartingPieces = Object.FindObjectsOfType<ChessboardPiece>();
			// Log.LogDebug($"[ChangeChessboardToExtendedClass] Resetting initial pieces" +
			//              $" {string.Join(", ", initialStartingPieces.Select(_ => _.name))}");

			Log.LogDebug($"[ChangeChessboardToExtendedClass] Destroying initial pieces");
			foreach (var piece in initialStartingPieces)
			{
				ext.pieces.Remove(piece);
				piece.MapNode.OccupyingPiece = null;
				piece.gameObject.SetActive(false);
				Object.Destroy(piece.gameObject);
			}
		}

		Log.LogDebug($"[ChangeChessboardToExtendedClass] Finished adding ChessboardMapExt");
	}

	private static void AddDeckReviewSequencerToScene()
	{
		if (DeckReviewSequencer.Instance is not null)
		{
			// DeckReviewSequencer reviewSequencer = deckReviewSequencerObj.GetComponent<DeckReviewSequencer>();
			SelectableCardArray cardArray = DeckReviewSequencer.Instance.GetComponentInChildren<SelectableCardArray>();
			cardArray.selectableCardPrefab = PrefabGrimoraSelectableCard;
			Log.LogDebug($"[AddRareCardSequencerToScene] Creating new rare choice generator");
		}
	}

	private static void AddRareCardSequencerToScene()
	{
		// GrimoraPlugin.Log.LogDebug($"Creating RareCardChoiceSelector");

		if (SpecialNodeHandler.Instance is null || SpecialNodeHandler.Instance.rareCardChoiceSequencer is not null)
		{
			return;
		}

		Log.LogDebug($"[AddRareCardSequencerToScene] Creating new rare choice generator");
		GameObject rareCardChoicesSelector = Object.Instantiate(
			ResourceBank.Get<GameObject>("Prefabs/SpecialNodeSequences/RareCardChoiceSelector"),
			SpecialNodeHandler.Instance.transform
		);
		rareCardChoicesSelector.name = rareCardChoicesSelector.name.Replace("(Clone)", "_Grimora");

		RareCardChoicesSequencer sequencer = rareCardChoicesSelector.GetComponent<RareCardChoicesSequencer>();

		// GrimoraPlugin.Log.LogDebug($"-> Setting RareCardChoicesSequencer choice generator to Part1RareChoiceGenerator");
		sequencer.choiceGenerator = rareCardChoicesSelector.AddComponent<GrimoraRareChoiceGenerator>();

		// GrimoraPlugin.Log.LogDebug($"-> Setting RareCardChoicesSequencer selectableCardPrefab to SelectableCard_Grimora");
		sequencer.selectableCardPrefab = PrefabGrimoraSelectableCard;

		// GrimoraPlugin.Log.LogDebug($"-> Setting SpecialNodeHandler rareCardChoiceSequencer to sequencer");
		SpecialNodeHandler.Instance.rareCardChoiceSequencer = sequencer;
		Log.LogDebug($"[AddRareCardSequencerToScene] Finished adding GrimoraRareChoiceGenerator");
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
		if (!SaveManager.SaveFile.IsGrimora || gameState is not GameState.Map)
		{
			yield return enumerator;
			yield break;
		}

		Log.LogDebug(
			$"[GameFlowManager.TransitionTo] Instance {__instance} " +
			$"Current state is [{__instance.CurrentGameState}] " +
			$"GameState [{gameState}]"
		);

		if(ChessboardMapExt.Instance is null)
		{
			// This is required because Unity takes a second to update
			while (ChessboardMapExt.Instance is null)
			{
				Log.LogDebug($"[TransitionTo] Waiting until MapExt is no longer null");
				yield return new WaitForSeconds(0.25f);
			}

			// we just want to run this once, not each time the transition happens
			Log.LogDebug($"[TransitionTo] Calling SetAnimActiveIfInactive()");
			ChessboardMapExt.Instance.SetAnimActiveIfInactive();
		}

		Log.LogDebug($"[TransitionTo] Getting bosses defeated");
		bool isBossDefeated = ChessboardMapExt.Instance.BossDefeated;
		Log.LogDebug($"[TransitionTo] Getting existing pieces");
		bool piecesExist = !ChessboardMapExt.Instance.pieces.IsNullOrEmpty();

		Log.LogDebug($"[TransitionTo] IsBossDefeated [{isBossDefeated}] Pieces exist [{piecesExist}]");

		// FOR ENUMS IN POSTFIX CALLS, THE OPERATOR TO USE IS 'IS' NOT '==' 
		// CORRECT  : gameState is GameState.Map
		// INCORRECT: gameState == GameState.Map
		if (piecesExist && isBossDefeated)
		{
			yield return ChessboardMapExt.Instance.CompleteRegionSequence();

			__instance.CurrentGameState = gameState;
		}
		else
		{
			Log.LogDebug($"[TransitionTo] Returning enumerator since pieces dont exist and/or boss has not been defeated");
			yield return enumerator;
		}
	}
}
