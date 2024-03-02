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
			if (ConfigHelper.Instance.InputType == 0)
			{
				MoveWasdInput(__instance);
			}
			else
			{
				MoveArrowKeyInput(__instance);
			}
		}

		return false;
	}

	private static void MoveArrowKeyInput(ChessboardPlayerMarker __instance)
	{
		if (Input.GetKeyDown(KeyCode.A))
		{
			__instance.MoveToZone(LookDirection.West);
		}
		if (Input.GetKeyDown(KeyCode.D))
		{
			__instance.MoveToZone(LookDirection.East);
		}
		if (Input.GetKeyDown(KeyCode.W))
		{
			__instance.MoveToZone(LookDirection.North);
		}
		if (Input.GetKeyDown(KeyCode.S))
		{
			__instance.MoveToZone(LookDirection.South);
		}
	}
	
	private static void MoveWasdInput(ChessboardPlayerMarker __instance)
	{
		if (Input.GetKeyDown(KeyCode.J) || Input.GetKeyDown(KeyCode.F))
		{
			__instance.MoveToZone(LookDirection.West);
		}
		if (Input.GetKeyDown(KeyCode.L) || Input.GetKeyDown(KeyCode.H))
		{
			__instance.MoveToZone(LookDirection.East);
		}
		if (Input.GetKeyDown(KeyCode.I) || Input.GetKeyDown(KeyCode.T))
		{
			__instance.MoveToZone(LookDirection.North);
		}
		if (Input.GetKeyDown(KeyCode.K) || Input.GetKeyDown(KeyCode.G))
		{
			__instance.MoveToZone(LookDirection.South);
		}
	}
}
