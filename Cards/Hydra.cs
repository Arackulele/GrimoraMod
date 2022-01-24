using APIPlugin;
using DiskCardGame;

namespace GrimoraMod;

public partial class GrimoraPlugin
{
	public const string NameHydra = "ara_Hydra";

	private void AddAra_Hydra()
	{
		List<Ability> abilities = new List<Ability>
		{
			Ability.DrawCopyOnDeath,
			Ability.TriStrike
		};

		NewCard.Add(CardBuilder.Builder
			.SetAsRareCard()
			.SetAbilities(abilities)
			.SetBaseAttackAndHealth(1, 1)
			.SetBoneCost(5)
			.SetDescription("Described by some as the truest nightmare.")
			.SetNames(NameHydra, "Hydra")
			.Build()
		);
	}
}
