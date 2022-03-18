using DiskCardGame;

namespace GrimoraMod;

public partial class GrimoraPlugin
{
	public const string NameFamily = $"{GUID}_Family";

	private void Add_Family()
	{
		CardBuilder.Builder
			.SetAsNormalCard()
			.SetAbilities(Ability.QuadrupleBones)
			.SetBaseAttackAndHealth(1, 2)
			.SetBoneCost(4)
			.SetDescription("THE FAMILY WISHES TO REST IN PEACE.")
			.SetNames(NameFamily, "The Walkers")
			.Build();
	}
}
