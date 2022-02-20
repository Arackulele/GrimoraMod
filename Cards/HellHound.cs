using APIPlugin;
using DiskCardGame;

namespace GrimoraMod;

public partial class GrimoraPlugin
{
	public const string NameHellHound = "GrimoraMod_HellHound";

	private void Add_HellHound()
	{
		NewCard.Add(CardBuilder.Builder
			.SetAsRareCard()
			.SetAbilities(Ability.WhackAMole, Ability.Reach)
			.SetAbilities(GainAttackBones.NewSpecialAbility.specialTriggeredAbility)
			.SetBaseAttackAndHealth(1, 9)
			.SetBoneCost(5)
			.SetDescription("A RABID DOG. IT KILLED ONE OF MY GHOULS.")
			.SetNames(NameHellHound, "Hell Hound")
			.Build()
		);
	}
}

