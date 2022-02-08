using APIPlugin;
using DiskCardGame;

namespace GrimoraMod;

public partial class GrimoraPlugin
{
	public const string NameDeadHand = "GrimoraMod_DeadHand";

	private void Add_DeadHand()
	{
		NewCard.Add(CardBuilder.Builder
			.SetAsNormalCard()
			.SetAbilities(Ability.DrawNewHand)
			.SetBaseAttackAndHealth(1, 1)
			.SetBoneCost(5)
			.SetDescription("Cut off from an ancient God, the Dead Hand took on its own Life.")
			.SetNames(NameDeadHand, "Dead Hand")
			.Build()
		);
	}
}
