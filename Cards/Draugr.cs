using APIPlugin;
using DiskCardGame;

namespace GrimoraMod;

public partial class GrimoraPlugin
{
	public const string NameDraugr = "ara_Draugr";

	private void AddAra_Draugr()
	{
		NewCard.Add(CardBuilder.Builder
				.SetAsNormalCard()
				.SetAbilities(Ability.IceCube)
				.SetBaseAttackAndHealth(0, 1)
				.SetBoneCost(1)
				.SetDescription("Hiding in a suit of armor, this skeleton won't last forever.")
				.SetNames(NameDraugr, "Draugr")
				.Build(),
			iceCubeId: new IceCubeIdentifier("Skeleton")
		);
	}
}
