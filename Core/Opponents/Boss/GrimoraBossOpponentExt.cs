using System.Collections;
using DiskCardGame;
using GracesGames.Common.Scripts;
using GrimoraMod.Saving;
using HarmonyLib;
using InscryptionAPI.Card;
using InscryptionAPI.Encounters;
using InscryptionAPI.Helpers.Extensions;
using Pixelplacement;
using UnityEngine;
using static GrimoraMod.GrimoraPlugin;

namespace GrimoraMod;

public class GrimoraBossOpponentExt : BaseBossExt
{
	public static readonly OpponentManager.FullOpponent FullOpponent = OpponentManager.Add(
		GUID,
		"GrimoraBoss",
		GrimoraModGrimoraBossSequencer.FullSequencer.Id,
		typeof(GrimoraBossOpponentExt)
	);

	public override StoryEvent EventForDefeat => GrimoraEnums.StoryEvents.GrimoraDefeated;

	public override string DefeatedPlayerDialogue => "Thank you!";

	public override int StartingLives => 3;

	private Animator _bonelordSnapAnim = null;

	private static void SetSceneEffectsShownGrimora(Color cardLightColor)
	{
		Color purple = GameColors.Instance.purple;
		Color darkPurple = GameColors.Instance.darkPurple;
		TableVisualEffectsManager.Instance.ChangeTableColors(
			darkPurple,
			cardLightColor,
			purple,
			darkPurple,
			darkPurple,
			purple,
			purple,
			darkPurple,
			purple
		);
	}


	public override IEnumerator IntroSequence(EncounterData encounter)
	{
		PlayTheme();

		encounter.startConditions = new List<EncounterData.StartCondition>()
		{
			new()
			{
				cardsInOpponentSlots = new[] { NameObol.GetCardInfo() }
			}
		};

		if (!ConfigHelper.Instance.IsDevModeEnabled)
		{
			yield return TextDisplayer.Instance.PlayDialogueEvent(
				"RoyalBossPreIntro",
				TextDisplayer.MessageAdvanceMode.Input
			);

			yield return TextDisplayer.Instance.PlayDialogueEvent(
				"LeshyBossIntro1",
				TextDisplayer.MessageAdvanceMode.Input
			);
		}

		yield return base.IntroSequence(encounter);

		ViewManager.Instance.SwitchToView(View.BossSkull, false, true);
		if (!ConfigHelper.Instance.IsDevModeEnabled)
		{
			yield return TextDisplayer.Instance.PlayDialogueEvent(
				"LeshyBossAddCandle",
				TextDisplayer.MessageAdvanceMode.Input
			);
			yield return new WaitForSeconds(0.4f);
		}

		bossSkull.EnterHand();

		SetSceneEffectsShownGrimora(GrimoraColors.GrimoraBossCardLight);

		yield return new WaitForSeconds(2f);
		ViewManager.Instance.SwitchToView(View.Default);

		if (ConfigHelper.HasIncreaseSlotsMod)
		{
			yield return TextDisplayer.Instance.ShowUntilInput("OH? FIVE LANES? HOW BOLD.");
		}

		ViewManager.Instance.SetViewUnlocked();

		if(ChessboardMapExt.Instance.debugHelper)
		{
			if (ChessboardMapExt.Instance.debugHelper.StartAtTwinGiants)
			{
				NumLives = 2;
				yield return PostResetScalesSequence();
			}
			else if (ChessboardMapExt.Instance.debugHelper.StartAtBonelord)
			{
				NumLives = 1;
				yield return PostResetScalesSequence();
			}
		}
		
	}

	public override void PlayTheme()
	{
		Log.LogDebug("Playing Grimora theme");
		AudioController.Instance.StopAllLoops();
		AudioController.Instance.SetLoopAndPlay("Grimoras_Theme_Phase1", 1);
		AudioController.Instance.SetLoopVolumeImmediate(0f, 1);
		AudioController.Instance.SetLoopVolume(0.8f, 10f, 1);
	}

	private bool _hasSpawnedFirstGiant;

	public override void ModifyQueuedCard(PlayableCard card)
	{
		if (card.Info.name == NameBoneLordsHorn)
		{
			ModifyBonelordsHorn(card);
		}
	}

