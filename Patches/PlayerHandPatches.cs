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

		if (SaveFile.IsAscension && AscensionSaveData.Data.ChallengeIsActive(ChallengeManagement.RoyalsRevenge))
		{
			if (BoardManager.Instance.PlayerCardsPlayedThisRound.Count == 2)
			{
				yield return TextDisplayer.Instance.ShowUntilInput("Careful, the life of your next card will be on a timer.");
			}
			else if (BoardManager.Instance.PlayerCardsPlayedThisRound.Count % 3 == 0)
			{
				yield return TextDisplayer.Instance.ShowUntilInput("I look forward to the explosive results!");
				ViewManager.Instance.SwitchToView(View.Board);
				yield return new WaitForSeconds(0.2f);
				card.AddTemporaryMod(new CardModificationInfo(LitFuse.ability));
				card.Anim.StrongNegationEffect();
				yield return new WaitForSeconds(0.2f);
			}
		}
	}

	[HarmonyPostfix, HarmonyPatch(nameof(PlayerHand.AddCardToHand))]
	public static void RerenderCard(ref PlayableCard card, Vector3 spawnOffset, float onDrawnTriggerDelay)
	{
		if (card.InfoName() == GrimoraPlugin.NameSpectrabbit)
		{
			card.RenderCard();
		}
	}
}
