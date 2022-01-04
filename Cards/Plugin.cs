using BepInEx;
using BepInEx.Logging;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;
using System.IO;
using System.Security.Cryptography;
using APIPlugin;
using DiskCardGame;
using HarmonyLib;
using UnityEngine;
using BepInEx.Configuration;
using GBC;
using I2.TextAnimation;
using Mono.Cecil;
using Mono.Cecil.Cil;
using Pixelplacement;
using Sirenix;
using Unity.Baselib.LowLevel;
using UnityEngine.Playables;
using UnityEngine.PostProcessing;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;
using UnityEngine.XR;
using Object = UnityEngine.Object;

namespace GrimoraMod
{

    [BepInDependency("cyantist.inscryption.api", BepInDependency.DependencyFlags.HardDependency)]
    [BepInPlugin(PluginGuid, PluginName, PluginVersion)]

    public class Plugin : BaseUnityPlugin
    {
        private const string PluginGuid = "arackulele.inscryption.grimoramod";
        private const string PluginName = "GrimoraMod";
        private const string PluginVersion = "1.0.1";

        internal static ManualLogSource Log;





        private void Awake()
        {
            Plugin.Log = base.Logger;
            Log.LogInfo("test");
            Log.LogInfo(Info.Location);
            Logger.LogInfo($"Loaded {PluginName}!");

            Harmony harmony = new Harmony(PluginGuid);
            harmony.PatchAll();

            ///Abilities
            CreateFlamestrafe();

            ///Cards
            AddAra_BoneDigger();
            AddAra_Bonelord();
            AddAra_BonePile();
            AddAra_BonePrince();
            AddAra_CrazedMantis();
            AddAra_DeadHand();
            AddAra_Draugr();
            AddAra_DrownedSoul();
            AddAra_Franknstein();
            AddAra_GraveRobber();
            AddAra_HeadlessHorseman();
            AddAra_BonelordsHorn();
            AddAra_RingWorm();
            AddAra_mummylord();
            AddAra_Necromancer();
            AddAra_obol();
            AddAra_Pets();
            AddAra_Pirateship();
            AddAra_Revenant();
            AddAra_sarcophagus();
            AddAra_Serpent();
            AddAra_Skelemancer();
            AddAra_SkeletonMage();
            AddAra_Snapper();
            AddAra_SporeDigger();
            AddAra_Wolf();
            AddAra_ZombGeck();
            AddAra_Zombie();
            AddAra_Flames();
            AddAra_Ember_spirit();
            AddAra_Wendigo();
            AddAra_Wyvern();
            AddAra_Poltergeist();
            AddAra_Hydra();

            ///Card Changes
            RemoveAll();
            ChangeSquirrel();
            ChangeGoat();
            ChangePackRat();



        }



        private void AddAra_BoneDigger()
        {
            List<CardMetaCategory> metaCategories = new List<CardMetaCategory>();
            metaCategories.Add(CardMetaCategory.ChoiceNode);
            metaCategories.Add(CardMetaCategory.TraderOffer);

            List<Ability> abilities = new List<Ability>();
            abilities.Add(Ability.BoneDigger);

            byte[] imgBytes = System.IO.File.ReadAllBytes(Path.Combine(this.Info.Location.Replace("GrimoraMod.dll", ""), "Artwork/Goat.png"));
            Texture2D tex = new Texture2D(2, 2);
            tex.LoadImage(imgBytes);
            NewCard.Add("ara_BoneDigger", "Gravedigger", 0, 3, metaCategories, CardComplexity.Simple, CardTemple.Nature, description: "He spends his time alone digging for Bones in hopes of finding a Tresure.", bonesCost: 1, abilities: abilities, defaultTex: tex);
        }

        private void AddAra_Bonelord()
        {
            List<CardMetaCategory> metaCategories = new List<CardMetaCategory>();
            metaCategories.Add(CardMetaCategory.Rare);

            List<CardAppearanceBehaviour.Appearance> appearanceBehaviour = new List<CardAppearanceBehaviour.Appearance>();
            appearanceBehaviour.Add(CardAppearanceBehaviour.Appearance.RareCardBackground);

            List<Ability> abilities = new List<Ability>();
            abilities.Add(Ability.Deathtouch);

            byte[] imgBytes = System.IO.File.ReadAllBytes(Path.Combine(this.Info.Location.Replace("GrimoraMod.dll", ""), "Artwork/bone_lord.png"));
            Texture2D tex = new Texture2D(2, 2);
            tex.LoadImage(imgBytes);

            byte[] imgBytes1 = System.IO.File.ReadAllBytes(Path.Combine(this.Info.Location.Replace("GrimoraMod.dll", ""), "Artwork/Energy6.png"));
            Texture2D tex1 = new Texture2D(2, 2);
            tex1.LoadImage(imgBytes1);

            List<Texture> decals = new();
            decals.Add(tex1);

            NewCard.Add("ara_Bonelord", "The Bone Lord", 5, 10, metaCategories, CardComplexity.Simple, CardTemple.Nature, description: "Lord of Bones, Lord of Bones answer our call.", bonesCost: 6, energyCost: 6, appearanceBehaviour: appearanceBehaviour, abilities: abilities, defaultTex: tex, decals: decals);
        }

