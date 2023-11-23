using DiskCardGame;

namespace GrimoraMod;

public partial class GrimoraPlugin
{
	public const string NameIceCube = $"{GUID}_IceCube";

	private void Add_Card_IceCube()
	{
		CardBuilder.Builder
		 .SetAsNormalCard()
		 .SetAbilities(ColdFront.ability)
		 .SetBaseAttackAndHealth(1, 1)
		 .SetBoneCost(4)
		 .SetDescription("Those who strike at it will end up sharing their frosty fate!")
		 .SetTraits(Trait.DeathcardCreationNonOption)
		 .SetNames(NameIceCube, "Ice Cube")
		 .Build();
	}
}
