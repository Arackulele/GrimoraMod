using DiskCardGame;
using HarmonyLib;

namespace GrimoraMod
{
	[HarmonyPatch(typeof(MapNodeManager))]
	public class MapNodeManagerPatches
	{
		[HarmonyPrefix, HarmonyPatch(nameof(MapNodeManager.SetActiveNode))]
		public static bool PrefixChangeActiveNodeLogic(ref MapNode newActiveNode)
		{
			if (!SaveManager.SaveFile.IsGrimora)
			{
				return true;
			}

			if (newActiveNode is null)
			{
				GrimoraPlugin.Log.LogDebug($"[SetActiveNode] NewActiveNode is null, setting to current active node");
				newActiveNode = MapNodeManager.Instance.ActiveNode;
				newActiveNode.nodeId = RunState.Run.currentNodeId;
			}

			GrimoraPlugin.Log.LogDebug(
				$"[SetActiveNode] Setting activeNode active x{newActiveNode.Data.gridX}y{newActiveNode.Data.gridY}");
			newActiveNode.SetActive(true);
			ChessboardNavGrid.instance.SetPlayerAdjacentNodesActive();

			return false;
		}
	}
}