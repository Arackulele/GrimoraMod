using APIPlugin;
using DiskCardGame;

namespace GrimoraMod;

public partial class GrimoraPlugin
{
	public const string NameZombieGeck = "GrimoraMod_ZombieGeck";

	private void Add_ZombieGeck()
	{
		NewCard.Add(CardBuilder.Builder
			.SetAsRareCard()
			.SetAbilities(Ability.Brittle)
			.SetBaseAttackAndHealth(2, 1)
			.SetBoneCost(1)
			.SetDescription("A BIT FAMISHED. COULD USE A BITE TO EAT.")
			.SetNames(NameZombieGeck, "Zomb-Geck")
			.Build()
		);
	}
}
