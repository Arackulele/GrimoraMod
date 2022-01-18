using APIPlugin;
using DiskCardGame;

namespace GrimoraMod;

public partial class GrimoraPlugin
{
	public const string NameFamily = "ara_Family";

	private void AddAra_Family()
	{
		NewCard.Add(CardBuilder.Builder
			.AsNormalCard()
			.WithAbilities(Ability.QuadrupleBones)
			.WithBaseAttackAndHealth(1, 2)
			.WithBonesCost(4)
			.WithDescription("The family wishes to rest in piece.")
			.WithNames(NameFamily, "The Walkers")
			.Build()
		);
	}
}