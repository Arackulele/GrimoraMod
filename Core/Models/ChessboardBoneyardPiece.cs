﻿using DiskCardGame;

namespace GrimoraMod;

public class ChessboardBoneyardPiece : ChessboardPieceExt
{
	// Bury one of your cards to give it Brittle and halve its cost
	public ChessboardBoneyardPiece()
	{
		NodeData = new CardStatBoostNodeData();
		newScale = 1.25f;
	}
	
}
