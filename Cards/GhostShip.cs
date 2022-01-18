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
			.AsNormalCard()
			.WithAbilities(abilities)
			.WithBaseAttackAndHealth(0, 1)
			.WithBonesCost(4)
			.WithDescription("The skeleton army never rests.")
			.WithNames(NameGhostShip, "Ghost Ship")
			.Build()
		);
	}
}