using APIPlugin;
using DiskCardGame;

namespace GrimoraMod;

public partial class GrimoraPlugin
{
	public const string NameBoneSerpent = "ara_BoneSerpent";

	private void AddAra_BoneSerpent()
	{
		NewCard.Add(CardBuilder.Builder
			.SetAsNormalCard()
			.SetAbilities(Ability.Deathtouch)
			.SetBaseAttackAndHealth(1, 1)
			.SetBoneCost(4)
			.SetNames(NameBoneSerpent, "Bone Serpent")
			.SetDescription("The poison strike will melt even the most dense bones.")
			.Build()
		);
	}
}
