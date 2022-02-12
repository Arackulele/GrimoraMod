using APIPlugin;
using DiskCardGame;

namespace GrimoraMod;

public partial class GrimoraPlugin
{
	public const string NameWendigo = "GrimoraMod_Wendigo";

	private void Add_Wendigo()
	{
		NewCard.Add(CardBuilder.Builder
			.SetAsRareCard()
			.SetAbilities(Ability.DebuffEnemy, Ability.Strafe)
			.SetBaseAttackAndHealth(2, 2)
			.SetBoneCost(5)
			.SetDescription("A sense of dread consumes you as you realize you are not alone in these woods.")
			.SetNames(NameWendigo, "Wendigo")
			.Build()
		);
	}
}
