using System.Collections;
using DiskCardGame;

namespace GrimoraMod;

public class SawyerBattleSequencer : Part1BossBattleSequencer
{
	public override Opponent.Type BossType => BaseBossExt.SawyerOpponent;

	public override StoryEvent DefeatedStoryEvent => StoryEvent.TutorialRunCompleted;

	public override EncounterData BuildCustomEncounter(CardBattleNodeData nodeData)
	{
		return new EncounterData()
		{
			opponentType = BossType
		};
	}

	public override bool RespondsToOtherCardDie(
		PlayableCard card, CardSlot deathSlot, bool fromCombat, PlayableCard killer
	)
	{
		return true;
	}

	public override IEnumerator OnOtherCardDie(
		PlayableCard card, CardSlot deathSlot, bool fromCombat, PlayableCard killer
	)
	{
		if (TurnManager.Instance.Opponent.GetComponent<SawyerBehaviour>() != null)
		{
			yield return TurnManager.Instance.Opponent.GetComponent<SawyerBehaviour>()
				.OnOtherCardDie(deathSlot.opposingSlot);
		}
	}

	public override bool RespondsToUpkeep(bool playerUpkeep)
	{
		return playerUpkeep;
	}

	public override IEnumerator OnUpkeep(bool playerUpkeep)
	{
		yield break;
	}
}