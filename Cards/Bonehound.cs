using DiskCardGame;

namespace GrimoraMod;

public partial class GrimoraPlugin
{
	public const string NameBonehound = $"{GUID}_Bonehound";

	private void Add_Bonehound()
	{
		CardInfo bonehound = "Bonehound".GetCardInfo();
		CardBuilder.Builder
			.SetAsNormalCard()
			.SetAbilities(bonehound.Abilities.ToArray())
			.SetBaseAttackAndHealth(bonehound.Attack, bonehound.Health)
			.SetBoneCost(bonehound.BonesCost)
			.SetDescription("THE DAUNTLESS BONEHOUND. IT LEAPS TO OPPOSE NEW CREATURES WHEN THEY ARE PLAYED.")
			.SetNames(NameBonehound, "Bonehound", bonehound.portraitTex)
			.Build();
	}
}
