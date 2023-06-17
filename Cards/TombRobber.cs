using UnityEngine;

namespace GrimoraMod;

public partial class GrimoraPlugin
{
	public const string NameTombRobber = $"{GUID}_TombRobber";

	private void Add_Card_TombRobber()
	{
		Sprite pixelSprite = "TombRobber".GetCardInfo().pixelPortrait;
		
		
		
		CardBuilder.Builder
			.SetAsNormalCard()
			.SetAbilities(ActivatedDrawSkeletonGrimora.ability)
			.SetBaseAttackAndHealth(0, 1)
			.SetDescription("NOTHING... NOTHING AGAIN... NO TREASURE IS LEFT ANYMORE.")
			.SetNames(NameTombRobber, "Tomb Robber")
			.SetPortraits(AssetUtils.GetPrefab<Sprite>("tomb_robber_new"), AssetUtils.GetPrefab<Sprite>("tomb_robber_emission_new"))
			.Build().pixelPortrait=pixelSprite;
	}
}
