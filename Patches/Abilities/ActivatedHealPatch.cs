using System.Collections;
using DiskCardGame;
using HarmonyLib;
using UnityEngine;

namespace GrimoraMod;

[HarmonyPatch(typeof(ActivatedHeal))]
public class ActivatedHealPatch
{
	[HarmonyPostfix, HarmonyPatch(nameof(ActivatedHeal.Activate))]
	public static IEnumerator FixLogicForHealing(IEnumerator enumerator, ActivatedHeal __instance)
	{
		int maxHealth = __instance.Card.MaxHealth;
		int currentHealth = __instance.Card.Health;
		if (__instance.Card && maxHealth > currentHealth)
		{
			__instance.Card.HealDamage(maxHealth - currentHealth);
			yield return new WaitForSeconds(0.25f);
		}
	}
}
