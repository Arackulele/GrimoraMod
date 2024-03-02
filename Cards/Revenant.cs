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
			.SetDescription("BRINGING THE SCYTHE OF DEATH, THE REVENANT SEEKS ONLY REVENGE.")
			.SetNames(NameRevenant, "Revenant", ogSprite)
			.Build().pixelPortrait = pixelSprite;
	}
}
