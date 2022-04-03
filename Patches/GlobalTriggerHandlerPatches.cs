using System.Collections;
using DiskCardGame;
using HarmonyLib;
using UnityEngine;

namespace GrimoraMod;

[HarmonyPatch(typeof(GlobalTriggerHandler))]
public class GlobalTriggerHandlerPatches
{

	[HarmonyPostfix, HarmonyPatch(nameof(GlobalTriggerHandler.TriggerCardsInHand))]
	private static void UpdateCardStatsA()
	{
		PlayableCardPatches.UpdateAllCards();
	}

	[HarmonyPostfix, HarmonyPatch(nameof(GlobalTriggerHandler.TriggerCardsOnBoard))]
	public static IEnumerator AdjustForSkinCrawler(IEnumerator enumerator, Trigger trigger, bool triggerFacedown, params object[] otherArgs)
	{
		yield return enumerator;
		PlayableCardPatches.UpdateAllCards();
		if(SkinCrawler.DoCreateAfterGlobalHandlerFinishes != null)
		{
			GrimoraPlugin.Log.LogDebug($"There is skin crawlers, invoking static action");
			yield return new WaitForSeconds(0.1f);
			SkinCrawler.DoCreateAfterGlobalHandlerFinishes?.Invoke();
		}
		SkinCrawler.DoCreateAfterGlobalHandlerFinishes = null;
	}
}

[HarmonyPatch(typeof(CardTriggerHandler))]
class CardTriggerHandlerPatches
{
	// [HarmonyPrefix, HarmonyPatch(nameof(CardTriggerHandler.RespondsToTrigger))]
	// public static void BeforeTriggers(CardTriggerHandler __instance)
	// {
	// 	__instance.StartCoroutine(GlobalTriggerHandlerPatches.CallUpdateStatsForAllCards());
	// }
	//
	// [HarmonyPostfix, HarmonyPatch(nameof(CardTriggerHandler.OnTrigger))]
	// public static IEnumerator AddTriggers(IEnumerator enumerator)
	// {
	// 	yield return enumerator;
	// 	yield return GlobalTriggerHandlerPatches.CallUpdateStatsForAllCards();
	// }
}
