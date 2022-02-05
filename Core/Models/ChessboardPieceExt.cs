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
		base.transform.position = ChessboardNavGrid.instance.zones[gridXPos, gridYPos].transform.position;
		if (newYPosition != 0f)
		{
			Vector3 copy = base.transform.localPosition;
			base.transform.localPosition = new Vector3(copy.x, newYPosition, copy.z);
		}

		if (newScale != 0f)
		{
			base.transform.localScale = new Vector3(newScale, newScale, newScale);
		}

		if (base.GetType() == typeof(ChessboardBlockerPieceExt))
		{
			ChessboardNavGrid.instance.zones[gridXPos, gridYPos]
				.GetComponent<ChessboardMapNode>()
				.gameObject.SetActive(false);
		}
		else
		{
			ChessboardNavGrid.instance.zones[gridXPos, gridYPos].GetComponent<ChessboardMapNode>().OccupyingPiece = this;
		}
		base.transform.rotation = Quaternion.Euler(0f, 0f, 0f);
	}
	
	public override void OnPlayerInteracted()
	{
		StartCoroutine(StartSpecialNodeSequence());
	}

	private IEnumerator StartSpecialNodeSequence()
	{
		GrimoraPlugin.Log.LogDebug($"[StartSpecialNodeSequence] Piece [{base.name}] Node [{GetType()}]");
		ConfigHelper.Instance.AddPieceToRemovedPiecesConfig(base.name);

		MapNodeManager.Instance.SetAllNodesInteractable(false);

		ViewManager.Instance.Controller.LockState = ViewLockState.Locked;

		PlayerMarker.Instance.Anim.Play("knock against", 0, 0f);
		yield return new WaitForSeconds(0.05f);
		
		ViewManager.Instance.Controller.LockState = ViewLockState.Unlocked;

		GameFlowManager.Instance.TransitionToGameState(GameState.SpecialCardSequence, base.NodeData);
	}
}
