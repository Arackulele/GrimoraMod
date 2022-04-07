using System.Collections;
using System.Diagnostics.CodeAnalysis;
using DiskCardGame;
using Pixelplacement;
using Sirenix.Utilities;
using UnityEngine;
using static GrimoraMod.GrimoraPlugin;

namespace GrimoraMod;

[SuppressMessage("ReSharper", "InconsistentNaming")]
public class ElectricChairSequencer : CardStatBoostSequencer
{
	public const string ModSingletonId = "GrimoraMod_ElectricChaired";
	
	public static ElectricChairSequencer Instance => FindObjectOfType<ElectricChairSequencer>();

	public static readonly List<Ability> AbilitiesToChoseRandomly = new()
	{
		ActivatedDealDamageGrimora.ability,
		ActivatedDrawSkeletonGrimora.ability,
		ActivatedEnergyDrawWyvern.ability,
		ActivatedGainEnergySoulSucker.ability,
		AlternatingStrike.ability,
		AreaOfEffectStrike.ability,
		BoneThief.ability,
		BuffCrewMates.ability,
		ChaosStrike.ability,
		ColdFront.ability,
		CreateArmyOfSkeletons.ability,
		Erratic.ability,
		FlameStrafe.ability,
		Fylgja_GuardDog.ability,
		GrimoraRandomAbility.ability,
		Haunter.ability,
		HookLineAndSinker.ability,
		Imbued.ability,
		InvertedStrike.ability,
		LatchSubmerge.ability,
		LooseLimb.ability,
		MarchingDead.ability,
		Possessive.ability,
		Puppeteer.ability,
		// SkinCrawler.ability,
		SpiritBearer.ability,
		Ability.ActivatedDealDamage,
		Ability.ActivatedHeal,
		Ability.ActivatedRandomPowerEnergy,
		Ability.ActivatedStatsUp,
		Ability.ActivatedStatsUpEnergy,
		Ability.BeesOnHit,
		Ability.BombSpawner,
		Ability.BoneDigger,
		Ability.BuffNeighbours,
		Ability.CorpseEater,
		Ability.CreateBells,
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
		Ability.GuardDog,
		Ability.IceCube,
		Ability.MoveBeside,
		Ability.QuadrupleBones,
		Ability.Reach,
		Ability.Sentry,
		Ability.Sharp,
		Ability.SkeletonStrafe,
		Ability.Sniper,
		Ability.SplitStrike,
		Ability.SteelTrap,
		Ability.Strafe,
		Ability.StrafePush,
		Ability.Submerge,
		Ability.SwapStats,
		Ability.TailOnHit,
		// Ability.Transformer,
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

	private ElectricChairLever _lever;

	private void Start()
	{
		_lever = figurines[0].transform.Find("Lever").gameObject.AddComponent<ElectricChairLever>();
	}

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
			if (!ProgressionData.LearnedMechanic(GrimoraEnums.Mechanics.ElectricChar))
			{
				yield return TextDisplayer.Instance.ShowUntilInput("OH! I LOVE THIS ONE!");
				yield return TextDisplayer.Instance.ShowUntilInput(
					$"YOU STRAP ONE OF YOUR CARDS TO THE CHAIR, {"EMPOWERING".Blue()} IT!"
				);
				yield return TextDisplayer.Instance.ShowUntilInput(
					"OF COURSE, IT DOESN'T HURT.\nYOU CAN'T DIE TWICE AFTER ALL."
				);

				ProgressionData.SetMechanicLearned(GrimoraEnums.Mechanics.ElectricChar);
			}

			yield return UntilFinishedBuffingOrCardIsDestroyed();
		}

