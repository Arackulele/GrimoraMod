﻿using System.Collections;
using DiskCardGame;
using UnityEngine;
using static GrimoraMod.GrimoraPlugin;

namespace GrimoraMod;

public class BoneyardBurialSequencer : CardStatBoostSequencer
{
	private void Start()
	{
		base.figurines.Add(CreateGrave());
		base.figurines.AddRange(CreateTombstones());
		SetMaterials();
	}

	private void SetMaterials()
	{
		Material statBoostBoneyard = AllMats.Single(_ => _.name.Equals("StatBoostBoneyard"));
		selectionSlot.specificRenderers[0].material = statBoostBoneyard;
		selectionSlot.specificRenderers[0].sharedMaterial = statBoostBoneyard;
		selectionSlot.transform.localPosition = new Vector3(0, 5.13f, 0.51f);
		selectionSlot.transform.localScale = new Vector3(1.2f, 1.2f, 1.2f);
		selectionSlot.transform.localRotation = Quaternion.Euler(0, 0, 0);
		// selectionSlot.transform.GetChild(0).localRotation = Quaternion.Euler(90, 0, 0);

		var stoneQuad = confirmStone.transform.Find("Quad").GetComponent<MeshRenderer>();
		stoneQuad.material = AllMats.Single(_ => _.name.Equals("BoneyardBurialShovel"));
		stoneQuad.sharedMaterial = AllMats.Single(_ => _.name.Equals("BoneyardBurialShovel"));
		base.selectionSlot.defaultColor = new Color(0.420f, 1f, 0.63f);
		base.selectionSlot.baseColor = new Color(0.420f, 1f, 0.63f);
		stoneQuad.transform.localScale = new Vector3(1.75f, 1.75f, 1.75f);
		stoneQuad.transform.localRotation = Quaternion.Euler(90, 0, 0);
	}

	private IEnumerator InitialSetup()
	{
		stakeRingParent.SetActive(false);
		campfireLight.gameObject.SetActive(false);
		campfireLight.intensity = 0f;
		campfireCardLight.intensity = 0f;
		selectionSlot.Disable();
		selectionSlot.gameObject.SetActive(false);
		yield return new WaitForSeconds(0.3f);

		ExplorableAreaManager.Instance.HangingLight.gameObject.SetActive(false);
		ExplorableAreaManager.Instance.HandLight.gameObject.SetActive(false);
		ViewManager.Instance.SwitchToView(View.Default, false, true);
		ViewManager.Instance.OffsetPosition(new Vector3(0f, 0f, 2.25f), 0.1f);
		yield return new WaitForSeconds(1f);

		figurines.ForEach(delegate(CompositeFigurine x) { x.gameObject.SetActive(value: true); });

		stakeRingParent.SetActive(true);
		ExplorableAreaManager.Instance.HandLight.gameObject.SetActive(true);
		campfireLight.gameObject.SetActive(true);
		selectionSlot.gameObject.SetActive(true);
		selectionSlot.RevealAndEnable();
		selectionSlot.ClearDelegates();

		SelectCardFromDeckSlot selectCardFromDeckSlot = selectionSlot;
		selectCardFromDeckSlot.CursorSelectStarted =
			(Action<MainInputInteractable>)Delegate.Combine(
				selectCardFromDeckSlot.CursorSelectStarted,
				new Action<MainInputInteractable>(OnSlotSelected)
			);
		if (UnityEngine.Random.value < 0.25f && VideoCameraRig.Instance != null)
		{
			VideoCameraRig.Instance.PlayCameraAnim("refocus_quick");
		}

		AudioController.Instance.PlaySound3D(
			"campfire_light",
			MixerGroup.TableObjectsSFX,
			selectionSlot.transform.position
		);
		AudioController.Instance.SetLoopAndPlay("campfire_loop", 1);
		AudioController.Instance.SetLoopVolumeImmediate(0f, 1);
		AudioController.Instance.FadeInLoop(0.5f, 0.75f, 1);
		InteractionCursor.Instance.SetEnabled(false);
		yield return new WaitForSeconds(0.25f);

		yield return pile.SpawnCards(GrimoraSaveUtil.DeckList.Count, 0.5f);
		TableRuleBook.Instance.SetOnBoard(true);
		InteractionCursor.Instance.SetEnabled(true);
	}

