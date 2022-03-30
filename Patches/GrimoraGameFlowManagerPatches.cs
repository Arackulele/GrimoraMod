using DiskCardGame;
using HarmonyLib;
using Pixelplacement;
using UnityEngine;
using static GrimoraMod.GrimoraPlugin;

namespace GrimoraMod;

[HarmonyPatch(typeof(GrimoraGameFlowManager))]
public class GrimoraGameFlowManagerPatches
{
	[HarmonyPrefix, HarmonyPatch(nameof(GrimoraGameFlowManager.CanTransitionToFirstPerson))]
	public static bool CanTransitionToFirstPerson(GrimoraGameFlowManager __instance, ref bool __result)
	{
		if (!Input.GetKeyDown(KeyCode.DownArrow)
		 && __instance.CurrentGameState == GameState.Map
		 && !__instance.Transitioning
		 && ProgressionData.LearnedMechanic(MechanicsConcept.FirstPersonNavigation)
		 && GameMap.Instance)
		{
			__result = GameMap.Instance.FullyUnrolled;
		}
		else
		{
			__result = false;
		}

		return false;
	}

	[HarmonyPrefix, HarmonyPatch(nameof(GrimoraGameFlowManager.SceneSpecificInitialization))]
	public static bool PrefixAddMultipleSequencersDuringLoad(ref GrimoraGameFlowManager __instance)
	{
		// bool skipIntro = GrimoraPlugin.ConfigHasPlayedRevealSequence.Value;
		bool setLightsActive = true;

		if (FinaleDeletionWindowManager.instance)
		{
			UnityObject.Destroy(FinaleDeletionWindowManager.instance.gameObject);
		}

		ViewManager.Instance.SwitchToView(View.Default, true);

		if (StoryEventsData.EventCompleted(StoryEvent.GrimoraReachedTable))
		{
			Log.LogDebug($"[SceneSpecificInitialization] GrimoraReachedTable is true.");
			AudioController.Instance.SetLoopAndPlay("finalegrimora_ambience");
			if (GameMap.Instance)
			{
				// this is so that it looks a little cleaner when entering for the first time
				ChessboardMap.Instance.gameObject.transform.GetChild(0).gameObject.SetActive(false);
				__instance.CurrentGameState = GameState.Map;
				__instance.StartCoroutine(__instance.TransitionTo(GameState.Map, null, true));
				PlayTombstonesFalling();
			}
		}
		else
		{
			Log.LogDebug($"[SceneSpecificInitialization] GrimoraReachedTable is false");

			if (GameMap.Instance)
			{
				GameMap.Instance.HideMapImmediate();
			}

			__instance.CurrentGameState = GameState.FirstPerson3D;

			__instance.StartCoroutine(__instance.TransitionTo(GameState.FirstPerson3D, null, true));

			SetLightsActive(__instance);

			__instance.StartCoroutine(__instance.StartSceneSequence());

			PlayTombstonesFalling();

			__instance.StartCoroutine(((GrimoraGameFlowManager)GameFlowManager.Instance).RevealGrimoraSequence());

			SaveManager.SaveToFile();
		}

		return false;
	}

	private static void PlayTombstonesFalling()
	{
		CryptEpitaphSlotInteractable cryptEpitaphSlotInteractable =
			UnityObject.FindObjectOfType<CryptEpitaphSlotInteractable>();

		if (cryptEpitaphSlotInteractable && cryptEpitaphSlotInteractable.isActiveAndEnabled)
		{
			AudioController.Instance.PlaySound3D(
				"giant_stones_falling",
				MixerGroup.ExplorationSFX,
				GameFlowManager.Instance.transform.position
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
		}
	}

	private static void SetLightsActive(GrimoraGameFlowManager __instance)
	{
		CryptManager.Instance.HangingLight.gameObject.SetActive(true);
		CryptManager.Instance.HandLight.gameObject.SetActive(true);

		__instance.gameTableCandlesParent.SetActive(true);

		Transform tableTransform = __instance.gameTableCandlesParent.transform;
		int childCountTable = tableTransform.childCount;
		for (int i = 0; i < childCountTable; i++)
		{
			var candle = tableTransform.GetChild(i).gameObject;
			candle.SetActive(true);
			candle.GetComponentInChildren<Animator>().Play("candle_light");
		}

		Transform cryptLightsTransform = CryptManager.Instance.transform.Find("Lights");
		int cryptLightsCount = cryptLightsTransform.childCount;
		for (int i = 0; i < cryptLightsCount; i++)
		{
			cryptLightsTransform.GetChild(i).gameObject.SetActive(true);
		}

		__instance.gravestoneNavZone.SetActive(true);
	}
}
