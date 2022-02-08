using APIPlugin;
using DiskCardGame;

namespace GrimoraMod;

public partial class GrimoraPlugin
{
	public const string NameHeadlessHorseman = "GrimoraMod_HeadlessHorseman";

	private void AddAra_HeadlessHorseman()
	{
		NewCard.Add(CardBuilder.Builder
			.SetAsRareCard()
			.SetAbilities(Ability.Flying, Ability.Strafe)
			.SetBaseAttackAndHealth(5, 5)
			.SetBoneCost(13)
			.SetDescription("The apocalypse is soon.")
			.SetNames(NameHeadlessHorseman, "Headless Horseman")
			.Build()
		);
	}
}
