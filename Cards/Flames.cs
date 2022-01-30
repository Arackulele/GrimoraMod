using APIPlugin;
using DiskCardGame;

namespace GrimoraMod;

public partial class GrimoraPlugin
{
	public const string NameFlames = "ara_Flames";

	private void AddAra_Flames()
	{
		NewCard.Add(CardBuilder.Builder
			.SetAbilities(Ability.Brittle, Ability.BuffNeighbours)
			.SetBaseAttackAndHealth(0, 1)
			.SetBoneCost(2)
			.SetNames(NameFlames, "Flames")
			.Build()
		);
	}
}
