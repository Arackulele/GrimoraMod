using DiskCardGame;

namespace GrimoraMod;

public partial class GrimoraPlugin
{
	public const string NamePiratePolly = $"{GUID}_PiratePolly";

	private void Add_Card_PiratePolly()
	{
		CardBuilder.Builder
		 .SetAsNormalCard()
		 .SetAbilities(Ability.Brittle, Ability.Flying, Anchored.ability)
		 .SetBaseAttackAndHealth(3, 1)
		 .SetBoneCost(4)
		 .SetDescription("Parrots usually make great sea-side companions, this one just ate a poisoned cracker...")
		 .SetNames(NamePiratePolly, "Polly")
		 .Build();
	}
}
