using DiskCardGame;
using HarmonyLib;
using UnityEngine;

namespace GrimoraMod;

// [HarmonyPatch(typeof(AbilityIconInteractable))]
public class AbilityIconInteractablePatches
{
	[HarmonyPrefix, HarmonyPatch(nameof(AbilityIconInteractable.SetFlippedY))]
	public static void ChangeStupidNegativeYScalingLogWarning(AbilityIconInteractable __instance, bool flippedY)
	{
		if (flippedY)
		{
			// float y = 1f * Mathf.Abs(__instance.transform.localScale.y);
			// __instance.transform.localScale = new Vector3(
			// 	__instance.transform.localScale.x,
			// 	y,
			// 	__instance.transform.localScale.z
			// );

			if (__instance.gameObject.GetComponent<MeshCollider>().IsNull())
			{
				// BoxCollider originalCollider = __instance.GetComponent<BoxCollider>();
				MeshCollider collider = __instance.gameObject.AddComponent<MeshCollider>();
				collider.convex = true;
				collider.sharedMesh = null;
				collider.sharedMesh = __instance.GetComponent<MeshFilter>().mesh;

				// __instance.coll.bounds.Expand(new Vector3(0.4f, 0.4f, 0.1f));
				UnityObject.Destroy(__instance.GetComponent<BoxCollider>());
				__instance.coll = null;
				__instance.coll = collider;
			}
		}

		// return true;
	}
}
