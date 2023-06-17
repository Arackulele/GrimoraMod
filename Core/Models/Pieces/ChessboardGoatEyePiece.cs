using System.Collections;
using DiskCardGame;
using GrimoraMod.Saving;
using HarmonyLib;
using InscryptionAPI.Helpers.Extensions;
using Pixelplacement;
using Sirenix.Utilities;
using UnityEngine;
using static GrimoraMod.BlueprintUtils;

namespace GrimoraMod;

public class ChessboardGoatEyePiece : ChessboardEnemyPiece
{
	public int Random;
	public override void OnPlayerInteracted()
	{
		Random = UnityEngine.Random.RandomRangeInt(0, 4);
		StartCoroutine(AnkhGuardPreStartDialogue());

	}


	public IEnumerator AnkhGuardPreStartDialogue()
	{
		Singleton<MapNodeManager>.Instance.SetAllNodesInteractable(nodesInteractable: false);
		Singleton<ViewManager>.Instance.Controller.LockState = ViewLockState.Locked;
		switch (Random)
		{


			default:
			case 0:

					yield return TextDisplayer.Instance.ShowUntilInput(
					$"An ancient Guard stands in front of you, noticing you are trying to sneak past."
					);

					yield return TextDisplayer.Instance.ShowUntilInput(
					$"You dare enter my {"TOMB".Red()}!Prepare for death."
					);

				break;

			case 1:

				yield return TextDisplayer.Instance.ShowUntilInput(
				$"An imposing Man holding a sculpture of an Ankh blocks the way."
				);

				yield return TextDisplayer.Instance.ShowUntilInput(
				$"My Ankh gives me life, you shall not rob my {"TOMB".Red()}."
				);

				break;

			case 2:

				yield return TextDisplayer.Instance.ShowUntilInput(
				$"An old Pharaoh climbs out of his Sarcophagus."
				);

				yield return TextDisplayer.Instance.ShowUntilInput(
				$"I have been called to protect my {"TOMB".Red()}."
				);

				break;

			case 3:

				yield return TextDisplayer.Instance.ShowUntilInput(
				$"A holy figure looms over you."
				);

				yield return TextDisplayer.Instance.ShowUntilInput(
				$"Your Fate is sealed, my {"TOMB".Red()} shall not be disturbed."
				);

				break;
		}
		Singleton<ViewManager>.Instance.Controller.LockState = ViewLockState.Unlocked;
		Singleton<ChessboardEnemyManager>.Instance.StartCombatWithEnemy(this, playerStarted: true);

		yield break;
	}

}

[HarmonyPatch(typeof(ChessboardMapNode))]
public class GoatEyePatch
{

	private static readonly List<System.Type> PiecesToNotRotate = new()
	{
		typeof(ChessboardPlayerMarker),
		typeof(ChessboardGoatEyePiece)
	};

	[HarmonyPostfix, HarmonyPatch(nameof(ChessboardMapNode.OnArriveAtNode))]
	public static IEnumerator EyeFollowsPlayerLikeMario(IEnumerator enumerator)
	{
		yield return enumerator;

		Vector3 playerPos = PlayerMarker.Instance.transform.position;

		if (GrimoraRunState.CurrentRun.regionTier == 3)
		{
			foreach (var blocker in UnityObject.FindObjectsOfType<ChessboardBlockerPieceExt>())
			{
				if (blocker.gameObject.FindChild("EyeRight") != null) TurnToFacePoint(blocker.gameObject, playerPos, 0.1f);

			}
		}
		
		for (int i = 0; i < 8; i++)
		{
			var node = ChessboardNavGrid.instance.zones[i, GrimoraSaveData.Data.gridY].GetComponent<ChessboardMapNode>();
			if (node.OccupyingPiece.SafeIsUnityNull()) continue;
			if (!node.OccupyingPiece.name.Contains("Boss") && !PiecesToNotRotate.Contains(node.OccupyingPiece.GetType()))
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
