using System.Collections;
using DiskCardGame;
using InscryptionAPI.Card;
using Pixelplacement;
using Sirenix.Utilities;
using UnityEngine;
using static GrimoraMod.GrimoraPlugin;

namespace GrimoraMod;

public class BoneyardBurialSequencer : CardStatBoostSequencer
{
	public const string ModSingletonId = "GrimoraMod_BoneyardBuried";
	
	[SerializeField] private GameObject revenantCard;

	private CardInfo _revenantCardReward;

	private static int effectrandomizer = 1;

	GameObject extralight;

	Light lightComp;

	private void Start()
	{
		_revenantCardReward = NameRevenant.GetCardInfo();

		extralight = new GameObject("ExtraLight");
		lightComp = extralight.AddComponent<Light>();
		lightComp.color = GameColors.Instance.orange;
		extralight.transform.position = new Vector3(0, 7.505f, - 0.0772f);
		lightComp.range = 6;
		lightComp.intensity = 11f;
		lightComp.enabled = false;
	}

	private IEnumerator InitialSetup()
	{
		effectrandomizer = UnityEngine.Random.Range(1, 6);
		stakeRingParent.SetActive(false);
		campfireLight.gameObject.SetActive(false);
		campfireLight.intensity = 1f;
		campfireCardLight.intensity = 1f;




		ExplorableAreaManager.Instance.HangingLight.gameObject.SetActive(false);
		ExplorableAreaManager.Instance.HandLight.gameObject.SetActive(false);
		ViewManager.Instance.SwitchToView(View.Default, false, true);
		ViewManager.Instance.OffsetPosition(new Vector3(0f, 0f, 2.25f), 0.1f);
		yield return new WaitForSeconds(1f);

		figurines.ForEach(delegate(CompositeFigurine x) { x.gameObject.SetActive(true); });

		stakeRingParent.SetActive(true);
		ExplorableAreaManager.Instance.HandLight.gameObject.SetActive(true);
		campfireLight.gameObject.SetActive(true);

	}


	public IEnumerator enableLight()
	{

		yield return new WaitForSeconds(1f);
		lightComp.enabled = true;
		extralight.transform.position = new Vector3(0, 7.505f, -0.0772f);
		extralight.transform.position = new Vector3(0, 7.507f, -0.0772f);
	}

