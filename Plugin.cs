using BepInEx;
using BepInEx.Logging;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;
using System.IO;
using DiskCardGame;
using HarmonyLib;
using UnityEngine;
using APIPlugin;

namespace GrimoraMod
{
	[BepInPlugin(PluginGuid, PluginName, PluginVersion)]
	[BepInDependency("cyantist.inscryption.api", BepInDependency.DependencyFlags.HardDependency)]
	public class Plugin : BaseUnityPlugin
	{
		private const string PluginGuid = "arackulele.inscryption.grimoramod";
		private const string PluginName = "GrimoraMod";
		private const string PluginVersion = "0.1";

		private void Awake()
		{
			Logger.LogInfo($"Loaded {PluginName}!");

			AddBoneDigger();
			AddBonelord();
			AddBonePile();
			AddBonePrince();
			AddCrazedMantis();
			AddDeadHand();
			AddDraugr();
			AddDrownedSoul();
			AddFranknstein();
			AddGraveRobber();
			AddHeadlessHorseman();
			AddBonelordsHorn();
			AddRingWorm();
			Addmummylord();
			AddNecromancer();
			Addobol();
			AddPets();
			AddPirateship();
			AddRevenant();
			Addsarcophagus();
			AddSerpent();
			AddSkelemancer();
			AddSkeletonMage();
			AddSnapper();
			AddSporeDigger();
			AddWolf();
			AddZombGeck();
			AddZombie();

			RemoveAll();
			ChangeSquirrel();
		}

		private void AddBoneDigger()
		{
			List<CardMetaCategory> metaCategories = new List<CardMetaCategory>();
			metaCategories.Add(CardMetaCategory.ChoiceNode);
			metaCategories.Add(CardMetaCategory.TraderOffer);

			List<Ability> abilities = new List<Ability>();
			abilities.Add(Ability.BoneDigger);

			byte[] imgBytes = System.IO.File.ReadAllBytes(Path.Combine(this.Info.Location.Replace("GrimoraMod.dll",""),"Artwork/Goat.png"));
			Texture2D tex = new Texture2D(2,2);
			tex.LoadImage(imgBytes);
			NewCard.Add("BoneDigger", "Gravedigger", 0, 3, metaCategories, CardComplexity.Simple, CardTemple.Nature, description:"He spends his time alone digging for Bones in hopes of finding a Tresure.", bonesCost:1, abilities:abilities, defaultTex:tex);
		}

		private void AddBonelord()
		{
			List<CardMetaCategory> metaCategories = new List<CardMetaCategory>();
			metaCategories.Add(CardMetaCategory.Rare);

			List<CardAppearanceBehaviour.Appearance> appearanceBehaviour = new List<CardAppearanceBehaviour.Appearance>();
			appearanceBehaviour.Add(CardAppearanceBehaviour.Appearance.RareCardBackground);

			List<Ability> abilities = new List<Ability>();
			abilities.Add(Ability.Deathtouch);

			byte[] imgBytes = System.IO.File.ReadAllBytes(Path.Combine(this.Info.Location.Replace("GrimoraMod.dll",""),"Artwork/bone_lord.png"));
			Texture2D tex = new Texture2D(2,2);
			tex.LoadImage(imgBytes);
			NewCard.Add("Bonelord", "The Bone Lord", 5, 10, metaCategories, CardComplexity.Simple, CardTemple.Nature, description:"Lord of Bones, Lord of Bones answer our call.", bonesCost:6, energyCost:6, appearanceBehaviour:appearanceBehaviour, abilities:abilities, defaultTex:tex);
		}

		private void AddBonePile()
		{
			List<CardMetaCategory> metaCategories = new List<CardMetaCategory>();
			metaCategories.Add(CardMetaCategory.ChoiceNode);
			metaCategories.Add(CardMetaCategory.TraderOffer);

			List<Ability> abilities = new List<Ability>();
			abilities.Add(Ability.QuadrupleBones);

			byte[] imgBytes = System.IO.File.ReadAllBytes(Path.Combine(this.Info.Location.Replace("GrimoraMod.dll",""),"Artwork/BonePile.png"));
			Texture2D tex = new Texture2D(2,2);
			tex.LoadImage(imgBytes);
			NewCard.Add("BonePile", "Bone Heap", 0, 2, metaCategories, CardComplexity.Vanilla, CardTemple.Nature, description:"An uninspiring Pile of Bones, you can have it.", bonesCost:1, abilities:abilities, defaultTex:tex);
		}

