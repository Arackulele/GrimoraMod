using APIPlugin;
using DiskCardGame;

namespace GrimoraMod;

public partial class GrimoraPlugin
{
	public const string NamePoltergeist = "ara_Poltergeist";

	private void AddAra_Poltergeist()
	{
		List<Ability> abilities = new List<Ability>
		{
			Ability.Flying,
			Ability.Submerge
		};

		NewCard.Add(CardBuilder.Builder
			.AsNormalCard()
			.WithAbilities(abilities)
			.WithBaseAttackAndHealth(1, 1)
			.WithEnergyCost(2)
			.WithDescription("A skilled haunting ghost. Handle with caution.")
			.WithNames(NamePoltergeist, "Poltergeist")
			.Build()
		);
	}
}