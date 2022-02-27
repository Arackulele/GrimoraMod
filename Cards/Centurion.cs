using DiskCardGame;

namespace GrimoraMod;

public partial class GrimoraPlugin
{
	public const string NameCenturion = "GrimoraMod_Centurion";

	private void Add_Centurion()
	{
		CardBuilder.Builder
			.SetAsNormalCard()
			.SetAbilities(Ability.DeathShield)
			.SetBaseAttackAndHealth(1, 4)
			.SetBoneCost(6)
			.SetNames(NameCenturion, "Centurion")
			// .SetDescription("NOT YOUR ORDINARY UNDEAD, THEY SEARCHED THROUGH A SCRAPYARD FOR THIS GEAR.")
			.Build();
	}
}
