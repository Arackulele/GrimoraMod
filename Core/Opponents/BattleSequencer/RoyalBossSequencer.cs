using System.Collections;
using DiskCardGame;

namespace GrimoraMod
{
	public class RoyalBossSequencer : Part1BossBattleSequencer
	{
		public override Opponent.Type BossType => Opponent.Type.ProspectorBoss;

		public override StoryEvent DefeatedStoryEvent => StoryEvent.TutorialRunCompleted;

		public override EncounterData BuildCustomEncounter(CardBattleNodeData nodeData)
		{
			return new EncounterData()
			{
				opponentType = BaseBossExt.RoyalOpponent
			};
		}

		public override bool RespondsToUpkeep(bool playerUpkeep)
		{
			return playerUpkeep;
		}

		public override IEnumerator OnUpkeep(bool playerUpkeep)
		{
			var playerSlotsWithCards = CardSlotUtils.GetPlayerSlotsWithCards();
			if (playerSlotsWithCards.Count >= 1)
			{
				var card = playerSlotsWithCards[UnityEngine.Random.Range(0, playerSlotsWithCards.Count)].Card;
				card.AddTemporaryMod(new CardModificationInfo(Ability.Submerge));
			}

			yield break;
		}
	}
}