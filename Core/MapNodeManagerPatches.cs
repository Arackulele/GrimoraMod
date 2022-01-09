using System.Collections;
using DiskCardGame;
using HarmonyLib;

namespace GrimoraMod
{
	// [HarmonyPatch(typeof(MapNodeManager))]
	public class MapNodeManagerPatches
	{
		[HarmonyPrefix, HarmonyPatch(nameof(MapNodeManager.DoMoveToNewNode))]
		public static void Prefix(ref MapNodeManager __instance, out MapNodeManager __state)
		{
			__state = __instance;
		}

		[HarmonyPostfix, HarmonyPatch(nameof(MapNodeManager.DoMoveToNewNode))]
		public static IEnumerator Postfix(MapNodeManager __state, MapNode newNode)
		{
			GrimoraPlugin.Log.LogDebug($"[MapNodeManager.DoMoveToNewNode][Postfix] State [{__state}] MapNode [{newNode}]");
			__state.MovingNodes = true;
			__state.SetAllNodesInteractable(false);
			__state.transitioningGridY = newNode.Data.gridY;
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
				yield return newNode.OnArriveAtNode();
			}
			
			ViewManager.Instance.Controller.LockState = ViewLockState.Unlocked;
			GrimoraPlugin.Log.LogDebug($"[MapNodeManager.DoMoveToNewNode][Postfix] No longer MovingNodes");
			__state.MovingNodes = false;
			CustomCoroutine.WaitOnConditionThenExecute(
				() => !GameFlowManager.Instance.Transitioning,
				delegate { RunState.Run.currentNodeId = newNode.nodeId; }
			);
			GrimoraPlugin.Log.LogDebug($"[MapNodeManager.DoMoveToNewNode][Postfix] Finished setting currentNodeId");
			yield break;
		}
	}
}