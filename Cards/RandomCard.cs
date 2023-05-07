using DiskCardGame;
using UnityEngine;
namespace GrimoraMod;

public partial class GrimoraPlugin
{
	public const string NameRandomCard = $"{GUID}_Random";

	private void Add_Card_RandomCard()
	{
		CardBuilder.Builder
			.SetAbilities(GrimoraRandomAbility.ability)
			.SetBaseAttackAndHealth(0, 0)
			.SetDescription("You shouldnt be seeing this right now")
			.SetNames(NameRandomCard, "Random Cards")
			.Build().pixelPortrait = AssetUtils.GetPrefab<Sprite>("randompixelportrait");
	}
}
