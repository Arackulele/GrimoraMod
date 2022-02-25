using DiskCardGame;

namespace GrimoraMod;

public partial class GrimoraPlugin
{
	public const string NameProject = "GrimoraMod_Project";

	private void Add_Project()
	{
		CardBuilder.Builder
			.SetAsNormalCard()
			.SetAbilities(Erratic.ability, Ability.SplitStrike)
			.SetBaseAttackAndHealth(1, 3)
			.SetBoneCost(5)
			.SetDescription("AN EXPERIMENT GONE WRONG, OR RIGHT. IT DEPENDS ON YOUR WORLD VIEW.")
			.SetNames(NameProject, "Project")
			.Build();
	}
}
