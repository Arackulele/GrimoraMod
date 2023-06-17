using DiskCardGame;

namespace GrimoraMod;

public partial class GrimoraPlugin
{
	public const string NameHellHound = $"{GUID}_HellHound";

	private void Add_Card_HellHound()
	{
		CardBuilder.Builder
			.SetAbilities(Ability.WhackAMole, Ability.Reach, Ability.MadeOfStone)
			.SetSpecialAbilities(GainAttackBones.FullSpecial.Id)
			.SetBaseAttackAndHealth(1, 9)
			.SetBoneCost(5)
			.SetDescription("A RABID DOG. IT KILLED ONE OF MY GHOULS.")
			.SetNames(NameHellHound, "Hell Hound")
			.SetTraits(Trait.Uncuttable)
			.SetSpecialStatIcon(GainAttackBones.FullStatIcon.Id)
			.Build();
	}
}
