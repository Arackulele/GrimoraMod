using System.Collections;
using DiskCardGame;
using Sirenix.Utilities;
using UnityEngine;
using static GrimoraMod.GrimoraPlugin;

namespace GrimoraMod;

public class GrimoraModGrimoraBossSequencer : GrimoraModBossBattleSequencer
{
	private readonly RandomEx _rng = new();

	private bool playedDeathTouchDialogue;

	public override Opponent.Type BossType => BaseBossExt.GrimoraOpponent;

	public override EncounterData BuildCustomEncounter(CardBattleNodeData nodeData)
	{
		return new EncounterData
		{
			opponentType = BossType
		};
	}

	public override IEnumerator GameEnd(bool playerWon)
	{
		if (playerWon)
		{
			if (!DialogueEventsData.EventIsPlayed("FinaleGrimoraBattleWon"))
			{
				Log.LogDebug($"FinaleGrimoraBattleWon has not played yet, playing now.");

				ViewManager.Instance.Controller.LockState = ViewLockState.Locked;
				yield return new WaitForSeconds(0.5f);
				yield return TextDisplayer.Instance.PlayDialogueEvent(
					"FinaleGrimoraBattleWon", TextDisplayer.MessageAdvanceMode.Input
				);
			}
		}
		else
		{
			yield return base.GameEnd(false);
		}
	}

	public override IEnumerator OpponentUpkeep()
	{
		if (!playedDeathTouchDialogue &&
		    BoardManager.Instance.GetSlots(true)
			    .Exists(x => x.Card != null && x.Card.HasAbility(Ability.Deathtouch))
		    && BoardManager.Instance.GetSlots(false)
			    .Exists(x =>
				    x.Card != null &&
				    x.Card.Info.SpecialAbilities.Contains(GrimoraGiant.NewSpecialAbility.specialTriggeredAbility))
		   )
		{
			yield return new WaitForSeconds(0.5f);
			yield return TextDisplayer.Instance.ShowUntilInput(
				"DEATH TOUCH WON'T HELP YOU HERE DEAR." +
				"\nI MADE THESE GIANTS SPECIAL, IMMUNE TO QUITE A FEW DIFFERENT TRICKS!"
			);
			playedDeathTouchDialogue = true;
		}
	}

	public override IEnumerator PlayerUpkeep()
	{
		if (!DialogueEventsData.EventIsPlayed("FinaleGrimoraBattleStart"))
		{
			yield return new WaitForSeconds(0.5f);
			yield return TextDisplayer.Instance.PlayDialogueEvent(
				"FinaleGrimoraBattleStart", TextDisplayer.MessageAdvanceMode.Input
			);
		}
	}

	public override bool RespondsToOtherCardDie(
		PlayableCard card, CardSlot deathSlot, bool fromCombat, PlayableCard killer
	)
	{
		return !card.OpponentCard;
	}

	public override IEnumerator OnOtherCardDie(
		PlayableCard card, CardSlot deathSlot, bool fromCombat, PlayableCard killer
	)
	{
		Log.LogDebug($"[{GetType()}] Other Card [{card.Info.name}] is happening");
		List<CardSlot> opponentQueuedSlots = BoardManager.Instance.GetQueueSlots();
		Log.LogDebug($"[{GetType()}] Opponent Slots count [{opponentQueuedSlots.Count}]");
		if (!opponentQueuedSlots.IsNullOrEmpty())
		{
			ViewManager.Instance.SwitchToView(View.BossCloseup);
			TextDisplayer.Instance.PlayDialogueEvent("GrimoraBossReanimate1", TextDisplayer.MessageAdvanceMode.Input);

			CardSlot slot = opponentQueuedSlots[UnityEngine.Random.Range(0, opponentQueuedSlots.Count)];
			yield return TurnManager.Instance.Opponent.QueueCard(card.Info, slot);
			yield return new WaitForSeconds(0.5f);
		}
	}

	public override bool RespondsToUpkeep(bool playerUpkeep)
	{
		return !playerUpkeep;
	}

	public override IEnumerator OnUpkeep(bool playerUpkeep)
	{
		if (_rng.NextBoolean())
		{
			TextDisplayer.Instance.ShowUntilInput("Only a few more turns before I can bring my army back...",
				letterAnimation: TextDisplayer.LetterAnimation.None);
		}

		yield break;
	}
}
