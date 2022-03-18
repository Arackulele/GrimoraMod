using DiskCardGame;

namespace GrimoraMod;

public partial class GrimoraPlugin
{
	public const string NameFesteringWretch = $"{GUID}_FesteringWretch";

	private void Add_FesteringWretch()
	{
		CardBuilder.Builder
			.SetAsNormalCard()
			.SetAbilities(Ability.DebuffEnemy)
			.SetBaseAttackAndHealth(1, 1)
			.SetBoneCost(3)
			.SetDescription("THE FESTERING WRETCH, IT IS SAID ITS ODOR IS SO STRONG EVEN THE UNDEAD CAN FEEL IT.")
			.SetNames(NameFesteringWretch, "Festering Wretch")
			.Build();
	}
}
