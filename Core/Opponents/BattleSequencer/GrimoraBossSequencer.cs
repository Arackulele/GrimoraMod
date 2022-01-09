using System.Collections;
using DiskCardGame;
using UnityEngine;

namespace GrimoraMod
{
	public class GrimoraBossSequencer : Part1BossBattleSequencer
	{
		public override Opponent.Type BossType => Opponent.Type.ProspectorBoss;

		public override StoryEvent DefeatedStoryEvent => StoryEvent.TutorialRunCompleted;

		public override EncounterData BuildCustomEncounter(CardBattleNodeData nodeData)
		{
			return new EncounterData
			{
				opponentType = BaseBossExt.GrimoraOpponent
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
			yield return TurnManager.Instance.Opponent.QueueCard(
				card.Info,
				BoardManager.Instance.OpponentSlotsCopy[Random.Range(0, 3)]
			);
		}

		public override bool RespondsToUpkeep(bool playerUpkeep)
		{
			return playerUpkeep;
		}

		public override IEnumerator OnUpkeep(bool playerUpkeep)
		{
			var playerSlots = CardSlotUtils.GetPlayerSlotsWithCards();
			if (playerSlots.Count >= 1)
			{
				var card = playerSlots[Random.Range(0, playerSlots.Count)].Card;
			}

			yield break;
		}
	}
}