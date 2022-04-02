namespace GrimoraMod;

public partial class GrimoraPlugin
{
	public const string NameCalaveraCatrina = $"{GUID}_CalaveraCatrina";

	private void Add_Card_CalaveraCatrina()
	{
		CardBuilder.Builder
		 .SetAsRareCard()
		 .SetAbilities(MarchingDead.ability)
		 .SetBaseAttackAndHealth(2, 2)
		 .SetBoneCost(8)
		 .SetDescription("A highly attractive mistress. Others will follow her closely wherever she goes.")
		 .SetNames(NameCalaveraCatrina, "Calavera Catrina")
		 .Build();
	}
}
