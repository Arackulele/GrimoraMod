using System.Collections;
using DiskCardGame;
using HarmonyLib;

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
		if (GrimoraSaveUtil.isGrimora && __instance.CardsInHand.Contains(card) && card.HasAbility(MarchingDead.ability))
		{
			card.GetComponent<MarchingDead>().SetAdjCardsToPlay(__instance.CardsInHand);
		}

		yield return enumerator;
	}
}