	public IEnumerator BurialSequence()
	{
		yield return InitialSetup();

		yield return TextDisplayer.Instance.PlayDialogueEvent("StatBoostIntro", TextDisplayer.MessageAdvanceMode.Input);

		yield return confirmStone.WaitUntilConfirmation();
		bool finishedBuffing = false;
		while (!finishedBuffing)
		{
			selectionSlot.Disable();
			RuleBookController.Instance.SetShown(false);
			yield return new WaitForSeconds(0.25f);
			AudioController.Instance.PlaySound3D(
				"card_blessing",
				MixerGroup.TableObjectsSFX,
				selectionSlot.transform.position
			);
			selectionSlot.Card.Anim.StrongNegationEffect();
			selectionSlot.Card.Anim.StrongNegationEffect();
			selectionSlot.Card.Anim.StrongNegationEffect();
			ApplyModToCard(selectionSlot.Card.Info);
			// selectionSlot.Card.RenderCard();
			yield return new WaitForSeconds(0.15f);
			selectionSlot.Card.SetInfo(selectionSlot.Card.Info);
			selectionSlot.Card.SetInteractionEnabled(false);
			yield return new WaitForSeconds(0.75f);
			finishedBuffing = true;
		}

		yield return TextDisplayer.Instance.PlayDialogueEvent("StatBoostOutro", TextDisplayer.MessageAdvanceMode.Input);

		yield return OutroEnvTeardown();

		if (GameFlowManager.Instance != null)
		{
			GameFlowManager.Instance.TransitionToGameState(GameState.Map);
		}
	}

	private IEnumerator OutroEnvTeardown()
	{
		yield return new WaitForSeconds(0.1f);
		selectionSlot.FlyOffCard();

		ViewManager.Instance.SwitchToView(View.Default);
		yield return new WaitForSeconds(0.25f);

		AudioController.Instance.PlaySound3D(
			"campfire_putout",
			MixerGroup.TableObjectsSFX,
			selectionSlot.transform.position
		);
		AudioController.Instance.StopLoop(1);
		campfireLight.gameObject.SetActive(false);
		ExplorableAreaManager.Instance.HandLight.gameObject.SetActive(false);
		yield return pile.DestroyCards();
		yield return new WaitForSeconds(0.2f);

		figurines.ForEach(delegate(CompositeFigurine x) { x.gameObject.SetActive(value: false); });

		stakeRingParent.SetActive(false);
		confirmStone.SetStoneInactive();
		selectionSlot.gameObject.SetActive(false);

		CustomCoroutine.WaitThenExecute(0.4f, delegate
		{
			ExplorableAreaManager.Instance.HangingLight.intensity = 0f;
			ExplorableAreaManager.Instance.HangingLight.gameObject.SetActive(true);
			ExplorableAreaManager.Instance.HandLight.intensity = 0f;
			ExplorableAreaManager.Instance.HandLight.gameObject.SetActive(true);
		});
	}

	private new static void ApplyModToCard(CardInfo card)
	{
		Log.LogDebug(
			$"[ApplyModToCard] Bones cost [{card.BonesCost}] after mathf.ciel [{Mathf.CeilToInt(card.BonesCost / 2f)}]");
		CardModificationInfo cardModificationInfo = new CardModificationInfo()
		{
			bonesCostAdjustment = -Mathf.CeilToInt(card.BonesCost / 2f),
			abilities = new List<Ability>() { Ability.Brittle }
		};
		GrimoraSaveUtil.DeckInfo.ModifyCard(card, cardModificationInfo);
	}

	private new void OnSlotSelected(MainInputInteractable slot)
	{
		selectionSlot.SetEnabled(false);
		selectionSlot.ShowState(HighlightedInteractable.State.NonInteractable);
		confirmStone.Exit();
		List<CardInfo> validCards = GetValidCards();
		((SelectCardFromDeckSlot)slot).SelectFromCards(validCards, OnSelectionEnded, false);
	}

	private new void OnSelectionEnded()
	{
		selectionSlot.SetShown(true);
		selectionSlot.ShowState(HighlightedInteractable.State.Interactable);
		ViewManager.Instance.SwitchToView(View.Default, false, true);
		if (selectionSlot.Card != null)
		{
			confirmStone.Enter();
		}
	}

	private new static List<CardInfo> GetValidCards()
	{
		List<CardInfo> list = GrimoraSaveUtil.DeckListCopy;
		list.RemoveAll(card => card.BonesCost <= 1
		                       || card.abilities.Contains(Ability.Brittle)
		                       || card.SpecialAbilities.Contains(SpecialTriggeredAbility.RandomCard)
		                       || card.traits.Contains(Trait.Pelt)
		                       || card.traits.Contains(Trait.Terrain)
		);

		return list;
	}

