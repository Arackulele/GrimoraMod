using APIPlugin;
using DiskCardGame;
using UnityEngine;

namespace GrimoraMod;

public partial class GrimoraPlugin
{
	public const string NameDeathKnellBell = "GrimoraMod_DeathKnell_Bell";

	private void Add_DeathKnellBell()
	{
		new CustomCard("DausBell")
		{
			tex = AssetUtils.GetPrefab<Sprite>("DeathKnell_Bell").texture
		};
	}
}
