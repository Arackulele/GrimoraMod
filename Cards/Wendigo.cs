using APIPlugin;
using DiskCardGame;

namespace GrimoraMod;

public partial class GrimoraPlugin
{
	public const string NameWendigo = "ara_Wendigo";

	private void AddAra_Wendigo()
	{
		List<Ability> abilities = new List<Ability>
		{
			Ability.DebuffEnemy,
			Ability.Strafe
		};

		NewCard.Add(CardBuilder.Builder
			.SetAsRareCard()
			.SetAbilities(abilities)
			.SetBaseAttackAndHealth(2, 2)
			.SetBoneCost(5)
			.SetDescription("A sense of dread consumes you as you realize you are not alone in these woods.")
			.SetNames(NameWendigo, "Wendigo")
			.Build()
		);
	}
}
