using System.Collections;
using DiskCardGame;
using HarmonyLib;
using UnityEngine;

namespace GrimoraMod;

[HarmonyPatch(typeof(CardDrawPiles))]
public class CardDrawPilesPatches
{
	[HarmonyPostfix, HarmonyPatch(nameof(CardDrawPiles.ExhaustedSequence))]
	public static IEnumerator GrimoraExhaustedSequence(IEnumerator enumerator, CardDrawPiles __instance)
	{
		CardSlot bonelordSlot = BoardManager.Instance.OpponentSlotsCopy.Find(slot => slot.Card && slot.Card.InfoName() == GrimoraPlugin.NameBonelord);
		if (GrimoraSaveUtil.isNotGrimora || bonelordSlot.IsNull())
		{
			yield return enumerator;
			yield break;
		}

		ViewManager.Instance.SwitchToView(View.CardPiles, false, true);
		yield return new WaitForSeconds(1f);

		if (bonelordSlot)
		{
			if (__instance.turnsSinceExhausted == 0)
			{
				yield return __instance.ExhaustedSequence();
			}

			var opponentOpenSlots = BoardManager.Instance.GetOpponentOpenSlots();
			if (opponentOpenSlots.Any())
			{
				CardInfo starvation = "Starvation".GetCardInfo();
				CardModificationInfo modInfo = new CardModificationInfo(__instance.turnsSinceExhausted, __instance.turnsSinceExhausted);
				if (__instance.turnsSinceExhausted >= 4)
				{
					modInfo.abilities.Add(Ability.Flying);
				}
				
				yield return BoardManager.Instance.CreateCardInSlot(starvation, opponentOpenSlots.GetRandomItem(), 0.1f, false);
			}
			
			ViewManager.Instance.SwitchToView(View.Board, false, true);
			yield return new WaitForSeconds(0.25f);
			bonelordSlot.Card.AddTemporaryMod(new CardModificationInfo(1, 0));
			bonelordSlot.Card.Anim.StrongNegationEffect();
			yield return new WaitForSeconds(1f);
		}

		ViewManager.Instance.SwitchToView(View.Default);
		ViewManager.Instance.Controller.LockState = ViewLockState.Unlocked;
		__instance.turnsSinceExhausted++;
	}
}
