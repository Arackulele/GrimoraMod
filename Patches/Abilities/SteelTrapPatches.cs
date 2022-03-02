using DiskCardGame;
using HarmonyLib;

namespace GrimoraMod;

[HarmonyPatch(typeof(SteelTrap))]
public class SteelTrapPatches
{
	
	[HarmonyPrefix, HarmonyPatch(nameof(SteelTrap.CardToDraw))]
	public static bool ChangeCardToDrawToVellum(ref CardInfo __result)
	{
		if (GrimoraSaveUtil.isNotGrimora)
		{
			return true;
		}

		__result = GrimoraPlugin.NameVellum.GetCardInfo();
		return false;
	}
}
