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
		Log.LogDebug($"[Controller] Attacks player? [{attackPlayer}] TargetSlot is [{targetSlot.Index}]");
		__instance.Anim.Play("shake", 0, 0f);
		__instance.armAnim.gameObject.SetActive(value: true);

		Quaternion currentRotation = __instance.armAnim.transform.rotation;
		bool isPlayerSideBeingAttacked = targetSlot.IsPlayerSlot;

		Log.LogDebug($"[Controller] Current rotation is [{currentRotation}]");

		int num = (targetSlot.Index - __instance.PlayableCard.Slot.Index) *
		          (__instance.PlayableCard.Slot.IsPlayerSlot ? 1 : -1);
		Log.LogDebug($"Num is [{num}]");

		// float newY = 0f;
		if (isPlayerSideBeingAttacked)
		{
			// default y position is -1.0 for straight ahead
			__instance.armAnim.transform.localRotation = num switch
			{
				< 0 => Quaternion.Euler(50, 90, 270),
				> 0 => Quaternion.Euler(50, 270, 90),
				_ => Quaternion.Euler(90, 180, 0)
			};
		}
		else
		{
			__instance.armAnim.transform.localRotation = num switch
			{
				< 0 => Quaternion.Euler(310, 270, 90),
				> 0 => Quaternion.Euler(310, 90, 270),
				_ => Quaternion.Euler(270, 0, 0)
			};
		}

		// __instance.armAnim.transform.Rotate(0, newY, 0f);
		__instance.armAnim.Play(attackPlayer ? "attack_player" : "attack_creature", 0, 0f);
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
}
