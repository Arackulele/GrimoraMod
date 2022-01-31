using System.Collections;
using DiskCardGame;
using Sirenix.Utilities;
using UnityEngine;
using static GrimoraMod.GrimoraPlugin;

namespace GrimoraMod;

public class GrimoraBossOpponentExt : BaseBossExt
{
	public const string SpecialId = "GrimoraBoss";

	public override StoryEvent EventForDefeat => StoryEvent.PhotoDroneSeenInCabin;

	public override Type Opponent => GrimoraOpponent;

	public override string DefeatedPlayerDialogue => "Thank you!";

	public override int StartingLives => 3;

	private static void SetSceneEffectsShownGrimora()
	{
		Color brightBlue = GameColors.Instance.brightBlue;
		brightBlue.a = 0.5f;
		TableVisualEffectsManager.Instance.ChangeTableColors(
			GameColors.Instance.darkPurple,
			GameColors.Instance.purple,
			GameColors.Instance.purple,
			GameColors.Instance.darkPurple,
			GameColors.Instance.darkPurple,
			GameColors.Instance.purple,
			GameColors.Instance.purple,
			GameColors.Instance.darkPurple,
			GameColors.Instance.purple
		);
	}


	public override IEnumerator IntroSequence(EncounterData encounter)
	{
		AudioController.Instance.SetLoopVolume(1f, 0.5f);
		yield return new WaitForSeconds(1f);

		SetSceneEffectsShownGrimora();

		yield return TextDisplayer.Instance.PlayDialogueEvent(
			"LeshyBossIntro1",
			TextDisplayer.MessageAdvanceMode.Input
		);
		yield return new WaitForSeconds(0.75f);

		// Log.LogDebug($"[{GetType()}] Calling base IntroSequence, this creates and sets the candle skull");
		yield return base.IntroSequence(encounter);

		ViewManager.Instance.SwitchToView(View.BossSkull, immediate: false, lockAfter: true);

		yield return new WaitForSeconds(0.25f);
		yield return TextDisplayer.Instance.PlayDialogueEvent(
			"LeshyBossAddCandle",
			TextDisplayer.MessageAdvanceMode.Input
		);
		yield return new WaitForSeconds(0.4f);

		Log.LogDebug($"Calling bossSkull.EnterHand();");
		bossSkull.EnterHand();

		yield return new WaitForSeconds(2f);
		ViewManager.Instance.SwitchToView(View.Default, lockAfter: false);
	}

	public override IEnumerator StartNewPhaseSequence()
	{
		base.TurnPlan.Clear();

		Log.LogDebug($"[GrimoraBoss] Clearing board");
		yield return base.ClearBoard();

		Log.LogDebug($"[GrimoraBoss] Clearing queue");
		yield return base.ClearQueue();

		yield return new WaitForSeconds(0.5f);

		switch (this.NumLives)
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

		ViewManager.Instance.SwitchToView(View.Default, false, false);

		yield break;
	}

	private static IEnumerator StartPlayerCardWeakeningPhase()
	{
		var playerCardsThatAreValidToWeaken
			= CardSlotUtils
				.GetPlayerSlotsWithCards()
				.Where(slot => slot.Card.Health > 1)
				.Select(slot => slot.Card)
				.ToList();
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

	private static IEnumerator StartSpawningGiantsPhase()
	{
		var oppSlots = BoardManager.Instance.OpponentSlotsCopy;

		yield return TextDisplayer.Instance.ShowUntilInput(
			"BEHOLD, MY LATEST CREATIONS! THE TWIN GIANTS!",
			letterAnimation: TextDisplayer.LetterAnimation.WavyJitter
		);

		ViewManager.Instance.SwitchToView(View.OpponentQueue, immediate: false, lockAfter: true);

		CardInfo modifiedGiant = CardLoader.GetCardByName(NameGiant);
		modifiedGiant.abilities = new List<Ability>() { Ability.AllStrike };
		modifiedGiant.specialAbilities.Add(GrimoraGiant.NewSpecialAbility.specialTriggeredAbility);

		yield return BoardManager.Instance.CreateCardInSlot(modifiedGiant, oppSlots[1], 0.2f);
		yield return new WaitForSeconds(0.5f);
		yield return BoardManager.Instance.CreateCardInSlot(modifiedGiant, oppSlots[3], 0.2f);
		yield return new WaitForSeconds(0.5f);
	}

	public IEnumerator StartBoneLordPhase()
	{
		var oppSlots = BoardManager.Instance.OpponentSlotsCopy;

		yield return TextDisplayer.Instance.ShowUntilInput("LET THE BONE LORD COMMETH!",
			letterAnimation: TextDisplayer.LetterAnimation.WavyJitter);

		ViewManager.Instance.SwitchToView(View.Board);

		yield return BoardManager.Instance.CreateCardInSlot(
			CardLoader.GetCardByName(NameBonelord), oppSlots[2], 0.2f
		);
		yield return new WaitForSeconds(0.25f);

		oppSlots.RemoveAt(2);

		yield return TextDisplayer.Instance.ShowUntilInput(
			"RISE MY ARMY! RIIIIIIIIIISE!",
			letterAnimation: TextDisplayer.LetterAnimation.WavyJitter
		);

		foreach (CardSlot cardSlot in oppSlots)
		{
			yield return BoardManager.Instance.CreateCardInSlot(
				CardLoader.GetCardByName(NameSkeletonArmy), cardSlot, 0.2f
			);

			yield return new WaitForSeconds(0.25f);
		}
	}
}
