using APIPlugin;
using DiskCardGame;

namespace GrimoraMod;

public partial class GrimoraPlugin
{
	public const string NameFamily = "GrimoraMod_Family";

	private void AddAra_Family()
	{
		NewCard.Add(CardBuilder.Builder
			.SetAsNormalCard()
			.SetAbilities(Ability.QuadrupleBones)
			.SetBaseAttackAndHealth(1, 2)
			.SetBoneCost(4)
			.SetDescription("The family wishes to rest in piece.")
			.SetNames(NameFamily, "The Walkers")
			.Build()
		);
	}
}
