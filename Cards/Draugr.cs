using APIPlugin;
using DiskCardGame;
using GrimoraMod.Properties;

namespace GrimoraMod;

public partial class GrimoraPlugin
{
	public const string NameDraugr = "ara_Draugr";

	private void AddAra_Draugr()
	{
		NewCard.Add(CardBuilder.Builder
				.AsNormalCard()
				.WithAbilities(Ability.IceCube)
				.WithBaseAttackAndHealth(0, 1)
				.WithBonesCost(1)
				.WithDescription("Hiding in a suit of armor, this skeleton won't last forever.")
				.WithNames(NameDraugr, "Draugr")
				.WithPortrait(Resources.Draugr)
				.Build(),
			iceCubeId: new IceCubeIdentifier("Skeleton")
		);
	}
}