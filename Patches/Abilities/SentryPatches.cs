using System.Collections;
using DiskCardGame;
using HarmonyLib;
using UnityEngine;

namespace GrimoraMod;

[HarmonyPatch(typeof(Sentry))]
public class SentryPatches
{
	[HarmonyPostfix, HarmonyPatch(nameof(Sentry.Awake))]
	public static void AddNewSkeletonArm(Sentry __instance)
	{
		if (__instance.Card.Anim is GravestoneCardAnimationController)
		{
			GrimoraPlugin.Log.LogDebug($"Adding skeleton arm shoot object to card [{__instance.Card.InfoName()}]");
			Animator skeletonArmShoot = Object.Instantiate(
					AssetUtils.GetPrefab<GameObject>("SkeletonArmAttack_Sentry"),
					__instance.transform
				)
				.AddComponent<Animator>();
			skeletonArmShoot.name = "SkeletonArms_Sentry";
			skeletonArmShoot.runtimeAnimatorController = AssetConstants.SkeletonArmController;
			skeletonArmShoot.gameObject.AddComponent<AnimMethods>();
			skeletonArmShoot.gameObject.SetActive(false);
			
			if (__instance.GetComponent<AnimMethods>().IsNull())
			{
				GrimoraPlugin.Log.LogDebug($"Adding AnimMethods component to [{__instance.Card.GetNameAndSlot()}]");
				__instance.gameObject.AddComponent<AnimMethods>();
			}
		}
	}

	[HarmonyPostfix, HarmonyPatch(nameof(Sentry.FireAtOpposingSlot))]
	public static IEnumerator PlayShootingAnim(IEnumerator enumerator, Sentry __instance, PlayableCard otherCard)
	{
		if (otherCard == __instance.lastShotCard && TurnManager.Instance.TurnNumber == __instance.lastShotTurn)
		{
			yield break;
		}

		__instance.lastShotCard = otherCard;
		__instance.lastShotTurn = Singleton<TurnManager>.Instance.TurnNumber;
		ViewManager.Instance.SwitchToView(View.Board, false, true);
		yield return new WaitForSeconds(0.25f);
		for (int i = 0; i < __instance.NumShots; i++)
		{
			if (otherCard != null && !otherCard.Dead)
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
				else if (__instance.Card.Anim is GravestoneCardAnimationController graveController)
				{
					GrimoraPlugin.Log.LogDebug($"Playing shoot animation!");
					graveController.PlayAttackAnimation(
						__instance.Card.IsFlyingAttackingReach(),
						null,
						delegate
						{
							impactFrameReached = true;
						}
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
