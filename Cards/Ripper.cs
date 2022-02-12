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
			.SetBoneCost(12)
			.SetNames(NameRipper, "Ripper")
			// .SetDescription("A vicious pile of bones. You can have it...")
			.Build()
		);
	}
}
