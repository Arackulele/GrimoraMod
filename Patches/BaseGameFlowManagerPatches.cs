using System.Collections;
using DiskCardGame;
using GrimoraMod.Consumables;
using HarmonyLib;
using InscryptionAPI.Card;
using InscryptionAPI.Helpers;
using Sirenix.Utilities;
using UnityEngine;
using static GrimoraMod.GrimoraPlugin;

namespace GrimoraMod;

[HarmonyPatch(typeof(GameFlowManager))]
public class BaseGameFlowManagerPatches
{
	[HarmonyPrefix, HarmonyPatch(nameof(GameFlowManager.Start))]
	public static void PrefixStart(GameFlowManager __instance)
	{
		if (GrimoraSaveUtil.isNotGrimora)
		{
			return;
		}
		
		Log.LogDebug($"[GameFlowManager] Instance is [{__instance.GetType()}] GameMap.Instance [{GameMap.Instance}]");

		AbilitiesUtil.allData.Single(abInfo => abInfo.ability == Ability.LatchBrittle)
		 .SetIcon(AssetUtils.GetPrefab<Texture2D>("ability_LatchBrittle"));
		
		AbilitiesUtil.allData.Single(abInfo => abInfo.ability == Ability.LatchDeathShield)
		 .SetIcon(AssetUtils.GetPrefab<Texture2D>("ability_LatchShield"));

		if (!AllSounds.Any(clip => AudioController.Instance.Loops.Contains(clip)))
		{
			AudioController.Instance.Loops.AddRange(AllSounds);
		}

		GameObject rightWrist = GameObject.Find("Grimora_RightWrist");
		if (rightWrist && rightWrist.transform.GetChild(6))
		{
			UnityObject.Destroy(rightWrist.transform.GetChild(6).gameObject);
		}

		DisableAttackAndHealthStatShadowsAndScaleUpStatIcons();

		SetupPlayableAndSelectableCardPrefabs();

		ChessboardMapExt.ChangeChessboardToExtendedClass();

		BoneyardBurialSequencer.CreateSequencerInScene();

		ElectricChairSequencer.CreateSequencerInScene();

		GrimoraCardRemoveSequencer.CreateSequencerInScene();

		GrimoraItemsManagerExt.AddHammer();

		AddDeckReviewSequencerToScene();

		AddEnergyDrone();

		AddRareCardSequencerToScene();

		ChangeStartDeckIfNotAlreadyChanged();

		AddCardSelectorObjectForTutor();

		// AddBoonLordBoonConsumable();

		// AddCustomEnergy();

		CryptHelper.SetupNewCryptAndZones();

		GrimoraAnimationController.Instance.transform.SetParent(UnityObject.FindObjectOfType<InputManagerSpawner>().transform);

		Log.LogDebug($"Assigning controller to game table");
		GameObject.Find("GameTable")
			.AddComponent<Animator>()
			.runtimeAnimatorController = AssetUtils.GetPrefab<RuntimeAnimatorController>("GrimoraGameTable");
	}


	private static void SetupPlayableAndSelectableCardPrefabs()
	{
		AssetConstants.GrimoraPlayableCard
			.transform
			.Find("SkeletonAttackAnim")
			.GetComponent<Animator>()
			.runtimeAnimatorController = AssetConstants.SkeletonArmController;

		AssetConstants.GrimoraPlayableCard
			.GetComponent<GravestoneCardAnimationController>()
			.Anim
			.runtimeAnimatorController = AssetConstants.GraveStoneController;

		AssetConstants.GrimoraSelectableCard
			.GetComponent<GravestoneCardAnimationController>()
			.Anim
			.runtimeAnimatorController = AssetConstants.GraveStoneController;

		CardSpawner.Instance.giantPlayableCardPrefab = AssetConstants.GrimoraPlayableCard;

		Vector3 boxColliderSize = new Vector3(0.4f, 0.4f, 0.1f);
		var cardAbilityIcons = AssetConstants.GrimoraPlayableCard.GetComponentInChildren<CardAbilityIcons>();
		foreach (var group in cardAbilityIcons.defaultIconGroups)
		{
			var childBoxColliders = group.GetComponentsInChildren<BoxCollider>();
			foreach (var collider in childBoxColliders)
			{
				collider.extents = boxColliderSize;
			}
		}

		cardAbilityIcons = AssetConstants.GrimoraSelectableCard.GetComponentInChildren<CardAbilityIcons>();
		foreach (var group in cardAbilityIcons.defaultIconGroups)
		{
			var childBoxColliders = group.GetComponentsInChildren<BoxCollider>();
			foreach (var collider in childBoxColliders)
			{
				collider.extents = boxColliderSize;
			}
		}
	}

