using System.Collections;
using DiskCardGame;
using HarmonyLib;
using UnityEngine;
using static GrimoraMod.GrimoraPlugin;

namespace GrimoraMod;

[HarmonyPatch(typeof(StartScreenThemeSetter))]
public class StartScreenThemeSetterPatches
{
	[HarmonyPrefix, HarmonyPatch(nameof(StartScreenThemeSetter.Start))]
	public static void AddGrimoraModMenuCardButton(StartScreenThemeSetter __instance)
	{
		var grimoraTheme = __instance.themes[0];
		if (ColorUtility.TryParseHtmlString("0F2623", out var color))
		{
			grimoraTheme.fillColor = color;
		}

		grimoraTheme.bgSpriteWide = AssetUtils.GetPrefab<Sprite>("Background");

		grimoraTheme.triggeringEvent = StoryEvent.PlayerDeletedArchivistFile;
		__instance.themes.Add(grimoraTheme);

		var findPlayerDeletedArchivistFile = __instance.themes.Find(theme =>
			theme.triggeringEvent == StoryEvent.PlayerDeletedArchivistFile);

		__instance.SetTheme(findPlayerDeletedArchivistFile);
		__instance.StartCoroutine(CreateButton());
	}

	public static IEnumerator CreateButton()
	{
		GameObject cardRow = null;

		yield return new WaitUntil(() => cardRow = GameObject.Find("CardRow"));

		// GrimoraPlugin.Log.LogDebug("Finding MenuCard_Continue gameObject");
		MenuCard menuCardGrimora = Object.Instantiate(
			ResourceBank.Get<MenuCard>("Prefabs/StartScreen/StartScreenMenuCard"),
			cardRow.transform
		);

		menuCardGrimora.GetComponent<SpriteRenderer>().sprite = AssetUtils.GetPrefab<Sprite>("MenuCard");

		menuCardGrimora.name = "MenuCard_Grimora";

		menuCardGrimora.StartPosition = new Vector2(1.378f, 0f);
		menuCardGrimora.targetPosition = new Vector2(1.378f, 0f);
		menuCardGrimora.rotationCenter = new Vector2(1.378f, 0f);
		menuCardGrimora.menuAction = MenuAction.Continue;
		menuCardGrimora.titleText = "Start Grimora Mod";
		menuCardGrimora.titleSprite = AssetUtils.GetPrefab<Sprite>("menutext_grimora_mod");

		Vector3 cardRowLocalPosition = cardRow.transform.localPosition;
		cardRow.transform.localPosition = new Vector3(-0.23f, cardRowLocalPosition.y, cardRowLocalPosition.z);
	}
}
