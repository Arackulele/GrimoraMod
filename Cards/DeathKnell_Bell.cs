using DiskCardGame;
using static DiskCardGame.CardAppearanceBehaviour;

namespace GrimoraMod;

public partial class GrimoraPlugin
{
	public const string NameDeathKnellBell = $"{GUID}_DeathKnell_Bell";

	private void Add_Card_DeathKnellBell()
	{
		CardBuilder.Builder
			.SetAppearance(Appearance.TerrainBackground, Appearance.TerrainLayout)
			.SetBaseAttackAndHealth(0, 1)
			.SetNames(NameDeathKnellBell, "Chime")
			.SetTraits(Trait.Structure, Trait.Terrain)
			.Build();
	}
}
