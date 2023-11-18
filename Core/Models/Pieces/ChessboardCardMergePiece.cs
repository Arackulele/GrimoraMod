namespace GrimoraMod;

public class ChessboardCardMergePiece : ChessboardPieceExt
{
	// Give a random positive sigil to your Card, similar to campfire, has an increasing chance the card will die
	public ChessboardCardMergePiece()
	{
		NodeData = new CardMergeNodeData();
	}
}
