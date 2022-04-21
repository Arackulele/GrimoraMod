using DiskCardGame;
using HarmonyLib;

namespace GrimoraMod;

[HarmonyPatch(typeof(GravestoneCardAnimationController))]
public class GravestoneCardAnimationControllerPatches
{
	[HarmonyPrefix, HarmonyPatch(nameof(GravestoneCardAnimationController.PlayRiffleSound))]
	public static bool DontPlayRiffleIfControllerHasBeenReplaced(GravestoneCardAnimationController __instance)
	{
		return __instance;
	}
	
	[HarmonyPrefix, HarmonyPatch(nameof(GravestoneCardAnimationController.PlayDeathAnimation))]
	public static bool PlayGlitchOutIfSelectableCard(GravestoneCardAnimationController __instance)
	{
		if (__instance.PlayableCard)
		{
			return true;
		}

		__instance.PlayGlitchOutAnimation();
		return false;
	}
}

[HarmonyPatch(typeof(CardAnimationController3D))]
public class CardAnimationController3DPatches
{
	[HarmonyPrefix, HarmonyPatch(nameof(CardAnimationController3D.Awake))]
	public static bool DontRunAwakeUntilSetupForGraveControllerExt(CardAnimationController3D __instance)
	{
		return __instance is not GraveControllerExt;
	}
}
