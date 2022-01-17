using APIPlugin;
using DiskCardGame;
using GrimoraMod.Properties;

namespace GrimoraMod;

public partial class GrimoraPlugin
{
	public const string NameZombieGeck = "ara_ZombieGeck";

	private void AddAra_ZombieGeck()
	{
		NewCard.Add(CardBuilder.Builder
			.AsRareCard()
			.WithAbilities(Ability.Brittle)
			.WithBaseAttackAndHealth(2, 1)
			.WithBonesCost(1)
			.WithDescription("A bit famished. Could use a bite to eat.")
			.WithNames(NameZombieGeck, "Zomb-Geck")
			.WithPortrait(Resources.Geck)
			.Build()
		);
	}
}