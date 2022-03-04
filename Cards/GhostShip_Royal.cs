using DiskCardGame;

namespace GrimoraMod;

public partial class GrimoraPlugin
{
	public const string NameGhostShipRoyal = "GrimoraMod_GhostShipRoyal";

	private void Add_GhostShipRoyal()
	{
		CardBuilder.Builder
			.SetAbilities(CreateRoyalsCrewMate.ability)
			.SetAppearance(CardAppearanceBehaviour.Appearance.RareCardBackground)
			.SetBaseAttackAndHealth(0, 10)
			.SetNames(NameGhostShipRoyal, "")
			.Build();
	}
}
