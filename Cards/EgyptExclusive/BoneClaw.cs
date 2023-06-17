using BepInEx.Bootstrap;
using DiskCardGame;
using UnityEngine;

namespace GrimoraMod;

public partial class GrimoraPlugin
{
	public const string NameBoneclaw = $"{GUID}_Boneclaw";

	private void Add_Card_Boneclaw()
	{
		if (Chainloader.PluginInfos.ContainsKey("arackulele.inscryption._grimoramodextracards"))
		{
			CardBuilder.Builder
			.SetAsNormalCard()
			.SetPortraits(AssetUtils.GetPrefab<Sprite>("boneclaw"), AssetUtils.GetPrefab<Sprite>("boneclaw_emission"))
			.SetAbilities(Slasher.ability)
			.SetBaseAttackAndHealth(2, 2)
			.SetNames(NameBoneclaw, "Boneclaw")
			.SetBoneCost(7)
			.Build();
		}
		else
		{
			CardBuilder.Builder
			.SetAbilities(Slasher.ability)
			.SetPortraits(AssetUtils.GetPrefab<Sprite>("boneclaw"), AssetUtils.GetPrefab<Sprite>("boneclaw_emission"))
			.SetBaseAttackAndHealth(2, 2)
			.SetNames(NameBoneclaw, "Boneclaw")
			.SetBoneCost(7)
			.Build();
		}
	}
}