	public IEnumerator BurialSequence()
	{
		yield return InitialSetup();
		yield return InitialSetup();

		yield return StartCoroutine(enableLight());

		if (GetValidCards().IsNullOrEmpty())
		{
			yield return new WaitForSeconds(1f);
			yield return TextDisplayer.Instance.PlayDialogueEvent(
				"GainConsumablesFull",
				TextDisplayer.MessageAdvanceMode.Input
			);
			yield return new WaitForSeconds(0.5f);
			yield return NoValidCardsSequence();
		}
		else
		{


			if (effectrandomizer == 1)
			{

				yield return TextDisplayer.Instance.ShowUntilInput(
					"A LONE GRAVE SITS SOLEMNLY IN FRONT OF YOU.",
					-0.65f
				);
				yield return TextDisplayer.Instance.ShowUntilInput(
					"IN FRONT OF IT IS A MOUND OF EARTH, LEFT BY SOMEONE WHO'S ALREADY PASSED ON.",
					-0.65f
				);
				yield return TextDisplayer.Instance.ShowUntilInput(
					$"PERHAPS A MEMBER OF YOUR UNDEAD HORDE COULD {"DIG THEM UP?".BrightRed()}",
					-0.65f
				);
				yield return TextDisplayer.Instance.ShowUntilInput(
					"THIS WOULDN'T BE WITHOUT REPERCUSSIONS OF COURSE, AS DEATH IS NEVER PERMANENT.",
					-0.65f
				);
				yield return TextDisplayer.Instance.ShowUntilInput(
				$"THE CORPSE SEEMS TO BE OF ROTTING FLESH, YOUR CARD WILL BECOME {"BRITTLE".BrightRed()}, BUT {"ITS BONE COST WILL BE HALVED".BrightRed()}.",
				-0.65f
				);

			}

			if (effectrandomizer == 2)
			{

				yield return TextDisplayer.Instance.ShowUntilInput(
					"A LONE GRAVE SITS SOLEMNLY IN FRONT OF YOU.",
					-0.65f
				);
				yield return TextDisplayer.Instance.ShowUntilInput(
					"IN FRONT OF IT IS A MOUND OF EARTH, LEFT BY SOMEONE WHO'S ALREADY PASSED ON.",
					-0.65f
				);
				yield return TextDisplayer.Instance.ShowUntilInput(
					$"PERHAPS A MEMBER OF YOUR UNDEAD HORDE COULD {"DIG THEM UP?".BrightRed()}",
					-0.65f
				);
				yield return TextDisplayer.Instance.ShowUntilInput(
					"THIS WOULDN'T BE WITHOUT REPERCUSSIONS OF COURSE, AS DEATH IS NEVER PERMANENT.",
					-0.65f
				);
				yield return TextDisplayer.Instance.ShowUntilInput(
				$"THE CORPSE SEEMS TO BE OF A STARVED MAN, YOUR CARD WILL {"STARVE FOREVER".BrightRed()}, BUT {"ITS BONE COST WILL BE REDUCED BY 2".BrightRed()}.",
				-0.65f
				);

			}

			if (effectrandomizer == 3)
			{

				yield return TextDisplayer.Instance.ShowUntilInput(
					"A LONE GRAVE SITS SOLEMNLY IN FRONT OF YOU.",
					-0.65f
				);
				yield return TextDisplayer.Instance.ShowUntilInput(
					"IN FRONT OF IT IS A MOUND OF EARTH, LEFT BY SOMEONE WHO'S ALREADY PASSED ON.",
					-0.65f
				);
				yield return TextDisplayer.Instance.ShowUntilInput(
					$"PERHAPS A MEMBER OF YOUR UNDEAD HORDE COULD {"DIG THEM UP?".BrightRed()}",
					-0.65f
				);
				yield return TextDisplayer.Instance.ShowUntilInput(
					"THIS WOULDN'T BE WITHOUT REPERCUSSIONS OF COURSE, AS DEATH IS NEVER PERMANENT.",
					-0.65f
				);
				yield return TextDisplayer.Instance.ShowUntilInput(
				$"THE CORPSE SEEMS TO BE OF AN INCORPOREAL GHOST, YOUR CARD WILL {"COST 2 SOULS".BrightRed()} , BUT {"ITS BONE COST WILL BE REDUCED BY 1".BrightRed()}.",
				-0.65f
				);

			}

			if (effectrandomizer == 4)
			{

				yield return TextDisplayer.Instance.ShowUntilInput(
					"A LONE GRAVE SITS SOLEMNLY IN FRONT OF YOU.",
					-0.65f
				);
				yield return TextDisplayer.Instance.ShowUntilInput(
					"IN FRONT OF IT IS A MOUND OF EARTH, LEFT BY SOMEONE WHO'S ALREADY PASSED ON.",
					-0.65f
				);
				yield return TextDisplayer.Instance.ShowUntilInput(
					$"PERHAPS A MEMBER OF YOUR UNDEAD HORDE COULD {"DIG THEM UP?".BrightRed()}",
					-0.65f
				);
				yield return TextDisplayer.Instance.ShowUntilInput(
					"THIS WOULDN'T BE WITHOUT REPERCUSSIONS OF COURSE, AS DEATH IS NEVER PERMANENT.",
					-0.65f
				);
				yield return TextDisplayer.Instance.ShowUntilInput(
				$"THE CORPSE SEEMS TO BE OF A CHAINED PRISONER, YOUR CARD WILL {"DEAL 1 MORE DAMAGE".BrightRed()} , BUT {"IT WILL BECOME VERY FRAIL".BrightRed()}.",
				-0.65f
				);

			}

			if (effectrandomizer == 5)
			{

				yield return TextDisplayer.Instance.ShowUntilInput(
					"A LONE GRAVE SITS SOLEMNLY IN FRONT OF YOU.",
					-0.65f
				);
				yield return TextDisplayer.Instance.ShowUntilInput(
					"IN FRONT OF IT IS A MOUND OF EARTH, LEFT BY SOMEONE WHO'S ALREADY PASSED ON.",
					-0.65f
				);
				yield return TextDisplayer.Instance.ShowUntilInput(
					$"PERHAPS A MEMBER OF YOUR UNDEAD HORDE COULD {"DIG THEM UP?".BrightRed()}",
					-0.65f
				);
				yield return TextDisplayer.Instance.ShowUntilInput(
					"THIS WOULDN'T BE WITHOUT REPERCUSSIONS OF COURSE, AS DEATH IS NEVER PERMANENT.",
					-0.65f
				);
				yield return TextDisplayer.Instance.ShowUntilInput(
				$"THE CORPSE SEEMS TO BE OF A MISCHIEVIOUS SPIRIT, YOUR CARD WILL {"HAUNT OTHERS".BrightRed()} , BUT {"WILL COST 3 SOULS".BrightRed()}.",
				-0.65f
				);

			}


			selectionSlot.gameObject.SetActive(true);

			switch (effectrandomizer)
			{
				default:
				case 1:
					selectionSlot.specificRenderers[0].material = AssetConstants.BrittleGraveSelectionSlot;
					selectionSlot.specificRenderers[0].sharedMaterial = AssetConstants.BrittleGraveSelectionSlot;

					break;

				case 2:
					selectionSlot.specificRenderers[0].material = AssetConstants.StarveGraveSelectionSlot;
					selectionSlot.specificRenderers[0].sharedMaterial = AssetConstants.StarveGraveSelectionSlot;

					break;

				case 3:
					selectionSlot.specificRenderers[0].material = AssetConstants.EnergyGraveSelectionSlot;
					selectionSlot.specificRenderers[0].sharedMaterial = AssetConstants.EnergyGraveSelectionSlot;

					break;

				case 4:

					selectionSlot.specificRenderers[0].material = AssetConstants.PrisonerGraveSelectionSlot;
					selectionSlot.specificRenderers[0].sharedMaterial = AssetConstants.PrisonerGraveSelectionSlot;

					break;

				case 5:
					selectionSlot.specificRenderers[0].material = AssetConstants.HaunterGraveSelectionSlot;
					selectionSlot.specificRenderers[0].sharedMaterial = AssetConstants.HaunterGraveSelectionSlot;

					break;

			}

			selectionSlot.RevealAndEnable();
			selectionSlot.ClearDelegates();

			SelectCardFromDeckSlot selectCardFromDeckSlot = selectionSlot;
			selectCardFromDeckSlot.CursorSelectStarted += OnSlotSelected;
			if (UnityRandom.value < 0.25f && VideoCameraRig.Instance)
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

			yield return pile.SpawnCards(RunState.Run.playerDeck.Cards.Count, 0.5f);
			TableRuleBook.Instance.SetOnBoard(true);
			InteractionCursor.Instance.SetEnabled(true);

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
				ApplyModToCard(selectionSlot.Card.Info);
				selectionSlot.Card.StatsLayer.SetEmissionColor(GameColors.Instance.darkLimeGreen);
				selectionSlot.Card.Anim.PlayTransformAnimation();
				yield return new WaitForSeconds(0.15f);
				selectionSlot.Card.SetInfo(selectionSlot.Card.Info);
				selectionSlot.Card.SetInteractionEnabled(false);
				yield return new WaitForSeconds(0.75f);
				finishedBuffing = true;
			}



			if (EventManagement.HasLearnedMechanicBoneyard)
			{
				yield return TextDisplayer.Instance.ShowUntilInput("MARVELOUS! THEY CAME CRAWLING BACK AFTER DIGGING UP THE CORPSE.");
				yield return TextDisplayer.Instance.ShowUntilInput("THEY STILL CARE ABOUT YOU IT SEEMS!");
			}
			else
			{
				yield return TextDisplayer.Instance.ShowUntilInput(
					$"TORN FROM ITS ETERNAL RESPITE WITH A RELUCTANT GROAN, THE {selectionSlot.Card.Info.DisplayedNameLocalized.Red()} SHAMBLES BACK TO ITS RIGHTFUL PLACE AMONG YOUR HORDE."
				);
				yield return TextDisplayer.Instance.ShowUntilInput(
					"ITS BONES HOLLOWED THROUGH BY THE CREATURES OF THE SOIL, LEAVING IT FEELING UNNATURAL AND UNFAMILIAR."
				);
				yield return TextDisplayer.Instance.ShowUntilInput("THOUGH THE WEIGHT OF CONSEQUENCE ALSO SEEMS LIFTED...");

				EventManagement.HasLearnedMechanicBoneyard = true;
			}
		}
		
