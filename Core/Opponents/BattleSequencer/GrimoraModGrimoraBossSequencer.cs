using System.Collections;
using DiskCardGame;
using GrimoraMod.Saving;
using InscryptionAPI.Card;
using InscryptionAPI.Encounters;
using InscryptionAPI.Helpers.Extensions;
using UnityEngine;
using static GrimoraMod.GrimoraPlugin;

namespace GrimoraMod;

public class GrimoraModGrimoraBossSequencer : GrimoraModBossBattleSequencer
{
	public new static readonly SpecialSequenceManager.FullSpecialSequencer FullSequencer = SpecialSequenceManager.Add(
		GUID,
		nameof(GrimoraModGrimoraBossSequencer),
		typeof(GrimoraModGrimoraBossSequencer)
	);

	private bool _playedDialogueDeathTouch;

	private bool _playedDialoguePossessive;

	public override Opponent.Type BossType => GrimoraBossOpponentExt.FullOpponent.Id;

	public override IEnumerator GameEnd(bool playerWon)
	{
		if (playerWon)
		{

			if (SaveFile.IsAscension)
			{
				//logic for calling Win Screen
				yield return TextDisplayer.Instance.ShowUntilInput("Wonderful! I am pleasantly surprised by your triumph against me!");
				yield return TextDisplayer.Instance.ShowUntilInput("...But it seems i cannot move on just yet.");
				yield return TextDisplayer.Instance.ShowUntilInput("Goodbye, Luke.");


				AscensionMenuScreens.ReturningFromSuccessfulRun = true;
				AscensionMenuScreens.ReturningFromFailedRun = false;

				AscensionStatsData.TryIncrementStat(AscensionStat.Type.Victories);

				if (!EventManagement.HasSeenCredits)
				{ 
				EventManagement.HasSeenCredits = true;
				SceneLoader.Load("Ascension_Credits");
				}
				else SceneLoader.Load("Ascension_Configure");

				//SceneLoader.Load("Ascension_Configure");

			}
			else
			{

				if (!DialogueEventsData.EventIsPlayed("FinaleGrimoraBattleWon"))
				{
					Log.LogDebug($"FinaleGrimoraBattleWon has not played yet, playing now.");

					ViewManager.Instance.SetViewLocked();
					yield return new WaitForSeconds(0.5f);
					yield return TextDisplayer.Instance.PlayDialogueEvent(
						"FinaleGrimoraBattleWon",
						TextDisplayer.MessageAdvanceMode.Input
					);
				}

				yield return new WaitForSeconds(0.5f);

				if (ConfigHelper.Instance.IsEndlessModeEnabled)
				{
					Log.LogInfo($"Player won against Grimora! Now to win again, endlessly...");
					yield return TextDisplayer.Instance.ShowUntilInput("Wonderful! I am pleasantly surprised by your triumph against me!");
					yield return TextDisplayer.Instance.ShowUntilInput("...You wish to continue? Endlessly? Splendid!");
					yield return TextDisplayer.Instance.ShowUntilInput("Please, take a card of your choosing.");
				}
				else
				{
					Log.LogInfo($"Player won against Grimora! Resetting run...");
					GrimoraSaveManager.ResetRun();
					GrimoraSaveUtil.IsGrimoraModRun = false;
					FinaleDeletionWindowManager.instance.mainWindow.gameObject.SetActive(true);
					yield return ((GrimoraGameFlowManager)GameFlowManager.Instance).EndSceneSequence();

					//if (SaveManager.saveFile.currentRun.currency == 10);
					//if (RunState.Run.currency == 1) ;
				}
			}

		}
		else
		{
			yield return base.GameEnd(false);
		}
	}


	private bool SlotContainsTwinGiant(CardSlot cardSlot)
	{
		return (cardSlot.HasCard(NameGiantEphialtes) || cardSlot.HasCard(NameGiantOtis)) && cardSlot.Card.IsGrimoraGiant();
	}

	public override IEnumerator OpponentUpkeep()
	{
		if (!_playedDialogueDeathTouch
		 && BoardManager.Instance.PlayerSlotsCopy.Exists(slot => slot.Card && slot.Card.HasAbility(Ability.Deathtouch))
		 && BoardManager.Instance.OpponentSlotsCopy.Exists(SlotContainsTwinGiant)
		)
		{
			yield return new WaitForSeconds(0.5f);
			yield return TextDisplayer.Instance.ShowUntilInput(
				"DEATH TOUCH WON'T HELP YOU HERE DEAR."
			+ "\nI MADE THESE GIANTS SPECIAL, IMMUNE TO QUITE A FEW DIFFERENT TRICKS!"
			);
			_playedDialogueDeathTouch = true;
		}
		else if (!_playedDialoguePossessive
		      && BoardManager.Instance.PlayerSlotsCopy.Exists(slot => slot.Card && slot.Card.HasAbility(Possessive.ability))
		      && BoardManager.Instance.OpponentSlotsCopy.Exists(slot => slot.HasCard(NameBonelord))
		)
		{
			yield return new WaitForSeconds(0.5f);
			yield return TextDisplayer.Instance.ShowUntilInput("THE BONELORD CANNOT BE POSSESSED!");
			_playedDialoguePossessive = true;
		}
	}

	public override IEnumerator PlayerUpkeep()
	{
		if (!DialogueEventsData.EventIsPlayed("FinaleGrimoraBattleStart"))
		{
			yield return new WaitForSeconds(0.5f);
			yield return TextDisplayer.Instance.PlayDialogueEvent(
				"FinaleGrimoraBattleStart",
				TextDisplayer.MessageAdvanceMode.Input
			);
		}
	}

	public override bool RespondsToOtherCardDie(
		PlayableCard card,
		CardSlot deathSlot,
		bool fromCombat,
		PlayableCard killer
	)
	{
		bool isPhaseOne = card.IsPlayerCard() && TurnManager.Instance.Opponent.NumLives == 3;
		return isPhaseOne;
	}

	private bool _willReanimateCardThatDied = false;

	public override IEnumerator OnOtherCardDie(
		PlayableCard card,
		CardSlot deathSlot,
		bool fromCombat,
		PlayableCard killer
	)
	{
		List<CardSlot> opponentQueuedSlots = BoardManager.Instance.GetQueueSlots();
		if (opponentQueuedSlots.IsNotEmpty() && _willReanimateCardThatDied)
		{
			ViewManager.Instance.SwitchToView(View.BossCloseup);
			yield return TextDisplayer.Instance.PlayDialogueEvent(
				"GrimoraBossReanimate1",
				TextDisplayer.MessageAdvanceMode.Input
			);

			CardSlot slot = opponentQueuedSlots.GetRandomItem();

			if (card != null && card.HasAbility(Haunter.ability)) {

				yield return TextDisplayer.Instance.ShowUntilInput("YOUR WEAK SPIRITS SHALL NOT HAUNT ME NO MORE.");

				yield return TurnManager.Instance.Opponent.QueueCard(NameVengefulSpirit.GetCardInfo(), slot);
				//else yield return TurnManager.Instance.Opponent.QueueCard(CardLoader.GetCardByName(card.name), slot);
			}
			else yield return TurnManager.Instance.Opponent.QueueCard(card.Info, slot);
			_willReanimateCardThatDied = false;
			yield return new WaitForSeconds(0.5f);
		}
		else { _willReanimateCardThatDied = true; }
	}
}
