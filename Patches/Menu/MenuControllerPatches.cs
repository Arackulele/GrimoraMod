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
	private const string ErrorMessageFromOldMod =
		"Due to changing the name prefix from `ara_` to `GrimoraMod_`, your deck needs to be reset. Otherwise exceptions will be thrown.";

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

		LoadingScreenManager.LoadScene(newGameGBC ? "GBC_Intro" : sceneToLoad);
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
			// since the card names are now prefixed with GrimoraMod_, any cards that have ara_ will throw an exception
			try
			{
				if (GrimoraSaveUtil.DeckListCopy.Exists(info => info.name.StartsWith("ara_")))
				{
					Log.LogWarning($"Card with ara_ exists in DeckList");
					Log.LogWarning(ErrorMessageFromOldMod);
					ConfigHelper.ResetDeck();
				}
			}
			catch (Exception e)
			{
				Log.LogWarning($"Exception thrown while attempting to reset deck with a card prefixed with 'ara_', resetting deck");
				ConfigHelper.ResetDeck();
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

	[HarmonyPostfix, HarmonyPatch(nameof(MenuController.Start))]
	public static void AddGrimoraCard(ref MenuController __instance)
	{
		if (SceneManager.GetActiveScene().name.ToLowerInvariant().Contains("start") 
		    && !__instance.cards.Exists(card => card.name.ToLowerInvariant().Contains("grimora")))
		{
			__instance.cards.Add(CreateButton(__instance));
		}
	}

	public static MenuCard CreateButton(MenuController controller)
	{
		GameObject cardRow = controller.transform.Find("CardRow").gameObject;

		// GrimoraPlugin.Log.LogDebug("Finding MenuCard_Continue gameObject");
		MenuCard menuCardGrimora = Object.Instantiate(
			ResourceBank.Get<MenuCard>("Prefabs/StartScreen/StartScreenMenuCard"),
			new Vector3(1.378f, -0.77f, 0),
			Quaternion.identity,
			cardRow.transform
		);
		menuCardGrimora.name = "MenuCard_Grimora";

		menuCardGrimora.GetComponent<SpriteRenderer>().sprite = AssetConstants.MenuCardGrimora;
		menuCardGrimora.menuAction = MenuAction.Continue;
		menuCardGrimora.titleText = "Start Grimora Mod";
		menuCardGrimora.titleSprite = AssetConstants.TitleSprite;

		Vector3 cardRowLocalPosition = cardRow.transform.localPosition;
		cardRow.transform.localPosition = new Vector3(-0.23f, cardRowLocalPosition.y, cardRowLocalPosition.z);

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
