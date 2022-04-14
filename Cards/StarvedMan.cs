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
		 .SetDescription("A rebellious Starvation who has chosen to fight with us and not against us!")
		 .SetNames(NameStarvedMan, "Starved Man")
		 .Build();
	}
}
