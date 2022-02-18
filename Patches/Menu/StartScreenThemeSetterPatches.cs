using System.Collections;
using DiskCardGame;
using HarmonyLib;
using UnityEngine;
using static GrimoraMod.GrimoraPlugin;

namespace GrimoraMod;

[HarmonyPatch(typeof(StartScreenThemeSetter))]
public class StartScreenPatches
{
	[HarmonyPostfix, HarmonyPatch(nameof(StartScreenThemeSetter.Start))]
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
	}
}
