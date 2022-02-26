namespace GrimoraMod;

public partial class GrimoraPlugin
{
	public const string NameCatacomb = "GrimoraMod_Catacomb";

	private static void Add_Catacomb()
	{
		CardBuilder.Builder
			.SetAsRareCard()
			.SetAbilities(LammergeierAttack.NewSpecialAbility.specialTriggeredAbility)
			.SetBaseAttackAndHealth(0, 10)
			.SetBoneCost(10)
			.SetNames(NameCatacomb, "Catacomb")
			.SetDescription("Its power scales in proportion to your Bones.")
			.SetSpecialStatIcon(LammergeierAttack.SpecialStatIcon)
			.Build();
	}
}
