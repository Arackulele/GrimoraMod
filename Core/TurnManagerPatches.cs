using System.Collections;
using DiskCardGame;
using HarmonyLib;
using static GrimoraMod.GrimoraPlugin;

namespace GrimoraMod;

[HarmonyPatch(typeof(TurnManager))]
public class TurnManagerPatches
{
	private static readonly Dictionary<string, Type> GrimoraModBattleSequencers = new()
	{
		{ SawyerBossOpponent.SpecialId, typeof(SawyerBattleSequencer) },
		{ GrimoraBossOpponentExt.SpecialId, typeof(GrimoraBossSequencer) },
		{ KayceeBossOpponent.SpecialId, typeof(KayceeBossSequencer) },
		{ RoyalBossOpponentExt.SpecialId, typeof(RoyalBossSequencer) },
		{ "GrimoraModBattleSequencer", typeof(GrimoraModBattleSequencer) }
	};

	[HarmonyPrefix, HarmonyPatch(nameof(TurnManager.UpdateSpecialSequencer))]
	public static bool Prefix(ref TurnManager __instance, string specialBattleId)
	{
		Log.LogDebug($"[UpdateSpecialSequencer][Prefix] " +
		             $"SpecialBattleId [{specialBattleId}] " +
		             $"SaveFile is grimora? [{SaveManager.SaveFile.IsGrimora}]");

		UnityEngine.Object.Destroy(__instance.SpecialSequencer);
		__instance.SpecialSequencer = null;

		if (SaveManager.SaveFile.IsGrimora && GrimoraModBattleSequencers.ContainsKey(specialBattleId))
		{
			__instance.SpecialSequencer =
				__instance.gameObject.AddComponent(GrimoraModBattleSequencers[specialBattleId]) as SpecialBattleSequencer;

			Log.LogDebug($"[UpdateSpecialSequencer][Prefix] SpecialSequencer is [{__instance.SpecialSequencer}]");
			return false;
		}

		return true;
	}


	[HarmonyPostfix, HarmonyPatch(nameof(TurnManager.SetupPhase))]
	public static IEnumerator PostfixAddStartingBones(
		IEnumerator enumerator,
		TurnManager __instance,
		EncounterData encounterData
	)
	{
		yield return enumerator;
		int bonesToAdd = ConfigHelper.Instance.BonesToAdd;
		Log.LogDebug($"[SetupPhase] Adding [{bonesToAdd}] bones");
		if (ConfigHelper.IsDevModeEnabled)
		{
			yield return ResourcesManager.Instance.AddBones(25);
		}
		else
		{
			yield return ResourcesManager.Instance.AddBones(bonesToAdd);
		}
	}
}
