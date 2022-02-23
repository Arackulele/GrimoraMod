using APIPlugin;

namespace GrimoraMod;

public partial class GrimoraPlugin
{
	public const string NameWyvern = "GrimoraMod_Wyvern";

	private void Add_Wyvern()
	{
		NewCard.Add(CardBuilder.Builder
			.SetAsRareCard()
			.SetAbilities(ActivatedEnergyDrawWyvern.ability)
			.SetBaseAttackAndHealth(1, 1)
			.SetBoneCost(4)
			.SetDescription("A SKELETAL BEAST, IT CALLS IN MORE OF ITS KIND.")
			.SetNames(NameWyvern, "Wyvern")
			.Build()
		);
	}
}
