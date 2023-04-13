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
			.SetAbilities(GrimoraRandomAbility.ability, Ability.Haunter)
			.SetBaseAttackAndHealth(1, 2)
			.SetEnergyCost(4)
			.SetDescription("Without a coherent form, who knows what sigil it will have next?")
			.SetNames(NameApparition, "Apparition")
			.Build().pixelPortrait = AssetUtils.GetPrefab<Sprite>("apparition_pixel");
	}
}
