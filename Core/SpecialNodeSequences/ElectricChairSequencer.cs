using System.Collections;
using DiskCardGame;
using Pixelplacement;
using Sirenix.Utilities;
using UnityEngine;
using static GrimoraMod.GrimoraPlugin;

namespace GrimoraMod;

public class ElectricChairSequencer : CardStatBoostSequencer
{
	public static ElectricChairSequencer Instance => FindObjectOfType<ElectricChairSequencer>();

	public IEnumerator StartSequence()
	{
		yield return InitialSetup();

		if (GetValidCards().IsNullOrEmpty())
		{
			// no valid cards
			yield return new WaitForSeconds(1f);
			yield return TextDisplayer.Instance.PlayDialogueEvent(
				"GainConsumablesFull",
				TextDisplayer.MessageAdvanceMode.Input
			);
			yield return new WaitForSeconds(0.5f);
		}
		else
		{
			// play dialogue
			yield return TextDisplayer.Instance.PlayDialogueEvent("StatBoostIntro", TextDisplayer.MessageAdvanceMode.Input);
			yield return WhileNotFinishedBuffingAndDestroyedCardIsNull();
		}

		yield return OutroEnvTeardown();

		if (GameFlowManager.Instance != null)
		{
			GameFlowManager.Instance.TransitionToGameState(GameState.Map);
		}
	}

	private IEnumerator WhileNotFinishedBuffingAndDestroyedCardIsNull()
	{
		yield return confirmStone.WaitUntilConfirmation();
		CardInfo destroyedCard = null;
		bool finishedBuffing = false;
		int numBuffsGiven = 0;
		while (!finishedBuffing && destroyedCard == null)
		{
			numBuffsGiven++;
			selectionSlot.Disable();
			RuleBookController.Instance.SetShown(false);
			yield return new WaitForSeconds(0.25f);
			AudioController.Instance.PlaySound3D(
				"card_blessing",
				MixerGroup.TableObjectsSFX,
				selectionSlot.transform.position
			);
			selectionSlot.Card.Anim.PlayTransformAnimation();
			ApplyModToCard(selectionSlot.Card.Info);
			yield return new WaitForSeconds(0.15f);
			selectionSlot.Card.SetInfo(selectionSlot.Card.Info);
			selectionSlot.Card.SetInteractionEnabled(false);
			yield return new WaitForSeconds(0.75f);

			if (numBuffsGiven == 4)
			{
				break;
			}

			if (!RunState.Run.survivorsDead)
			{
				yield return TextDisplayer.Instance.PlayDialogueEvent("StatBoostPushLuck" + numBuffsGiven,
					TextDisplayer.MessageAdvanceMode.Input);
				yield return new WaitForSeconds(0.1f);
				switch (numBuffsGiven)
				{
					case 1:
						TextDisplayer.Instance.ShowMessage("Push your luck? Or pull away?", Emotion.Neutral,
							TextDisplayer.LetterAnimation.WavyJitter);
						break;
					case 2:
						TextDisplayer.Instance.ShowMessage("Push your luck further? Or run back?", Emotion.Neutral,
							TextDisplayer.LetterAnimation.WavyJitter);
						break;
					case 3:
						TextDisplayer.Instance.ShowMessage("Recklessly continue?", Emotion.Neutral,
							TextDisplayer.LetterAnimation.WavyJitter);
						break;
				}
			}

			bool cancelledByClickingCard = false;
			retrieveCardInteractable.gameObject.SetActive(true);
			retrieveCardInteractable.CursorSelectEnded = null;
			GenericMainInputInteractable genericMainInputInteractable = retrieveCardInteractable;
			genericMainInputInteractable.CursorSelectEnded = (Action<MainInputInteractable>)Delegate.Combine(
				genericMainInputInteractable.CursorSelectEnded,
				(Action<MainInputInteractable>)delegate { cancelledByClickingCard = true; });
			confirmStone.Unpress();
			StartCoroutine(confirmStone.WaitUntilConfirmation());
			yield return new WaitUntil(() =>
				confirmStone.SelectionConfirmed || InputButtons.GetButton(Button.LookDown) ||
				InputButtons.GetButton(Button.Cancel) || cancelledByClickingCard);
			TextDisplayer.Instance.Clear();
			retrieveCardInteractable.gameObject.SetActive(false);
			confirmStone.Disable();
			yield return new WaitForSeconds(0.1f);
			if (confirmStone.SelectionConfirmed)
			{
				float num = 1f - numBuffsGiven * 0.225f;
				if (SeededRandom.Value(SaveManager.SaveFile.GetCurrentRandomSeed()) > num)
				{
					destroyedCard = selectionSlot.Card.Info;
					selectionSlot.Card.Anim.PlayDeathAnimation();
					GrimoraSaveData.Data.deck.RemoveCard(selectionSlot.Card.Info);
					yield return new WaitForSeconds(1f);
				}
			}
			else
			{
				finishedBuffing = true;
			}
		}

		if (destroyedCard != null)
		{
			yield return TextDisplayer.Instance.PlayDialogueEvent(
				"StatBoostCardEaten",
				TextDisplayer.MessageAdvanceMode.Input,
				TextDisplayer.EventIntersectMode.Wait, new[] { selectionSlot.Card.Info.DisplayedNameLocalized }
			);
			yield return new WaitForSeconds(0.1f);
			selectionSlot.DestroyCard();
		}
		else
		{
			yield return TextDisplayer.Instance.PlayDialogueEvent("StatBoostOutro", TextDisplayer.MessageAdvanceMode.Input);

			yield return new WaitForSeconds(0.1f);
			selectionSlot.FlyOffCard();
		}
	}

