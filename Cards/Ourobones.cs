using DiskCardGame;

namespace GrimoraMod;

public partial class GrimoraPlugin
{
	public const string NameOurobones = $"{GUID}_Ourobones";

	private void Add_Card_Ourobones()
	{
		CardBuilder.Builder
		 .SetAsRareCard()
		 .SetAbilities(CumulativeTorment.ability)
		 .SetBaseAttackAndHealth(1, 1)
		 .SetBoneCost(2)
		 .SetDescription("An endless cycle of life and death has taken a toll on these discarded remains.")
		 .SetNames(NameOurobones, "Ourobones")
		 .Build();
	}
}
