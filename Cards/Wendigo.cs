using APIPlugin;
using DiskCardGame;

namespace GrimoraMod;

public partial class GrimoraPlugin
{
	public const string NameWendigo = "GrimoraMod_Wendigo";

	private void Add_Wendigo()
	{
		NewCard.Add(CardBuilder.Builder
			.SetAsNormalCard()
			.SetAbilities(Ability.DebuffEnemy, Ability.Strafe)
			.SetBaseAttackAndHealth(1, 2)
			.SetBoneCost(5)
			.SetDescription("A SENSE OF DREAD CONSUMES YOU AS YOU REALIZE YOU ARE NOT ALONE IN THESE WOODS.")
			.SetNames(NameWendigo, "Wendigo")
			.Build()
		);
	}
}
