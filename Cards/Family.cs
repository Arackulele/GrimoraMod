using DiskCardGame;
using UnityEngine;
namespace GrimoraMod;

public partial class GrimoraPlugin
{
	public const string NameFamily = $"{GUID}_Family";



	private void Add_Card_Family()
	{
		Sprite pixelSprite = "Family".GetCardInfo().pixelPortrait;

		CardBuilder.Builder
			.SetAsNormalCard()
			.SetAbilities(Ability.QuadrupleBones)
			.SetBaseAttackAndHealth(1, 2)
			.SetBoneCost(4)
			.SetDescription("THE FAMILY WISHES TO REST IN PIECE, YET THEY ARE SUMMONED AGAIN AND AGAIN IN AN ETERNAL BATTLE.")
			.SetNames(NameFamily, "The Walkers")
			.Build().pixelPortrait = pixelSprite;
	}
}
