using DiskCardGame;

namespace GrimoraMod;

public partial class GrimoraPlugin
{
	public const string NameMassGrave = $"{GUID}_MassGrave";

	private void Add_Card_Gashadokuro_Stage1_MassGrave()
	{
		CardBuilder.Builder
			.SetAsRareCard()
			.SetAbilities(Ability.Evolve, Ability.QuadrupleBones)
			.SetBaseAttackAndHealth(0, 2)
			.SetBoneCost(4)
			.SetDescription("WHAT CREATED THESE BONES, WAR, FAMINE OR ANOTHER TRAGEDY? It may be insignificant now, but I fear what it might become...")
			.SetEvolve(NameRisingHunger, 1)
			.SetNames(NameMassGrave, "Mass Grave")
			.Build();
	}
}
