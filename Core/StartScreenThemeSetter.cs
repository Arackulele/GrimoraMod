using System.Collections;
using DiskCardGame;
using HarmonyLib;
using UnityEngine;
using Resources = GrimoraMod.Properties.Resources;

namespace GrimoraMod
{
	[HarmonyPatch(typeof(StartScreenThemeSetter))]
	public class StartScreenThemeSetterPatches
	{
		[HarmonyPrefix, HarmonyPatch(nameof(StartScreenThemeSetter.Start))]
		public static void Prefix(StartScreenThemeSetter __instance)
		{
			var grimoraTheme = __instance.themes[0];
			if (ColorUtility.TryParseHtmlString("0F2623", out var color))
			{
				grimoraTheme.fillColor = color;
			}

			Texture2D tex3 = ImageUtils.LoadTextureFromResource(Resources.background_grimora);

			grimoraTheme.bgSpriteWide = Sprite.Create(
				tex3,
				new Rect(0.0f, 0.0f, tex3.width, tex3.height),
				new Vector2(0.5f, 0.5f)
			);

			grimoraTheme.triggeringEvent = StoryEvent.PlayerDeletedArchivistFile;
			__instance.themes.Add(grimoraTheme);
		}

		[HarmonyPostfix, HarmonyPatch(nameof(StartScreenThemeSetter.Start))]
		public static void Postfix(StartScreenThemeSetter __instance)
		{
			foreach (var theme in __instance.themes)
			{
				// GrimoraPlugin.Log.LogDebug($"Theme triggering event is [{theme.triggeringEvent}]");
			}

			var findPlayerDeletedArchivistFile = __instance.themes.Find(theme =>
				theme.triggeringEvent == StoryEvent.PlayerDeletedArchivistFile);

			__instance.SetTheme(findPlayerDeletedArchivistFile);
			// GrimoraPlugin.Log.LogDebug(findPlayerDeletedArchivistFile);

			__instance.StartCoroutine(CreateButton());
		}

		public static IEnumerator CreateButton()
		{
			yield return new WaitUntil(() => Singleton<StartScreenController>.Instance.menu.gameObject.activeSelf);
			yield return new WaitForSeconds(1.8f);
			
			GameObject cardRow = GameObject.Find("CardRow");

			// GrimoraPlugin.Log.LogDebug("Finding MenuCard_Continue gameObject");
			GameObject menuCardContinue = UnityEngine.Object.Instantiate(
				GameObject.Find("MenuCard_Quit"), cardRow.transform 
			);

			menuCardContinue.name = "MenuCard_Grimora";

			Texture2D tex3 = ImageUtils.LoadTextureFromResource(Resources.menucard_grimora);
			menuCardContinue.GetComponent<SpriteRenderer>().sprite = Sprite.Create(
				tex3,
				new Rect(0.0f, 0.0f, tex3.width, tex3.height),
				new Vector2(0.5f, 0.5f)
			);

			MenuCard menuCardComp = menuCardContinue.GetComponent<MenuCard>();
			menuCardComp.StartPosition = new Vector2(1.378f, 0f);
			menuCardComp.targetPosition = new Vector2(1.378f, 0f);
			menuCardComp.rotationCenter = new Vector2(1.378f, 0f);
			menuCardComp.menuAction = MenuAction.Continue;
			menuCardComp.titleText = "Start Grimora Mod";
		}
	}
}