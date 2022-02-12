using APIPlugin;
using DiskCardGame;

namespace GrimoraMod;

public partial class GrimoraPlugin
{
	public const string NameHydra = "GrimoraMod_Hydra";

	private void Add_Hydra()
	{
		NewCard.Add(CardBuilder.Builder
			.SetAsRareCard()
			.SetAbilities(Ability.DrawCopyOnDeath, Ability.TriStrike)
			.SetBaseAttackAndHealth(1, 1)
			.SetBoneCost(5)
			.SetDescription("Described by some as the truest nightmare.")
			.SetNames(NameHydra, "Hydra")
			.Build()
		);
	}
}
