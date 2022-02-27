namespace GrimoraMod;

public partial class GrimoraPlugin
{
	public const string NameWyvern = "GrimoraMod_Wyvern";

	private void Add_Wyvern()
	{
		CardBuilder.Builder
			.SetAsRareCard()
			.SetAbilities(ActivatedEnergyDrawWyvern.ability)
			.SetBaseAttackAndHealth(1, 1)
			.SetBoneCost(3)
			.SetDescription("A SKELETAL BEAST, IT CALLS IN MORE OF ITS KIND.")
			.SetNames(NameWyvern, "Wyvern")
			.Build();
	}
}
