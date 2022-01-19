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
			.SetAsNormalCard()
			.SetAbilities(abilities)
			.SetBaseAttackAndHealth(1, 1)
			.SetEnergyCost(2)
			.SetDescription("A skilled haunting ghost. Handle with caution.")
			.SetNames(NamePoltergeist, "Poltergeist")
			.Build()
		);
	}
}
