using DiskCardGame;

namespace GrimoraMod;

public partial class GrimoraPlugin
{
	public const string NameShipwreckDams = $"{GUID}_Shipwreck_Dams";

	private void Add_Card_Shipwreck_Dams()
	{
		CardBuilder.Builder
		 .SetBaseAttackAndHealth(0, 2)
			.SetNames(NameShipwreckDams, "Shipwreck")
			.SetTraits(Trait.Structure, Trait.Terrain)
			.Build();
	}
}
