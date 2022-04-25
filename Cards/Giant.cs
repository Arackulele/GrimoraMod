using DiskCardGame;

namespace GrimoraMod;

public partial class GrimoraPlugin
{
	public const string NameGiant = $"{GUID}_Giant";

	private void Add_Card_Giant()
	{
		CardBuilder.Builder
		 .SetAsNormalCard()
		 .SetAbilities(Ability.QuadrupleBones, Ability.SplitStrike)
		 .SetBaseAttackAndHealth(3, 8)
		 .SetBoneCost(15)
		 .SetDescription("TRULY A SIGHT TO BEHOLD.")
		 .SetNames(NameGiant, "Giant")
		 .SetTraits(Trait.Giant)
		 .Build()
			;
	}
}
