using DiskCardGame;
using InscryptionAPI.Card;
using UnityEngine;
using static GrimoraMod.GrimoraPlugin;

namespace GrimoraMod;

public partial class GrimoraPlugin
{
	public const string NameSummoner = $"{GUID}_Summoner";

	private void Add_Card_Summoner()
	{
		CardBuilder.Builder
			.SetAsNormalCard()
			.SetAbilities(Ability.SplitStrike)
			.SetBaseAttackAndHealth(1, 1)
			.SetBoneCost(4)
			.SetDescription("HE HAS SPENT YEARS IN HIS STUDY, IN A TIRELESS ATTEMPT TO CHANGE HIS FATE. YET FATE IS A CRUEL MISTRESS.")
			.SetNames(NameSummoner, "Summoner")
			.SetPortraits(AssetUtils.GetPrefab<Sprite>("summoner_new"), AssetUtils.GetPrefab<Sprite>("summoner_new_emission"))
			.Build().pixelPortrait = AssetUtils.GetPrefab<Sprite>("summoner_pixel");


	}
}
