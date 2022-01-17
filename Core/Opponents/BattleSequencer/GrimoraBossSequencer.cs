using System.Collections;
using DiskCardGame;
using UnityEngine;
using static GrimoraMod.GrimoraPlugin;

namespace GrimoraMod;

public class GrimoraBossSequencer : Part1BossBattleSequencer
{
	public override Opponent.Type BossType => BaseBossExt.GrimoraOpponent;

	public override StoryEvent DefeatedStoryEvent => StoryEvent.TutorialRunCompleted;

	public override EncounterData BuildCustomEncounter(CardBattleNodeData nodeData)
	{
		return new EncounterData
		{
			opponentType = BossType
		};
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
		var opponentSlots = CardSlotUtils.GetOpponentSlotsWithCards();
		Log.LogDebug($"[{GetType()}] Opponent Slots count [{opponentSlots.Capacity}]");
		if (opponentSlots.Count > 0)
		{
			ViewManager.Instance.SwitchToView(View.BossCloseup);
			TextDisplayer.Instance.PlayDialogueEvent(
				"GrimoraBossReanimate1",
				TextDisplayer.MessageAdvanceMode.Input
			);

			BoardManager.Instance.QueueCardForSlot(
				card,
				opponentSlots[UnityEngine.Random.RandomRangeInt(0, opponentSlots.Count)]
			);
		}

		yield return TurnManager.Instance.Opponent.QueueCard(
			card.Info,
			BoardManager.Instance.OpponentSlotsCopy[Random.Range(0, 3)]
		);
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