	private static void AddCardSelectorObjectForTutor()
	{
		if (BoardManager.Instance && BoardManager.Instance.cardSelector.IsNull())
		{
			SelectableCardArray boardCardSelection
				= new GameObject("BoardCardSelection").AddComponent<SelectableCardArray>();
			boardCardSelection.arrayWidth = 5;
			boardCardSelection.cardsTilt = 0;
			boardCardSelection.leftAnchor = -2.5f;
			boardCardSelection.selectableCardPrefab = AssetConstants.GrimoraSelectableCard;

			boardCardSelection.transform.SetParent(BoardManager.Instance.transform);
			boardCardSelection.transform.position = new Vector3(0.81f, 5.01f, -3.45f);

			BoardManager.Instance.cardSelector = boardCardSelection;
		}
	}

	private static void DisableAttackAndHealthStatShadowsAndScaleUpStatIcons()
	{
		GravestoneCardDisplayer displayer = UnityObject.FindObjectOfType<GravestoneCardDisplayer>();
		var statsParent = displayer.transform.Find("Stats");
		statsParent.Find("Attack_Shadow").gameObject.SetActive(false);
		statsParent.Find("Health_Shadow").gameObject.SetActive(false);

		CardStatIcons statIcons = displayer.StatIcons;
		statIcons.attackIconRenderer.transform.localPosition = new Vector3(-0.39f, 0.19f, 0);
		statIcons.attackIconRenderer.transform.localScale = new Vector3(0.33f, 0.33f, 1);

		statIcons.healthIconRenderer.transform.localPosition = new Vector3(-0.39f, 0.19f, 0);
		statIcons.healthIconRenderer.transform.localScale = new Vector3(0.33f, 0.33f, 1);

		CardAbilityIcons cardAbilityIcons = displayer.AbilityIcons;
		Vector3 boxColliderSize = new Vector3(0.4f, 0.4f, 0.1f);
		foreach (var group in cardAbilityIcons.defaultIconGroups)
		{
			var childBoxColliders = group.GetComponentsInChildren<BoxCollider>();
			foreach (var collider in childBoxColliders)
			{
				collider.extents = boxColliderSize;
			}
		}
	}

