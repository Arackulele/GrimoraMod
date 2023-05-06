using DiskCardGame;
using UnityEngine;
namespace GrimoraMod;

public partial class GrimoraPlugin
{
	public const string NameGlacier = $"{GUID}_Glacier";

	private void Add_Card_Glacier_Stage1()
	{
		CardBuilder.Builder
			.SetAsNormalCard()
			.SetAbilities(Ability.IceCube)
			.SetBaseAttackAndHealth(0, 4)
			.SetBoneCost(10)
			.SetIceCube(NameFrostGiant)
			.SetNames(NameGlacier, "Glacier")
			.SetTraits(Trait.Giant, Trait.Uncuttable, Trait.DeathcardCreationNonOption)
			.SetDescription("I WONDER WHAT HAS BEEN FROZEN AWAY FOR SO LONG?")
			.Build().pixelPortrait = AssetUtils.GetPrefab<Sprite>("glacier_pixel");
		;
	}
}
