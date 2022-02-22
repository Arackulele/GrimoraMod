using System.Collections;
using DiskCardGame;
using Pixelplacement;
using Sirenix.Utilities;
using UnityEngine;
using static GrimoraMod.GrimoraPlugin;
using Random = UnityEngine.Random;

namespace GrimoraMod;

public class GrimoraCardRemoveSequencer : CardRemoveSequencer
{
	private static readonly int Exit = Animator.StringToHash("exit");

	private IEnumerator InitialSetup()
	{
		sacrificeSlot.Disable();
		TableRuleBook.Instance.SetOnBoard(true);

		ViewManager.Instance.Controller.SwitchToControlMode(ViewController.ControlMode.CardMerging);
		ViewManager.Instance.Controller.LockState = ViewLockState.Locked;
		yield return new WaitForSeconds(0.3f);

		stoneCircleAnim.gameObject.SetActive(true);
		yield return new WaitForSeconds(0.5f);
		if (!ProgressionData.LearnedMechanic(GrimoraMechanics.CardRemoval))
		{
			yield return TextDisplayer.Instance.ShowUntilInput(
				"HE WILL PROVIDE A HELPFUL OR HARMFUL CURSE UPON YOUR ARMY IF YOU LEAVE HIM AN OFFERING.");
		}

		yield return deckPile.SpawnCards(GrimoraSaveUtil.DeckList.Count, 0.5f);
		ViewManager.Instance.SwitchToView(View.CardMergeSlots);

		ExplorableAreaManager.Instance.TweenHangingLightColors(
			GameColors.Instance.glowRed,
			GameColors.Instance.orange,
			0.1f
		);
		if (!ProgressionData.LearnedMechanic(GrimoraMechanics.CardRemoval))
		{
			yield return TextDisplayer.Instance.ShowUntilInput("I HOPE FOR YOUR SAKE HE IS FEELING GENEROUS.");

			ProgressionData.SetMechanicLearned(GrimoraMechanics.CardRemoval);
		}

		sacrificeSlot.RevealAndEnable();
		sacrificeSlot.ClearDelegates();

		SelectCardFromDeckSlot selectCardFromDeckSlot = sacrificeSlot;
		selectCardFromDeckSlot.CursorSelectStarted =
			(Action<MainInputInteractable>)Delegate.Combine(
				selectCardFromDeckSlot.CursorSelectStarted,
				new Action<MainInputInteractable>(OnSlotSelected)
			);

		sacrificeSlot.backOutInputPressed = null;
		SelectCardFromDeckSlot selectCardFromDeckSlot2 = sacrificeSlot;
		selectCardFromDeckSlot2.backOutInputPressed = (Action)Delegate.Combine(selectCardFromDeckSlot2.backOutInputPressed,
			(Action)delegate
			{
				if (sacrificeSlot.Enabled)
				{
					OnSlotSelected(sacrificeSlot);
				}
			});
		gamepadGrid.enabled = true;
		yield return confirmStone.WaitUntilConfirmation();
	}

