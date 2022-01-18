using APIPlugin;
using DiskCardGame;

namespace GrimoraMod;

public partial class GrimoraPlugin
{
	public const string NameSarcophagus = "ara_Sarcophagus";

	private void AddAra_Sarcophagus()
	{
		NewCard.Add(CardBuilder.Builder
				.AsNormalCard()
				.WithAbilities(Ability.Evolve)
				.WithBaseAttackAndHealth(0, 2)
				.WithBonesCost(4)
				.WithDescription("The cycle of the Mummy Lord, never ending.")
				.WithNames(NameSarcophagus, "Sarcophagus")
				.Build(),
			evolveId: new EvolveIdentifier(NameMummy, 1)
		);
	}
}