using System.Collections;
using DiskCardGame;
using Pixelplacement;
using UnityEngine;
using static GrimoraMod.GrimoraPlugin;

namespace GrimoraMod;

public class GrimoraCardRemoveSequencer : CardRemoveSequencer
{
	private static readonly int Exit = Animator.StringToHash("exit");

	public static GrimoraCardRemoveSequencer GetSequencer =>
		UnityEngine.Object.FindObjectOfType<GrimoraCardRemoveSequencer>();

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
			(Action<MainInputInteractable>)Delegate.Combine(selectCardFromDeckSlot.CursorSelectStarted,
				new Action<MainInputInteractable>(OnSlotSelected));

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
			Log.LogDebug($"Spawning card");
			SelectableCard boonCard = SpawnCard(base.transform);
			Log.LogDebug($"boon card gameobject is now active");
			boonCard.gameObject.SetActive(value: true);

			if (sacrificedInfo.HasTrait(Trait.Goat))
			{
				boonCard.SetInfo(BoonsUtil.CreateCardForBoon(BoonData.Type.StartingBones));
			}
			else
			{
				boonCard.SetInfo(BoonsUtil.CreateCardForBoon(BoonData.Type.MinorStartingBones));
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

			boonCard.CursorSelectEnded = (Action<MainInputInteractable>)Delegate.Combine(boonCard.CursorSelectEnded,
				new Action<MainInputInteractable>(OnBoonSelected));

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
			UnityEngine.Object.Destroy(boonCard.gameObject);
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
		GrimoraSaveData.Data.deck.AddBoon(data.type);
		RuleBookController.Instance.SetShown(shown: false);
		boonCard.SetEnabled(enabled: false);
		((SelectableCard)boonCard).SetInteractionEnabled(interactionEnabled: false);

		boonTaken = true;
	}

	private new void OnSlotSelected(MainInputInteractable slot)
	{
		gamepadGrid.enabled = false;
		sacrificeSlot.SetEnabled(enabled: false);
		sacrificeSlot.ShowState(HighlightedInteractable.State.NonInteractable);
		confirmStone.Exit();
		(slot as SelectCardFromDeckSlot).SelectFromCards(GetValidCards(), OnSelectionEnded, forPositiveEffect: false);
	}

	private new List<CardInfo> GetValidCards()
	{
		return new List<CardInfo>(GrimoraSaveData.Data.deck.Cards);
	}
}
