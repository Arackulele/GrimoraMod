using APIPlugin;

namespace GrimoraMod;

public partial class GrimoraPlugin
{
	public const string NameSkeletonArmy = "GrimoraMod_SkeletonArmy";

	private void Add_SkeletonArmy()
	{
		NewCard.Add(CardBuilder.Builder
			.SetAsNormalCard()
			.SetAbilities(CreateArmyOfSkeletons.ability)
			.SetBaseAttackAndHealth(1, 3)
			.SetBoneCost(5)
			.SetDescription("[c:bR]RISE MY ARMY, RIIIIIISE[c:]")
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
