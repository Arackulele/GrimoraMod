using APIPlugin;
using DiskCardGame;

namespace GrimoraMod;

public partial class GrimoraPlugin
{
	public const string NameRipper = "GrimoraMod_Ripper";

	private void Add_Ripper()
	{
		NewCard.Add(CardBuilder.Builder
			.SetAsRareCard()
			.SetAbilities(Ability.Brittle)
			.SetBaseAttackAndHealth(6, 6)
			.SetBoneCost(9)
			.SetNames(NameRipper, "Ripper")
			.SetDescription("When all hope is lost, you can always count on this Demon straight from hell!")
			.Build()
		);
	}
}
