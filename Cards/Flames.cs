using DiskCardGame;

namespace GrimoraMod;

public partial class GrimoraPlugin
{
	public const string NameFlames = "GrimoraMod_Flames";

	private void Add_Flames()
	{
		CardBuilder.Builder
			.SetAbilities(Ability.Brittle, Ability.BuffNeighbours)
			.SetBaseAttackAndHealth(0, 1)
			.SetBoneCost(2)
			.SetNames(NameFlames, "Flames")
			.Build();
	}
}