		private void AddBonePrince()
		{
			List<CardMetaCategory> metaCategories = new List<CardMetaCategory>();
			metaCategories.Add(CardMetaCategory.GBCPlayable);

			byte[] imgBytes = System.IO.File.ReadAllBytes(Path.Combine(this.Info.Location.Replace("GrimoraMod.dll",""),"Artwork/BonePrince.png"));
			Texture2D tex = new Texture2D(2,2);
			tex.LoadImage(imgBytes);
			NewCard.Add("BonePrince", "Bone Prince", 2, 1, metaCategories, CardComplexity.Simple, CardTemple.Nature, bonesCost:1, defaultTex:tex);
		}

		private void AddCrazedMantis()
		{
			List<CardMetaCategory> metaCategories = new List<CardMetaCategory>();
			metaCategories.Add(CardMetaCategory.ChoiceNode);
			metaCategories.Add(CardMetaCategory.TraderOffer);

			List<Ability> abilities = new List<Ability>();
			abilities.Add(Ability.SplitStrike);

			byte[] imgBytes = System.IO.File.ReadAllBytes(Path.Combine(this.Info.Location.Replace("GrimoraMod.dll",""),"Artwork/Mantis.png"));
			Texture2D tex = new Texture2D(2,2);
			tex.LoadImage(imgBytes);
			NewCard.Add("CrazedMantis", "Crazed Mantis", 1, 1, metaCategories, CardComplexity.Vanilla, CardTemple.Nature, description:"The poor mantis has gone Insane, a gruesome fate.", bonesCost:4, abilities:abilities, defaultTex:tex);
		}

		private void AddDeadHand()
		{
			List<CardMetaCategory> metaCategories = new List<CardMetaCategory>();
			metaCategories.Add(CardMetaCategory.ChoiceNode);
			metaCategories.Add(CardMetaCategory.TraderOffer);

			List<Ability> abilities = new List<Ability>();
			abilities.Add(Ability.DrawNewHand);

			byte[] imgBytes = System.IO.File.ReadAllBytes(Path.Combine(this.Info.Location.Replace("GrimoraMod.dll",""),"Artwork/DeadHand.png"));
			Texture2D tex = new Texture2D(2,2);
			tex.LoadImage(imgBytes);
			NewCard.Add("DeadHand", "Dead Hand", 1, 1, metaCategories, CardComplexity.Intermediate, CardTemple.Nature, description:"Cut off from an ancient God, the Dead Hand took on its own Life.", bonesCost:5, abilities:abilities, defaultTex:tex);
		}

		private void AddDraugr()
		{
			List<CardMetaCategory> metaCategories = new List<CardMetaCategory>();
			metaCategories.Add(CardMetaCategory.ChoiceNode);
			metaCategories.Add(CardMetaCategory.TraderOffer);

			List<Ability> abilities = new List<Ability>();
			abilities.Add(Ability.IceCube);

			byte[] imgBytes = System.IO.File.ReadAllBytes(Path.Combine(this.Info.Location.Replace("GrimoraMod.dll",""),"Artwork/Skelarmor.png"));
			Texture2D tex = new Texture2D(2,2);
			tex.LoadImage(imgBytes);
			NewCard.Add("Draugr", "Draugr", 0, 1, metaCategories, CardComplexity.Simple, CardTemple.Nature, description:"Hiding in a Suit of Armor, this Skeleton wont last forever.", bonesCost:1, abilities:abilities, defaultTex:tex);
		}

