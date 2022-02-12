using APIPlugin;
using DiskCardGame;

namespace GrimoraMod;

public partial class GrimoraPlugin
{
	public const string NameSkelemagus = "GrimoraMod_Skelemagus";

	private void Add_Skelemagus()
	{
		NewCard.Add(CardBuilder.Builder
			.SetAsNormalCard()
			.SetAbilities(Ability.Brittle)
			.SetBaseAttackAndHealth(4, 1)
			.SetEnergyCost(5)
			.SetDescription("The Skelemagus, they have learned the ancient spell of Death.")
			.SetNames(NameSkelemagus, "Skelemagus")
			.Build()
		);
	}
}
