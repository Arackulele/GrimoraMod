using DiskCardGame;
using UnityEngine;

namespace GrimoraMod;

public partial class GrimoraPlugin
{
	public const string NameBonepile = $"{GUID}_Bonepile";

	private void Add_Card_Bonepile()
	{
		Sprite pixelSprite = "Bonepile".GetCardInfo().pixelPortrait;
		CardBuilder.Builder
		 .SetAsNormalCard()
		 .SetAbilities(Ability.QuadrupleBones)
		 .SetBaseAttackAndHealth(0, 1)
		 .SetBoneCost(1)
		 .SetDescription("AN UNINSPIRING PILE OF BONES. YOU CAN HAVE IT.")
		 .SetNames(NameBonepile, "Bone Heap")
		 .Build().pixelPortrait = pixelSprite;
	}
}
