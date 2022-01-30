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
	private static readonly GameObject PrefabGrimoraSelectableCard =
		ResourceBank.Get<GameObject>("Prefabs/Cards/SelectableCard_Grimora");

	private static readonly GameObject PrefabGrimoraPlayableCard =
		ResourceBank.Get<GameObject>("Prefabs/Cards/PlayableCard_Grimora");

	private static readonly GameObject PrefabGrimoraCardBack =
		ResourceBank.Get<GameObject>("Prefabs/Cards/CardBack_Grimora");

	private static readonly int EmissionColor = Shader.PropertyToID("_EmissionColor");

	private static GameObject ChessboardGameMap => GameObject.Find("ChessboardGameMap");
	
	private static SpecialNodeHandler SpecialNodeHandler => Object.FindObjectOfType<SpecialNodeHandler>();

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
		// GameObject giantPrefab = ResourceBank.Get<GameObject>("Prefabs/Cards/PlayableCard_Grimora");
		// giantPrefab.name += "_Giant";
		//
		// giantPrefab.GetComponent<Animator>().runtimeAnimatorController 
		// 	= ResourceBank.Get<GameObject>("Prefabs/Cards/PlayableCard_Giant")
		// 		.GetComponent<Animator>().runtimeAnimatorController;

		// giantPrefab.transform.GetChild(0).GetChild(0).GetChild(0).localPosition = new Vector3(-0.65f, 1.25f, 0f);
		// giantPrefab.transform.GetChild(0).GetChild(0).GetChild(0).localScale = new Vector3(1f, 1.175f, 0.2f);
		CardSpawner.Instance.giantPlayableCardPrefab = PrefabGrimoraPlayableCard;

		// Log.LogDebug($"[GameFlowManager.Start] Instance is [{__instance.GetType()}]" +
		//              $" Board Already exists? [{boardObj is not null}]");
		if (SaveManager.SaveFile.IsGrimora && ChessboardGameMap is not null)
		{
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
	}

	public static void AddBoonLordBoonConsumable()
	{
		Log.LogDebug($"Adding Boon Lord Consumable");
		GameObject ramSkull = UnityEngine.Object.Instantiate(
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

	internal static void AddHammer()
	{

		GameObject managerObj = Object.FindObjectOfType<GrimoraItemsManager>().gameObject;
		GrimoraItemsManager currentItemsManager = managerObj.GetComponent<GrimoraItemsManager>();
		
		GrimoraItemsManagerExt ext = managerObj.GetComponent<GrimoraItemsManagerExt>();

		if (ext is null)
		{
			Log.LogDebug($"Creating hammer and GrimoraItemsManagerExt");
			
			ext = managerObj.AddComponent<GrimoraItemsManagerExt>();
			ext.consumableSlots = currentItemsManager.consumableSlots;

			Part3ItemsManager part3ItemsManager = Object.Instantiate(
				ResourceBank.Get<Part3ItemsManager>("Prefabs/Items/ItemsManager_Part3")
			);

			part3ItemsManager.hammerSlot.transform.SetParent(ext.transform);
			
			ext.HammerSlot = part3ItemsManager.hammerSlot;
			
			float xVal = Harmony.HasAnyPatches("julianperge.inscryption.act1.increaseCardSlots") ? -8.75f : -7.5f;
			ext.HammerSlot.gameObject.transform.localPosition = new Vector3(xVal, 0.81f, -0.48f);
			ext.HammerSlot.gameObject.transform.rotation = Quaternion.Euler(270f, 315f, 0f);
			
			Log.LogDebug($"Destroying old part3ItemsManager");
			Object.Destroy(part3ItemsManager);
		}

		if (GameObject.Find("ItemsManager_Part3(Clone)"))
		{
			Log.LogDebug($"Destroying existing part3ItemsManager");
			Object.Destroy(GameObject.Find("ItemsManager_Part3(Clone)"));
		}
	}

	private static void AddEnergyDrone()
	{
		BoardManager3D boardManager = Object.FindObjectOfType<BoardManager3D>();
		ResourceDrone resourceEnergy = Object.FindObjectOfType<ResourceDrone>();

		if (boardManager is not null && resourceEnergy is null)
		{
			resourceEnergy = Object.Instantiate(
				ResourceBank.Get<ResourceDrone>("Prefabs/CardBattle/ResourceModules"),
				new Vector3(5.3f, 5.5f, 1.92f),
				Quaternion.Euler(270f, 0f, -146.804f),
				boardManager.transform
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
		// Log.LogDebug($"[ChangeChessboardToExtendedClass] Adding MapExt to ChessboardMapGameObject");
		GameObject boardObj = GameObject.Find("ChessboardGameMap");
		ChessboardMapExt ext = boardObj.gameObject.GetComponent<ChessboardMapExt>();

		if (ext is null)
		{
			ChessboardMap boardComp = boardObj.GetComponent<ChessboardMap>();

			ext = boardObj.gameObject.AddComponent<ChessboardMapExt>();

			ext.dynamicElementsParent = boardComp.dynamicElementsParent;
			ext.mapAnim = boardComp.mapAnim;
			ext.navGrid = boardComp.navGrid;
			ext.pieces = new List<ChessboardPiece>();
			ext.defaultPosition = boardComp.defaultPosition;

			// GrimoraPlugin.Log.LogDebug($"Destroying old chessboard component");
			Object.Destroy(boardComp);

			var initialStartingPieces = Object.FindObjectsOfType<ChessboardPiece>();
			// Log.LogDebug($"[ChangeChessboardToExtendedClass] Resetting initial pieces" +
			//              $" {string.Join(", ", initialStartingPieces.Select(_ => _.name))}");

			foreach (var piece in initialStartingPieces)
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

		if (specialNodeHandler is not null)
		{
			if (specialNodeHandler.rareCardChoiceSequencer is null)
			{
				GameObject rareCardChoicesSelector = Object.Instantiate(
					ResourceBank.Get<GameObject>("Prefabs/SpecialNodeSequences/RareCardChoiceSelector"),
					specialNodeHandler.transform
				);

				RareCardChoicesSequencer sequencer = rareCardChoicesSelector.GetComponent<RareCardChoicesSequencer>();

				// GrimoraPlugin.Log.LogDebug($"-> Setting RareCardChoicesSequencer choice generator to Part1RareChoiceGenerator");
				sequencer.choiceGenerator = rareCardChoicesSelector.AddComponent<GrimoraRareChoiceGenerator>();

				// GrimoraPlugin.Log.LogDebug($"-> Setting RareCardChoicesSequencer selectableCardPrefab to SelectableCard_Grimora");
				sequencer.selectableCardPrefab = PrefabGrimoraSelectableCard;

				// GrimoraPlugin.Log.LogDebug($"-> Setting SpecialNodeHandler rareCardChoiceSequencer to sequencer");
				specialNodeHandler.rareCardChoiceSequencer = sequencer;
			}
		}
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
		Log.LogDebug($"[GameFlowManager.TransitionTo] Current state is [{__instance.CurrentGameState}]");

		if (SaveManager.SaveFile.IsGrimora)
		{
			Log.LogDebug($"[TransitionTo] Getting bosses defeated");
			bool isBossDefeated = ChessboardMapExt.Instance.BossDefeated;
			Log.LogDebug($"[TransitionTo] Getting existing pieces");
			bool piecesExist = !ChessboardMapExt.Instance.pieces.IsNullOrEmpty();

			Log.LogDebug($"[TransitionTo] IsBossDefeated [{isBossDefeated}] Pieces exist [{piecesExist}]");

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
