using GrimoraMod.Saving;

namespace GrimoraMod;

public class ChessboardBlockerPieceExt : ChessboardPieceExt
{
	public ChessboardBlockerPieceExt()
	{
		if (GrimoraRunState.CurrentRun.regionTier == 3)
		{
			newYPosition = 1.25f;
		}
	}
}
