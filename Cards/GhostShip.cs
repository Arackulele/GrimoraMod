using DiskCardGame;
using UnityEngine;

namespace GrimoraMod;

public partial class GrimoraPlugin
{
	public const string NameGhostShip = $"{GUID}_GhostShip";

	private void Add_Card_GhostShip()
	{

		Sprite pixelSprite = "GhostShip".GetCardInfo().pixelPortrait;



		CardBuilder.Builder
		 .SetAsNormalCard()
		 .SetAbilities(Ability.SkeletonStrafe, Ability.Submerge)
		 .SetBaseAttackAndHealth(0, 1)
		 .SetBoneCost(4)
		 .SetDescription("THE SKELETON ARMY NEVER RESTS.")
		 .FlipPortraitForStrafe()
		 .SetNames(NameGhostShip, "Ghost Ship")
		 .Build().pixelPortrait = pixelSprite;
	}
}
