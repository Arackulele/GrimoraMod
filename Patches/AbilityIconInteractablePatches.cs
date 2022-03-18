using DiskCardGame;
using HarmonyLib;
using UnityEngine;

namespace GrimoraMod;

// [HarmonyPatch(typeof(AbilityIconInteractable))]
public class AbilityIconInteractablePatches
{
	[HarmonyPrefix, HarmonyPatch(nameof(AbilityIconInteractable.SetFlippedY))]
	public static bool ChangeStupidNegativeYScalingLogWarning(AbilityIconInteractable __instance, bool flippedY)
	{
		if (flippedY)
		{
			GrimoraPlugin.Log.LogDebug(
				$"[AbilityIconPatch.Before] Card [{__instance.card.InfoName()}] Ability [{__instance.Ability}] Is Opponent Card [{__instance.card.OpponentCard}] FlippedY? [{flippedY}]"
			);
			GrimoraPlugin.Log.LogDebug(
				$"[AbilityIconPatch.Before] Card [{__instance.card.InfoName()}] Rotation [{__instance.transform.rotation}] Local Rotation [{__instance.transform.localRotation}]"
			);
			float y = 1f * Mathf.Abs(__instance.transform.localScale.y);
			__instance.transform.localScale = new Vector3(
				__instance.transform.localScale.x,
				y,
				__instance.transform.localScale.z
			);
			__instance.transform.localRotation = Quaternion.Euler(0, 0, 180);
			// // if (flippedY)
			// // {
			// // 	GrimoraPlugin.Log.LogDebug($"[AbilityIconPatch] Card [{__instance.card.name}] Rotation [{__instance.transform.rotation}] Local Rotation [{__instance.transform.localRotation}]");
			// // 	__instance.transform.Rotate(new Vector3(0, 0, 180));
			// // }
			return false;
		}

		return true;
	}

	[HarmonyPostfix, HarmonyPatch(nameof(AbilityIconInteractable.SetFlippedY))]
	public static void ChangeStupidNegativeYScalingLogWarningAfter(
		AbilityIconInteractable __instance,
		bool flippedY
	)
	{
		if (flippedY)
		{
			GrimoraPlugin.Log.LogDebug(
				$"[AbilityIconPatch.After ] Card [{__instance.card.InfoName()}] Ability [{__instance.Ability}] Is Opponent Card [{__instance.card.OpponentCard}] FlippedY? [{flippedY}]"
			);
			GrimoraPlugin.Log.LogDebug(
				$"[AbilityIconPatch.After ] Card [{__instance.card.InfoName()}] Rotation [{__instance.transform.rotation}] Local Rotation [{__instance.transform.localRotation}]"
			);
		}
		// float y = 1f * Mathf.Abs(__instance.transform.localScale.y);
		// __instance.transform.localScale = new Vector3(__instance.transform.localScale.x, y, __instance.transform.localScale.z);
		// // if (flippedY)
		// // {
		// // 	GrimoraPlugin.Log.LogDebug($"[AbilityIconPatch] Card [{__instance.card.name}] Rotation [{__instance.transform.rotation}] Local Rotation [{__instance.transform.localRotation}]");
		// // 	__instance.transform.Rotate(new Vector3(0, 0, 180));
		// // }
		// return false;
	}
}
