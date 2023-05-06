using DiskCardGame;
using UnityEngine;

namespace GrimoraMod;

public partial class GrimoraPlugin
{
	public const string NameDavyJonesLocker = $"{GUID}_DavyJonesLocker";

	private void Add_Card_DavyJonesLocker()
	{
		CardBuilder.Builder
		 .SetAbilities(Ability.DrawRandomCardOnDeath)
		 .SetBaseAttackAndHealth(0, 3)
		 .SetBoneCost(2)
		 .SetDescription("The elusive Locker.")
		 .SetNames(NameDavyJonesLocker, "Davy Jones Locker")
		 .Build();
	}
}
