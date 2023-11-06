using DiskCardGame;

namespace GrimoraMod;

public partial class GrimoraPlugin
{
	public const string NameUrn = $"{GUID}_Urn";

	private void Add_Card_Urn()
	{
		CardBuilder.Builder
			.SetAbilities(Ability.MadeOfStone, NegateFire.ability)
			.SetBaseAttackAndHealth(0, 1)
			.SetNames(NameUrn, "Water Urn")
			.SetTraits(Trait.Structure, Trait.Terrain)
			.Build();
	}
}
