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
			.SetDescription("CUT OFF FROM AN ANCIENT GOD, THE DEAD HAND TOOK ON ITS OWN LIFE.")
			.SetNames(NameDeadHand, "Dead Hand")
			.Build()
		);
	}
}
