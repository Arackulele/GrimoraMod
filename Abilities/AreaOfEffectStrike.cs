﻿using System.Collections;
using APIPlugin;
using DiskCardGame;
using HarmonyLib;
using UnityEngine;
using static GrimoraMod.GrimoraPlugin;

namespace GrimoraMod;

public class AreaOfEffectStrike : AbilityBehaviour
{
	public static Ability ability;
	public override Ability Ability => ability;

	public int damageDoneToPlayer = 0;

	public override bool RespondsToSlotTargetedForAttack(CardSlot slot, PlayableCard attacker)
	{
		// check if the attacking card is this card
		if (attacker is not null && attacker.Slot == Card.Slot && slot.Card is null)
		{
			if (attacker.Slot.IsPlayerSlot)
			{
				// if the attacker slot is the player, return if the targeted slot is also the player slot 
				return slot.IsPlayerSlot;
			}

			// check if slot being attacked is the opponent slot if the attacking slot is the opponent
			return !slot.IsPlayerSlot;
		}

		return false;
	}

	public override IEnumerator OnSlotTargetedForAttack(CardSlot slot, PlayableCard attacker)
	{
		damageDoneToPlayer += Card.Attack;
		yield break;
	}

	public override bool RespondsToUpkeep(bool playerUpkeep)
	{
		return damageDoneToPlayer > 0;
	}

	public override IEnumerator OnUpkeep(bool playerUpkeep)
	{
		damageDoneToPlayer = 0;
		yield break;
	}

	public static NewAbility Create()
	{
		const string rulebookDescription =
			"[creature] will strike it's adjacent slots, and each opposing space to the left, right, and center of it.";

		return ApiUtils.CreateAbility<AreaOfEffectStrike>(rulebookDescription, flipYIfOpponent: true);
	}
}

/// <summary>
/// This patch is so that any card containing this sigil will act like if it has TriStrike.
/// This is so that extra logic is not necessary to add which slots it targets and so on.
/// We just have to say it has TriStrike and the game does the rest
/// </summary>
[HarmonyPatch]
public class PatchesForAreaOfEffectStrike
{
	[HarmonyPostfix, HarmonyPatch(typeof(CombatPhaseManager), nameof(CombatPhaseManager.SlotAttackSequence))]
	public static IEnumerator MinusDamageDealtThisPhase(
		IEnumerator enumerator,
		CombatPhaseManager __instance,
		CardSlot slot
	)
	{
		yield return enumerator;

		if (slot.Card is not null && slot.Card.HasAbility(AreaOfEffectStrike.ability))
		{
			yield return new WaitForSeconds(1f);
			int dmgDoneToPlayer = slot.Card.GetComponent<AreaOfEffectStrike>().damageDoneToPlayer;
			Log.LogDebug($"[SlotAttackSequence.AOE] Dealing [{dmgDoneToPlayer}] to player");
			yield return LifeManager.Instance.ShowDamageSequence(
				dmgDoneToPlayer,
				dmgDoneToPlayer,
				!slot.Card.OpponentCard,
				0.2f
			);

			Log.LogDebug($"[SlotAttackSequence.AOE] Subtracting [{dmgDoneToPlayer}] from DamageDealtThisPhase");
			__instance.DamageDealtThisPhase -= dmgDoneToPlayer;
		}
	}
}
