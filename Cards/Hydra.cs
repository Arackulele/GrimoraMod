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
			.AsRareCard()
			.WithAbilities(abilities)
			.WithBaseAttackAndHealth(1, 1)
			.WithBonesCost(4)
			.WithDescription("Described by some as the truest nightmare.")
			.WithNames(NameHydra, "Hydra")
			.Build()
		);
	}
}