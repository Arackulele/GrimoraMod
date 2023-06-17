using DiskCardGame;
using UnityEngine;
namespace GrimoraMod;

public partial class GrimoraPlugin
{
	public const string NameNecromancer = $"{GUID}_Necromancer";

	Sprite pixelSprite = "Necromancer".GetCardInfo().pixelPortrait;
	private void Add_Card_Necromancer()
	{
		CardBuilder.Builder
			.SetAsRareCard()
			.SetAbilities(Ability.DoubleDeath)
			.SetBaseAttackAndHealth(1, 2)
			.SetBoneCost(3)
			.SetDescription("ALL EFFORTS TO PRESERVE LIFE AFTER DEATH HAVE BEEN FUTILE, YET THE NECROMANCER GOES ON. HE HAS TO.")
			.SetNames(NameNecromancer, "Necromancer")
			.Build().pixelPortrait = pixelSprite;
	}
}
