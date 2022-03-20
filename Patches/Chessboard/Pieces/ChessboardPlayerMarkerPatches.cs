using DiskCardGame;
using HarmonyLib;
using UnityEngine;

namespace GrimoraMod;

[HarmonyPatch(typeof(ChessboardPlayerMarker))]
public class ChessboardPlayerMarkerPatches
{

	[HarmonyPrefix, HarmonyPatch(nameof(ChessboardPlayerMarker.ManagedUpdate))]
	public static bool ChangeWhatInputButtonsDo(ChessboardPlayerMarker __instance)
	{
		if (!__instance.Hidden && !MapNodeManager.Instance.MovingNodes && !GameFlowManager.Instance.Transitioning)
		{
			if (Input.GetKeyDown(KeyCode.LeftArrow))
			{
				__instance.MoveToZone(LookDirection.West);
			}
			if (Input.GetKeyDown(KeyCode.RightArrow))
			{
				__instance.MoveToZone(LookDirection.East);
			}
			if (Input.GetKeyDown(KeyCode.UpArrow))
			{
				__instance.MoveToZone(LookDirection.North);
			}
			if (Input.GetKeyDown(KeyCode.DownArrow))
			{
				__instance.MoveToZone(LookDirection.South);
			}
		}

		return false;
	}
}
