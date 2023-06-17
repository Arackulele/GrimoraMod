using DiskCardGame;

namespace GrimoraMod;

public partial class GrimoraPlugin
{
	public const string NameCenturion = $"{GUID}_Centurion";

	private void Add_Card_Centurion()
	{
		CardBuilder.Builder
			.SetAsNormalCard()
			.SetAbilities(Ability.DeathShield)
			.SetBaseAttackAndHealth(1, 4)
			.SetBoneCost(6)
			.SetNames(NameCenturion, "Centurion")
			.SetDescription("A heavily armoured warrior, last of his century, leader to none.")
			.Build();
	}
}