	public new IEnumerator RemoveSequence()
	{
		yield return InitialSetup();

		sacrificeSlot.Disable();

		RuleBookController.Instance.SetShown(false);
		yield return new WaitForSeconds(0.25f);
		SpecialCardBehaviour[] components = sacrificeSlot.Card.GetComponents<SpecialCardBehaviour>();
		foreach (SpecialCardBehaviour specialCardBehaviour in components)
		{
			yield return specialCardBehaviour.OnSelectedForCardRemoval();
		}

		CardInfo sacrificedInfo = sacrificeSlot.Card.Info;

		GrimoraSaveUtil.RemoveCard(sacrificedInfo);

		sacrificeSlot.Card.Anim.PlayDeathAnimation(false);

		AudioController.Instance.PlaySound3D(
			"sacrifice_default",
			MixerGroup.TableObjectsSFX,
			sacrificeSlot.transform.position
		);
		yield return new WaitForSeconds(0.5f);

		sacrificeSlot.DestroyCard();

		yield return new WaitForSeconds(0.5f);

		skullEyes.SetActive(true);
		AudioController.Instance.PlaySound2D("creepy_rattle_lofi");

		if (!sacrificedInfo.HasTrait(Trait.Pelt))
		{
			CardInfo randomCard = GetRandomCardForEffect();

			Log.LogDebug($"Spawning card");
			SelectableCard boonCard = SpawnCard(transform);
			Log.LogDebug($"boon card game object is now active");
			boonCard.gameObject.SetActive(true);

			boonCard.SetInfo(sacrificedInfo.HasTrait(Trait.Goat)
				? BoonsUtil.CreateCardForBoon(BoonData.Type.StartingBones)
				: randomCard);

			Log.LogDebug($"boon card is now inactive");
			boonCard.SetEnabled(false);
			gamepadGrid.Rows[0].interactables.Add(boonCard);
			boonCard.transform.position = sacrificeSlot.transform.position + Vector3.up * 3f;
			boonCard.Anim.Play("fly_on");

			Log.LogDebug($"Placing card on slot");
			sacrificeSlot.PlaceCardOnSlot(boonCard.transform, 0.5f);
			yield return new WaitForSeconds(0.25f);

			Log.LogDebug($"boon card is now active");
			boonCard.SetEnabled(true);

			boonCard.CursorSelectEnded = (Action<MainInputInteractable>)Delegate.Combine(
				boonCard.CursorSelectEnded,
				new Action<MainInputInteractable>(OnBoonSelected)
			);

			Log.LogDebug($"boon card taken is false");
			boonTaken = false;

			yield return new WaitUntil(() => boonTaken);
			gamepadGrid.enabled = false;

			boonCard.Anim.PlayQuickRiffleSound();
			Tween.Position(
				boonCard.transform,
				boonCard.transform.position + Vector3.back * 4f + Vector3.up * 0.5f,
				0.2f,
				0f,
				Tween.EaseInOut
			);
			Tween.Rotate(boonCard.transform, new Vector3(0f, 90f, 0f), Space.World, 0.2f, 0f);
			yield return new WaitForSeconds(0.25f);

			Log.LogDebug($"Destroying boon card game object");
			Destroy(boonCard.gameObject);
		}

		yield return OutroSequence();
	}

	private IEnumerator OutroSequence()
	{
		Log.LogDebug($"Destroying deck pile");
		yield return deckPile.DestroyCards();

		Log.LogDebug($"Setting Exit trigger");
		stoneCircleAnim.SetTrigger(Exit);

		ExplorableAreaManager.Instance.ResetHangingLightsToZoneColors(0.25f);
		// dustEmission.rateOverTime = new ParticleSystem.MinMaxCurve(0f, 0f);
		yield return new WaitForSeconds(0.25f);

		Log.LogDebug($"Confirm stone exit");
		confirmStone.Exit();
		yield return new WaitForSeconds(0.75f);

		Log.LogDebug($"stoneCircleAnim.gameObject false");
		stoneCircleAnim.gameObject.SetActive(false);

		Log.LogDebug($"skullEyes.SetActive(value: false);");
		skullEyes.SetActive(false);

		Log.LogDebug($"confirmStone.SetStoneInactive");
		confirmStone.SetStoneInactive();

		GameFlowManager.Instance.TransitionToGameState(GameState.Map);
	}

	private new void OnBoonSelected(MainInputInteractable boonCard)
	{
		BoonData data = BoonsUtil.GetData(((SelectableCard)boonCard).Info.boon);
		RuleBookController.Instance.SetShown(false);
		boonCard.SetEnabled(false);
		((SelectableCard)boonCard).SetInteractionEnabled(false);

		boonTaken = true;
	}

