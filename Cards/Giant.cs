using DiskCardGame;
using UnityEngine;
namespace GrimoraMod;

public partial class GrimoraPlugin
{
	public const string NameGiant = $"{GUID}_Giant";

	private void Add_Card_Giant()
	{
		CardBuilder.Builder
		 .SetAsNormalCard()
		 .SetAbilities(Ability.QuadrupleBones, Ability.SplitStrike)
		 .SetBaseAttackAndHealth(3, 8)
		 .SetBoneCost(15)
		 .SetDescription("THE FAMED RACE OF GIANTS IS SAID TO HAVE DIED OUT LONG AGO, THIS IS PROOF. TRULY A SIGHT TO BEHOLD.")
		 .SetNames(NameGiant, "Giant")
		 .SetTraits(Trait.Giant)
		 .Build().pixelPortrait = AssetUtils.GetPrefab<Sprite>("giant_pixel");
		;
	}
}
