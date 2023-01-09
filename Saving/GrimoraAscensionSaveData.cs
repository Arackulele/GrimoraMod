using DiskCardGame;

namespace GrimoraMod.Saving;

public class GrimoraAscensionSaveData : AscensionSaveData
{
	public static bool RunExists
	{
		get
		{
			return GrimoraSaveManager.CurrentSaveFile.AscensionSaveData.currentRun != null && GrimoraSaveManager.CurrentSaveFile.AscensionSaveData.currentRun.playerLives > 0; 
		}
	}
	
	public new void Initialize()
	{
		this.dialogueData = new DialogueEventsData();
		this.progressionData = new ProgressionData();
		this.progressionData.learnedAbilities.AddRange(AscensionStoryAndProgressFlags.DEFAULT_LEARNED_ABILITIES);
		this.itemUnlockEvents = new List<StoryEvent>();
		this.stats = new AscensionStatsData();
		this.stats.Initialize();
		this.activeChallenges = new List<AscensionChallenge>();
		this.conqueredChallenges = new List<AscensionChallenge>();
		this.challengeLevel = 1;
		this.conqueredStarterDecks = new List<string>();
		this.currentStarterDeck = "Vanilla";
		this.oilPaintingState = new OilPaintingPuzzle.SaveState();
		this.oilPaintingState.isAscension = true;
		this.playerAvatarHead = CompositeFigurine.RandomType();
		this.playerAvatarBody = CompositeFigurine.RandomType();
		this.playerAvatarArms = CompositeFigurine.RandomType();
	}
	
	public new void NewRun(List<CardInfo> starterDeck)
	{
		GrimoraPlugin.Log.LogInfo("[GrimoraAscensionSaveData] NewRun");
		this.currentRunSeed = Environment.TickCount;
		this.currentOuroborosDeaths = 0;
		
		GrimoraRunState grimoraRunState = new GrimoraRunState();
		this.currentRun = grimoraRunState;
		grimoraRunState.Initialize();
		grimoraRunState.playerLives = 1;
		
		this.RollCurrentRunRegionOrder();
		this.oilPaintingState.TryAdvanceRewardIndex();
		this.oilPaintingState.puzzleSolution = OilPaintingPuzzle.GenerateSolution(true);
		
		this.currentRun.consumables.Clear();
		this.currentRun.consumables.Add("Pliers");
		
		foreach (CardInfo cardInfo in starterDeck)
		{
			this.currentRun.playerDeck.AddCard(CardLoader.GetCardByName(cardInfo.name));
		}
	}
}
