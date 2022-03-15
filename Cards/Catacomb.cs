namespace GrimoraMod;

public partial class GrimoraPlugin
{
	public const string NameCatacomb = "GrimoraMod_Catacomb";

	private static void Add_Catacomb()
	{
		CardBuilder.Builder
			.SetAsNormalCard()
			.SetBaseAttackAndHealth(0, 10)
			.SetBoneCost(10)
			.SetDescription("Its power scales in proportion to your Bones.")
			.SetNames(NameCatacomb, "Catacomb")
			.SetSpecialStatIcon(LammergeierAttack.SpecialStatIcon)
			.Build(LammergeierAttack.NewSpecialAbility.id);
	}
}
