using System.Collections;
using DiskCardGame;
using HarmonyLib;
using static GrimoraMod.GrimoraPlugin;
using GrimoraMod.Saving;

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
		if (ConfigHelper.Instance.IsDevModeEnabled)
		{
			yield return ResourcesManager.Instance.AddBones(25);
		}

		if (!SaveFile.IsAscension ||( SaveFile.IsAscension && !AscensionSaveData.Data.ChallengeIsActive(ChallengeManagement.NoBones)))
		{
			int bonesToAdd = GrimoraRunState.CurrentRun.regionTier;
			Log.LogDebug($"[SetupPhase] Adding [{bonesToAdd}] bones");
			yield return ResourcesManager.Instance.AddBones(bonesToAdd);
		}
		else
		{
			Log.LogInfo($"{SaveFile.IsAscension } +  {AscensionSaveData.Data.ChallengeIsActive(ChallengeManagement.NoBones)}");
		}
	}
}
