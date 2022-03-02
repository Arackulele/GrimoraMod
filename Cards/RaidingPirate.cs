using DiskCardGame;

namespace GrimoraMod;

public partial class GrimoraPlugin
{
	public const string NameRaidingPirate = "GrimoraMod_RaidingPirate";

	private void Add_RaidingPirate()
	{
		CardBuilder.Builder
			.SetAbilities(Raider.ability)
			.SetBaseAttackAndHealth(1, 2)
			.SetDescription("CUT OFF FROM AN ANCIENT GOD, THE DEAD HAND TOOK ON ITS OWN LIFE.")
			.SetNames(NameRaidingPirate, "Raiding Pirate")
			.Build();
	}
}
