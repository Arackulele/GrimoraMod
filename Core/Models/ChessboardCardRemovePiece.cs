using System.Collections;
using DiskCardGame;
using UnityEngine;
using static GrimoraMod.GrimoraPlugin;

namespace GrimoraMod;

public class ChessboardCardRemovePiece : ChessboardPieceExt
{
	public ChessboardCardRemovePiece()
	{
		newYPosition = 1.4f;
		newScale = 0.25f;
	}

	private void Awake()
	{
		base.NodeData = new CardRemoveNodeData();
	}

	public override void OnPlayerInteracted()
	{
		StartCoroutine(OpenSequence());
	}

	private IEnumerator OpenSequence()
	{
		Log.LogDebug($"[ChessboardCardRemovalPiece.OpenSequence] Piece [{base.name}]");
		ConfigHelper.Instance.AddPieceToRemovedPiecesConfig(base.name);

		MapNodeManager.Instance.SetAllNodesInteractable(false);

		ViewManager.Instance.Controller.LockState = ViewLockState.Locked;

		PlayerMarker.Instance.Anim.Play("knock against", 0, 0f);
		yield return new WaitForSeconds(0.05f);
		
		ViewManager.Instance.Controller.LockState = ViewLockState.Unlocked;

		GameFlowManager.Instance.TransitionToGameState(GameState.SpecialCardSequence, base.NodeData);
	}
	
}
