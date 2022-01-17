using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DiskCardGame;
using HarmonyLib;
using UnityEngine;

namespace GrimoraMod;

[HarmonyPatch(typeof(MenuController))]
public class MenuControllerPatches
{
	private static readonly List<StoryEvent> TutorialsToFinish = Enum.GetValues(typeof(StoryEvent))
		.Cast<StoryEvent>()
		.Where(ev => ev.ToString().Contains("Tutorial"))
		.ToList();

	[HarmonyPrefix, HarmonyPatch(nameof(MenuController.LoadGameFromMenu))]
	public static bool ContinueActOne(bool newGameGBC)
	{
		SaveManager.LoadFromFile();
		LoadingScreenManager.LoadScene(newGameGBC ? "GBC_Intro" : "Part1_Cabin");
		SaveManager.savingDisabled = false;

		return false;
	}

	[HarmonyPrefix, HarmonyPatch(nameof(MenuController.OnCardReachedSlot))]
	public static bool MainMenuThree(MenuController __instance, MenuCard card, bool skipTween = false)
	{
		if (SaveManager.SaveFile.IsGrimora && card.MenuAction == MenuAction.ReturnToStartMenu)
		{
			GrimoraPlugin.Log.LogDebug($"[MenuController.OnCardReachedSlot] saving before exiting");
			SaveManager.SaveToFile();
		}

		if (card.titleText == "Start Grimora Mod")
		{
			// GrimoraPlugin.Log.LogDebug($"[MenuController.OnCardReachedSlot] Card.titleText is 'Start Grimora Mod'");
			__instance.DoingCardTransition = false;
			card.transform.parent = __instance.menuSlot.transform;
			card.SetBorderColor(__instance.slottedBorderColor);
			AudioController.Instance.PlaySound2D("crunch_short#1", MixerGroup.None, 0.6f);

			__instance.Shake(0.015f, 0.3f);
			__instance.StartCoroutine(__instance.TransitionToGame2());
			return false;
		}

		return true;
	}
}

public static class TransitionExt
{
	public static IEnumerator TransitionToGame2(this MenuController controller)
	{
		controller.TransitioningToScene = true;
		yield return new WaitForSecondsRealtime(0.75f);
		AudioController.Instance.FadeOutLoop(0.75f);
		GBC.CameraEffects.Instance.FadeOut();
		yield return new WaitForSecondsRealtime(0.75f);
		MenuController.LoadGameFromMenu(false);

		LoadingScreenManager.LoadScene("finale_grimora");
	}
}