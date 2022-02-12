using APIPlugin;
using DiskCardGame;

namespace GrimoraMod;

public partial class GrimoraPlugin
{
	public const string NameProject = "GrimoraMod_Project";

	private void Add_Project()
	{
		NewCard.Add(CardBuilder.Builder
			.SetAsNormalCard()
			.SetAbilities(Erratic.ability, Ability.SplitStrike)
			.SetBaseAttackAndHealth(1, 3)
			.SetBoneCost(6)
			.SetNames(NameProject, "Project")
			.SetDescription("An experiment gone wrong, or right.It depends on your world view")
			.Build()
		);
	}
}
