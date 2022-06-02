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
			.SetDescription("GOING INTO THAT WELL WASN'T THE BEST IDEA...")
			.SetEnergyCost(5)
			.SetNames(NameDrownedSoul, "Drowned Soul")
			.Build().pixelPortrait=pixel;
	}
}