        private void AddAra_BonePile()
        {
            List<CardMetaCategory> metaCategories = new List<CardMetaCategory>();
            metaCategories.Add(CardMetaCategory.ChoiceNode);
            metaCategories.Add(CardMetaCategory.TraderOffer);

            List<Ability> abilities = new List<Ability>();
            abilities.Add(Ability.QuadrupleBones);

            byte[] imgBytes = System.IO.File.ReadAllBytes(Path.Combine(this.Info.Location.Replace("GrimoraMod.dll", ""), "Artwork/BonePile.png"));
            Texture2D tex = new Texture2D(2, 2);
            tex.LoadImage(imgBytes);
            NewCard.Add("ara_BonePile", "Bone Heap", 0, 2, metaCategories, CardComplexity.Vanilla, CardTemple.Nature, description: "An uninspiring Pile of Bones, you can have it.", bonesCost: 1, abilities: abilities, defaultTex: tex);
        }

        private void AddAra_Wyvern()
        {
            List<CardMetaCategory> metaCategories = new List<CardMetaCategory>();
            metaCategories.Add(CardMetaCategory.ChoiceNode);
            metaCategories.Add(CardMetaCategory.TraderOffer);

            List<Ability> abilities = new List<Ability>();
            abilities.Add(Ability.DrawCopy);

            byte[] imgBytes = System.IO.File.ReadAllBytes(Path.Combine(this.Info.Location.Replace("GrimoraMod.dll", ""), "Artwork/Wyvern.png"));
            Texture2D tex = new Texture2D(2, 2);
            tex.LoadImage(imgBytes);
            NewCard.Add("ara_Wyvern", "Wyvern", 1, 1, metaCategories, CardComplexity.Vanilla, CardTemple.Nature, description: "The Wyvern army approaches.", bonesCost: 1, abilities: abilities, defaultTex: tex);
        }

        private void AddAra_BonePrince()
        {
            List<CardMetaCategory> metaCategories = new List<CardMetaCategory>();
            metaCategories.Add(CardMetaCategory.GBCPlayable);

            byte[] imgBytes = System.IO.File.ReadAllBytes(Path.Combine(this.Info.Location.Replace("GrimoraMod.dll", ""), "Artwork/BonePrince.png"));
            Texture2D tex = new Texture2D(2, 2);
            tex.LoadImage(imgBytes);
            NewCard.Add("ara_BonePrince", "Bone Prince", 2, 1, metaCategories, CardComplexity.Simple, CardTemple.Nature, bonesCost: 1, defaultTex: tex);
        }

        private void AddAra_CrazedMantis()
        {
            List<CardMetaCategory> metaCategories = new List<CardMetaCategory>();
            metaCategories.Add(CardMetaCategory.ChoiceNode);
            metaCategories.Add(CardMetaCategory.TraderOffer);

            List<Ability> abilities = new List<Ability>();
            abilities.Add(Ability.SplitStrike);

            byte[] imgBytes = System.IO.File.ReadAllBytes(Path.Combine(this.Info.Location.Replace("GrimoraMod.dll", ""), "Artwork/Mantis.png"));
            Texture2D tex = new Texture2D(2, 2);
            tex.LoadImage(imgBytes);
            NewCard.Add("ara_CrazedMantis", "Crazed Mantis", 1, 1, metaCategories, CardComplexity.Vanilla, CardTemple.Nature, description: "The poor mantis has gone Insane, a gruesome fate.", bonesCost: 4, abilities: abilities, defaultTex: tex);
        }

        private void AddAra_DeadHand()
        {
            List<CardMetaCategory> metaCategories = new List<CardMetaCategory>();
            metaCategories.Add(CardMetaCategory.ChoiceNode);
            metaCategories.Add(CardMetaCategory.TraderOffer);

            List<Ability> abilities = new List<Ability>();
            abilities.Add(Ability.DrawNewHand);

            byte[] imgBytes = System.IO.File.ReadAllBytes(Path.Combine(this.Info.Location.Replace("GrimoraMod.dll", ""), "Artwork/DeadHand.png"));
            Texture2D tex = new Texture2D(2, 2);
            tex.LoadImage(imgBytes);
            NewCard.Add("ara_DeadHand", "Dead Hand", 1, 1, metaCategories, CardComplexity.Intermediate, CardTemple.Nature, description: "Cut off from an ancient God, the Dead Hand took on its own Life.", bonesCost: 5, abilities: abilities, defaultTex: tex);
        }

        private void AddAra_Draugr()
        {
            List<CardMetaCategory> metaCategories = new List<CardMetaCategory>();
            metaCategories.Add(CardMetaCategory.ChoiceNode);
            metaCategories.Add(CardMetaCategory.TraderOffer);

            List<Ability> abilities = new List<Ability>();
            abilities.Add(Ability.IceCube);

            byte[] imgBytes = System.IO.File.ReadAllBytes(Path.Combine(this.Info.Location.Replace("GrimoraMod.dll", ""), "Artwork/Skelarmor.png"));
            Texture2D tex = new Texture2D(2, 2);
            tex.LoadImage(imgBytes);
            NewCard.Add("ara_Draugr", "Draugr", 0, 1, metaCategories, CardComplexity.Simple, CardTemple.Nature, description: "Hiding in a Suit of Armor, this Skeleton wont last forever.", bonesCost: 1, abilities: abilities, defaultTex: tex, iceCubeId: new IceCubeIdentifier("Squirrel"));
        }

