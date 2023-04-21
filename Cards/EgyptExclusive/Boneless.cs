using DiskCardGame;

namespace GrimoraMod;

public partial class GrimoraPlugin
{
	public const string NameBoneless = $"{GUID}_Boneless";

	private void Add_Card_Boneless()
	{
		CardBuilder.Builder
			.SetAbilities(Boneless.ability, GainAttackNoBones.ability)
			.SetBaseAttackAndHealth(1, 3)
			.SetNames(NameBoneless, "Boneless")
			.SetBoneCost(4)
			.Build();
	}
}
