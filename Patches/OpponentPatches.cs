﻿using DiskCardGame;
using HarmonyLib;
using UnityEngine;
using static GrimoraMod.GrimoraPlugin;

namespace GrimoraMod;

[HarmonyPatch(typeof(Opponent))]
public class OpponentPatches
{
	// this.AddComponent(typeof (T)) as T;
	private static readonly Dictionary<Opponent.Type, Type> BossBattleSequencers = new()
	{
		{ BaseBossExt.SawyerOpponent, typeof(SawyerBossOpponent) },
		{ BaseBossExt.GrimoraOpponent, typeof(GrimoraBossOpponentExt) },
		{ BaseBossExt.KayceeOpponent, typeof(KayceeBossOpponent) },
		{ BaseBossExt.RoyalOpponent, typeof(RoyalBossOpponentExt) }
	};

	[HarmonyPrefix, HarmonyPatch(nameof(Opponent.SpawnOpponent))]
	public static bool Prefix(EncounterData encounterData, ref Opponent __result)
	{
		if (!SaveManager.SaveFile.IsGrimora)
		{
			return true;
		}

		GameObject gameObject = new GameObject
		{
			name = "Opponent"
		};
		
		Opponent opponent = null;

		if (BossBattleSequencers.TryGetValue(encounterData.opponentType, out Type typeOut))
		{
			opponent = gameObject.AddComponent(typeOut) as Opponent;
		}
		else
		{
			opponent = gameObject.AddComponent<FinaleGrimoraOpponent>();
		}

		Log.LogDebug($"[SpawnOpponent] Spawning opponent [{opponent}]");

		string text = encounterData.aiId;
		if (string.IsNullOrEmpty(text))
		{
			text = "AI";
		}

		opponent.AI = Activator.CreateInstance(CustomType.GetType("DiskCardGame", text)) as AI;
		opponent.NumLives = opponent.StartingLives;
		opponent.OpponentType = encounterData.opponentType;
		opponent.TurnPlan = EncounterBuilder.BuildOpponentTurnPlan(encounterData.Blueprint, 0, false);
		opponent.Blueprint = encounterData.Blueprint;
		opponent.Difficulty = encounterData.Difficulty;
		opponent.ExtraTurnsToSurrender = SeededRandom.Range(3, 4, SaveManager.SaveFile.GetCurrentRandomSeed());
		__result = opponent;

		Log.LogDebug($"[Opponent.SpawnOpponent] Opponent result [{__result}]");

		return false;

	}
}

[HarmonyPatch(typeof(Part1BossOpponent))]
public class Part1BossOpponentPatches
{
	[HarmonyPrefix, HarmonyPatch(nameof(Part1BossOpponent.ReducePlayerLivesSequence))]
	public static void SetPlayerLivesToOne(Part1BossOpponent __instance)
	{
		if (SaveManager.SaveFile.IsGrimora)
		{
			RunState.Run.playerLives = 1;
		}
	}

	[HarmonyPrefix, HarmonyPatch(nameof(Part1BossOpponent.BossDefeatedSequence))]
	public static bool Prefix()
	{
		// the reason for this patch is so that the game doesn't try to replenish lives since the candle doesn't exist
		return !SaveManager.SaveFile.IsGrimora;
	}
}