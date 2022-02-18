using System.Collections;
using DiskCardGame;
using HarmonyLib;
using UnityEngine;
using static GrimoraMod.GrimoraPlugin;

namespace GrimoraMod;

[HarmonyPatch(typeof(GravestoneCardAnimationController))]
public class GravestoneCardAnimationControllerPatches
{
	[HarmonyPatch(nameof(GravestoneCardAnimationController.PlayAttackAnimation))]
	public static bool Prefix(ref GravestoneCardAnimationController __instance, bool attackPlayer, CardSlot targetSlot)
	{
		__instance.Anim.Play("shake", 0, 0f);
		__instance.armAnim.gameObject.SetActive(value: true);


		// target slot = 3 (far right)
		// player slot = 0, 3 - 0 == 3
		// player slot, 0, 3 * 1 == 3
		int numToDetermineRotation = (
			                             targetSlot.Index // 0
			                             -
			                             __instance.PlayableCard.Slot.Index // 3
		                             ) // == -3
		                             *
		                             (__instance.PlayableCard.Slot.IsPlayerSlot ? 1 : -1);

		bool isPlayerSideBeingAttacked = targetSlot.IsPlayerSlot;
		bool isCardOpponents = __instance.PlayableCard.OpponentCard;
		bool targetSlotIsFarthestAway =
			Mathf.Abs(numToDetermineRotation) == BoardManager.Instance.PlayerSlotsCopy.Count - 1;

		bool hasInvertedStrikeAndTargetIsFarthestSlot
			= __instance.PlayableCard.HasAbility(InvertedStrike.ability) && targetSlotIsFarthestAway;

		string directionToAttack = numToDetermineRotation switch
		{
			< 0 => "_left",
			> 0 => "_right",
			_ => ""
		};

		string animToPlay = (attackPlayer ? "attack_player" : "attack_creature") + directionToAttack;

		if (hasInvertedStrikeAndTargetIsFarthestSlot)
		{
			animToPlay += "_invertedstrike";
		}
		else if (__instance.PlayableCard.HasAbility(AreaOfEffectStrike.ability))
		{
			if (isPlayerSideBeingAttacked)
			{
				if (!isCardOpponents)
				{
					animToPlay += "_adj";
				}
			}
			else
			{
				if (isCardOpponents)
				{
					animToPlay += "_adj";
				}
			}
		}

		__instance.armAnim.Play(animToPlay, 0, 0f);
		string soundId = "gravestone_card_attack_" + (attackPlayer ? "player" : "creature");
		AudioController.Instance.PlaySound3D(
			soundId,
			MixerGroup.TableObjectsSFX,
			__instance.transform.position,
			1f,
			0f,
			new AudioParams.Pitch(AudioParams.Pitch.Variation.Small),
			new AudioParams.Repetition(0.05f));

		return false;
	}

	[HarmonyPrefix, HarmonyPatch(nameof(GravestoneCardAnimationController.PlayDeathAnimation))]
	public static bool ChangeDeathAnimationToNotNullOut(
		GravestoneCardAnimationController __instance,
		bool playSound = true
	)
	{
		if (__instance.PlayableCard is not null)
		{
			return true;
		}

		__instance.PlayGlitchOutAnimation();
		return false;
	}
}

[HarmonyPatch(typeof(CardAnimationController))]
public class GravestoneCardAnimBaseClassPatches
{
	private static readonly int Hover = Animator.StringToHash("hover");
	private static readonly int Hovering = Animator.StringToHash("hovering");

	[HarmonyPrefix, HarmonyPatch(nameof(CardAnimationController.SetHovering))]
	public static bool ChangeSetHoveringForGraveCards(CardAnimationController __instance, bool hovering)
	{
		if (GrimoraSaveUtil.isNotGrimora)
		{
			return true;
		}

		GravestoneCardAnimationController controller = (GravestoneCardAnimationController)__instance;

		if (hovering)
		{
			controller.Anim.ResetTrigger(Hover);
			controller.Anim.SetTrigger(Hover);
		}
		
		controller.Anim.SetBool(Hovering, hovering);

		return false;
	}

	[HarmonyPostfix, HarmonyPatch(nameof(CardAnimationController.FlipInAir))]
	public static IEnumerator PlayCardFlipInHandAnim(IEnumerator enumerator, CardAnimationController __instance)
	{
		if (GrimoraSaveUtil.isNotGrimora)
		{
			yield return enumerator;
			yield break;
		}

		yield return new WaitForSeconds(0.6f);
		__instance.Anim.Play("card_flip_inair");
		yield return new WaitForSeconds(0.15f);
	}

	[HarmonyPrefix, HarmonyPatch(nameof(CardAnimationController.PlayTransformAnimation))]
	public static bool PlayCardFlipAnim(CardAnimationController __instance)
	{
		if (GrimoraSaveUtil.isNotGrimora)
		{
			return true;
		}

		((GravestoneCardAnimationController)__instance).SetTrigger("flip");
		return false;
	}
}
