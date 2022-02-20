using APIPlugin;

namespace GrimoraMod;

public partial class GrimoraPlugin
{
	public const string NameDanseMacabre = "GrimoraMod_DanseMacabre";
	
	private void Add_DanseMacabre()
	{
		NewCard.Add(CardBuilder.Builder
			.SetAsNormalCard()
			.SetAbilities(AlternatingStrike.ability, Erratic.ability)
			.SetBaseAttackAndHealth(3, 3)
			.SetBoneCost(8)
			.SetDescription("THEY CAN NEVER DECIDE, TRULY A PAINFUL EXISTENCE...")
			.SetNames(NameDanseMacabre, "Danse Macabre")
			.Build()
		);
	}
}
