﻿using System.Collections;
using DiskCardGame;
using HarmonyLib;
using UnityEngine;
using static GrimoraMod.GrimoraPlugin;

namespace GrimoraMod;

[HarmonyPatch(typeof(CombatPhaseManager))]
public class CombatPhaseManagerPatches
{
	[HarmonyPostfix, HarmonyPatch(nameof(CombatPhaseManager.SlotAttackSequence))]
	public static IEnumerator HandleSpecificAttacksForCustomAnims(
		IEnumerator enumerator,
		CombatPhaseManager __instance,
		CardSlot slot
	)
	{
		bool cardIsGrimoraGiant = slot.Card.HasSpecialAbility(GrimoraGiant.FullSpecial.Id);
		if (cardIsGrimoraGiant)
		{
			PlayableCard giantCard = slot.Card;
			List<CardSlot> opposingSlots = giantCard.GetOpposingSlots();
			ViewManager.Instance.SwitchToView(BoardManager.Instance.CombatView);
			ViewManager.Instance.Controller.LockState = ViewLockState.Locked;

			foreach (var opposingSlot in opposingSlots)
			{
				ViewManager.Instance.SwitchToView(BoardManager.Instance.CombatView);
				if (opposingSlot.Card)
				{
					yield return GlobalTriggerHandler.Instance.TriggerCardsOnBoard(
						Trigger.SlotTargetedForAttack,
						false,
						opposingSlot,
						giantCard
					);
					
					yield return new WaitForSeconds(0.025f);
					
					if (giantCard.Anim.DoingAttackAnimation)
					{
						yield return new WaitUntil(() => !giantCard.Anim.DoingAttackAnimation);
						yield return new WaitForSeconds(0.25f);
					}
					
					bool impactFrameReached = false;
					giantCard.Anim.PlayAttackAnimation(giantCard.IsFlyingAttackingReach(), opposingSlot, delegate { impactFrameReached = true; });

					yield return new WaitForSeconds(0.07f);
					giantCard.Anim.SetAnimationPaused(true);
					yield return GlobalTriggerHandler.Instance.TriggerCardsOnBoard(
						Trigger.CardGettingAttacked,
						false,
						opposingSlot.Card
					);
					giantCard.Anim.SetAnimationPaused(false);
					yield return new WaitForSeconds(0.05f);

					Log.LogInfo($"[{giantCard.Info.displayedName}] Waiting until keyframe has been reached");
					yield return new WaitUntil(() => impactFrameReached);
					Log.LogInfo($"[{giantCard.Info.displayedName}] Keyframe reached");
					yield return opposingSlot.Card.TakeDamage(giantCard.Attack, giantCard);
				}
				else
				{
					yield return __instance.SlotAttackSlot(slot, opposingSlot, opposingSlots.Count > 1 ? 0.1f : 0f);
				}

				yield return new WaitForSeconds(0.1f);
			}
		}
		else
		{
			yield return enumerator;

			bool cardHasStrikeAdjacentAbility = slot.Card && slot.Card.GetComponent<StrikeAdjacentSlots>();
			if (cardHasStrikeAdjacentAbility)
			{
				yield return new WaitForSeconds(1f);
				int dmgDoneToPlayer = slot.Card.GetComponent<StrikeAdjacentSlots>().damageDoneToPlayer;
				if (dmgDoneToPlayer > 0)
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
}
