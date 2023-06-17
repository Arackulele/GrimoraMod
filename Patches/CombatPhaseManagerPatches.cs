using System.Collections;
using System.Diagnostics;
using DiskCardGame;
using HarmonyLib;
using InscryptionAPI.Card;
using InscryptionAPI.Helpers.Extensions;
using Sirenix.Utilities;
using UnityEngine;
using static GrimoraMod.GrimoraPlugin;

namespace GrimoraMod;

[HarmonyPatch(typeof(CombatPhaseManager))]
public class CombatPhaseManagerPatches
{
	private static readonly Stopwatch Stopwatch = new Stopwatch();

	[HarmonyPostfix, HarmonyPatch(nameof(CombatPhaseManager.SlotAttackSequence))]
	public static IEnumerator HandleSpecificAttacksForCustomAnims(
		IEnumerator enumerator,
		CombatPhaseManager __instance,
		CardSlot slot
	)
	{
		if (GrimoraSaveUtil.IsNotGrimoraModRun)
		{
			yield return enumerator;
			yield break;
		}

		if (Stopwatch.IsRunning)
		{
			Stopwatch.Stop();
		}
		Stopwatch.Start();
		Animator customArmPrefab = slot.Card.Anim.GetCustomArm();
		bool cardIsGrimoraGiant = slot.Card.IsGrimoraGiant();
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

				if (giantCard.SafeIsUnityNull() || giantCard.Dead)
				{
					Log.LogWarning($"[SlotAttackSequence.Giant] Giant has died/is dying, breaking out of loop.");
					yield break;
				}

				if (giantCard.Anim.DoingAttackAnimation)
				{
					yield return new WaitUntil(() => !giantCard.Anim.DoingAttackAnimation);
					yield return new WaitForSeconds(0.25f);
				}

				if (opposingSlot.Card && giantCard.AttackIsBlocked(opposingSlot))
				{
					yield return __instance.ShowCardBlocked(giantCard);
				}
				else if (giantCard.CanAttackDirectly(opposingSlot))
				{
					__instance.DamageDealtThisPhase += giantCard.Attack;
					yield return __instance.VisualizeCardAttackingDirectly(slot, opposingSlot, giantCard.Attack);
					if (giantCard.TriggerHandler.RespondsToTrigger(Trigger.DealDamageDirectly, giantCard.Attack))
					{
						yield return giantCard.TriggerHandler.OnTrigger(
							Trigger.DealDamageDirectly,
							giantCard.Attack
						);
					}
				}
				else
				{
					Log.LogInfo($"[SlotAttackSequence.Giant] Giant is now targeting card {opposingSlot.Card.GetNameAndSlot()}, playing with impact keyframes, is doing attack anim? [{giantCard.Anim.DoingAttackAnimation}]");
					bool impactFrameReached = false;
					giantCard.Anim.PlayAttackAnimation(
						giantCard.IsFlyingAttackingReach(),
						opposingSlot,
						delegate { impactFrameReached = true; }
					);

					yield return new WaitForSeconds(0.07f);
					customArmPrefab.speed = 0f;
					PlayableCard giantCopy = slot.Card;
					yield return GlobalTriggerHandler.Instance.TriggerCardsOnBoard(
						Trigger.CardGettingAttacked,
						false,
						opposingSlot.Card
					);
					if (giantCopy && giantCopy.Slot && opposingSlot.Card.NotDead())
					{
						customArmPrefab.speed = 1f;
						yield return new WaitForSeconds(0.05f);

						yield return new WaitUntil(() => impactFrameReached);
						yield return opposingSlot.Card.TakeDamage(giantCard.Attack, giantCard);
					}

					Log.LogInfo($"[SlotAttackSequence.Giant] --> Finished custom SlotAttackSlot, is doing attack anim? [{giantCard.Anim.DoingAttackAnimation}]");

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
					Log.LogInfo($"[SlotAttackSequence.StrikeAdj] Dealing [{dmgDoneToPlayer}] to player");
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
		}

		if (slot.Card.NotDead())
		{
			if (slot.Card.Anim.DoingAttackAnimation)
			{
				Log.LogInfo($"[SlotAttackSequence] [{slot.Card.GetNameAndSlot()}] is still doing attack anim, waiting until finished");
				yield return new WaitUntil(() => !slot.Card.Anim.DoingAttackAnimation);
			}

			Stopwatch.Stop();
			Log.LogInfo($"[SlotAttackSequence] [{slot.Card.GetNameAndSlot()}] no longer attacking. Time taken: [{Stopwatch.ElapsedMilliseconds}]ms");
			if (cardIsGrimoraGiant)
			{
				yield return new WaitForSeconds(0.25f);
			}

			if (customArmPrefab)
			{
				customArmPrefab.gameObject.SetActive(false);
			}
		}
	}
}
