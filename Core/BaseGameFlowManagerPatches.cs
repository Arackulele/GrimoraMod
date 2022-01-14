using System.Collections;
using DiskCardGame;
using HarmonyLib;
using UnityEngine;

namespace GrimoraMod
{
	[HarmonyPatch(typeof(GameFlowManager))]
	public class BaseGameFlowManagerPatches
	{
		[HarmonyPostfix, HarmonyPatch(nameof(GameFlowManager.TransitionTo))]
		public static IEnumerator PostfixGameLogicPatch(
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

				SaveManager.SaveToFile();
				
				bool isBossDefeated = ChessboardMapExt.Instance.BossDefeated;
				bool piecesExist = ChessboardMapExt.Instance.pieces.Count > 0;

				// GrimoraPlugin.Log.LogDebug($"[GameFlowManager.TransitionTo] IsBossDefeated? [{isBossDefeated}] Pieces exist? [{piecesExist}]");
				if (piecesExist && isBossDefeated)
				{
					// GrimoraPlugin.Log.LogDebug($"[GameFlowManager.TransitionTo] ChessboardMapExt is not null");
					ChessboardMapExt.Instance.BossDefeated = false;
					// GrimoraPlugin.Log.LogDebug($"[GameFlowManager.TransitionTo] Calling CompleteRegionSequence");
					yield return ChessboardMapExt.Instance.CompleteRegionSequence();
				}
				
				// GrimoraPlugin.Log.LogDebug($"[GameFlowManager.TransitionTo] ShowMapSequence");
				yield return GameMap.Instance.ShowMapSequence(__instance.SpecialSequencer
					? __instance.SpecialSequencer.MapUnrollSpeed : 1f);

				if (unlockViewAfterTransition)
				{
					ViewManager.Instance.Controller.LockState = ViewLockState.Unlocked;
				}

				// GrimoraPlugin.Log.LogDebug($"[GameFlowManager.TransitionTo] Setting CurrentGameState");
				__instance.CurrentGameState = gameState;
			}
			else
			{
				GrimoraPlugin.Log.LogDebug($"[GameFlowManager.TransitionTo] yield return the enumerator");
				yield return enumerator;
			}

		}
	}
}