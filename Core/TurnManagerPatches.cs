using System;
using System.Collections.Generic;
using DiskCardGame;
using HarmonyLib;

namespace GrimoraMod
{
	[HarmonyPatch(typeof(TurnManager))]
	public class TurnManagerPatches
	{
		private static readonly Dictionary<string, Type> BossBattleSequencers = new()
		{
			{ "DoggyBoss", typeof(DoggyBossSequencer) },
			{ "GrimoraBoss", typeof(GrimoraBossSequencer) },
			{ "KayceeBoss", typeof(KayceeBossSequencer) },
			{ "RoyalBoss", typeof(RoyalBossSequencer) }
		};

		[HarmonyPrefix, HarmonyPatch(nameof(TurnManager.StartGame), typeof(CardBattleNodeData))]
		public static void StartGameCardBattleNodeDataPrefix(CardBattleNodeData nodeData)
		{
			GrimoraPlugin.Log.LogDebug($"[TurnManager.StartGame, CardBattleNodeData][Prefix] " +
			                           $"SpecialBattle Id [{nodeData?.specialBattleId}]");
		}
		
		[HarmonyPrefix, HarmonyPatch(nameof(TurnManager.StartGame), typeof(EncounterData))]
		public static void StartGamePrefix(EncounterData encounterData)
		{
			GrimoraPlugin.Log.LogDebug($"[TurnManager.StartGame, EncounterData][Prefix] " +
			                           $"Opponent [{encounterData.opponentType}]");
		}
		
		[HarmonyPrefix, HarmonyPatch(nameof(TurnManager.StartGame), typeof(EncounterData), typeof(string))]
		public static void StartGameSpecialBattleIdPrefix(EncounterData encounterData)
		{
			GrimoraPlugin.Log.LogDebug($"[TurnManager.StartGame, EncounterData, specialBattleId][Prefix]" +
			                           $" Opponent [{encounterData.opponentType}]");
		}
		
		[HarmonyPrefix, HarmonyPatch(nameof(TurnManager.CreateOpponent), typeof(EncounterData))]
		public static void CreateOpponentPrefix(EncounterData encounterData)
		{
			GrimoraPlugin.Log.LogDebug($"[TurnManager.CreateOpponent][Prefix] Opponent [{encounterData.opponentType}]");
		}

		[HarmonyPrefix, HarmonyPatch(nameof(TurnManager.UpdateSpecialSequencer))]
		public static bool Prefix(ref TurnManager __instance, string specialBattleId)
		{
			GrimoraPlugin.Log.LogDebug($"[TurnManager.UpdateSpecialSequencer][Prefix] " +
			                           $"SpecialBattleId [{specialBattleId}] " +
			                           $"SaveFile is grimora? [{SaveManager.SaveFile.IsGrimora}]");

			UnityEngine.Object.Destroy(__instance.SpecialSequencer);
			__instance.SpecialSequencer = null;
			
			if (SaveManager.SaveFile.IsGrimora && BossBattleSequencers.ContainsKey(specialBattleId))
			{
				__instance.SpecialSequencer =
					__instance.gameObject.AddComponent(BossBattleSequencers[specialBattleId]) as SpecialBattleSequencer;

				GrimoraPlugin.Log.LogDebug(
					$"[TurnManager.UpdateSpecialSequencer][Prefix] SpecialSequencer is [{__instance.SpecialSequencer}]");
				return false;
			}

			return true;
		}
	}
}