using APIPlugin;
using DiskCardGame;

namespace GrimoraMod;

public partial class GrimoraPlugin
{
	public const string NameDeathKnell = "GrimoraMod_DeathKnell";

	private void Add_DeathKnell()
	{
		NewCard.Add(CardBuilder.Builder
			.SetAsRareCard()
			.SetAbilities(Ability.CreateBells)
			.SetBaseAttackAndHealth(2, 2)
			.SetBoneCost(8)
			// .SetDescription("")
			.SetNames(NameDeathKnell, "Death Knell")
			.Build()
		);
	}
}
