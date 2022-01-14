using DiskCardGame;
using HarmonyLib;
using Pixelplacement;
using UnityEngine;

namespace GrimoraMod
{
	[HarmonyPatch(typeof(GrimoraGameFlowManager))]
	public class GrimoraGameFlowManagerPatches
	{
		[HarmonyPrefix, HarmonyPatch(nameof(GrimoraGameFlowManager.SceneSpecificInitialization))]
		public static bool PrefixAddMultipleSequencersDuringLoad(GrimoraGameFlowManager __instance)
		{
			// GrimoraPlugin.Log.LogDebug($"[SceneSpecificInitialization] Instance is [{__instance.GetType()}]");

			// bool skipIntro = GrimoraPlugin.ConfigHasPlayedRevealSequence.Value;
			bool setLightsActive = true;

			if (FinaleDeletionWindowManager.instance != null)
			{
				// GrimoraPlugin.Log.LogDebug($"[SceneSpecificInitialization] Destroying FinaleDeletionWindowManager as it exists");
				Object.Destroy(FinaleDeletionWindowManager.instance.gameObject);
			}

			ViewManager.Instance.SwitchToView(View.Default, immediate: true);

			if (!StoryEventsData.EventCompleted(StoryEvent.GrimoraReachedTable))
			{
				StoryEventsData.SetEventCompleted(StoryEvent.GrimoraReachedTable, true);

				GrimoraPlugin.Log.LogDebug($"[SceneSpecificInitialization] GrimoraReachedTable is false");

				if (GameMap.Instance != null)
				{
					// GrimoraPlugin.Log.LogDebug($"[SceneSpecificInitialization] Calling GameMap.HideMapImmediate as it exists");
					GameMap.Instance.HideMapImmediate();
				}

				// GrimoraPlugin.Log.LogDebug($"[SceneSpecificInitialization] Setting __instance.CurrentGameState to GameState.FirstPerson3D");
				__instance.CurrentGameState = GameState.FirstPerson3D;

				// GrimoraPlugin.Log.LogDebug($"[SceneSpecificInitialization] Transitioning to FirstPerson3D");
				__instance.StartCoroutine(__instance.TransitionTo(GameState.FirstPerson3D, null, immediate: true));

				ExplorableAreaManager.Instance.HangingLight.gameObject.SetActive(setLightsActive);
				ExplorableAreaManager.Instance.HandLight.gameObject.SetActive(setLightsActive);

				__instance.gameTableCandlesParent.SetActive(setLightsActive);
				__instance.gravestoneNavZone.SetActive(setLightsActive);

				CryptEpitaphSlotInteractable cryptEpitaphSlotInteractable =
					Object.FindObjectOfType<CryptEpitaphSlotInteractable>();

				AudioController.Instance.PlaySound3D(
					"giant_stones_falling",
					MixerGroup.ExplorationSFX,
					__instance.transform.position
				);

				Tween.Position(
					cryptEpitaphSlotInteractable.tombstoneParent,
					cryptEpitaphSlotInteractable.tombstoneParent.position + Vector3.down * 11f,
					6f,
					0f,
					Tween.EaseIn
				);

				Tween.Shake(
					cryptEpitaphSlotInteractable.tombstoneAnim,
					cryptEpitaphSlotInteractable.tombstoneAnim.localPosition,
					new Vector3(0.05f, 0.05f, 0.05f),
					0.1f,
					0f,
					Tween.LoopType.Loop
				);
				__instance.StartCoroutine((
						(GrimoraGameFlowManager)Singleton<GameFlowManager>.Instance).RevealGrimoraSequence()
				);
				// else
				// {
				// 	// GrimoraPlugin.Log.LogDebug($"[SceneSpecificInitialization] Intro is not being skipped");
				// 	__instance.StartCoroutine(__instance.StartSceneSequence());
				// }
				SaveManager.SaveToFile();
			}
			else
			{
				GrimoraPlugin.Log.LogDebug($"[SceneSpecificInitialization] GrimoraReachedTable is true, playing finalegrimora_ambience");
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