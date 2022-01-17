using System.Collections;
using System.Linq;
using DiskCardGame;

namespace GrimoraMod;

public class KayceeBossSequencer : Part1BossBattleSequencer
{
	public override Opponent.Type BossType => BaseBossExt.KayceeOpponent;

	public override StoryEvent DefeatedStoryEvent => StoryEvent.TutorialRunCompleted;

	public override EncounterData BuildCustomEncounter(CardBattleNodeData nodeData)
	{
		return new EncounterData()
		{
			opponentType = BossType
		};
	}

	public override bool RespondsToUpkeep(bool playerUpkeep)
	{
		return playerUpkeep;
	}

	public int freezeCounter = 0;

	public override IEnumerator OnUpkeep(bool playerUpkeep)
	{
		var playerSlotsWithCards = CardSlotUtils.GetPlayerSlotsWithCards();
		if (playerSlotsWithCards.Capacity > 0)
		{
			if (++freezeCounter == 3)
			{
				StartCoroutine(TextDisplayer.Instance.ShowUntilInput("Freeze!"));
				foreach (var card in playerSlotsWithCards.Select(slot => slot.Card))
				{
					card.Anim.StrongNegationEffect();
					card.Anim.StrongNegationEffect();
					card.Anim.StrongNegationEffect();
					card.Anim.StrongNegationEffect();
					card.Anim.StrongNegationEffect();
					card.AddTemporaryMod(new CardModificationInfo(1 - card.Attack, 0));

					freezeCounter = 0;
				}
			}
		}

		yield break;
	}
}