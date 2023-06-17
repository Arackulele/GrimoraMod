using DiskCardGame;
using UnityEngine;
namespace GrimoraMod;

public partial class GrimoraPlugin
{
	public const string NameAvalanche = $"{GUID}_Avalanche";

	private void Add_Card_Avalanche()
	{
		CardBuilder.Builder
			.SetAbilities(Ability.Reach, Ability.IceCube)
			.SetBaseAttackAndHealth(0, 10)
			.SetBoneCost(6)
			.SetNames(NameAvalanche, "Avalanche")
			.SetTraits(Trait.Giant, Trait.Uncuttable, Trait.DeathcardCreationNonOption)
			.Build();
		;
	}
}
