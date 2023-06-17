using UnityEngine;
using UnityEngine;
namespace GrimoraMod;

public partial class GrimoraPlugin
{
	public const string NamePirateFirstMateSnag = $"{GUID}_PirateFirstMateSnag";

	private void Add_Card_PirateFirstMateSnag()
	{
		CardBuilder.Builder
		 .SetAsNormalCard()
		 .SetAbilities(HookLineAndSinker.ability, Anchored.ability)
		 .SetBaseAttackAndHealth(2, 2)
		 .SetBoneCost(7)
		 .SetDescription("He betrayed his captain for that large hook, now he is sure to make anyone a double-crosser!")
		 .SetNames(NamePirateFirstMateSnag, "First Mate Snag")
		 .Build().pixelPortrait = AssetUtils.GetPrefab<Sprite>("snag_pixel");
	}
}