	public override void ModifySpawnedCard(PlayableCard card)
	{
		switch (card.Info.name)
		{
			case NameGiantEphialtes:
			case NameGiantOtis:
				HandleResizingGiantCards(card);
				break;
			case NameBonelord:
				HandleResizingGiantCards(card, true);
				break;
			case NameBoneLordsHorn:
				ModifyBonelordsHorn(card);
				break;
			case NameBonePrince:
				ModifyBonePrince(card);
				break;
		}
	}

	private void HandleResizingGiantCards(PlayableCard playableCard, bool isBonelord = false)
	{
		// Card -> RotatingParent (child zero) -> TombstoneParent -> Cardbase_StatsLayer
		Log.LogDebug($"[GiantCardResize] Resizing [{playableCard}]");
		Transform rotatingParent = playableCard.transform.GetChild(0);

		float xValPosition = -0.7f;
		float xValScale = 2.1f;
		if (ConfigHelper.HasIncreaseSlotsMod && isBonelord)
		{
			xValPosition = -1.4f;
			xValScale = 3.3f;
		}

		rotatingParent.localPosition = new Vector3(xValPosition, 1.05f, 0);
		rotatingParent.localScale = new Vector3(xValScale, 2.1f, 1);
	}

	private void ModifyTwinGiant(PlayableCard playableCard)
	{
		var modInfo = new CardModificationInfo(-1, 1)
		{
			abilities = new List<Ability> { Ability.Reach, GiantStrike.ability, Ability.MadeOfStone },
			negateAbilities = new List<Ability> { Ability.QuadrupleBones, Ability.SplitStrike }
		};
		var modNameReplace = new CardModificationInfo
		{
			nameReplacement = _hasSpawnedFirstGiant ? "Ephialtes" : "Otis",
			specialAbilities = new List<SpecialTriggeredAbility> { GrimoraGiant.FullSpecial.Id }
		};
		_hasSpawnedFirstGiant = true;
		CardInfo clone = (CardInfo)playableCard.Info.Clone();
		clone.Mods.Add(modNameReplace);
		playableCard.SetInfo(clone);
		HandleResizingGiantCards(playableCard);
		playableCard.AddTemporaryMod(modInfo);
	}

	private void ModifyBonelordsHorn(PlayableCard playableCard)
	{
		var modInfo = new CardModificationInfo(2, 0)
		{
			negateAbilities = new List<Ability> { Ability.QuadrupleBones }
		};
		playableCard.AddTemporaryMod(modInfo);
	}

	private void ModifyBonePrince(PlayableCard playableCard)
	{
		playableCard.AddTemporaryMod(new CardModificationInfo(Ability.BuffNeighbours));
	}

	public override IEnumerator StartNewPhaseSequence()
	{
		TurnPlan.Clear();
		yield return ClearBoard();
		yield return ClearQueue();
		Log.LogInfo("[Grimora] Cleared board and queue");

		yield return new WaitForSeconds(0.5f);

		switch (NumLives)
		{
			case 1:
			{
				yield return StartBoneLordPhase();
				break;
			}
			case 2:
			{
				yield return StartSpawningGiantsPhase();
				break;
			}
		}

		ViewManager.Instance.SetViewUnlocked();
	}

	private IEnumerator StartSpawningGiantsPhase()
	{
		int secondGiantIndex = ConfigHelper.HasIncreaseSlotsMod ? 4 : 3;
		Log.LogInfo("[Grimora] Start of giants phase");
		var oppSlots = BoardManager.Instance.OpponentSlotsCopy;

		yield return TextDisplayer.Instance.ShowUntilInput(
			"BEHOLD, MY LATEST CREATIONS! THE TWIN GIANTS!",
			letterAnimation: TextDisplayer.LetterAnimation.WavyJitter
		);

		ViewManager.Instance.SwitchToView(View.OpponentQueue, false, true);

		SetSceneEffectsShownGrimora(GameColors.Instance.lightPurple);

		yield return CreateTwinGiants(oppSlots[1], oppSlots[secondGiantIndex]);

		Log.LogInfo("[Grimora] Finished creating giants");

		if (ConfigHelper.HasIncreaseSlotsMod)
		{
			CardInfo obol = NameObol.GetCardInfo();
			yield return oppSlots[2].CreateCardInSlot(obol, 0.2f);
			CardSlot thirdLaneQueueSlot = BoardManager.Instance.GetQueueSlots()[2];
			yield return TurnManager.Instance.Opponent.QueueCard(obol, thirdLaneQueueSlot);
		}

		yield return new WaitForSeconds(0.5f);
	}

