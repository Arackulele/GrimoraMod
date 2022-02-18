using DiskCardGame;
using HarmonyLib;

namespace GrimoraMod;

[HarmonyPatch(typeof(AnimatedGameMapMarker))]
public class PlayerMarkerPatches
{
	[HarmonyPrefix, HarmonyPatch(nameof(AnimatedGameMapMarker.Show))]
	public static bool PrefixShowHandleGrimoraBossPiece(AnimatedGameMapMarker __instance)
	{
		if (GrimoraSaveUtil.isNotGrimora)
		{
			return true;
		}
		
		if (__instance.name.Contains("Boss") && ConfigHelper.Instance.BossesDefeated == 3)
		{
			__instance.anim.gameObject.SetActive(true);
			__instance.Hidden = false;
			return false;
		}

		return true;
	}

	[HarmonyPostfix, HarmonyPatch(nameof(AnimatedGameMapMarker.Show))]
	public static void PostfixAddExtraLogicAfterUnrolling(AnimatedGameMapMarker __instance)
	{
		if (GrimoraSaveUtil.isNotGrimora)
		{
			return;
		}

		if (__instance is PlayerMarker)
		{
			// Have you call the setter for this twice otherwise the PlayerMarker might appear at the previous active node visually,
			//	but is actually at the new node.
			// e.g. Shows the marker at 0,1 , but is actually at 6,7
			if (MapNodeManager.Instance.ActiveNode is null)
			{
				GrimoraPlugin.Log.LogDebug($"[AnimatedGameMapMarker.Show] activeNode is null, setting");
				MapNodeManager.Instance.ActiveNode
					= ChessboardNavGrid
						.instance
						.zones[GrimoraSaveData.Data.gridX, GrimoraSaveData.Data.gridY]
						.GetComponent<ChessboardMapNode>();
			}
		}
	}

	[HarmonyPrefix, HarmonyPatch(nameof(AnimatedGameMapMarker.Hide))]
	public static bool PrefixHideHandleGrimoraBossPiece(AnimatedGameMapMarker __instance, bool immediate = false)
	{
		if (GrimoraSaveUtil.isNotGrimora)
		{
			return true;
		}
		
		if (__instance.name.Contains("Boss") && ConfigHelper.Instance.BossesDefeated == 3)
		{
			CustomCoroutine.WaitThenExecute(immediate ? 0f : 0.5f, delegate { __instance.anim.gameObject.SetActive(false); });
			__instance.Hidden = true;
			return false;
		}
		
		return true;
	}
}
