using DiskCardGame;
using HarmonyLib;
using InscryptionAPI.Card;

namespace GrimoraMod;

// [HarmonyPatch(typeof(VariableStatBehaviour))]
public class VariableStatBehaviourPatches
{
	
	[HarmonyPostfix, HarmonyPatch(nameof(VariableStatBehaviour.Start))]
	public static void CallUpdateStatsAfterStart(VariableStatBehaviour __instance)
	{
		if (__instance.PlayableCard.NotDead())
		{
			GrimoraPlugin.Log.LogDebug($"[VariableStatBehaviour] After Start for card {__instance.PlayableCard}");
			__instance.UpdateStats();
		}
	}
	
	[HarmonyPrefix, HarmonyPatch(nameof(VariableStatBehaviour.ManagedUpdate))]
	public static bool NoMoreManagedUpdate()
	{
		return false;
	}
	
	// [HarmonyPrefix, HarmonyPatch(nameof(VariableStatBehaviour.UpdateStats))]
	public static void LogCall(VariableStatBehaviour __instance)
	{
		GrimoraPlugin.Log.LogDebug($"[VariableStatBehaviour] UpdateStats for card {__instance.PlayableCard}");
	}
}
