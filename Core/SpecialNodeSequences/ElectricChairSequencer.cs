using System.Collections;
using System.Diagnostics.CodeAnalysis;
using DiskCardGame;
using HarmonyLib;
using InscryptionAPI.Card;
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

	private static readonly ViewInfo ChairViewInfo = new()
	{
		handPosition = PlayerHand3D.HIDDEN_HAND_POS,
		camPosition = new Vector3(0f, 9.3f, -3.5f),
		camRotation = new Vector3(32.5f, 0f, 0f),
		fov = 55f
	};

	private ElectricChairLever _lever;
	private int _burnRateTypeInt = 0;

	private void Start()
	{
		_lever = figurines[0].transform.Find("Lever").gameObject.AddComponent<ElectricChairLever>();
		_burnRateTypeInt = ConfigHelper.Instance.ElectricChairBurnRateType;
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
			if (!EventManagement.HasLearnedMechanicElectricChair)
			{

				yield return TextDisplayer.Instance.ShowUntilInput("Oh, I love this one!");
				yield return TextDisplayer.Instance.ShowUntilInput($"The Electric Chair, it allows you to harness the power of lightning itself, letting you {"empower".Blue()} your cards with mutating abilties!");
				yield return TextDisplayer.Instance.ShowUntilInput("However, I must warn you, it is no ordinary chair. With the ability to adjust the voltage, one wrong move could lead to a shocking disaster.");
				yield return TextDisplayer.Instance.ShowUntilInput($"The brave will master the current, the cowardly will blindly follow it.");
				yield return TextDisplayer.Instance.ShowUntilInput($"Are you bold enough to tinker with its lever on the left, letting the voltage corrupt the selection of the sigils?");



				EventManagement.HasLearnedMechanicElectricChair = true;
			}

			yield return UntilFinishedBuffingOrCardIsDestroyed();
		}

		yield return OutroEnvTeardown();
		if (GameFlowManager.Instance)
		{
			GameFlowManager.Instance.TransitionToGameState(GameState.Map);
		}
	}
	
	public static Dictionary<ElectricChairLever.SigilRisk, float> BuildWithChances(float safeRiskChance, float minorRiskChance, float majorRiskChance)
	{
		return new Dictionary<ElectricChairLever.SigilRisk, float>
		{
			{ ElectricChairLever.SigilRisk.Safe, safeRiskChance },
			{ ElectricChairLever.SigilRisk.Minor, minorRiskChance },
			{ ElectricChairLever.SigilRisk.Major, majorRiskChance }
		};
	}

	private readonly Dictionary<int, Dictionary<ElectricChairLever.SigilRisk, float>> ElectricChairBurnRateByType = new()
	{
		{ 0, BuildWithChances(0.000f, 0.000f, 0.000f) },
		{ 1, BuildWithChances(0.000f, 0.100f, 0.200f) },
		{ 2, BuildWithChances(0.125f, 0.175f, 0.300f) },
		{ 3, BuildWithChances(0.125f, 0.200f, 0.275f) }
	};

	private float GetInitialChanceToDie()
	{
		if (_burnRateTypeInt == 0)
		{
			return 0.5f;
		}
		
		float chance = _burnRateTypeInt == 1 ? 0.3f : 0f;
		chance += ElectricChairBurnRateByType.GetValueSafe(_burnRateTypeInt)[_lever.currentSigilRisk];
		return chance;
	}
	
	private float AddChanceToDieForSecondZap()
	{
		return ElectricChairBurnRateByType.GetValueSafe(_burnRateTypeInt)[_lever.currentSigilRisk];
	}

	private IEnumerator UntilFinishedBuffingOrCardIsDestroyed()
	{
		yield return confirmStone.WaitUntilConfirmation();
		CardInfo destroyedCard = null;
		bool finishedBuffing = false;
		float chanceToDie = GetInitialChanceToDie();
		Log.LogDebug($"[ElectricChair] Initial chance to die is [{chanceToDie}] for second zap");
		int numBuffsGiven = 0;
		while (!finishedBuffing && destroyedCard.SafeIsUnityNull())
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

			if (numBuffsGiven == 2 || selectionSlot.Card.Info.Abilities.Count == 5)
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
			genericMainInputInteractable.CursorSelectEnded += delegate { cancelledByClickingCard = true; }; 
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
				chanceToDie += AddChanceToDieForSecondZap();
				Log.LogDebug($"[ElectricChair] Chance to die is now [{chanceToDie}]");
				if (UnityRandom.value < chanceToDie )
				{

					if (AscensionSaveData.Data.ChallengeIsActive(ChallengeManagement.SafeChair)) { 
						//see when safe chair saves your card from dying
						ChallengeActivationUI.TryShowActivation(ChallengeManagement.SafeChair);
					}
					else { 
					AudioController.Instance.PlaySound3D(
						"teslacoil_overload",
						MixerGroup.TableObjectsSFX,
						selectionSlot.transform.position
					);
					destroyedCard = selectionSlot.Card.Info;
					((GravestoneCardAnimationController)selectionSlot.Card.Anim).PlayGlitchOutAnimation();
					GrimoraSaveData.Data.deck.RemoveCard(selectionSlot.Card.Info);
					yield return new WaitForSeconds(1f);
					}
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
		CardModificationInfo cardModificationInfo;

		if (card.name == NameFranknstein && card.displayedName != "Frankenstein")
		{
			card.displayedName = "Frankenstein";
			card.portraitTex = AssetUtils.GetPrefab<Sprite>("FrankenStein");
			card.SetEmissivePortrait(AssetUtils.GetPrefab<Sprite>("FrankenStein_emission"));

			cardModificationInfo = new CardModificationInfo
			{

				abilities = new List<Ability> { Ability.PermaDeath },
				singletonId = ModSingletonId,
				nameReplacement = "Frankenstein",
				attackAdjustment = 1,
				healthAdjustment = 1

			};
		}

		else
		{
			cardModificationInfo = new CardModificationInfo
			{

				abilities = new List<Ability> { GetRandomAbility(card) },
				singletonId = ModSingletonId,
				nameReplacement = card.displayedName.Replace("Yellowbeard", "Bluebeard")



			};

		}


		RunState.Run.playerDeck.ModifyCard(card, cardModificationInfo);
	}

	private Ability GetRandomAbility(CardInfo card)
	{
		Ability randomSigil = _lever.GetAbilityFromLeverRisk();
		while (card.HasAbility(randomSigil) || HasAbilityComboThatWillBreakTheGame(card, randomSigil))
		{
			randomSigil = _lever.GetAbilityFromLeverRisk();
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
		AlternatingStrike.ability, AreaOfEffectStrike.ability, BloodGuzzler.ability, BoneThief.ability, ChaosStrike.ability, InvertedStrike.ability,
		Ability.AllStrike, Ability.DoubleStrike, Ability.GainAttackOnKill, Ability.Sniper, Ability.SplitStrike, Ability.TriStrike
	};

	private static readonly List<Ability> AbilitiesThatShouldNotExistOnSkinCrawler = new()
	{
		Ability.SkeletonStrafe, Ability.SquirrelStrafe, Ability.Strafe, Ability.StrafePush, Ability.StrafeSwap
	};

	private bool HasAbilityComboThatWillBreakTheGame(CardInfo card, Ability randomSigil)
	{
		return randomSigil == Haunter.ability && card.Abilities.Count == 0
		    || randomSigil == Ability.SwapStats && (card.Attack < 1 || card.Health < 3)
		    || RandomSigilShouldNotExistOnZeroAttackCard(card, randomSigil)
		    || RandomSigilShouldNotExistWithSkinCrawlerOrStrafe(card, randomSigil)
			;
	}

	private bool RandomSigilShouldNotExistWithSkinCrawlerOrStrafe(CardInfo card, Ability randomSigil)
	{
		return card.HasAbility(SkinCrawler.ability) && AbilitiesThatShouldNotExistOnSkinCrawler.Contains(randomSigil)
		       || randomSigil == SkinCrawler.ability && AbilitiesThatShouldNotExistOnSkinCrawler.Exists(card.HasAbility);
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
		List<CardInfo> deckCopy = new List<CardInfo>(RunState.Run.playerDeck.Cards);
		deckCopy.RemoveAll(
			card => card.Abilities.Count == 5
			        || card.HasAbility(SkinCrawler.ability)
			        || card.SpecialAbilities.Contains(SpecialTriggeredAbility.RandomCard)
			        || card.traits.Contains(Trait.Pelt)
			        || card.traits.Contains(Trait.Terrain)
		);

		return deckCopy;
	}

	private IEnumerator InitialSetup()
	{
		InteractionCursor.Instance.SetEnabled(false);
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
		selectCardFromDeckSlot.CursorSelectStarted += OnSlotSelected;
		if (UnityRandom.value < 0.25f && VideoCameraRig.Instance)
		{
			VideoCameraRig.Instance.PlayCameraAnim("refocus_quick");
		}

		// TODO: Change campfire loop to something electrical like
		AudioController.Instance.SetLoopAndPlay("campfire_loop", 1);
		AudioController.Instance.SetLoopVolumeImmediate(0f, 1);
		AudioController.Instance.FadeInLoop(0.5f, 0.75f, 1);
		yield return new WaitForSeconds(0.25f);

		yield return pile.SpawnCards(RunState.Run.playerDeck.Cards.Count, 0.5f);
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
		
		_lever.ResetRisk();

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
		if (SpecialNodeHandler.Instance.SafeIsUnityNull())
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
