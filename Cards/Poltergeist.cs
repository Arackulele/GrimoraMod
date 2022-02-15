using APIPlugin;
using DiskCardGame;

namespace GrimoraMod;

public partial class GrimoraPlugin
{
	public const string NamePoltergeist = "GrimoraMod_Poltergeist";

	private void Add_Poltergeist()
	{
		NewCard.Add(CardBuilder.Builder
			.SetAsNormalCard()
			.SetAbilities(Ability.Flying, Ability.Submerge)
			.SetBaseAttackAndHealth(1, 1)
			.SetEnergyCost(3)
			.SetDescription("A SKILLED HAUNTING GHOST. HANDLE WITH CAUTION.")
			.SetNames(NamePoltergeist, "Poltergeist")
			.Build()
		);
	}
}
