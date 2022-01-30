using System.Collections;
using APIPlugin;
using DiskCardGame;
using HarmonyLib;
using Sirenix.Utilities;
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
		return attacker.Slot == base.Card.Slot && slot.IsPlayerSlot && slot.Card is null;
	}

	public override IEnumerator OnSlotTargetedForAttack(CardSlot slot, PlayableCard attacker)
	{
		damageDoneToPlayer += base.Card.Attack;
		yield break;
	}

	public override bool RespondsToUpkeep(bool playerUpkeep)
	{
		return playerUpkeep && damageDoneToPlayer > 0;
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

		return ApiUtils.CreateAbility<AreaOfEffectStrike>(rulebookDescription);
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

	[HarmonyPostfix, HarmonyPatch(typeof(PlayableCard), nameof(PlayableCard.GetOpposingSlots))]
	public static void AreaOfEffectStrikeGetOpposingSlotsPatch(PlayableCard __instance, ref List<CardSlot> __result)
	{
		if (__instance.HasAbility(AreaOfEffectStrike.ability))
		{
			// Log.LogDebug($"[GetOpposingSlotsPatch] Adding adj slots from [{__instance.Slot.Index}]");
			var toLeftSlot = BoardManager.Instance.GetAdjacent(__instance.Slot, true);
			var toRightSlot = BoardManager.Instance.GetAdjacent(__instance.Slot, false);
			
			// insert at beginning
			if(toLeftSlot is not null)
			{
				__result.Insert(0, toLeftSlot);
			}
			// insert at end
			if(toRightSlot is not null)
			{
				__result.Insert(__result.Count, toRightSlot);
			}
		}
	}

	[HarmonyPostfix, HarmonyPatch(typeof(PlayableCard), nameof(PlayableCard.HasTriStrike))]
	public static void PlayableCardHasTriStrikePatches(PlayableCard __instance, ref bool __result)
	{
		// Log.LogDebug($"Setting player is attacker to [{playerIsAttacker}] Board is [{board}]");
		if (__instance.HasAbility(AreaOfEffectStrike.ability))
		{
			// Log.LogDebug($"[PlayableCardHasTriStrikePatches] Has area of effect strike ability");
			__result = true;
		}
	}

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
			int dmgDoneToPlayer = slot.Card.GetComponent<AreaOfEffectStrike>().damageDoneToPlayer;
			Log.LogDebug($"[SlotAttackSequence] Dealing [{dmgDoneToPlayer}] to player");
			yield return LifeManager.Instance.ShowDamageSequence(
				dmgDoneToPlayer, dmgDoneToPlayer, true, 0.2f
			);
			
			__instance.DamageDealtThisPhase -= dmgDoneToPlayer;
		}
	}
}
