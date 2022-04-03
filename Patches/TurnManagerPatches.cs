using System.Collections;
using DiskCardGame;
using HarmonyLib;
using static GrimoraMod.GrimoraPlugin;

namespace GrimoraMod;

[HarmonyPatch(typeof(TurnManager))]
public class TurnManagerPatches
{
	[HarmonyPostfix, HarmonyPatch(nameof(TurnManager.SetupPhase))]
	public static IEnumerator PostfixAddStartingBones(
		IEnumerator enumerator,
		TurnManager __instance,
		EncounterData encounterData
	)
	{
		yield return enumerator;
		if (ConfigHelper.Instance.isDevModeEnabled)
		{
			yield return ResourcesManager.Instance.AddBones(25);
		}

		int bonesToAdd = ConfigHelper.Instance.BonesToAdd;
		Log.LogDebug($"[SetupPhase] Adding [{bonesToAdd}] bones");
		yield return ResourcesManager.Instance.AddBones(bonesToAdd);
	}
}
