using System.Collections;
using DiskCardGame;
using HarmonyLib;

namespace GrimoraMod;

[HarmonyPatch(typeof(Loot))]
public class LootPatch
{
	[HarmonyPrefix, HarmonyPatch(nameof(Loot.OnDealDamageDirectly))]
	public static IEnumerator FixVisualDraw(Loot __instance, int amount)
	{
		yield return __instance.PreSuccessfulTriggerSequence();
		bool drawPile3DIsActive = CardDrawPiles3D.Instance && CardDrawPiles3D.Instance.pile;
		for (int i = 0; i < amount; i++)
		{
			if (drawPile3DIsActive)
			{
				CardDrawPiles3D.Instance.pile.Draw();
			}
			yield return CardDrawPiles.Instance.DrawCardFromDeck();
		}
		yield return __instance.LearnAbility();
	}
}
