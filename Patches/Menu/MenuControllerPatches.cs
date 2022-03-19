using System.Collections;
using DiskCardGame;
using HarmonyLib;
using UnityEngine;
using UnityEngine.SceneManagement;
using static GrimoraMod.GrimoraPlugin;

namespace GrimoraMod;

[HarmonyPatch(typeof(MenuController))]
public class MenuControllerPatches
{
	[HarmonyPrefix, HarmonyPatch(nameof(MenuController.LoadGameFromMenu))]
	public static bool ContinueActOne(bool newGameGBC)
	{
		SaveManager.LoadFromFile();
		// this should fix the issue of having your current scene already at the Grimora finale 
		string sceneToLoad = "Part1_Cabin";
		if (!SaveManager.SaveFile.currentScene.ToLowerInvariant().Contains("grimora"))
		{
			sceneToLoad = SaveManager.SaveFile.currentScene;
		}

		LoadingScreenManager.LoadScene(
			newGameGBC
				? "GBC_Intro"
				: sceneToLoad
		);
		SaveManager.savingDisabled = false;

		return false;
	}

	[HarmonyPrefix, HarmonyPatch(nameof(MenuController.OnCardReachedSlot))]
	public static bool OnCardReachedSlotPatch(MenuController __instance, MenuCard card, bool skipTween = false)
	{
		if (GrimoraSaveUtil.isGrimora && card.MenuAction == MenuAction.ReturnToStartMenu)
		{
			Log.LogDebug($"[MenuController.OnCardReachedSlot] Saving before exiting");
			SaveManager.SaveToFile();
		}
		else if (card.titleText == "Start Grimora Mod")
		{
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

	[HarmonyPostfix, HarmonyPatch(nameof(MenuController.Start))]
	public static void AddGrimoraCard(MenuController __instance)
	{
		if (SceneManager.GetActiveScene().name.Equals("Start"))
		{
			if (__instance.cardRow.Find("MenuCard_Grimora").IsNull())
			{
				Log.LogDebug($"Non-hot reload menu button creation");
				__instance.cards.Add(CreateButton(__instance));
			}
		}
	}

	public static MenuCard CreateButton(MenuController controller)
	{
		Log.LogDebug("Creating MenuCard button");

		var cardRow = controller.transform.Find("CardRow").GetComponent<StartMenuAscensionCardInitializer>();
		float xPosition = cardRow.menuCards.Count == 5
			? 1.373f
			: 1.603f;

		MenuCard menuCardGrimora = Object.Instantiate(
			ResourceBank.Get<MenuCard>("Prefabs/StartScreen/StartScreenMenuCard"),
			new Vector3(xPosition, -0.77f, 0),
			Quaternion.identity,
			cardRow.transform
		);
		menuCardGrimora.name = "MenuCard_Grimora";

		menuCardGrimora.GetComponent<SpriteRenderer>().sprite = AssetConstants.MenuCardGrimora;
		menuCardGrimora.menuAction = MenuAction.Continue;
		menuCardGrimora.titleText = "Start Grimora Mod";
		menuCardGrimora.titleSprite = AssetConstants.TitleSprite;

		return menuCardGrimora;
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
