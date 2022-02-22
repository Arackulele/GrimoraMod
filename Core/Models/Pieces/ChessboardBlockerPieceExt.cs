namespace GrimoraMod;

public class ChessboardBlockerPieceExt : ChessboardPieceExt
{
	public ChessboardBlockerPieceExt()
	{
		if (ConfigHelper.Instance.BossesDefeated == 3)
		{
			newYPosition = 1.25f;
		}
	}
}
