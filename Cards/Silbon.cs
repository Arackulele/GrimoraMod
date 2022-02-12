using APIPlugin;
using DiskCardGame;

namespace GrimoraMod;

public partial class GrimoraPlugin
{
	public const string NameSilbon = "GrimoraMod_Silbon";

	private void Add_Silbon()
	{
		NewCard.Add(CardBuilder.Builder
			.SetAsRareCard()
			.SetAbilities(InvertedStrike.ability, Ability.Strafe)
			.SetBaseAttackAndHealth(3, 2)
			.SetBoneCost(7)
			.SetDescription("The Silbon, a skilled hunter. WHat did it hunt?Unknown.")
			.SetNames(NameSilbon, "Silbon")
			.Build()
		);
	}
}
