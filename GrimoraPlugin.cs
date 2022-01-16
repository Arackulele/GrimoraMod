using System.Collections.Generic;
using System.IO;
using System.Linq;
using APIPlugin;
using BepInEx;
using BepInEx.Configuration;
using BepInEx.Logging;
using DiskCardGame;
using HarmonyLib;
using UnityEngine;

namespace GrimoraMod
{
	[BepInDependency("cyantist.inscryption.api")]
	[BepInPlugin(PluginGuid, PluginName, PluginVersion)]
	public partial class GrimoraPlugin : BaseUnityPlugin
	{
		public const string PluginGuid = "arackulele.inscryption.grimoramod";
		public const string PluginName = "GrimoraMod";
		private const string PluginVersion = "2.1.0";

		internal static ManualLogSource Log;

		private static Harmony _harmony;

		public static Object[] AllAssets;

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
			AddAra_Snapper();
			AddAra_SporeDigger();
			AddAra_TombRobber();
			AddAra_Wendigo();
			AddAra_Wolf();
			AddAra_Wyvern();
			AddAra_ZombieGeck();
			AddAra_Zombie();

			#endregion

			DisableAllActOneCardsFromAppearing();
			// ChangePackRat();
			// ChangeSquirrel();
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

			ConfigDeveloperMode = GrimoraConfigFile.Bind(
				PluginName,
				"Enable Developer Mode",
				false,
				new ConfigDescription("Does not generate blocker or enemy pieces except boss. Chests fill first row.")
			);

			ConfigCurrentRemovedPieces = GrimoraConfigFile.Bind(
				PluginName,
				"Current Removed Pieces",
				StaticDefaultRemovedPiecesList,
				new ConfigDescription("Contains all the current removed pieces." +
				                      "\nDo not alter this list unless you know what you are doing!")
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
			AssetBundle bundle = AssetBundle.LoadFromMemory(Properties.Resources.GrimoraMod_Prefabs_Blockers);
			AllAssets = bundle.LoadAllAssets();
		}

		private static void UnlockAllNecessaryEventsToPlay()
		{
			if (StoryEventsToBeCompleteBeforeStarting.Any(evt => !StoryEventsData.EventCompleted(evt)))
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
			ConfigKayceeFirstBossDead.Value = false;
			ConfigSawyerSecondBossDead.Value = false;
			ConfigRoyalThirdBossDead.Value = false;
			ConfigGrimoraBossDead.Value = false;
			ConfigCurrentRemovedPieces.Value = StaticDefaultRemovedPiecesList;
			ConfigCurrentChessboardIndex.Value = 0;
		}

		private static void DisableAllActOneCardsFromAppearing()
		{
			List<string> cards = new List<string>
			{
				"Adder", "Alpha", "Amalgam", "Ant", "AntQueen",
				"Bee", "Beaver", "Beehive", "Bloodhound", "Bullfrog",
				"Cat", "Cockroach", "Coyote",
				"Daus",
				"Elk", "ElkCub",
				"FieldMouse",
				"Geck", "Goat", "Grizzly",
				"JerseyDevil",
				"Kingfisher",
				"Magpie", "Mantis", "MantisGod", "Mole", "MoleMan", "Moose", "Mothman_Stage1",
				"Opossum", "Otter", "Ouroboros",
				"Porcupine", "Pronghorn",
				"RatKing", "Raven", "RavenEgg", "RingWorm",
				"Shark", "Skink", "Skunk", "Snapper", "Snelk", "Sparrow", "SquidBell", "SquidCards", "SquidMirror",
				"Urayuli", "Warren", "Wolf", "WolfCub",
				"PeltGolden", "PeltHare", "PeltWolf",
				"Stinkbug_Talking", "Stoat_Talking", "Wolf_Talking",
				"!STATIC!GLITCH",
			};

			foreach (var card in cards)
			{
				List<CardMetaCategory> metaCategories = new List<CardMetaCategory>();
				new CustomCard(card) { metaCategories = metaCategories, temple = CardTemple.NUM_TEMPLES };
			}
		}
	}
}