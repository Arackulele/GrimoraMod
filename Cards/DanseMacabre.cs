using APIPlugin;

namespace GrimoraMod;

public partial class GrimoraPlugin
{
	public const string NameDanseMacabre = "GrimoraMod_DanseMacabre";
	
	private void Add_DanseMacabre()
	{
		NewCard.Add(CardBuilder.Builder
			.SetAsRareCard()
			.SetAbilities(AlternatingStrike.ability, Erratic.ability)
			.SetBaseAttackAndHealth(3, 3)
			.SetBoneCost(8)
			.SetNames(NameDanseMacabre, "Danse Macabre")
			.SetDescription("They can never decide, truly a painful existance...")
			.Build()
		);
	}
}
