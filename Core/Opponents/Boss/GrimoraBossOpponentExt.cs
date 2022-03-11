using System.Collections;
using DiskCardGame;
using UnityEngine;
using static GrimoraMod.GrimoraPlugin;

namespace GrimoraMod;

public class GrimoraBossOpponentExt : BaseBossExt
{
	public override StoryEvent EventForDefeat => StoryEvent.PhotoDroneSeenInCabin;

	public override Type Opponent => GrimoraOpponent;

	public override string SpecialEncounterId => "GrimoraBoss";

	public override string DefeatedPlayerDialogue => "Thank you!";

	public override int StartingLives => 3;

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

		if (!ConfigHelper.Instance.isDevModeEnabled)
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
		if (!ConfigHelper.Instance.isDevModeEnabled)
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
	}

	public override void PlayTheme()
	{
		Log.LogDebug("Playing Grimora theme");
		AudioController.Instance.StopAllLoops();
		AudioController.Instance.SetLoopAndPlay("Grimoras_Theme", 1);
		AudioController.Instance.SetLoopVolumeImmediate(0f, 1);
		AudioController.Instance.SetLoopVolume(0.8f, 10f, 1);
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
		int secondGiantIndex = ConfigHelper.HasIncreaseSlotsMod
			? 4
			: 3;
		Log.LogInfo("[Grimora] Start of giants phase");
		var oppSlots = BoardManager.Instance.OpponentSlotsCopy;

		yield return TextDisplayer.Instance.ShowUntilInput(
			"BEHOLD, MY LATEST CREATIONS! THE TWIN GIANTS!",
			letterAnimation: TextDisplayer.LetterAnimation.WavyJitter
		);

		ViewManager.Instance.SwitchToView(View.OpponentQueue, false, true);

		SetSceneEffectsShownGrimora(GameColors.Instance.lightPurple);

		// mimics the moon phase
		Log.LogInfo("[Grimora] Creating first giant in slot");
		yield return CreateAndPlaceModifiedGiant("Otis", oppSlots[1]);
		yield return CreateAndPlaceModifiedGiant("Ephialtes", oppSlots[secondGiantIndex]);

		Log.LogInfo("[Grimora] Finished creating giants");

		if (ConfigHelper.HasIncreaseSlotsMod)
		{
			yield return BoardManager.Instance.CreateCardInSlot(NameObol.GetCardInfo(), oppSlots[2], 0.2f);
			CardSlot thirdLaneQueueSlot = BoardManager.Instance.GetQueueSlots()[2];
			yield return TurnManager.Instance.Opponent.QueueCard(NameObol.GetCardInfo(), thirdLaneQueueSlot);
		}

		yield return new WaitForSeconds(0.5f);
	}

	private IEnumerator CreateAndPlaceModifiedGiant(string giantName, CardSlot slotToSpawnIn)
	{
		Log.LogInfo("[Grimora] Creating modified Giant");
		PlayableCard playableGiant = CardSpawner.SpawnPlayableCard(NameGiant.GetCardInfo());
		playableGiant.SetIsOpponentCard();
		
		CardInfo infoGiant = NameGiant.GetCardInfo().Clone() as CardInfo;
		infoGiant.displayedName = giantName;
		infoGiant.abilities = new List<Ability> { Ability.Reach, GiantStrike.ability };
		infoGiant.specialAbilities.Add(GrimoraGiant.SpecialTriggeredAbility);
		infoGiant.Mods.Add(new CardModificationInfo(-1, 1));
		
		playableGiant.SetInfo(infoGiant);
		yield return BoardManager.Instance.TransitionAndResolveCreatedCard(playableGiant, slotToSpawnIn, 0.3f);
		yield return TextDisplayer.Instance.ShowUntilInput($"{giantName}!");
	}

	public IEnumerator StartBoneLordPhase()
	{
		Log.LogInfo("[Grimora] Start of Bonelord phase");
		AudioController.Instance.FadeOutLoop(3f);
		AudioController.Instance.StopAllLoops();
		AudioController.Instance.SetLoopAndPlay("Bone_Lords_Theme", 1);
		AudioController.Instance.SetLoopVolumeImmediate(0.1f, 1);
		AudioController.Instance.FadeInLoop(7f, 0.5f, 1);

		ViewManager.Instance.SwitchToView(View.DefaultUpwards, false, true);
		
		SetSceneEffectsShownGrimora(GrimoraColors.GrimoraBossCardLight);
		yield return new WaitForSeconds(0.1f);
		
		Log.LogDebug($"Spawning Bonelord effects");
		GameObject bonelordEffect = Instantiate(AssetUtils.GetPrefab<GameObject>("BonelordTableEffects"));
		yield return new WaitForSeconds(6f);
		
		var oppSlots = BoardManager.Instance.OpponentSlotsCopy;
		yield return TextDisplayer.Instance.ShowUntilInput(
			"LET THE BONE LORD COMMETH!",
			letterAnimation: TextDisplayer.LetterAnimation.WavyJitter
		);
		ViewManager.Instance.SwitchToView(View.OpponentQueue, false, true);

		int bonelordSlotIndex = ConfigHelper.HasIncreaseSlotsMod
			? 3
			: 2;
		Log.LogInfo("[Grimora] Creating Bonelord");
		yield return BoardManager.Instance.CreateCardInSlot(
			NameBonelord.GetCardInfo(),
			oppSlots[bonelordSlotIndex],
			0.75f
		);
		yield return new WaitForSeconds(0.25f);

		yield return CreateHornsInFarLeftAndRightLanes(oppSlots);
	}

	private IEnumerator CreateHornsInFarLeftAndRightLanes(List<CardSlot> oppSlots)
	{
		Log.LogInfo("[Grimora] Spawning Bone Lord's Horns");
		yield return TextDisplayer.Instance.ShowUntilInput(
			"RISE MY ARMY! RIIIIIIIIIISE!",
			letterAnimation: TextDisplayer.LetterAnimation.WavyJitter
		);


		oppSlots.RemoveRange(
			1,
			ConfigHelper.HasIncreaseSlotsMod
				? 3
				: 2
		); // slot 1, slot 4 remain
		var leftAndRightQueueSlots = GetFarLeftAndFarRightQueueSlots();

		CardInfo bonelordsHorn = CreateModifiedBonelordsHorn();
		for (int i = 0; i < 2; i++)
		{
			yield return TurnManager.Instance.Opponent.QueueCard(bonelordsHorn, leftAndRightQueueSlots[i]);
			yield return BoardManager.Instance.CreateCardInSlot(bonelordsHorn, oppSlots[i], 0.2f);
			yield return new WaitForSeconds(0.25f);
		}
	}

	private CardInfo CreateModifiedBonelordsHorn()
	{
		Log.LogInfo("[Grimora] Creating modified Bone Lords Horn");
		PlayableCard playableHorn = CardSpawner.SpawnPlayableCard(NameBoneLordsHorn.GetCardInfo());
		playableHorn.SetIsOpponentCard();
		
		CardInfo infoHorn = NameBoneLordsHorn.GetCardInfo().Clone() as CardInfo;
		infoHorn.Mods.Add(new CardModificationInfo { attackAdjustment = 2 });
		infoHorn.abilities.Remove(Ability.QuadrupleBones);
		infoHorn.iceCubeParams.creatureWithin.abilities.Add(Ability.BuffNeighbours);
		
		playableHorn.SetInfo(infoHorn);
		return infoHorn;
	}

	private List<CardSlot> GetFarLeftAndFarRightQueueSlots()
	{
		Log.LogInfo("[Grimora] GetFarLeftAndFarRightQueueSlots");
		var qSlots = BoardManager.Instance.GetQueueSlots();
		CardSlot farRightSlot = qSlots[ConfigHelper.HasIncreaseSlotsMod
			? 4
			: 3];
		return new List<CardSlot>
		{
			qSlots[0], farRightSlot
		};
	}
}
