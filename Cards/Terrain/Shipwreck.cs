using DiskCardGame;

namespace GrimoraMod;

public partial class GrimoraPlugin
{
	public const string NameShipwreck = $"{GUID}_Shipwreck";

	private void Add_Shipwreck()
	{
		CardBuilder.Builder
			.SetAbilities(Ability.Sharp)
			.SetBaseAttackAndHealth(0, 3)
			.SetNames(NameShipwreck, "Shipwreck")
			.SetTraits(Trait.Structure, Trait.Terrain)
			.Build();
	}
}
