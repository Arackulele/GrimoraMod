namespace GrimoraMod;

public partial class GrimoraPlugin
{
	public const string NamePirateFirstMateSnag = "GrimoraMod_PirateFirstMateSnag";

	private void Add_PirateFirstMateSnag()
	{
		CardBuilder.Builder
			// .SetAbilities(Raider.ability)
			.SetBaseAttackAndHealth(1, 2)
			.SetNames(NamePirateFirstMateSnag, "Fist Mate Snag")
			.Build();
	}
}
