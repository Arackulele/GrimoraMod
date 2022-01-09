using System.Collections.Generic;
using APIPlugin;
using BepInEx;
using BepInEx.Logging;
using DiskCardGame;
using HarmonyLib;
using UnityEngine;
using Object = System.Object;

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
        public static string Staticpath;
        
        internal static ManualLogSource Log;

        private static Harmony _harmony;

        private void Awake()
        {
            Log = base.Logger;

            Logger.LogInfo($"Loaded {PluginName}!");
            
            Staticpath = Info.Location.Replace("GrimoraMod.dll", "");
            CustomAssetBundle = AssetBundle.LoadFromFile(Staticpath + "Artwork/grimora");
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
                new CustomCard(card) { metaCategories = metaCategories };
            }
        }

    }


}