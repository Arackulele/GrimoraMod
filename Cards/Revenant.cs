using DiskCardGame;
using UnityEngine;

namespace GrimoraMod;

public partial class GrimoraPlugin
{
	public const string NameRevenant = $"{GUID}_Revenant";

	private void Add_Card_Revenant()
	{
		Sprite pixelSprite = "Revenant".GetCardInfo().pixelPortrait;

		Sprite ogSprite = "Revenant".GetCardInfo().portraitTex;
		CardBuilder.Builder
			.SetAsNormalCard()
			.SetAbilities(Ability.Brittle)
			.SetBaseAttackAndHealth(3, 1)
			.SetBoneCost(3)
			.SetDescription("THE REVENANT, BRINGING THE SCYTHE OF DEATH.")
			.SetNames(NameRevenant, "Revenant", ogSprite)
			.Build().pixelPortrait = pixelSprite;
	}
}
