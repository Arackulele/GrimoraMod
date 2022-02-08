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
			.SetDescription("Best friends, brothers, and fighters.")
			.SetNames(NameFranknstein, "Frank & Stein", ogSprite)
			.Build()
		);
	}
}
