using DiskCardGame;
using HarmonyLib;

namespace GrimoraMod;

[HarmonyPatch(typeof(DrawRabbits))]
public class DrawRabbitsPatches
{
	[HarmonyPrefix, HarmonyPatch(nameof(DrawRabbits.CardToDraw), MethodType.Getter)]
	public static bool ChangeDefaultRabbitToSpectrabbit(DrawRabbits __instance, ref CardInfo __result)
	{
		if (GrimoraSaveUtil.IsNotGrimora)
		{
			return true;
		}

		CardInfo cardByName = GrimoraPlugin.NameSpectrabbit.GetCardInfo();
		cardByName.Mods.AddRange(__instance.GetNonDefaultModsFromSelf(__instance.Ability, Ability.DrawCopy));
		__result = cardByName;
		return false;
	}
}
