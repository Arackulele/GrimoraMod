using DiskCardGame;

namespace GrimoraMod;

public partial class GrimoraPlugin
{
	public const string NamePoltergeist = $"{GUID}_Poltergeist";

	private void Add_Card_Poltergeist()
	{
		CardBuilder.Builder
			.SetAsNormalCard()
			.SetAbilities(Ability.Flying, Ability.Submerge)
			.SetBaseAttackAndHealth(1, 1)
			.SetEnergyCost(3)
			.SetDescription("A SKILLED HAUNTING GHOST. HANDLE WITH CAUTION.")
			.SetNames(NamePoltergeist, "Poltergeist")
			.Build();
	}
}
