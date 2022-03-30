namespace GrimoraMod;

public partial class GrimoraPlugin
{
	public const string NamePirateFirstMateSnag = $"{GUID}_PirateFirstMateSnag";

	private void Add_Card_PirateFirstMateSnag()
	{
		CardBuilder.Builder
			.SetAsNormalCard()
			.SetAbilities(HookLineAndSinker.ability, Anchored.ability)
			.SetBaseAttackAndHealth(2, 2)
			.SetBoneCost(7)
			.SetNames(NamePirateFirstMateSnag, "First Mate Snag")
			.Build();
	}
}
