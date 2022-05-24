using DiskCardGame;
using HarmonyLib;
using GrimoraMod;

namespace GrimoraMod;

[HarmonyPatch(typeof(AscensionSaveData))]
public static class AscensionSaveDataPatches
{
	[HarmonyPatch(nameof(AscensionSaveData.EndRun))]
	[HarmonyPrefix]
	public static void EndRun()
	{
		
		GrimoraMod.GrimoraPlugin.Log.LogInfo("End of run");
		ConfigHelper.Instance.ResetRun();
		
	}
}
