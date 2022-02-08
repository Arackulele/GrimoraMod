using APIPlugin;
using DiskCardGame;

namespace GrimoraMod;

public partial class GrimoraPlugin
{
	public const string NameSkeletonArmy = "GrimoraMod_SkeletonArmy";

	private void AddAra_SkeletonArmy()
	{
		NewCard.Add(CardBuilder.Builder
			.SetAsNormalCard()
			.SetAbilities(PayBonesForSkeleton.ability)
			.SetBaseAttackAndHealth(1, 3)
			.SetBoneCost(5)
			.SetDescription("The skeleton army, boons of the Bone Lord")
			.SetNames(NameSkeletonArmy, "Skeleton Army")
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
