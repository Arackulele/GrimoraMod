using DiskCardGame;

namespace GrimoraMod;

public partial class GrimoraPlugin
{
	public const string NameMudWorm = "GrimoraMod_MudWorm";

	private void Add_MudWorm()
	{
		CardBuilder.Builder
			// .SetAsNormalCard()
			.SetAbilities(Ability.DebuffEnemy)
			.SetBaseAttackAndHealth(2, 1)
			.SetBoneCost(5)
			.SetDescription("LIKE A TRUE WORM, LOVES TO DIG IN THE DIRT.")
			.SetNames(NameMudWorm, "Mud Worm")
			.Build();
	}
}
