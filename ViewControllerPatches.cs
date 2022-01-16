using System.Collections.Generic;
using DiskCardGame;
using HarmonyLib;
using static GrimoraMod.GrimoraPlugin;

namespace GrimoraMod
{
	[HarmonyPatch(typeof(ViewController))]
	public class ViewControllerPatches
	{
		[HarmonyPrefix, HarmonyPatch(nameof(ViewController.SwitchToControlMode))]
		public static bool Prefix(
			ref ViewController __instance,
			ref ViewController.ControlMode mode,
			bool immediate = false)
		{
			if (!SaveManager.SaveFile.IsGrimora || mode != ViewController.ControlMode.Map)
			{
				return true;
			}

			__instance.controlMode = mode;
			View currentView = ViewManager.Instance.CurrentView;
			__instance.altTransitionInputs.Clear();

			if (mode == ViewController.ControlMode.Map)
			{
				Log.LogDebug($"[ViewController.SwitchToControlMode] Adding MapDeckReview to allowed views");
				__instance.allowedViews = new List<View> { View.MapDefault, View.MapDeckReview };

				if (!__instance.allowedViews.Contains(currentView))
				{
					ViewManager.Instance.SwitchToView(View.MapDefault, immediate);
				}
			}

			return false;
		}
	}
}