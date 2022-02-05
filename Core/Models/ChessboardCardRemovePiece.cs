using DiskCardGame;

namespace GrimoraMod;

public class ChessboardCardRemovePiece : ChessboardPieceExt
{
	public ChessboardCardRemovePiece()
	{
		base.NodeData = new CardRemoveNodeData();
		newYPosition = 1.4f;
		newScale = 0.25f;
	}

}
