using System.Collections;
using System.Text;
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
		if (targetSlot.IsNull() && __instance.PlayableCard.HasAbility(Ability.Sentry))
		{
			var skeletonArmShoot = __instance.transform.Find("SkeletonArmAttackShoot").GetComponent<Animator>();
			skeletonArmShoot.gameObject.SetActive(true);
			skeletonArmShoot.Play("attack_sentry", 0, 0);
			return false;
		}

		__instance.Anim.Play("shake", 0, 0f);
		__instance.armAnim.gameObject.SetActive(true);

		int numToDetermineRotation = (
			                             targetSlot.Index                     // 0
			                             - __instance.PlayableCard.Slot.Index // 3
		                             )                                      // == -3
		                             * (__instance.PlayableCard.Slot.IsPlayerSlot
			                             ? 1
			                             : -1);

		string typeToAttack = attackPlayer
			? "attack_player"
			: "attack_creature";

		var animToPlay = GetAnimToPlay(
			__instance.PlayableCard,
			typeToAttack,
			numToDetermineRotation,
			targetSlot
		);

		__instance.armAnim.Play(animToPlay, 0, 0f);
		string soundId = "gravestone_card_" + typeToAttack;
		AudioController.Instance.PlaySound3D(
			soundId,
			MixerGroup.TableObjectsSFX,
			__instance.transform.position,
			1f,
			0f,
			new AudioParams.Pitch(AudioParams.Pitch.Variation.Small),
			new AudioParams.Repetition(0.05f)
		);

		__instance.UpdateHoveringForCard();

		return false;
	}

	private static string GetAnimToPlay(
		PlayableCard playableCard,
		string typeToAttack,
		int numToDetermineRotation,
		CardSlot targetSlot
	)
	{
		string directionToAttack = numToDetermineRotation switch
		{
			< 0 => "_left",
			> 0 => "_right",
			_   => ""
		};

		bool isPlayerSideBeingAttacked = targetSlot.IsPlayerSlot;
		bool isCardOpponents = playableCard.OpponentCard;
		bool hasAreaOfEffectStrike = playableCard.HasAbility(AreaOfEffectStrike.ability);
		bool hasInvertedStrike = playableCard.HasAbility(InvertedStrike.ability);
		bool targetSlotIsFarthestAway =
			Mathf.Abs(numToDetermineRotation) == BoardManager.Instance.PlayerSlotsCopy.Count - 1;

		bool cardIsTargetingAdjFriendly = isPlayerSideBeingAttacked && !isCardOpponents
		                                  || !isPlayerSideBeingAttacked && isCardOpponents;

		StringBuilder animToPlay = new StringBuilder(typeToAttack + directionToAttack);

		if (playableCard.HasSpecialAbility(GrimoraGiant.FullAbility.Id))
		{
			animToPlay.Append("_giant");
		}
		else if (hasInvertedStrike)
		{
			if (targetSlotIsFarthestAway)
			{
				animToPlay.Append("_invertedstrike_far");
			}
			else if (Math.Abs(targetSlot.Index - playableCard.Slot.Index) == 2)
			{
				animToPlay.Append("_invertedstrike");	
			}
		}
		else if (hasAreaOfEffectStrike || cardIsTargetingAdjFriendly)
		{
			if (isPlayerSideBeingAttacked)
			{
				if (!isCardOpponents)
				{
					animToPlay.Append("_adj");
				}
			}
			else
			{
				if (isCardOpponents)
				{
					animToPlay.Append("_adj");
				}
			}
		}

		return animToPlay.ToString();
	}

	private static void SetSkeletonArmAttackPositionForGiantCards(GravestoneCardAnimationController __instance)
	{
		if (__instance.PlayableCard.Info.HasTrait(Trait.Giant))
		{
			float xValPosition = -0.7f;
			Transform skeletonArm = __instance.transform.GetChild(1);
			Vector3 skeletonArmAnimPosition = skeletonArm.localPosition;
			if (ConfigHelper.HasIncreaseSlotsMod && __instance.Card.InfoName().Equals(NameBonelord))
			{
				xValPosition = -1.4f;
				Log.LogDebug($"[Giant] Setting skeleArm bonelord");
			}

			skeletonArm.localPosition = new Vector3(xValPosition, skeletonArmAnimPosition.y, skeletonArmAnimPosition.z);
		}
	}

	[HarmonyPrefix, HarmonyPatch(nameof(GravestoneCardAnimationController.PlayDeathAnimation))]
	public static bool ChangeDeathAnimationToNotNullOut(
		GravestoneCardAnimationController __instance,
		bool playSound = true
	)
	{
		if (__instance.PlayableCard)
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
	[HarmonyPrefix, HarmonyPatch(nameof(CardAnimationController.SetHovering))]
	public static bool ChangeSetHoveringForGraveCards(CardAnimationController __instance, bool hovering)
	{
		if (GrimoraSaveUtil.isNotGrimora)
		{
			return true;
		}

		((GravestoneCardAnimationController)__instance).UpdateHoveringForCard(hovering);

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

		__instance.Anim.Play("card_flip_inair");
	}

	[HarmonyPrefix, HarmonyPatch(nameof(CardAnimationController.PlayTransformAnimation))]
	public static bool PlayCardFlipAnim(CardAnimationController __instance)
	{
		if (GrimoraSaveUtil.isNotGrimora)
		{
			return true;
		}

		__instance.Anim.Play("card_flip");
		return false;
	}
}
