using DiskCardGame;

namespace GrimoraMod;

public partial class GrimoraPlugin
{
	public const string NameVoodooDoll = $"{GUID}_Voodoo_Doll";

	private void Add_Card_Voodoo_Doll()
	{
		CardBuilder.Builder
		 .SetBaseAttackAndHealth(0, 2)
		 .SetNames(NameVoodooDoll, "Voodoo Doll")
		 .SetTraits(Trait.Structure, Trait.Terrain)
		 .SetAbilities(Haunter.ability, InvertedStrike.ability)
		 .Build();
	}
}
