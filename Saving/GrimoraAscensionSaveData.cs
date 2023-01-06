using DiskCardGame;

namespace GrimoraMod.Saving;

public class GrimoraAscensionSaveData : AscensionSaveData
{
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
		this.currentRunSeed = Environment.TickCount;
		this.currentOuroborosDeaths = 0;
		
		GrimoraRunState grimoraRunState = new GrimoraRunState();
		this.currentRun = grimoraRunState;
		grimoraRunState.Initialize();
		this.RollCurrentRunRegionOrder();
		this.oilPaintingState.TryAdvanceRewardIndex();
		this.oilPaintingState.puzzleSolution = OilPaintingPuzzle.GenerateSolution(true);
		this.currentRun.playerDeck = new DeckInfo();
		foreach (CardInfo cardInfo in starterDeck)
		{
			this.currentRun.playerDeck.AddCard(CardLoader.GetCardByName(cardInfo.name));
		}
		if (this.numRunsSinceReachedFirstBoss == 0)
		{
			this.currentRun.playerDeck.AddCard(CardLoader.GetCardByName("PeltHare"));
			this.currentRun.playerDeck.AddCard(CardLoader.GetCardByName("PeltHare"));
		}
		else if (this.numRunsSinceReachedFirstBoss == 1)
		{
			this.currentRun.playerDeck.AddCard(CardLoader.GetCardByName("Opossum"));
			this.currentRun.playerDeck.AddCard(CardLoader.GetCardByName("PeltHare"));
		}
		else if (this.numRunsSinceReachedFirstBoss > 1)
		{
			this.currentRun.playerDeck.AddCard(CardLoader.GetCardByName("Opossum"));
			this.currentRun.playerDeck.AddCard(CardLoader.GetCardByName("RingWorm"));
		}
		this.currentRun.consumables.Add("SquirrelBottle");
		if (this.GetNumChallengesOfTypeActive(AscensionChallenge.LessConsumables) < 2)
		{
			this.currentRun.consumables.Add("Pliers");
		}
		if (!this.ChallengeIsActive(AscensionChallenge.NoHook))
		{
			if (this.currentRun.consumables.Count == this.currentRun.MaxConsumables)
			{
				this.currentRun.consumables.RemoveAt(this.currentRun.consumables.Count - 1);
			}
			this.currentRun.consumables.Add("FishHook");
		}
		if (this.ChallengeIsActive(AscensionChallenge.LessLives))
		{
			this.currentRun.maxPlayerLives = (this.currentRun.playerLives = 1);
		}
	}
}
