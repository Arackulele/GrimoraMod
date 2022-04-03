using System.Collections;
using DiskCardGame;
using HarmonyLib;

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
	}
}
