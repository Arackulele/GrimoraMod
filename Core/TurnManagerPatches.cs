using System.Collections;
using DiskCardGame;
using HarmonyLib;

namespace GrimoraMod;

[HarmonyPatch(typeof(TurnManager))]
public class TurnManagerPatches
{
	private static readonly Dictionary<string, Type> BossBattleSequencers = new()
	{
		{ SawyerBossOpponent.SpecialId, typeof(SawyerBattleSequencer) },
		{ GrimoraBossOpponentExt.SpecialId, typeof(GrimoraBossSequencer) },
		{ KayceeBossOpponent.SpecialId, typeof(KayceeBossSequencer) },
		{ RoyalBossOpponentExt.SpecialId, typeof(RoyalBossSequencer) }
	};

	[HarmonyPrefix, HarmonyPatch(nameof(TurnManager.UpdateSpecialSequencer))]
	public static bool Prefix(ref TurnManager __instance, string specialBattleId)
	{
		GrimoraPlugin.Log.LogDebug($"[UpdateSpecialSequencer][Prefix] " +
		                           $"SpecialBattleId [{specialBattleId}] " +
		                           $"SaveFile is grimora? [{SaveManager.SaveFile.IsGrimora}]");

		UnityEngine.Object.Destroy(__instance.SpecialSequencer);
		__instance.SpecialSequencer = null;

		if (SaveManager.SaveFile.IsGrimora && BossBattleSequencers.ContainsKey(specialBattleId))
		{
			__instance.SpecialSequencer =
				__instance.gameObject.AddComponent(BossBattleSequencers[specialBattleId]) as SpecialBattleSequencer;

			// GrimoraPlugin.Log.LogDebug($"[UpdateSpecialSequencer][Prefix] SpecialSequencer is [{__instance.SpecialSequencer}]");
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
		int bonesToAdd = ChessboardMapExt.BonesToAdd;
		GrimoraPlugin.Log.LogDebug($"[SetupPhase] Adding [{bonesToAdd}] bones");
		yield return ResourcesManager.Instance.AddBones(bonesToAdd);
	}
}