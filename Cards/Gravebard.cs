using DiskCardGame;

namespace GrimoraMod;

public partial class GrimoraPlugin
{
	public const string NameGravebard = $"{GUID}_Gravebard";

	private void Add_Card_Gravebard()
	{
		CardBuilder.Builder
			.SetAsNormalCard()
			.SetAbilities(Ability.BuffEnemy, Ability.BuffNeighbours)
			.SetBaseAttackAndHealth(1, 1)
			.SetBoneCost(4)
			.SetDescription("THE SINGING IS SO BAD IT RAISES THE DEAD...")
			.SetNames(NameGravebard, "Gravebard")
			.Build();
	}
}
