using System.Collections;
using DiskCardGame;
using HarmonyLib;
using UnityEngine;

namespace GrimoraMod;

[HarmonyPatch(typeof(Loot))]
public class LootPatch
{
	[HarmonyPostfix, HarmonyPatch(nameof(Loot.OnDealDamageDirectly))]
	public static IEnumerator FixVisualDraw(IEnumerator enumerator, Loot __instance, int amount)
	{
		yield return __instance.PreSuccessfulTriggerSequence();
		bool drawPile3DIsActive = CardDrawPiles3D.Instance && CardDrawPiles3D.Instance.pile;
		if (amount > 0)
		{
			yield return new WaitForSeconds(0.75f);
			ViewManager.Instance.SwitchToView(View.CardPiles, lockAfter: true);
			yield return new WaitForSeconds(0.5f);
		}
		for (int i = 0; i < amount; i++)
		{
			if (drawPile3DIsActive)
			{
				CardDrawPiles3D.Instance.pile.Draw();
			}
			yield return CardDrawPiles.Instance.DrawCardFromDeck();
			yield return new WaitForSeconds(0.2f);
		}

		yield return new WaitForSeconds(0.5f);
		ViewManager.Instance.SwitchToView(View.Default);
		ViewManager.Instance.SetViewUnlocked();
		yield return __instance.LearnAbility();
	}
}
