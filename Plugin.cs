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
			NewCard.Add("BoneDigger", metaCategories, CardComplexity.Simple, CardTemple.Nature, "Gravedigger", 0, 3, description:"He spends his time alone digging for Bones in hopes of finding a Tresure.", bonesCost:1, abilities:abilities, tex:tex);
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
			NewCard.Add("Bonelord", metaCategories, CardComplexity.Simple, CardTemple.Nature, "The Bone Lord", 5, 10, description:"Lord of Bones, Lord of Bones answer our call.", bonesCost:6, energyCost:6, appearanceBehaviour:appearanceBehaviour, abilities:abilities, tex:tex);
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
			NewCard.Add("BonePile", metaCategories, CardComplexity.Vanilla, CardTemple.Nature, "Bone Heap", 0, 2, description:"An uninspiring Pile of Bones, you can have it.", bonesCost:1, abilities:abilities, tex:tex);
		}

		private void AddBonePrince()
		{
			List<CardMetaCategory> metaCategories = new List<CardMetaCategory>();
			metaCategories.Add(CardMetaCategory.GBCPlayable);

			byte[] imgBytes = System.IO.File.ReadAllBytes(Path.Combine(this.Info.Location.Replace("GrimoraMod.dll",""),"Artwork/BonePrince.png"));
			Texture2D tex = new Texture2D(2,2);
			tex.LoadImage(imgBytes);
			NewCard.Add("BonePrince", metaCategories, CardComplexity.Simple, CardTemple.Nature, "Bone Prince", 2, 1, bonesCost:1, tex:tex);
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
			NewCard.Add("CrazedMantis", metaCategories, CardComplexity.Vanilla, CardTemple.Nature, "Crazed Mantis", 1, 1, description:"The poor mantis has gone Insane, a gruesome fate.", bonesCost:4, abilities:abilities, tex:tex);
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
			NewCard.Add("DeadHand", metaCategories, CardComplexity.Intermediate, CardTemple.Nature, "Dead Hand", 1, 1, description:"Cut off from an ancient God, the Dead Hand took on its own Life.", bonesCost:5, abilities:abilities, tex:tex);
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
			NewCard.Add("Draugr", metaCategories, CardComplexity.Simple, CardTemple.Nature, "Draugr", 0, 1, description:"Hiding in a Suit of Armor, this Skeleton wont last forever.", bonesCost:1, abilities:abilities, tex:tex);
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
			NewCard.Add("DrownedSoul", metaCategories, CardComplexity.Simple, CardTemple.Nature, "Drowned Soul", 1, 1, description:"Going into that Well wasnt the best idea.", bonesCost:4, abilities:abilities, tex:tex);
		}

		private void AddFranknstein()
		{
			List<CardMetaCategory> metaCategories = new List<CardMetaCategory>();
			metaCategories.Add(CardMetaCategory.ChoiceNode);
			metaCategories.Add(CardMetaCategory.TraderOffer);

			byte[] imgBytes = System.IO.File.ReadAllBytes(Path.Combine(this.Info.Location.Replace("GrimoraMod.dll",""),"Artwork/Franknstein.png"));
			Texture2D tex = new Texture2D(2,2);
			tex.LoadImage(imgBytes);
			NewCard.Add("Franknstein", metaCategories, CardComplexity.Vanilla, CardTemple.Nature, "Frank & Stein", 2, 2, description:"Frank and Stein, best Friends brothers and Fighters.", bonesCost:5, tex:tex);
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
			NewCard.Add("GraveRobber", metaCategories, CardComplexity.Intermediate, CardTemple.Nature, "Tomb Robber", 0, 1, description:"Nothing, nothing again, no treasure is left anymore.", bonesCost:0, abilities:abilities, tex:tex);
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
			NewCard.Add("HeadlessHorseman", metaCategories, CardComplexity.Vanilla, CardTemple.Nature, "Headless Horseman", 4, 3, description:"The apocalypse is soon, get ready.", bonesCost:9, abilities:abilities, tex:tex);
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
			NewCard.Add("BonelordsHorn", metaCategories, CardComplexity.Simple, CardTemple.Nature, "Bone Lords Horn", 0, 1, description:"The Horn of the Bonelord, you do not want to find out whats inside.", energyCost:4, appearanceBehaviour:appearanceBehaviour, abilities:abilities, tex:tex);
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
			NewCard.Add("RingWorm", metaCategories, CardComplexity.Simple, CardTemple.Nature, "Mudworm", 2, 1, description:"The Mudworm digs through the Earth day in day out.", bonesCost:5, abilities:abilities, tex:tex);
		}

		private void Addmummylord()
		{
			List<CardMetaCategory> metaCategories = new List<CardMetaCategory>();
			metaCategories.Add(CardMetaCategory.GBCPlayable);

			byte[] imgBytes = System.IO.File.ReadAllBytes(Path.Combine(this.Info.Location.Replace("GrimoraMod.dll",""),"Artwork/Mummy.png"));
			Texture2D tex = new Texture2D(2,2);
			tex.LoadImage(imgBytes);
			NewCard.Add("mummylord", metaCategories, CardComplexity.Advanced, CardTemple.Nature, "Mummy Lord", 3, 3, description:"The Cycle of the Mummy Lord, never ending.", bonesCost:2, tex:tex);
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
			NewCard.Add("Necromancer", metaCategories, CardComplexity.Vanilla, CardTemple.Nature, "Necromancer", 2, 1, description:"The vicious Necromancer, nothing dies once.", bonesCost:3, abilities:abilities, tex:tex);
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
			NewCard.Add("obol", metaCategories, CardComplexity.Simple, CardTemple.Nature, "Ancient Obol", 0, 6, description:"The Ancient Obol, the Bone Lord likes this one.", energyCost:3, abilities:abilities, tex:tex);
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
			NewCard.Add("Pets", metaCategories, CardComplexity.Vanilla, CardTemple.Nature, "Pharaohs Pets", 2, 1, description:"The Undying Pets of the Pharao, enchanting.", bonesCost:5, appearanceBehaviour:appearanceBehaviour, abilities:abilities, tex:tex);
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
			NewCard.Add("Pirateship", metaCategories, CardComplexity.Simple, CardTemple.Nature, "Ghost Ship", 0, 1, description:"The Skeleton Army never rests.", bonesCost:4, abilities:abilities, tex:tex);
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
			NewCard.Add("Revenant", metaCategories, CardComplexity.Intermediate, CardTemple.Nature, "Revenant", 3, 1, description:"The Revenant, bringing the Scythe of death.", bonesCost:3, abilities:abilities, tex:tex);
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
			NewCard.Add("sarcophagus", metaCategories, CardComplexity.Advanced, CardTemple.Nature, "Sarcophagus", 0, 2, description:"The Cycle of the Mummy Lord, never ending.", bonesCost:2, abilities:abilities, tex:tex, evolveId:new EvolveIdentifier("mummylord",1));
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
			NewCard.Add("Serpent", metaCategories, CardComplexity.Vanilla, CardTemple.Nature, "Bone Serpent", 1, 1, description:"Serpent of Bones, its poison can melt even Bones.", bonesCost:4, abilities:abilities, tex:tex);
		}

		private void AddSkelemancer()
		{
			List<CardMetaCategory> metaCategories = new List<CardMetaCategory>();
			metaCategories.Add(CardMetaCategory.ChoiceNode);
			metaCategories.Add(CardMetaCategory.TraderOffer);

			byte[] imgBytes = System.IO.File.ReadAllBytes(Path.Combine(this.Info.Location.Replace("GrimoraMod.dll",""),"Artwork/SkeletonJuniorSage.png"));
			Texture2D tex = new Texture2D(2,2);
			tex.LoadImage(imgBytes);
			NewCard.Add("Skelemancer", metaCategories, CardComplexity.Simple, CardTemple.Nature, "Skelemancer", 1, 1, description:"The humble Skelemancer, he likes a good fight.", energyCost:2, tex:tex);
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
			NewCard.Add("SkeletonMage", metaCategories, CardComplexity.Simple, CardTemple.Nature, "Skelemagus", 1, 4, description:"The Skelemagus, they have learned the Ancient Spell of Death.", energyCost:4, abilities:abilities, tex:tex);
		}

		private void AddSnapper()
		{
			List<CardMetaCategory> metaCategories = new List<CardMetaCategory>();
			metaCategories.Add(CardMetaCategory.ChoiceNode);
			metaCategories.Add(CardMetaCategory.TraderOffer);

			byte[] imgBytes = System.IO.File.ReadAllBytes(Path.Combine(this.Info.Location.Replace("GrimoraMod.dll",""),"Artwork/Snapper.png"));
			Texture2D tex = new Texture2D(2,2);
			tex.LoadImage(imgBytes);
			NewCard.Add("Snapper", metaCategories, CardComplexity.Vanilla, CardTemple.Nature, "Bone Snapper", 1, 6, description:"Snap snap your Bones are gone.", bonesCost:5, tex:tex);
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
			NewCard.Add("SporeDigger", metaCategories, CardComplexity.Simple, CardTemple.Nature, "Sporedigger", 0, 3, description:"The Spore Sigger, an excellent digger.", bonesCost:1, appearanceBehaviour:appearanceBehaviour, abilities:abilities, tex:tex);
		}

		private void AddWolf()
		{
			List<CardMetaCategory> metaCategories = new List<CardMetaCategory>();
			metaCategories.Add(CardMetaCategory.ChoiceNode);
			metaCategories.Add(CardMetaCategory.TraderOffer);

			byte[] imgBytes = System.IO.File.ReadAllBytes(Path.Combine(this.Info.Location.Replace("GrimoraMod.dll",""),"Artwork/Wolf.png"));
			Texture2D tex = new Texture2D(2,2);
			tex.LoadImage(imgBytes);
			NewCard.Add("Wolf", metaCategories, CardComplexity.Vanilla, CardTemple.Nature, "Undead Wolf", 3, 2, description:"A diseased Wolf, but the Pack stays Strong.", cost:0, bonesCost:7, tex:tex);
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
			NewCard.Add("ZombGeck", metaCategories, CardComplexity.Vanilla, CardTemple.Nature, "Zomb-Geck", 1, 1, description:"Da Geeckkkkk is deaddd nooooooooo.", abilities:abilities, tex:tex);
		}

		private void AddZombie()
		{
			List<CardMetaCategory> metaCategories = new List<CardMetaCategory>();
			metaCategories.Add(CardMetaCategory.ChoiceNode);
			metaCategories.Add(CardMetaCategory.TraderOffer);

			byte[] imgBytes = System.IO.File.ReadAllBytes(Path.Combine(this.Info.Location.Replace("GrimoraMod.dll",""),"Artwork/Zombie.png"));
			Texture2D tex = new Texture2D(2,2);
			tex.LoadImage(imgBytes);
			NewCard.Add("Zombie", metaCategories, CardComplexity.Simple, CardTemple.Nature, "Zombie", 1, 1, description:"The humble Zombie, a respected member of the Army.", bonesCost:2, tex:tex);
		}

		private void RemoveAll()
		{
			List<string> cards = new List<string> { "Adder", "Alpha", "Amalgam", "Ant", "AntQueen", "Bee", "Beaver", "Beehive", "Bloodhound", "Bullfrog", "Cat", "Cockroach", "Daus", "Elk", "ElkCub", "FieldMouse", "Geck", "Goat", "Grizzly", "JerseyDevil",
			"Kingfisher", "Magpie", "Mantis", "MantisGod", "Mole", "MoleMan", "Moose", "Mothman_Stage1", "Opossum", "Otter", "Ouroboros", "PackRat", "Porcupine", "Pronghorn", "RatKing", "Raven", "RavenEgg", "Shark", "Skink", "Skunk", "Snapper",
			"Sparrow", "SquidCards", "SquidBell", "SquidMirror", "Urayuli", "Warren", "Wolf", "WolfCub", "PeltHare", "PeltWolf", "PeltGolden", "RingWorm", "Stinkbug_Talking", "Stoat_Talking", "Wolf_Talking"};
			
			for (int i = 0; i < cards.Count; i++)
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
