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

		bool cardHasStrikeAdjacentAbility = slot.CardHasAbility(AreaOfEffectStrike.ability)
		                                    || slot.CardHasAbility(Raider.ability);
		if (cardHasStrikeAdjacentAbility)
		{
			yield return new WaitForSeconds(1f);
			int dmgDoneToPlayer = slot.Card.GetComponent<StrikeAdjacentSlots>().damageDoneToPlayer;
			Log.LogDebug($"[SlotAttackSequence.StrikeAdj] Dealing [{dmgDoneToPlayer}] to player");
			yield return LifeManager.Instance.ShowDamageSequence(
				dmgDoneToPlayer,
				dmgDoneToPlayer,
				!slot.Card.OpponentCard,
				0.2f
			);

			Log.LogDebug($"[SlotAttackSequence.StrikeAdj] Subtracting [{dmgDoneToPlayer}] from DamageDealtThisPhase");
			__instance.DamageDealtThisPhase -= dmgDoneToPlayer;
		}
	}
}
