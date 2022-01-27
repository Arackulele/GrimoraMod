using APIPlugin;
using BepInEx;
using BepInEx.Configuration;
using BepInEx.Logging;
using DiskCardGame;
using HarmonyLib;
using UnityEngine;

namespace GrimoraMod;

[BepInDependency("cyantist.inscryption.api")]
[BepInPlugin(PluginGuid, PluginName, PluginVersion)]
public partial class GrimoraPlugin : BaseUnityPlugin
{
	public const string PluginGuid = "arackulele.inscryption.grimoramod";
	public const string PluginName = "GrimoraMod";
	private const string PluginVersion = "2.4.1";

	internal static ManualLogSource Log;

	private static Harmony _harmony;

	public static UnityEngine.Object[] AllAssets;
	public static UnityEngine.Sprite[] AllSpriteAssets;
	public static UnityEngine.Texture[] AllAbilityAssets;


	private static readonly List<StoryEvent> StoryEventsToBeCompleteBeforeStarting = new()
	{
		StoryEvent.BasicTutorialCompleted, StoryEvent.TutorialRunCompleted, StoryEvent.BonesTutorialCompleted,
		StoryEvent.TutorialRun2Completed, StoryEvent.TutorialRun3Completed
	};


	private void Awake()
	{
		Log = base.Logger;

		LoadAssets();

		ConfigHelper.Instance.BindConfig();

		UnlockAllNecessaryEventsToPlay();

		_harmony = new Harmony(PluginGuid);
		_harmony.PatchAll();

		#region AddingAbilities

		BoneLordsReign.CreateBoneLordsReign();
		FlameStrafe.CreateFlameStrafe();
		PayBonesForSkeleton.CreatePayBonesForSkeleton();
		PayEnergyForWyvern.CreatePayEnergyForWyvern();

		#endregion

		#region AddingCards

		AddAra_ArmoredZombie();
		AddAra_Bonepile();
		AddAra_BonePrince();
		AddAra_Bonelord();
		AddAra_BonelordsHorn();
		AddAra_BooHag();					// Bt Y#0895
		AddAra_DanseMacabre();		// Bt Y#0895
		AddAra_DeadHand();
		AddAra_DeadPets();
		AddAra_Draugr();
		AddAra_DrownedSoul();
		AddAra_Dybbuk();					// Bt Y#0895
		AddAra_Ember_spirit();
		AddAra_Family();
		AddAra_Flames();
		AddAra_Franknstein();
		AddAra_Giant();						// Bt Y#0895
		AddAra_GhostShip();
		AddAra_GraveDigger();
		AddAra_HeadlessHorseman();
		AddAra_Hydra();
		AddAra_Mummy();
		AddAra_Necromancer();
		AddAra_Obol();
		AddAra_PlagueDoctor();
		AddAra_Poltergeist();
		AddAra_Project();					// Bt Y#0895
		AddAra_Revenant();
		AddAra_RingWorm();
		AddAra_Ripper();					// Bt Y#0895
		AddAra_Sarcophagus();
		AddAra_Silbon();					// Bt Y#0895
		AddAra_ScreamingSkull();	// Bt Y#0895
		AddAra_Skelemancer();
		AddAra_SkeletonArmy();
		AddAra_SkeletonMage();
		AddAra_SporeDigger();
		AddAra_Summoner();
		AddAra_TombRobber();
		AddAra_Wendigo();
		AddAra_Wyvern();
		AddAra_ZombieGeck();
		AddAra_Zombie();

		#endregion

		ResizeArtworkForVanillaBoneCards();

		// ChangePackRat();
		// ChangeSquirrel();
	}

	private static void ResizeArtworkForVanillaBoneCards()
	{
		List<string> cardsToResizeArtwork = new List<string>
		{
			"Amoeba", "Maggots"
		};

		foreach (var cardName in cardsToResizeArtwork)
		{
			CardInfo cardInfo = CardLoader.Clone(CardLoader.GetCardByName(cardName));
			CardBuilder builder = CardBuilder.Builder
				.SetAsNormalCard()
				.SetAbilities(cardInfo.abilities)
				.SetBaseAttackAndHealth(cardInfo.baseAttack, cardInfo.baseHealth)
				.SetBoneCost(cardInfo.bonesCost)
				.SetDescription(cardInfo.description)
				.SetNames("ara_" + cardInfo.name, cardInfo.displayedName)
				.SetTribes(cardInfo.tribes);

			if (cardName == "Amoeba")
			{
				builder.SetAsRareCard();
			}

			NewCard.Add(builder.Build());
		}
	}

	private void OnDestroy()
	{
		_harmony?.UnpatchSelf();
	}

	private static void LoadAssets()
	{
		Log.LogDebug($"Loading asset bundles");
		string blockersFile = FileUtils.FindFileInPluginDir("GrimoraMod_Prefabs_Blockers");
		string spritesFile = FileUtils.FindFileInPluginDir("grimoramod_sprites");

		AssetBundle blockerBundle = AssetBundle.LoadFromFile(blockersFile);
		AssetBundle spritesBundle = AssetBundle.LoadFromFile(spritesFile);
		// Log.LogDebug($"Sprites bundle {string.Join(",", spritesBundle.GetAllAssetNames())}");

		AllAssets = blockerBundle.LoadAllAssets();
		AllSpriteAssets = spritesBundle.LoadAllAssets<Sprite>();
		// Log.LogDebug($"Sprites loaded {string.Join(",", AllSpriteAssets.Select(spr => spr.name))}");
	}

	private static void UnlockAllNecessaryEventsToPlay()
	{
		if (!StoryEventsToBeCompleteBeforeStarting.TrueForAll(StoryEventsData.EventCompleted))
		{
			Log.LogWarning($"You haven't completed a required event... Starting unlock process");
			StoryEventsToBeCompleteBeforeStarting.ForEach(evt => StoryEventsData.SetEventCompleted(evt));
			ProgressionData.UnlockAll();
			SaveManager.SaveToFile();
		}
	}

	private static void ResetConfigDataIfGrimoraHasNotReachedTable()
	{
		if (!StoryEventsData.EventCompleted(StoryEvent.GrimoraReachedTable))
		{
			Log.LogWarning($"Grimora has not reached the table yet, resetting values to false again.");
			ResetConfig();
		}
	}

	public static void ResetConfig()
	{
		Log.LogWarning($"Resetting Grimora Mod config");
		ConfigKayceeFirstBossDead.Value = false;
		ConfigSawyerSecondBossDead.Value = false;
		ConfigRoyalThirdBossDead.Value = false;
		ConfigGrimoraBossDead.Value = false;
		ConfigCurrentRemovedPieces.Value = StaticDefaultRemovedPiecesList;
		ConfigCurrentChessboardIndex.Value = 0;
	}

	public static void ResetDeck()
	{
		Log.LogWarning($"Resetting Grimora Deck Data");
		GrimoraSaveData.Data.Initialize();
	}
}
