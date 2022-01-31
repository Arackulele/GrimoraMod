using APIPlugin;
using DiskCardGame;

namespace GrimoraMod;

public partial class GrimoraPlugin
{
	public const string NameGhostShip = "ara_GhostShip";

	private void AddAra_GhostShip()
	{
		NewCard.Add(CardBuilder.Builder
			.SetAsNormalCard()
			.SetAbilities(Ability.SkeletonStrafe, Ability.Submerge)
			.SetBaseAttackAndHealth(0, 1)
			.SetBoneCost(4)
			.SetDescription("The skeleton army never rests.")
			.SetNames(NameGhostShip, "Ghost Ship")
			.Build()
		);
	}
}
