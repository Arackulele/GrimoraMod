using DiskCardGame;
using HarmonyLib;

namespace GrimoraMod
{
	[HarmonyPatch(typeof(ChessboardMapNode))]
	public class MapNodePatches
	{
		
		[HarmonyPostfix, HarmonyPatch(nameof(ChessboardMapNode.OnArriveAtNode))]
		public static void PostfixOnArriveAtNode()
		{
			GrimoraPlugin.Log.LogDebug($"Active node is now " +
			                           $"x[{GrimoraSaveData.Data.gridX}] " +
			                           $"y[{GrimoraSaveData.Data.gridY}]");
		}
		
	}
}