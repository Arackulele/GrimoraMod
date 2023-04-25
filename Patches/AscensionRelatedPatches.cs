using DiskCardGame;
using GBC;
using GrimoraMod.Saving;
using HarmonyLib;
using InscryptionAPI.Ascension;
using UnityEngine;
using static GrimoraMod.GrimoraPlugin;

namespace GrimoraMod;

[HarmonyPatch(typeof(CardInfo), nameof(CardInfo.EnergyCost), MethodType.Getter)]
public static class SoullessPatch
{
	
	[HarmonyPostfix]
	public static void EnergyChange(ref int __result, ref CardInfo __instance)
	{
		if (__instance.name != "Skeleton") return;
		int num=0;
		num = AscensionSaveData.Data.GetNumChallengesOfTypeActive(ChallengeManagement.Soulless);
		if (num > 0)
		{
			__result += num;
		}
	}
}

[HarmonyPatch(typeof(AscensionStartScreen), nameof(AscensionStartScreen.OnEnable))]
public static class AscensionStartScreen_OnEnable
{
	[HarmonyPrefix]
	public static void Prefix()
	{
		Log.LogDebug($"[AscensionMenuScreens.OnEnable]");
		GrimoraSaveUtil.IsGrimoraModRun = false; // No runs active right now!
	}
}

[HarmonyPatch(typeof(AscensionMenuScreens), nameof(AscensionMenuScreens.ConfigurePostGameScreens), new Type[]{})]
public static class AscensionMenuScreens_ConfigurePostGameScreens_EndRunFix
{
	// When we end a run we want to end the correct one.
	// Set isGromoraModRun to true so this method uses the correct data when calling EndRun().
	public static bool Prefix()
	{
		GrimoraSaveUtil.IsGrimoraModRun = GrimoraSaveUtil.LastRunWasGrimoraModRun;
		return true;
	}
	
	public static void Postfix()
	{
		GrimoraSaveUtil.IsGrimoraModRun = false;
	}
}

[HarmonyPatch(typeof(AscensionStartScreen), nameof(AscensionStartScreen.Start))]
public static class CreateAscensionButtonsOnStart
{
	[HarmonyPrefix]
	public static void Prefix(ref AscensionStartScreen __instance)
	{
		Log.LogDebug($"[AscensionMenuScreens.Start] CreateAscensionButtonsOnStart");
		AscensionStartScreen ascensionStartScreen = UnityObject.FindObjectOfType<AscensionStartScreen>();
		AdjustAscensionMenuItemsSpacing itemsSpacing = UnityObject.FindObjectOfType<AdjustAscensionMenuItemsSpacing>();
		AscensionMenuInteractable newGameButtonTemplate = ascensionStartScreen.newRunText;
		AscensionMenuInteractable continueGameButtonTemplate = ascensionStartScreen.continueRunText;
		AscensionMenuInteractable continueGameButtonDisabledTemplate = ascensionStartScreen.continueRunDisabledText;
		Log.LogDebug($"[AscensionMenuScreens.Start] continueGameButtonTemplate " + continueGameButtonTemplate);

		AscensionMenuScreenTransition transitionController = AscensionMenuScreens.Instance.startScreen.GetComponent<AscensionMenuScreenTransition>();
		List<GameObject> onEnableRevealedObjects = transitionController.onEnableRevealedObjects;
		List<MainInputInteractable> screenInteractables = transitionController.screenInteractables;

		if (ConfigHelper.HasP03Mod)
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
			newGameButtonTemplate.GetComponentInChildren<PixelText>().SetText("- NEW LESHY RUN -");
			continueGameButtonTemplate.GetComponentInChildren<PixelText>().SetText("- CONTINUE LESHY RUN -");
			continueGameButtonDisabledTemplate.GetComponentInChildren<PixelText>().SetText(continueGameButtonTemplate.GetComponentInChildren<PixelText>().Text);
		}
		
		// Clone the new button
		Transform continueGrimoraButtonContainer = CreateContinueGrimoraAscensionButtons(ascensionStartScreen);
		AscensionMenuInteractable newGrimoraButtonController = CreateNewGrimoraAscensionButton(newGameButtonTemplate);
		Log.LogDebug($"[AscensionMenuScreens.Start] continueGrimoraButtonController " + continueGrimoraButtonContainer);

