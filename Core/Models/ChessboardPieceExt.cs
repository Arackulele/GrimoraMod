using System.Collections;
using DiskCardGame;
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
		}
	}

	public override void OnPlayerInteracted()
	{
		StartCoroutine(StartSpecialNodeSequence());
	}

	private IEnumerator StartSpecialNodeSequence()
	{
		GrimoraPlugin.Log.LogDebug($"[StartSpecialNodeSequence] Piece [{name}] Node [{GetType()}]");
		ConfigHelper.Instance.AddPieceToRemovedPiecesConfig(name);

		MapNodeManager.Instance.SetAllNodesInteractable(false);

		ViewManager.Instance.Controller.LockState = ViewLockState.Locked;

		yield return ChessboardEnemyManager.Instance.KnockPiecesTogether(PlayerMarker.Instance, this, 0.25f);
		yield return new WaitForSeconds(0.05f);

		ViewManager.Instance.Controller.LockState = ViewLockState.Unlocked;

		GameFlowManager.Instance.TransitionToGameState(GameState.SpecialCardSequence, NodeData);
	}
}
