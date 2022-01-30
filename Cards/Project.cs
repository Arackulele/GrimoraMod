using APIPlugin;
using DiskCardGame;

namespace GrimoraMod;

public partial class GrimoraPlugin
{
	public const string NameProject = "ara_Project";

	private void AddAra_Project()
	{
		NewCard.Add(CardBuilder.Builder
			.SetAsNormalCard()
			.SetAbilities(Ability.TriStrike)
			.SetBaseAttackAndHealth(1, 1)
			.SetBoneCost(8)
			.SetNames(NameProject, "Project")
			// .SetDescription("A vicious pile of bones. You can have it...")
			.Build()
		);
	}
}
