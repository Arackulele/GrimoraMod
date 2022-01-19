using APIPlugin;
using DiskCardGame;

namespace GrimoraMod;

public partial class GrimoraPlugin
{
	public const string NameGhostShip = "ara_GhostShip";

	private void AddAra_GhostShip()
	{
		List<Ability> abilities = new List<Ability>
		{
			Ability.SkeletonStrafe,
			Ability.Submerge
		};

		NewCard.Add(CardBuilder.Builder
			.SetAsNormalCard()
			.SetAbilities(abilities)
			.SetBaseAttackAndHealth(0, 1)
			.SetBoneCost(4)
			.SetDescription("The skeleton army never rests.")
			.SetNames(NameGhostShip, "Ghost Ship")
			.Build()
		);
	}
}
