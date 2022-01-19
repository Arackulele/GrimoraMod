using DiskCardGame;
using HarmonyLib;

namespace GrimoraMod;

[HarmonyPatch(typeof(AnimatedGameMapMarker))]
public class PlayerMarkerPatches
{
	[HarmonyPostfix, HarmonyPatch(nameof(AnimatedGameMapMarker.Show))]
	public static void PostfixAddExtraLogicAfterUnrolling(AnimatedGameMapMarker __instance)
	{
		if (SaveManager.SaveFile.IsGrimora && __instance is PlayerMarker)
		{
			// Have you call the setter for this twice otherwise the PlayerMarker might appear at the previous active node visually,
			//	but is actually at the new node.
			// e.g. Shows the marker at 0,1 , but is actually at 6,7
			GrimoraPlugin.Log.LogDebug($"[AnimatedGameMapMarker.Show] Setting PlayerMarker");
			PlayerMarker.Instance.transform.position = MapNodeManager.Instance.ActiveNode.transform.position;
		}
	}
}