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
	[BepInDependency("cyantist.inscryption.api", BepInDependency.DependencyFlags.HardDependency)]
	[BepInPlugin(PluginGuid, PluginName, PluginVersion)]
	public partial class GrimoraPlugin : BaseUnityPlugin
	{
		private const string PluginGuid = "arackulele.inscryption.grimoramod";
		private const string PluginName = "GrimoraMod";
		private const string PluginVersion = "1.0.1";

		public static AssetBundle CustomAssetBundle;
		public static Object[] AllAssets;
		public static string StaticPath;

		internal static ManualLogSource Log;

		private static Harmony _harmony;

		public static readonly ConfigFile GrimoraConfigFile = new(
			Path.Combine(Paths.ConfigPath, "grimora_mod_config.cfg"),
			true
		);

		public static string StaticDefaultRemovedPiecesList =
			"BossFigurine," +
			"ChessboardChestPiece," +
			"EnemyPiece_Skelemagus,EnemyPiece_Gravedigger," +
			"Tombstone_North1," +
			"Tombstone_Wall1,Tombstone_Wall2,Tombstone_Wall3,Tombstone_Wall4,Tombstone_Wall5," +
			"Tombstone_South1,Tombstone_South2,Tombstone_South3,";

		public static ConfigEntry<int> ConfigCurrentChessboardIndex;

		public static ConfigEntry<bool> ConfigKayceeFirstBossDead;

		public static ConfigEntry<bool> ConfigDoggySecondBossDead;

		public static ConfigEntry<bool> ConfigRoyalThirdBossDead;

		public static ConfigEntry<bool> ConfigGrimoraBossDead;

		public static ConfigEntry<bool> ConfigFirstTimeBoardInteraction;

		public static ConfigEntry<string> ConfigCurrentRemovedPieces;

		public static ConfigEntry<string> ConfigCurrentActivePieces;

		private static void BindConfig()
		{
			ConfigCurrentChessboardIndex
				= GrimoraConfigFile.Bind(PluginName, "Current chessboard layout index", -1);

			ConfigKayceeFirstBossDead
				= GrimoraConfigFile.Bind(PluginName, "Kaycee defeated?", false);

			ConfigDoggySecondBossDead
				= GrimoraConfigFile.Bind(PluginName, "Doggy defeated?", false);

			ConfigRoyalThirdBossDead
				= GrimoraConfigFile.Bind(PluginName, "Royal defeated?", false);

			ConfigGrimoraBossDead
				= GrimoraConfigFile.Bind(PluginName, "Grimora defeated?", false);

			ConfigFirstTimeBoardInteraction
				= GrimoraConfigFile.Bind(PluginName, "Player interacted with board first time?", false);

			ConfigCurrentRemovedPieces = GrimoraConfigFile.Bind(
				PluginName, "Current Removed Pieces", StaticDefaultRemovedPiecesList);

			ConfigCurrentActivePieces = GrimoraConfigFile.Bind(PluginName, "Current Active Pieces", "");

			var list = ConfigCurrentRemovedPieces.Value.Split(',').ToList();
			if (list.Contains("ChessboardGameMap"))
			{
				list.RemoveAll(piece => piece.Equals("ChessboardGameMap"));
			}

			ConfigCurrentRemovedPieces.Value = string.Join(",", list.Distinct());
		}
		
		private bool toggleEncounterMenu = false;

		private string[] buttonNames = new[]
		{
			"Win Round", "Deck View"
		};

		private void OnGUI()
		{
			toggleEncounterMenu = GUI.Toggle(
				new Rect(20, 100, 200, 20),
				toggleEncounterMenu,
				"Debug Tools"
			);

			if (!toggleEncounterMenu) return;

			int selectedButton = GUI.SelectionGrid(
				new Rect(25, 150, 300, 300),
				-1,
				buttonNames,
				2
			);

			if (selectedButton >= 0)
			{
				Log.LogDebug($"Calling button [{selectedButton}]");
				switch (buttonNames[selectedButton])
				{
					case "Win Round":
						LifeManager.Instance.StartCoroutine(
							LifeManager.Instance.ShowDamageSequence(10, 1, false)
						);
						break;
					case "Deck View":
						ViewManager.Instance.SwitchToView(View.MapDeckReview);
						break;
				}
			}
		}

		private void Awake()
		{
			Log = base.Logger;
			
			BindConfig();

			GrimoraConfigFile.SaveOnConfigSet = true;
			if (!StoryEventsData.EventCompleted(StoryEvent.GrimoraReachedTable))
			{
				Log.LogWarning($"Grimora has not reached the table yet, resetting values to false again.");
				ConfigKayceeFirstBossDead.Value = false;
				ConfigDoggySecondBossDead.Value = false;
				ConfigRoyalThirdBossDead.Value = false;
				ConfigGrimoraBossDead.Value = false;
				ConfigFirstTimeBoardInteraction.Value = false;
				ConfigCurrentRemovedPieces.Value = StaticDefaultRemovedPiecesList;
				ConfigCurrentChessboardIndex.Value = -1;
			}

			if (!StoryEventsData.EventCompleted(StoryEvent.BasicTutorialCompleted))
			{
				Log.LogWarning($"You haven't completed the basic tutorial... THIS WILL UNLOCK A LOT THINGS");
				ProgressionData.UnlockAll();
				StoryEventsData.SetEventCompleted(StoryEvent.BasicTutorialCompleted);
				StoryEventsData.SetEventCompleted(StoryEvent.TutorialRunCompleted);
				StoryEventsData.SetEventCompleted(StoryEvent.StoatIntroduction);
				StoryEventsData.SetEventCompleted(StoryEvent.StoatIntroduction2);
				StoryEventsData.SetEventCompleted(StoryEvent.StoatIntroduction3);
				StoryEventsData.SetEventCompleted(StoryEvent.BonesTutorialCompleted);
				StoryEventsData.SetEventCompleted(StoryEvent.TutorialRun2Completed);
				StoryEventsData.SetEventCompleted(StoryEvent.FigurineFetched);
				StoryEventsData.SetEventCompleted(StoryEvent.LeshyLostCamera);
				StoryEventsData.SetEventCompleted(StoryEvent.TutorialRun3Completed);
				StoryEventsData.SetEventCompleted(StoryEvent.SafeOpened);
				StoryEventsData.SetEventCompleted(StoryEvent.StinkbugCardDiscovered);
				StoryEventsData.SetEventCompleted(StoryEvent.StinkbugStoatReunited);
				StoryEventsData.SetEventCompleted(StoryEvent.StinkbugIntroduction2);
				StoryEventsData.SetEventCompleted(StoryEvent.WoodcarverMet);
				StoryEventsData.SetEventCompleted(StoryEvent.StartScreenNewGameUnlocked);
				StoryEventsData.SetEventCompleted(StoryEvent.StartScreenNewGameUsed);
				StoryEventsData.SetEventCompleted(StoryEvent.Part2Completed);
				StoryEventsData.SetEventCompleted(StoryEvent.GBCIntroCompleted);
				StoryEventsData.SetEventCompleted(StoryEvent.GBCProspectorPhoto);
				StoryEventsData.SetEventCompleted(StoryEvent.GBCAnglerPhoto);
				StoryEventsData.SetEventCompleted(StoryEvent.GBCTrapperPhoto);
				StoryEventsData.SetEventCompleted(StoryEvent.GBCGrimoraDefeated);
				StoryEventsData.SetEventCompleted(StoryEvent.GBCLeshyDefeated);
				StoryEventsData.SetEventCompleted(StoryEvent.GBCPoeDefeated);
				StoryEventsData.SetEventCompleted(StoryEvent.GBCMagnificusDefeated);
			}


			Logger.LogInfo($"Loaded {PluginName}!");

			StaticPath = Info.Location.Replace("GrimoraMod.dll", "");
			CustomAssetBundle = AssetBundle.LoadFromFile(StaticPath + "Artwork/grimora");
			AllAssets = CustomAssetBundle.LoadAllAssets();

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
			// AddAra_Franknstein();
			AddAra_GhostShip();
			// AddAra_GraveDigger();
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

			///Card Changes
			DisableAllActOneCardsFromAppearing();
			// ChangePackRat();
			// ChangeSquirrel();
		}

		private void OnDestroy()
		{
			_harmony?.UnpatchSelf();
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