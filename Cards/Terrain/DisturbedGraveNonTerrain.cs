using DiskCardGame;

namespace GrimoraMod;

public partial class GrimoraPlugin
{
	public const string NameDisturbedGraveNonTerrain = $"{GUID}_DisturbedGraveNonTerrain";

	private void Add_Card_DisturbedGraveNonTerrain()
	{
		CardBuilder.Builder
			.SetAsNormalCard()
			.SetAbilities(Ability.Evolve)
			.SetBaseAttackAndHealth(0, 1)
			.SetDescription("Its low cost is justified only by its low stats.")
			.SetEvolve(NameZombie, 1)
			.SetNames(NameDisturbedGraveNonTerrain, "Disturbed Grave")
			.Build();
	}
}
