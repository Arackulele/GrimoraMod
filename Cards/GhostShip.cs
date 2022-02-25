using DiskCardGame;

namespace GrimoraMod;

public partial class GrimoraPlugin
{
	public const string NameGhostShip = "GrimoraMod_GhostShip";

	private void Add_GhostShip()
	{
		CardBuilder.Builder
			.SetAsNormalCard()
			.SetAbilities(Ability.SkeletonStrafe, Ability.Submerge)
			.SetBaseAttackAndHealth(0, 1)
			.SetBoneCost(4)
			.SetDescription("THE SKELETON ARMY NEVER RESTS.")
			.SetNames(NameGhostShip, "Ghost Ship")
			.Build();
	}
}
