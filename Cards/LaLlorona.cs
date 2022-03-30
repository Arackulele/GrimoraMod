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
			.SetBaseAttackAndHealth(1, 1)
			// .SetDescription("GOING INTO THAT WELL WASN'T THE BEST IDEA...")
			.SetEnergyCost(3)
			.SetNames(NameLaLlorona, "La Llorona")
			.Build();
	}
}
