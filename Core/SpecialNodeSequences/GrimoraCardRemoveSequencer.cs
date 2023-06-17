using System.Collections;
using DiskCardGame;
using GrimoraMod.Consumables;
using Pixelplacement;
using Sirenix.Utilities;
using UnityEngine;
using static GrimoraMod.GrimoraPlugin;

namespace GrimoraMod;

public class GrimoraCardRemoveSequencer : CardRemoveSequencer
{
	private static readonly int Exit = Animator.StringToHash("exit");

	private static int SigilOffer = 0;

	public const string ModSingletonId = "GrimoraMod_BonelordBuffed";

	private IEnumerator InitialSetup()
	{
		sacrificeSlot.Disable();
		TableRuleBook.Instance.SetOnBoard(true);

		ViewManager.Instance.Controller.SwitchToControlMode(ViewController.ControlMode.CardMerging);
		ViewManager.Instance.SetViewLocked();
		yield return new WaitForSeconds(0.3f);

		stoneCircleAnim.gameObject.SetActive(true);
		yield return new WaitForSeconds(0.5f);
		if (!EventManagement.HasLearnedMechanicCardRemoval)
		{
			yield return TextDisplayer.Instance.ShowUntilInput(
				"HE WILL PROVIDE A HELPFUL CURSE OR BLESSING, AS SOME CALL IT UPON YOUR ARMY IF YOU LEAVE HIM AN OFFERING."
			);
		}

		yield return deckPile.SpawnCards(RunState.Run.playerDeck.Cards.Count, 0.5f);
		ViewManager.Instance.SwitchToView(View.CardMergeSlots);

		ExplorableAreaManager.Instance.TweenHangingLightColors(
			GameColors.Instance.glowRed,
			GameColors.Instance.orange,
			0.1f
		);
		if (!EventManagement.HasLearnedMechanicCardRemoval)
		{
			yield return TextDisplayer.Instance.ShowUntilInput("I HOPE FOR YOUR SAKE HE IS FEELING GENEROUS.");

			EventManagement.HasLearnedMechanicCardRemoval = true;
		}

		sacrificeSlot.RevealAndEnable();
		sacrificeSlot.ClearDelegates();

		SelectCardFromDeckSlot selectCardFromDeckSlot = sacrificeSlot;
		selectCardFromDeckSlot.CursorSelectStarted += OnSlotSelected;

		sacrificeSlot.backOutInputPressed = null;
		SelectCardFromDeckSlot selectCardFromDeckSlot2 = sacrificeSlot;
		selectCardFromDeckSlot2.backOutInputPressed += delegate
		{
			if (sacrificeSlot.Enabled)
			{
				OnSlotSelected(sacrificeSlot);
			}
		};
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

		SigilOffer = sacrificedInfo.Abilities.Count;

		if (sacrificedInfo.metaCategories.Contains(CardMetaCategory.Rare)) SigilOffer++;

		RunState.Run.playerDeck.RemoveCard(sacrificedInfo);

		((GravestoneCardAnimationController)sacrificeSlot.Card.Anim).PlayGlitchOutAnimation();

		AudioController.Instance.PlaySound3D(
			"sacrifice_default",
			MixerGroup.TableObjectsSFX,
			sacrificeSlot.transform.position
		);
		yield return new WaitForSeconds(0.5f);


		if (sacrificedInfo.name == NameObol)
		{

			if (RunState.Run.consumables.Count < 3)
			{

								yield return TextDisplayer.Instance.ShowUntilInput(
					"THE BONELORD IS VERY HAPPY WITH THIS OFFERING"
				);

				yield return new WaitForSeconds(1f);

				ExplorableAreaManager.Instance.TweenHangingLightColors(
				GameColors.Instance.nearBlack,
				GameColors.Instance.darkRed,
				0.1f);


				yield return TextDisplayer.Instance.ShowUntilInput(
										$"{"I AM VERY THANKFUL FOR THIS".Red()}", speaker: DialogueEvent.Speaker.Bonelord, letterAnimation: TextDisplayer.LetterAnimation.WavyJitter, effectEyelidIntensity: 1f, effectFOVOffset: -4
					);

				yield return TextDisplayer.Instance.ShowUntilInput(
						 $"{"TAKE THIS IN RETURN, YOU CAN USE IT TO CALL ME ANYTIME".Red()}", speaker: DialogueEvent.Speaker.Bonelord, letterAnimation: TextDisplayer.LetterAnimation.WavyJitter, effectEyelidIntensity: 1f, effectFOVOffset: -4
					);


				Singleton<ViewManager>.Instance.SwitchToView(View.Consumables);

				

				RunState.Run.consumables.Add(GrimoraPlugin.AllGrimoraItems[10].name);
				Singleton<ItemsManager>.Instance.UpdateItems();




				ExplorableAreaManager.Instance.TweenHangingLightColors(
							GameColors.Instance.glowRed,
							GameColors.Instance.orange,
							0.1f
							);


				yield return TextDisplayer.Instance.ShowUntilInput(
						 $"{"YOU CANNOT END THIS, END ME, THAT IS".Red()}", speaker: DialogueEvent.Speaker.Bonelord, letterAnimation: TextDisplayer.LetterAnimation.WavyJitter, effectEyelidIntensity: 1f, effectFOVOffset: -4
);

				yield return new WaitForSeconds(0.5f);

				Singleton<ViewManager>.Instance.SwitchToView(View.Default);

			}

			else
			{



			}


		}

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

			boonCard.SetInfo(
				sacrificedInfo.HasTrait(Trait.Goat)
					? BoonsUtil.CreateCardForBoon(BoonData.Type.StartingBones)
					: randomCard
			);

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

			boonCard.CursorSelectEnded += OnBoonSelected; 

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
		float rngValue = UnityRandom.value;

		CardInfo cardThatWillHaveEffectApplied = BoonsUtil.CreateCardForBoon(BoonData.Type.StartingBones);




		if (SigilOffer < 2)
		{

			switch (rngValue)
			{

				case <= 0.001f:
					{
						cardThatWillHaveEffectApplied = ApplyEffectToCards(
							"grimora_deck_bones_decrease",
							$"... WHAT? WHY DID YOU DO THAT BONELORD?! {"DECREASING THE COST OF THE ENTIRE DECK?!".BrightRed()} YOU FOOL!",
							$"THAT'S UNFORTUNATE. YOU WERE SUPPOSED TO HAVE YOUR ENTIRE DECK DECREASED, BUT IT LOOKS LIKE THE BONELORD HAS ALREADY GIFTED YOU THAT. BEGONE!",
							false,
							info => info.BonesCost > 0
						);

						break;
					}


				case <= 0.002f:
					{
						cardThatWillHaveEffectApplied = ApplyEffectToCards(
							"grimora_deck_health_increase",
							$"... WHAT? WHY DID YOU DO THAT BONELORD?! {"INCREASING THE HEALTH OF THE ENTIRE DECK?!".BrightRed()} YOU FOOL!",
							$"THAT'S UNFORTUNATE. YOU WERE SUPPOSED TO HAVE YOUR ENTIRE DECK DECREASED, BUT IT LOOKS LIKE THE BONELORD HAS ALREADY GIFTED YOU THAT. BEGONE!",
							false
						);

						break;
					}

				case <= 0.52f:
					{
						// grimora_deck_decrease_cost
						cardThatWillHaveEffectApplied = ApplyEffectToCards(
							"grimora_card_health_increase",
							"The Bonelord has been generous.\n[c:bR]{0}[c:] base health has increased!",
							$"YOU DON'T HAVE ANYMORE CARDS TO {"GAIN HP".BrightRed()}, HOW SAD. NOW PLEASE LEAVE.",
							true
						);

						break;
					}

				case <= 1.1f:
					{
						// grimora_deck_decrease_cost
						cardThatWillHaveEffectApplied = ApplyEffectToCards(
							"grimora_card_bones_decrease",
							"The Bonelord has been generous.\n[c:bR]{0}[c:] bone cost has decreased!",
							$"YOU DON'T HAVE ANYMORE CARDS TO {"GAIN HP".BrightRed()}, HOW SAD. NOW PLEASE LEAVE.",
							true,
							filterCardsOnPredicate: info => info.BonesCost > 0
						);

						break;
					}

			}

		}

		else if (SigilOffer < 4)
		{

			switch (rngValue)
			{

				case <= 0.02f:
					{
						// grimora_deck_decrease_cost
						cardThatWillHaveEffectApplied = ApplyEffectToCards(
							"grimora_deck_bones_decrease",
							$"... WHAT? WHY DID YOU DO THAT BONELORD?! {"DECREASING THE COST OF THE ENTIRE DECK?!".BrightRed()} YOU FOOL!",
							$"THAT'S UNFORTUNATE. YOU WERE SUPPOSED TO HAVE YOUR ENTIRE DECK DECREASED, BUT IT LOOKS LIKE THE BONELORD HAS ALREADY GIFTED YOU THAT. BEGONE!",
							false,
							info => info.BonesCost > 0
						);

						break;
					}


				case <= 0.04f:
					{
						// grimora_deck_decrease_cost
						cardThatWillHaveEffectApplied = ApplyEffectToCards(
							"grimora_deck_health_increase",
							$"... WHAT? WHY DID YOU DO THAT BONELORD?! {"INCREASING THE HEALTH OF THE ENTIRE DECK?!".BrightRed()} YOU FOOL!",
							$"THAT'S UNFORTUNATE. YOU WERE SUPPOSED TO HAVE YOUR ENTIRE DECK INCREASED, BUT IT LOOKS LIKE THE BONELORD HAS ALREADY GIFTED YOU THAT. BEGONE!",
							false
						);

						break;
					}

				case <= 0.23f:
					{
						// grimora_deck_decrease_cost
						cardThatWillHaveEffectApplied = ApplyEffectToCards(
							"grimora_card_attack_increase",
							"The Bonelord has been very generous.\n[c:bR]{0}[c:] base attack has increased!",
							$"YOU DON'T HAVE ANYMORE CARDS TO {"GAIN ATTACK".BrightRed()}, HOW SAD. NOW PLEASE LEAVE.",
							true
						);

						break;
					}

				case <= 0.42f:
					{
						// grimora_deck_decrease_cost
						cardThatWillHaveEffectApplied = ApplyEffectToCards(
							"grimora_card_health_boost",
							"The Bonelord has been very generous.\n[c:bR]{0}[c:] base health has increased greatly!",
							$"YOU DON'T HAVE ANYMORE CARDS TO {"GAIN HP".BrightRed()}, HOW SAD. NOW PLEASE LEAVE.",
							true
						);

						break;
					}

				case <= 0.61f:
					{
						// grimora_deck_decrease_cost
						cardThatWillHaveEffectApplied = ApplyEffectToCards(
							"grimora_card_cost_sink",
							"The Bonelord has been generous.\n[c:bR]{0}[c:] bone cost has decreased greatly!",
							$"YOU DON'T HAVE ANYMORE CARDS TO {"GAIN HP".BrightRed()}, HOW SAD. NOW PLEASE LEAVE.",
							true
						);

						break;
					}

				case <= 0.80f:
					{
						// grimora_deck_decrease_cost
						cardThatWillHaveEffectApplied = ApplyEffectToCards(
							"grimora_card_health_increase",
							"The Bonelord has been generous.\n[c:bR]{0}[c:] base health has increased!",
							$"YOU DON'T HAVE ANYMORE CARDS TO {"GAIN HP".BrightRed()}, HOW SAD. NOW PLEASE LEAVE.",
							true
						);

						break;
					}

				case <= 1.1f:
					{
						// grimora_deck_decrease_cost
						cardThatWillHaveEffectApplied = ApplyEffectToCards(
							"grimora_card_bones_decrease",
							"Oh dear, it looks like [c:bR]{0}[c:] bone cost has decreased!",
							$"YOU DON'T HAVE ANYMORE CARDS TO {"REDUCE THEIR BONE COST".BrightRed()}, HOW SAD. NOW PLEASE LEAVE.",
							true,
						filterCardsOnPredicate: info => info.BonesCost > 0
						);

						break;
					}

			}

		}

		else if (SigilOffer < 5)
		{

			switch (rngValue)
			{

				case <= 0.02f:
					{
						// grimora_deck_decrease_cost
						cardThatWillHaveEffectApplied = ApplyEffectToCards(
							"grimora_deck_bones_decrease",
							$"... WHAT? WHY DID YOU DO THAT BONELORD?! {"DECREASING THE COST OF THE ENTIRE DECK?!".BrightRed()} YOU FOOL!",
							$"THAT'S UNFORTUNATE. YOU WERE SUPPOSED TO HAVE YOUR ENTIRE DECK DECREASED, BUT IT LOOKS LIKE THE BONELORD HAS ALREADY GIFTED YOU THAT. BEGONE!",
							false,
							info => info.BonesCost > 0
						);

						break;
					}


				case <= 0.04f:
					{
						// grimora_deck_decrease_cost
						cardThatWillHaveEffectApplied = ApplyEffectToCards(
							"grimora_deck_health_increase",
							"The Bonelord has been generous.\n[c:bR]{0}[c:] base health has increased!",
							$"YOU DON'T HAVE ANYMORE CARDS TO {"GAIN HP".BrightRed()}, HOW SAD. NOW PLEASE LEAVE.",
							false
						);

						break;
					}

				case <= 0.10f:
					{
						// grimora_deck_decrease_cost
						cardThatWillHaveEffectApplied = ApplyEffectToCards(
							"grimora_card_attack_increase",
							"The Bonelord has been very generous.\n[c:bR]{0}[c:] base attack has increased!",
							$"YOU DON'T HAVE ANYMORE CARDS TO {"GAIN ATTACK".BrightRed()}, HOW SAD. NOW PLEASE LEAVE.",
							true
						);

						break;
					}

				case <= 0.69f:
					{
						// grimora_deck_decrease_cost
						cardThatWillHaveEffectApplied = ApplyEffectToCards(
							"grimora_card_health_boost",
							"The Bonelord has been very generous.\n[c:bR]{0}[c:] base health has increased greatly!",
							$"YOU DON'T HAVE ANYMORE CARDS TO {"GAIN HP".BrightRed()}, HOW SAD. NOW PLEASE LEAVE.",
							true
						);

						break;
					}

				case <= 1.1f:
					{
						// grimora_deck_decrease_cost
						cardThatWillHaveEffectApplied = ApplyEffectToCards(
							"grimora_card_cost_sink",
							"The Bonelord has been generous.\n[c:bR]{0}[c:] bone cost has decreased greatly!",
							$"YOU DON'T HAVE ANYMORE CARDS TO {"GAIN HP".BrightRed()}, HOW SAD. NOW PLEASE LEAVE.",
							true,
							filterCardsOnPredicate: info => info.BonesCost > 1
						);

						break;
					}


			}

		}

		else if (SigilOffer < 6)
		{

			switch (rngValue)
			{


				case <= 0.1f:
					{
						// grimora_deck_decrease_cost
						cardThatWillHaveEffectApplied = ApplyEffectToCards(
							"grimora_deck_bones_decrease",
							$"... WHAT? WHY DID YOU DO THAT BONELORD?! {"DECREASING THE COST OF THE ENTIRE DECK?!".BrightRed()} YOU FOOL!",
							$"THAT'S UNFORTUNATE. YOU WERE SUPPOSED TO HAVE YOUR ENTIRE DECK DECREASED, BUT IT LOOKS LIKE THE BONELORD HAS ALREADY GIFTED YOU THAT. BEGONE!",
							false,
							info => info.BonesCost > 0
						);

						break;
					}


				case <= 0.2f:
					{
						// grimora_deck_decrease_cost
						cardThatWillHaveEffectApplied = ApplyEffectToCards(
							"grimora_deck_health_increase",
							$"... WHAT? WHY DID YOU DO THAT BONELORD?! {"INCREASING THE HEALTH OF THE ENTIRE DECK?!".BrightRed()} YOU FOOL!",
							$"THAT'S UNFORTUNATE. YOU WERE SUPPOSED TO HAVE YOUR ENTIRE DECK INCREASED, BUT IT LOOKS LIKE THE BONELORD HAS ALREADY GIFTED YOU THAT. BEGONE!",
							false
						);

						break;
					}

				case <= 0.44f:
					{
						// grimora_deck_decrease_cost
						cardThatWillHaveEffectApplied = ApplyEffectToCards(
							"grimora_card_attack_increase",
							"The Bonelord has been very generous.\n[c:bR]{0}[c:] base attack has increased!",
							$"YOU DON'T HAVE ANYMORE CARDS TO {"GAIN ATTACK".BrightRed()}, HOW SAD. NOW PLEASE LEAVE.",
							true
						);

						break;
					}

				case <= 0.70f:
					{
						// grimora_deck_decrease_cost
						cardThatWillHaveEffectApplied = ApplyEffectToCards(
							"grimora_card_health_boost",
							"The Bonelord has been very generous.\n[c:bR]{0}[c:] base health has increased greatly!",
							$"YOU DON'T HAVE ANYMORE CARDS TO {"GAIN HP".BrightRed()}, HOW SAD. NOW PLEASE LEAVE.",
							true
						);

						break;
					}

				case <= 1.1f:
					{
						// grimora_deck_decrease_cost
						cardThatWillHaveEffectApplied = ApplyEffectToCards(
							"grimora_card_cost_sink",
							"The Bonelord has been generous.\n[c:bR]{0}[c:] bone cost has decreased greatly!",
							$"YOU DON'T HAVE ANYMORE CARDS TO {"GAIN HP".BrightRed()}, HOW SAD. NOW PLEASE LEAVE.", 
							true,
							filterCardsOnPredicate: info => info.BonesCost > 1
						);

						break;
					}

			}

		}



		RunState.Run.playerDeck.UpdateModDictionary();

		return cardThatWillHaveEffectApplied;
	}

