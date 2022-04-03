using DiskCardGame;
using UnityEngine;

namespace GrimoraMod;

public partial class GrimoraPlugin
{
	public const string NameGravedigger = $"{GUID}_Gravedigger";
	public const string NameSporedigger = $"{GUID}_Sporedigger";

	private void Add_Card_Gravedigger()
	{
		Sprite ogSprite = "Gravedigger".GetCardInfo().portraitTex;
		CardBuilder.Builder
			.SetAsNormalCard()
			.SetAbilities(Ability.BoneDigger)
			.SetBaseAttackAndHealth(0, 3)
			.SetBoneCost(1)
			.SetDescription(
				"HE SPENDS HIS TIME ALONE DIGGING FOR BONES IN HOPES OF FINDING A TREASURE. JUST LIKE HIS GRANDPA."
			)
			.SetNames(NameGravedigger, "Gravedigger", ogSprite)
			.Build();
	}

	private void Add_Card_Sporedigger()
	{
		CardBuilder.Builder
			.SetAsRareCard()
			.SetAbilities(Ability.BoneDigger, Ability.BoneDigger)
			.SetBaseAttackAndHealth(0, 3)
			.SetBoneCost(1)
			.SetDescription("A POOR, BRUTALIZED SOUL. ITS MYCELIA PROBES FAR INTO THE SOIL, GUIDING ITS SPADE.")
			.SetNames(NameSporedigger, "Sporedigger")
			.SetTraits(Trait.Fused)
			.Build();
	}
}
