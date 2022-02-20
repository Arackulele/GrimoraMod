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
	}
}

[HarmonyPatch(typeof(ChessboardMapNode), nameof(ChessboardMapNode.OnArriveAtNode))]
public class GoatEyePatch
{

	private static readonly List<System.Type> PiecesToNotRotate = new()
	{
		typeof(ChessboardPlayerMarker),
		typeof(ChessboardGoatEyePiece)
	};

	[HarmonyPostfix]
	public static IEnumerator EyeFollowsPlayerLikeMario(IEnumerator enumerator)
	{
		yield return enumerator;

		Vector3 playerPos = PlayerMarker.Instance.transform.position;
		foreach (var goatEyePiece in Object.FindObjectsOfType<ChessboardGoatEyePiece>())
		{
			TurnToFacePoint(goatEyePiece.gameObject, playerPos, 0.1f);
		}

		if (ConfigHelper.Instance.BossesDefeated == 3)
		{
			foreach (var blocker in Object.FindObjectsOfType<ChessboardBlockerPieceExt>())
			{
				TurnToFacePoint(blocker.gameObject, playerPos, 0.1f);
			}
		}
		
		for (int i = 0; i < 8; i++)
		{
			var node = ChessboardNavGrid.instance.zones[i, GrimoraSaveData.Data.gridY].GetComponent<ChessboardMapNode>();
			if (node.OccupyingPiece is null) continue;
			if (!node.OccupyingPiece.name.Contains("Boss") || !PiecesToNotRotate.Contains(node.OccupyingPiece.GetType()))
			{
				node.OccupyingPiece.TurnToFacePoint(PlayerMarker.Instance.transform.position, 0.1f);
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