		yield return OutroEnvTeardown();
		if (GameFlowManager.Instance)
		{
			GameFlowManager.Instance.TransitionToGameState(GameState.Map);
		}
	}

	private IEnumerator UntilFinishedBuffingOrCardIsDestroyed()
	{
		yield return confirmStone.WaitUntilConfirmation();
		CardInfo destroyedCard = null;
		bool finishedBuffing = false;
		int numBuffsGiven = 0;
		while (!finishedBuffing && destroyedCard.IsNull())
		{
			Vector3 rulebookLocalPos = TableRuleBook.Instance.gameObject.transform.position;
			Tween.Position(
				TableRuleBook.Instance.gameObject.transform,
				new Vector3(rulebookLocalPos.x, rulebookLocalPos.y, 1),
				0.25f,
				0f
			);

			numBuffsGiven++;
			selectionSlot.Disable();
			RuleBookController.Instance.SetShown(false);
			yield return new WaitForSeconds(0.25f);
			AudioController.Instance.PlaySound3D(
				"teslacoil_charge",
				MixerGroup.TableObjectsSFX,
				selectionSlot.transform.position,
				skipToTime: 0.5f
			);
			ApplyModToCard(selectionSlot.Card.Info);
			selectionSlot.Card.Anim.PlayTransformAnimation();
			yield return new WaitForSeconds(0.15f);
			selectionSlot.Card.StatsLayer.SetEmissionColor(GameColors.Instance.blue);
			selectionSlot.Card.SetInfo(selectionSlot.Card.Info);
			selectionSlot.Card.SetInteractionEnabled(true);
			yield return new WaitForSeconds(0.75f);

			if (numBuffsGiven == 2 || selectionSlot.Card.Info.Abilities.Count == 4)
			{
				break;
			}

			yield return TextDisplayer.Instance.ShowUntilInput(
				"SURELY YOUR CREATURE COULD BECOME MORE POWERFUL..."
			);
			yield return TextDisplayer.Instance.ShowUntilInput(
				"BUT YOU WOULD NEED TO RISK ANOTHER MOMENT UNDER THE SHOCK."
			);
			yield return new WaitForSeconds(0.1f);

			switch (numBuffsGiven)
			{
				case 1:
					TextDisplayer.Instance.ShowMessage(
						"PUSH YOUR LUCK FOR ONE MORE ABILITY?\nOR PULL AWAY?",
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
				(Action<MainInputInteractable>)delegate { cancelledByClickingCard = true; }
			);
			confirmStone.Unpress();
			StartCoroutine(confirmStone.WaitUntilConfirmation());
			yield return new WaitUntil(
				() =>
					confirmStone.SelectionConfirmed
					|| InputButtons.GetButton(Button.LookDown)
					|| InputButtons.GetButton(Button.Cancel)
					|| cancelledByClickingCard
			);
			TextDisplayer.Instance.Clear();
			retrieveCardInteractable.gameObject.SetActive(false);
			confirmStone.Disable();
			yield return new WaitForSeconds(0.1f);
			if (confirmStone.SelectionConfirmed)
			{
				if (UnityRandom.value > 0.5f)
				{
					AudioController.Instance.PlaySound3D(
						"teslacoil_overload",
						MixerGroup.TableObjectsSFX,
						selectionSlot.transform.position
					);
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

		if (destroyedCard)
		{
			// "Before you could pull away, one of the survivors leapt upon the [c:bR][v:0][c:]."
			// "Another jabbed it with a spear."
			// "You looked away as a grotesque feeding frenzy ensued."
			// "Blood and bones flew left and right as you retreated from the scene."
			yield return TextDisplayer.Instance.ShowUntilInput(
				"BONES FLEW LEFT AND RIGHT AS YOU RETREATED FROM THE SCENE."
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
				$"YOU EVER SO CAREFULLY PULL THE {selectionSlot.Card.Info.DisplayedNameLocalized.BrightRed()} AWAY FROM THE ELECTRICITY AND LEAVE."
			);

			AudioController.Instance.PlaySound3D(
				"teslacoil_spark",
				MixerGroup.TableObjectsSFX,
				selectionSlot.transform.position
			);
			yield return new WaitForSeconds(0.1f);
			selectionSlot.FlyOffCard();
		}
	}

	private void OnSlotSelected(MainInputInteractable slot)
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
		if (selectionSlot.Card)
		{
			confirmStone.Enter();
		}

		Tween.LocalPosition(ViewManager.Instance.CameraParent, ChairViewInfo.camPosition, 0.16f, 0f, Tween.EaseInOut);
		Tween.LocalRotation(ViewManager.Instance.CameraParent, ChairViewInfo.camRotation, 0.16f, 0f, Tween.EaseInOut);
	}

	#region ApplyingModToCard

	private void ApplyModToCard(CardInfo card)
	{
		CardModificationInfo cardModificationInfo = new CardModificationInfo
		{
			abilities = new List<Ability> { GetRandomAbility(card) },
			singletonId = ModSingletonId,
			nameReplacement = card.displayedName.Replace("Yellowbeard", "Bluebeard")
		};
		GrimoraSaveUtil.DeckInfo.ModifyCard(card, cardModificationInfo);
	}

	private Ability GetRandomAbility(CardInfo card)
	{
		Ability randomSigil = AbilitiesToChoseRandomly.GetRandomItem();
		while (card.HasAbility(randomSigil) || HasAbilityComboThatWillBreakTheGame(card, randomSigil))
		{
			randomSigil = AbilitiesToChoseRandomly.GetRandomItem();
		}

		if (randomSigil == Ability.IceCube)
		{
			card.iceCubeParams = new IceCubeParams { creatureWithin = NameSkeleton.GetCardInfo() };
		}

		Log.LogDebug($"[ApplyModToCard] Ability from electric chair [{randomSigil}]");
		return randomSigil;
	}

	private static readonly List<Ability> AbilitiesThatShouldNotExistOnZeroAttackCards = new()
	{
		AlternatingStrike.ability, AreaOfEffectStrike.ability, BoneThief.ability, ChaosStrike.ability, InvertedStrike.ability,
		Ability.Sniper, Ability.SplitStrike, Ability.TriStrike
	};

	private bool HasAbilityComboThatWillBreakTheGame(CardInfo card, Ability randomSigil)
	{
		return CheckCardHavingAbilityAndViceVersa(card, Ability.StrafePush, randomSigil, SkinCrawler.ability)
		       || randomSigil == Ability.SwapStats && (card.Attack < 1 || card.Health < 3)
		       || RandomSigilShouldNotExistOnZeroAttackCard(card, randomSigil)
			;
	}

	private bool RandomSigilShouldNotExistOnZeroAttackCard(CardInfo card, Ability randomSigil)
	{
		if (card.Attack >= 1)
		{
			return false;
		}
		return AbilitiesThatShouldNotExistOnZeroAttackCards.Contains(randomSigil)
			 || randomSigil == Ability.Deathtouch && !card.Abilities.Exists(ab => ab is Ability.Sentry or Ability.Sharp);
	}

	private static bool CheckCardHavingAbilityAndViceVersa(
		CardInfo card,
		Ability cardAbility,
		Ability randomSigil,
		Ability abilityToCheckAgainst
	)
	{
		return card.HasAbility(cardAbility) && randomSigil == abilityToCheckAgainst
		       || card.HasAbility(abilityToCheckAgainst) && randomSigil == cardAbility;
	}

	#endregion


	private static List<CardInfo> GetValidCards()
	{
		List<CardInfo> deckCopy = GrimoraSaveUtil.DeckListCopy;
		deckCopy.RemoveAll(
			card => card.Abilities.Count == 4
			        || card.HasAbility(SkinCrawler.ability)
			        || card.SpecialAbilities.Contains(SpecialTriggeredAbility.RandomCard)
			        || card.traits.Contains(Trait.Pelt)
			        || card.traits.Contains(Trait.Terrain)
		);

		return deckCopy;
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
		if (UnityRandom.value < 0.25f && VideoCameraRig.Instance)
		{
			VideoCameraRig.Instance.PlayCameraAnim("refocus_quick");
		}

		// TODO: Change campfire loop to something electrical like
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

		AudioController.Instance.StopLoop(1);

		campfireLight.gameObject.SetActive(false);
		ExplorableAreaManager.Instance.HandLight.gameObject.SetActive(false);
		yield return pile.DestroyCards();
		yield return new WaitForSeconds(0.2f);

		figurines.ForEach(delegate(CompositeFigurine x) { x.gameObject.SetActive(false); });

		stakeRingParent.SetActive(false);
		confirmStone.SetStoneInactive();
		selectionSlot.gameObject.SetActive(false);

		CustomCoroutine.WaitThenExecute(
			0.4f,
			delegate
			{
				ExplorableAreaManager.Instance.HangingLight.intensity = 0f;
				ExplorableAreaManager.Instance.HangingLight.gameObject.SetActive(true);
				ExplorableAreaManager.Instance.HandLight.intensity = 0f;
				ExplorableAreaManager.Instance.HandLight.gameObject.SetActive(true);
			}
		);

		yield return TableRuleBook.Instance.MoveOnBoard();
	}

	#region CreatingInScene

	public static void CreateSequencerInScene()
	{
		if (SpecialNodeHandler.Instance.IsNull())
		{
			return;
		}

		GameObject cardStatObj = Instantiate(
			AssetConstants.CardStatBoostSequencer,
			SpecialNodeHandler.Instance.transform
		);
		cardStatObj.name = "ElectricChairSequencer_Grimora";

		var oldSequencer = cardStatObj.GetComponent<CardStatBoostSequencer>();

		// destroying things
		Destroy(oldSequencer.selectionSlot.transform.Find("FireAnim").gameObject);
		for (int i = 0; i < cardStatObj.transform.childCount; i++)
		{
			var child = cardStatObj.transform.GetChild(i);
			if (child.name.Equals("Figurine"))
			{
				Destroy(child.gameObject);
			}
		}

		for (int i = 0; i < oldSequencer.stakeRingParent.transform.childCount; i++)
		{
			// don't need the stake rings
			Destroy(oldSequencer.stakeRingParent.transform.GetChild(i).gameObject);
		}


		var newSequencer = cardStatObj.AddComponent<ElectricChairSequencer>();

		newSequencer.campfireLight = oldSequencer.campfireLight;
		newSequencer.campfireLight.transform.localPosition = new Vector3(0, 6.75f, 0.63f);
		newSequencer.campfireLight.color = GrimoraColors.ElectricChairLight;

		newSequencer.campfireCardLight = oldSequencer.campfireCardLight;
		newSequencer.campfireCardLight.color = GrimoraColors.ElectricChairLight;
		newSequencer.campfireCardLight.range = 8;

		// TODO: fix creation of lever
		// newSequencer.confirmStone = CreateLever(cardStatObj);
		newSequencer.confirmStone = oldSequencer.confirmStone;
		newSequencer.confirmStone.confirmView = View.CardMergeSlots;
		// ConfirmStoneButton -> Anim -> model -> ConfirmButton -> Quad
		var confirmStoneButton = newSequencer.transform.Find("ConfirmStoneButton");
		var positionCopy = confirmStoneButton.position;
		confirmStoneButton.position = new Vector3(positionCopy.x, positionCopy.y, -0.5f);

		newSequencer.figurines = new List<CompositeFigurine>{ CreateElectricChair(cardStatObj) };

		newSequencer.pile = oldSequencer.pile;
		newSequencer.pile.cardbackPrefab = AssetConstants.GrimoraCardBack;

		newSequencer.selectionSlot = oldSequencer.selectionSlot;
		newSequencer.selectionSlot.transform.localPosition = new Vector3(0, 7.2f, 1.2f);
		newSequencer.selectionSlot.transform.localRotation = Quaternion.Euler(270, 0, 0);
		newSequencer.selectionSlot.cardSelector.selectableCardPrefab = AssetConstants.GrimoraSelectableCard;
		newSequencer.selectionSlot.pile.cardbackPrefab = AssetConstants.GrimoraCardBack;
		var stoneQuad = newSequencer.selectionSlot.transform.Find("Quad").GetComponent<MeshRenderer>();
		Material abilityBoostMat = AssetConstants.ElectricChairSelectionSlot;
		stoneQuad.material = abilityBoostMat;
		stoneQuad.sharedMaterial = abilityBoostMat;

		newSequencer.retrieveCardInteractable = oldSequencer.retrieveCardInteractable;
		newSequencer.retrieveCardInteractable.transform.localPosition = new Vector3(0, 7.2f, 1.4f);
		newSequencer.retrieveCardInteractable.transform.localRotation = Quaternion.Euler(270, 0, 0);

		newSequencer.stakeRingParent = oldSequencer.stakeRingParent;
		// this will throw an exception if we don't remove the specific renderer for fire anim
		newSequencer.selectionSlot.specificRenderers.RemoveAt(1);

		Destroy(oldSequencer);
	}

	private static CompositeFigurine CreateElectricChair(GameObject cardStatObj)
	{
		CompositeFigurine electricChair = Instantiate(
				AssetConstants.ElectricChair,
				new Vector3(-0.05f, 4.9f, 1.2f),
				Quaternion.Euler(0, 180, 0),
				cardStatObj.transform
			)
			.AddComponent<CompositeFigurine>();
		electricChair.name = "Electric Chair";
		electricChair.gameObject.SetActive(false);

		return electricChair;
	}

	#endregion
}
