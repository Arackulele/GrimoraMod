using System.Collections;
using DiskCardGame;
using HarmonyLib;
using Pixelplacement;
using UnityEngine;

namespace GrimoraMod;

[HarmonyPatch(typeof(Latch))]
public class LatchPatches
{
	[HarmonyPostfix, HarmonyPatch(nameof(Latch.OnPreDeathAnimation))]
	public static IEnumerator PostfixChangeLogicForGrimora(IEnumerator enumerator, Latch __instance, bool wasSacrifice)
	{
		if (GrimoraSaveUtil.isNotGrimora)
		{
			yield return enumerator;
			yield break;
		}

		List<CardSlot> validTargets = BoardManager.Instance.AllSlotsCopy;
		validTargets.RemoveAll(
			x => x.Card == null || x.Card.Dead || __instance.CardHasLatchMod(x.Card) || x.Card == __instance.Card
		);
		if (validTargets.Count <= 0)
		{
			yield break;
		}

		ViewManager.Instance.SwitchToView(View.Board);
		__instance.Card.Anim.PlayHitAnimation();
		yield return new WaitForSeconds(0.1f);

		GravestoneCardAnimationController cardAnim = __instance.Card.Anim as GravestoneCardAnimationController;
		cardAnim.armAnim.gameObject.SetActive(true);
		// GameObject claw = Object.Instantiate(__instance.clawPrefab, cardAnim.WeaponParent.transform);
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
			if (selectedSlot != null && selectedSlot.Card != null)
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
					if (s.Card != null)
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
		if (selectedSlot != null && selectedSlot.Card != null)
		{
			yield return new WaitForSeconds(0.05f);
			CardModificationInfo cardModificationInfo = new CardModificationInfo
			{
				abilities = new List<Ability> { Ability.Brittle }
			};
			GrimoraPlugin.Log.LogDebug($"Applying brittle");
			selectedSlot.Card.Info.Mods.Add(cardModificationInfo);
			selectedSlot.Card.Anim.PlayTransformAnimation();
			yield return new WaitForSeconds(0.05f);
			selectedSlot.Card.RenderCard();

			selectedSlot.Card.Info.Mods.Remove(cardModificationInfo);
			selectedSlot.Card.AddTemporaryMod(cardModificationInfo);
			yield return new WaitForSeconds(0.75f);
			yield return __instance.LearnAbility();
		}

		cardAnim.armAnim.gameObject.SetActive(false);
	}

	public static void AimWeaponAnim(Transform armAnim, Vector3 target)
	{
		Quaternion lookAt;
		if (target.x > armAnim.parent.position.x)
		{
			// right
			lookAt = Quaternion.Euler(0, 90, 270);
		}
		else
		{
			lookAt = Quaternion.Euler(0, 270, 90);
		}

		Tween.LocalRotation(armAnim, lookAt, 0.075f, 0f, Tween.EaseInOut);
	}
}
