using System.Collections;
using DiskCardGame;
using HarmonyLib;
using UnityEngine;

namespace GrimoraMod
{
	[HarmonyPatch]
	public class TestingPatches
	{
		[HarmonyPatch(typeof(GameMap))]
		public class BaseGameMapPatches
		{
			[HarmonyPostfix, HarmonyPatch(nameof(GameMap.ShowMapSequence))]
			public static IEnumerator AddLogging(IEnumerator enumerator, GameMap __instance, float unrollSpeed = 1f)
			{
				if (SaveManager.SaveFile.IsGrimora)
				{
					__instance.FullyUnrolled = false;
					yield return __instance.UnrollingSequence(unrollSpeed);

					// todo: have to set this again because for some reason it doesn't take it during transitions?
					PlayerMarker.Instance.transform.position = MapNodeManager.Instance.ActiveNode.transform.position;


					PlayerMarker.Instance.Show();

					__instance.FullyUnrolled = true;
					yield break;
				}
				else
				{
					yield return enumerator;
					yield break;
				}
			}
		}

		[HarmonyPatch(typeof(ViewController))]
		public class ViewControllerPatches
		{
			[HarmonyPostfix, HarmonyPatch(nameof(ViewController.SwitchToControlMode))]
			public static void Postfix(ref ViewController.ControlMode mode, bool immediate = false)
			{
				if (!SaveManager.SaveFile.IsGrimora)
				{
					return;
				}
				
				if (mode == ViewController.ControlMode.Map 
				    && !ViewManager.Instance.controller.allowedViews.Contains(View.MapDeckReview))
				{
					GrimoraPlugin.Log.LogDebug($"-> Adding MapDeckReview to allowed views");
					ViewManager.Instance.controller.allowedViews.Add(View.MapDeckReview);
				}
			}
		}

		[HarmonyPatch(typeof(GameFlowManager))]
		public class GameFlowManagerPatches
		{

		}

		[HarmonyPatch(typeof(ViewManager))]
		public class ViewManagerPatches
		{
			[HarmonyPostfix, HarmonyPatch(nameof(ViewManager.SwitchToView))]
			public static void Postfix(View view, bool immediate = false, bool lockAfter = false)
			{
				GrimoraPlugin.Log.LogDebug($"[ViewManager.SwitchToView] Called with view [{view}]");
			}
		}
	}
}