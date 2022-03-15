using DiskCardGame;

namespace GrimoraMod;

public partial class GrimoraPlugin
{
	public const string NameHellHound = "GrimoraMod_HellHound";

	private void Add_HellHound()
	{
		CardBuilder.Builder
			.SetAbilities(Ability.WhackAMole, Ability.Reach)
			.SetBaseAttackAndHealth(1, 9)
			.SetBoneCost(5)
			.SetDescription("A RABID DOG. IT KILLED ONE OF MY GHOULS.")
			.SetNames(NameHellHound, "Hell Hound")
			.SetSpecialStatIcon(GainAttackBones.FullStatIcon.Id)
			.Build();
	}
}
