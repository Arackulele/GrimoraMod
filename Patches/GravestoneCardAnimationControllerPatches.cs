using System.Collections;
using System.Text;
using DiskCardGame;
using HarmonyLib;
using InscryptionAPI.Card;
using UnityEngine;
using static GrimoraMod.GrimoraPlugin;

namespace GrimoraMod;

[HarmonyPatch(typeof(GravestoneCardAnimationController))]
public class GravestoneCardAnimationControllerPatches
{
	[HarmonyPatch(nameof(GravestoneCardAnimationController.PlayAttackAnimation))]
	public static bool Prefix(
		ref GravestoneCardAnimationController __instance,
		bool attackPlayer,
		CardSlot targetSlot
	)
	{
		PlayableCard playableCard = __instance.PlayableCard;
		Animator customArmPrefab = playableCard.GetCorrectCustomArmsPrefab(targetSlot);

		__instance.armAnim.gameObject.SetActive(false);
		__instance.Anim.Play("shake", 0, 0f);

		string typeToAttack = attackPlayer ? "attack_player" : "attack_creature";

		var animToPlay = GetAnimToPlay(
			playableCard,
			typeToAttack,
			targetSlot
		);
		bool doPlayCustomAttack = animToPlay == "sniper_shoot" || animToPlay == "attack_sentry" || animToPlay == "attack_middle_finger";
		string soundId = "gravestone_card_" + typeToAttack;

		if (playableCard.HasSpecialAbility(GrimoraGiant.FullSpecial.Id))
		{
			Log.LogDebug($"Playing giant attack [{animToPlay}] for card {playableCard.GetNameAndSlot()}");
			customArmPrefab.gameObject.SetActive(true);
			customArmPrefab.Play(animToPlay, 0, 0f);

			AudioController.Instance.PlaySound3D(
				soundId,
				MixerGroup.TableObjectsSFX,
				__instance.transform.position,
				1f,
				0.1f, // TODO: make it play only once or somehow stretch the knocks to time with the slams
				new AudioParams.Pitch(AudioParams.Pitch.Variation.Small),
				new AudioParams.Repetition(0.05f)
			);
		}
		else
		{
			if (doPlayCustomAttack)
			{
				Log.LogDebug($"Playing custom attack [{animToPlay}] for card {playableCard.GetNameAndSlot()}");
				customArmPrefab.gameObject.SetActive(true);
				customArmPrefab.Play(animToPlay, 0, 0f);
				if (animToPlay == "sniper_shoot")
				{
					var instance = __instance;
					CustomCoroutine.WaitThenExecute(
						0.1f,
						delegate
						{
							if (attackPlayer)
							{
								instance.OnImpactAttackPlayerKeyframe();
							} else
							{
								instance.OnImpactKeyframe();
							}
						});
				}
			}
			else
			{
				Log.LogDebug($"Playing regular attack [{animToPlay}] for card {playableCard.GetNameAndSlot()}");
				__instance.armAnim.gameObject.SetActive(true);
				__instance.armAnim.Play(animToPlay, 0, 0f);
			}

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
		}

		return false;
	}

	private static int GetNumToDetermineRotation(PlayableCard cardThatIsAttacking, CardSlot targetSlot)
	{
		if (cardThatIsAttacking.HasSpecialAbility(GrimoraGiant.FullSpecial.Id))
		{
			// 0 < 1 for example
			if (targetSlot.Index < cardThatIsAttacking.Slot.Index)
			{
				return -1;
			}

			return 1;
		}

		return (targetSlot.Index - cardThatIsAttacking.Slot.Index) * (cardThatIsAttacking.Slot.IsPlayerSlot ? 1 : -1);
	}

	private static string GetAnimToPlay(
		PlayableCard playableCard,
		string typeToAttack,
		CardSlot targetSlot
	)
	{
		Log.LogDebug($"Checking Playable card {playableCard.GetNameAndSlot()} TargetSlot {targetSlot} Attack == 0 ? [{playableCard.Attack == 0}] Has sniper? [{playableCard.HasAbility(Ability.Sniper)}]");
		bool doPlaySentryAttack = targetSlot.IsNull() && playableCard.HasAbility(Ability.Sentry);
		if (doPlaySentryAttack)
		{
			return "attack_sentry";
		}
		if (playableCard.Attack == 0)
		{
			return "attack_middle_finger";
		}
		if (playableCard.HasAbility(Ability.Sniper))
		{
			return "sniper_shoot";
		}

		Log.LogDebug($"TargetSlotIdx [{targetSlot.Index}] Card Attacking idx [{playableCard.Slot.Index}] is player? [{playableCard.Slot.IsPlayerSlot}]");
		int numToDetermineRotation = GetNumToDetermineRotation(playableCard, targetSlot);
		string directionToAttack = numToDetermineRotation switch
		{
			< 0 => "_left",
			> 0 => "_right",
			_   => ""
		};
		Log.LogDebug($"Num to determine rotation [{numToDetermineRotation}] Direction To Attack [{directionToAttack}]");

		bool isPlayerSideBeingAttacked = targetSlot.IsPlayerSlot;
		bool isCardOpponents = playableCard.OpponentCard;
		bool hasAreaOfEffectStrike = playableCard.HasAbility(AreaOfEffectStrike.ability);
		bool hasInvertedStrike = playableCard.HasAbility(InvertedStrike.ability);
		bool targetSlotIsFarthestAway =
			Mathf.Abs(numToDetermineRotation) == BoardManager.Instance.PlayerSlotsCopy.Count - 1;

		bool cardIsTargetingAdjFriendly = isPlayerSideBeingAttacked && !isCardOpponents
		                               || !isPlayerSideBeingAttacked && isCardOpponents;

		StringBuilder animToPlay = new StringBuilder(typeToAttack + directionToAttack);

		if (playableCard.HasSpecialAbility(GrimoraGiant.FullSpecial.Id))
		{
			animToPlay.Append("_giant");
		}
		// else if (hasInvertedStrike)
		// {
		// 	if (targetSlotIsFarthestAway)
		// 	{
		// 		animToPlay.Append("_invertedstrike_far");
		// 	}
		// 	else if (Math.Abs(targetSlot.Index - playableCard.Slot.Index) == 2)
		// 	{
		// 		animToPlay.Append("_invertedstrike");
		// 	}
		// }
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
		if (__instance.PlayableCard.HasTrait(Trait.Giant))
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

		__instance.Anim.Play("card_flip_inair", 0, 0);
	}

	[HarmonyPrefix, HarmonyPatch(nameof(CardAnimationController.PlayTransformAnimation))]
	public static bool PlayCardFlipAnim(CardAnimationController __instance)
	{
		if (GrimoraSaveUtil.isNotGrimora)
		{
			return true;
		}

		__instance.Anim.Play("card_flip", 0, 0);
		return false;
	}
}
