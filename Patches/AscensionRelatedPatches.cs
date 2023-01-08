using DiskCardGame;
using GBC;
using GrimoraMod.Saving;
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
						SaveDataRelatedPatches.IsGrimoraModRun = true;
						break;
					}
					case "P03":
					{
						ScreenManagement.ScreenState = CardTemple.Tech;
						SaveDataRelatedPatches.IsGrimoraModRun = false;
						break;
					}
					case "LESHY":
					{
						ScreenManagement.ScreenState = CardTemple.Nature;
						SaveDataRelatedPatches.IsGrimoraModRun = false;
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
		GrimoraAscensionSaveData ascensionSaveData = (GrimoraAscensionSaveData)AscensionSaveData.Data;
		Log.LogInfo($"[AscensionMenuScreens.TransitionToGame] " +
		            $"IsGrimoraRun [{SaveDataRelatedPatches.IsGrimoraModRun}] " +
		            $"newRun [{newRun}] " +
		            $"screen state [{ScreenManagement.ScreenState}] " +
		            $"currentStarterDeck [{ascensionSaveData.currentStarterDeck}]" +
		            $"currentRun [{ascensionSaveData.currentRun}]"
		);

		SaveDataRelatedPatches.IsGrimoraModRun = ScreenManagement.ScreenState == CardTemple.Undead;
		
		if (newRun && SaveDataRelatedPatches.IsGrimoraModRun)
		{
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

	internal static AscensionMenuInteractable CreateAscensionButton(AscensionMenuInteractable newRunButton)
	{
		Log.LogDebug($"[AscensionMenuScreens.Start] Creating new Grimora ascension run button");
		AscensionMenuInteractable newGrimoraButton = UnityObject.Instantiate(newRunButton, newRunButton.transform.parent);
		newGrimoraButton.name = "Menu_New_Grimora";
		newGrimoraButton.CursorSelectStarted = delegate
		{
			SaveDataRelatedPatches.IsGrimoraModRun = true;
			ScreenManagement.ScreenState = CardTemple.Undead;
			ChallengeManager.SyncChallengeList();
			GrimoraSaveManager.CurrentSaveFile.NewAscensionRun();
			Log.LogDebug($"[AscensionMenuScreens.Start] Set screen state to undead, invoking CursorSelectStart");
			newRunButton.CursorSelectStart();
		};
		newGrimoraButton.GetComponentInChildren<PixelText>().SetText("- NEW GRIMORA RUN -");

		return newGrimoraButton;
	}
}
