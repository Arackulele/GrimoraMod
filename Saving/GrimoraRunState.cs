using DiskCardGame;

namespace GrimoraMod.Saving;

public class GrimoraRunState : RunState
{
	public int gridX;
	public int gridY;
	public List<int> removedPieces;
	
	public new void Initialize()
	{
		this.gridX = 0;
		this.gridY = 0;
		this.removedPieces = new List<int>();
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
		
		this.riggedDraws = new List<string>();
		this.storyEventsCompleted = new List<StoryEvent>();
		this.consumables = new List<string>();
		this.totems = new List<TotemDefinition>();
		this.totemTops = new List<Tribe>();
		this.totemBottoms = new List<Ability>();
	}
}
