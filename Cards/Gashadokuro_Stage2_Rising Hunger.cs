using DiskCardGame;

namespace GrimoraMod;

public partial class GrimoraPlugin
{
	public const string NameRisingHunger = $"{GUID}_RisingHunger";

	private void Add_Card_Gashadokuro_Stage2_RisingHunger()
	{
		CardBuilder.Builder
			.SetAbilities(Ability.Evolve, Ability.QuadrupleBones)
			.SetBaseAttackAndHealth(0, 2)
			.SetEvolve(NameGashadokuro, 1)
			// .SetDescription("THE CYCLE OF THE MUMMY LORD, NEVER ENDING.")
			.SetNames(NameRisingHunger, "Rising Hunger")
			.Build();
	}
}