	private List<CardInfo> GetCardsWithoutMod(string singletonId, Predicate<CardInfo> cardInfoPredicate = null)
	{
		return RunState.Run.playerDeck.Cards
		 .Where(
				info => (cardInfoPredicate == null || cardInfoPredicate.Invoke(info))
				     && info.Mods.IsNotNull()
				     && !info.Mods.Exists(mod => mod.singletonId == singletonId)
			)
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

		if (whetherIncreaseOrDecrease.Equals("boost")) howMuchToAdjust = 2;

		if (whetherIncreaseOrDecrease.Equals("sink")) howMuchToAdjust = -2;

		if (whatToApplyTo.Equals("health"))
		{
			modificationInfo.healthAdjustment = howMuchToAdjust;
		}
		else if (whatToApplyTo.Equals("attack"))
		{
			modificationInfo.attackAdjustment = howMuchToAdjust;
		}
		else
		{
			modificationInfo.bonesCostAdjustment = howMuchToAdjust;
		}
		modificationInfo.singletonId = ModSingletonId;
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

		List<CardInfo> cards = GetCardsWithoutMod(ModSingletonId, filterCardsOnPredicate);

		CardInfo cardToReturn = BoonsUtil.CreateCardForBoon(BoonData.Type.StartingBones);
		if (cards.IsNullOrEmpty())
		{
			StartCoroutine(
				TextDisplayer.Instance.ShowUntilInput(
					dialogueNoCardsChosen,
					-0.65f,
					0.4f,
					Emotion.Neutral,
					TextDisplayer.LetterAnimation.WavyJitter
				)
			);
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

			StartCoroutine(
				TextDisplayer.Instance.ShowUntilInput(
					dialogueOnAtLeastOneCard,
					-0.65f,
					0.4f,
					Emotion.Neutral,
					TextDisplayer.LetterAnimation.WavyJitter
				)
			);
		}

		return cardToReturn;
	}

	private new void OnSlotSelected(MainInputInteractable slot)
	{
		gamepadGrid.enabled = false;
		sacrificeSlot.SetEnabled(false);
		sacrificeSlot.ShowState(HighlightedInteractable.State.NonInteractable);
		confirmStone.Exit();

		List<CardInfo> cards = new List<CardInfo>(RunState.Run.playerDeck.Cards);
		((SelectCardFromDeckSlot)slot).SelectFromCards(cards, OnSelectionEnded, false);
	}

	public static void CreateSequencerInScene()
	{
		// TODO: This will work, but it doesn't show the boon art correctly.

		if (SpecialNodeHandler.Instance.SafeIsUnityNull() || SpecialNodeHandler.Instance.cardRemoveSequencer)
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
}