		private void AddDrownedSoul()
		{
			List<CardMetaCategory> metaCategories = new List<CardMetaCategory>();
			metaCategories.Add(CardMetaCategory.ChoiceNode);
			metaCategories.Add(CardMetaCategory.TraderOffer);

			List<Ability> abilities = new List<Ability>();
			abilities.Add(Ability.Deathtouch);
			abilities.Add(Ability.Submerge);

			byte[] imgBytes = System.IO.File.ReadAllBytes(Path.Combine(this.Info.Location.Replace("GrimoraMod.dll",""),"Artwork/DrownedSoul.png"));
			Texture2D tex = new Texture2D(2,2);
			tex.LoadImage(imgBytes);
			NewCard.Add("DrownedSoul", "Drowned Soul", 1, 1, metaCategories, CardComplexity.Simple, CardTemple.Nature, description:"Going into that Well wasnt the best idea.", bonesCost:4, abilities:abilities, defaultTex:tex);
		}

		private void AddFranknstein()
		{
			List<CardMetaCategory> metaCategories = new List<CardMetaCategory>();
			metaCategories.Add(CardMetaCategory.ChoiceNode);
			metaCategories.Add(CardMetaCategory.TraderOffer);

			byte[] imgBytes = System.IO.File.ReadAllBytes(Path.Combine(this.Info.Location.Replace("GrimoraMod.dll",""),"Artwork/Franknstein.png"));
			Texture2D tex = new Texture2D(2,2);
			tex.LoadImage(imgBytes);
			NewCard.Add("Franknstein", "Frank & Stein", 2, 2, metaCategories, CardComplexity.Vanilla, CardTemple.Nature, description:"Frank and Stein, best Friends brothers and Fighters.", bonesCost:5, defaultTex:tex);
		}

		private void AddGraveRobber()
		{
			List<CardMetaCategory> metaCategories = new List<CardMetaCategory>();
			metaCategories.Add(CardMetaCategory.ChoiceNode);
			metaCategories.Add(CardMetaCategory.TraderOffer);

			List<Ability> abilities = new List<Ability>();
			abilities.Add(Ability.ExplodeOnDeath);

			byte[] imgBytes = System.IO.File.ReadAllBytes(Path.Combine(this.Info.Location.Replace("GrimoraMod.dll",""),"Artwork/GraveRobber.png"));
			Texture2D tex = new Texture2D(2,2);
			tex.LoadImage(imgBytes);
			NewCard.Add("GraveRobber", "Tomb Robber", 0, 1, metaCategories, CardComplexity.Intermediate, CardTemple.Nature, description:"Nothing, nothing again, no treasure is left anymore.", bonesCost:0, abilities:abilities, defaultTex:tex);
		}

		private void AddHeadlessHorseman()
		{
			List<CardMetaCategory> metaCategories = new List<CardMetaCategory>();
			metaCategories.Add(CardMetaCategory.ChoiceNode);
			metaCategories.Add(CardMetaCategory.TraderOffer);

			List<Ability> abilities = new List<Ability>();
			abilities.Add(Ability.Strafe);
			abilities.Add(Ability.Flying);

			byte[] imgBytes = System.IO.File.ReadAllBytes(Path.Combine(this.Info.Location.Replace("GrimoraMod.dll",""),"Artwork/HeadlessHorseman.png"));
			Texture2D tex = new Texture2D(2,2);
			tex.LoadImage(imgBytes);
			NewCard.Add("HeadlessHorseman", "Headless Horseman", 4, 3, metaCategories, CardComplexity.Vanilla, CardTemple.Nature, description:"The apocalypse is soon, get ready.", bonesCost:9, abilities:abilities, defaultTex:tex);
		}

