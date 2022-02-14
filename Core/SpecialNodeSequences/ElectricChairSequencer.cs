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

	private void Start()
	{
		var stoneQuad = selectionSlot.transform.Find("Quad").GetComponent<MeshRenderer>();
		stoneQuad.material = AssetUtils.GetPrefab<Material>("ElectricChair_Stat_AbilityBoost");
		stoneQuad.sharedMaterial = AssetUtils.GetPrefab<Material>("ElectricChair_Stat_AbilityBoost");

		var confirmStoneButton = transform.Find("ConfirmStoneButton");
		var positionCopy = confirmStoneButton.position;
		confirmStoneButton.position = new Vector3(positionCopy.x, positionCopy.y, -0.5f);
	}

	public static readonly List<Ability> AbilitiesToChoseRandomly = new()
	{
		Ability.ActivatedDealDamage,
		Ability.ActivatedDrawSkeleton,
		Ability.ActivatedHeal,
		Ability.ActivatedRandomPowerEnergy,
		Ability.ActivatedSacrificeDrawCards,
		Ability.ActivatedStatsUp,
		Ability.ActivatedStatsUpEnergy,
		Ability.BeesOnHit,
		Ability.BombSpawner,
		Ability.BoneDigger,
		Ability.BuffEnemy,
		Ability.BuffGems,
		Ability.BuffNeighbours,
		Ability.CorpseEater,
		Ability.DeathShield,
		Ability.Deathtouch,
		Ability.DebuffEnemy,
		Ability.DoubleDeath,
		Ability.DrawCopy,
		Ability.DrawNewHand,
		Ability.DrawRabbits,
		Ability.Evolve,
		Ability.ExplodeOnDeath,
		Ability.Flying,
		Ability.GainBattery,
		Ability.GuardDog,
		Ability.IceCube,
		Ability.MoveBeside,
		Ability.PermaDeath,
		Ability.PreventAttack,
		Ability.QuadrupleBones,
		Ability.RandomAbility,
		Ability.RandomConsumable,
		Ability.Reach,
		Ability.Sentry,
		Ability.Sharp,
		Ability.ShieldGems,
		Ability.SkeletonStrafe,
		Ability.Sniper,
		Ability.SplitStrike,
		Ability.SteelTrap,
		Ability.Strafe,
		Ability.StrafePush,
		Ability.Submerge,
		Ability.SwapStats,
		Ability.TailOnHit,
		Ability.Transformer,
		Ability.TriStrike,
		Ability.Tutor,
		Ability.WhackAMole
	};

	private static readonly ViewInfo ChairViewInfo = new()
	{
		handPosition = PlayerHand3D.HIDDEN_HAND_POS,
		camPosition = new Vector3(0f, 9.3f, -3.5f),
		camRotation = new Vector3(32.5f, 0f, 0f),
		fov = 55f
	};

	public IEnumerator StartSequence()
	{
		yield return InitialSetup();

		if (GetValidCards().IsNullOrEmpty())
		{
			// no valid cards
			yield return new WaitForSeconds(1f);
			yield return TextDisplayer.Instance.PlayDialogueEvent(
				"GainConsumablesFull", TextDisplayer.MessageAdvanceMode.Input
			);
			yield return new WaitForSeconds(0.5f);
		}
		else
		{
			yield return TextDisplayer.Instance.ShowUntilInput("Oh! I love this one!", -0.65f);
			yield return new WaitForSeconds(0.1f);
			yield return TextDisplayer.Instance.ShowUntilInput(
				"You strap one of your cards to the chair, [c:B]empowering[c:] it!", -0.65f
			);
			yield return new WaitForSeconds(0.1f);
			yield return TextDisplayer.Instance.ShowUntilInput("Of course, it doesn't hurt.\nYou can't die twice after all.",
				-0.65f
			);
			yield return new WaitForSeconds(0.1f);

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
			selectionSlot.Card.RenderCard();
			yield return new WaitForSeconds(0.15f);
			selectionSlot.Card.SetInfo(selectionSlot.Card.Info);
			selectionSlot.Card.SetInteractionEnabled(false);
			yield return new WaitForSeconds(0.75f);

			if (numBuffsGiven == 2)
			{
				break;
			}

			yield return TextDisplayer.Instance.ShowUntilInput(
				"Surely your creature could become more powerful...", -0.66f
			);
			yield return TextDisplayer.Instance.ShowUntilInput(
				"But you would need to risk another moment under the shock.", -0.66f
			);
			yield return new WaitForSeconds(0.1f);

			switch (numBuffsGiven)
			{
				case 1:
					TextDisplayer.Instance.ShowMessage(
						"Push your luck for one more ability?\nOr pull away?",
						Emotion.Neutral,
						TextDisplayer.LetterAnimation.WavyJitter
					);
					break;
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
				const float changeToDestroyCard = 0.5f;
				if (SeededRandom.Value(SaveManager.SaveFile.GetCurrentRandomSeed()) > changeToDestroyCard)
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
			// "Before you could pull away, one of the survivors leapt upon the [c:bR][v:0][c:]."
			// "Another jabbed it with a spear."
			// "You looked away as a grotesque feeding frenzy ensued."
			// "Blood and bones flew left and right as you retreated from the scene."
			yield return TextDisplayer.Instance.ShowUntilInput(
				"Bones flew left and right as you retreated from the scene", -0.65f
			);

			yield return new WaitForSeconds(0.1f);
			selectionSlot.DestroyCard();
		}
		else
		{
			// "The fire warmed the poor [c:bR][v:1][c:], enhancing its [c:bR][v:0][c:].",
			// "One of the survivors reached toward it.",
			// "Another gnashed their teeth.",
			// "Without a word, you pulled the [c:bR][v:1][c:] away from the fire and left.",
			// yield return TextDisplayer.Instance.PlayDialogueEvent("StatBoostOutro", TextDisplayer.MessageAdvanceMode.Input);

			yield return TextDisplayer.Instance.ShowUntilInput(
				$"The shock empowered the poor [c:bR]{selectionSlot.Card.Info.DisplayedNameLocalized}[c:], enhancing its abilities.",
				-0.65f
			);
			yield return TextDisplayer.Instance.ShowUntilInput(
				$"You ever so carefully pull the [c:bR]{selectionSlot.Card.Info.DisplayedNameLocalized}[c:] away from the electricity and left.",
				-0.65f
			);

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

	private new void OnSelectionEnded()
	{
		selectionSlot.SetShown(true);
		selectionSlot.ShowState(HighlightedInteractable.State.Interactable);
		if (selectionSlot.Card != null)
		{
			confirmStone.Enter();
		}

		Tween.LocalPosition(ViewManager.Instance.CameraParent, ChairViewInfo.camPosition, 0.16f, 0f, Tween.EaseInOut);
		Tween.LocalRotation(ViewManager.Instance.CameraParent, ChairViewInfo.camRotation, 0.16f, 0f, Tween.EaseInOut);
	}

	private new static void ApplyModToCard(CardInfo card)
	{
		CardModificationInfo cardModificationInfo = new CardModificationInfo
		{
			abilities = new List<Ability> { GetRandomAbility(card) },
			singletonId = "GrimoraMod_ElectricChaired"
		};
		GrimoraSaveUtil.DeckInfo.ModifyCard(card, cardModificationInfo);
	}

	private static Ability GetRandomAbility(CardInfo card)
	{
		Ability randomSigil = AbilitiesToChoseRandomly
			.Randomize()
			.ToList()[SeededRandom.Range(0, AbilitiesToChoseRandomly.Count, RandomUtils.GenerateRandomSeed())];
		while (card.HasAbility(randomSigil))
		{
			randomSigil = AbilitiesToChoseRandomly
				.Randomize()
				.ToList()[SeededRandom.Range(0, AbilitiesToChoseRandomly.Count, RandomUtils.GenerateRandomSeed())];
		}

		if (randomSigil == Ability.IceCube)
		{
			card.iceCubeParams = new IceCubeParams { creatureWithin = NameSkeleton.GetCardInfo() };
		}

		Log.LogDebug($"[ApplyModToCard] Ability [{randomSigil}]");
		return randomSigil;
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

	public static void CreateSequencerInScene()
	{
		if (SpecialNodeHandler.Instance is null)
		{
			return;
		}

		Log.LogDebug("[ElectricChair] Creating boneyard burial");
		GameObject cardStatObj = Instantiate(
			PrefabConstants.CardStatBoostSequencer,
			SpecialNodeHandler.Instance.transform
		);
		cardStatObj.name = "ElectricChairSequencer_Grimora";

		Log.LogDebug("[ElectricChair] getting selection slot");
		var selectionSlot = cardStatObj.transform.GetChild(1);

		Log.LogDebug("[ElectricChair] getting stake ring");
		var stakeRing = cardStatObj.transform.Find("StakeRing");

		// destroying things

		Log.LogDebug("[ElectricChair] destroying fireanim");
		Destroy(selectionSlot.GetChild(1).gameObject); //FireAnim 
		for (int i = 0; i < cardStatObj.transform.childCount; i++)
		{
			var child = cardStatObj.transform.GetChild(i);
			if (child.name.Equals("Figurine"))
			{
				Destroy(child.gameObject);
			}
		}

		Log.LogDebug($"[ElectricChair] destroying existing stake rings [{stakeRing.childCount}]");
		for (int i = 0; i < stakeRing.childCount; i++)
		{
			// don't need the stake rings
			Destroy(stakeRing.GetChild(i).gameObject);
		}

		var oldSequencer = cardStatObj.GetComponent<CardStatBoostSequencer>();

		Log.LogDebug("Adding component");
		var newSequencer = cardStatObj.AddComponent<ElectricChairSequencer>();

		Log.LogDebug("Transferring old to new");
		newSequencer.campfireLight = oldSequencer.campfireLight;
		newSequencer.campfireLight.transform.localPosition = new Vector3(0, 6.75f, 0.63f);
		newSequencer.campfireLight.color = new Color(0, 1, 1, 1);
		newSequencer.campfireCardLight = oldSequencer.campfireCardLight;
		newSequencer.campfireCardLight.color = new Color(0, 1, 1, 1);
		newSequencer.campfireCardLight.range = 8;

		// TODO: fix creation of leve
		// newSequencer.confirmStone = CreateLever(cardStatObj);
		newSequencer.confirmStone = oldSequencer.confirmStone;
		newSequencer.confirmStone.confirmView = View.CardMergeSlots;

		newSequencer.figurines = new List<CompositeFigurine>();
		newSequencer.figurines.AddRange(CreateElectricChair(cardStatObj));

		newSequencer.pile = oldSequencer.pile;
		newSequencer.pile.cardbackPrefab = PrefabConstants.GrimoraCardBack;

		newSequencer.selectionSlot = oldSequencer.selectionSlot;
		newSequencer.selectionSlot.transform.localPosition = new Vector3(0, 7.2f, 1.2f);
		newSequencer.selectionSlot.transform.localRotation = Quaternion.Euler(270, 0, 0);
		newSequencer.selectionSlot.cardSelector.selectableCardPrefab = PrefabConstants.GrimoraSelectableCard;
		newSequencer.selectionSlot.pile.cardbackPrefab = PrefabConstants.GrimoraCardBack;

		newSequencer.retrieveCardInteractable = oldSequencer.retrieveCardInteractable;
		newSequencer.stakeRingParent = oldSequencer.stakeRingParent;
		// this will throw an exception if we don't remove the specific renderer for fire anim
		newSequencer.selectionSlot.specificRenderers.RemoveAt(1);

		Log.LogDebug("Destroying old sequencer");
		Destroy(oldSequencer);
	}

	private static IEnumerable<CompositeFigurine> CreateElectricChair(GameObject cardStatObj)
	{
		Log.LogDebug("[ElectricChair] creating chair");
		CompositeFigurine chairFigurine = Instantiate(
			PrefabConstants.ElectricChairForSelectionSlot,
			new Vector3(-0.05f, 4.9f, 1.2f),
			Quaternion.Euler(0, 180, 0),
			cardStatObj.transform
		).AddComponent<CompositeFigurine>();
		chairFigurine.name = "Electric Chair Selection Slot";
		chairFigurine.transform.localScale = new Vector3(1.15f, 1, 0.75f);
		chairFigurine.gameObject.SetActive(false);

		return new List<CompositeFigurine> { chairFigurine };
	}

	private static ConfirmStoneButton CreateLever(GameObject cardStatObj)
	{
		GameObject lever = Instantiate(
			AssetUtils.GetPrefab<GameObject>("ElectricChair_Lever"),
			new Vector3(0, 5, -0.5f),
			Quaternion.identity,
			cardStatObj.transform
		);
		lever.transform.localScale = new Vector3(1.5f, 1.5f, 1.5f);
		ConfirmStoneButton button = lever.transform.GetChild(0).GetChild(1).gameObject.AddComponent<ConfirmStoneButton>();
		button.confirmView = View.CardMergeSlots;
		button.anim = button.transform.parent.GetComponent<Animator>();
		lever.gameObject.SetActive(false);

		return button;
	}
}
