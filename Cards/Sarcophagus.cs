using DiskCardGame;

namespace GrimoraMod;

public partial class GrimoraPlugin
{
	public const string NameSarcophagus = $"{GUID}_Sarcophagus";

	private void Add_Card_Sarcophagus()
	{
		CardBuilder.Builder
			.SetAsNormalCard()
			.SetAbilities(Ability.Evolve)
			.SetBaseAttackAndHealth(0, 2)
			.SetBoneCost(4)
			.SetEvolve(NameMummy, 1)
			.SetDescription("EMERGING FROM AN OLD EGYPTIAN TOMB, MADE TO HOUSE THE LORDS OF OLD.")
			.SetNames(NameSarcophagus, "Sarcophagus")
			.Build();
	}
}