		private void AddBonelordsHorn()
		{
			List<CardMetaCategory> metaCategories = new List<CardMetaCategory>();
			metaCategories.Add(CardMetaCategory.Rare);

			List<CardAppearanceBehaviour.Appearance> appearanceBehaviour = new List<CardAppearanceBehaviour.Appearance>();
			appearanceBehaviour.Add(CardAppearanceBehaviour.Appearance.RareCardBackground);

			List<Ability> abilities = new List<Ability>();
			abilities.Add(Ability.QuadrupleBones);
			abilities.Add(Ability.IceCube);

			byte[] imgBytes = System.IO.File.ReadAllBytes(Path.Combine(this.Info.Location.Replace("GrimoraMod.dll",""),"Artwork/BonelordsHorn.png"));
			Texture2D tex = new Texture2D(2,2);
			tex.LoadImage(imgBytes);
			NewCard.Add("BonelordsHorn", "Bone Lords Horn", 0, 1, metaCategories, CardComplexity.Simple, CardTemple.Nature, description:"The Horn of the Bonelord, you do not want to find out whats inside.", energyCost:4, appearanceBehaviour:appearanceBehaviour, abilities:abilities, defaultTex:tex);
		}

		private void AddRingWorm()
		{
			List<CardMetaCategory> metaCategories = new List<CardMetaCategory>();
			metaCategories.Add(CardMetaCategory.ChoiceNode);
			metaCategories.Add(CardMetaCategory.TraderOffer);

			List<Ability> abilities = new List<Ability>();
			abilities.Add(Ability.BoneDigger);

			byte[] imgBytes = System.IO.File.ReadAllBytes(Path.Combine(this.Info.Location.Replace("GrimoraMod.dll",""),"Artwork/RingWorm.png"));
			Texture2D tex = new Texture2D(2,2);
			tex.LoadImage(imgBytes);
			NewCard.Add("RingWorm", "Mudworm", 2, 1, metaCategories, CardComplexity.Simple, CardTemple.Nature, description:"The Mudworm digs through the Earth day in day out.", bonesCost:5, abilities:abilities, defaultTex:tex);
		}

		private void Addmummylord()
		{
			List<CardMetaCategory> metaCategories = new List<CardMetaCategory>();
			metaCategories.Add(CardMetaCategory.GBCPlayable);

			byte[] imgBytes = System.IO.File.ReadAllBytes(Path.Combine(this.Info.Location.Replace("GrimoraMod.dll",""),"Artwork/Mummy.png"));
			Texture2D tex = new Texture2D(2,2);
			tex.LoadImage(imgBytes);
			NewCard.Add("mummylord", "Mummy Lord", 3, 3, metaCategories, CardComplexity.Advanced, CardTemple.Nature,  description:"The Cycle of the Mummy Lord, never ending.", bonesCost:2, defaultTex:tex);
		}

		private void AddNecromancer()
		{
			List<CardMetaCategory> metaCategories = new List<CardMetaCategory>();
			metaCategories.Add(CardMetaCategory.ChoiceNode);
			metaCategories.Add(CardMetaCategory.TraderOffer);

			List<Ability> abilities = new List<Ability>();
			abilities.Add(Ability.DoubleDeath);

			byte[] imgBytes = System.IO.File.ReadAllBytes(Path.Combine(this.Info.Location.Replace("GrimoraMod.dll",""),"Artwork/Necromancer.png"));
			Texture2D tex = new Texture2D(2,2);
			tex.LoadImage(imgBytes);
			NewCard.Add("Necromancer", "Necromancer", 2, 1, metaCategories, CardComplexity.Vanilla, CardTemple.Nature, description:"The vicious Necromancer, nothing dies once.", bonesCost:3, abilities:abilities, defaultTex:tex);
		}

		private void Addobol()
		{
			List<CardMetaCategory> metaCategories = new List<CardMetaCategory>();
			metaCategories.Add(CardMetaCategory.ChoiceNode);
			metaCategories.Add(CardMetaCategory.TraderOffer);

			List<Ability> abilities = new List<Ability>();
			abilities.Add(Ability.Sharp);

			byte[] imgBytes = System.IO.File.ReadAllBytes(Path.Combine(this.Info.Location.Replace("GrimoraMod.dll",""),"Artwork/Obol.png"));
			Texture2D tex = new Texture2D(2,2);
			tex.LoadImage(imgBytes);
			NewCard.Add("obol", "Ancient Obol", 0, 6, metaCategories, CardComplexity.Simple, CardTemple.Nature, description:"The Ancient Obol, the Bone Lord likes this one.", energyCost:3, abilities:abilities, defaultTex:tex);
		}

