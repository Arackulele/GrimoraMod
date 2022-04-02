using DiskCardGame;
using HarmonyLib;

namespace GrimoraMod;

[HarmonyPatch(typeof(DrawRandomCardOnDeath))]
public class DrawRandomCardOnDeathPatches
{

	[HarmonyPrefix, HarmonyPatch(nameof(DrawRandomCardOnDeath.CardToDraw), MethodType.Getter)]
	public static bool ChangeCardToDraw(ref CardInfo __result)
	{
		if (GrimoraSaveUtil.isNotGrimora)
		{
			return true;
		}

		__result = GrimoraPlugin.AllPlayableGrimoraModCards.GetRandomItem();
		return false;
	}
}
