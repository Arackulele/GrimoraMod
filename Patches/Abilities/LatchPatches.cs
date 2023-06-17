using System.Collections;
using DiskCardGame;
using HarmonyLib;
using InscryptionAPI.Card;
using Pixelplacement;
using Sirenix.Utilities;
using UnityEngine;

namespace GrimoraMod;

[HarmonyPatch(typeof(Latch))]
public class LatchPatches
{
	private static List<CardSlot> GetValidTargets(Latch latch)
	{

		GrimoraPlugin.Log.LogDebug("Getting valid Targets for Latch");

		List<CardSlot> validTargets = BoardManager.Instance.AllSlotsCopy;
		validTargets.RemoveAll(
			x => x.Card.SafeIsUnityNull() 
			  || x.Card.Dead 
			  || latch.CardHasLatchMod(x.Card) 
			  || x.Card == latch.Card
				|| x.Card.Info.HasTrait(Trait.Uncuttable)
				|| x.Card.AllAbilities().Count > 4
		);

		return validTargets;
	}

	[HarmonyPostfix, HarmonyPatch(nameof(Latch.OnPreDeathAnimation))]
	public static IEnumerator PostfixChangeLogicForGrimora(IEnumerator enumerator, Latch __instance, bool wasSacrifice)
	{
		if (GrimoraSaveUtil.IsNotGrimoraModRun)
		{
			yield return enumerator;
			yield break;
		}

		List<CardSlot> validTargets = GetValidTargets(__instance);
		if (validTargets.Count <= 0)
		{
			yield break;
		}

		__instance.Card.GetComponent<GraveControllerExt>().AddCustomArmPrefabs(__instance.Card);

		ViewManager.Instance.SwitchToView(View.Board);
		__instance.Card.Anim.PlayHitAnimation();
		yield return new WaitForSeconds(0.1f);

		GravestoneCardAnimationController cardAnim = __instance.Card.Anim as GravestoneCardAnimationController;
		cardAnim.armAnim.gameObject.SetActive(true);
		// GameObject claw = UnityObject.Instantiate(__instance.clawPrefab, cardAnim.WeaponParent.transform);
		CardSlot selectedSlot = null;
		if (__instance.Card.OpponentCard)
		{
			yield return new WaitForSeconds(0.3f);
			yield return __instance.AISelectTarget(
				validTargets,
				delegate(CardSlot s)
				{
					selectedSlot = s;
				}
			);
			if (selectedSlot && selectedSlot.Card)
			{
				AimWeaponAnim(cardAnim.armAnim.transform, selectedSlot.transform.position);
				yield return new WaitForSeconds(0.3f);
			}
		}
		else
		{
			List<CardSlot> allSlotsCopy = BoardManager.Instance.AllSlotsCopy;
			allSlotsCopy.Remove(__instance.Card.Slot);
			yield return BoardManager.Instance.ChooseTarget(
				allSlotsCopy,
				validTargets,
				delegate(CardSlot s)
				{
					selectedSlot = s;
				},
				__instance.OnInvalidTarget,
				delegate(CardSlot s)
				{
					if (s.Card)
					{
						AimWeaponAnim(cardAnim.armAnim.transform, s.transform.position);
					}
				},
				null,
				CursorType.Target
			);
		}

		CustomCoroutine.FlickerSequence(
			delegate
			{
				// claw.SetActive(true);
			},
			delegate
			{
				// claw.SetActive(false);
			},
			true,
			false,
			0.05f,
			2
		);
		if (selectedSlot && selectedSlot.Card)
		{
			yield return new WaitForSeconds(0.05f);
			CardModificationInfo cardModificationInfo = new CardModificationInfo
			{
				abilities = new List<Ability> { __instance.LatchAbility }
			};
			selectedSlot.Card.AddTemporaryMod(cardModificationInfo);
			__instance.OnSuccessfullyLatched(selectedSlot.Card);
			yield return new WaitForSeconds(0.75f);
			yield return __instance.LearnAbility();
		}

		cardAnim.armAnim.gameObject.SetActive(false);
	}

	public static void AimWeaponAnim(Transform armAnim, Vector3 target)
	{
		// Quaternion lookAt;
		// if (target.x > armAnim.parent.position.x)
		// {
		// 	// right
		// 	lookAt = Quaternion.Euler(0, 90, 270);
		// }
		// else
		// {
		// 	lookAt = Quaternion.Euler(0, 270, 90);
		// }
		//
		// Tween.LocalRotation(armAnim, lookAt, 0.075f, 0f, Tween.EaseInOut);
		Tween.LookAt(armAnim.transform, target, Vector3.up, 0.075f, 0f, Tween.EaseInOut);
	}
}