		private void AddPets()
		{
			List<CardMetaCategory> metaCategories = new List<CardMetaCategory>();
			metaCategories.Add(CardMetaCategory.Rare);

			List<CardAppearanceBehaviour.Appearance> appearanceBehaviour = new List<CardAppearanceBehaviour.Appearance>();
			appearanceBehaviour.Add(CardAppearanceBehaviour.Appearance.RareCardBackground);

			List<Ability> abilities = new List<Ability>();
			abilities.Add(Ability.DrawCopyOnDeath);

			byte[] imgBytes = System.IO.File.ReadAllBytes(Path.Combine(this.Info.Location.Replace("GrimoraMod.dll",""),"Artwork/Pets.png"));
			Texture2D tex = new Texture2D(2,2);
			tex.LoadImage(imgBytes);
			NewCard.Add("Pets", "Pharaohs Pets", 2, 1, metaCategories, CardComplexity.Vanilla, CardTemple.Nature, description:"The Undying Pets of the Pharao, enchanting.", bonesCost:5, appearanceBehaviour:appearanceBehaviour, abilities:abilities, defaultTex:tex);
		}

		private void AddPirateship()
		{
			List<CardMetaCategory> metaCategories = new List<CardMetaCategory>();
			metaCategories.Add(CardMetaCategory.ChoiceNode);
			metaCategories.Add(CardMetaCategory.TraderOffer);

			List<Ability> abilities = new List<Ability>();
			abilities.Add(Ability.SkeletonStrafe);
			abilities.Add(Ability.Submerge);

			byte[] imgBytes = System.IO.File.ReadAllBytes(Path.Combine(this.Info.Location.Replace("GrimoraMod.dll",""),"Artwork/Pirateship.png"));
			Texture2D tex = new Texture2D(2,2);
			tex.LoadImage(imgBytes);
			NewCard.Add("Pirateship", "Ghost Ship", 0, 1, metaCategories, CardComplexity.Simple, CardTemple.Nature, description:"The Skeleton Army never rests.", bonesCost:4, abilities:abilities, defaultTex:tex);
		}

		private void AddRevenant()
		{
			List<CardMetaCategory> metaCategories = new List<CardMetaCategory>();
			metaCategories.Add(CardMetaCategory.ChoiceNode);
			metaCategories.Add(CardMetaCategory.TraderOffer);

			List<Ability> abilities = new List<Ability>();
			abilities.Add(Ability.Brittle);

			byte[] imgBytes = System.IO.File.ReadAllBytes(Path.Combine(this.Info.Location.Replace("GrimoraMod.dll",""),"Artwork/Revenant.png"));
			Texture2D tex = new Texture2D(2,2);
			tex.LoadImage(imgBytes);
			NewCard.Add("Revenant", "Revenant", 3, 1, metaCategories, CardComplexity.Intermediate, CardTemple.Nature, description:"The Revenant, bringing the Scythe of death.", bonesCost:3, abilities:abilities, defaultTex:tex);
		}

		private void Addsarcophagus()
		{
			List<CardMetaCategory> metaCategories = new List<CardMetaCategory>();
			metaCategories.Add(CardMetaCategory.ChoiceNode);
			metaCategories.Add(CardMetaCategory.TraderOffer);

			List<Ability> abilities = new List<Ability>();
			abilities.Add(Ability.Evolve);

			byte[] imgBytes = System.IO.File.ReadAllBytes(Path.Combine(this.Info.Location.Replace("GrimoraMod.dll",""),"Artwork/Sarcopagus.png"));
			Texture2D tex = new Texture2D(2,2);
			tex.LoadImage(imgBytes);
			NewCard.Add("sarcophagus", "Sarcophagus", 0, 2, metaCategories, CardComplexity.Advanced, CardTemple.Nature, description:"The Cycle of the Mummy Lord, never ending.", bonesCost:4, abilities:abilities, defaultTex:tex, evolveId:new EvolveIdentifier("mummylord",1));
		}

