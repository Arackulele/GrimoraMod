using System.Collections.Generic;
using APIPlugin;
using DiskCardGame;
using GrimoraMod.Properties;

namespace GrimoraMod;

public partial class GrimoraPlugin
{
	public const string NameDrownedSoul = "ara_DrownedSoul";

	private void AddAra_DrownedSoul()
	{
		List<Ability> abilities = new List<Ability>
		{
			Ability.Deathtouch,
			Ability.Submerge
		};

		NewCard.Add(CardBuilder.Builder
			.AsNormalCard()
			.WithAbilities(abilities)
			.WithBaseAttackAndHealth(1, 1)
			.WithBonesCost(4)
			.WithDescription("Going into that well wasn't the best idea...")
			.WithNames(NameDrownedSoul, "Drowned Soul")
			.WithPortrait(Resources.DrownedSoul)
			.Build()
		);
	}
}