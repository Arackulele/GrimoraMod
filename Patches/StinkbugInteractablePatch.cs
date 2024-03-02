using DiskCardGame;
using HarmonyLib;
using Pixelplacement;
using System.Collections;
using UnityEngine;

namespace GrimoraMod;

[HarmonyPatch(typeof(StinkbugInteractable))]
public class StinkbugInteractablePatch
{


	[HarmonyPrefix, HarmonyPatch(nameof(StinkbugInteractable.OnCursorSelectStart))]
	public static bool OnCursorSelectStart(StinkbugInteractable __instance)
	{
		Singleton<InteractionCursor>.Instance.ForceCursorType(CursorType.Pickup);
		//harmony patch is static so i have to search
		Tween.LocalScale(__instance.transform, new Vector3(0.87f, 0.87f, 0.87f), 0.1f, 0);
		Tween.LocalScale(__instance.transform, new Vector3(1f, 1f, 1f), 0.25f, 0.11f);
		Debug.Log("Stinkbug patch run");

		CustomCoroutine.Instance.StartCoroutine(ChangeCursorBack());

		return false;
	}

	public static IEnumerator ChangeCursorBack()
	{
		yield return new WaitForSeconds(0.3f);

		Singleton<InteractionCursor>.Instance.ForceCursorType(CursorType.Default);


		yield break;
	}


}
