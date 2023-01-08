using DiskCardGame;

namespace GrimoraMod.Saving;

public class GrimoraRunState : RunState
{
	public static GrimoraRunState CurrentRun
	{
		get { return SaveFile.IsAscension ? (GrimoraRunState)GrimoraSaveManager.CurrentSaveFile.AscensionSaveData.currentRun : GrimoraSaveManager.CurrentSaveFile.CurrentRun; }
	}

	public GrimoraSaveData boardData;

	public List<List<int>> CurrentChessboard;
	public List<string> PiecesRemovedFromBoard = new List<string>();
	
	public new void Initialize()
	{
		GrimoraPlugin.Log.LogDebug($"[GrimoraChessboard] Initialize");
		this.playerLives = 1;
		this.regionTier = -1;
		this.regionOrder = new int[]
		{
			0,
			1, 
			2
		}; // TODO: Change to use regions of the bosses!
		this.skullTeeth = 0;
		
		this.playerDeck = new DeckInfo();

		this.consumables = new List<string>();
		
		this.boardData = new GrimoraSaveData();
		this.boardData.gridX = 0;
		this.boardData.gridY = 0;
		this.boardData.removedPieces = new List<int>();
		this.boardData.deck = this.playerDeck; // Yuck. But Reference just works 

		this.riggedDraws = new List<string>();
		this.storyEventsCompleted = new List<StoryEvent>();
		this.totems = new List<TotemDefinition>();
		this.totemTops = new List<Tribe>();
		this.totemBottoms = new List<Ability>();
	}

	public void NewStandardGame()
	{
		GrimoraPlugin.Log.LogDebug($"[GrimoraChessboard] NewStandardGame");
		this.playerLives = 1;
		this.skullTeeth = 0;
		this.regionTier = 0;
		
		this.playerDeck.Cards.Clear();
		this.playerDeck.AddCard(GrimoraPlugin.NameBonepile.GetCardInfo());
		this.playerDeck.AddCard(GrimoraPlugin.NameGravedigger.GetCardInfo());
		this.playerDeck.AddCard(GrimoraPlugin.NameGravedigger.GetCardInfo());
		this.playerDeck.AddCard(GrimoraPlugin.NameFranknstein.GetCardInfo());
		this.playerDeck.AddCard(GrimoraPlugin.NameZombie.GetCardInfo());

		this.consumables.Clear();
		this.consumables.Add("FishHook");
		
		this.boardData.gridX = 0;
		this.boardData.gridY = 0;
		this.boardData.removedPieces.Clear();
		
		PiecesRemovedFromBoard.Clear();
		CurrentChessboard = null;
	}

	public GrimoraChessboard SetupNewStandardRegion()
	{
		if (regionTier < 0)
		{
			GrimoraPlugin.Log.LogDebug($"[GrimoraChessboard] Setting up a new game");
			NewStandardGame();
		}
		
		GrimoraChessboard generateChessboard = ChessboardMapExt.Instance.GenerateChessboard(regionTier);
		CurrentChessboard = generateChessboard.Export();

		return generateChessboard;
	}
}