        private void AddAra_DrownedSoul()
        {
            List<CardMetaCategory> metaCategories = new List<CardMetaCategory>();
            metaCategories.Add(CardMetaCategory.ChoiceNode);
            metaCategories.Add(CardMetaCategory.TraderOffer);

            List<Ability> abilities = new List<Ability>();
            abilities.Add(Ability.Deathtouch);
            abilities.Add(Ability.Submerge);

            byte[] imgBytes = System.IO.File.ReadAllBytes(Path.Combine(this.Info.Location.Replace("GrimoraMod.dll", ""), "Artwork/DrownedSoul.png"));
            Texture2D tex = new Texture2D(2, 2);
            tex.LoadImage(imgBytes);
            NewCard.Add("ara_DrownedSoul", "Drowned Soul", 1, 1, metaCategories, CardComplexity.Simple, CardTemple.Nature, description: "Going into that Well wasnt the best idea.", bonesCost: 4, abilities: abilities, defaultTex: tex);
        }

        private void AddAra_Franknstein()
        {
            List<CardMetaCategory> metaCategories = new List<CardMetaCategory>();
            metaCategories.Add(CardMetaCategory.ChoiceNode);
            metaCategories.Add(CardMetaCategory.TraderOffer);

            byte[] imgBytes = System.IO.File.ReadAllBytes(Path.Combine(this.Info.Location.Replace("GrimoraMod.dll", ""), "Artwork/Franknstein.png"));
            Texture2D tex = new Texture2D(2, 2);
            tex.LoadImage(imgBytes);
            NewCard.Add("ara_Franknstein", "Frank & Stein", 2, 2, metaCategories, CardComplexity.Vanilla, CardTemple.Nature, description: "Frank and Stein, best Friends brothers and Fighters.", bonesCost: 5, defaultTex: tex);
        }

        private void AddAra_GraveRobber()
        {
            List<CardMetaCategory> metaCategories = new List<CardMetaCategory>();
            metaCategories.Add(CardMetaCategory.ChoiceNode);
            metaCategories.Add(CardMetaCategory.TraderOffer);

            List<Ability> abilities = new List<Ability>();
            abilities.Add(Ability.ExplodeOnDeath);

            byte[] imgBytes = System.IO.File.ReadAllBytes(Path.Combine(this.Info.Location.Replace("GrimoraMod.dll", ""), "Artwork/GraveRobber.png"));
            Texture2D tex = new Texture2D(2, 2);
            tex.LoadImage(imgBytes);
            NewCard.Add("ara_GraveRobber", "Tomb Robber", 0, 1, metaCategories, CardComplexity.Intermediate, CardTemple.Nature, description: "Nothing, nothing again, no treasure is left anymore.", bonesCost: 0, abilities: abilities, defaultTex: tex);
        }

        private void AddAra_HeadlessHorseman()
        {
            List<CardMetaCategory> metaCategories = new List<CardMetaCategory>();
            metaCategories.Add(CardMetaCategory.ChoiceNode);
            metaCategories.Add(CardMetaCategory.TraderOffer);

            List<Ability> abilities = new List<Ability>();
            abilities.Add(Ability.Strafe);
            abilities.Add(Ability.Flying);

            byte[] imgBytes = System.IO.File.ReadAllBytes(Path.Combine(this.Info.Location.Replace("GrimoraMod.dll", ""), "Artwork/HeadlessHorseman.png"));
            Texture2D tex = new Texture2D(2, 2);
            tex.LoadImage(imgBytes);
            NewCard.Add("ara_HeadlessHorseman", "Headless Horseman", 4, 3, metaCategories, CardComplexity.Vanilla, CardTemple.Nature, description: "The apocalypse is soon, get ready.", bonesCost: 9, abilities: abilities, defaultTex: tex);
        }

        private void AddAra_Poltergeist()
        {
            List<CardMetaCategory> metaCategories = new List<CardMetaCategory>();
            metaCategories.Add(CardMetaCategory.ChoiceNode);
            metaCategories.Add(CardMetaCategory.TraderOffer);

            List<Ability> abilities = new List<Ability>();
            abilities.Add(Ability.Submerge);
            abilities.Add(Ability.Flying);

            byte[] imgBytes = System.IO.File.ReadAllBytes(Path.Combine(this.Info.Location.Replace("GrimoraMod.dll", ""), "Artwork/Ghost.png"));
            Texture2D tex = new Texture2D(2, 2);
            tex.LoadImage(imgBytes);
            NewCard.Add("ara_Poltergeist", "Poltergeist", 1, 1, metaCategories, CardComplexity.Vanilla, CardTemple.Nature, description: "A skilled Haunting Ghost, handle with caution.", energyCost: 3, abilities: abilities, defaultTex: tex);
        }

        private void AddAra_BonelordsHorn()
        {
            List<CardMetaCategory> metaCategories = new List<CardMetaCategory>();
            metaCategories.Add(CardMetaCategory.Rare);

            List<CardAppearanceBehaviour.Appearance> appearanceBehaviour = new List<CardAppearanceBehaviour.Appearance>();
            appearanceBehaviour.Add(CardAppearanceBehaviour.Appearance.RareCardBackground);

            List<Ability> abilities = new List<Ability>();
            abilities.Add(Ability.QuadrupleBones);
            abilities.Add(Ability.IceCube);

            byte[] imgBytes = System.IO.File.ReadAllBytes(Path.Combine(this.Info.Location.Replace("GrimoraMod.dll", ""), "Artwork/BonelordsHorn.png"));
            Texture2D tex = new Texture2D(2, 2);
            tex.LoadImage(imgBytes);

            byte[] imgBytes1 = System.IO.File.ReadAllBytes(Path.Combine(this.Info.Location.Replace("GrimoraMod.dll", ""), "Artwork/Energy4.png"));
            Texture2D tex1 = new Texture2D(2, 2);
            tex1.LoadImage(imgBytes1);

            List<Texture> decals = new();
            decals.Add(tex1);


            NewCard.Add("ara_BonelordsHorn", "Bone Lords Horn", 0, 1, metaCategories, CardComplexity.Simple, CardTemple.Nature, description: "The Horn of the Bonelord, you do not want to find out whats inside.", energyCost: 4, appearanceBehaviour: appearanceBehaviour, abilities: abilities, defaultTex: tex, decals: decals, iceCubeId: new IceCubeIdentifier("ara_BonePrince"));
        }

