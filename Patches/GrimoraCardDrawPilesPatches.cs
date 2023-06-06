using DiskCardGame;
using HarmonyLib;

namespace GrimoraMod;

[HarmonyPatch(typeof(GrimoraCardDrawPiles))]
public class GrimoraCardDrawPilesPatches
{
	[HarmonyPostfix, HarmonyPatch(nameof(GrimoraCardDrawPiles.SideDeckData), MethodType.Getter)]
	public static void Postfix(ref List<CardInfo> __result)
	{
		GrimoraPlugin.Log.LogWarning($"[GrimoraCardDrawPiles.SideDeckData] Adding 3 more skeletons to the side deck.");
		CardInfo skeletonInfo = "Skeleton".GetCardInfo();
		__result.Add(skeletonInfo);
		__result.Add(skeletonInfo);
		__result.Add(skeletonInfo);

		if (ConfigHelper.Instance.ConfigHardSave == 44731) { 
		foreach (CardInfo info in __result)
		{

			info.appearanceBehaviour.Add(CardAppearanceBehaviour.Appearance.HologramPortrait);

		}
		}
	}
}
