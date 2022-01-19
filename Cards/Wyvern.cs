using APIPlugin;
using DiskCardGame;

namespace GrimoraMod;

public partial class GrimoraPlugin
{
	public const string NameWyvern = "ara_Wyvern";

	private void AddAra_Wyvern()
	{
		NewCard.Add(CardBuilder.Builder
			.SetAsRareCard()
			.SetAbilities(Ability.DrawCopy)
			.SetBaseAttackAndHealth(1, 1)
			.SetBoneCost(5)
			.SetDescription("The wyvern army approaches.")
			.SetNames(NameWyvern, "Wyvern")
			.Build()
		);
	}
}