	// TODO: Need to refactor this into much cleaner method calls
	private CardInfo GetRandomCardForEffect()
	{
		float rngValue = Random.value;

		CardInfo cardThatWillHaveEffectApplied = BoonsUtil.CreateCardForBoon(BoonData.Type.StartingBones);
		switch (rngValue)
		{
			// decrease entire deck by 1
			case <= 0.005f:
			{
				// grimora_deck_decrease_cost
				cardThatWillHaveEffectApplied = ApplyEffectToCards(
					"grimora_deck_bones_decrease",
					$"... WHAT? WHY DID YOU DO THAT BONE LORD?! {"DECREASING THE COST OF THE ENTIRE DECK?!".BrightRed()} YOU FOOL!",
					$"THAT'S UNFORTUNATE. YOU WERE SUPPOSED TO HAVE YOUR ENTIRE DECK DECREASED, BUT IT LOOKS LIKE THE BONE LORD HAS ALREADY GIFTED YOU THAT. BEGONE!",
					false,
					info => info.BonesCost > 0
				);

				break;
			}
			// increase entire deck by 1
			case <= 0.01f:
			{
				cardThatWillHaveEffectApplied = ApplyEffectToCards(
					"grimora_deck_bones_increase",
					$"OH MY, THE BONE LORD HAS NO EMPATHY TODAY. {"INCREASING THE COST OF YOUR ENTIRE DECK BY 1".BrightRed()}, I AM QUITE CURIOUS HOW YOU'LL SURVIVE NOW.",
					$"YOU'RE QUITE LUCKY. THE BONE LORD {"WANTED".BrightRed()} TO INCREASE YOUR ENTIRE DECK BY 1, BUT I FELT THAT WAS A BIT HARSH SINCE IT ALREADY HAS HAPPENED. YOU BEST THANK ME.",
					false
				);

				break;
			}
			// card bonesCost increase = 9%~
			case <= 0.10f:
			{
				cardThatWillHaveEffectApplied = ApplyEffectToCards(
					"grimora_card_bones_increase",
					"I hope this doesn't hurt too much.\n[c:bR]{0}[c:] cost has increased!",
					$"YOU DON'T HAVE ANYMORE CARDS TO {"INCREASE THEIR BONE COST".BrightRed()}, HOW SAD. NOW PLEASE LEAVE."
				);

				break;
			}
			// card bonesCost reduce = 20% of the time
			case <= 0.30f:
			{
				cardThatWillHaveEffectApplied = ApplyEffectToCards(
					"grimora_card_bones_decrease",
					"Oh dear, it looks like [c:bR]{0}[c:] cost has decreased!",
					$"YOU DON'T HAVE ANYMORE CARDS TO {"REDUCE THEIR BONE COST".BrightRed()}, HOW SAD. NOW PLEASE LEAVE.",
					filterCardsOnPredicate: info => info.BonesCost > 0
				);

				break;
			}
			// card gains 1 HP = 10%?
			case <= 40f:
			{
				cardThatWillHaveEffectApplied = ApplyEffectToCards(
					"grimora_card_health_increase",
					"The Bone Lord has been generous.\n[c:bR]{0}[c:] base health has increased!",
					$"YOU DON'T HAVE ANYMORE CARDS TO {"GAIN HP".BrightRed()}, HOW SAD. NOW PLEASE LEAVE.",
					filterCardsOnPredicate: info => info.Health > 0
				);

				break;
			}
			// card loses 1 HP = 10%?
			case <= 50f:
			{
				cardThatWillHaveEffectApplied = ApplyEffectToCards(
					"grimora_card_health_decrease",
					"Be glad the Bone Lord doesn't take more.\n[c:bR]{0}[c:] base health has decreased!",
					$"YOU DON'T HAVE ANYMORE CARDS TO {"LOSE HP".BrightRed()}, HOW SAD. NOW PLEASE LEAVE.",
					filterCardsOnPredicate: info => info.Health > 1
				);

				break;
			}
		}

		GrimoraSaveUtil.DeckInfo.UpdateModDictionary();

		return cardThatWillHaveEffectApplied;
	}

	private List<CardInfo> GetCardsWithoutMod(string singletonId, Predicate<CardInfo> cardInfoPredicate = null)
	{
		return GrimoraSaveUtil.DeckList
			.Where(info => (cardInfoPredicate is null || cardInfoPredicate.Invoke(info))
			               && info.Mods is not null && !info.Mods.Exists(mod => mod.singletonId == singletonId))
			.Randomize()
			.ToList();
	}

	private CardModificationInfo GetModInfoFromSingletonId(string singletonId)
	{
		string[] splitSingleton = singletonId.Split('_');
		string whatToApplyTo = splitSingleton[2];
		string whetherIncreaseOrDecrease = splitSingleton[3];
		var modificationInfo = new CardModificationInfo();

		int howMuchToAdjust = whetherIncreaseOrDecrease.Equals("increase") ? 1 : -1;

		if (whatToApplyTo.Equals("health"))
		{
			modificationInfo.healthAdjustment = howMuchToAdjust;
		}
		else
		{
			modificationInfo.bonesCostAdjustment = howMuchToAdjust;
		}

		return modificationInfo;
	}

	private CardInfo ApplyEffectToCards(
		string singletonId,
		string dialogueOnAtLeastOneCard,
		string dialogueNoCardsChosen,
		bool isForSingleCard = true,
		Predicate<CardInfo> filterCardsOnPredicate = null
	)
	{
		var modificationInfo = GetModInfoFromSingletonId(singletonId);

		List<CardInfo> cards = GetCardsWithoutMod(singletonId, filterCardsOnPredicate);

		CardInfo cardToReturn = BoonsUtil.CreateCardForBoon(BoonData.Type.StartingBones);
		if (cards.IsNullOrEmpty())
		{
			StartCoroutine(TextDisplayer.Instance.ShowUntilInput(
				dialogueNoCardsChosen,
				-0.65f,
				0.4f,
				Emotion.Neutral,
				TextDisplayer.LetterAnimation.WavyJitter
			));
		}
		else
		{
			if (isForSingleCard)
			{
				cardToReturn = cards[0];
				cardToReturn.Mods.Add(modificationInfo);
				dialogueOnAtLeastOneCard = dialogueOnAtLeastOneCard.Replace("{0}", $"{cardToReturn.DisplayedNameLocalized}");
			}
			else
			{
				cards.ForEach(info => info.Mods.Add(modificationInfo));
			}

			StartCoroutine(TextDisplayer.Instance.ShowUntilInput(
				dialogueOnAtLeastOneCard,
				-0.65f,
				0.4f,
				Emotion.Neutral,
				TextDisplayer.LetterAnimation.WavyJitter
			));
		}

		return cardToReturn;
	}