        private void AddAra_RingWorm()
        {
            List<CardMetaCategory> metaCategories = new List<CardMetaCategory>();
            metaCategories.Add(CardMetaCategory.ChoiceNode);
            metaCategories.Add(CardMetaCategory.TraderOffer);

            List<Ability> abilities = new List<Ability>();
            abilities.Add(Ability.BoneDigger);

            byte[] imgBytes = System.IO.File.ReadAllBytes(Path.Combine(this.Info.Location.Replace("GrimoraMod.dll", ""), "Artwork/RingWorm.png"));
            Texture2D tex = new Texture2D(2, 2);
            tex.LoadImage(imgBytes);
            NewCard.Add("ara_RingWorm", "Mudworm", 2, 1, metaCategories, CardComplexity.Simple, CardTemple.Nature, description: "The Mudworm digs through the Earth day in day out.", bonesCost: 5, abilities: abilities, defaultTex: tex);
        }

        private void AddAra_mummylord()
        {
            List<CardMetaCategory> metaCategories = new List<CardMetaCategory>();
            metaCategories.Add(CardMetaCategory.GBCPlayable);

            byte[] imgBytes = System.IO.File.ReadAllBytes(Path.Combine(this.Info.Location.Replace("GrimoraMod.dll", ""), "Artwork/Mummy.png"));
            Texture2D tex = new Texture2D(2, 2);
            tex.LoadImage(imgBytes);
            NewCard.Add("ara_mummylord", "Mummy Lord", 3, 3, metaCategories, CardComplexity.Advanced, CardTemple.Nature, description: "The Cycle of the Mummy Lord, never ending.", bonesCost: 2, defaultTex: tex);
        }

        private void AddAra_Necromancer()
        {
            List<CardMetaCategory> metaCategories = new List<CardMetaCategory>();
            metaCategories.Add(CardMetaCategory.ChoiceNode);
            metaCategories.Add(CardMetaCategory.TraderOffer);

            List<Ability> abilities = new List<Ability>();
            abilities.Add(Ability.DoubleDeath);

            byte[] imgBytes = System.IO.File.ReadAllBytes(Path.Combine(this.Info.Location.Replace("GrimoraMod.dll", ""), "Artwork/Necromancer.png"));
            Texture2D tex = new Texture2D(2, 2);
            tex.LoadImage(imgBytes);
            NewCard.Add("ara_Necromancer", "Necromancer", 1, 2, metaCategories, CardComplexity.Vanilla, CardTemple.Nature, description: "The vicious Necromancer, nothing dies once.", bonesCost: 3, abilities: abilities, defaultTex: tex);
        }

        private void AddAra_obol()
        {
            List<CardMetaCategory> metaCategories = new List<CardMetaCategory>();
            metaCategories.Add(CardMetaCategory.ChoiceNode);
            metaCategories.Add(CardMetaCategory.TraderOffer);

            List<Ability> abilities = new List<Ability>();
            abilities.Add(Ability.Sharp);
            byte[] imgBytes1 = System.IO.File.ReadAllBytes(Path.Combine(this.Info.Location.Replace("GrimoraMod.dll", ""), "Artwork/Energy3.png"));
            Texture2D tex1 = new Texture2D(2, 2);
            tex1.LoadImage(imgBytes1);

            List<Texture> decals = new();
            decals.Add(tex1);

            byte[] imgBytes = System.IO.File.ReadAllBytes(Path.Combine(this.Info.Location.Replace("GrimoraMod.dll", ""), "Artwork/Obol.png"));
            Texture2D tex = new Texture2D(2, 2);
            tex.LoadImage(imgBytes);
            NewCard.Add("ara_obol", "Ancient Obol", 0, 6, metaCategories, CardComplexity.Simple, CardTemple.Nature, description: "The Ancient Obol, the Bone Lord likes this one.", energyCost: 3, abilities: abilities, defaultTex: tex, decals: decals);
        }

        private void AddAra_Pets()
        {
            List<CardMetaCategory> metaCategories = new List<CardMetaCategory>();
            metaCategories.Add(CardMetaCategory.Rare);

            List<CardAppearanceBehaviour.Appearance> appearanceBehaviour = new List<CardAppearanceBehaviour.Appearance>();
            appearanceBehaviour.Add(CardAppearanceBehaviour.Appearance.RareCardBackground);

            List<Ability> abilities = new List<Ability>();
            abilities.Add(Ability.DrawCopyOnDeath);
            abilities.Add(Ability.Brittle);

            byte[] imgBytes = System.IO.File.ReadAllBytes(Path.Combine(this.Info.Location.Replace("GrimoraMod.dll", ""), "Artwork/Pets.png"));
            Texture2D tex = new Texture2D(2, 2);
            tex.LoadImage(imgBytes);
            NewCard.Add("ara_Pets", "Pharaohs Pets", 3, 1, metaCategories, CardComplexity.Vanilla, CardTemple.Nature, description: "The Undying Pets of the Pharao, enchanting.", bonesCost: 4, appearanceBehaviour: appearanceBehaviour, abilities: abilities, defaultTex: tex);
        }

