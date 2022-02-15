using APIPlugin;
using UnityEngine;

namespace GrimoraMod;

public partial class GrimoraPlugin
{
	public const string NameFranknstein = "GrimoraMod_Franknstein";

	private void Add_Franknstein()
	{
		Sprite ogSprite = "FrankNStein".GetCardInfo().portraitTex;
		NewCard.Add(CardBuilder.Builder
			.SetAsNormalCard()
			.SetBaseAttackAndHealth(2, 2)
			.SetBoneCost(5)
			.SetDescription("BEST FRIENDS, BROTHERS, AND FIGHTERS.")
			.SetNames(NameFranknstein, "Frank & Stein", ogSprite)
			.Build()
		);
	}
}
