using System.Collections;
using DiskCardGame;
using GrimoraMod.Saving;
using HarmonyLib;
using InscryptionAPI.Ascension;
using Sirenix.Utilities;
using UnityEngine;
using UnityEngine.SceneManagement;
using static GrimoraMod.GrimoraPlugin;

namespace GrimoraMod;

[HarmonyPatch(typeof(MenuController))]
public class MenuControllerPatches
{
	[HarmonyPrefix, HarmonyPatch(nameof(MenuController.OnCardReachedSlot))]
	public static bool OnCardReachedSlotPatch(MenuController __instance, MenuCard card, bool skipTween = false)
	{
		ChallengeManager.SyncChallengeList();
		if (SaveManager.SaveFile.IsGrimora)
		{
			if (!SaveFile.IsAscension)
			{
				switch (card.MenuAction)
				{
					case MenuAction.ReturnToStartMenu:
					{
						Log.LogWarning($"[MenuController.OnCardReachedSlot] Saving before exiting");
						SaveManager.SaveToFile();
						break;
					}
					case MenuAction.EndRun:
					{
						
						__instance.DoingCardTransition = false;
						card.transform.parent = __instance.menuSlot.transform;
						card.SetBorderColor(__instance.slottedBorderColor);
						AudioController.Instance.PlaySound2D("crunch_short#1", MixerGroup.None, 0.6f);
						__instance.Shake(0.015f, 0.3f);
						GrimoraSaveUtil.IsGrimoraModRun = true;
						CustomCoroutine.Instance.StartCoroutine(__instance.TransitionToGame2(true));

						return false;
					}
				}
			}
		}
		else if (card.titleText == "Start Grimora Mod")
		{
			__instance.DoingCardTransition = false;
			card.transform.parent = __instance.menuSlot.transform;
			card.SetBorderColor(__instance.slottedBorderColor);
			AudioController.Instance.PlaySound2D("crunch_short#1", MixerGroup.None, 0.6f);

			__instance.Shake(0.015f, 0.3f);
			GrimoraSaveUtil.IsGrimoraModRun = true;
			bool resetRun = GrimoraSaveManager.CurrentSaveFile.CurrentRun == null || GrimoraSaveManager.CurrentSaveFile.CurrentRun.playerLives <= 0;
			CustomCoroutine.Instance.StartCoroutine(__instance.TransitionToGame2(resetRun));
			return false;
		}

		return true;
	}

	[HarmonyPostfix, HarmonyPatch(nameof(MenuController.Start))]
	public static void AddGrimoraCard(MenuController __instance)
	{
		if (SceneManager.GetActiveScene().name.Equals("Start"))
		{
			if (__instance.cardRow.Find("MenuCard_Grimora").SafeIsUnityNull())
			{
				Log.LogDebug($"Non-hot reload menu button creation");
				__instance.cards.Add(CreateMenuButton(__instance));
			}
		}
		else if (GrimoraSaveUtil.IsGrimoraModRun)
		{
			if (__instance.cardRow.Find("MenuCard_ResetRun").SafeIsUnityNull())
			{
				CreateButtonResetRun(__instance);
			}
		}
	}
	
	[HarmonyPrefix, HarmonyPatch(nameof(MenuController.LoadGameFromMenu))]
	[HarmonyBefore(ConfigHelper.P03ModGuid)]
	public static bool LoadGameFromMenu(bool newGameGBC)
	{
		Log.LogDebug($"[MenuController.LoadGameFromMenu] " +
		             $"NewGameGBC [{newGameGBC}] " +
		             $"SaveFile.IsAscension [{SaveFile.IsAscension}] " +
		             $"Is Grimora run? [{GrimoraSaveUtil.IsGrimoraModRun}] " +
		             $"CurrentRun [{AscensionSaveData.Data.currentRun}]");
		if (!newGameGBC && SaveFile.IsAscension && GrimoraSaveUtil.IsGrimoraModRun)
		{
			Log.LogDebug($"[MenuController.LoadGameFromMenu] --> Save file is ascension and IsGrimoraRun");
			SaveManager.LoadFromFile();
			LoadingScreenManager.LoadScene("finale_grimora");
			SaveManager.savingDisabled = false;
			return false;
		}

		return true;
	}

	public static MenuCard CreateButtonResetRun(MenuController controller)
	{
		Log.LogDebug("Creating ResetRun button");

		var libraryCard = controller.cards.Single(card => card.menuAction == MenuAction.Library);
		UnityObject.Destroy(libraryCard.transform.Find("GlitchedVersion").gameObject);
		libraryCard.name = "MenuCard_ResetRun";
		libraryCard.GetComponent<SpriteRenderer>().sprite = AssetUtils.GetPrefab<Sprite>("MenuCard_ResetRun");
		libraryCard.GetComponent<SpriteRenderer>().enabled = true;
		libraryCard.menuAction = MenuAction.EndRun;
		libraryCard.lockBeforeStoryEvent = false;
		libraryCard.lockAfterStoryEvent = false;
		libraryCard.permanentlyLocked = false;
		libraryCard.glitchedCard = null;
		libraryCard.storyEvent = StoryEvent.BasicTutorialCompleted;
		libraryCard.lockedTitleSprite = null;
		libraryCard.titleSprite = null;
		libraryCard.titleText = "RESET RUN";
		libraryCard.defaultBorderColor = GameColors.instance.red;

		return libraryCard;
	}

	public static MenuCard CreateMenuButton(MenuController controller)
	{
		Log.LogDebug("Creating MenuCard button");

		var cardRow = controller.transform.Find("CardRow").GetComponent<StartMenuAscensionCardInitializer>();
		bool doesAscensionCardExist = cardRow.menuCards.Count == 6;
		float xPosition = doesAscensionCardExist ? 1.603f : 1.373f;

		MenuCard menuCardGrimora = UnityObject.Instantiate(
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

		if (doesAscensionCardExist)
		{
			cardRow.transform.position -= new Vector3(0.22f, 0, 0);
		}


		//add Logo
		GameObject Logo = new GameObject();
		Logo.transform.parent = GameObject.Find("CardMenuTitle").transform;



		return menuCardGrimora;
	}
}

public static class TransitionExt
{
	public static IEnumerator TransitionToGame2(bool resetRun = false)
	{
		yield return TransitionToGame2(MenuController.Instance, resetRun);
	}

	public static IEnumerator TransitionToGame2(this MenuController controller, bool resetRun = false)
	{
		Log.LogDebug($"[TransitionToGame2] TransitioningToScene = true");
		if (controller)
		{
			controller.TransitioningToScene = true;
		}

		yield return new WaitForSecondsRealtime(0.75f);
		if (AudioController.Instance)
		{
			Log.LogDebug($"[TransitionToGame2] Fade Out Loop");
			AudioController.Instance.FadeOutLoop(0.75f);
		}

		if (controller)
		{
			Log.LogDebug($"[TransitionToGame2] Fade Out");
			yield return controller.FadeOut();
		}

		yield return new WaitForSecondsRealtime(0.75f);
		if (resetRun)
		{
			Log.LogDebug($"[TransitionToGame2] Before reset run");
			GrimoraSaveManager.ResetRun();
			yield return new WaitForSecondsRealtime(0.75f);
		}

		// SaveManager.LoadFromFile();
		SceneLoader.Load("finale_grimora");
		Time.timeScale = 1f;
		FrameLoopManager.Instance.SetIterationDisabled(false);
	}
}
