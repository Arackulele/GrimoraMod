using APIPlugin;
using GrimoraMod.Properties;

namespace GrimoraMod;

public partial class GrimoraPlugin
{
	public const string NameSkelemancer = "ara_Skelemancer";

	private void AddAra_Skelemancer()
	{
		NewCard.Add(CardBuilder.Builder
			.AsNormalCard()
			.WithBaseAttackAndHealth(1, 1)
			.WithEnergyCost(2)
			.WithDescription("Going into that well wasn't the best idea...")
			.WithNames(NameSkelemancer, "Skelemancer")
			.WithPortrait(Resources.SkeletonJuniorSage)
			.Build()
		);
	}
}