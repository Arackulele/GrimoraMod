using APIPlugin;
using DiskCardGame;
using Sirenix.Utilities;
using UnityEngine;
using UnityEngine.UIElements;

namespace GrimoraMod;

public partial class GrimoraPlugin
{
	public const string NameSilbon = "ara_Silbon";

	private void AddAra_Silbon()
	{
		NewCard.Add(CardBuilder.Builder
			.SetAsNormalCard()
			.SetAbilities(InvertedStrike.ability, Ability.Strafe)
			.SetBaseAttackAndHealth(3, 2)
			.SetBoneCost(5)
			// .SetDescription("Going into that well wasn't the best idea...")
			.SetNames(NameSilbon, "Silbon")
			.Build()
		);
	}
}
