namespace GrimoraMod;

public partial class GrimoraPlugin
{
	public const string NameArmoredZombie = "GrimoraMod_ArmoredZombie";

	private void Add_ArmoredZombie()
	{
		CardBuilder.Builder
			// .SetAsNormalCard()
			.SetBaseAttackAndHealth(1, 6)
			.SetBoneCost(6)
			.SetNames(NameArmoredZombie, "Armored Zombie")
			.SetDescription("NOT YOUR ORDINARY UNDEAD, THEY SEARCHED THROUGH A SCRAPYARD FOR THIS GEAR.")
			.Build();
	}
}
