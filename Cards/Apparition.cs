using DiskCardGame;
using UnityEngine;
namespace GrimoraMod;

public partial class GrimoraPlugin
{
	public const string NameApparition = $"{GUID}_Apparition";

	private void Add_Card_Apparition()
	{
		CardBuilder.Builder
			.SetAsNormalCard()
			.SetAbilities(GrimoraRandomAbility.ability, Haunter.ability)
			.SetBaseAttackAndHealth(1, 1)
			.SetEnergyCost(4)
			.SetDescription("A formless Creature, that yet lingers. Truly a horrific sight.")
			.SetNames(NameApparition, "Apparition")
			.Build().pixelPortrait = AssetUtils.GetPrefab<Sprite>("apparition_pixel");
	}
}
