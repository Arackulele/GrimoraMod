using System.Collections;
using DiskCardGame;

namespace GrimoraMod
{
	public class KayceeBossSequencer : Part1BossBattleSequencer
	{
		public override Opponent.Type BossType => Opponent.Type.ProspectorBoss;

		public override StoryEvent DefeatedStoryEvent => StoryEvent.TutorialRunCompleted;

		public override EncounterData BuildCustomEncounter(CardBattleNodeData nodeData)
		{
			return new EncounterData()
			{
				opponentType = BaseBossExt.KayceeOpponent
			};
		}

		public override bool RespondsToUpkeep(bool playerUpkeep)
		{
			return playerUpkeep;
		}

		public int freezeCounter = 0;

		public override IEnumerator OnUpkeep(bool playerUpkeep)
		{
			if (++this.freezeCounter == 3)
			{
				StartCoroutine(TextDisplayer.Instance.ShowUntilInput("Freeze!"));
				foreach (var slot in CardSlotUtils.GetPlayerSlotsWithCards())
				{
					slot.Card.Anim.StrongNegationEffect();
					slot.Card.Anim.StrongNegationEffect();
					slot.Card.Anim.StrongNegationEffect();
					slot.Card.Anim.StrongNegationEffect();
					slot.Card.Anim.StrongNegationEffect();
					slot.Card.AddTemporaryMod(new CardModificationInfo(-1, 0));
					
					freezeCounter = 0;
				}
			}

			yield break;
		}
	}
}