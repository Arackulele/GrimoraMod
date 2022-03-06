using DiskCardGame;
using HarmonyLib;
using UnityEngine;

namespace GrimoraMod;

[HarmonyPatch(typeof(AbilityIconInteractable))]
public class AbilityIconInteractablePatches
{
	[HarmonyPrefix, HarmonyPatch(nameof(AbilityIconInteractable.SetFlippedY))]
	public static bool ChangeStupidNegativeYScalingLogWarning(AbilityIconInteractable __instance, bool flippedY)
	{
		var localScale = __instance.transform.localScale;
		float y = 1f * Mathf.Abs(localScale.y);
		__instance.transform.localScale = new Vector3(localScale.x, y, localScale.z);
		if (flippedY)
		{
			__instance.transform.Rotate(new Vector3(0, 0, 180));
		}
		else
		{
			__instance.transform.rotation = Quaternion.identity;
		}
		return false;
	}
	
}
