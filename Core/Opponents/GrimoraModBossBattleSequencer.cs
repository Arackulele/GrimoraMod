using DiskCardGame;

namespace GrimoraMod;

public abstract class GrimoraModBossBattleSequencer : GrimoraModBattleSequencer
{
	public abstract Opponent.Type BossType { get; }
}
