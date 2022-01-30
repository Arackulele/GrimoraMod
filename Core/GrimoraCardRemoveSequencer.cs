using System.Collections;
using DiskCardGame;
using Pixelplacement;
using UnityEngine;
using static GrimoraMod.GrimoraPlugin;
using Random = UnityEngine.Random;

namespace GrimoraMod;

public class GrimoraCardRemoveSequencer : CardRemoveSequencer
{
	private static readonly int Exit = Animator.StringToHash("exit");

	public static GrimoraCardRemoveSequencer GetSequencer =>
		FindObjectOfType<GrimoraCardRemoveSequencer>();

	public new IEnumerator RemoveSequence()
	{
		Log.LogDebug($"Starting removal sequence");
		sacrificeSlot.Disable();
		Log.LogDebug($"Setting rulebook on board");
		TableRuleBook.Instance.SetOnBoard(onBoard: true);

		// ParticleSystem.EmissionModule dustEmission = dustParticles.emission;
		// dustEmission.rateOverTime = new ParticleSystem.MinMaxCurve(10f, 10f);

		ViewManager.Instance.Controller.SwitchToControlMode(ViewController.ControlMode.CardMerging);
		ViewManager.Instance.Controller.LockState = ViewLockState.Locked;
		yield return new WaitForSeconds(0.3f);

		Log.LogDebug($"stoneCircleAnim is active");
		stoneCircleAnim.gameObject.SetActive(value: true);
		yield return new WaitForSeconds(0.5f);

		Log.LogDebug($"Spawning cards");
		yield return deckPile.SpawnCards(GrimoraSaveData.Data.deck.Cards.Count, 0.5f);
		ViewManager.Instance.SwitchToView(View.CardMergeSlots);

		ExplorableAreaManager.Instance.TweenHangingLightColors(
			GameColors.Instance.glowRed,
			GameColors.Instance.orange,
			0.1f
		);

		Log.LogDebug($"sacrificeSlot reveal and enable");
		sacrificeSlot.RevealAndEnable();

		Log.LogDebug($"sacrificeSlot clear delegates");
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

		Log.LogDebug($"sacrificeSlot disable");
		sacrificeSlot.Disable();

		Log.LogDebug($"rulebook controller set shown to false");
		RuleBookController.Instance.SetShown(shown: false);
		yield return new WaitForSeconds(0.25f);
		SpecialCardBehaviour[] components = sacrificeSlot.Card.GetComponents<SpecialCardBehaviour>();
		foreach (SpecialCardBehaviour specialCardBehaviour in components)
		{
			yield return specialCardBehaviour.OnSelectedForCardRemoval();
		}

		CardInfo sacrificedInfo = sacrificeSlot.Card.Info;

		Log.LogDebug($"Removing card from deck");
		GrimoraSaveData.Data.deck.RemoveCard(sacrificedInfo);

		Log.LogDebug($"Playing death animation");
		sacrificeSlot.Card.Anim.PlayDeathAnimation(playSound: false);

		Log.LogDebug($"Sound 3D");
		AudioController.Instance.PlaySound3D(
			"sacrifice_default",
			MixerGroup.TableObjectsSFX,
			sacrificeSlot.transform.position
		);
		yield return new WaitForSeconds(0.5f);

		Log.LogDebug($"Destroy card");
		sacrificeSlot.DestroyCard();

		yield return new WaitForSeconds(0.5f);

		Log.LogDebug($"Skull eyes active");
		skullEyes.SetActive(value: true);
		AudioController.Instance.PlaySound2D("creepy_rattle_lofi");
		yield return new WaitForSeconds(0.5f);

		if (!sacrificedInfo.HasTrait(Trait.Pelt))
		{
			CardInfo randomCard = GetRandomCardForEffect();

			Log.LogDebug($"Spawning card");
			SelectableCard boonCard = SpawnCard(base.transform);
			Log.LogDebug($"boon card game object is now active");
			boonCard.gameObject.SetActive(value: true);

			if (sacrificedInfo.HasTrait(Trait.Goat))
			{
				boonCard.SetInfo(BoonsUtil.CreateCardForBoon(BoonData.Type.StartingBones));
			}
			else
			{
				boonCard.SetInfo(randomCard);
			}

			Log.LogDebug($"boon card is now inactive");
			boonCard.SetEnabled(enabled: false);
			gamepadGrid.Rows[0].interactables.Add(boonCard);
			boonCard.transform.position = sacrificeSlot.transform.position + Vector3.up * 3f;
			boonCard.Anim.Play("fly_on");

			Log.LogDebug($"Placing card on slot");
			sacrificeSlot.PlaceCardOnSlot(boonCard.transform, 0.5f);
			yield return new WaitForSeconds(0.5f);

			Log.LogDebug($"boon card is now active");
			boonCard.SetEnabled(enabled: true);

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

		if (!ProgressionData.LearnedMechanic(MechanicsConcept.CardRemoval))
		{
			yield return TextDisplayer.Instance.ShowUntilInput(
				string.Format(
					Localization.Translate("You shook off the viscera of the poor [c:bR]{0}[c:] and carried onwards."),
					sacrificedInfo.DisplayedNameLocalized), -0.65f, 0.4f, Emotion.Neutral,
				TextDisplayer.LetterAnimation.WavyJitter);
		}

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
		stoneCircleAnim.gameObject.SetActive(value: false);

		Log.LogDebug($"skullEyes.SetActive(value: false);");
		skullEyes.SetActive(value: false);

		Log.LogDebug($"confirmStone.SetStoneInactive");
		confirmStone.SetStoneInactive();

		GameFlowManager.Instance.TransitionToGameState(GameState.Map);
	}

	private new void OnBoonSelected(MainInputInteractable boonCard)
	{
		BoonData data = BoonsUtil.GetData(((SelectableCard)boonCard).Info.boon);
		// GrimoraSaveData.Data.deck.AddBoon(data.type);
		RuleBookController.Instance.SetShown(shown: false);
		boonCard.SetEnabled(enabled: false);
		((SelectableCard)boonCard).SetInteractionEnabled(interactionEnabled: false);

		boonTaken = true;
	}

	// TODO: Need to refactor this into much cleaner method calls
	private CardInfo GetRandomCardForEffect()
	{
		float rngValue = Random.value;

		CardInfo cardThatWillHaveEffectApplied = null;
		switch (rngValue)
		{
			// decrease entire deck by 1
			case <= 0.005f:
			{
				// grimora_deck_decrease_cost
				var cardsWithoutMod = GetCardsWithoutMod("grimora_deck_decrease_cost");

				if (cardsWithoutMod.Count != 0)
				{
					cardsWithoutMod.ForEach(info => info.mods.Add(new CardModificationInfo()
						{ bonesCostAdjustment = -1, singletonId = "grimora_deck_decrease_cost" }));

					StartCoroutine(TextDisplayer.Instance.ShowUntilInput(
						$"... WHAT? WHY DID YOU DO THAT BONE LORD?! [c:bR]DECREASING THE COST OF THE ENTIRE DECK?![c:] YOU FOOL!",
						-0.65f,
						0.4f,
						Emotion.Anger,
						TextDisplayer.LetterAnimation.WavyJitter
					));
				}
				else
				{
					StartCoroutine(TextDisplayer.Instance.ShowUntilInput(
						$"THAT'S UNFORTUNATE. YOU WERE SUPPOSED TO HAVE YOUR ENTIRE DECK DECREASED, BUT IT LOOKS LIKE THE BONE LORD HAS ALREADY GIFTED YOU THAT. BEGONE!",
						-0.65f,
						0.4f,
						Emotion.Anger,
						TextDisplayer.LetterAnimation.WavyJitter
					));
				}

				break;
			}
			// increase entire deck by 1
			case <= 0.01f:
			{
				var cardsWithoutMod = GetCardsWithoutMod("grimora_deck_increase_cost");

				if (cardsWithoutMod.Count != 0)
				{
					cardsWithoutMod.ForEach(info => info.mods.Add(new CardModificationInfo()
						{ bonesCostAdjustment = 1, singletonId = "grimora_deck_increase_cost" }));

					StartCoroutine(TextDisplayer.Instance.ShowUntilInput(
						$"OH MY, THE BONE LORD SEEMS TO HAVE NO EMPATHY TODAY. [c:bR]INCREASING THE COST OF YOUR ENTIRE DECK BY 1[c:], I AM QUITE CURIOUS HOW YOU'LL SURVIVE NOW.",
						-0.65f,
						0.4f,
						Emotion.Neutral,
						TextDisplayer.LetterAnimation.WavyJitter
					));
				}
				else
				{
					StartCoroutine(TextDisplayer.Instance.ShowUntilInput(
						$"YOU'RE QUITE LUCKY. THE BONE LORD [c:bR]WANTED[c:] TO INCREASE YOUR ENTIRE DECK BY 1, BUT I FELT THAT WAS A BIT HARSH SINCE IT ALREADY HAS HAPPENED. YOU BEST THANK ME.",
						-0.65f,
						0.4f,
						Emotion.Neutral,
						TextDisplayer.LetterAnimation.WavyJitter
					));
				}

				break;
			}
			// bone increase = 9%~
			case <= 0.10f:
			{
				var cardsWithoutMod = GetCardsWithoutMod("grimora_bones_increase");

				if (cardsWithoutMod.Count != 0)
				{
					var chosenCard = cardsWithoutMod[0];
					cardThatWillHaveEffectApplied = chosenCard;

					chosenCard.mods.Add(
						new CardModificationInfo() { bonesCostAdjustment = 1, singletonId = "grimora_bones_increase" }
					);

					StartCoroutine(TextDisplayer.Instance.ShowUntilInput(
						$"I hope this doesn't hurt too much. [c:bR]{chosenCard.displayedName}[c:] cost has increased!",
						-0.65f,
						0.4f,
						Emotion.Neutral,
						TextDisplayer.LetterAnimation.WavyJitter
					));
				}
				else
				{
					StartCoroutine(TextDisplayer.Instance.ShowUntilInput(
						$"YOU DON'T HAVE ANYMORE CARDS TO [c:bR]INCREASE THEIR BONE COST[c:], HOW SAD. NOW PLEASE LEAVE.",
						-0.65f,
						0.4f,
						Emotion.Neutral,
						TextDisplayer.LetterAnimation.WavyJitter
					));
				}

				break;
			}
			// bone reduce = 20% of the time
			case <= 0.30f:
			{
				var cardsWithoutMod = GetCardsWithoutMod("grimora_bones_reduce", info => info.BonesCost > 0);

				if (cardsWithoutMod.Count != 0)
				{
					var chosenCard = cardsWithoutMod[0];
					cardThatWillHaveEffectApplied = chosenCard;

					chosenCard.mods.Add(
						new CardModificationInfo() { bonesCostAdjustment = -1, singletonId = "grimora_bones_reduce" }
					);

					StartCoroutine(TextDisplayer.Instance.ShowUntilInput(
						$"Oh dear, it looks like [c:bR]{chosenCard.displayedName}[c:] cost has decreased!",
						-0.65f,
						0.4f,
						Emotion.Neutral,
						TextDisplayer.LetterAnimation.WavyJitter
					));
				} 
				else
				{
					StartCoroutine(TextDisplayer.Instance.ShowUntilInput(
						$"YOU DON'T HAVE ANYMORE CARDS TO [c:bR]REDUCE THEIR BONE COST[c:], HOW SAD. NOW PLEASE LEAVE.",
						-0.65f,
						0.4f,
						Emotion.Neutral,
						TextDisplayer.LetterAnimation.WavyJitter
					));
				}

				break;
			}
			// card gains 1 HP = 10%?
			case <= 40f:
			{
				var cardsWithoutMod = GetCardsWithoutMod("grimora_health_increase", info => info.Health > 0);

				if (cardsWithoutMod.Count != 0)
				{
					var chosenCard = cardsWithoutMod[0];
					cardThatWillHaveEffectApplied = chosenCard;

					chosenCard.mods.Add(
						new CardModificationInfo() { healthAdjustment = 1, singletonId = "grimora_health_increase" }
					);

					StartCoroutine(TextDisplayer.Instance.ShowUntilInput(
						$"The Bone Lord has been generous. [c:bR]{chosenCard.displayedName}[c:] base health has increased!",
						-0.65f,
						0.4f,
						Emotion.Neutral,
						TextDisplayer.LetterAnimation.WavyJitter
					));
				}
				else
				{
					StartCoroutine(TextDisplayer.Instance.ShowUntilInput(
						$"YOU DON'T HAVE ANYMORE CARDS TO [c:bR]GAIN HP[c:], HOW SAD. NOW PLEASE LEAVE.",
						-0.65f,
						0.4f,
						Emotion.Neutral,
						TextDisplayer.LetterAnimation.WavyJitter
					));
				}

				break;
			}
			// card loses 1 HP = 10%?
			case <= 50f:
			{
				var cardsWithoutMod = GetCardsWithoutMod("grimora_health_decrease", info => info.Health > 1);

				if (cardsWithoutMod.Count != 0)
				{
					var chosenCard = cardsWithoutMod[0];
					cardThatWillHaveEffectApplied = chosenCard;

					chosenCard.mods.Add(
						new CardModificationInfo() { healthAdjustment = -1, singletonId = "grimora_health_decrease" }
					);

					StartCoroutine(TextDisplayer.Instance.ShowUntilInput(
						$"Be glad the Bone Lord doesn't take more. [c:bR]{chosenCard.displayedName}[c:] base health has decreased!",
						-0.65f,
						0.4f,
						Emotion.Neutral,
						TextDisplayer.LetterAnimation.WavyJitter
					));
				}
				else
				{
					StartCoroutine(TextDisplayer.Instance.ShowUntilInput(
						$"YOU DON'T HAVE ANYMORE CARDS TO [c:bR]LOSE HP[c:], HOW SAD. NOW PLEASE LEAVE.",
						-0.65f,
						0.4f,
						Emotion.Neutral,
						TextDisplayer.LetterAnimation.WavyJitter
					));
				}

				break;
			}
		}

		GrimoraSaveData.Data.deck.UpdateModDictionary();

		return cardThatWillHaveEffectApplied ??= BoonsUtil.CreateCardForBoon(BoonData.Type.StartingBones);
	}

	private List<CardInfo> GetCardsWithoutMod(string singletonId, Predicate<CardInfo> cardInfoPredicate = null)
	{
		return GrimoraSaveData.Data.deck.Cards
			.Where(info => (cardInfoPredicate is null || cardInfoPredicate.Invoke(info))
			               && !info.mods.Exists(mod => mod.singletonId == singletonId))
			.Randomize()
			.ToList();
	}

	private new void OnSlotSelected(MainInputInteractable slot)
	{
		gamepadGrid.enabled = false;
		sacrificeSlot.SetEnabled(enabled: false);
		sacrificeSlot.ShowState(HighlightedInteractable.State.NonInteractable);
		confirmStone.Exit();
		(slot as SelectCardFromDeckSlot).SelectFromCards(GetValidCards(), OnSelectionEnded, false);
	}

	private new List<CardInfo> GetValidCards()
	{
		return new List<CardInfo>(GrimoraSaveData.Data.deck.Cards);
	}
}
