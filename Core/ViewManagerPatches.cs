using DiskCardGame;
using HarmonyLib;

namespace GrimoraMod
{
	
	[HarmonyPatch(typeof(DiskCardGame.ViewManager))]
	public class ViewManagerPatches
	{

		[HarmonyPostfix, HarmonyPatch(nameof(ViewManager.SwitchToView))]
		public static void Postfix(View view, bool immediate = false, bool lockAfter = false)
		{
			GrimoraPlugin.Log.LogDebug($"[ViewManager.SwitchToView] Called with view [{view}]");
		}
		
	}
}