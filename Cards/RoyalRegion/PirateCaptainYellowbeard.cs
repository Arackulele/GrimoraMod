using UnityEngine;

namespace GrimoraMod;

public partial class GrimoraPlugin
{
	public const string NamePirateCaptainYellowbeard = $"{GUID}_PirateCaptainYellowbeard";

	private void Add_Card_PirateCaptainYellowbeard()
	{
		CardBuilder.Builder
		 .SetAsRareCard()
		 .SetAbilities(BuffSkeletonsSeaShanty.ability, Anchored.ability)
		 .SetBaseAttackAndHealth(2, 2)
		 .SetBoneCost(7)
		 .SetDescription("His songs raise the morale of only Skeletons, as no one else seems to like it.")
		 .SetNames(NamePirateCaptainYellowbeard, "Yellowbeard")
		 .Build().pixelPortrait = AssetUtils.GetPrefab<Sprite>("yellowbeard_pixel");
	}
}
