using DiskCardGame;
using UnityEngine;
namespace GrimoraMod;

public partial class GrimoraPlugin
{
	public const string NameSkelemagus = $"{GUID}_Skelemagus";

	private void Add_Card_Skelemagus()
	{
		CardBuilder.Builder
			.SetAsNormalCard()
			.SetAbilities(Ability.Brittle)
			.SetBaseAttackAndHealth(4, 1)
			.SetEnergyCost(5)
			.SetDescription("THEY HAVE LEARNED THE ANCIENT SPELL OF DEATH.")
			.SetNames(NameSkelemagus, "Skelemagus")
			.SetPortraits(AssetUtils.GetPrefab<Sprite>("Skelemagus_new"), AssetUtils.GetPrefab<Sprite>("Skelemagus_emission_new"))
			.Build();
	}
}