	private new void OnSlotSelected(MainInputInteractable slot)
	{
		gamepadGrid.enabled = false;
		sacrificeSlot.SetEnabled(false);
		sacrificeSlot.ShowState(HighlightedInteractable.State.NonInteractable);
		confirmStone.Exit();
		((SelectCardFromDeckSlot)slot).SelectFromCards(GrimoraSaveUtil.DeckListCopy, OnSelectionEnded, false);
	}

	public static void CreateSequencerInScene()
	{
		// TODO: This will work, but it doesn't show the boon art correctly.

		if (SpecialNodeHandler.Instance is null || SpecialNodeHandler.Instance.cardRemoveSequencer is not null)
		{
			return;
		}

		GameObject cardRemoveSequencerObj = Instantiate(
			ResourceBank.Get<GameObject>("Prefabs/SpecialNodeSequences/CardRemoveSequencer"),
			SpecialNodeHandler.Instance.transform
		);
		cardRemoveSequencerObj.name = "CardRemoveSequencer_Grimora";

		var oldRemoveSequencer = cardRemoveSequencerObj.GetComponent<CardRemoveSequencer>();

		var cardRemoveSequencer = cardRemoveSequencerObj.AddComponent<GrimoraCardRemoveSequencer>();

		cardRemoveSequencer.gamepadGrid = oldRemoveSequencer.gamepadGrid;
		cardRemoveSequencer.selectableCardPrefab = AssetConstants.GrimoraSelectableCard;
		cardRemoveSequencer.confirmStone = oldRemoveSequencer.confirmStone;
		cardRemoveSequencer.sacrificeSlot = oldRemoveSequencer.sacrificeSlot;
		cardRemoveSequencer.sacrificeSlot.cardSelector.selectableCardPrefab = AssetConstants.GrimoraSelectableCard;
		cardRemoveSequencer.sacrificeSlot.pile.cardbackPrefab = AssetConstants.GrimoraCardBack;
		cardRemoveSequencer.skullEyes = oldRemoveSequencer.skullEyes;
		cardRemoveSequencer.stoneCircleAnim = oldRemoveSequencer.stoneCircleAnim;

		cardRemoveSequencer.deckPile = oldRemoveSequencer.deckPile;
		cardRemoveSequencer.deckPile.cardbackPrefab = AssetConstants.GrimoraCardBack;

		Destroy(oldRemoveSequencer);

		SpecialNodeHandler.Instance.cardRemoveSequencer = cardRemoveSequencer;
	}

	private static void AddDecalRenders()
	{
		// TODO: HOW DO WE GET THOSE DECALS TO SHOW UP
		Log.LogDebug($"Adding decal renders");
		CardDisplayer3D graveDisplayer = FindObjectOfType<CardDisplayer3D>();

		BoonIconInteractable cardAbilityIcons = Instantiate(
			ResourceBank.Get<BoonIconInteractable>("Prefabs/Cards/CardSurfaceInteraction/BoonIcon"),
			graveDisplayer.GetComponentInChildren<CardAbilityIcons>().transform
		);
		graveDisplayer.GetComponentInChildren<CardAbilityIcons>().boonIcon = cardAbilityIcons;

		CardDisplayer3D cardElements = ResourceBank.Get<CardDisplayer3D>("Prefabs/Cards/CardElements");
		CardDisplayer3D cardDisplayer3D = Instantiate(cardElements, graveDisplayer.transform);

		var cardDecals = cardDisplayer3D.transform.Find("CardDecals");
		cardDecals.SetParent(graveDisplayer.transform);
		Destroy(cardDisplayer3D.gameObject);

		graveDisplayer.decalRenderers.Clear();
		for (int i = 0; i < cardDecals.transform.childCount; i++)
		{
			graveDisplayer.decalRenderers.Add(cardDecals.transform.GetChild(i).GetComponent<MeshRenderer>());
			graveDisplayer.decalRenderers[i].enabled = true;
		}
	}
}
