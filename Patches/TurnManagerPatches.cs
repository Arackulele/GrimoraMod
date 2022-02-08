using System.Collections;
using DiskCardGame;
using HarmonyLib;
using static GrimoraMod.GrimoraPlugin;

namespace GrimoraMod;

[HarmonyPatch(typeof(TurnManager))]
public class TurnManagerPatches
{

	[HarmonyPrefix, HarmonyPatch(nameof(TurnManager.UpdateSpecialSequencer))]
	public static bool Prefix(ref TurnManager __instance, string specialBattleId)
	{
		if (!SaveManager.SaveFile.IsGrimora)
		{
			return true;
		}

		Log.LogDebug($"[UpdateSpecialSequencer][Prefix] " +
		             $"SpecialBattleId [{specialBattleId}] " +
		             $"SaveFile is grimora? [{SaveManager.SaveFile.IsGrimora}]");

		Object.Destroy(__instance.SpecialSequencer);
		__instance.SpecialSequencer = null;

		if (BaseBossExt.OpponentTupleBySpecialId.ContainsKey(specialBattleId))
		{
			__instance.SpecialSequencer = __instance.gameObject
					.AddComponent(BaseBossExt.OpponentTupleBySpecialId[specialBattleId].Item2) as SpecialBattleSequencer;

			Log.LogDebug($"[UpdateSpecialSequencer][Prefix] SpecialSequencer is [{__instance.SpecialSequencer}]");
		}

		return false;
	}


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
		else
		{
			int bonesToAdd = ConfigHelper.Instance.BonesToAdd;
			Log.LogDebug($"[SetupPhase] Adding [{bonesToAdd}] bones");
			yield return ResourcesManager.Instance.AddBones(bonesToAdd);
		}
	}
}