	private IEnumerator CreateTwinGiants(CardSlot otisSlot, CardSlot ephiSlot)
	{

		// mimics the moon phase
		Log.LogInfo("[Grimora] Creating Otis");
		yield return otisSlot.CreateCardInSlot(NameGiantOtis.GetCardInfo(), 0.3f);
		yield return TextDisplayer.Instance.ShowUntilInput($"[size:5]Otis![size:]");
		
		Log.LogInfo("[Grimora] Creating Ephi");
		yield return ephiSlot.CreateCardInSlot(NameGiantEphialtes.GetCardInfo(), 0.3f);
		yield return TextDisplayer.Instance.ShowUntilInput($"[size:5]Ephialtes![size:]");
	}

	public IEnumerator StartBoneLordPhase()
	{
		Log.LogInfo("[Grimora] Start of Bonelord phase");
		AudioController.Instance.FadeOutLoop(3f);
		AudioController.Instance.StopAllLoops();
		AudioController.Instance.SetLoopAndPlay("Grimora_Bone_Lords_Theme_Phase3", 1);
		AudioController.Instance.SetLoopVolumeImmediate(0.1f, 1);
		AudioController.Instance.FadeInLoop(7f, 0.5f, 1);

		ViewManager.Instance.SwitchToView(View.DefaultUpwards, false, true);

		SetSceneEffectsShownGrimora(GrimoraColors.GrimoraBossCardLight);
		yield return new WaitForSeconds(0.1f);

		Log.LogDebug($"Spawning Bonelord effects");
		GameObject bonelordEffect = Instantiate(AssetUtils.GetPrefab<GameObject>("BonelordTableEffects"));
		CameraEffects.Instance.Shake(0.15f, 5f);
		yield return TextDisplayer.Instance.ShowThenClear(
			$"LET THE [size:5]{"BONELORD".BrightRed()}[size:] COMMETH!",
			4.6f,
			letterAnimation: TextDisplayer.LetterAnimation.WavyJitter
		);

		var oppSlots = BoardManager.Instance.OpponentSlotsCopy;
		int bonelordSlotIndex = ConfigHelper.HasIncreaseSlotsMod ? 3 : 2;
		yield return GlitchInCard(NameBonelord.GetCardInfo(), oppSlots[bonelordSlotIndex]);

		yield return BeginBonelordsReign();

		yield return CreateHornsInFarLeftAndRightLanes(oppSlots);

		int ashpowerpool = RunState.Run.currency;

		CardInfo AshCard = CardLoader.GetCardByName(NameAshes);

		Ability AshAbility;

		switch (ashpowerpool)
		{
			case 0:
				AshAbility = Ability.Brittle;
				break;
			case >= 100:
				AshAbility = Ability.TriStrike;
				break;
			case >= 80:
				AshAbility = Ability.DoubleStrike;
				break;
			case >= 70:
				AshAbility = Slasher.ability;
				break;
			case >= 50:
				AshAbility = Imbued.ability;
				break;
			case >= 40:
				AshAbility = Ability.Tutor;
				break;
			case >= 30:
				AshAbility = LooseLimb.ability;
				break;
			case >= 20:
				AshAbility = Ability.Sharp;
				break;
			case >= 10:
				AshAbility = Ability.Strafe;
				break;
			default:
			case >=5:
				AshAbility = Boneless.ability;
				break;
		}

		CardModificationInfo AshMods;

		if (ashpowerpool == 0) { 
		AshMods = new CardModificationInfo
		{
			attackAdjustment = 0,
			healthAdjustment = 1,
			abilities = new List<Ability> { AshAbility }

		};
		}
		else { 
			AshMods = new CardModificationInfo
			{
			attackAdjustment = (int)(ashpowerpool / 12),
			healthAdjustment = (int)(ashpowerpool / 6),
			abilities = new List<Ability> { AshAbility }

			};
		}

		yield return new WaitForSeconds(0.8f);

		yield return TextDisplayer.Instance.ShowUntilInput(
						$"{"I ALMOST FORGOT".Red()}", speaker: DialogueEvent.Speaker.Bonelord, letterAnimation: TextDisplayer.LetterAnimation.WavyJitter, effectEyelidIntensity: 1f, effectFOVOffset: -4
	);

		yield return TextDisplayer.Instance.ShowUntilInput(
						$"{"ALL OF THIS SUFFERING YOU CAUSED, ALL OF THIS PAIN".Red()}", speaker: DialogueEvent.Speaker.Bonelord, letterAnimation: TextDisplayer.LetterAnimation.WavyJitter, effectEyelidIntensity: 1f, effectFOVOffset: -4
	);

		yield return TextDisplayer.Instance.ShowUntilInput(
						$"{"YOU OUGHT TO BE REWARDED FOR ALL THAT EXCESS DAMAGE".Red()}", speaker: DialogueEvent.Speaker.Bonelord, letterAnimation: TextDisplayer.LetterAnimation.WavyJitter, effectEyelidIntensity: 1f, effectFOVOffset: -4
	);

		ViewManager.Instance.SwitchToView(View.Hand, false, true);

		yield return TextDisplayer.Instance.ShowUntilInput(
						$"{"TAKE THIS FOR YOUR EFFORTS".Red()}", speaker: DialogueEvent.Speaker.Bonelord, letterAnimation: TextDisplayer.LetterAnimation.WavyJitter
	);

		if (ashpowerpool < 24)
		{

			yield return TextDisplayer.Instance.ShowUntilInput(
				$"{"WHAT A TERRIBLE FOE, THIS WONT DEFEAT ME!".Red()}", speaker: DialogueEvent.Speaker.Bonelord, letterAnimation: TextDisplayer.LetterAnimation.WavyJitter);

				AshCard.displayedName = "Minor Ashes";
				AshCard.portraitTex = AssetUtils.GetPrefab<Sprite>("LesserAshes");
				AshCard.SetEmissivePortrait(AssetUtils.GetPrefab<Sprite>("LesserAshes_emission"));

		}
		else if (ashpowerpool > 47)
		{

			yield return TextDisplayer.Instance.ShowUntilInput(
				$"{"WHAT DID YOU DO. I AM DOOMED.".Red()}", speaker: DialogueEvent.Speaker.Bonelord, letterAnimation: TextDisplayer.LetterAnimation.WavyJitter);

			AshCard.displayedName = "Greater Ashes";
			AshCard.portraitTex = AssetUtils.GetPrefab<Sprite>("GreaterAshes");
			AshCard.SetEmissivePortrait(AssetUtils.GetPrefab<Sprite>("GreaterAshes_emission"));

		}
		else yield return TextDisplayer.Instance.ShowUntilInput(
				$"{"A FORMIDABLE OPPONENT, YOU DID WELL IT SEEMS.".Red()}", speaker: DialogueEvent.Speaker.Bonelord, letterAnimation: TextDisplayer.LetterAnimation.WavyJitter);

		yield return Singleton<CardSpawner>.Instance.SpawnCardToHand(AshCard, new List<CardModificationInfo> { AshMods }, 0.25f );

	}

