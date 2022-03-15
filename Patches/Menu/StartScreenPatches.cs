using DiskCardGame;
using HarmonyLib;
using UnityEngine;
using static GrimoraMod.GrimoraPlugin;

namespace GrimoraMod;

[HarmonyPatch(typeof(StartScreenThemeSetter))]
public class StartScreenPatches
{
	// [HarmonyPostfix, HarmonyPatch(nameof(StartScreenThemeSetter.Start))]
	public static void SetBackgroundToGrimoraTheme(StartScreenThemeSetter __instance)
	{
		Log.LogDebug($"Setting new background theme");
		var grimoraTheme = __instance.themes[0];
		if (ColorUtility.TryParseHtmlString("0F2623", out var color))
		{
			grimoraTheme.fillColor = color;
		}

		grimoraTheme.bgSpriteWide = AssetUtils.GetPrefab<Sprite>("Background");
		grimoraTheme.triggeringEvent = StoryEvent.PlayerDeletedArchivistFile;

		__instance.SetTheme(grimoraTheme);
	}
}
