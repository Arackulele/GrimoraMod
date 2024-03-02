using UnityEngine;

namespace GrimoraMod;

public partial class GrimoraPlugin
{
	public const string NameFranknstein = $"{GUID}_Franknstein";

	private void Add_Card_Franknstein()
	{
		Sprite ogSprite = "FrankNStein".GetCardInfo().portraitTex;
		Sprite pixelSprite = "FrankNStein".GetCardInfo().pixelPortrait;

		CardBuilder.Builder
		 .SetAsNormalCard()
		 .SetBaseAttackAndHealth(2, 2)
		 .SetBoneCost(5)
		 .SetDescription("BEST FRIENDS, BROTHERS, AND FIGHTERS.")
		 .SetIceCube(NameZombie)
		 .SetNames(NameFranknstein, "Frank & Stein", ogSprite)
		 .Build().pixelPortrait = pixelSprite;
	}
}
