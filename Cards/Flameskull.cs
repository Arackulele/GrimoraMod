using DiskCardGame;

namespace GrimoraMod;

public partial class GrimoraPlugin
{
	public const string NameFlameskull = $"{GUID}_Flameskull";

	private void Add_Flameskull()
	{
		CardBuilder.Builder
			.SetAsNormalCard()
			.SetAbilities(Ability.BuffNeighbours, Ability.Flying)
			.SetBaseAttackAndHealth(2, 1)
			.SetBoneCost(6)
			.SetNames(NameFlameskull, "Flameskull")
			.SetDescription("THE BLAZING FLAMESKULL, ITS HEAT ENHANCES CREATURES ADJACENT TO IT.")
			.Build();
	}
}
