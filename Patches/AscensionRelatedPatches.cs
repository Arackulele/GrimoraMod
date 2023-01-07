using DiskCardGame;
using GBC;
using HarmonyLib;
using InscryptionAPI.Ascension;
using Sirenix.Utilities;
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












[HarmonyPatch(typeof(AscensionStartScreen), nameof(AscensionStartScreen.Start))]
public static class RunStartWhenEnabled
{
	[HarmonyPrefix]
	public static void Prefix(ref AscensionStartScreen __instance)
	{
		AdjustAscensionMenuItemsSpacing itemsSpacing = UnityObject.FindObjectOfType<AdjustAscensionMenuItemsSpacing>();
		AscensionMenuInteractable menuText = itemsSpacing.menuItems[0].GetComponent<AscensionMenuInteractable>();

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
			menuText.GetComponentInChildren<PixelText>().SetText("- NEW LESHY RUN -");
		}

		// Clone the new button
		AscensionMenuInteractable grimoraButtonController = AscensionRelatedPatches.CreateAscensionButton(menuText);

		// Add to transition

		onEnableRevealedObjects.Insert(onEnableRevealedObjects.IndexOf(menuText.gameObject) + 1, grimoraButtonController.gameObject);
		screenInteractables.Insert(screenInteractables.IndexOf(menuText) + 1, grimoraButtonController);
		foreach (var button in GetInteractableButtons(screenInteractables))
		{
			button.CursorSelectStarted += delegate(MainInputInteractable interactable)
			{
				var scrybe = button.GetComponentInChildren<PixelText>().Text.Replace("- NEW ", "").Replace(" RUN -", "");
				switch (scrybe)
				{
					case "GRIMORA":
					{
						ScreenManagement.ScreenState = CardTemple.Undead;
						SaveDataRelatedPatches.IsGrimoraRun = true;
						break;
					}
					case "P03":
					{
						ScreenManagement.ScreenState = CardTemple.Tech;
						SaveDataRelatedPatches.IsGrimoraRun = false;
						break;
					}
					case "LESHY":
					{
						ScreenManagement.ScreenState = CardTemple.Nature;
						SaveDataRelatedPatches.IsGrimoraRun = false;
						break;
					}
				}

				ChallengeManager.SyncChallengeList();
			};
		}

		itemsSpacing.menuItems.Insert(1, grimoraButtonController.transform);

		for (int i = 1; i < itemsSpacing.menuItems.Count; i++)
		{
			Transform item = itemsSpacing.menuItems[i];
			item.localPosition = new Vector2(item.localPosition.x, i * -0.11f);
		}

		// itemsSpacing.SpaceOutItems();
	}

	private static List<MainInputInteractable> GetInteractableButtons(List<MainInputInteractable> screenInteractables)
	{
		return screenInteractables.FindAll(b => b.gameObject.GetComponentInChildren<PixelText>().Text.StartsWith("- NEW") &&
		                                        b.gameObject.GetComponentInChildren<PixelText>().Text.EndsWith("RUN -"));
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
				SaveDataRelatedPatches.IsGrimoraRun = true;
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
	
	
	
	


	internal static AscensionMenuInteractable CreateAscensionButton(AscensionMenuInteractable newRunButton)
	{
		Log.LogDebug($"[AscensionMenuScreens.Start] Creating new Grimora ascension run button");
		AscensionMenuInteractable newGrimoraButton = UnityObject.Instantiate(newRunButton, newRunButton.transform.parent);
		newGrimoraButton.name = "Menu_New_Grimora";
		newGrimoraButton.CursorSelectStarted = delegate
		{
			SaveDataRelatedPatches.IsGrimoraRun = true;
			ScreenManagement.ScreenState = CardTemple.Undead;
			ChallengeManager.SyncChallengeList();
			Log.LogDebug($"[AscensionMenuScreens.Start] Set screen state to undead, invoking CursorSelectStart");
			newRunButton.CursorSelectStart();
		};
		newGrimoraButton.GetComponentInChildren<PixelText>().SetText("- NEW GRIMORA RUN -");

		return newGrimoraButton;
	}
}
