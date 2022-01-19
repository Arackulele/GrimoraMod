using APIPlugin;
using DiskCardGame;

namespace GrimoraMod;

public partial class GrimoraPlugin
{
	public const string NameHeadlessHorseman = "ara_HeadlessHorseman";

	private void AddAra_HeadlessHorseman()
	{
		List<Ability> abilities = new List<Ability>
		{
			Ability.Flying,
			Ability.Strafe
		};

		NewCard.Add(CardBuilder.Builder
			.SetAsRareCard()
			.SetAbilities(abilities)
			.SetBaseAttackAndHealth(4, 3)
			.SetBoneCost(9)
			.SetDescription("The apocalypse is soon.")
			.SetNames(NameHeadlessHorseman, "Headless Horseman")
			.Build()
		);
	}
}
