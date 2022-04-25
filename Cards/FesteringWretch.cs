using DiskCardGame;

namespace GrimoraMod;

public partial class GrimoraPlugin
{
	public const string NameFesteringWretch = $"{GUID}_FesteringWretch";

	private void Add_Card_FesteringWretch()
	{
		CardBuilder.Builder
			.SetAsNormalCard()
			.SetAbilities(Ability.DebuffEnemy)
			.SetBaseAttackAndHealth(1, 1)
			.SetBoneCost(3)
			.SetDescription("It's said their odor is so strong and repugnant, even the undead can feel it!")
			.SetNames(NameFesteringWretch, "Festering Wretch")
			.Build();
	}
}
