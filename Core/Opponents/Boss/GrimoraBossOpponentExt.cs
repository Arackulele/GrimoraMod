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

	private static void SetSceneEffectsShownGrimora()
	{
		Color purple = GameColors.Instance.purple;
		Color darkPurple = GameColors.Instance.darkPurple;
		TableVisualEffectsManager.Instance.ChangeTableColors(
			darkPurple,
			GrimoraColors.GrimoraBossCardLight,
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

		yield return TextDisplayer.Instance.PlayDialogueEvent(
			"RoyalBossPreIntro",
			TextDisplayer.MessageAdvanceMode.Input
		);

		yield return TextDisplayer.Instance.PlayDialogueEvent(
			"LeshyBossIntro1",
			TextDisplayer.MessageAdvanceMode.Input
		);

		yield return base.IntroSequence(encounter);

		ViewManager.Instance.SwitchToView(View.BossSkull, false, true);

		yield return TextDisplayer.Instance.PlayDialogueEvent(
			"LeshyBossAddCandle",
			TextDisplayer.MessageAdvanceMode.Input
		);
		yield return new WaitForSeconds(0.4f);

		bossSkull.EnterHand();

		SetSceneEffectsShownGrimora();

		yield return new WaitForSeconds(2f);
		ViewManager.Instance.SwitchToView(View.Default);

		if (ConfigHelper.HasIncreaseSlotsMod)
		{
			yield return TextDisplayer.Instance.ShowUntilInput("OH? FIVE LANES? HOW BOLD.");
		}

		ViewManager.Instance.Controller.LockState = ViewLockState.Unlocked;
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

		ViewManager.Instance.SwitchToView(View.Default);
	}

	private IEnumerator StartSpawningGiantsPhase()
	{
		Log.LogInfo("[Grimora] Start of giants phase");
		var oppSlots = BoardManager.Instance.OpponentSlotsCopy;

		yield return TextDisplayer.Instance.ShowUntilInput(
			"BEHOLD, MY LATEST CREATIONS! THE TWIN GIANTS!",
			letterAnimation: TextDisplayer.LetterAnimation.WavyJitter
		);

		ViewManager.Instance.SwitchToView(View.OpponentQueue, false, true);

		// mimics the moon phase
		CardInfo giantOtis = CreateModifiedGiant("Otis");
		CardInfo giantEphialtes = CreateModifiedGiant("Ephialtes");
		Log.LogInfo("[Grimora] Creating first giant in slot");
		yield return BoardManager.Instance.CreateCardInSlot(giantOtis, oppSlots[1], 0.3f);
		yield return new WaitForSeconds(0.5f);
		if (ConfigHelper.Instance.HasIncreaseSlotsMod)
		{
			yield return TextDisplayer.Instance.ShowUntilInput("OH? FIVE LANES? HOW BOLD.");
			yield return BoardManager.Instance.CreateCardInSlot(giantEphialtes, oppSlots[4], 0.3f);

			yield return BoardManager.Instance.CreateCardInSlot(NameObol.GetCardInfo(), oppSlots[2], 0.2f);
			CardSlot thirdLaneQueueSlot = BoardManager.Instance.GetQueueSlots()[2];
			yield return TurnManager.Instance.Opponent.QueueCard(NameObol.GetCardInfo(), thirdLaneQueueSlot);
		}
		else
		{
			yield return BoardManager.Instance.CreateCardInSlot(giantEphialtes, oppSlots[3], 0.3f);
		}

		Log.LogInfo("[Grimora] Finished creating giants");

		yield return new WaitForSeconds(0.5f);
		ViewManager.Instance.Controller.LockState = ViewLockState.Unlocked;
	}

	private CardInfo CreateModifiedGiant(string giantName)
	{
		Log.LogInfo("[Grimora] Creating modified Giant");
		CardInfo modifiedGiant = NameGiant.GetCardInfo();
		modifiedGiant.displayedName = giantName;
		modifiedGiant.abilities = new List<Ability> { GiantStrike.ability, Ability.Reach };
		modifiedGiant.specialAbilities.Add(GrimoraGiant.NewSpecialAbility.specialTriggeredAbility);
		modifiedGiant.baseAttack = 1;
		modifiedGiant.baseHealth = 8;
		return modifiedGiant;
	}

	public IEnumerator StartBoneLordPhase()
	{
		Log.LogInfo("[Grimora] Start of Bonelord phase");
		AudioController.Instance.FadeOutLoop(3f);
		AudioController.Instance.StopAllLoops();
		AudioController.Instance.SetLoopAndPlay("Bone_Lords_Theme", 1);
		AudioController.Instance.SetLoopVolumeImmediate(0.1f, 1);
		AudioController.Instance.FadeInLoop(7f, 0.5f, 1);

		var oppSlots = BoardManager.Instance.OpponentSlotsCopy;
		yield return TextDisplayer.Instance.ShowUntilInput(
			"LET THE BONE LORD COMMETH!",
			letterAnimation: TextDisplayer.LetterAnimation.WavyJitter
		);
		ViewManager.Instance.SwitchToView(View.OpponentQueue, false, true);

		int bonelordSlotIndex = ConfigHelper.Instance.HasIncreaseSlotsMod
			? 3
			: 2;
		Log.LogInfo("[Grimora] Creating Bonelord");
		yield return BoardManager.Instance.CreateCardInSlot(
			CreateModifiedBonelord(),
			oppSlots[bonelordSlotIndex],
			0.75f
		);
		yield return new WaitForSeconds(0.25f);

		yield return CreateHornsInFarLeftAndRightLanes(oppSlots);

		ViewManager.Instance.Controller.LockState = ViewLockState.Unlocked;
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
			ConfigHelper.Instance.HasIncreaseSlotsMod
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

	private CardInfo CreateModifiedBonelord()
	{
		Log.LogInfo("[Grimora] Creating modified Bonelord");
		CardInfo bonelord = NameBonelord.GetCardInfo();
		CardModificationInfo mod = new CardModificationInfo
		{
			abilities = new List<Ability> { GiantStrike.ability, Ability.Reach },
			specialAbilities = new List<SpecialTriggeredAbility> { GrimoraGiant.NewSpecialAbility.specialTriggeredAbility }
		};

		bonelord.traits.Add(Trait.Giant);
		bonelord.Mods.Add(mod);

		return bonelord;
	}

	private CardInfo CreateModifiedBonelordsHorn()
	{
		Log.LogInfo("[Grimora] Creating modified Bone Lords Horn");
		CardInfo bonelordsHorn = NameBoneLordsHorn.GetCardInfo();
		bonelordsHorn.Mods.Add(new CardModificationInfo { attackAdjustment = 2 });
		bonelordsHorn.abilities.Remove(Ability.QuadrupleBones);
		return bonelordsHorn;
	}

	private List<CardSlot> GetFarLeftAndFarRightQueueSlots()
	{
		Log.LogInfo("[Grimora] GetFarLeftAndFarRightQueueSlots");
		var qSlots = BoardManager.Instance.GetQueueSlots();
		CardSlot farRightSlot = qSlots[ConfigHelper.Instance.HasIncreaseSlotsMod
			? 4
			: 3];
		return new List<CardSlot>
		{
			qSlots[0], farRightSlot
		};
	}
}
