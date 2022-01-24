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
			.SetBaseAttackAndHealth(5, 5)
			.SetBoneCost(13)
			.SetDescription("The apocalypse is soon.")
			.SetNames(NameHeadlessHorseman, "Headless Horseman")
			.Build()
		);
	}
}
