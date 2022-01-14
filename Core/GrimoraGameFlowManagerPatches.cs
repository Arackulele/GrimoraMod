using System.Collections.Generic;
using DiskCardGame;
using HarmonyLib;
using UnityEngine;

namespace GrimoraMod
{
	[HarmonyPatch(typeof(GrimoraGameFlowManager))]
	public class GrimoraGameFlowManagerPatches
	{


		[HarmonyPrefix, HarmonyPatch(nameof(GrimoraGameFlowManager.SceneSpecificInitialization))]
		public static bool PrefixAddMultipleSequencersDuringLoad(GrimoraGameFlowManager __instance)
		{
			if (SaveManager.SaveFile.IsGrimora)
			{
				ChangeChessboardToExtendedClass();

				AddRareCardSequencerToScene();

				AddDeckReviewSequencerToScene();

				bool skipIntro = false;
				bool skipTombstone = false;

				if (FinaleDeletionWindowManager.instance != null)
				{
					// GrimoraPlugin.Log.LogDebug($"[SceneSpecificInitialization] Destroying FinaleDeletionWindowManager as it exists");
					Object.Destroy(FinaleDeletionWindowManager.instance.gameObject);
				}

				ViewManager.Instance.SwitchToView(View.Default, immediate: true);

				if (!StoryEventsData.EventCompleted(StoryEvent.GrimoraReachedTable))
				{
					// GrimoraPlugin.Log.LogDebug($"[SceneSpecificInitialization] GrimoraReachedTable is false");

					if (GameMap.Instance != null)
					{
						// GrimoraPlugin.Log.LogDebug($"[SceneSpecificInitialization] Calling GameMap.HideMapImmediate as it exists");
						GameMap.Instance.HideMapImmediate();
					}

					// GrimoraPlugin.Log.LogDebug($"[SceneSpecificInitialization] Setting __instance.CurrentGameState to GameState.FirstPerson3D");
					__instance.CurrentGameState = GameState.FirstPerson3D;

					// GrimoraPlugin.Log.LogDebug($"[SceneSpecificInitialization] Transitioning to FirstPerson3D");
					__instance.StartCoroutine(__instance.TransitionTo(GameState.FirstPerson3D, null, immediate: true));

					ExplorableAreaManager.Instance.HangingLight.gameObject.SetActive(skipTombstone);
					ExplorableAreaManager.Instance.HandLight.gameObject.SetActive(skipTombstone);

					__instance.gameTableCandlesParent.SetActive(skipTombstone);
					__instance.gravestoneNavZone.SetActive(skipTombstone);

					if (!skipIntro)
					{
						// GrimoraPlugin.Log.LogDebug($"[SceneSpecificInitialization] Intro is not being skipped");
						__instance.StartCoroutine(__instance.StartSceneSequence());
					}
				}
				else
				{
					// GrimoraPlugin.Log.LogDebug($"[SceneSpecificInitialization] GrimoraReachedTable is true, playing finalegrimora_ambience");
					AudioController.Instance.SetLoopAndPlay("finalegrimora_ambience");
					if (GameMap.Instance != null)
					{
						// GrimoraPlugin.Log.LogDebug($"[SceneSpecificInitialization] Setting CurrentGameState to GameState.Map");
						__instance.CurrentGameState = GameState.Map;
						// GrimoraPlugin.Log.LogDebug($"[SceneSpecificInitialization] Transitioning to GameState.Map");
						__instance.StartCoroutine(__instance.TransitionTo(GameState.Map, null, immediate: true));
					}
				}

				return false;
		}




	}
}