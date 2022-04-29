using DiskCardGame;
using HarmonyLib;

namespace GrimoraMod;

[HarmonyPatch(typeof(SteelTrap))]
public class SteelTrapPatches
{
	
	[HarmonyPrefix, HarmonyPatch(nameof(SteelTrap.CardToDraw), MethodType.Getter)]
	public static bool ChangeCardToDrawToVellum(ref CardInfo __result)
	{
		if (GrimoraSaveUtil.IsNotGrimora)
		{
			return true;
		}

		__result = GrimoraPlugin.NameVellum.GetCardInfo();
		return false;
	}
}
