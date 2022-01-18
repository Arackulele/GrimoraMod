using APIPlugin;
using DiskCardGame;

namespace GrimoraMod;

public partial class GrimoraPlugin
{
	public const string NameBoneSerpent = "ara_BoneSerpent";

	private void AddAra_BoneSerpent()
	{
		NewCard.Add(CardBuilder.Builder
			.AsNormalCard()
			.WithAbilities(Ability.Deathtouch)
			.WithBaseAttackAndHealth(1, 1)
			.WithBonesCost(4)
			.WithNames(NameBoneSerpent, "Bone Serpent")
			.WithDescription("The poison strike will melt even the most dense bones.")
			.Build()
		);
	}
}