	private IEnumerator BeginBonelordsReign()
	{
		var activePlayerCards = BoardManager.Instance.GetPlayerCards();
		Log.LogDebug($"[BonelordsReign] Player cards [{activePlayerCards.Count}]");
		if (activePlayerCards.Any())
		{
			ViewManager.Instance.SwitchToView(View.Default, false, true);
			_bonelordSnapAnim.gameObject.SetActive(true);
			yield return new WaitForSeconds(0.1f);
			_bonelordSnapAnim.Play("bonelord_snap", 0, 0);
			yield return new WaitForSeconds(1.2f);
			foreach (var playableCard in activePlayerCards)
			{
				playableCard.Anim.StrongNegationEffect();
				int attack = playableCard.Attack == 0 ? 0 : 1 - playableCard.Attack;
				CardModificationInfo mod = new CardModificationInfo(attack, 1 - playableCard.Health);
				playableCard.AddTemporaryMod(mod);
				playableCard.Anim.PlayTransformAnimation();
				yield return new WaitForSeconds(0.25f);
			}

			yield return TextDisplayer.Instance.ShowThenClear(
				"DID YOU REALLY THINK THE BONELORD WOULD LET YOU OFF THAT EASILY?!",
				3f,
				letterAnimation: TextDisplayer.LetterAnimation.WavyJitter
			);
			_bonelordSnapAnim.gameObject.SetActive(false);
		}
	}

