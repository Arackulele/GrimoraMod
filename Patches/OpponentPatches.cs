using DiskCardGame;
using HarmonyLib;
using static GrimoraMod.GrimoraPlugin;

namespace GrimoraMod;

[HarmonyPatch(typeof(Part1BossOpponent))]
public class Part1BossOpponentPatches
{
	[HarmonyPrefix, HarmonyPatch(nameof(Part1BossOpponent.ReducePlayerLivesSequence))]
	public static void SetPlayerLivesToOne(Part1BossOpponent __instance)
	{
		if (GrimoraSaveUtil.IsGrimoraModRun)
		{
			RunState.Run.playerLives = 1;
		}
	}

	[HarmonyPrefix, HarmonyPatch(nameof(Part1BossOpponent.BossDefeatedSequence))]
	public static bool Prefix()
	{
		// the reason for this patch is so that the game doesn't try to replenish lives since the candle doesn't exist
		HammerItemExt.useCounter = 0;
		return GrimoraSaveUtil.IsNotGrimoraModRun;
	}
}
