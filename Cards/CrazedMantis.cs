using APIPlugin;
using DiskCardGame;

namespace GrimoraMod;

public partial class GrimoraPlugin
{
	public const string NameCrazedMantis = "ara_CrazedMantis";

	private void AddAra_CrazedMantis()
	{
		NewCard.Add(CardBuilder.Builder
			.SetAsNormalCard()
			.SetAbilities(Ability.SplitStrike)
			.SetBaseAttackAndHealth(1, 1)
			.SetBoneCost(4)
			.SetDescription("The poor mantis has gone insane.")
			.SetNames(NameCrazedMantis, "Crazed Mantis")
			.Build()
		);
	}
}
