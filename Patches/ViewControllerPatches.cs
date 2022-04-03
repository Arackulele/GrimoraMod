using DiskCardGame;
using HarmonyLib;

namespace GrimoraMod;

[HarmonyPatch(typeof(ViewController))]
public class ViewControllerPatches
{
	[HarmonyPrefix, HarmonyPatch(nameof(ViewController.SwitchToControlMode))]
	public static bool OverrideAllowedViews(
		ref ViewController __instance,
		ViewController.ControlMode mode,
		bool immediate = false
	)
	{
		if (mode == ViewController.ControlMode.Map)
		{
			__instance.controlMode = mode;
			View currentView = ViewManager.Instance.CurrentView;
			__instance.altTransitionInputs.Clear();

			__instance.allowedViews = new List<View> { View.MapDefault, View.MapDeckReview };
			if (!__instance.allowedViews.Contains(currentView))
			{
				ViewManager.Instance.SwitchToView(View.MapDefault, immediate);
			}
			return false;
		}

		return true;
	}
}
