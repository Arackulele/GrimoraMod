using APIPlugin;
using DiskCardGame;

namespace GrimoraMod;

public partial class GrimoraPlugin
{
	public const string NameSarcophagus = "GrimoraMod_Sarcophagus";

	private void AddAra_Sarcophagus()
	{
		NewCard.Add(CardBuilder.Builder
			.SetAsNormalCard()
			.SetAbilities(Ability.Evolve)
			.SetBaseAttackAndHealth(0, 2)
			.SetBoneCost(4)
			.SetEvolve(NameMummy, 1)
			.SetDescription("The cycle of the Mummy Lord, never ending.")
			.SetNames(NameSarcophagus, "Sarcophagus")
			.Build()
		);
	}
}
