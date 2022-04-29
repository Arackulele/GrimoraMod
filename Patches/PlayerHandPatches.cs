using System.Collections;
using DiskCardGame;
using HarmonyLib;
using UnityEngine;

namespace GrimoraMod;

[HarmonyPatch(typeof(PlayerHand))]
public class PlayerHandPatches
{
	[HarmonyPostfix, HarmonyPatch(nameof(PlayerHand.PlayCardOnSlot))]
	public static IEnumerator HandleMarchingDeadLogic(
		IEnumerator enumerator,
		PlayerHand __instance,
		PlayableCard card,
		CardSlot slot
	)
	{
		if (GrimoraSaveUtil.IsGrimora && __instance.CardsInHand.Contains(card) && card.HasAbility(MarchingDead.ability))
		{
			card.GetComponent<MarchingDead>().SetAdjCardsToPlay(__instance.CardsInHand);
		}

		yield return enumerator;
	}
	
	[HarmonyPostfix, HarmonyPatch(nameof(PlayerHand.AddCardToHand))]
	public static void RerenderCard(ref PlayableCard card, Vector3 spawnOffset, float onDrawnTriggerDelay)
	{
		card.RenderCard();
	}
}
