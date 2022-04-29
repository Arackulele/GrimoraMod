using DiskCardGame;

namespace GrimoraMod;

public partial class GrimoraPlugin
{
	public const string NameOurobones = $"{GUID}_Ourobones";

	private void Add_Card_Ourobones()
	{
		CardBuilder.Builder
		 .SetAsRareCard()
		 .SetSpecialAbilities(OurobonesBattle.FullSpecial.Id)
		 .SetBaseAttackAndHealth(1, 1)
		 .SetBoneCost(2)
		 .SetDescription("THE KEY TO EVERYTHING, SOMEHOW.")
		 .SetNames(NameOurobones, "Ourobones")
		 .Build();
	}
}
