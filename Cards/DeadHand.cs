using DiskCardGame;

namespace GrimoraMod;

public partial class GrimoraPlugin
{
	public const string NameDeadHand = $"{GUID}_DeadHand";

	private void Add_Card_DeadHand()
	{
		CardBuilder.Builder
			.SetAsNormalCard()
			.SetAbilities(Ability.DrawNewHand)
			.SetBaseAttackAndHealth(1, 1)
			.SetBoneCost(4)
			.SetDescription("SOME SAY THIS HAND ONCE BELONGED TO AN ANCIENT GOD. ANYTHING IT TOUCHES ROTS, BUT AS WITH ANYTHING THAT ENDS, IT IS ALSO A NEW BEGINNING.")
			.SetNames(NameDeadHand, "Dead Hand")
			.Build();
	}
}
