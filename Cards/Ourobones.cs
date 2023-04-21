using DiskCardGame;

namespace GrimoraMod;

public partial class GrimoraPlugin
{
	public const string NameOurobones = $"{GUID}_Ourobones";

	private void Add_Card_Ourobones()
	{
		CardBuilder.Builder
		 .SetAsRareCard()
		 .SetAbilities(Ability.Brittle)
		 .SetSpecialAbilities(OurobonesCore.FullSpecial.Id)
		 .SetBaseAttackAndHealth(1, 1)
		 .SetBoneCost(2)
		 .SetDescription("IT IS SAID ALL IS WELL THAT ENDS WELL. IT IS SAID TIME IS LIKE AN OUROBOROS, IT DOES NOT END, IT ONLY REPEATS ITSELF.")
		 .SetNames(NameOurobones, "Ourobones")
		 .Build();
	}
}
