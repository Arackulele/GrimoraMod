namespace GrimoraMod;

public class ChessboardElectricChairPiece : ChessboardPieceExt
{
	// Give a random positive sigil to your Card, similar to campfire, has an increasing chance the card will die
	public ChessboardElectricChairPiece()
	{
		NodeData = new ElectricChairNodeData();
		newYPosition = 1.2f;
	}
}
