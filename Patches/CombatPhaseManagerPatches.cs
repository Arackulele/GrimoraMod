using System.Collections;
using DiskCardGame;
using HarmonyLib;
using InscryptionAPI.Card;
using InscryptionAPI.Helpers.Extensions;
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
		if (GrimoraSaveUtil.isNotGrimora)
		{
			yield return enumerator;
			yield break;
		}

		Animator customArmPrefab = slot.Card.Anim.GetCustomArm();
		bool cardIsGrimoraGiant = slot.Card.HasTrait(Trait.Giant);
		if (cardIsGrimoraGiant)
		{
			PlayableCard giantCard = slot.Card;
			List<CardSlot> opposingSlots = giantCard.GetOpposingSlots();
			ViewManager.Instance.SwitchToView(BoardManager.Instance.CombatView, lockAfter: true);

			foreach (var opposingSlot in opposingSlots)
			{
				ViewManager.Instance.SwitchToView(BoardManager.Instance.CombatView);
				yield return GlobalTriggerHandler.Instance.TriggerCardsOnBoard(
					Trigger.SlotTargetedForAttack,
					false,
					opposingSlot,
					giantCard
				);

				yield return new WaitForSeconds(0.025f);

				if (giantCard.IsNull() || giantCard.Dead)
				{
					Log.LogWarning($"[SlotAttackSequence.Giant] Giant has died/is dying, breaking out of loop.");
					yield break;
				}

				if (giantCard.Anim.DoingAttackAnimation)
				{
					yield return new WaitUntil(() => !giantCard.Anim.DoingAttackAnimation);
					yield return new WaitForSeconds(0.25f);
				}

				if (opposingSlot.Card)
				{
					Log.LogWarning($"[SlotAttackSequence.Giant] Giant is now targeting card {opposingSlot.Card.GetNameAndSlot()}, playing with impact keyframes, is doing attack anim? [{giantCard.Anim.DoingAttackAnimation}]");
					bool impactFrameReached = false;
					giantCard.Anim.PlayAttackAnimation(giantCard.IsFlyingAttackingReach(), opposingSlot, delegate { impactFrameReached = true; });

					yield return new WaitForSeconds(0.07f);
					customArmPrefab.speed = 0f;
					PlayableCard giantCopy = slot.Card;
					yield return GlobalTriggerHandler.Instance.TriggerCardsOnBoard(
						Trigger.CardGettingAttacked,
						false,
						opposingSlot.Card
					);
					if (giantCopy && giantCopy.Slot)
					{
						customArmPrefab.speed = 1f;
						yield return new WaitForSeconds(0.05f);

						yield return new WaitUntil(() => impactFrameReached);
						yield return opposingSlot.Card.TakeDamage(giantCard.Attack, giantCard);
					}

					Log.LogInfo($"[SlotAttackSequence.Giant] --> Finished custom SlotAttackSlot, is doing attack anim? [{giantCard.Anim.DoingAttackAnimation}]");
				}
				else
				{
					__instance.DamageDealtThisPhase += giantCard.Attack;
					yield return __instance.VisualizeCardAttackingDirectly(slot, opposingSlot, giantCard.Attack);
				}

				yield return new WaitForSeconds(0.1f);
			}

			if (giantCard.NotDead())
			{
				if (giantCard.Anim.DoingAttackAnimation)
				{
					Log.LogWarning($"[SlotAttackSequence.Giant] Giant is still doing attack anim, waiting until finished");
					yield return new WaitUntil(() => !giantCard.Anim.DoingAttackAnimation);
					yield return new WaitForSeconds(0.25f);
				}

				customArmPrefab.gameObject.SetActive(false);
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
					yield return (__instance as CombatPhaseManager3D).VisualizeDamageMovingToScales(slot.IsOpponentSlot());
					(__instance as CombatPhaseManager3D).damageWeights.Clear();
				}
			}

			if (slot.Card.NotDead())
			{
				Log.LogWarning($"[SlotAttackSequence.Regular] Card is still doing attack anim, waiting until finished");
				yield return new WaitUntil(() => !slot.Card.Anim.DoingAttackAnimation);
				customArmPrefab.gameObject.SetActive(false);
			}
		}
	}
}
