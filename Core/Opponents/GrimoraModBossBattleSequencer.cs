using DiskCardGame;

namespace GrimoraMod;

public abstract class GrimoraModBossBattleSequencer : GrimoraModBattleSequencer
{
	public abstract Opponent.Type BossType { get; }

	public override EncounterData BuildCustomEncounter(CardBattleNodeData nodeData)
	{
		return new EncounterData
		{
			opponentType = BossType
		};
	}
}