		// Add to transition

		onEnableRevealedObjects.Insert(onEnableRevealedObjects.IndexOf(newGameButtonTemplate.gameObject) + 1, newGrimoraButtonController.gameObject);
		screenInteractables.Insert(screenInteractables.IndexOf(newGameButtonTemplate) + 1, newGrimoraButtonController);
		onEnableRevealedObjects.Insert(onEnableRevealedObjects.IndexOf(continueGameButtonTemplate.gameObject) + 1, continueGrimoraButtonContainer.gameObject);
		screenInteractables.Insert(screenInteractables.IndexOf(continueGameButtonTemplate) + 1, continueGrimoraButtonContainer.GetChild(0).GetComponent<MainInputInteractable>());
		screenInteractables.Insert(screenInteractables.IndexOf(continueGameButtonTemplate) + 2, continueGrimoraButtonContainer.GetChild(1).GetComponent<MainInputInteractable>());
		foreach (var button in GetInteractableButtons(screenInteractables))
		{
			button.CursorSelectStarted += delegate(MainInputInteractable interactable)
			{
				var scrybe = button.GetComponentInChildren<PixelText>().Text.Replace("- NEW ", "").Replace(" RUN -", "").Replace("- CONTINUE ", "");
				switch (scrybe)
				{
					case "GRIMORA":
					{
						ScreenManagement.ScreenState = CardTemple.Undead;
						GrimoraSaveUtil.IsGrimoraModRun = true;
						break;
					}
					case "P03":
					{
						ScreenManagement.ScreenState = CardTemple.Tech;
						GrimoraSaveUtil.IsGrimoraModRun = false;
						break;
					}
					case "LESHY":
					{
						ScreenManagement.ScreenState = CardTemple.Nature;
						GrimoraSaveUtil.IsGrimoraModRun = false;
						break;
					}
				}

				ChallengeManager.SyncChallengeList();
				GrimoraSaveUtil.LastRunWasGrimoraModRun = GrimoraSaveUtil.IsGrimoraModRun;
				Log.LogDebug($"[AscensionMenuScreens.Start] CursorSelectStarted scrybe[{scrybe}] ScreenState[{ScreenManagement.ScreenState}] IsGrimoraModRun[{GrimoraSaveUtil.IsGrimoraModRun}]");
			};
		}

		itemsSpacing.menuItems.Insert(1, newGrimoraButtonController.transform);
		itemsSpacing.menuItems.Insert(itemsSpacing.menuItems.IndexOf(continueGameButtonTemplate.transform.parent) + 1, continueGrimoraButtonContainer.transform);

