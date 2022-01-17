using APIPlugin;
using DiskCardGame;
using GrimoraMod.Properties;

namespace GrimoraMod;

public partial class GrimoraPlugin
{
	public const string NameSkeletonMage = "ara_SkeletonMage";

	private void AddAra_SkeletonMage()
	{
		NewCard.Add(CardBuilder.Builder
			.AsNormalCard()
			.WithAbilities(Ability.Brittle)
			.WithBaseAttackAndHealth(4, 1)
			.WithEnergyCost(5)
			.WithDescription("The Skelemagus, they have learned the ancient spell of Death.")
			.WithNames(NameSkeletonMage, "Skelemagus")
			.WithPortrait(Resources.SkeletonMage)
			.Build()
		);
	}
}