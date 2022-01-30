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
		// Log.LogDebug($"[Controller] Attacks player? [{attackPlayer}] TargetSlot is [{targetSlot.Index}]");
		__instance.Anim.Play("shake", 0, 0f);
		__instance.armAnim.gameObject.SetActive(value: true);

		bool isPlayerSideBeingAttacked = targetSlot.IsPlayerSlot;

		// Log.LogDebug($"[Controller] Current rotation is [{currentRotation}]");

		// target slot = 3 (far right)
		// player slot = 0, 3 - 0 == 3
		// player slot, 0, 3 * 1 == 3
		int numToDetermineRotation = (
			                             targetSlot.Index // 0
			                             -
			                             __instance.PlayableCard.Slot.Index // 1
		                             ) // == -1
		                             *
		                             (__instance.PlayableCard.Slot.IsPlayerSlot ? 1 : -1);
		// Log.LogDebug($"[GravestoneAnim] Num is [{numToDetermineRotation}]");

		if (isPlayerSideBeingAttacked)
		{
			// for Area Of Effect Strike
			if (!__instance.PlayableCard.OpponentCard)
			{
				Log.LogDebug($"[GravestoneAnim] is not opponent card [{__instance.PlayableCard}]");

				__instance.armAnim.transform.localRotation = DetermineRotationForAdjCards(numToDetermineRotation, true);
			}
			else
			{
				// default y position is -1.0 for straight ahead
				__instance.armAnim.transform.localRotation = numToDetermineRotation switch
				{
					< 0 => Quaternion.Euler(50, 90, 270),
					> 0 => Quaternion.Euler(50, 270, 90),
					_ => Quaternion.Euler(90, 180, 0)
				};
			}
		}
		else
		{
			if (__instance.PlayableCard.OpponentCard)
			{
				Log.LogDebug($"[GravestoneAnim] is opponent card [{__instance.PlayableCard}]");

				__instance.armAnim.transform.localRotation = DetermineRotationForAdjCards(numToDetermineRotation, false);
			}
			else
			{
				int increaseX = 310 + (Mathf.Abs(numToDetermineRotation) == 1 ? 0 : 30);
				// don't hardcode the max slot index value to 3 
				float zed = Mathf.Abs(numToDetermineRotation) == BoardManager.Instance.PlayerSlotsCopy.Count - 1
					? 1.2f
					: 0.5f;

				// Log.LogDebug($"[GravestoneAnim] X [{increaseX}] Zed [{zed}]");
				__instance.armAnim.transform.localScale = new Vector3(0.5f, 0.5f, zed);

				__instance.armAnim.transform.localRotation = numToDetermineRotation switch
				{
					< 0 => Quaternion.Euler(increaseX, 270, 90),
					> 0 => Quaternion.Euler(increaseX, 90, 270),
					_ => Quaternion.Euler(270, 0, 0)
				};
			}
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

	private static Quaternion DetermineRotationForAdjCards(int numToDetermineRotation, bool isPlayerSide)
	{
		if (isPlayerSide)
		{
			return numToDetermineRotation == -1
				? Quaternion.Euler(0, 270, 90) // left of the player card slot	
				: Quaternion.Euler(0, 90, 270); // right of the player card slot
		}
		else
		{
			return numToDetermineRotation == -1
				? Quaternion.Euler(0, 90, 270) // right of the opponent slot
				: Quaternion.Euler(0, 270, 90); // left of the opponent slot
		}
	}
}
