using System.Collections;
using DiskCardGame;
using UnityEngine;
using static GrimoraMod.GrimoraPlugin;

namespace GrimoraMod;

public class GrimoraBossSequencer : GrimoraModBossBattleSequencer
{
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
		List<CardSlot> opponentQueuedSlots = BoardManager.Instance
			.GetSlots(getPlayerSlots: false)
			.FindAll((CardSlot x) => x.Card == null && !TurnManager.Instance.Opponent.QueuedSlots.Contains(x));
		Log.LogDebug($"[{GetType()}] Opponent Slots count [{opponentQueuedSlots.Count}]");
		if (opponentQueuedSlots.Count > 0)
		{
			ViewManager.Instance.SwitchToView(View.BossCloseup);
			TextDisplayer.Instance.PlayDialogueEvent(
				"GrimoraBossReanimate1",
				TextDisplayer.MessageAdvanceMode.Input
			);

			CardSlot slot = opponentQueuedSlots[UnityEngine.Random.Range(0, opponentQueuedSlots.Count)];
			yield return TurnManager.Instance.Opponent.QueueCard(card.Info, slot);
			yield return new WaitForSeconds(0.5f);
		}

		yield break;
	}

	public override bool RespondsToUpkeep(bool playerUpkeep)
	{
		return !playerUpkeep;
	}

	public override IEnumerator OnUpkeep(bool playerUpkeep)
	{
		RandomEx rnd = new RandomEx();
		if (rnd.NextBoolean())
		{
			TextDisplayer.Instance.ShowUntilInput("Only a few more turns before I can bring my army back...",
				letterAnimation: TextDisplayer.LetterAnimation.None);
		}

		yield break;
	}
}
