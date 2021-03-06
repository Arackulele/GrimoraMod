namespace GrimoraMod;

public partial class GrimoraPlugin
{
	public const string NameSkeleton = "Skeleton";
	public const string NameSkeletonArmy = $"{GUID}_SkeletonArmy";

	private void Add_Card_SkeletonArmy()
	{
		CardBuilder.Builder
			.SetAsNormalCard()
			.SetAbilities(CreateArmyOfSkeletons.ability)
			.SetBaseAttackAndHealth(1, 2)
			.SetBoneCost(5)
			.SetDescription($"RISE MY ARMY, RIIIIIISE".BrightRed())
			.SetNames(NameSkeletonArmy, "Skeleton Army")
			.Build();
	}
}
