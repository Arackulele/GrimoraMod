using DiskCardGame;
using UnityEngine;
namespace GrimoraMod;

public partial class GrimoraPlugin
{
	public const string NameBonehound = $"{GUID}_Bonehound";

	private void Add_Card_Bonehound()
	{
		Sprite pixelSprite = "Bonehound".GetCardInfo().pixelPortrait;

		CardInfo bonehound = "Bonehound".GetCardInfo();
		CardBuilder.Builder
			.SetAsNormalCard()
			.SetAbilities(bonehound.Abilities.ToArray())
			.SetBaseAttackAndHealth(bonehound.Attack, bonehound.Health)
			.SetBoneCost(bonehound.BonesCost)
			.SetDescription("THE FLESHLESS BONEHOUND. IT LEAPS TO OPPOSE NEW CREATURES WHEN THEY ARE PLAYED... OR AT LEAST THAT IS HOW HE WOULD SAY IT.")
			.SetNames(NameBonehound, "Bonehound", bonehound.portraitTex)
			.Build().pixelPortrait = pixelSprite;
	}
}
