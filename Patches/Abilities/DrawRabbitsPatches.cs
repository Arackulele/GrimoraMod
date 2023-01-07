using DiskCardGame;
using HarmonyLib;

namespace GrimoraMod;

[HarmonyPatch(typeof(DrawRabbits))]
public class DrawRabbitsPatches
{
	[HarmonyPrefix, HarmonyPatch(nameof(DrawRabbits.CardToDraw), MethodType.Getter)]
	public static bool ChangeDefaultRabbitToSpectrabbit(DrawRabbits __instance, ref CardInfo __result)
	{
		if (GrimoraSaveUtil.IsNotGrimoraModRun)
		{
			return true;
		}

		CardInfo cardByName = (CardInfo)GrimoraPlugin.NameSpectrabbit.GetCardInfo().Clone();
		cardByName.Mods.AddRange(__instance.GetNonDefaultModsFromSelf(__instance.Ability));
		__result = cardByName;
		return false;
	}
}
