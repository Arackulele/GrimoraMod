using APIPlugin;
using DiskCardGame;

namespace GrimoraMod;

public partial class GrimoraPlugin
{
	public const string NameDeadPets = "ara_DeadPets";

	private void AddAra_DeadPets()
	{
		NewCard.Add(CardBuilder.Builder
			.SetAsRareCard()
			.SetAbilities(Ability.Brittle, Ability.DrawCopyOnDeath)
			.SetBaseAttackAndHealth(3, 1)
			.SetBoneCost(4)
			.SetDescription("The undying underlings of the Pharaoh.")
			.SetNames(NameDeadPets, "Pharaoh's Pets")
			.Build()
		);
	}
}
