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
		if (GrimoraSaveUtil.IsNotGrimoraModRun)
		{
			return true;
		}
		
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
			GrimoraPlugin.Log.LogDebug($"[BombCard] Target {target.GetNameAndSlot()} is not null");
			yield return enumerator;
		}
		else
		{
			GrimoraPlugin.Log.LogDebug($"[BombCard] Target is NULL. Rerunning ExplodeFromSlot. Attacker? [{attacker}]");
			yield return __instance.ExplodeFromSlot(attacker.Slot);
		}
	}
}
