using APIPlugin;
using DiskCardGame;

namespace GrimoraMod;

public partial class GrimoraPlugin
{
	public const string NameSarcophagus = "ara_Sarcophagus";

	private void AddAra_Sarcophagus()
	{
		NewCard.Add(CardBuilder.Builder
				.SetAsNormalCard()
				.SetAbilities(Ability.Evolve)
				.SetBaseAttackAndHealth(0, 2)
				.SetBoneCost(4)
				.SetDescription("The cycle of the Mummy Lord, never ending.")
				.SetNames(NameSarcophagus, "Sarcophagus")
				.Build(),
			evolveId: new EvolveIdentifier(NameMummy, 1)
		);
	}
}
