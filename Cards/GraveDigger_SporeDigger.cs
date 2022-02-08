using APIPlugin;
using DiskCardGame;
using UnityEngine;

namespace GrimoraMod;

public partial class GrimoraPlugin
{
	public const string NameGravedigger = "GrimoraMod_Gravedigger";
	public const string NameSporedigger = "GrimoraMod_Sporedigger";

	private void Add_GraveDigger()
	{
		Sprite ogSprite = "Gravedigger".GetCardInfo().portraitTex;
		NewCard.Add(CardBuilder.Builder
			.SetAsNormalCard()
			.SetAbilities(Ability.BoneDigger)
			.SetBaseAttackAndHealth(0, 3)
			.SetBoneCost(1)
			.SetDescription(
				"He spends his time alone digging for bones in hopes of finding a treasure. Just like his grandpa.")
			.SetNames(NameGravedigger, "Gravedigger", ogSprite)
			.Build()
		);
	}

	private void Add_SporeDigger()
	{
		NewCard.Add(CardBuilder.Builder
			.SetAsRareCard()
			.SetAbilities(Ability.BoneDigger, Ability.BoneDigger)
			.SetBaseAttackAndHealth(0, 3)
			.SetBoneCost(1)
			.SetDescription("An excellent digger.")
			.SetNames(NameSporedigger, "Sporedigger")
			.SetTraits(Trait.Fused)
			.Build()
		);
	}
}
