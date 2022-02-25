using DiskCardGame;

namespace GrimoraMod;

public partial class GrimoraPlugin
{
	public const string NameCatacomb = "GrimoraMod_Catacomb";

	private static void Add_Catacomb()
	{
		CardBuilder.Builder
			.SetAsRareCard()
			.SetAbilities(SpecialTriggeredAbility.Lammergeier)
			.SetBaseAttackAndHealth(0, 10)
			.SetBoneCost(10)
			.SetNames(NameCatacomb, "Catacomb")
			.SetDescription("Its power scales in proportion to your Bones.")
			.Build();
	}
}