		yield return OutroEnvTeardown();

		if (GameFlowManager.Instance)
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

		figurines.ForEach(delegate(CompositeFigurine x) { x.gameObject.SetActive(false); });

		stakeRingParent.SetActive(false);
		confirmStone.SetStoneInactive();
		selectionSlot.gameObject.SetActive(false);
		lightComp.enabled = false;

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
	}

	private IEnumerator NoValidCardsSequence()
	{
		revenantCard = Instantiate(
			AssetConstants.GrimoraSelectableCard,
			new Vector3(0, 12, 1.75f),
			Quaternion.identity,
			transform
		);
		var revenantSelectableCard = revenantCard.GetComponent<SelectableCard>();
		revenantSelectableCard.ClearDelegates();
		revenantSelectableCard.SetInfo(_revenantCardReward);

		bool cardGrabbed = false;
		Log.LogDebug($"Playing lowering sequence");
		Vector3 targetPos = new Vector3(0, 5, 0);
		Tween.Position(revenantCard.transform, revenantCard.transform.position - targetPos, 2f, 0f);
		revenantSelectableCard.CursorSelectEnded += delegate { cardGrabbed = true; };
		yield return new WaitUntil(() => cardGrabbed);

		RuleBookController.Instance.SetShown(false);
		TableRuleBook.Instance.SetOnBoard(false);
		yield return new WaitForEndOfFrame();

		revenantCard.SetActive(true);
		revenantCard.transform.parent = null;
		revenantCard.transform.position = revenantCard.transform.position;
		revenantCard.transform.rotation = revenantCard.transform.rotation;
		revenantSelectableCard.SetInfo(_revenantCardReward);
		revenantSelectableCard.SetInteractionEnabled(false);

		string text = _revenantCardReward.description;
		yield return LearnObjectSequence(revenantCard.transform, 1f, new Vector3(20f, 0f, 0f), text);
		Tween.Position(
			revenantCard.transform,
			revenantCard.transform.position + Vector3.up * 2f + Vector3.forward * 0.5f,
			0.1f,
			0f,
			null,
			Tween.LoopType.None,
			null,
			delegate { Destroy(revenantCard); }
		);

		yield return new WaitForSeconds(0.5f);
		GrimoraSaveData.Data.deck.AddCard(_revenantCardReward);
	}

	private IEnumerator LearnObjectSequence(Transform obj, float heightOffset, Vector3 baseRotation, string text)
	{
		Tween.Position(obj, new Vector3(0f, 5.7f + heightOffset, -4.25f), 0.1f, 0f, Tween.EaseInOut);
		Tween.Rotation(obj, baseRotation, 0.1f, 0f, Tween.EaseInOut);
		Tween.Rotate(
			obj,
			new Vector3(1f, 5f, 3f),
			Space.World,
			3f,
			0.1f,
			Tween.EaseInOut,
			Tween.LoopType.PingPong
		);
		yield return TextDisplayer.Instance.ShowUntilInput(text);
	}

	private static void ApplyModToCard(CardInfo card)
	{

		CardModificationInfo cardModificationInfo = new CardModificationInfo();


		if ( effectrandomizer == 1)
		{ 

			cardModificationInfo = new CardModificationInfo
			{
				abilities = new List<Ability> { Ability.Brittle },
				bonesCostAdjustment = -Mathf.CeilToInt(card.BonesCost / 2f),
				singletonId = ModSingletonId
			};

		}

		if (effectrandomizer == 2)
		{

			cardModificationInfo = new CardModificationInfo
			{
				abilities = new List<Ability> { Malnourishment.ability },
				bonesCostAdjustment = -2,
			singletonId = ModSingletonId
			};

		}

		if (effectrandomizer == 3)
		{

			cardModificationInfo = new CardModificationInfo
			{
				bonesCostAdjustment = -1,
				energyCostAdjustment = 2,
				singletonId = ModSingletonId
			};

		}

		if (effectrandomizer == 4)
		{

			cardModificationInfo = new CardModificationInfo
			{
				healthAdjustment = -(card.Health - 1),
				attackAdjustment = 1,
				singletonId = ModSingletonId
			};

		}

		if (effectrandomizer == 5)
		{

			cardModificationInfo = new CardModificationInfo
			{
				abilities = new List<Ability> { Haunter.ability },
				energyCostAdjustment = 3,
				singletonId = ModSingletonId
			};

		}

		RunState.Run.playerDeck.ModifyCard(card, cardModificationInfo);
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
		ViewManager.Instance.SwitchToView(View.Default, false, true);
		if (selectionSlot.Card)
		{
			confirmStone.Enter();
		}
	}

	private static List<CardInfo> GetValidCards()
	{
		List<CardInfo> list = new List<CardInfo>(RunState.Run.playerDeck.Cards);

		switch (effectrandomizer)
		{
			default:
			case 1:
		list.RemoveAll(
			card => card.BonesCost <= 1
			        || card.Abilities.Count == 5
			        || card.HasAbility(Ability.Brittle)
			        || card.HasSpecialAbility(SpecialTriggeredAbility.RandomCard)
			        || card.traits.Exists(trait => trait == Trait.Pelt || trait == Trait.Terrain)
		);
				break;

			case 2:
				list.RemoveAll(
					card => card.BonesCost <= 1
									|| card.Abilities.Count == 5
									|| card.HasAbility(Malnourishment.ability)
									|| card.HasSpecialAbility(SpecialTriggeredAbility.RandomCard)
									|| card.traits.Exists(trait => trait == Trait.Pelt || trait == Trait.Terrain)
				);
				break;

			case 4:
				list.RemoveAll(
					card => card.Health <= 1
									|| card.HasSpecialAbility(SpecialTriggeredAbility.RandomCard)
									|| card.traits.Exists(trait => trait == Trait.Pelt || trait == Trait.Terrain)
				);
				break;

			case 3:
				list.RemoveAll(
					card => card.BonesCost <= 0
									|| card.EnergyCost >= 5
									|| card.HasSpecialAbility(SpecialTriggeredAbility.RandomCard)
									|| card.traits.Exists(trait => trait == Trait.Pelt || trait == Trait.Terrain)
				);
				break;

			case 5:
				list.RemoveAll(
					card => card.EnergyCost >= 4
									|| card.Abilities.Count == 5
									|| card.HasAbility(Haunter.ability)
									|| card.HasSpecialAbility(SpecialTriggeredAbility.RandomCard)
									|| card.traits.Exists(trait => trait == Trait.Pelt || trait == Trait.Terrain)
				);
				break;


		}

		return list;
	}

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
		cardStatObj.name = "BoneyardBurialSequencer_Grimora";

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

		var newSequencer = cardStatObj.AddComponent<BoneyardBurialSequencer>();

		newSequencer.campfireLight = oldSequencer.campfireLight;
		newSequencer.campfireCardLight = oldSequencer.campfireCardLight;

		newSequencer.confirmStone = oldSequencer.confirmStone;

		newSequencer.figurines = new List<CompositeFigurine>()
			{ CreateGravediggerFigurine(cardStatObj), CreateGrave(cardStatObj) };
		newSequencer.figurines.AddRange(CreateTombstones(cardStatObj));

		newSequencer.pile = oldSequencer.pile;
		newSequencer.pile.cardbackPrefab = AssetConstants.GrimoraCardBack;

		newSequencer.selectionSlot = oldSequencer.selectionSlot;


		var selectionSlot = newSequencer.selectionSlot;
		selectionSlot.cardSelector.selectableCardPrefab = AssetConstants.GrimoraSelectableCard;
		selectionSlot.pile.cardbackPrefab = AssetConstants.GrimoraCardBack;
		selectionSlot.specificRenderers.RemoveAt(1);

		switch (effectrandomizer)
		{
			default:
			case 1:
				selectionSlot.specificRenderers[0].material = AssetConstants.BrittleGraveSelectionSlot;
				selectionSlot.specificRenderers[0].sharedMaterial = AssetConstants.BrittleGraveSelectionSlot;

				break;

			case 2:
				selectionSlot.specificRenderers[0].material = AssetConstants.StarveGraveSelectionSlot;
				selectionSlot.specificRenderers[0].sharedMaterial = AssetConstants.StarveGraveSelectionSlot;

				break;

			case 3:
				selectionSlot.specificRenderers[0].material = AssetConstants.PrisonerGraveSelectionSlot;
				selectionSlot.specificRenderers[0].sharedMaterial = AssetConstants.PrisonerGraveSelectionSlot;

									break;

			case 4:
				selectionSlot.specificRenderers[0].material = AssetConstants.EnergyGraveSelectionSlot;
				selectionSlot.specificRenderers[0].sharedMaterial = AssetConstants.EnergyGraveSelectionSlot;

				break;

			case 5:
				selectionSlot.specificRenderers[0].material = AssetConstants.HaunterGraveSelectionSlot;
				selectionSlot.specificRenderers[0].sharedMaterial = AssetConstants.HaunterGraveSelectionSlot;

				break;

		}

		selectionSlot.transform.localPosition = new Vector3(0, 5.4f, 0.4f);
		selectionSlot.transform.localScale = new Vector3(1f, 1f, 1f);
		selectionSlot.transform.localRotation = Quaternion.Euler(0, 0, 0);
		selectionSlot.defaultColor = GrimoraColors.GrimoraText;
		selectionSlot.baseColor = GrimoraColors.GrimoraText;

		var stoneQuad = newSequencer.confirmStone.transform.Find("Quad").GetComponent<MeshRenderer>();
		Material shovelForConfirmButton = AssetConstants.BoneyardConfirmButton;
		stoneQuad.material = shovelForConfirmButton;
		stoneQuad.sharedMaterial = shovelForConfirmButton;
		stoneQuad.transform.localScale = new Vector3(1.75f, 1.75f, 1.75f);
		stoneQuad.transform.localRotation = Quaternion.Euler(90, 0, 0);

		newSequencer.retrieveCardInteractable = oldSequencer.retrieveCardInteractable;

		newSequencer.stakeRingParent = oldSequencer.stakeRingParent;

		SpecialNodeHandler.Instance.cardStatBoostSequencer = newSequencer;

		

		Destroy(oldSequencer);
	}

	private static CompositeFigurine CreateGravediggerFigurine(GameObject cardStatObj)
	{
		CompositeFigurine gravediggerFigurine = Instantiate(
				AssetConstants.BoneyardFigurine,
				new Vector3(2.2f, 5, 3.75f),
				Quaternion.Euler(0, 208, 0),
				cardStatObj.transform
			)
			.AddComponent<CompositeFigurine>();
		gravediggerFigurine.name = "Boneyard Gravedigger";
		gravediggerFigurine.transform.localPosition = new Vector3(2.2f, 5, 3.75f);
		gravediggerFigurine.transform.localScale = new Vector3(4, 4, 4);
		gravediggerFigurine.gameObject.SetActive(false);

		return gravediggerFigurine;
	}

	private static CompositeFigurine CreateGrave(GameObject cardStatObj)
	{
		CompositeFigurine grave = Instantiate(
				AssetConstants.BoneyardGrave,
				new Vector3(-0.0145f, 6.4309f, 2.1464f),
				Quaternion.Euler(270, 270, 0),
				cardStatObj.transform
			)
			.AddComponent<CompositeFigurine>();
		grave.name = "Boneyard Burial Grave";
		grave.transform.localScale = new Vector3(40f, 40f, 40);
		grave.gameObject.SetActive(false);


	

		return grave;
	}

	private static List<CompositeFigurine> CreateTombstones(GameObject cardStatObj)
	{
		var stakeRing = cardStatObj.transform.Find("StakeRing");
		var tombstone1 = Instantiate(AssetConstants.Tombstone3, stakeRing).AddComponent<CompositeFigurine>();
		tombstone1.transform.localPosition = new Vector3(-3, 0, -2.5f);
		tombstone1.transform.localRotation = Quaternion.Euler(0, 90, 0);
		tombstone1.transform.localScale = new Vector3(10, 10, 10);

		var tombstone2 = Instantiate(AssetConstants.Tombstone3, stakeRing).AddComponent<CompositeFigurine>();
		tombstone2.transform.localPosition = new Vector3(-1.8f, 0, 0);
		tombstone2.transform.localRotation = Quaternion.Euler(0, 135, 0);
		tombstone2.transform.localScale = new Vector3(10, 10, 10);

		var tombstone3 = Instantiate(AssetConstants.Tombstone3, stakeRing).AddComponent<CompositeFigurine>();
		tombstone3.transform.localPosition = new Vector3(1.8f, 0, 0);
		tombstone3.transform.localRotation = Quaternion.Euler(0, -135, 0);
		tombstone3.transform.localScale = new Vector3(10, 10, 10);

		var tombstone4 = Instantiate(AssetConstants.Tombstone3, stakeRing).AddComponent<CompositeFigurine>();
		tombstone4.transform.localPosition = new Vector3(3f, 0, -2.5f);
		tombstone3.transform.localRotation = Quaternion.Euler(0, 90, 0);
		tombstone4.transform.localScale = new Vector3(10, 10, 10);

		return new List<CompositeFigurine>() { tombstone1, tombstone2, tombstone3, tombstone4 };
	}
}
