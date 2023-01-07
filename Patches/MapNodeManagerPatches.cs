using DiskCardGame;
using HarmonyLib;

namespace GrimoraMod;

[HarmonyPatch(typeof(MapNodeManager))]
public class MapNodeManagerPatches
{
	[HarmonyPrefix, HarmonyPatch(nameof(MapNodeManager.SetActiveNode))]
	public static bool PrefixChangeActiveNodeLogic(ref MapNodeManager __instance, ref MapNode newActiveNode)
	{
		if (GrimoraSaveUtil.IsNotGrimoraModRun)
		{
			return true;
		}

		// GrimoraPlugin.Log.LogDebug($"[SetActiveNode] activeNode x{newActiveNode.Data.gridX}y{newActiveNode.Data.gridY}");
		newActiveNode.SetActive(true);
		ChessboardNavGrid.instance.SetPlayerAdjacentNodesActive();

		__instance.ActiveNode = newActiveNode;

		return false;
	}
}
