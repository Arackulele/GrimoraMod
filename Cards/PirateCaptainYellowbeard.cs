namespace GrimoraMod;

public partial class GrimoraPlugin
{
	public const string NamePirateCaptainYellowbeard = "GrimoraMod_PirateCaptainYellowbeard";

	private void Add_PirateCaptainYellowbeard()
	{
		CardBuilder.Builder
			.SetAsRareCard()
			.SetAbilities(BuffCrewMates.ability, SeaLegs.ability)
			.SetBaseAttackAndHealth(2, 2)
			.SetBoneCost(7)
			.SetNames(NamePirateCaptainYellowbeard, "Yellowbeard")
			.Build();
	}
}
