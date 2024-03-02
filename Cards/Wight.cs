using DiskCardGame;

namespace GrimoraMod;

public partial class GrimoraPlugin
{
	public const string NameWight = $"{GUID}_Wight";

	private void Add_Card_Wight()
	{
		CardBuilder.Builder
			.SetAsNormalCard()
			.SetAbilities(Ability.CorpseEater)
			.SetBaseAttackAndHealth(2, 1)
			.SetBoneCost(6)
			.SetDescription("Hideous beings that will jump out to consume freshly killed corpses. They only spare the bones of the victim.")
			.SetNames(NameWight, "Wight")
			.Build();
	}
}
