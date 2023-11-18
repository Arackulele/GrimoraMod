namespace GrimoraMod;

public class ChessboardGoatEyePiece : ChessboardPieceExt
{
	// Give a random positive sigil to your Card, similar to campfire, has an increasing chance the card will die
	public ChessboardGoatEyePiece()
	{
		NodeData = new GoatEyeNodeData();
		newYPosition = 1.275f;
	}
}
