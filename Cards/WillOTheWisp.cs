using DiskCardGame;
using UnityEngine;

namespace GrimoraMod;

public partial class GrimoraPlugin
{
	public const string NameWillOTheWisp = $"{GUID}_WillOTheWisp";

	private void Add_Card_WillOTheWisp()
	{
		CardBuilder.Builder
		 .SetAsNormalCard()
		 .SetAbilities(SpiritBearer.ability)
		 .SetBaseAttackAndHealth(0, 1)
		 .SetBoneCost(1)
		 .SetDescription("Joyous spirits, THEY WARM WITH THEIR BODIES OF LIGHT, DRAWING IN HOPELESS SOULS INTO THEIR BEACON.")
		 .SetNames(NameWillOTheWisp, "Will 'O' The Wisp")
		 .Build()
		 .pixelPortrait = GrimoraPlugin.AllSprites.Find(o => o.name == "pixel_wisp");
			
			;
	}
}
