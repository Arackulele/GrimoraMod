namespace GrimoraMod;

public partial class GrimoraPlugin
{
	public const string NamePirateCaptainYellowbeard = $"{GUID}_PirateCaptainYellowbeard";

	private void Add_Card_PirateCaptainYellowbeard()
	{
		CardBuilder.Builder
			.SetAsRareCard()
			.SetAbilities(BuffCrewMates.ability, Anchored.ability)
			.SetBaseAttackAndHealth(2, 2)
			.SetBoneCost(7)
			.SetNames(NamePirateCaptainYellowbeard, "Yellowbeard")
			.Build();
	}
}
