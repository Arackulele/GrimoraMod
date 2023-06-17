using DiskCardGame;

namespace GrimoraMod;

public partial class GrimoraPlugin
{
	public const string NameDisturbedGrave = $"{GUID}_DisturbedGrave";

	private void Add_Card_DisturbedGrave()
	{
		CardBuilder.Builder
			.SetAbilities(Ability.Evolve)
			.SetBaseAttackAndHealth(0, 1)
			.SetDescription("Its low cost is justified only by its low stats.")
			.SetEvolve(NameZombie, 2)
			.SetNames(NameDisturbedGrave, "Disturbed Grave")
			.SetTraits(Trait.Structure, Trait.Terrain)
			.Build();
	}
}
