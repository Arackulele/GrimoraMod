using DiskCardGame;
using HarmonyLib;

namespace GrimoraMod;

[HarmonyPatch(typeof(DrawRabbits))]
public class DrawRabbitsPatches
{

	[HarmonyPrefix, HarmonyPatch(nameof(DrawRabbits.CardToDraw), MethodType.Getter)]
	public static bool ChangeDefaultRabbitToSpectrabbit(DrawRabbits __instance, ref CardInfo __result)
	{
		if (GrimoraSaveUtil.isNotGrimora)
		{
			return true;
		}

		CardInfo cardByName = GrimoraPlugin.NameSpectrabbit.GetCardInfo();
		cardByName.Mods.AddRange(__instance.GetNonDefaultModsFromSelf(__instance.Ability));
		__result = cardByName;
		return false;
	}
}
