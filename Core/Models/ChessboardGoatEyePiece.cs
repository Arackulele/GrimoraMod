using System.Collections;
using DiskCardGame;
using HarmonyLib;
using Pixelplacement;
using UnityEngine;

namespace GrimoraMod;

public class ChessboardGoatEyePiece : ChessboardPieceExt
{
	public ChessboardGoatEyePiece()
	{
		newYPosition = 1.275f;
		newScale = 0.4f;
	}
}

[HarmonyPatch(typeof(ChessboardMapNode), nameof(ChessboardMapNode.OnArriveAtNode))]
public class GoatEyePatch
{
	[HarmonyPostfix]
	public static IEnumerator EyeFollowsPlayerLikeMario(IEnumerator enumerator)
	{
		yield return enumerator;

		Vector3 playerPos = Object.FindObjectOfType<PlayerMarker>().transform.position;
		// GrimoraPlugin.Log.LogDebug($"Player position [{playerPos}]");
		foreach (var goatEyePiece in Object.FindObjectsOfType<ChessboardGoatEyePiece>())
		{
			// Log.LogDebug($"[GoatEyePatch] Rotating [{goatEyePiece}]");
			TurnToFacePoint(goatEyePiece.gameObject, playerPos, 0.1f);
		}
	}
	
	public static void TurnToFacePoint(GameObject gameObject, Vector3 point, float time)
	{
		if (gameObject.activeSelf)
		{
			Quaternion rotation = gameObject.transform.rotation;
			gameObject.transform.LookAt(point);
			Quaternion rotation2 = gameObject.transform.rotation;
			rotation2.eulerAngles = new Vector3(0f, rotation2.eulerAngles.y - 90f, 0f);
			gameObject.transform.rotation = rotation;
			Tween.Rotation(gameObject.transform, rotation2, time, 0f, Tween.EaseInOut);
		}
	}
}
