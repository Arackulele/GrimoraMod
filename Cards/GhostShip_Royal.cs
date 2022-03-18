using DiskCardGame;

namespace GrimoraMod;

public partial class GrimoraPlugin
{
	public const string NameGhostShipRoyal = $"{GUID}_GhostShipRoyal";

	private void Add_GhostShipRoyal()
	{
		CardBuilder.Builder
			.SetAbilities(SeaLegs.ability)
			.SetSpecialAbilities(CreateRoyalsCrewMate.FullSpecial.Id)
			.SetAppearance(CardAppearanceBehaviour.Appearance.RareCardBackground)
			.SetBaseAttackAndHealth(0, 15)
			.SetNames(NameGhostShipRoyal, "")
			.Build();
	}
}
