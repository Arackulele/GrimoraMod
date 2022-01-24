using APIPlugin;
using DiskCardGame;

namespace GrimoraMod;

public partial class GrimoraPlugin
{
	public const string NameFlames = "ara_Flames";

	private void AddAra_Flames()
	{
		List<Ability> abilities = new List<Ability>
		{
			Ability.BuffNeighbours,
			Ability.Brittle
		};

		NewCard.Add(CardBuilder.Builder
			.SetAbilities(abilities)
			.SetBaseAttackAndHealth(0, 1)
			.SetBoneCost(2)
			.SetNames(NameFlames, "Flames")
			.Build()
		);
	}
}
