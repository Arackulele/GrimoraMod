using System.Collections;
using DiskCardGame;
using HarmonyLib;
using Pixelplacement;
using Sirenix.Utilities;
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

		ViewManager.Instance.Controller.SwitchToControlMode(ViewController.ControlMode.CardMerging);
		ViewManager.Instance.Controller.LockState = ViewLockState.Locked;
		yield return new WaitForSeconds(0.3f);

		Log.LogDebug($"stoneCircleAnim is active");
		stoneCircleAnim.gameObject.SetActive(value: true);
		yield return new WaitForSeconds(0.5f);

		Log.LogDebug($"Spawning cards");
		yield return deckPile.SpawnCards(GrimoraSaveUtil.DeckList.Count, 0.5f);
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
		GrimoraSaveUtil.RemoveCard(sacrificedInfo);

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

			boonCard.SetInfo(sacrificedInfo.HasTrait(Trait.Goat)
				? BoonsUtil.CreateCardForBoon(BoonData.Type.StartingBones)
				: randomCard);

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

		CardInfo cardThatWillHaveEffectApplied = BoonsUtil.CreateCardForBoon(BoonData.Type.StartingBones);
		switch (rngValue)
		{
			// decrease entire deck by 1
			case <= 0.005f:
			{
				// grimora_deck_decrease_cost
				cardThatWillHaveEffectApplied = ApplyEffectToCards(
					"grimora_deck_bones_decrease",
					"... WHAT? WHY DID YOU DO THAT BONE LORD?! [c:bR]DECREASING THE COST OF THE ENTIRE DECK?![c:] YOU FOOL!",
					"THAT'S UNFORTUNATE. YOU WERE SUPPOSED TO HAVE YOUR ENTIRE DECK DECREASED, BUT IT LOOKS LIKE THE BONE LORD HAS ALREADY GIFTED YOU THAT. BEGONE!",
					false
				);

				break;
			}
			// increase entire deck by 1
			case <= 0.01f:
			{
				cardThatWillHaveEffectApplied = ApplyEffectToCards(
					"grimora_deck_bones_increase",
					"OH MY, THE BONE LORD HAS NO EMPATHY TODAY. [c:bR]INCREASING THE COST OF YOUR ENTIRE DECK BY 1[c:], I AM QUITE CURIOUS HOW YOU'LL SURVIVE NOW.",
					"YOU'RE QUITE LUCKY. THE BONE LORD [c:bR]WANTED[c:] TO INCREASE YOUR ENTIRE DECK BY 1, BUT I FELT THAT WAS A BIT HARSH SINCE IT ALREADY HAS HAPPENED. YOU BEST THANK ME.",
					false
				);

				break;
			}
			// bone increase = 9%~
			case <= 0.10f:
			{
				cardThatWillHaveEffectApplied = ApplyEffectToCards(
					"grimora_card_bones_increase",
					"I hope this doesn't hurt too much. [c:bR]{0}[c:] cost has increased!",
					"YOU DON'T HAVE ANYMORE CARDS TO [c:bR]INCREASE THEIR BONE COST[c:], HOW SAD. NOW PLEASE LEAVE."
				);

				break;
			}
			// bone reduce = 20% of the time
			case <= 0.30f:
			{
				cardThatWillHaveEffectApplied = ApplyEffectToCards(
					"grimora_card_bones_decrease",
					"Oh dear, it looks like [c:bR]{0}[c:] cost has decreased!",
					"YOU DON'T HAVE ANYMORE CARDS TO [c:bR]REDUCE THEIR BONE COST[c:], HOW SAD. NOW PLEASE LEAVE.",
					cardInfoPredicate: info => info.BonesCost > 0
				);

				break;
			}
			// card gains 1 HP = 10%?
			case <= 40f:
			{
				cardThatWillHaveEffectApplied = ApplyEffectToCards(
					"grimora_card_health_increase",
					"The Bone Lord has been generous. [c:bR]{0}[c:] base health has increased!",
					"YOU DON'T HAVE ANYMORE CARDS TO [c:bR]GAIN HP[c:], HOW SAD. NOW PLEASE LEAVE.",
					cardInfoPredicate: info => info.Health > 0
				);

				break;
			}
			// card loses 1 HP = 10%?
			case <= 50f:
			{
				cardThatWillHaveEffectApplied = ApplyEffectToCards(
					"grimora_card_health_decrease",
					"Be glad the Bone Lord doesn't take more. [c:bR]{0}[c:] base health has decreased!",
					"YOU DON'T HAVE ANYMORE CARDS TO [c:bR]LOSE HP[c:], HOW SAD. NOW PLEASE LEAVE.",
					cardInfoPredicate: info => info.Health > 1
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
			               && !info.mods.Exists(mod => mod.singletonId == singletonId))
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
		Predicate<CardInfo> cardInfoPredicate = null
	)
	{
		var modificationInfo = GetModInfoFromSingletonId(singletonId);

		List<CardInfo> cards = GetCardsWithoutMod(singletonId, cardInfoPredicate);

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
				cardToReturn.mods.Add(modificationInfo);
				dialogueOnAtLeastOneCard = dialogueOnAtLeastOneCard.Replace("{0}", $"{cardToReturn.displayedName}");
			}
			else
			{
				cards.ForEach(info => info.mods.Add(modificationInfo));
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
		sacrificeSlot.SetEnabled(enabled: false);
		sacrificeSlot.ShowState(HighlightedInteractable.State.NonInteractable);
		confirmStone.Exit();
		((SelectCardFromDeckSlot)slot).SelectFromCards(GrimoraSaveUtil.DeckListCopy, OnSelectionEnded, false);
	}

	private new List<CardInfo> GetValidCards()
	{
		return new List<CardInfo>(GrimoraSaveData.Data.deck.Cards);
	}
}


[HarmonyPatch(typeof(SpecialNodeHandler), nameof(SpecialNodeHandler.StartSpecialNodeSequence))]
public class CardRemoveSequencerPatches
{

	[HarmonyPrefix]
	public static bool CastToGrimoraCardRemoveSequencer(SpecialNodeHandler __instance, ref SpecialNodeData nodeData)
	{
		if (!SaveManager.SaveFile.IsGrimora || nodeData is not CardRemoveNodeData)
		{
			return true;
		}
		
		// We have to cast it, otherwise it tries to call the base version of it
		__instance.StartCoroutine(((GrimoraCardRemoveSequencer)__instance.cardRemoveSequencer).RemoveSequence());
		return false;
	}
	
}
