using APIPlugin;
using DiskCardGame;
using GrimoraMod.Properties;

namespace GrimoraMod;

public partial class GrimoraPlugin
{
	public const string NameSkeletonArmy = "ara_SkeletonArmy";

	private void AddAra_SkeletonArmy()
	{
		NewCard.Add(CardBuilder.Builder
			.AsNormalCard()
			.WithAbilities(Ability.SkeletonStrafe)
			.WithBaseAttackAndHealth(2, 4)
			.WithBonesCost(2)
			.WithEnergyCost(4)
			.WithDescription("The skeleton army, boons of the Bone Lord")
			.WithNames(NameSkeletonArmy, "Skeleton Army")
			.WithPortrait(Resources.SkeletonArmy)
			.Build()
		);

		// new CustomCard("Goat")
		// {
		// 	displayedName = NameSkeletonArmy,
		// 	baseAttack = 2,
		// 	baseHealth = 4,
		// 	energyCost = 2,
		// 	cost = 0,
		// 	tex = defaultTex,
		// 	abilities = abilities,
		// 	decals = decals,
		// 	metaCategories = metaCategories,
		// 	description = "The skeleton army, boons of the Bone Lord."
		// };
	}
}