using DiskCardGame;

namespace GrimoraMod;

public partial class GrimoraPlugin
{
	public const string NameDisturbedGrave = $"{GUID}_DisturbedGrave";

	private void Add_DisturbedGrave()
	{
		CardBuilder.Builder
			.SetAsNormalCard()
			.SetAbilities(Ability.Evolve)
			.SetBaseAttackAndHealth(0, 1)
			.SetDescription("Its low cost is justified only by its low stats.")
			.SetEvolve(NameZombie, 1)
			.SetNames(NameDisturbedGrave, "Disturbed Grave")
			.Build();
	}
}
