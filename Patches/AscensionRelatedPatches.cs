using BepInEx.Bootstrap;
using DiskCardGame;
using GBC;
using HarmonyLib;
using UnityEngine;
using static GrimoraMod.GrimoraPlugin;

namespace GrimoraMod;

[HarmonyPatch(typeof(AscensionMenuScreens))]
public class AscensionRelatedPatches
{
	[HarmonyPrefix, HarmonyPatch(nameof(AscensionMenuScreens.TransitionToGame))]
	[HarmonyBefore("zorro.inscryption.infiniscryption.packmanager", P03ModGuid)]
	public static bool InitializeGrimoraSaveData(ref AscensionMenuScreens __instance, bool newRun = true)
	{
		Log.LogDebug($"[AscensionMenuScreens.TransitionToGame] " +
		             $"IsGrimoraRun [{SaveDataRelatedPatches.IsGrimoraRun}] " +
		             $"newRun [{newRun}] " +
		             $"screen state [{ScreenManagement.ScreenState}] " +
		             $"currentStarterDeck [{AscensionSaveData.Data.currentStarterDeck}]" +
		             $"currentRun [{AscensionSaveData.Data.currentRun}]"
		            );
		if (newRun)
		{
			if (ScreenManagement.ScreenState == CardTemple.Undead)
			{
				// Ensure the old grimora save data gets saved if it needs to be
				Log.LogInfo($"[AscensionMenuScreens.TransitionToGame] Ensuring regular save");
				SaveDataRelatedPatches.EnsureRegularSave();
				SaveDataRelatedPatches.IsGrimoraRun = true;
				SaveManager.SaveToFile();
			}
			else
			{
				SaveDataRelatedPatches.IsGrimoraRun = false;
			}
		}

		if (SaveDataRelatedPatches.IsGrimoraRun)
		{
			Log.LogInfo($"[AscensionMenuScreens.TransitionToGame] Setting ScreenState to Undead");
			ScreenManagement.ScreenState = CardTemple.Undead;
			if (AscensionSaveData.Data.currentStarterDeck.Equals("Vanilla", StringComparison.InvariantCultureIgnoreCase))
			{
				Log.LogInfo($"[AscensionMenuScreens.TransitionToGame] --> Changing current starter deck to default");
				AscensionSaveData.Data.currentStarterDeck = StarterDecks.DefaultStarterDeck;
			}
			
			Log.LogInfo($"[AscensionMenuScreens.TransitionToGame] Creating new ascension run from starter deck [{StarterDecks.DefaultStarterDeck}]");
			StarterDeckInfo deckInfo = StarterDecksUtil.GetInfo(StarterDecks.DefaultStarterDeck);
			if (deckInfo)
			{
				Log.LogInfo($"[AscensionMenuScreens.TransitionToGame] --> DeckInfo not null [{deckInfo.cards.Join(c => c.name)}]");
				AscensionSaveData.Data.NewRun(deckInfo.cards);
				SaveManager.SaveToFile(false);
			}
			else
			{
				Log.LogWarning($"[AscensionMenuScreens.TransitionToGame] --> StarterDeck is null, did it get created correctly? Current starter decks: [{StarterDecksUtil.AllData.Join(data => data.title)}]");
			}

			CustomCoroutine.WaitThenExecute(0.25f, delegate
			{
				MenuController.LoadGameFromMenu(false);
			});
			InteractionCursor.Instance.SetHidden(true);
			__instance.DeactivateAllScreens();

			return false;
		}

		return true;
	}

	private static void ClearGrimoraData()
	{
		if (!AscensionMenuScreens.ReturningFromFailedRun && !AscensionMenuScreens.ReturningFromSuccessfulRun)
		{
			ScreenManagement.ScreenState = CardTemple.Undead;
		}
	}

