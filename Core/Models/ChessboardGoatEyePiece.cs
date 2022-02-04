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

		// GrimoraPlugin.Log.LogDebug($"[GoatEye] Getting player marker");
		Vector3 playerPos = Object.FindObjectOfType<PlayerMarker>().transform.position;
		foreach (var goatEyePiece in Object.FindObjectsOfType<ChessboardGoatEyePiece>())
		{
			// Log.LogDebug($"[GoatEyePatch] Rotating [{goatEyePiece}]");
			TurnToFacePoint(goatEyePiece.gameObject, playerPos, 0.1f);
		}

		// GrimoraPlugin.Log.LogDebug($"[GoatEyePatch] Checking if bosses defeated is 3");
		if (ConfigHelper.Instance.BossesDefeated == 3)
		{
			// GrimoraPlugin.Log.LogDebug($"[GoatEyePatch] Rotating all skulls");
			foreach (var blocker in Object.FindObjectsOfType<ChessboardBlockerPieceExt>())
			{
				TurnToFacePoint(blocker.gameObject, playerPos, 0.1f);
			}
		}
	}

	public static void TurnToFacePoint(GameObject gameObject, Vector3 point, float time)
	{
		if (gameObject.activeSelf)
		{
			float yRotation = gameObject.GetComponent<ChessboardBlockerPieceExt>() ? 180f : 90f;
			Quaternion rotation = gameObject.transform.rotation;
			gameObject.transform.LookAt(point);
			Quaternion rotation2 = gameObject.transform.rotation;
			rotation2.eulerAngles = new Vector3(0f, rotation2.eulerAngles.y - yRotation, 0f);
			gameObject.transform.rotation = rotation;
			Tween.Rotation(gameObject.transform, rotation2, time, 0f, Tween.EaseInOut);
		}
	}
}