		for (int i = 1; i < itemsSpacing.menuItems.Count; i++)
		{
			Transform item = itemsSpacing.menuItems[i];
			item.localPosition = new Vector2(item.localPosition.x, i * -0.11f);
		}
	}

	private static List<MainInputInteractable> GetInteractableButtons(List<MainInputInteractable> screenInteractables)
	{
		return screenInteractables.FindAll(b =>
		{
			string buttonText = b.gameObject.GetComponentInChildren<PixelText>().Text;
			bool startsWith = buttonText.StartsWith("- NEW") || buttonText.StartsWith("- CONTINUE");
			return startsWith && buttonText.EndsWith("RUN -");
		});
	}

	internal static AscensionMenuInteractable CreateNewGrimoraAscensionButton(AscensionMenuInteractable newRunButton)
	{
		Log.LogDebug($"[AscensionMenuScreens.Start] Creating new Grimora ascension run button");
		AscensionMenuInteractable newGrimoraButton = UnityObject.Instantiate(newRunButton, newRunButton.transform.parent);
		newGrimoraButton.name = "Menu_New_Grimora";
		newGrimoraButton.CursorSelectStarted = delegate
		{
			GrimoraSaveUtil.IsGrimoraModRun = true;
			GrimoraSaveUtil.LastRunWasGrimoraModRun = GrimoraSaveUtil.IsGrimoraModRun;
			ScreenManagement.ScreenState = CardTemple.Undead;
			ChallengeManager.SyncChallengeList();
			GrimoraSaveManager.NewAscensionRun();
			Log.LogDebug($"[AscensionMenuScreens.Start] Set screen state to undead, invoking CursorSelectStart");
			newRunButton.CursorSelectStart();
		};
		newGrimoraButton.GetComponentInChildren<PixelText>().SetText("- NEW GRIMORA RUN -");

		return newGrimoraButton;
	}

	internal static Transform CreateContinueGrimoraAscensionButtons(AscensionStartScreen startScreen)
	{
		Log.LogDebug($"[AscensionMenuScreens.Start] Creating continue Grimora ascension run button");
		Transform container = UnityObject.Instantiate(startScreen.continueRunText.transform.parent, startScreen.continueRunText.transform.parent.parent);

		// Enabled button
		AscensionMenuInteractable enabledButton = container.GetChild(0).GetComponent<AscensionMenuInteractable>();
		enabledButton.name = "Menu_Continue_Grimora";
		enabledButton.CursorSelectStarted = delegate
		{
			GrimoraSaveUtil.IsGrimoraModRun = true;
			GrimoraSaveUtil.LastRunWasGrimoraModRun = GrimoraSaveUtil.IsGrimoraModRun;
			ScreenManagement.ScreenState = CardTemple.Undead;
			Log.LogDebug($"[AscensionMenuScreens.Start] Set screen state to undead, invoking CursorSelectStart");
			startScreen.continueRunText.CursorSelectStart();
		};
		enabledButton.GetComponentInChildren<PixelText>().SetText("- CONTINUE GRIMORA RUN -");
		enabledButton.gameObject.SetActive(GrimoraAscensionSaveData.RunExists);
		
		// Disabled button
		AscensionMenuInteractable disabledButton = container.GetChild(1).GetComponent<AscensionMenuInteractable>();
		disabledButton.name = "Menu_Continue_Grimora_Disabled";
		disabledButton.GetComponentInChildren<PixelText>().SetText("- CONTINUE GRIMORA RUN -");
		disabledButton.gameObject.SetActive(!GrimoraAscensionSaveData.RunExists);

		return container;
	}
}


[HarmonyPatch(typeof(AscensionMenuScreens))]
public class AscensionRelatedPatches
{
	[HarmonyPrefix, HarmonyPatch(nameof(AscensionMenuScreens.TransitionToGame))]
	[HarmonyBefore(ConfigHelper.PackManagerGuid, ConfigHelper.P03ModGuid)]
	public static bool InitializeGrimoraSaveData(ref AscensionMenuScreens __instance, bool newRun = true)
	{
		Log.LogInfo($"[AscensionMenuScreens.TransitionToGame] " +
		            $"IsGrimoraRun [{GrimoraSaveUtil.IsGrimoraModRun}] " +
		            $"newRun [{newRun}] " +
		            $"screen state [{ScreenManagement.ScreenState}] " +
		            $"currentStarterDeck [{AscensionSaveData.Data.currentStarterDeck}]" +
		            $"currentRun [{AscensionSaveData.Data.currentRun}]"
		);

		GrimoraSaveUtil.IsGrimoraModRun = ScreenManagement.ScreenState == CardTemple.Undead;
		GrimoraSaveUtil.LastRunWasGrimoraModRun = GrimoraSaveUtil.IsGrimoraModRun;
		
		if (newRun && GrimoraSaveUtil.IsGrimoraModRun)
		{
			GrimoraAscensionSaveData ascensionSaveData = (GrimoraAscensionSaveData)AscensionSaveData.Data;
			Log.LogInfo($"[AscensionMenuScreens.TransitionToGame] Setting ScreenState to Undead");
			if (ascensionSaveData.currentStarterDeck.Equals("Vanilla", StringComparison.InvariantCultureIgnoreCase))
			{
				Log.LogInfo($"[AscensionMenuScreens.TransitionToGame] --> Changing current starter deck to default");
				ascensionSaveData.currentStarterDeck = StarterDecks.DefaultStarterDeck;
			}

			Log.LogInfo($"[AscensionMenuScreens.TransitionToGame] Creating new ascension run from starter deck [{ascensionSaveData.currentStarterDeck}]");
			StarterDeckInfo deckInfo = StarterDecksUtil.GetInfo(ascensionSaveData.currentStarterDeck);
			if (deckInfo)
			{
				Log.LogInfo($"[AscensionMenuScreens.TransitionToGame] --> DeckInfo not null [{deckInfo.cards.Join(c => c.name)}]");
				ascensionSaveData.NewRun(deckInfo.cards);
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
}