	[HarmonyPrefix, HarmonyPatch(nameof(AscensionMenuScreens.SwitchToScreen))]
	public static void ClearGrimoraSaveOnNewRun(AscensionMenuScreens __instance, AscensionMenuScreens.Screen screen)
	{
		if (screen == AscensionMenuScreens.Screen.Start) // At the main screen, you can't be in any style of run. Not yet.
		{
			ClearGrimoraData();
		}
	}

	[HarmonyPrefix, HarmonyPatch(nameof(AscensionMenuScreens.Start))]
	public static void ClearScreenStatePrefix(AscensionMenuScreens __instance)
	{
		ClearGrimoraData();
	}

	public const string P03ModGuid = "zorro.inscryption.infiniscryption.p03kayceerun";

	[HarmonyPostfix, HarmonyPatch(nameof(AscensionMenuScreens.Start))]
	[HarmonyAfter(P03ModGuid)]
	public static void AddGrimoraStartOption(AscensionMenuScreens __instance)
	{
		AdjustAscensionMenuItemsSpacing itemsSpacing = UnityObject.FindObjectOfType<AdjustAscensionMenuItemsSpacing>();
		AscensionMenuInteractable menuText = itemsSpacing.menuItems[0].GetComponent<AscensionMenuInteractable>();
		
		AscensionMenuScreenTransition transitionController = AscensionMenuScreens.Instance.startScreen.GetComponent<AscensionMenuScreenTransition>();
		List<GameObject> onEnableRevealedObjects = transitionController.onEnableRevealedObjects;
		List<MainInputInteractable> screenInteractables = transitionController.screenInteractables;
		
		bool hasP03Mod = Chainloader.PluginInfos.ContainsKey(P03ModGuid);
		if (hasP03Mod)
		{
			GameObject p03Button = onEnableRevealedObjects.Single(obj => obj.name == "Menu_New_P03");
			Log.LogDebug($"[AscensionMenuScreens.Start] Has P03 mod installed");
			if (!itemsSpacing.menuItems.Exists(obj => obj.name == "Menu_New_P03"))
			{
				itemsSpacing.menuItems.Insert(1, p03Button.transform);
			}
		}
		else
		{
			Log.LogDebug($"[AscensionMenuScreens.Start] Setting new text for continue button...");
			menuText.GetComponentInChildren<PixelText>().SetText("- NEW LESHY RUN -");
		}

		// Clone the new button
		AscensionMenuInteractable grimoraButtonController = CreateAscensionButton(menuText);
		
		// Add to transition
		onEnableRevealedObjects.Insert(onEnableRevealedObjects.IndexOf(menuText.gameObject) + 1, grimoraButtonController.gameObject);
		screenInteractables.Insert(screenInteractables.IndexOf(menuText) + 1, grimoraButtonController);
		itemsSpacing.menuItems.Insert(1, grimoraButtonController.transform);

		for (int i = 1; i < itemsSpacing.menuItems.Count; i++)
		{
			Transform item = itemsSpacing.menuItems[i];
			item.localPosition = new Vector2(item.localPosition.x, i * -0.11f);
		}

		// itemsSpacing.SpaceOutItems();
	}

	private static AscensionMenuInteractable CreateAscensionButton(AscensionMenuInteractable newRunButton)
	{
		Log.LogDebug($"[AscensionMenuScreens.Start] Creating new Grimora ascension run button");
		AscensionMenuInteractable newGrimoraButton = UnityObject.Instantiate(newRunButton, newRunButton.transform.parent);
		newGrimoraButton.name = "Menu_New_Grimora";
		newGrimoraButton.CursorSelectStarted = delegate
		{
			ScreenManagement.ScreenState = CardTemple.Undead;
			Log.LogDebug($"[AscensionMenuScreens.Start] Set screen state to undead, invoking CursorSelectStart");
			newRunButton.CursorSelectStart();
		};
		newGrimoraButton.GetComponentInChildren<PixelText>().SetText("- NEW GRIMORA RUN -");

		return newGrimoraButton;
	}
}
