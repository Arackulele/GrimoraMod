namespace GrimoraMod;

public partial class GrimoraPlugin
{
	public const string NameCalaveraCatrina = $"{GUID}_CalaveraCatrina";

	private static void Add_CalaveraCatrina()
	{
		CardBuilder.Builder
			.SetAsRareCard()
			.SetAbilities(MarchingDead.ability)
			.SetBaseAttackAndHealth(2, 2)
			.SetBoneCost(8)
			.SetNames(NameCalaveraCatrina, "Calavera Catrina")
			// .SetDescription("There will be quite a bit of difficulty putting it back togethe- OW!")
			.Build();
	}
}
