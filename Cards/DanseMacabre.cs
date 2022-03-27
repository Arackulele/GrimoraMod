namespace GrimoraMod;

public partial class GrimoraPlugin
{
	public const string NameDanseMacabre = $"{GUID}_DanseMacabre";

	private void Add_Card_DanseMacabre()
	{
		CardBuilder.Builder
			.SetAsNormalCard()
			.SetAbilities(AlternatingStrike.ability, Erratic.ability)
			.SetBaseAttackAndHealth(3, 3)
			.SetBoneCost(6)
			.SetDescription("THEY CAN NEVER DECIDE, TRULY A PAINFUL EXISTENCE...")
			.SetNames(NameDanseMacabre, "Danse Macabre")
			.Build();
	}
}
