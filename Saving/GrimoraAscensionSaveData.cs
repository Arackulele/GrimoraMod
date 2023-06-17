using BepInEx.Bootstrap;
using DiskCardGame;

namespace GrimoraMod.Saving;

public class GrimoraAscensionSaveData : AscensionSaveData
{
	public static CardInfo GetRandomCardBones(int minbones, int maxbones)
	{
		GrimoraPlugin.Log.LogDebug("Start Get Random Card");
		List<CardInfo> playablecards = new List<CardInfo>(GrimoraPlugin.AllPlayableGrimoraModCards);
		List<CardInfo> validcards = new List<CardInfo>();
		GrimoraPlugin.Log.LogDebug("Added Lists");
		foreach (var i in playablecards)
			{
			GrimoraPlugin.Log.LogDebug("Looping through all cards");
			if (i.BonesCost > maxbones | i.BonesCost < minbones | i.EnergyCost > 0) { }
				else validcards.Add(i);
			}
		GrimoraPlugin.Log.LogDebug("Returning Item");
		return validcards.GetRandomItem();
	}

	public static CardInfo GetRandomCardEnergy(int minbones, int maxbones)
	{
		GrimoraPlugin.Log.LogDebug("Start Get Random Card");
		List<CardInfo> playablecards = new List<CardInfo>(GrimoraPlugin.AllPlayableGrimoraModCards);
		List<CardInfo> validcards = new List<CardInfo>();
		GrimoraPlugin.Log.LogDebug("Added Lists");
		foreach (var i in playablecards)
			{
			GrimoraPlugin.Log.LogDebug("Looping through all cards");
			if (i.EnergyCost > maxbones | i.EnergyCost < minbones | i.BonesCost > 0) { }
				else validcards.Add(i);
			}
		GrimoraPlugin.Log.LogDebug("Returning Item");
		return validcards.GetRandomItem();

	}



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

		//AscensionSaveData.Data.challengeLevel = 15;
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
		this.currentRun.consumables.Add(GrimoraPlugin.AllGrimoraItems[5].name);

		if (starterDeck.Count() > 4 )
		{
			foreach (CardInfo cardInfo in starterDeck)
			this.currentRun.playerDeck.AddCard(CardLoader.GetCardByName(cardInfo.name));
		}
		else
		{
			if (Chainloader.PluginInfos.ContainsKey("arackulele.inscryption._grimoramodextracards"))
			{

								List<CardInfo> EnergyCreation = new List<CardInfo> {
				GrimoraPlugin.NameWillOTheWisp.GetCardInfo(), GrimoraPlugin.NameWillOTheWisp.GetCardInfo(), GrimoraPlugin.NameWillOTheWisp.GetCardInfo(),
				GrimoraPlugin.NameMoroi.GetCardInfo(), GrimoraPlugin.NameMoroi.GetCardInfo(),
				GrimoraPlugin.NameDalgyal.GetCardInfo(),
				GrimoraPlugin.NameSluagh.GetCardInfo(),
				};

				List<CardInfo> BoneCreation = new List<CardInfo> {
					GrimoraPlugin.NameBonepile.GetCardInfo(), GrimoraPlugin.NameBonepile.GetCardInfo(),
					GrimoraPlugin.NameGravedigger.GetCardInfo(), GrimoraPlugin.NameGravedigger.GetCardInfo(),
					GrimoraPlugin.NameDraugr.GetCardInfo(), GrimoraPlugin.NameDraugr.GetCardInfo(),
					GrimoraPlugin.NameCrossBones.GetCardInfo(), GrimoraPlugin.NameCrossBones.GetCardInfo(),
					GrimoraPlugin.NameGratefulDead.GetCardInfo(), GrimoraPlugin.NameGratefulDead.GetCardInfo(),
					GrimoraPlugin.NameGhostShip.GetCardInfo(),
					GrimoraPlugin.NameNecromancer.GetCardInfo(),
					GrimoraPlugin.NameBoneLordsHorn.GetCardInfo(),
					GrimoraPlugin.NameSporedigger.GetCardInfo(),
				};

	bool IsEnergy = false;

				if (UnityEngine.Random.Range(0, 10) < 4)
				{

					List<string> randomloser = StarterDecks.loosingdecks.GetRandomItem();

						foreach (var i in randomloser)
						this.currentRun.playerDeck.AddCard(CardLoader.GetCardByName(i));

				}
				else {
				if (UnityEngine.Random.Range(0, 10) > 6) IsEnergy = true;

				if (IsEnergy)
				{
					GrimoraPlugin.Log.LogDebug("Energy Pre adding Card 1");
					this.currentRun.playerDeck.AddCard(EnergyCreation.GetRandomItem());
					this.currentRun.playerDeck.AddCard(EnergyCreation.GetRandomItem());
					GrimoraPlugin.Log.LogDebug("Energy Pre adding Card 3");

					if (UnityEngine.Random.Range(0, 10) > 5) this.currentRun.playerDeck.AddCard(GetRandomCardBones(1, 6));
					else this.currentRun.playerDeck.AddCard(GetRandomCardEnergy(2, 5));

					GrimoraPlugin.Log.LogDebug("Energy Pre adding Card 4 & 5");
					this.currentRun.playerDeck.AddCard(GetRandomCardEnergy(1, 6));
					this.currentRun.playerDeck.AddCard(GetRandomCardEnergy(3, 6));
				}
				else
				{
					GrimoraPlugin.Log.LogDebug("Bone Pre adding Card 1");
					this.currentRun.playerDeck.AddCard(BoneCreation.GetRandomItem());
					this.currentRun.playerDeck.AddCard(BoneCreation.GetRandomItem());
					GrimoraPlugin.Log.LogDebug("Bone Pre adding Card 3");

					if (UnityEngine.Random.Range(0, 10) > 5) this.currentRun.playerDeck.AddCard(GetRandomCardEnergy(2, 5));
					else this.currentRun.playerDeck.AddCard(GetRandomCardBones(2, 5));

					GrimoraPlugin.Log.LogDebug("Bone Pre adding Card 4 & 5");
					this.currentRun.playerDeck.AddCard(GetRandomCardBones(3, 9));
					this.currentRun.playerDeck.AddCard(GetRandomCardBones(5, 20));
				}
				}
			}
		}
	}
}
