namespace GrimoraMod;

public partial class GrimoraPlugin
{
	public const string NamePirateFirstMateSnag = "GrimoraMod_PirateFirstMateSnag";

	private void Add_PirateFirstMateSnag()
	{
		CardBuilder.Builder
			.SetAsNormalCard()
			.SetAbilities(HookLineAndSinker.ability, SeaLegs.ability)
			.SetBaseAttackAndHealth(2, 2)
			.SetBoneCost(7)
			.SetNames(NamePirateFirstMateSnag, "First Mate Snag")
			.Build();
	}
}