	public static void CreateSequencerInScene()
	{
		if (SpecialNodeHandler.Instance is null)
		{
			return;
		}

		Log.LogDebug($"[AddBoneyardBurialSequencer] Creating boneyard burial");
		GameObject cardStatObj = Instantiate(
			PrefabConstants.CardStatBoostSequencer,
			SpecialNodeHandler.Instance.transform
		);
		cardStatObj.name = "BoneyardBurialSequencer_Grimora";

		Log.LogDebug($"[Boneyard] getting selection slot");
		var selectionSlot = cardStatObj.transform.GetChild(1);

		Log.LogDebug($"[Boneyard] getting stake ring");
		var stakeRing = cardStatObj.transform.Find("StakeRing");

		// destroying things

		Log.LogDebug($"[Boneyard] destroying fireanim");
		Destroy(selectionSlot.GetChild(1).gameObject); //FireAnim 
		//
		// while (cardStatObj.transform.Find("Figurine") is not null)
		// {
		// 	Log.LogDebug($"[Boneyard] destroying figurines");
		// 	Destroy(cardStatObj.transform.Find("Figurine").gameObject);
		// }

		Log.LogDebug($"[Boneyard] destroying existing stake rings");
		for (int i = 0; i < stakeRing.childCount; i++)
		{
			// don't need the stake rings
			Destroy(stakeRing.GetChild(i).gameObject);
		}

		var oldSequencer = cardStatObj.GetComponent<CardStatBoostSequencer>();

		var newSequencer = cardStatObj.AddComponent<BoneyardBurialSequencer>();

		newSequencer.campfireLight = oldSequencer.campfireLight;
		newSequencer.campfireCardLight = oldSequencer.campfireCardLight;
		newSequencer.confirmStone = oldSequencer.confirmStone;
		newSequencer.figurines = new List<CompositeFigurine>() { CreateGravediggerFigurine(cardStatObj) };
		newSequencer.pile = oldSequencer.pile;
		newSequencer.pile.cardbackPrefab = PrefabConstants.GrimoraCardBack;
		newSequencer.selectionSlot = oldSequencer.selectionSlot;
		newSequencer.selectionSlot.cardSelector.selectableCardPrefab = PrefabConstants.GrimoraSelectableCard;
		newSequencer.selectionSlot.pile.cardbackPrefab = PrefabConstants.GrimoraCardBack;
		newSequencer.retrieveCardInteractable = oldSequencer.retrieveCardInteractable;
		newSequencer.stakeRingParent = oldSequencer.stakeRingParent;
		newSequencer.selectionSlot.specificRenderers.RemoveAt(1);


		SpecialNodeHandler.Instance.cardStatBoostSequencer = newSequencer;

		Destroy(oldSequencer);
	}

	private static CompositeFigurine CreateGravediggerFigurine(GameObject cardStatObj)
	{
		Log.LogDebug($"[Boneyard] creating gravedigger");

		CompositeFigurine gravediggerFigurine = Instantiate(
			PrefabConstants.GraveDiggerFigurine,
			new Vector3(0, 5, 2.5f),
			Quaternion.Euler(0, 180, 0),
			cardStatObj.transform
		).AddComponent<CompositeFigurine>();
		gravediggerFigurine.transform.localPosition = new Vector3(0, 5, 2.75f);
		gravediggerFigurine.transform.localScale = new Vector3(4, 4, 4);
		gravediggerFigurine.gameObject.SetActive(false);

		return gravediggerFigurine;
	}

	private CompositeFigurine CreateGrave()
	{
		Log.LogDebug($"[Boneyard] creating grave");
		CompositeFigurine grave = Instantiate(
			AllPrefabs.Single(obj => obj.name.Equals("BoneyardBurialGrave", StringComparison.OrdinalIgnoreCase)),
			new Vector3(0, 4f, 0.6f),
			Quaternion.Euler(0, 90, 0),
			base.transform
		).AddComponent<CompositeFigurine>();
		grave.transform.localScale = new Vector3(0.4f, 0.6f, 0.6f);
		grave.gameObject.SetActive(false);

		return grave;
	}

	private List<CompositeFigurine> CreateTombstones()
	{
		var tombstone1 = Instantiate(
			PrefabConstants.Tombstone3,
			stakeRingParent.transform
		).AddComponent<CompositeFigurine>();

		tombstone1.transform.localPosition = new Vector3(-3, 0, -2.5f);
		tombstone1.transform.localScale = new Vector3(10, 10, 10);

		var tombstone2 = Instantiate(
			PrefabConstants.Tombstone3,
			new Vector3(-1.8f, 0, 0),
			Quaternion.Euler(0, 135, 0),
			stakeRingParent.transform
		).AddComponent<CompositeFigurine>();
		;
		tombstone2.transform.localPosition = new Vector3(-1.8f, 0, 0);
		tombstone2.transform.localScale = new Vector3(10, 10, 10);

		var tombstone3 = Instantiate(
			PrefabConstants.Tombstone3,
			new Vector3(1.8f, 0, 0),
			Quaternion.Euler(0, 215, 0),
			stakeRingParent.transform
		).AddComponent<CompositeFigurine>();
		;
		tombstone3.transform.localPosition = new Vector3(1.8f, 0, 0);
		tombstone3.transform.localScale = new Vector3(10, 10, 10);

		var tombstone4 = Instantiate(
			GrimoraMod.PrefabConstants.Tombstone3,
			new Vector3(3f, 0, -2.5f),
			Quaternion.Euler(0, 90, 0),
			stakeRingParent.transform
		).AddComponent<CompositeFigurine>();
		;
		tombstone4.transform.localPosition = new Vector3(3f, 0, -2.5f);
		tombstone4.transform.localScale = new Vector3(10, 10, 10);

		return new List<CompositeFigurine>() { tombstone1, tombstone2, tombstone3, tombstone4 };
	}
}