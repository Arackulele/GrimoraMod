using DiskCardGame;

namespace GrimoraMod;

public partial class GrimoraPlugin
{
	public const string NameMassGrave = $"{GUID}_MassGrave";

	private void Add_Gashadokuro_Stage1_MassGrave()
	{
		CardBuilder.Builder
			.SetAsNormalCard()
			.SetAbilities(Ability.Evolve, Ability.QuadrupleBones)
			.SetBaseAttackAndHealth(0, 2)
			.SetBoneCost(4)
			.SetEvolve(NameRisingHunger, 1)
			// .SetDescription("Over time the Mass grave gathers bones, collecting them until it is destroyed.")
			.SetNames(NameMassGrave, "Mass Grave")
			.Build();
	}
}