        private void AddAra_Pirateship()
        {
            List<CardMetaCategory> metaCategories = new List<CardMetaCategory>();
            metaCategories.Add(CardMetaCategory.ChoiceNode);
            metaCategories.Add(CardMetaCategory.TraderOffer);

            List<Ability> abilities = new List<Ability>();
            abilities.Add(Ability.SkeletonStrafe);
            abilities.Add(Ability.Submerge);

            byte[] imgBytes = System.IO.File.ReadAllBytes(Path.Combine(this.Info.Location.Replace("GrimoraMod.dll", ""), "Artwork/Pirateship.png"));
            Texture2D tex = new Texture2D(2, 2);
            tex.LoadImage(imgBytes);
            NewCard.Add("ara_Pirateship", "Ghost Ship", 0, 1, metaCategories, CardComplexity.Simple, CardTemple.Nature, description: "The Skeleton Army never rests.", bonesCost: 4, abilities: abilities, defaultTex: tex);
        }

        private void AddAra_Revenant()
        {
            List<CardMetaCategory> metaCategories = new List<CardMetaCategory>();
            metaCategories.Add(CardMetaCategory.ChoiceNode);
            metaCategories.Add(CardMetaCategory.TraderOffer);

            List<Ability> abilities = new List<Ability>();
            abilities.Add(Ability.Brittle);

            byte[] imgBytes = System.IO.File.ReadAllBytes(Path.Combine(this.Info.Location.Replace("GrimoraMod.dll", ""), "Artwork/Revenant.png"));
            Texture2D tex = new Texture2D(2, 2);
            tex.LoadImage(imgBytes);
            NewCard.Add("ara_Revenant", "Revenant", 3, 1, metaCategories, CardComplexity.Intermediate, CardTemple.Nature, description: "The Revenant, bringing the Scythe of death.", bonesCost: 3, abilities: abilities, defaultTex: tex);
        }

        private void AddAra_sarcophagus()
        {
            List<CardMetaCategory> metaCategories = new List<CardMetaCategory>();
            metaCategories.Add(CardMetaCategory.ChoiceNode);
            metaCategories.Add(CardMetaCategory.TraderOffer);

            List<Ability> abilities = new List<Ability>();
            abilities.Add(Ability.Evolve);

            byte[] imgBytes = System.IO.File.ReadAllBytes(Path.Combine(this.Info.Location.Replace("GrimoraMod.dll", ""), "Artwork/Sarcopagus.png"));
            Texture2D tex = new Texture2D(2, 2);
            tex.LoadImage(imgBytes);
            NewCard.Add("ara_sarcophagus", "Sarcophagus", 0, 2, metaCategories, CardComplexity.Advanced, CardTemple.Nature, description: "The Cycle of the Mummy Lord, never ending.", bonesCost: 4, abilities: abilities, defaultTex: tex, evolveId: new EvolveIdentifier("ara_mummylord", 1));
        }

        private void AddAra_Serpent()
        {
            List<CardMetaCategory> metaCategories = new List<CardMetaCategory>();
            metaCategories.Add(CardMetaCategory.ChoiceNode);
            metaCategories.Add(CardMetaCategory.TraderOffer);

            List<Ability> abilities = new List<Ability>();
            abilities.Add(Ability.Deathtouch);

            byte[] imgBytes = System.IO.File.ReadAllBytes(Path.Combine(this.Info.Location.Replace("GrimoraMod.dll", ""), "Artwork/Adder.png"));
            Texture2D tex = new Texture2D(2, 2);
            tex.LoadImage(imgBytes);
            NewCard.Add("ara_Serpent", "Bone Serpent", 1, 1, metaCategories, CardComplexity.Vanilla, CardTemple.Nature, description: "Serpent of Bones, its poison can melt even Bones.", bonesCost: 4, abilities: abilities, defaultTex: tex);
        }

        private void AddAra_Skelemancer()
        {
            List<CardMetaCategory> metaCategories = new List<CardMetaCategory>();
            metaCategories.Add(CardMetaCategory.ChoiceNode);
            metaCategories.Add(CardMetaCategory.TraderOffer);

            byte[] imgBytes = System.IO.File.ReadAllBytes(Path.Combine(this.Info.Location.Replace("GrimoraMod.dll", ""), "Artwork/SkeletonJuniorSage.png"));
            Texture2D tex = new Texture2D(2, 2);
            tex.LoadImage(imgBytes);

            byte[] imgBytes1 = System.IO.File.ReadAllBytes(Path.Combine(this.Info.Location.Replace("GrimoraMod.dll", ""), "Artwork/Energy2.png"));
            Texture2D tex1 = new Texture2D(2, 2);
            tex1.LoadImage(imgBytes1);

            List<Texture> decals = new();
            decals.Add(tex1);


            NewCard.Add("ara_Skelemancer", "Skelemancer", 1, 1, metaCategories, CardComplexity.Simple, CardTemple.Nature, description: "The humble Skelemancer, he likes a good fight.", energyCost: 2, defaultTex: tex, decals: decals);
        }

