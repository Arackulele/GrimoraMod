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
	private const string PluginVersion = "2.4.0";

	internal static ManualLogSource Log;

	private static Harmony _harmony;

	public static UnityEngine.Object[] AllAssets;
	public static UnityEngine.Sprite[] AllSpriteAssets;

	public static readonly ConfigFile GrimoraConfigFile = new(
		Path.Combine(Paths.ConfigPath, "grimora_mod_config.cfg"),
		true
	);

	public const string StaticDefaultRemovedPiecesList =
		"BossFigurine," +
		"ChessboardChestPiece," +
		"EnemyPiece_Skelemagus,EnemyPiece_Gravedigger," +
		"Tombstone_North1," +
		"Tombstone_Wall1,Tombstone_Wall2,Tombstone_Wall3,Tombstone_Wall4,Tombstone_Wall5," +
		"Tombstone_South1,Tombstone_South2,Tombstone_South3,";

	public static ConfigEntry<int> ConfigCurrentChessboardIndex;

	public static ConfigEntry<bool> ConfigKayceeFirstBossDead;

	public static ConfigEntry<bool> ConfigSawyerSecondBossDead;

	public static ConfigEntry<bool> ConfigRoyalThirdBossDead;

	public static ConfigEntry<bool> ConfigGrimoraBossDead;

	public static ConfigEntry<bool> ConfigDeveloperMode;

	public static ConfigEntry<string> ConfigCurrentRemovedPieces;

	private static readonly List<StoryEvent> StoryEventsToBeCompleteBeforeStarting = new()
	{
		StoryEvent.BasicTutorialCompleted, StoryEvent.TutorialRunCompleted, StoryEvent.BonesTutorialCompleted,
		StoryEvent.TutorialRun2Completed, StoryEvent.TutorialRun3Completed
	};


	private void Awake()
	{
		Log = base.Logger;

		LoadAssets();

		BindConfig();

		GrimoraConfigFile.SaveOnConfigSet = true;
		ResetConfigDataIfGrimoraHasNotReachedTable();

		UnlockAllNecessaryEventsToPlay();

		_harmony = new Harmony(PluginGuid);
		_harmony.PatchAll();

		#region AddingAbilities

		FlameStrafe.CreateFlameStrafe();

		#endregion

		#region AddingCards

		AddAra_Bonepile();
		AddAra_BonePrince();
		AddAra_Bonelord();
		AddAra_BonelordsHorn();
		AddAra_BoneSerpent();
		AddAra_CrazedMantis();
		AddAra_DeadHand();
		AddAra_DeadPets();
		AddAra_Draugr();
		AddAra_DrownedSoul();
		AddAra_Ember_spirit();
		AddAra_Family();
		AddAra_Flames();
		AddAra_Franknstein();
		AddAra_GhostShip();
		AddAra_GraveDigger();
		AddAra_HeadlessHorseman();
		AddAra_Hydra();
		AddAra_Mummy();
		AddAra_Necromancer();
		AddAra_Obol();
		AddAra_Poltergeist();
		AddAra_Revenant();
		AddAra_RingWorm();
		AddAra_Sarcophagus();
		AddAra_Skelemancer();
		AddAra_Skelemaniac();
		AddAra_SkeletonArmy();
		AddAra_SkeletonMage();
		AddAra_BoneSnapper();
		AddAra_SporeDigger();
		AddAra_TombRobber();
		AddAra_UndeadWolf();
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
			"Amoeba", "Bat", "Maggots", "Rattler", "Vulture",
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

	private static void BindConfig()
	{
		ConfigCurrentChessboardIndex
			= GrimoraConfigFile.Bind(PluginName, "Current chessboard layout index", 0);

		ConfigKayceeFirstBossDead
			= GrimoraConfigFile.Bind(PluginName, "Kaycee defeated?", false);

		ConfigSawyerSecondBossDead
			= GrimoraConfigFile.Bind(PluginName, "Sawyer defeated?", false);

		ConfigRoyalThirdBossDead
			= GrimoraConfigFile.Bind(PluginName, "Royal defeated?", false);

		ConfigGrimoraBossDead
			= GrimoraConfigFile.Bind(PluginName, "Grimora defeated?", false);

		ConfigCurrentRemovedPieces = GrimoraConfigFile.Bind(
			PluginName,
			"Current Removed Pieces",
			StaticDefaultRemovedPiecesList,
			new ConfigDescription("Contains all the current removed pieces." +
			                      "\nDo not alter this list unless you know what you are doing!")
		);

		ConfigDeveloperMode = GrimoraConfigFile.Bind(
			PluginName,
			"Enable Developer Mode",
			false,
			new ConfigDescription("Does not generate blocker or enemy pieces except boss. Chests fill first row.")
		);

		var list = ConfigCurrentRemovedPieces.Value.Split(',').ToList();
		// this is so that for whatever reason the game map gets added to the removal list,
		//	this will automatically remove those entries
		if (list.Contains("ChessboardGameMap"))
		{
			list.RemoveAll(piece => piece.Equals("ChessboardGameMap"));
		}

		ConfigCurrentRemovedPieces.Value = string.Join(",", list.Distinct());
	}

	private static void LoadAssets()
	{
		Log.LogDebug($"Loading asset bundles");
		string blockersFile = FileUtils.FindFileInPluginDir("GrimoraMod_Prefabs_Blockers");
		string spritesFile = FileUtils.FindFileInPluginDir("grimoramod_all_assets.sprites");

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
