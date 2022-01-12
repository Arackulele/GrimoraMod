using System.Collections;
using DiskCardGame;
using HarmonyLib;

namespace GrimoraMod
{
	[HarmonyPatch(typeof(MapNodeManager))]
	public class MapNodeManagerPatches
	{
		// [HarmonyPrefix, HarmonyPatch(nameof(MapNodeManager.DoMoveToNewNode))]
		// public static void Prefix(ref MapNodeManager __instance, out MapNodeManager __state)
		// {
		// 	__state = __instance;
		// }

		[HarmonyPostfix, HarmonyPatch(nameof(MapNodeManager.DoMoveToNewNode))]
		public static IEnumerator Postfix(IEnumerator enumerator, MapNodeManager __instance, MapNode newNode)
		{
			GrimoraPlugin.Log.LogDebug($"[MapNodeManager.DoMoveToNewNode][Postfix] State [{__instance}] MapNode [{newNode}]");
			__instance.MovingNodes = true;
			__instance.SetAllNodesInteractable(false);
			__instance.transitioningGridY = newNode.Data.gridY;
			ViewManager.Instance.Controller.LockState = ViewLockState.Locked;
			yield return PlayerMarker.Instance.MoveToPoint(newNode.transform.position, true);

			if (newNode.Data is null)
			{
				GrimoraPlugin.Log.LogDebug("this is indeed null");
				newNode.Data = new NodeData();
				newNode.Data.prefabPath = "Prefabs/Map/MapNodesPart1/MapNode_Empty";
			}

			if (newNode.Data != null)
			{
				GrimoraPlugin.Log.LogDebug($"[MapNodeManager.DoMoveToNewNode][Postfix] Calling OnArriveAtNode");
				yield return newNode.OnArriveAtNode();
			}

			ViewManager.Instance.Controller.LockState = ViewLockState.Unlocked;
			GrimoraPlugin.Log.LogDebug($"[MapNodeManager.DoMoveToNewNode][Postfix] No longer MovingNodes");

			__instance.MovingNodes = false;

			CustomCoroutine.WaitOnConditionThenExecute(
				() => !GameFlowManager.Instance.Transitioning,
				delegate { RunState.Run.currentNodeId = newNode.nodeId; }
			);
			GrimoraPlugin.Log.LogDebug($"[MapNodeManager.DoMoveToNewNode][Postfix] Finished setting currentNodeId");
			yield break;
		}

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

			GrimoraPlugin.Log.LogDebug($"[SetActiveNode] Setting active node to true");
			newActiveNode.SetActive(true);
			ChessboardNavGrid.instance.SetPlayerAdjacentNodesActive();

			return false;
		}
	}
}