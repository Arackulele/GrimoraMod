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
		 .SetDescription("SHE COMMANDS OTHERS WITH STRENGTH AND GLAMOUR. THEY WILL FOLLOW HER EVERYWHERE")
		 .SetNames(NameCalaveraCatrina, "Calavera Catrina")
		 .Build();
	}
}
