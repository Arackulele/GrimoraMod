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
	public static void AdjustForSkinCrawler(Trigger trigger, bool triggerFacedown, params object[] otherArgs)
	{
		PlayableCardPatches.UpdateAllCards();
	}
}
