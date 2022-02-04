using APIPlugin;
using DiskCardGame;

namespace GrimoraMod;

public partial class GrimoraPlugin
{
	public const string NameDrownedSoul = "ara_DrownedSoul";

	private void AddAra_DrownedSoul()
	{
		NewCard.Add(CardBuilder.Builder
			.SetAsNormalCard()
			.SetAbilities(Ability.Deathtouch, Ability.Submerge)
			.SetBaseAttackAndHealth(1, 1)
			.SetEnergyCost(5)
			.SetDescription("Going into that well wasn't the best idea...")
			.SetNames(NameDrownedSoul, "Drowned Soul")
			.Build()
		);
	}
}
