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
			.SetAbilities(Ability.WhackAMole, Ability.Reach, GainAttackBones.ability)
			.SetBaseAttackAndHealth(1, 9)
			.SetBoneCost(5)
			.SetDescription("A rabid dog, killed one of my ghouls.")
			.SetNames(NameHellHound, "Hell Hound")
			.Build()
		);
	}
}

