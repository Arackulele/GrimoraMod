using UnityEngine;

namespace GrimoraMod;

public partial class GrimoraPlugin
{
	public const string NameZombie = $"{GUID}_Zombie";

	private void Add_Card_Zombie()
	{
		Sprite pixelSprite = "Zombie".GetCardInfo().pixelPortrait;
		CardBuilder.Builder
			.SetAsNormalCard()
			.SetBaseAttackAndHealth(1, 1)
			.SetBoneCost(2)
			.SetDescription("THE SIMPLEST OF UNDEAD, CHARACTERIZED BY ITS ROTTEN SMELL. IT ALWAYS TRAVELS WITH OTHERS, SEEKING PROTECTION AND GUIDANCE.")
			.SetNames(NameZombie, "Zombie")
			.Build().pixelPortrait=pixelSprite;
	}
}
