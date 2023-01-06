using DiskCardGame;

namespace GrimoraMod.Saving;

public class GrimoraRunState : RunState
{
	public static GrimoraRunState CurrentRun => GrimoraSaveManager.CurrentSaveFile.CurrentRun;
	
	public GrimoraSaveData boardData;
	
	public new void Initialize()
	{
		this.playerLives = 1;
		this.regionOrder = new int[]
		{
			0,
			1, 
			2
		}; // TODO: Change to use regions of the bosses!
		this.skullTeeth = 0;
		
		this.playerDeck = new DeckInfo();
		this.playerDeck.AddCard(CardLoader.GetCardByName("Gravedigger"));
		this.playerDeck.AddCard(CardLoader.GetCardByName("Gravedigger"));
		this.playerDeck.AddCard(CardLoader.GetCardByName("Gravedigger"));
		this.playerDeck.AddCard(CardLoader.GetCardByName("FrankNStein"));
		this.playerDeck.AddCard(CardLoader.GetCardByName("FrankNStein"));

		this.consumables = new List<string>();
		this.consumables.Add("FishHook");
		
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
}
