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
		 .SetDescription("A CORPSE WHO DIED WITH HIS HEAD IN A GLACIAL STREAM. AFTER HIS REANIMATION, HE COULDN'T BEAR TO PART WITH THE ICE THAT HAD FORMED.")
		 .SetTraits(Trait.DeathcardCreationNonOption)
		 .SetNames(NameIceCube, "Ice Cube")
		 .Build();
	}
}
