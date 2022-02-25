using APIPlugin;
using DiskCardGame;

namespace GrimoraMod;

public partial class GrimoraPlugin
{
	public static readonly CardInfo Catacomb = Add_Catacomb();

	public const string NameCatacomb = "GrimoraMod_Catacomb";

	private static CardInfo Add_Catacomb()
	{
		return AddCardToPool(
			CardBuilder.Builder
				.SetAsRareCard()
				.SetAbilities(SpecialTriggeredAbility.Lammergeier)
				.SetBaseAttackAndHealth(0, 10)
				.SetBoneCost(10)
				.SetNames(NameCatacomb, "Catacomb")
				.SetDescription("Its power scales in proportion to your Bones.")
				.Build()
		);
	}
}
