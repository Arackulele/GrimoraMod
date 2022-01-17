using APIPlugin;
using DiskCardGame;
using GrimoraMod.Properties;

namespace GrimoraMod;

public partial class GrimoraPlugin
{
	public const string NameTombRobber = "ara_TombRobber";

	private void AddAra_TombRobber()
	{
		NewCard.Add(CardBuilder.Builder
			.AsNormalCard()
			.WithAbilities(Ability.ExplodeOnDeath)
			.WithBaseAttackAndHealth(0, 1)
			.WithDescription("Nothing... Nothing again... No treasure is left anymore.")
			.WithNames(NameTombRobber, "Tomb Robber")
			.WithPortrait(Resources.TombRobber)
			.Build()
		);
	}
}