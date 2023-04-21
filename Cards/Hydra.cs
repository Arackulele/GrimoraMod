using DiskCardGame;
using UnityEngine;
namespace GrimoraMod;

public partial class GrimoraPlugin
{
	public const string NameHydra = $"{GUID}_Hydra";

	private void Add_Card_Hydra()
	{
		CardBuilder.Builder
			.SetAsRareCard()
			.SetAbilities(Ability.DrawCopyOnDeath, Ability.TriStrike)
			.SetBaseAttackAndHealth(1, 1)
			.SetBoneCost(5)
			.SetDescription("LEGENDS HAVE BEEN TOLD ABOUT THE LEGENDARY HYDRA, THE BEAST THAT SWALLOWS ALL., AND THE BANE OF ALL THAT SAIL THE SEAS.")
			.SetNames(NameHydra, "Hydra")
			.SetPortraits(AssetUtils.GetPrefab<Sprite>("hydra_new"), AssetUtils.GetPrefab<Sprite>("hydra_emission_new"))
			.Build();
	}
}
