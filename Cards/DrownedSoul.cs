using DiskCardGame;
using UnityEngine;

namespace GrimoraMod;

public partial class GrimoraPlugin
{
	public const string NameDrownedSoul = $"{GUID}_DrownedSoul";

	private void Add_Card_DrownedSoul()
	{
		Sprite pixel = "DrownedSoul".GetCardInfo().pixelPortrait;
		CardBuilder.Builder
			.SetAsNormalCard()
			.SetAbilities(Ability.Deathtouch, Ability.Submerge)
			.SetBaseAttackAndHealth(1, 1)
			.SetDescription("WHAT A SAD SIGHT, NO ONE SHALL KNOW WHAT LIES AT THE BOTTOM OF THAT WELL.")
			.SetEnergyCost(5)
			.SetNames(NameDrownedSoul, "Drowned Soul")
			.Build().pixelPortrait=pixel;
	}
}
