using System.Collections;
using DiskCardGame;
using HarmonyLib;
using UnityEngine;

namespace GrimoraMod;

[HarmonyPatch(typeof(ExplodeOnDeath))]
public class ExplodeOnDeathPatches
{
	[HarmonyPrefix, HarmonyPatch(nameof(ExplodeOnDeath.Awake))]
	public static bool ChangeToGrimoraBombFist(ExplodeOnDeath __instance)
	{
		__instance.bombPrefab = AssetUtils.GetPrefab<GameObject>("SkeletonArm_BombFist");
		return false;
	}
	
	[HarmonyPostfix, HarmonyPatch(nameof(ExplodeOnDeath.BombCard))]
	public static IEnumerator CorrectExceptionIfHooked(
		IEnumerator enumerator,
		ExplodeOnDeath __instance,
		PlayableCard target,
		PlayableCard attacker
	)
	{
		if (target)
		{
			yield return enumerator;
		}
		else
		{
			yield return __instance.ExplodeFromSlot(attacker.Slot);
		}
	}
}