	private new void OnSlotSelected(MainInputInteractable slot)
	{
		selectionSlot.SetEnabled(false);
		selectionSlot.ShowState(HighlightedInteractable.State.NonInteractable);
		confirmStone.Exit();
		List<CardInfo> validCards = GetValidCards();
		((SelectCardFromDeckSlot)slot).SelectFromCards(validCards, OnSelectionEnded, false);
	}

	private new static void ApplyModToCard(CardInfo card)
	{
		Ability randomSigil = AbilitiesUtil.GetRandomAbility(
			RandomUtils.GenerateRandomSeed(new List<CardInfo>() { card }), true
		);
		Log.LogDebug($"[ApplyModToCard] Ability [{randomSigil}]");
		CardModificationInfo cardModificationInfo = new CardModificationInfo()
		{
			abilities = new List<Ability>() { randomSigil },
			singletonId = "GrimoraMod_ElectricChaired"
		};
		GrimoraSaveUtil.DeckInfo.ModifyCard(card, cardModificationInfo);
	}

	private new static List<CardInfo> GetValidCards()
	{
		List<CardInfo> list = GrimoraSaveUtil.DeckListCopy;
		list.RemoveAll(card => card.Abilities.Count == 4
		                       || card.SpecialAbilities.Contains(SpecialTriggeredAbility.RandomCard)
		                       || card.traits.Contains(Trait.Pelt)
		                       || card.traits.Contains(Trait.Terrain)
		);

		return list;
	}

	private IEnumerator LearnObjectSequence(Transform obj, float heightOffset, Vector3 baseRotation, string text)
	{
		Tween.Position(obj, new Vector3(0f, 5.7f + heightOffset, -4.25f), 0.1f, 0f, Tween.EaseInOut);
		Tween.Rotation(obj, baseRotation, 0.1f, 0f, Tween.EaseInOut);
		Tween.Rotate(obj, new Vector3(1f, 5f, 3f), Space.World, 3f, 0.1f, Tween.EaseInOut,
			Tween.LoopType.PingPong);
		yield return TextDisplayer.Instance.ShowUntilInput(text);
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

		figurines.ForEach(delegate(CompositeFigurine x) { x.gameObject.SetActive(true); });

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

	private IEnumerator OutroEnvTeardown()
	{
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

		figurines.ForEach(delegate(CompositeFigurine x) { x.gameObject.SetActive(false); });

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
}
