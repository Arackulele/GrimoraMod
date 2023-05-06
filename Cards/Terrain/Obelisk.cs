using DiskCardGame;

namespace GrimoraMod;

public partial class GrimoraPlugin
{
	public const string NameObelisk = $"{GUID}_Obelisk";

	private void Add_Card_Obelisk()
	{
		CardBuilder.Builder
			.SetAbilities(Ability.MadeOfStone )
			.SetBaseAttackAndHealth(0, 3)
			.SetNames(NameObelisk, "Obelisk")
			.SetTraits(Trait.Structure, Trait.Terrain)
			.Build();
	}
}
