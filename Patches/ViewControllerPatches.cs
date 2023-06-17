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
		if (GrimoraSaveUtil.IsNotGrimoraModRun || mode != ViewController.ControlMode.Map)
		{
			return true;
		}

		__instance.controlMode = mode;
		View currentView = ViewManager.Instance.CurrentView;

		__instance.allowedViews = new List<View> { View.MapDefault, View.MapDeckReview };
		__instance.altTransitionInputs = new List<ViewController.ViewTransitionInput>
		{
			new(View.MapDefault, View.Consumables, Button.LookRight),
			new(View.Consumables, View.MapDefault, Button.LookLeft),
			new(View.Consumables, View.MapDefault, Button.LookDown),
			new(View.Consumables, View.MapDefault, Button.LookUp),
		};

		if (!__instance.allowedViews.Contains(currentView))
		{
			ViewManager.Instance.SwitchToView(View.MapDefault, immediate);
		}

		return false;
	}
}
