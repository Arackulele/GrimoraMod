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
		 .SetDescription("The true captain of the crew, respected by all yet still elusive.")
		 .SetNames(NameDavyJones, "Davy Jones")
			.SetSpecialStatIcon(GainAttackPirates.FullStatIcon.Id)
		 .Build();
	}
}