		private void AddSerpent()
		{
			List<CardMetaCategory> metaCategories = new List<CardMetaCategory>();
			metaCategories.Add(CardMetaCategory.ChoiceNode);
			metaCategories.Add(CardMetaCategory.TraderOffer);

			List<Ability> abilities = new List<Ability>();
			abilities.Add(Ability.Deathtouch);

			byte[] imgBytes = System.IO.File.ReadAllBytes(Path.Combine(this.Info.Location.Replace("GrimoraMod.dll",""),"Artwork/Adder.png"));
			Texture2D tex = new Texture2D(2,2);
			tex.LoadImage(imgBytes);
			NewCard.Add("Serpent", "Bone Serpent", 1, 1, metaCategories, CardComplexity.Vanilla, CardTemple.Nature, description:"Serpent of Bones, its poison can melt even Bones.", bonesCost:4, abilities:abilities, defaultTex:tex);
		}

		private void AddSkelemancer()
		{
			List<CardMetaCategory> metaCategories = new List<CardMetaCategory>();
			metaCategories.Add(CardMetaCategory.ChoiceNode);
			metaCategories.Add(CardMetaCategory.TraderOffer);

			byte[] imgBytes = System.IO.File.ReadAllBytes(Path.Combine(this.Info.Location.Replace("GrimoraMod.dll",""),"Artwork/SkeletonJuniorSage.png"));
			Texture2D tex = new Texture2D(2,2);
			tex.LoadImage(imgBytes);
			NewCard.Add("Skelemancer", "Skelemancer", 1, 1, metaCategories, CardComplexity.Simple, CardTemple.Nature, description:"The humble Skelemancer, he likes a good fight.", energyCost:2, defaultTex:tex);
		}

		private void AddSkeletonMage()
		{
			List<CardMetaCategory> metaCategories = new List<CardMetaCategory>();
			metaCategories.Add(CardMetaCategory.ChoiceNode);
			metaCategories.Add(CardMetaCategory.TraderOffer);

			List<Ability> abilities = new List<Ability>();
			abilities.Add(Ability.Deathtouch);

			byte[] imgBytes = System.IO.File.ReadAllBytes(Path.Combine(this.Info.Location.Replace("GrimoraMod.dll",""),"Artwork/SkeletonMage.png"));
			Texture2D tex = new Texture2D(2,2);
			tex.LoadImage(imgBytes);
			NewCard.Add("SkeletonMage", "Skelemagus", 1, 4, metaCategories, CardComplexity.Simple, CardTemple.Nature, description:"The Skelemagus, they have learned the Ancient Spell of Death.", energyCost:4, abilities:abilities, defaultTex:tex);
		}

		private void AddSnapper()
		{
			List<CardMetaCategory> metaCategories = new List<CardMetaCategory>();
			metaCategories.Add(CardMetaCategory.ChoiceNode);
			metaCategories.Add(CardMetaCategory.TraderOffer);

			byte[] imgBytes = System.IO.File.ReadAllBytes(Path.Combine(this.Info.Location.Replace("GrimoraMod.dll",""),"Artwork/Snapper.png"));
			Texture2D tex = new Texture2D(2,2);
			tex.LoadImage(imgBytes);
			NewCard.Add("Snapper", "Bone Snapper", 1, 6, metaCategories, CardComplexity.Vanilla, CardTemple.Nature, description:"Snap snap your Bones are gone.", bonesCost:5, defaultTex:tex);
		}

		private void AddSporeDigger()
		{
			List<CardMetaCategory> metaCategories = new List<CardMetaCategory>();
			metaCategories.Add(CardMetaCategory.Rare);

			List<CardAppearanceBehaviour.Appearance> appearanceBehaviour = new List<CardAppearanceBehaviour.Appearance>();
			appearanceBehaviour.Add(CardAppearanceBehaviour.Appearance.RareCardBackground);

			List<Ability> abilities = new List<Ability>();
			abilities.Add(Ability.BoneDigger);
			abilities.Add(Ability.BoneDigger);

			byte[] imgBytes = System.IO.File.ReadAllBytes(Path.Combine(this.Info.Location.Replace("GrimoraMod.dll",""),"Artwork/SporeDigger.png"));
			Texture2D tex = new Texture2D(2,2);
			tex.LoadImage(imgBytes);
			NewCard.Add("SporeDigger", "Sporedigger", 0, 3, metaCategories, CardComplexity.Simple, CardTemple.Nature, description:"The Spore Sigger, an excellent digger.", bonesCost:1, appearanceBehaviour:appearanceBehaviour, abilities:abilities, defaultTex:tex);
		}

