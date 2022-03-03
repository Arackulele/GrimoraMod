namespace GrimoraMod;

public partial class GrimoraPlugin
{
	public const string NamePirateCaptainYellowbeard = "GrimoraMod_PirateCaptainYellowbeard";

	private void Add_PirateCaptainYellowbeard()
	{
		CardBuilder.Builder
			.SetAsRareCard()
			.SetAbilities(BuffCrewMates.ability)
			.SetBaseAttackAndHealth(4, 4)
			.SetBoneCost(10)
			.SetNames(NamePirateCaptainYellowbeard, "Captain Yellowbeard")
			.Build();
	}
}
