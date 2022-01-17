using APIPlugin;
using DiskCardGame;
using GrimoraMod.Properties;

namespace GrimoraMod;

public partial class GrimoraPlugin
{
	public const string NameCrazedMantis = "ara_CrazedMantis";

	private void AddAra_CrazedMantis()
	{
		NewCard.Add(CardBuilder.Builder
			.AsNormalCard()
			.WithAbilities(Ability.SplitStrike)
			.WithBaseAttackAndHealth(1, 1)
			.WithBonesCost(4)
			.WithDescription("The poor mantis has gone insane.")
			.WithNames(NameCrazedMantis, "Crazed Mantis")
			.WithPortrait(Resources.CrazedMantis)
			.Build()
		);
	}
}