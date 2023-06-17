using DiskCardGame;
using HarmonyLib;
using UnityEngine;

namespace GrimoraMod;

[HarmonyPatch(typeof(RuleBookController))]
public class RulebookControllerPatches
{
	[HarmonyPrefix, HarmonyPatch(nameof(RuleBookController.Start))]
	public static void ChangeRuleBookInfo(ref RuleBookController __instance)
	{
		if (GrimoraSaveUtil.IsNotGrimoraModRun)
		{
			return;
		}

		GameObject grimoraRangePrefab = __instance.bookInfo.pageRanges[0].rangePrefab;
		__instance.bookInfo = ResourceBank.Get<RuleBookInfo>("Data/rulebook/RuleBookInfo");
		foreach (var pageRange in __instance.bookInfo.pageRanges)
		{
			pageRange.rangePrefab = grimoraRangePrefab;
		}
	}
}

[HarmonyPatch(typeof(PageContentLoader))]
public class PageContentLoaderPatch
{
	[HarmonyPrefix, HarmonyPatch(nameof(PageContentLoader.LoadPage))]
	public static bool Prefix(ref PageContentLoader __instance, RuleBookPageInfo pageInfo)
	{
		if (GrimoraSaveUtil.IsNotGrimoraModRun || pageInfo.abilityPage)
		{
			return true;
		}

		if (__instance.currentPagePrefab != pageInfo.pagePrefab)
		{
			if (__instance.currentPageObj)
			{
				UnityObject.Destroy(__instance.currentPageObj);
			}

			__instance.currentPageObj = UnityObject.Instantiate(
				pageInfo.pagePrefab,
				__instance.transform.position,
				__instance.transform.rotation,
				__instance.transform
			);
		}


		foreach (GameObject currentAdditiveObject in __instance.currentAdditiveObjects)
		{
			UnityObject.Destroy(currentAdditiveObject);
		}

		__instance.currentAdditiveObjects.Clear();
		foreach (var additivePrefab in pageInfo.additivePrefabs.Where(additivePrefab => additivePrefab))
		{
			GameObject gameObject = UnityObject.Instantiate(
				additivePrefab,
				__instance.transform.position,
				__instance.transform.rotation,
				__instance.transform
			);
			gameObject.SetActive(true);
			__instance.currentAdditiveObjects.Add(gameObject);
		}

		AbilityPage component = __instance.currentPageObj.GetComponent<AbilityPage>();
		component.name = "Grimora_StatIconRulebookPage";
		StatIconPage statIconPage = component.gameObject.AddComponent<StatIconPage>();
		statIconPage.descriptionTextMesh = component.mainAbilityGroup.descriptionTextMesh;
		statIconPage.iconRenderer = component.mainAbilityGroup.iconRenderer;
		statIconPage.nameTextMesh = component.mainAbilityGroup.nameTextMesh;
		statIconPage.headerTextMesh = component.headerTextMesh;
		UnityObject.Destroy(statIconPage.GetComponent<AbilityPage>());
		__instance.currentPagePrefab = statIconPage.gameObject;
		statIconPage.FillPage(pageInfo.headerText, pageInfo.pageId);
		return false;
	}
}
