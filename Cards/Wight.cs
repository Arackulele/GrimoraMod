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
			.SetBoneCost(5)
			.SetDescription("Hideous beings that will jump out to consume freshly killed corpses. They'll spare any expense to do so.")
			.SetNames(NameWight, "Wight")
			.Build();
	}
}
