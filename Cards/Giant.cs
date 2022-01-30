using System.Collections;
using APIPlugin;
using DiskCardGame;
using HarmonyLib;
using UnityEngine;

namespace GrimoraMod;

public partial class GrimoraPlugin
{
	public const string NameGiant = "ara_Giant";

	private void AddAra_Giant()
	{
		NewCard.Add(CardBuilder.Builder
			.SetAsNormalCard()
			.SetAbilities(Ability.QuadrupleBones, Ability.SplitStrike)
			.SetBaseAttackAndHealth(2, 7)
			.SetBoneCost(15)
			.SetNames(NameGiant, "Giant")
			.SetTraits(Trait.Giant)
			// .SetDescription("A vicious pile of bones. You can have it...")
			.Build()
		);
	}
}
