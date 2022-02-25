using DiskCardGame;

namespace GrimoraMod;

public partial class GrimoraPlugin
{
	public const string NameDrownedSoul = "GrimoraMod_DrownedSoul";

	private void Add_DrownedSoul()
	{
		CardBuilder.Builder
			.SetAsNormalCard()
			.SetAbilities(Ability.Deathtouch, Ability.Submerge)
			.SetBaseAttackAndHealth(1, 1)
			.SetDescription("GOING INTO THAT WELL WASN'T THE BEST IDEA...")
			.SetEnergyCost(5)
			.SetNames(NameDrownedSoul, "Drowned Soul")
			.Build();
	}
}
