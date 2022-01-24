using APIPlugin;
using DiskCardGame;
using UnityEngine;

namespace GrimoraMod;

public partial class GrimoraPlugin
{
	public const string NameGraveDigger = "ara_Gravedigger";
	public const string NameSporeDigger = "ara_Sporedigger";

	private void AddAra_GraveDigger()
	{
		Sprite ogSprite = CardLoader.GetCardByName("Gravedigger").portraitTex;
		NewCard.Add(CardBuilder.Builder
			.SetAsNormalCard()
			.SetAbilities(Ability.BoneDigger)
			.SetBaseAttackAndHealth(0, 3)
			.SetBoneCost(1)
			.SetDescription(
				"He spends his time alone digging for bones in hopes of finding a treasure. Just like his grandpa.")
			.SetNames(NameGraveDigger, "Gravedigger", ogSprite)
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
			.SetAsRareCard()
			.SetAbilities(abilities)
			.SetBaseAttackAndHealth(0, 3)
			.SetBoneCost(1)
			.SetDescription("An excellent digger.")
			.SetNames(NameSporeDigger, "Sporedigger")
			.SetTraits(Trait.Fused)
			.Build()
		);
	}
}
