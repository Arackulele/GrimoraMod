using DiskCardGame;

namespace GrimoraMod;

public partial class GrimoraPlugin
{
	public const string NameFlames = $"{GUID}_Flames";

	private void Add_Card_Flames()
	{
		CardBuilder.Builder
			.SetAbilities(Ability.BuffNeighbours, Burning.ability)
			.SetBaseAttackAndHealth(0, 2)
			.SetBoneCost(2)
			.SetNames(NameFlames, "Flames")
			.SetTraits(Trait.Terrain, Trait.Structure)
			.Build();
	}
}
