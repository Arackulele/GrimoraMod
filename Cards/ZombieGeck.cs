using DiskCardGame;

namespace GrimoraMod;

public partial class GrimoraPlugin
{
	public const string NameZombieGeck = "GrimoraMod_ZombieGeck";

	private void Add_ZombieGeck()
	{
		CardBuilder.Builder
			//Removing Zomb geck for theming reasons, this was only here as filler
			.SetAsRareCard()
			.SetAbilities(Ability.Brittle)
			.SetBaseAttackAndHealth(2, 1)
			.SetBoneCost(1)
			.SetDescription("A BIT FAMISHED. COULD USE A BITE TO EAT.")
			.SetNames(NameZombieGeck, "Zomb-Geck")
			.Build();
	}
}
