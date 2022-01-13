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
				if (mode == ViewController.ControlMode.Map && SaveManager.SaveFile.IsGrimora)
				{
					GrimoraPlugin.Log.LogDebug($"-> Adding MapDeckReview to allowed views");
					ViewManager.Instance.controller.allowedViews.Add(View.MapDeckReview);
				}
			}
		}

		[HarmonyPatch(typeof(GameFlowManager))]
		public class GameFlowManagerPatches
		{
			[HarmonyPostfix, HarmonyPatch(nameof(GameFlowManager.TransitionTo))]
			public static IEnumerator Postfix(
				IEnumerator enumerator,
				GameFlowManager __instance,
				GameState gameState,
				NodeData triggeringNodeData = null,
				bool immediate = false,
				bool unlockViewAfterTransition = true
			)
			{
				// GrimoraPlugin.Log.LogDebug($"[GameFlowManager.TransitionTo] GameState is [{gameState}]");

				if (SaveManager.SaveFile.IsGrimora && gameState == GameState.Map)
				{
					// GrimoraPlugin.Log.LogDebug($"[GameFlowManager.TransitionTo] SaveFile is Grimora and GameState is GameState.Map");
					ViewManager.Instance.Controller.SwitchToControlMode(ViewController.ControlMode.Map);

					ViewManager.Instance.Controller.LockState = ViewLockState.Locked;

					// GrimoraPlugin.Log.LogDebug($"[GameFlowManager.TransitionTo] SceneSpecificTransitionTo");
					__instance.SceneSpecificTransitionTo(GameState.Map, immediate);

					// GrimoraPlugin.Log.LogDebug($"[GameFlowManager.TransitionTo] SaveToFile");
					SaveManager.SaveToFile();

					yield return new WaitForSeconds(0.2f);
					// GrimoraPlugin.Log.LogDebug($"[GameFlowManager.TransitionTo] " +
					//                            $"map is null? [{RunState.Run.map == null}])");

					bool isBossDefeated = ChessboardMapExt.Instance.BossDefeated;
					bool piecesExist = ChessboardMapExt.Instance.pieces.Count > 0;

					GrimoraPlugin.Log.LogDebug($"[GameFlowManager.TransitionTo] IsBossDefeated? [{isBossDefeated}] Pieces exist? [{piecesExist}]");
					if (piecesExist && isBossDefeated)
					{
						// GrimoraPlugin.Log.LogDebug($"[GameFlowManager.TransitionTo] ChessboardMapExt is not null");
						ChessboardMapExt.Instance.BossDefeated = false;
						GrimoraPlugin.Log.LogDebug($"[GameFlowManager.TransitionTo] Calling CompleteRegionSequence");
						yield return ChessboardMapExt.Instance.CompleteRegionSequence();
					}

					// GrimoraPlugin.Log.LogDebug($"[GameFlowManager.TransitionTo] unlockViewAfterTransition");
					if (unlockViewAfterTransition)
					{
						ViewManager.Instance.Controller.LockState = ViewLockState.Unlocked;
					}
				}

				yield return enumerator;
				yield break;
			}
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