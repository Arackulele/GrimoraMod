using DiskCardGame;
using HarmonyLib;
using InscryptionAPI.Card;
using Sirenix.Utilities;

namespace GrimoraMod;

[HarmonyPatch(typeof(Deck))]
public class DeckPatches
{
	[HarmonyPostfix, HarmonyPatch(nameof(Deck.Draw), typeof(CardInfo))]
	public static void AddOnePowerToLastSkeletonInSideDeck(Deck __instance, ref CardInfo __result)
	{
		if (GrimoraSaveUtil.IsNotGrimoraModRun || __instance.CardsInDeck > 0 || __result.SafeIsUnityNull() || __result.name != "Skeleton")
		{
			return;
		}


		GrimoraPlugin.Log.LogWarning($"[Deck.Draw] Giving last skeleton 2 attack");
		__result = ((CardInfo)__result.Clone()).SetBaseAttackAndHealth(2, 1);
	}
}
