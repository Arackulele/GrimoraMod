using DiskCardGame;

namespace GrimoraMod;

public partial class GrimoraPlugin
{
	public const string NameWarthr = $"{GUID}_Warthr";

	private void Add_Card_Warthr()
	{
		CardBuilder.Builder
			.SetAsNormalCard()
			.SetAbilities(Ability.WhackAMole)
			.SetBaseAttackAndHealth(0, 5)
			.SetEnergyCost(4)
			.SetDescription("THIS GELID SPECTER ENVELOPS WOULD-BE ATTACKERS IN AN ICY MIST.")
			.SetNames(NameWarthr, "Warthr")
			.Build();
	}
}