        private void AddAra_SkeletonMage()
        {
            List<CardMetaCategory> metaCategories = new List<CardMetaCategory>();
            metaCategories.Add(CardMetaCategory.ChoiceNode);
            metaCategories.Add(CardMetaCategory.TraderOffer);

            List<Ability> abilities = new List<Ability>();
            abilities.Add(Ability.Deathtouch);

            byte[] imgBytes = System.IO.File.ReadAllBytes(Path.Combine(this.Info.Location.Replace("GrimoraMod.dll", ""), "Artwork/SkeletonMage.png"));
            Texture2D tex = new Texture2D(2, 2);
            tex.LoadImage(imgBytes);

            byte[] imgBytes1 = System.IO.File.ReadAllBytes(Path.Combine(this.Info.Location.Replace("GrimoraMod.dll", ""), "Artwork/Energy4.png"));
            Texture2D tex1 = new Texture2D(2, 2);
            tex1.LoadImage(imgBytes1);

            List<Texture> decals = new();
            decals.Add(tex1);


            NewCard.Add("ara_SkeletonMage", "Skelemagus", 1, 1, metaCategories, CardComplexity.Simple, CardTemple.Nature, description: "The Skelemagus, they have learned the Ancient Spell of Death.", energyCost: 4, abilities: abilities, defaultTex: tex, decals: decals);
        }

        private void AddAra_Snapper()
        {
            List<CardMetaCategory> metaCategories = new List<CardMetaCategory>();
            metaCategories.Add(CardMetaCategory.ChoiceNode);
            metaCategories.Add(CardMetaCategory.TraderOffer);

            byte[] imgBytes = System.IO.File.ReadAllBytes(Path.Combine(this.Info.Location.Replace("GrimoraMod.dll", ""), "Artwork/Snapper.png"));
            Texture2D tex = new Texture2D(2, 2);
            tex.LoadImage(imgBytes);
            NewCard.Add("ara_Snapper", "Bone Snapper", 1, 6, metaCategories, CardComplexity.Vanilla, CardTemple.Nature, description: "Snap snap your Bones are gone.", bonesCost: 5, defaultTex: tex);
        }

        private void AddAra_SporeDigger()
        {
            List<CardMetaCategory> metaCategories = new List<CardMetaCategory>();
            metaCategories.Add(CardMetaCategory.Rare);

            List<CardAppearanceBehaviour.Appearance> appearanceBehaviour = new List<CardAppearanceBehaviour.Appearance>();
            appearanceBehaviour.Add(CardAppearanceBehaviour.Appearance.RareCardBackground);

            List<Ability> abilities = new List<Ability>();
            abilities.Add(Ability.BoneDigger);
            abilities.Add(Ability.BoneDigger);

            byte[] imgBytes = System.IO.File.ReadAllBytes(Path.Combine(this.Info.Location.Replace("GrimoraMod.dll", ""), "Artwork/SporeDigger.png"));
            Texture2D tex = new Texture2D(2, 2);
            tex.LoadImage(imgBytes);
            NewCard.Add("ara_SporeDigger", "Sporedigger", 0, 3, metaCategories, CardComplexity.Simple, CardTemple.Nature, description: "The Spore Sigger, an excellent digger.", bonesCost: 1, appearanceBehaviour: appearanceBehaviour, abilities: abilities, defaultTex: tex);
        }

        private void AddAra_Wolf()
        {
            List<CardMetaCategory> metaCategories = new List<CardMetaCategory>();

            byte[] imgBytes = System.IO.File.ReadAllBytes(Path.Combine(this.Info.Location.Replace("GrimoraMod.dll", ""), "Artwork/Wolf.png"));
            Texture2D tex = new Texture2D(2, 2);
            tex.LoadImage(imgBytes);
            NewCard.Add("ara_Wolf", "Undead Wolf", 3, 2, metaCategories, CardComplexity.Vanilla, CardTemple.Nature, description: "A diseased Wolf, but the Pack stays Strong.", bonesCost: 7, defaultTex: tex);
        }

        private void AddAra_ZombGeck()
        {
            List<CardMetaCategory> metaCategories = new List<CardMetaCategory>();
            metaCategories.Add(CardMetaCategory.Rare);

            List<Ability> abilities = new List<Ability>();
            abilities.Add(Ability.Brittle);

            List<CardAppearanceBehaviour.Appearance> appearanceBehaviour = new List<CardAppearanceBehaviour.Appearance>();
            appearanceBehaviour.Add(CardAppearanceBehaviour.Appearance.RareCardBackground);

            byte[] imgBytes = System.IO.File.ReadAllBytes(Path.Combine(this.Info.Location.Replace("GrimoraMod.dll", ""), "Artwork/Geck.png"));
            Texture2D tex = new Texture2D(2, 2);
            tex.LoadImage(imgBytes);
            NewCard.Add("ara_ZombGeck", "Zomb-Geck", 2, 1, metaCategories, CardComplexity.Vanilla, CardTemple.Nature, description: "Da Geeckkkkk is deaddd nooooooooo.", abilities: abilities, defaultTex: tex, appearanceBehaviour: appearanceBehaviour);
        }

