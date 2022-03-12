using System.Collections;
using DiskCardGame;
using HarmonyLib;
using UnityEngine;
using static GrimoraMod.GrimoraPlugin;

namespace GrimoraMod;

[HarmonyPatch(typeof(CombatPhaseManager))]
public class CombatPhaseManagerPatches
{
	[HarmonyPostfix, HarmonyPatch(nameof(CombatPhaseManager.SlotAttackSequence))]
	public static IEnumerator MinusDamageDealtThisPhase(
		IEnumerator enumerator,
		CombatPhaseManager __instance,
		CardSlot slot
	)
	{
		yield return enumerator;

		bool cardHasStrikeAdjacentAbility = slot.CardIsNotNullAndHasAbility(AreaOfEffectStrike.ability)
		                                    || slot.CardIsNotNullAndHasAbility(Raider.ability);
		if (cardHasStrikeAdjacentAbility)
		{
			yield return new WaitForSeconds(1f);
			int dmgDoneToPlayer = slot.Card.GetComponent<StrikeAdjacentSlots>().damageDoneToPlayer;
			if(dmgDoneToPlayer > 0)
			{
				Log.LogDebug($"[SlotAttackSequence.StrikeAdj] Dealing [{dmgDoneToPlayer}] to player");
				yield return LifeManager.Instance.ShowDamageSequence(
					dmgDoneToPlayer,
					dmgDoneToPlayer,
					slot.IsPlayerSlot,
					0.2f
				);

				Log.LogDebug($"[SlotAttackSequence.StrikeAdj] Subtracting [{dmgDoneToPlayer}] from DamageDealtThisPhase");
				__instance.DamageDealtThisPhase -= dmgDoneToPlayer;
				yield return (__instance as CombatPhaseManager3D).VisualizeDamageMovingToScales(!slot.IsPlayerSlot);
				(__instance as CombatPhaseManager3D).damageWeights.Clear();
			}
		}
	}
}
