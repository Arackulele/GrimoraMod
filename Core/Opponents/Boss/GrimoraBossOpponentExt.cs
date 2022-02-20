using System.Collections;
using DiskCardGame;
using Sirenix.Utilities;
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
		Color cardLightColorPurple = new Color(0.55f, 0.1f, 0.72f, 1);
		TableVisualEffectsManager.Instance.ChangeTableColors(
			darkPurple,
			cardLightColorPurple,
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
		AudioController.Instance.SetLoopVolume(1f, 0.5f);
		yield return new WaitForSeconds(1f);

		yield return TextDisplayer.Instance.PlayDialogueEvent(
			"RoyalBossPreIntro",
			TextDisplayer.MessageAdvanceMode.Input
		);


		yield return TextDisplayer.Instance.PlayDialogueEvent(
			"LeshyBossIntro1",
			TextDisplayer.MessageAdvanceMode.Input
		);

		// Log.LogDebug($"[{GetType()}] Calling base IntroSequence, this creates and sets the candle skull");
		yield return base.IntroSequence(encounter);

		ViewManager.Instance.SwitchToView(View.BossSkull, false, true);


		yield return TextDisplayer.Instance.PlayDialogueEvent(
			"LeshyBossAddCandle",
			TextDisplayer.MessageAdvanceMode.Input
		);
		yield return new WaitForSeconds(0.4f);

		bossSkull.EnterHand();

		SetSceneEffectsShownGrimora();

		PlayTheme();

		yield return new WaitForSeconds(2f);
		ViewManager.Instance.SwitchToView(View.Default, lockAfter: false);
	}

	public override void PlayTheme()
	{
		Log.LogDebug($"Playing Grimora theme");
		AudioController.Instance.FadeOutLoop(3f);
		AudioController.Instance.StopAllLoops();
		AudioController.Instance.SetLoopAndPlay("Risen_Again", 1);
		AudioController.Instance.SetLoopVolumeImmediate(0f, 1);
		AudioController.Instance.FadeInLoop(5f, 0.75f, 1);
	}

	public override IEnumerator StartNewPhaseSequence()
	{
		TurnPlan.Clear();
		yield return ClearBoard();
		yield return ClearQueue();

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
				yield return StartPlayerCardWeakeningPhase();

				yield return StartSpawningGiantsPhase();

				break;
			}
		}

		ViewManager.Instance.SwitchToView(View.Default);
	}

	private IEnumerator StartPlayerCardWeakeningPhase()
	{
		var playerCardsThatAreValidToWeaken
			= BoardManager.Instance.GetPlayerCards(pCard => pCard.Health > 1).ToList();
		if (!playerCardsThatAreValidToWeaken.IsNullOrEmpty())
		{
			yield return TextDisplayer.Instance.ShowUntilInput(
				"I WILL MAKE YOU WEAK!",
				letterAnimation: TextDisplayer.LetterAnimation.WavyJitter
			);

			ViewManager.Instance.SwitchToView(View.Board);

			foreach (var playableCard in playerCardsThatAreValidToWeaken)
			{
				int attack = playableCard.Attack == 0 ? 0 : -playableCard.Attack + 1;
				playableCard.AddTemporaryMod(new CardModificationInfo(attack, -playableCard.Health + 1));
				playableCard.Anim.StrongNegationEffect();
				yield return new WaitForSeconds(0.25f);
				playableCard.Anim.StrongNegationEffect();
			}

			yield return new WaitForSeconds(0.75f);
		}
	}

	private IEnumerator StartSpawningGiantsPhase()
	{
		var oppSlots = BoardManager.Instance.OpponentSlotsCopy;

		yield return TextDisplayer.Instance.ShowUntilInput(
			"BEHOLD, MY LATEST CREATIONS! THE TWIN GIANTS!",
			letterAnimation: TextDisplayer.LetterAnimation.WavyJitter
		);

		ViewManager.Instance.SwitchToView(View.OpponentQueue, false, true);

		// mimics the moon phase
		CardInfo modifiedGiant = CreateModifiedGiant();
		yield return BoardManager.Instance.CreateCardInSlot(modifiedGiant, oppSlots[1], 0.3f);
		yield return new WaitForSeconds(0.5f);
		yield return BoardManager.Instance.CreateCardInSlot(modifiedGiant, oppSlots[3], 0.3f);
		yield return new WaitForSeconds(0.5f);
	}

	private CardInfo CreateModifiedGiant()
	{
		CardInfo modifiedGiant = NameGiant.GetCardInfo();
		modifiedGiant.abilities = new List<Ability>() { GiantStrike.ability, Ability.Reach };
		modifiedGiant.specialAbilities.Add(GrimoraGiant.NewSpecialAbility.specialTriggeredAbility);
		return modifiedGiant;
	}

	public IEnumerator StartBoneLordPhase()
	{
		AudioController.Instance.FadeOutLoop(3f);
		AudioController.Instance.StopAllLoops();


		AudioController.Instance.SetLoopAndPlay("Bone_Lords_Theme");
		AudioController.Instance.SetLoopVolumeImmediate(0.1f);
		AudioController.Instance.FadeInLoop(7f, 0.4f, default(int));

		var oppSlots = BoardManager.Instance.OpponentSlotsCopy;
		yield return TextDisplayer.Instance.ShowUntilInput(
			"LET THE BONE LORD COMMETH!",
			letterAnimation: TextDisplayer.LetterAnimation.WavyJitter
		);
		ViewManager.Instance.SwitchToView(View.OpponentQueue, false, true);

		yield return BoardManager.Instance.CreateCardInSlot(CreateModifiedBonelord(), oppSlots[2], 0.75f);
		yield return new WaitForSeconds(0.25f);

		yield return TextDisplayer.Instance.ShowUntilInput(
			"RISE MY ARMY! RIIIIIIIIIISE!",
			letterAnimation: TextDisplayer.LetterAnimation.WavyJitter
		);


		oppSlots.RemoveRange(1, 2); // slot 1, slot 4 remain
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
		CardInfo bonelord = NameBonelord.GetCardInfo();
		CardModificationInfo mod = new CardModificationInfo()
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
		CardInfo bonelordsHorn = NameBoneLordsHorn.GetCardInfo();
		bonelordsHorn.mods.Add(new CardModificationInfo() { attackAdjustment = 2 });
		bonelordsHorn.abilities.Remove(Ability.QuadrupleBones);
		return bonelordsHorn;
	}

	private List<CardSlot> GetFarLeftAndFarRightQueueSlots()
	{
		var qSlots = BoardManager.Instance.GetQueueSlots();
		return new List<CardSlot> { qSlots[0], qSlots[3] };
	}
}