		private void AddWolf()
		{
			List<CardMetaCategory> metaCategories = new List<CardMetaCategory>();
			metaCategories.Add(CardMetaCategory.ChoiceNode);
			metaCategories.Add(CardMetaCategory.TraderOffer);

			byte[] imgBytes = System.IO.File.ReadAllBytes(Path.Combine(this.Info.Location.Replace("GrimoraMod.dll",""),"Artwork/Wolf.png"));
			Texture2D tex = new Texture2D(2,2);
			tex.LoadImage(imgBytes);
			NewCard.Add("Wolf", "Undead Wolf", 3, 2, metaCategories, CardComplexity.Vanilla, CardTemple.Nature, description:"A diseased Wolf, but the Pack stays Strong.", bonesCost:7, defaultTex:tex);
		}

		private void AddZombGeck()
		{
			List<CardMetaCategory> metaCategories = new List<CardMetaCategory>();
			metaCategories.Add(CardMetaCategory.Rare);

			List<Ability> abilities = new List<Ability>();
			abilities.Add(Ability.Brittle);

			byte[] imgBytes = System.IO.File.ReadAllBytes(Path.Combine(this.Info.Location.Replace("GrimoraMod.dll",""),"Artwork/Geck.png"));
			Texture2D tex = new Texture2D(2,2);
			tex.LoadImage(imgBytes);
			NewCard.Add("ZombGeck", "Zomb-Geck", 1, 1, metaCategories, CardComplexity.Vanilla, CardTemple.Nature, description:"Da Geeckkkkk is deaddd nooooooooo.", abilities:abilities, defaultTex:tex);
		}

		private void AddZombie()
		{
			List<CardMetaCategory> metaCategories = new List<CardMetaCategory>();
			metaCategories.Add(CardMetaCategory.ChoiceNode);
			metaCategories.Add(CardMetaCategory.TraderOffer);

			byte[] imgBytes = System.IO.File.ReadAllBytes(Path.Combine(this.Info.Location.Replace("GrimoraMod.dll",""),"Artwork/Zombie.png"));
			Texture2D tex = new Texture2D(2,2);
			tex.LoadImage(imgBytes);
			NewCard.Add("Zombie", "Zombie", 1, 1, metaCategories, CardComplexity.Simple, CardTemple.Nature, description:"The humble Zombie, a respected member of the Army." , bonesCost: 2, defaultTex:tex);
		}

		private void RemoveAll()
		{
			List<string> cards = new List<string> { "AACard", "Adder", "Alpha", "Amalgam", "Ant", "AntQueen", "Bee", "Beaver", "Beehive", "Bloodhound", "Bullfrog", "Cat", "Cockroach", "Daus", "Elk", "ElkCub", "FieldMouse", "Geck", "Goat", "Grizzly", "JerseyDevil",
			"Kingfisher", "Magpie", "Mantis", "MantisGod", "Mole", "MoleMan", "Moose", "Mothman_Stage1", "Opossum", "Otter", "Ouroboros", "PackRat", "Porcupine", "Pronghorn", "RatKing", "Raven", "RavenEgg", "Shark", "Skink", "Skunk", "Snapper",
			"Sparrow", "SquidCards", "SquidBell", "SquidMirror", "Urayuli", "Warren", "Wolf", "WolfCub", "PeltHare", "PeltWolf", "PeltGolden", "RingWorm", "Stinkbug_Talking", "Stoat_Talking", "Wolf_Talking"};
			
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

	}
}
