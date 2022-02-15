﻿using System.Collections;
using DiskCardGame;
using HarmonyLib;
using UnityEngine;

namespace GrimoraMod;

[HarmonyPatch(typeof(DrawNewHand))]
public class DrawNewHandPatch
{
	
	[HarmonyPostfix, HarmonyPatch(nameof(DrawNewHand.OnResolveOnBoard))]
	public static IEnumerator PostfixChangeViewAndCorrectVisuals(IEnumerator enumerator, DrawNewHand __instance)
	{
		if (GrimoraSaveUtil.isNotGrimora)
		{
			yield return enumerator;
			yield break;
		}
		
		ViewManager.Instance.SwitchToView(View.Hand);
		yield return __instance.PreSuccessfulTriggerSequence();
		yield return new WaitForSeconds(0.25f);
		List<PlayableCard> cardsNotChoosingASlot 
			= PlayerHand.Instance.CardsInHand.FindAll(x => x != PlayerHand.Instance.ChoosingSlotCard);
		while (cardsNotChoosingASlot.Count > 0)
		{
			cardsNotChoosingASlot[0].SetInteractionEnabled(false);
			cardsNotChoosingASlot[0].Anim.PlayDeathAnimation();
			Object.Destroy(cardsNotChoosingASlot[0].gameObject, 1f);
			PlayerHand.Instance.RemoveCardFromHand(cardsNotChoosingASlot[0]);
			cardsNotChoosingASlot.RemoveAt(0);
		}
		yield return new WaitForSeconds(0.5f);
		for (int i = 0; i < 4; i++)
		{
			CardDrawPiles3D.Instance.pile.Draw();
			yield return CardDrawPiles.Instance.DrawCardFromDeck();
		}
		yield return __instance.LearnAbility(0.5f);
	}
}