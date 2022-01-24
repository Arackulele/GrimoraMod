using APIPlugin;
using DiskCardGame;

namespace GrimoraMod;

public partial class GrimoraPlugin
{
	public const string NameDeadPets = "ara_DeadPets";

	private void AddAra_DeadPets()
	{
		List<Ability> abilities = new List<Ability>
		{
			Ability.DrawCopyOnDeath,
			Ability.Brittle
		};

		NewCard.Add(CardBuilder.Builder
			.SetAsRareCard()
			.SetAbilities(abilities)
			.SetBaseAttackAndHealth(3, 1)
			.SetBoneCost(4)
			.SetDescription("The undying underlings of the Pharaoh.")
			.SetNames(NameDeadPets, "Pharaoh's Pets")
			.Build()
		);
	}
}
