using UnityEngine;

namespace GrimoraMod;

public partial class GrimoraPlugin
{
	public const string NameDavyJones = $"{GUID}_PirateDavyJones";

	private void Add_Card_PirateDavyJones()
	{
		CardBuilder.Builder
		 .SetAsRareCard()
		 .SetAbilities(Anchored.ability)
		 .SetSpecialAbilities(GainAttackPirates.FullSpecial.Id)
		 .SetBaseAttackAndHealth(0, 3)
		 .SetBoneCost(5)
		 .SetDescription("His songs raise the morale of only Skeletons, as no one else seems to like it.")
		 .SetNames(NameDavyJones, "Davy Jones")
			.SetSpecialStatIcon(GainAttackPirates.FullStatIcon.Id)
		 .Build();
	}
}
