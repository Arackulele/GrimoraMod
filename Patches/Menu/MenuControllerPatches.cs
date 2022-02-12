using System.Collections;
using DiskCardGame;
using HarmonyLib;
using UnityEngine;
using static GrimoraMod.GrimoraPlugin;

namespace GrimoraMod;

[HarmonyPatch(typeof(MenuController))]
public class MenuControllerPatches
{
	[HarmonyPrefix, HarmonyPatch(nameof(MenuController.LoadGameFromMenu))]
	public static bool ContinueActOne(bool newGameGBC)
	{
		SaveManager.LoadFromFile();
		// Falsy value originally is `SaveManager.SaveFile.currentScene`,
		//	but this means that if you start GrimoraMod, currentScene is now the Grimora finale. 
		LoadingScreenManager.LoadScene(newGameGBC ? "GBC_Intro" : "Part1_Cabin");
		SaveManager.savingDisabled = false;

		return false;
	}

	[HarmonyPrefix, HarmonyPatch(nameof(MenuController.OnCardReachedSlot))]
	public static bool MainMenuThree(MenuController __instance, MenuCard card, bool skipTween = false)
	{
		if (SaveManager.SaveFile.IsGrimora && card.MenuAction == MenuAction.ReturnToStartMenu)
		{
			Log.LogDebug($"[MenuController.OnCardReachedSlot] Saving before exiting");
			SaveManager.SaveToFile();
		}
		else if (card.titleText == "Start Grimora Mod")
		{
			// since the card names are now prefixed with GrimoraMod_, any cards that have ara_ will throw an exception
			try
			{
				if (GrimoraSaveUtil.DeckListCopy.Exists(info => info.name.StartsWith("ara_")))
				{
					Log.LogWarning(
						"Due to changing the name prefix from `ara_` to `GrimoraMod_`, your run will be reset otherwise exceptions will be thrown.");
					ConfigHelper.Instance.ResetRun();
				}
			}
			catch (Exception e)
			{
				Log.LogWarning("Due to changing the name prefix from `ara_` to `GrimoraMod_`, your run will be reset otherwise exceptions will be thrown.");
				ConfigHelper.Instance.ResetRun();
			}
			
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
