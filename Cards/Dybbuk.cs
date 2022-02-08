using APIPlugin;
using DiskCardGame;
using Sirenix.Utilities;
using UnityEngine.UIElements;

namespace GrimoraMod;

public partial class GrimoraPlugin
{
	public const string NameDybbuk = "GrimoraMod_Dybbuk";

	private void Add_Dybbuk()
	{
		NewCard.Add(CardBuilder.Builder
			.SetAsNormalCard()
			.SetAbilities(Possessive.ability)
			.SetBaseAttackAndHealth(0, 1)
			.SetBoneCost(6)
			// .SetDescription("Going into that well wasn't the best idea...")
			.SetNames(NameDybbuk, "Dybbuk")
			.Build()
		);
	}
}
