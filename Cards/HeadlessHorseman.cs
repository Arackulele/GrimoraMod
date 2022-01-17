using System.Collections.Generic;
using APIPlugin;
using DiskCardGame;
using GrimoraMod.Properties;

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
			.AsRareCard()
			.WithAbilities(abilities)
			.WithBaseAttackAndHealth(4, 3)
			.WithBonesCost(9)
			.WithDescription("The apocalypse is soon.")
			.WithNames(NameHeadlessHorseman, "Headless Horseman")
			.WithPortrait(Resources.HeadlessHorseman)
			.Build()
		);
	}
}