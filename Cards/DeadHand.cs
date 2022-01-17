using APIPlugin;
using DiskCardGame;
using GrimoraMod.Properties;

namespace GrimoraMod;

public partial class GrimoraPlugin
{
	public const string NameDeadHand = "ara_DeadHand";

	private void AddAra_DeadHand()
	{
		NewCard.Add(CardBuilder.Builder
			.AsNormalCard()
			.WithAbilities(Ability.DrawNewHand)
			.WithBaseAttackAndHealth(1, 1)
			.WithBonesCost(5)
			.WithDescription("Cut off from an ancient God, the Dead Hand took on its own Life.")
			.WithNames(NameDeadHand, "Dead Hand")
			.WithPortrait(Resources.DeadHand)
			.Build()
		);
	}
}