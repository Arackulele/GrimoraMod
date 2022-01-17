using System.Collections.Generic;
using APIPlugin;
using DiskCardGame;
using GrimoraMod.Properties;

namespace GrimoraMod;

public partial class GrimoraPlugin
{
	public const string NameGraveDigger = "ara_Gravedigger";
	public const string NameSporeDigger = "ara_Sporedigger";

	private void AddAra_GraveDigger()
	{
		NewCard.Add(CardBuilder.Builder
			.AsNormalCard()
			.WithAbilities(Ability.BoneDigger)
			.WithBaseAttackAndHealth(0, 3)
			.WithBonesCost(1)
			.WithDescription(
				"He spends his time alone digging for bones in hopes of finding a treasure. Just like his grandpa.")
			.WithNames(NameGraveDigger, "Gravedigger")
			.WithPortrait(Resources.GraveDigger)
			.Build()
		);
	}

	private void AddAra_SporeDigger()
	{
		List<Ability> abilities = new List<Ability>
		{
			Ability.BoneDigger,
			Ability.BoneDigger
		};

		NewCard.Add(CardBuilder.Builder
			.AsRareCard()
			.WithAbilities(abilities)
			.WithBaseAttackAndHealth(0, 3)
			.WithBonesCost(1)
			.WithDescription("The Sporedigger, an excellent digger.")
			.WithNames(NameSporeDigger, "Sporedigger")
			.WithPortrait(Resources.SporeDigger)
			.WithTraits(Trait.Fused)
			.Build()
		);
	}
}