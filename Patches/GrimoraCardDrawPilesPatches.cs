using DiskCardGame;
using HarmonyLib;

namespace GrimoraMod;

[HarmonyPatch(typeof(GrimoraCardDrawPiles))]
public class GrimoraCardDrawPilesPatches
{
	[HarmonyPostfix, HarmonyPatch(nameof(GrimoraCardDrawPiles.SideDeckData), MethodType.Getter)]
	public static void Postfix(ref List<CardInfo> __result)
	{
		for (int i = 0; i < 3; i++)
		{
			GrimoraPlugin.Log.LogDebug($"[DrawPile] Adding 1 more skeleton");
			__result.Add("Skeleton".GetCardInfo());
		}
	}
}
