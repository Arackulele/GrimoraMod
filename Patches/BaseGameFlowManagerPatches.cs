using System.Collections;
using DiskCardGame;
using GrimoraMod.Saving;
using HarmonyLib;
using Sirenix.Utilities;
using UnityEngine;
using static GrimoraMod.GrimoraPlugin;
using static GrimoraMod.GrimoraPlugin;

namespace GrimoraMod;

[HarmonyPatch(typeof(GameFlowManager))]
public class BaseGameFlowManagerPatches
{
	[HarmonyPrefix, HarmonyPatch(nameof(GameFlowManager.Start))]
	public static void PrefixStart(GameFlowManager __instance)
	{
		if (GrimoraSaveUtil.IsNotGrimoraModRun)
		{
			return;
		}
		
		Log.LogDebug($"[GameFlowManager] Instance is [{__instance.GetType()}] GameMap.Instance [{GameMap.Instance}]");

		if (!AllSounds.Any(clip => AudioController.Instance.Loops.Contains(clip)))
		{
			AudioController.Instance.Loops.AddRange(AllSounds);
		}

		GameObject rightWrist = GameObject.Find("Grimora_RightWrist");
		if (rightWrist && rightWrist.transform.GetChild(6))
		{
			UnityObject.Destroy(rightWrist.transform.GetChild(6).gameObject);
		}

		if (ResourceBank.instance == null)
		{
			__instance.gameObject.AddComponent<ResourceBank>();
		}

		DisableAttackAndHealthStatShadowsAndScaleUpStatIcons();

		if (UnityObject.FindObjectOfType<CombatBell3D>())
		{
			CombatBell3D bell = UnityObject.FindObjectOfType<CombatBell3D>();
			SphereCollider collider = bell.GetComponent<SphereCollider>();
			Vector3 currentCenter = collider.center;
			collider.center = new Vector3(currentCenter.x, 0.75f, 0);
			collider.radius = 0.5f;
		}

		SetupPlayableAndSelectableCardPrefabs();

		ChessboardMapExt.ChangeChessboardToExtendedClass();

		BoneyardBurialSequencer.CreateSequencerInScene();

		ElectricChairSequencer.CreateSequencerInScene();

		GrimoraCardRemoveSequencer.CreateSequencerInScene();

		GrimoraGainConsumableSequencer.CreateSequencerInScene();

		GrimoraItemsManagerExt.CreateHammerSlot();

		AddDeckReviewSequencerToScene();

		AddEnergyDrone();

		AddRareCardSequencerToScene();

		AddCardSelectorObjectForTutor();

		// AddBoonLordBoonConsumable();

		// AddCustomEnergy();

		CryptHelper.SetupNewCryptAndZones();

		GrimoraAnimationController.Instance.transform.SetParent(UnityObject.FindObjectOfType<InputManagerSpawner>().transform);

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
	}

	private static void AddCardSelectorObjectForTutor()
	{
		if (BoardManager.Instance && BoardManager.Instance.cardSelector.SafeIsUnityNull())
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
	}

	private static void AddEnergyDrone()
	{

		ResourceDrone resourceEnergy = ResourceDrone.Instance;

		if (BoardManager3D.Instance && resourceEnergy.SafeIsUnityNull())
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


		//Ghost Bottle Management
		bool Soul = false;
		if (Soul == true) { 
		GameObject SoulCounter = new GameObject("SoulCounter");
		SoulCounter.transform.parent = resourceEnergy.transform;

		GameObject Half1 = new GameObject("SoulCounter");
		Half1.transform.position = new Vector3(0.9618f, 0, 0);
		Half1.transform.parent = SoulCounter.transform;

		GameObject Half2 = new GameObject("SoulCounter");
		Half2.transform.parent = SoulCounter.transform;

		GameObject Bottle1 = GameObject.Instantiate(AssetConstants.GhostBottle);
		GameObject Bottle2 = GameObject.Instantiate(AssetConstants.GhostBottle);
		GameObject Bottle3 = GameObject.Instantiate(AssetConstants.GhostBottle);
		GameObject Bottle4 = GameObject.Instantiate(AssetConstants.GhostBottle);
		GameObject Bottle5 = GameObject.Instantiate(AssetConstants.GhostBottle);
		GameObject Bottle6 = GameObject.Instantiate(AssetConstants.GhostBottle);

		Bottle1.transform.position = new Vector3(2.9582f, 5.9637f, 4.8164f);
		Bottle1.transform.rotation = Quaternion.Euler(0f, 192.4423f, 0f);
		Bottle1.transform.parent = Half1.transform;

		Bottle2.transform.position = new Vector3(4.251f, 5.9455f, 4.8164f);
		Bottle2.transform.rotation = Quaternion.Euler(0f, 192.4423f, 0f);
		Bottle2.transform.parent = Half1.transform;

		Bottle3.transform.position = new Vector3(5.5619f, 5.9455f, 4.8164f);
		Bottle3.transform.rotation = Quaternion.Euler(0f, 192.4423f, 0f);
		Bottle3.transform.parent = Half1.transform;

		Bottle4.transform.position = new Vector3(3.1422f, 7.8347f, 6.6033f);
		Bottle4.transform.rotation = Quaternion.Euler(0f, 192.4423f, 0f);
		Bottle4.transform.parent = Half2.transform;

		Bottle5.transform.position = new Vector3(4.5004f, 7.8347f, 6.6033f);
		Bottle5.transform.rotation = Quaternion.Euler(0f, 192.4423f, 0f);
		Bottle5.transform.parent = Half2.transform;

		Bottle6.transform.position = new Vector3(5.8022f, 7.8347f, 6.6033f);
		Bottle6.transform.rotation = Quaternion.Euler(0f, 192.4423f, 0f);
		Bottle6.transform.parent = Half2.transform;

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
		if (SpecialNodeHandler.Instance.SafeIsUnityNull())
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
		sequencer.rareChoiceGenerator = rareCardChoicesSelector.AddComponent<GrimoraRareChoiceGenerator>();
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
		if (GrimoraSaveUtil.IsNotGrimoraModRun || gameState != GameState.Map)
		{
			// run the original code
			yield return enumerator;
			yield break;
		}

		if (ChessboardMapExt.Instance.SafeIsUnityNull())
		{
			// This is required because Unity takes a second to update
			while (ChessboardMapExt.Instance.SafeIsUnityNull())
			{
				yield return new WaitForSeconds(0.25f);
			}

			// we just want to run this once, not each time the transition happens
			ChessboardMapExt.Instance.SetAnimActiveIfInactive();
		}

		bool isBossDefeated = GrimoraRunState.CurrentRun.PiecesRemovedFromBoard.Exists(piece => piece.Contains("BossPiece"));
		bool piecesExist = ChessboardMapExt.Instance.pieces.IsNotEmpty();

		Log.LogInfo($"[TransitionTo] IsBossDefeated [{isBossDefeated}] Pieces exist [{piecesExist}]");
		Log.LogInfo($"[TransitionTo] IsBossDefeated [{triggeringNodeData}]");
		if (triggeringNodeData != null)
		{
			Log.LogInfo($"[TransitionTo] IsBossDefeated [{triggeringNodeData.id}]");
			Log.LogInfo($"[TransitionTo] IsBossDefeated [{triggeringNodeData.prefabPath}]");
			Log.LogInfo($"[TransitionTo] IsBossDefeated [{triggeringNodeData.GetType()}]");
			Log.LogInfo($"[TransitionTo] IsBossDefeated [{triggeringNodeData.position}]");
		}

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
