using DiskCardGame;
using HarmonyLib;

namespace GrimoraMod
{
	[HarmonyPatch(typeof(ChessboardMap))]
	public class ChessboardMapPatches
	{

		[HarmonyPrefix, HarmonyPatch(typeof(ChessboardMap), nameof(ChessboardMap.UnrollingSequence))]
		public static void PrefixSetState(ref ChessboardMap __instance, out ChessboardMap __state)
		{
			__state = __instance;

			// if (ViewManager.Instance.Controller is not null 
			//     && !ViewManager.Instance.Controller.allowedViews.Contains(View.MapDeckReview))
			// {
			// 	ViewManager.Instance.Controller.allowedViews.Add(View.MapDeckReview);
			// }
			//
			// GrimoraPlugin.Log.LogDebug($"[ChessboardMap.UnrollingSequence] Setting ViewManager instance ");

		}
		


	}
}