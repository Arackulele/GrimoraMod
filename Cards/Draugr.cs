using DiskCardGame;
using UnityEngine;

namespace GrimoraMod;

public partial class GrimoraPlugin
{
	public const string NameDraugr = $"{GUID}_Draugr";

	private void Add_Card_Draugr()
	{

		Sprite pixelSprite = "Draugr".GetCardInfo().pixelPortrait;
		
		CardBuilder.Builder
			.SetAsNormalCard()
			.SetAbilities(Ability.IceCube)
			.SetEvolve(NameGlacier, 1)
			.SetBaseAttackAndHealth(0, 1)
			.SetBoneCost(1)
			.SetDescription("HIDING IN A SUIT OF ARMOR, OR ICE, WHAT DOES IT MATTER. THIS SKELETON WON'T LAST FOREVER.")
			.SetIceCube(NameSkeleton)
			//Just using an unused Trait so we dont have to make a new one
			.SetTraits(Trait.DeathcardCreationNonOption)
			.SetNames(NameDraugr, "Draugr")
			.Build()
			.pixelPortrait=pixelSprite;
	}
}
