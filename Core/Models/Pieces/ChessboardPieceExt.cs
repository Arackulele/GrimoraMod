using System.Collections;
using DiskCardGame;
using GrimoraMod.Saving;
using UnityEngine;

namespace GrimoraMod;

public class ChessboardPieceExt : ChessboardPiece
{
	public float newYPosition;
	public float newScale;

	private void Start()
	{
		var navZone = ChessboardNavGrid.instance.zones[gridXPos, gridYPos];
		transform.position = navZone.transform.position;
		if (newYPosition != 0f)
		{
			Vector3 copy = transform.localPosition;
			transform.localPosition = new Vector3(copy.x, newYPosition, copy.z);
		}

		if (newScale != 0f)
		{
			transform.localScale = new Vector3(newScale, newScale, newScale);
		}

		if (GetType() == typeof(ChessboardBlockerPieceExt))
		{
			ChessboardNavGrid.instance.zones[gridXPos, gridYPos]
				.GetComponent<ChessboardMapNode>()
				.gameObject.SetActive(false);
		}
		else
		{
			navZone.GetComponent<ChessboardMapNode>().OccupyingPiece = this;
			var zone = ChessboardNavGrid.instance.zones[3, 3];
			TurnToFacePoint(zone.transform.position, 0.1f);
		}
	}

	public override void OnPlayerInteracted()
	{
		StartCoroutine(StartSpecialNodeSequence());
	}

	private IEnumerator StartSpecialNodeSequence()
	{
		GrimoraPlugin.Log.LogDebug($"[StartSpecialNodeSequence] Piece [{name}] Node [{GetType()}]");
		GrimoraRunState.CurrentRun.PiecesRemovedFromBoard.Add(name);

		MapNodeManager.Instance.SetAllNodesInteractable(false);

		ViewManager.Instance.SetViewLocked();

		yield return ChessboardEnemyManager.Instance.KnockPiecesTogether(PlayerMarker.Instance, this, 0.25f);
		yield return new WaitForSeconds(0.05f);

		ViewManager.Instance.SetViewUnlocked();

		GameFlowManager.Instance.TransitionToGameState(GameState.SpecialCardSequence, NodeData);
	}
}
