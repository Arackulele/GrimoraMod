using DiskCardGame;

namespace GrimoraMod;

public partial class GrimoraPlugin
{
	public const string NameLaLlorona = $"{GUID}_LaLlorona";

	private void Add_Card_LaLlorona()
	{
		CardBuilder.Builder
			.SetAsNormalCard()
			.SetAbilities(Ability.Submerge)
			.SetEvolve(NameNixie, 1)
			.SetBaseAttackAndHealth(1, 1)
			.SetDescription("After drowning her children and then herself, she continues to drown others.")
			.SetEnergyCost(3)
			.SetNames(NameLaLlorona, "La Llorona")
			.Build();
	}
}
