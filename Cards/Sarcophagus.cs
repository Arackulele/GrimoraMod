using DiskCardGame;

namespace GrimoraMod;

public partial class GrimoraPlugin
{
	public const string NameSarcophagus = $"{GUID}_Sarcophagus";

	private void Add_Sarcophagus()
	{
		CardBuilder.Builder
			.SetAsNormalCard()
			.SetAbilities(Ability.Evolve)
			.SetBaseAttackAndHealth(0, 2)
			.SetBoneCost(4)
			.SetEvolve(NameMummy, 1)
			.SetDescription("THE CYCLE OF THE MUMMY LORD, NEVER ENDING.")
			.SetNames(NameSarcophagus, "Sarcophagus")
			.Build();
	}
}