        private void AddAra_Zombie()
        {
            List<CardMetaCategory> metaCategories = new List<CardMetaCategory>();
            metaCategories.Add(CardMetaCategory.ChoiceNode);
            metaCategories.Add(CardMetaCategory.TraderOffer);

            byte[] imgBytes = System.IO.File.ReadAllBytes(Path.Combine(this.Info.Location.Replace("GrimoraMod.dll", ""), "Artwork/Zombie.png"));
            Texture2D tex = new Texture2D(2, 2);
            tex.LoadImage(imgBytes);
            NewCard.Add("ara_Zombie", "Zombie", 1, 1, metaCategories, CardComplexity.Simple, CardTemple.Nature, description: "The humble Zombie, a respected member of the Army.", bonesCost: 2, defaultTex: tex);
        }

        private void AddAra_Flames()
        {
            List<CardMetaCategory> metaCategories = new List<CardMetaCategory>();

            List<Ability> abilities = new List<Ability>();
            abilities.Add(Ability.BuffNeighbours);
            abilities.Add(Ability.Brittle);
            byte[] imgBytes = System.IO.File.ReadAllBytes(Path.Combine(this.Info.Location.Replace("GrimoraMod.dll", ""), "Artwork/flames.png"));
            Texture2D tex = new Texture2D(2, 2);
            tex.LoadImage(imgBytes);
            NewCard.Add("ara_Flames", "Flames", 0, 1, metaCategories, CardComplexity.Simple, CardTemple.Nature, description: "Unused Part.", bonesCost: 2, defaultTex: tex, abilities: abilities);
        }

        private void AddAra_Wendigo()
        {
            List<CardMetaCategory> metaCategories = new List<CardMetaCategory>();
            metaCategories.Add(CardMetaCategory.Rare);

            List<Ability> abilities = new List<Ability>();
            abilities.Add(Ability.Strafe);
            abilities.Add(Ability.DebuffEnemy);

            List<CardAppearanceBehaviour.Appearance> appearanceBehaviour = new List<CardAppearanceBehaviour.Appearance>();
            appearanceBehaviour.Add(CardAppearanceBehaviour.Appearance.RareCardBackground);

            byte[] imgBytes = System.IO.File.ReadAllBytes(Path.Combine(this.Info.Location.Replace("GrimoraMod.dll", ""), "Artwork/Wendigo.png"));
            Texture2D tex = new Texture2D(2, 2);
            tex.LoadImage(imgBytes);
            NewCard.Add("ara_Wendigo", "Wendigo", 2, 2, metaCategories, CardComplexity.Vanilla, CardTemple.Nature, description: "Described by some as the truest nightmare", bonesCost: 5, abilities: abilities, defaultTex: tex, appearanceBehaviour: appearanceBehaviour);
        }

        private void AddAra_Hydra()
        {
            List<CardMetaCategory> metaCategories = new List<CardMetaCategory>();
            metaCategories.Add(CardMetaCategory.Rare);

            List<Ability> abilities = new List<Ability>();
            abilities.Add(Ability.DrawCopyOnDeath);
            abilities.Add(Ability.TriStrike);

            List<CardAppearanceBehaviour.Appearance> appearanceBehaviour = new List<CardAppearanceBehaviour.Appearance>();
            appearanceBehaviour.Add(CardAppearanceBehaviour.Appearance.RareCardBackground);

            byte[] imgBytes = System.IO.File.ReadAllBytes(Path.Combine(this.Info.Location.Replace("GrimoraMod.dll", ""), "Artwork/Wendigo.png"));
            Texture2D tex = new Texture2D(2, 2);
            tex.LoadImage(imgBytes);
            NewCard.Add("ara_Hydra", "Hydra", 1, 1, metaCategories, CardComplexity.Vanilla, CardTemple.Nature, description: "Described by some as the truest nightmare", bonesCost: 4, abilities: abilities, defaultTex: tex, appearanceBehaviour: appearanceBehaviour);
        }

        private void AddAra_Ember_spirit()
        {
            List<CardMetaCategory> metaCategories = new List<CardMetaCategory>();
            metaCategories.Add(CardMetaCategory.Rare);

            List<CardAppearanceBehaviour.Appearance> appearanceBehaviour = new List<CardAppearanceBehaviour.Appearance>();
            appearanceBehaviour.Add(CardAppearanceBehaviour.Appearance.RareCardBackground);

            List<Ability> abilities = new List<Ability>();
            abilities.Add(Flamestrafe.ability);

            byte[] imgBytes = System.IO.File.ReadAllBytes(Path.Combine(this.Info.Location.Replace("GrimoraMod.dll", ""), "Artwork/ember.png"));
            Texture2D tex = new Texture2D(2, 2);
            tex.LoadImage(imgBytes);

            byte[] imgBytes1 = System.IO.File.ReadAllBytes(Path.Combine(this.Info.Location.Replace("GrimoraMod.dll", ""), "Artwork/Energy3.png"));
            Texture2D tex1 = new Texture2D(2, 2);
            tex1.LoadImage(imgBytes1);

            List<Texture> decals = new();
            decals.Add(tex1);

            NewCard.Add("ara_Ember_spirit", "Spirit of Ember", 1, 3, metaCategories, CardComplexity.Simple, CardTemple.Nature, description: "A trickster Spirit fleeing and leaving behind its Flames.", bonesCost: 2, energyCost: 3, appearanceBehaviour: appearanceBehaviour, defaultTex: tex, decals: decals, abilities: abilities);
        }

        /// Card Edits


