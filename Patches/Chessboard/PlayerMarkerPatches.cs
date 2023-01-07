using DiskCardGame;
using GrimoraMod.Saving;
using HarmonyLib;

namespace GrimoraMod;

[HarmonyPatch(typeof(AnimatedGameMapMarker))]
public class PlayerMarkerPatches
{
	[HarmonyPrefix, HarmonyPatch(nameof(AnimatedGameMapMarker.Show))]
	public static bool PrefixShowHandleGrimoraBossPiece(AnimatedGameMapMarker __instance)
	{
		if (GrimoraSaveUtil.IsNotGrimoraModRun || GrimoraRunState.CurrentRun.regionTier != 3 || !__instance.name.Contains("Boss"))
		{
			return true;
		}

		__instance.anim.gameObject.SetActive(true);
		__instance.Hidden = false;
		return false;
	}

	[HarmonyPrefix, HarmonyPatch(nameof(AnimatedGameMapMarker.Hide))]
	public static bool PrefixHideHandleGrimoraBossPiece(AnimatedGameMapMarker __instance, bool immediate = false)
	{
		if (GrimoraSaveUtil.IsNotGrimoraModRun || GrimoraRunState.CurrentRun.regionTier != 3 || !__instance.name.Contains("Boss"))
		{
			return true;
		}

		CustomCoroutine.WaitThenExecute(immediate ? 0f : 0.5f, delegate { __instance.anim.gameObject.SetActive(false); });
		__instance.Hidden = true;
		return false;
	}
}