	private IEnumerator GlitchInCard(CardInfo cardInfo, CardSlot slotToSpawnIn)
	{
		ViewManager.Instance.SwitchToView(View.OpponentQueue, false, true);

		Log.LogInfo($"[Grimora] Creating [{cardInfo.name}]");
		PlayableCard playableCard = CardSpawner.SpawnPlayableCard(cardInfo);

		Log.LogDebug($"Try load glitch3d mat");
		GlitchOutAssetEffect.TryLoad3DMaterial();
		Material glitch3DMaterial = GlitchOutAssetEffect.glitch3DMaterial;

		Renderer[] componentsInChildren = playableCard.GetComponentsInChildren<Renderer>();
		Dictionary<Renderer, Material> originalMats = componentsInChildren.ToDictionary(
			render => render,
			render => render.material
		);
		Log.LogDebug($"Setting mats to glitch material");
		foreach (var t in componentsInChildren)
		{
			t.material = glitch3DMaterial;
		}

		AudioController.Instance.PlaySound2D("broken_hum");
		UIManager.Instance.Effects.GetEffect<ScreenGlitchEffect>().SetIntensity(1f, 1f);
		Log.LogDebug($"Glitch sound");
		GlitchOutAssetEffect.PlayGlitchSound(playableCard.transform.position);
		TurnManager.Instance.Opponent.ModifySpawnedCard(playableCard);
		yield return BoardManager.Instance.TransitionAndResolveCreatedCard(
			playableCard,
			slotToSpawnIn,
			0f
		);
		Log.LogDebug($"Playing glitch in effect for [{cardInfo.name}], setting inactive first");
		playableCard.gameObject.SetActive(false);
		yield return new WaitForSeconds(0.1f);

		Log.LogDebug($"Setting active");
		playableCard.gameObject.SetActive(true);
		Log.LogDebug($"Setting mats back to original state");
		foreach (var renderer in componentsInChildren)
		{
			renderer.material = originalMats.GetValueSafe(renderer);
		}

		playableCard.RenderCard();

		Log.LogDebug($"Tween.Shake");
		Tween.Shake(
			playableCard.transform,
			playableCard.transform.localPosition,
			Vector3.one * 0.2f,
			0.5f,
			0f,
			Tween.LoopType.None,
			null,
			null,
			false
		);

		AddBonelordSnapAnim(playableCard);
		yield return new WaitForSeconds(1f);
	}

	private void AddBonelordSnapAnim(PlayableCard playableCard)
	{
		Log.LogDebug($"Spawning new sentry prefab for card [{playableCard.Info.displayedName}]");
		_bonelordSnapAnim = Instantiate(
				AssetUtils.GetPrefab<GameObject>("SkeletonArm_BoneLordSnap"),
				playableCard.transform
			)
		 .GetComponent<Animator>();
		_bonelordSnapAnim.runtimeAnimatorController = AssetConstants.SkeletonArmController;
		_bonelordSnapAnim.name = "SkeletonArm_BoneLordSnap";
		_bonelordSnapAnim.gameObject.AddComponent<AnimMethods>();
		_bonelordSnapAnim.gameObject.SetActive(false);
	}

	private IEnumerator CreateHornsInFarLeftAndRightLanes(List<CardSlot> oppSlots)
	{
		Log.LogInfo("[Grimora] Spawning Bonelord's Horns");
		yield return TextDisplayer.Instance.ShowUntilInput(
			"RISE MY ARMY! RIIIIIIIIIISE!",
			letterAnimation: TextDisplayer.LetterAnimation.WavyJitter
		);


		oppSlots.RemoveRange(
			1,
			ConfigHelper.HasIncreaseSlotsMod ? 3 : 2
		); // slot 1, slot 4 remain
		var leftAndRightQueueSlots = GetFarLeftAndFarRightQueueSlots();

		CardInfo bonelordsHorn = NameBoneLordsHorn.GetCardInfo();
		for (int i = 0; i < 2; i++)
		{
			yield return TurnManager.Instance.Opponent.QueueCard(bonelordsHorn, leftAndRightQueueSlots[i]);
			yield return oppSlots[i].CreateCardInSlot(bonelordsHorn, 0.2f);
			yield return new WaitForSeconds(0.25f);
		}
	}

	private List<CardSlot> GetFarLeftAndFarRightQueueSlots()
	{
		Log.LogInfo("[Grimora] GetFarLeftAndFarRightQueueSlots");
		var qSlots = BoardManager.Instance.GetQueueSlots();
		CardSlot farRightSlot = qSlots[ConfigHelper.HasIncreaseSlotsMod ? 4 : 3];
		return new List<CardSlot>
		{
			qSlots[0], farRightSlot
		};
	}
}
