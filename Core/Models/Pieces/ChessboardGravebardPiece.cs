namespace GrimoraMod;

public class ChessboardGravebardPiece : ChessboardPieceExt
{
	// Give a random positive sigil to your Card, similar to campfire, has an increasing chance the card will die
	public ChessboardGravebardPiece()
	{
		NodeData = new GravebardCampNodeData();
	}
}
