using APIPlugin;

namespace GrimoraMod;

public partial class GrimoraPlugin
{
	public const string NameArmoredZombie = "GrimoraMod_ArmoredZombie";

	private void AddAra_ArmoredZombie()
	{
		NewCard.Add(CardBuilder.Builder
			.SetAsNormalCard()
			.SetBaseAttackAndHealth(1, 6)
			.SetBoneCost(6)
			.SetNames(NameArmoredZombie, "Armored Zombie")
			.SetDescription("Not your ordinary Undead, they searched through a Scrapyard for this Gear.")
			.Build()
		);
	}
}
