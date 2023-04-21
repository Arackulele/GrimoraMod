namespace GrimoraMod;

public partial class GrimoraPlugin
{
	public const string NameStarvedMan = $"{GUID}_StarvedMan";

	private void Add_Card_StarvedMan()
	{
		CardBuilder.Builder
		 .SetAsRareCard()
		 .SetAbilities(Malnourishment.ability)
		 .SetBaseAttackAndHealth(3, 3)
		 .SetBoneCost(4)
		 .SetDescription("DEATH IS A CRUEL FATE. STARVATION AN EVEN CRUELER ONE. WHY DO YOU FIGHT, OH DYING CHILD.")
		 .SetNames(NameStarvedMan, "Starved Man")
		 .Build();
	}
}
