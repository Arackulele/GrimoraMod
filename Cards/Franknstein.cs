using APIPlugin;
using DiskCardGame;
using UnityEngine;

namespace GrimoraMod;

public partial class GrimoraPlugin
{
	public const string NameFranknstein = "ara_Franknstein";

	private void AddAra_Franknstein()
	{
		Sprite ogSprite = CardLoader.GetCardByName("FrankNStein").portraitTex;
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
