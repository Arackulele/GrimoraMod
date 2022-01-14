using DiskCardGame;
using HarmonyLib;

namespace GrimoraMod
{
	[HarmonyPatch(typeof(ChessboardMap))]
	public class BaseChessboardMapPatches
	{

		[HarmonyPrefix, HarmonyPatch(nameof(ChessboardMap.UnrollingSequence))]
		public static bool PrefixPatchOutLogic(ChessboardMap __instance)
		{
			GrimoraPlugin.Log.LogDebug($"[ChessboardMap.UnrollingSequence] Instance is [{__instance.GetType()}]");
			return __instance is ChessboardMapExt;
		}
		
	}
}