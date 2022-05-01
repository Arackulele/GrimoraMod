using System.Collections;
using DiskCardGame;
using HarmonyLib;
using Sirenix.Utilities;
using UnityEngine;

namespace GrimoraMod;

[HarmonyPatch(typeof(ActivatedHeal))]
public class ActivatedHealPatch
{
	[HarmonyPostfix, HarmonyPatch(nameof(ActivatedHeal.Activate))]
	public static IEnumerator FixLogicForHealing(IEnumerator enumerator, ActivatedHeal __instance)
	{
		if (CardCanActivateHeal(__instance.Card))
		{
			__instance.Card.HealDamage(__instance.Card.MaxHealth - __instance.Card.Health);
			yield return new WaitForSeconds(0.25f);
		}
	}

	public static bool CardCanActivateHeal(PlayableCard playableCard)
	{
		return !playableCard.SafeIsUnityNull() && playableCard.MaxHealth > playableCard.Health;
	}
}

[HarmonyPatch(typeof(ActivatedAbilityBehaviour))]
public class FixingActivatedHeal
{
	[HarmonyPostfix, HarmonyPatch(nameof(ActivatedAbilityBehaviour.CanActivate))]
	public static void DontSpendBonesForActivatedHeal(ActivatedAbilityBehaviour __instance, ref bool __result)
	{
		if (__instance.Ability == Ability.ActivatedHeal)
		{
			__result = ActivatedHealPatch.CardCanActivateHeal(__instance.Card);
		}
	}
}
