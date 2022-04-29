using DiskCardGame;
using HarmonyLib;

namespace GrimoraMod;

[HarmonyPatch(typeof(CreateBells))]
public class CreateBellsPatch
{
	[HarmonyPrefix, HarmonyPatch(nameof(CreateBells.SpawnedCardId), MethodType.Getter)]
	public static bool ChangeBellsToCustomBells(ref string __result)
	{
		if (GrimoraSaveUtil.IsNotGrimora)
		{
			return true;
		}

		__result = GrimoraPlugin.NameDeathKnellBell;
		return false;
	}
}