        private void RemoveAll()
        {
            List<string> cards = new List<string> {"placeholder", "Adder", "Alpha", "Amalgam", "Ant", "AntQueen", "Bee", "Beaver", "Beehive", "Bloodhound", "Bullfrog", "Cat", "Cockroach", "Daus", "Elk", "ElkCub", "FieldMouse", "Geck", "Grizzly", "JerseyDevil",
            "Kingfisher", "Magpie", "Mantis", "MantisGod", "Mole", "MoleMan", "Moose", "Mothman_Stage1", "Opossum", "Otter", "Ouroboros", "Porcupine", "Pronghorn", "RatKing", "Raven", "RavenEgg", "Shark", "Skink", "Skunk", "Snapper",
            "Sparrow", "SquidCards", "SquidBell", "SquidMirror", "Urayuli", "Warren", "Wolf", "WolfCub", "PeltHare", "PeltWolf", "PeltGolden", "RingWorm", "Stinkbug_Talking", "Stoat_Talking", "Wolf_Talking", "!STATIC!GLITCH", "Snelk", "Coyote"};

            for (int i = 1; i < cards.Count; i++)
            {
                List<CardMetaCategory> metaCategories = new List<CardMetaCategory>();
                new CustomCard(cards[i]) { metaCategories = metaCategories };
            }
        }

        private void ChangeSquirrel()
        {
            List<Ability> abilities = new List<Ability>();
            abilities.Add(Ability.Brittle);

            byte[] imgBytes = System.IO.File.ReadAllBytes(Path.Combine(this.Info.Location.Replace("GrimoraMod.dll", ""), "Artwork/Skeleton.png"));
            Texture2D tex = new Texture2D(2, 2);
            tex.LoadImage(imgBytes);

            new CustomCard("Squirrel")
            {
                displayedName = "Skeleton",
                baseAttack = 1,
                abilities = abilities,
                tex = tex
            };

        }

        private void ChangeGoat()
        {
            List<CardMetaCategory> metaCategories = new List<CardMetaCategory>();

            List<Ability> abilities = new List<Ability>();
            abilities.Add(Ability.SkeletonStrafe);

            byte[] imgBytes = System.IO.File.ReadAllBytes(Path.Combine(this.Info.Location.Replace("GrimoraMod.dll", ""), "Artwork/skeletonarmy.png"));
            Texture2D tex = new Texture2D(2, 2);
            tex.LoadImage(imgBytes);

            byte[] imgBytes1 = System.IO.File.ReadAllBytes(Path.Combine(this.Info.Location.Replace("GrimoraMod.dll", ""), "Artwork/Energy2.png"));
            Texture2D tex1 = new Texture2D(2, 2);
            tex1.LoadImage(imgBytes1);

            List<Texture> decals = new();
            decals.Add(tex1);

            new CustomCard("Goat")
            {
                displayedName = "Skeleton Army",
                baseAttack = 2,
                baseHealth = 4,
                energyCost = 2,
                cost = 0,
                tex = tex,
                abilities = abilities,
                decals = decals,
                metaCategories = metaCategories,
                description = "The Skeleton Army, boons of the Bone Lord"
            };

        }

        private void ChangePackRat()
        {
            List<CardMetaCategory> metaCategories = new List<CardMetaCategory>();

            List<Ability> abilities = new List<Ability>();
            abilities.Add(Ability.GuardDog);

            byte[] imgBytes = System.IO.File.ReadAllBytes(Path.Combine(this.Info.Location.Replace("GrimoraMod.dll", ""), "Artwork/Skelemaniac.png"));
            Texture2D tex = new Texture2D(2, 2);
            tex.LoadImage(imgBytes);

            new CustomCard("PackRat")
            {
                displayedName = "Skelemaniac",
                baseAttack = 1,
                cost = 0,
                bonesCost = 4,
                baseHealth = 3,
                abilities = abilities,
                tex = tex,
                metaCategories = metaCategories,
                description = "A Skeleton gone Mad, at least it follows your command."

            };


        }






        ///sigils
        private NewAbility CreateFlamestrafe()
        {
            AbilityInfo info = ScriptableObject.CreateInstance<AbilityInfo>();
            info.powerLevel = 5;
            info.rulebookName = "Flighty Flames";
            info.rulebookDescription = "Whenever this Card moves, it leaves a trail of Embers.The warmth of the Embers shall enlighten nearby Cards.";
            info.metaCategories = new List<AbilityMetaCategory> { AbilityMetaCategory.Part1Rulebook, AbilityMetaCategory.Part1Modular };

            byte[] imgBytes = System.IO.File.ReadAllBytes(Path.Combine(this.Info.Location.Replace("GrimoraMod.dll", ""), "Artwork/dropflames.png"));
            Texture2D tex = new Texture2D(2, 2);
            tex.LoadImage(imgBytes);

            NewAbility flamestrafe = new NewAbility(info, typeof(Flamestrafe), tex);
            Flamestrafe.ability = flamestrafe.ability;
            return flamestrafe;
        }

        public class Flamestrafe : Strafe
        {
            // Token: 0x1700029D RID: 669
            // (get) Token: 0x06001418 RID: 5144 RVA: 0x000444B8 File Offset: 0x000426B8
            public override Ability Ability
            {
                get
                {
                    return ability;
                }
            }


            public static Ability ability;

            // Token: 0x06001419 RID: 5145 RVA: 0x000444BC File Offset: 0x000426BC
            public override IEnumerator PostSuccessfulMoveSequence(CardSlot cardSlot)
            {
                if (cardSlot.Card == null)
                {
                    yield return Singleton<BoardManager>.Instance.CreateCardInSlot(CardLoader.GetCardByName("ara_Flames"), cardSlot, 0.1f, true);
                }
                yield break;
            }
        }








    }


}