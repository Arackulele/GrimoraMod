using GrimoraMod.Saving;
namespace GrimoraMod;
using InscryptionAPI.Helpers;
using InscryptionAPI.Helpers.Extensions;

public class ChessboardBlockerPieceExt : ChessboardPieceExt
{
	public ChessboardBlockerPieceExt()
	{
		if (GrimoraRunState.CurrentRun.regionTier == 3)
		{
			if (this.gameObject.FindChild("EyeRight") != null) newYPosition = 1.25f;
		}
	}
}
