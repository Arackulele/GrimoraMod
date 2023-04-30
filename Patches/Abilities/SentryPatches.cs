using System.Collections;
using DiskCardGame;
using HarmonyLib;
using InscryptionAPI.Card;
using UnityEngine;

namespace GrimoraMod;

[HarmonyPatch(typeof(Sentry))]
public class SentryPatches
{
	[HarmonyPostfix, HarmonyPatch(nameof(Sentry.FireAtOpposingSlot))]
	public static IEnumerator PlayShootingAnim(IEnumerator enumerator, Sentry __instance, PlayableCard otherCard)
	{

		GrimoraPlugin.Log.LogDebug($"Trying sentry animation patch.");

		if (otherCard == __instance.lastShotCard && TurnManager.Instance.TurnNumber == __instance.lastShotTurn)
		{
			yield break;
		}

		__instance.lastShotCard = otherCard;
		__instance.lastShotTurn = TurnManager.Instance.TurnNumber;
		ViewManager.Instance.SwitchToView(View.Board, false, true);
		yield return new WaitForSeconds(0.25f);
		for (int i = 0; i < __instance.NumShots; i++)
		{


			if (otherCard.NotDead())
			{
				yield return __instance.PreSuccessfulTriggerSequence();
				__instance.Card.Anim.LightNegationEffect();
				bool impactFrameReached = false;
				if (__instance.Card.Anim is DiskCardAnimationController diskController)
				{
					diskController.SetWeaponMesh(DiskCardWeapon.Turret);
					diskController.AimWeaponAnim(otherCard.Slot.transform.position);
					diskController.ShowWeaponAnim();
					yield return new WaitForSeconds(0.5f);
					__instance.Card.Anim.PlayAttackAnimation(
						__instance.Card.IsFlyingAttackingReach(),
						otherCard.Slot,
						delegate
						{
							impactFrameReached = true;
						}
					);
				}
				else if (__instance.Card.Anim is GraveControllerExt graveController)
				{
					GrimoraPlugin.Log.LogDebug($"Playing shoot animation! Instance is [{__instance.Card}]");
					graveController.PlaySpecificAttackAnimation(
						"attack_sentry",
						__instance.Card.IsFlyingAttackingReach(),
						otherCard.Slot,
						delegate { impactFrameReached = true; }
					);
				}

				yield return new WaitUntil(() => impactFrameReached);
				yield return otherCard.TakeDamage(1, __instance.Card);
			}
		}

		yield return __instance.LearnAbility(0.5f);
		ViewManager.Instance.Controller.LockState = ViewLockState.Unlocked;
	}
}