	public static void AddBoonLordBoonConsumable()
	{
		Log.LogDebug($"Adding Boon Lord Consumable");
		GameObject ramSkull = UnityObject.Instantiate(
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
		// itemData.rulebookSprite = Sprite.Create(Rect.zero, Vector2.zero, float.Epsilon);
		itemData.regionSpecific = false;

		if (!ItemsUtil.allData.Exists(x => ((ConsumableItemData)x).rulebookName == itemData.rulebookName))
		{
			Log.LogDebug($"Adding consumable in ItemsUtil.allData");
			ItemsUtil.allData.Add(itemData);
		}

		// Log.LogDebug($"Updating items");
		// GrimoraItemsManagerExt.Instance.UpdateItems();
	}

	private static void AddCustomEnergy()
	{
		// Log.LogDebug($"Starting load of custom energy object");
		// var prefab = AllPrefabAssets.LoadAssetWithSubAssets("Hexalantern")[0];
		//
		// Log.LogDebug($"Creating custom energy object [{prefab}]");
		// GameObject energyObj = (GameObject)UnityObject.Instantiate(
		// 	prefab,
		// 	new Vector3(-2.69f, 5.82f, -0.48f),
		// 	Quaternion.Euler(0, 0, 0f),
		// 	Object.FindObjectOfType<BoardManager3D>().transform
		// );

		// FixShaders(energyObj);
	}

	private static void AddEnergyDrone()
	{
		ResourceDrone resourceEnergy = ResourceDrone.Instance;

		if (BoardManager3D.Instance && resourceEnergy.IsNull())
		{
			resourceEnergy = UnityObject.Instantiate(
				ResourceBank.Get<ResourceDrone>("Prefabs/CardBattle/ResourceModules"),
				new Vector3(5.3f, 5.5f, 1.92f),
				Quaternion.Euler(270f, 0f, -146.804f),
				BoardManager3D.Instance.gameObject.transform
			);
		}

		resourceEnergy.name = "Grimora Resource Modules";
		resourceEnergy.baseCellColor = GrimoraColors.GrimoraText;
		resourceEnergy.highlightedCellColor = GrimoraColors.ResourceEnergyCell;

		Animator animator = resourceEnergy.GetComponentInChildren<Animator>();
		Transform moduleEnergy = animator.transform.GetChild(0);
		moduleEnergy.gameObject.GetComponent<MeshFilter>().mesh = null;

		for (int i = 1; i < 7; i++)
		{
			Transform energyCell = moduleEnergy.GetChild(i);
			energyCell.gameObject.GetComponent<MeshFilter>().mesh = null;
			var energyCellCase = energyCell.GetChild(0);
			energyCellCase.GetChild(0).GetComponent<MeshRenderer>().material.SetEmissionColor(resourceEnergy.baseCellColor);
			energyCellCase.GetChild(1).GetComponent<MeshFilter>().mesh = null;
			energyCellCase.GetChild(2).GetComponent<MeshFilter>().mesh = null;
		}

		UnityObject.Destroy(moduleEnergy.Find("Connector").gameObject);
		resourceEnergy.emissiveRenderers.Clear();
	}

	private static void ChangeStartDeckIfNotAlreadyChanged()
	{
		if (GrimoraSaveUtil.DeckInfo.CardInfos.IsNullOrEmpty())
		{
			GrimoraSaveData.Data.Initialize();
		}
		else
		{
			List<CardInfo> grimoraDeck = GrimoraSaveUtil.DeckList;
			int graveDiggerCount = grimoraDeck.Count(info => info.name == "Gravedigger");
			int frankNSteinCount = grimoraDeck.Count(info => info.name == "FrankNStein");
			if (grimoraDeck.Count == 5 && graveDiggerCount == 3 && frankNSteinCount == 2)
			{
				Log.LogWarning($"[ChangeStartDeckIfNotAlreadyChanged] Starter deck needs reset");
				GrimoraSaveData.Data.Initialize();
			}
		}
	}

	private static void AddDeckReviewSequencerToScene()
	{
		if (DeckReviewSequencer.Instance)
		{
			// DeckReviewSequencer reviewSequencer = deckReviewSequencerObj.GetComponent<DeckReviewSequencer>();
			SelectableCardArray cardArray = DeckReviewSequencer.Instance.GetComponentInChildren<SelectableCardArray>();
			cardArray.selectableCardPrefab = AssetConstants.GrimoraSelectableCard;
		}
	}

	private static void AddRareCardSequencerToScene()
	{
		if (SpecialNodeHandler.Instance.IsNull())
		{
			return;
		}

		GameObject rareCardChoicesSelector = UnityObject.Instantiate(
			ResourceBank.Get<GameObject>("Prefabs/SpecialNodeSequences/RareCardChoiceSelector"),
			SpecialNodeHandler.Instance.transform
		);
		rareCardChoicesSelector.name = rareCardChoicesSelector.name.Replace("(Clone)", "_Grimora");

		RareCardChoicesSequencer sequencer = rareCardChoicesSelector.GetComponent<RareCardChoicesSequencer>();
		sequencer.deckPile.cardbackPrefab = AssetConstants.GrimoraCardBack;
		sequencer.choiceGenerator = rareCardChoicesSelector.AddComponent<GrimoraRareChoiceGenerator>();
		sequencer.selectableCardPrefab = AssetConstants.GrimoraSelectableCard;

		SpecialNodeHandler.Instance.rareCardChoiceSequencer = sequencer;
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
		if (GrimoraSaveUtil.isNotGrimora || gameState is not GameState.Map)
		{
			// run the original code
			yield return enumerator;
			yield break;
		}

		if (ChessboardMapExt.Instance.IsNull())
		{
			// This is required because Unity takes a second to update
			while (ChessboardMapExt.Instance.IsNull())
			{
				yield return new WaitForSeconds(0.25f);
			}

			// we just want to run this once, not each time the transition happens
			ChessboardMapExt.Instance.SetAnimActiveIfInactive();
		}

		bool isBossDefeated = ChessboardMapExt.Instance.BossDefeated;
		bool piecesExist = ChessboardMapExt.Instance.pieces.IsNotEmpty();

		Log.LogDebug($"[TransitionTo] IsBossDefeated [{isBossDefeated}] Pieces exist [{piecesExist}]");

		// FOR ENUMS IN POSTFIX CALLS, 'IS', 'IS NOT' is the same as '==' and '!=' respectively, despite what the IDE says
		if (piecesExist && isBossDefeated)
		{
			yield return ChessboardMapExt.Instance.CompleteRegionSequence();

			__instance.CurrentGameState = gameState;
		}
		else
		{
			Log.LogDebug($"[TransitionTo] Running original code as pieces dont exist and/or boss has not been defeated");
			yield return enumerator;
		}
	}
}
