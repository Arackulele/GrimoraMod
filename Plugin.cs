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
using BepInEx.Configuration;
using GBC;
using I2.TextAnimation;
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
using Random = UnityEngine.Random;


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

		public Vector3 spawnpoint = new Vector3(-6.5f, 6, 0);
		public Quaternion angle = new Quaternion(0F, 0F, 0F, 0F);


		public static bool mousepressed2 = false;
		public int i = 0;


		public int b = 0;



		private void Awake()
			{
				Logger.LogInfo($"Loaded {PluginName}!");

				Harmony harmony = new Harmony(PluginGuid);
				Plugin.Log = base.Logger;
				harmony.PatchAll();


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

				RemoveAll();
				ChangeSquirrel();
				ChangeGoat();
				ChangePackRat();

			}


			[HarmonyPatch(typeof(TextDisplayer), "Start")]
			public class BoardManager3D_Initialize
			{
				public static void Prefix()
				{
					if (!GameObject.Find("TextDisplayer_Grimora(Clone)"))
					{
						Instantiate(Resources.Load("prefabs/ui/TextDisplayer_Grimora"));
						GameObject.Find("TextDisplayer").GetComponent<TextDisplayer>().voiceSoundIdPrefix = GameObject.Find("TextDisplayer_Grimora(Clone)").GetComponent<TextDisplayer>().voiceSoundIdPrefix;
						GameObject.Find("TextDisplayer").GetComponent<TextDisplayer>().continuePressed = GameObject.Find("TextDisplayer_Grimora(Clone)").GetComponent<TextDisplayer>().continuePressed;
						GameObject.Find("TextDisplayer").GetComponent<TextDisplayer>().defaultStyle = GameObject.Find("TextDisplayer_Grimora(Clone)").GetComponent<TextDisplayer>().defaultStyle;
						GameObject.Find("TextDisplayer").GetComponent<TextDisplayer>().textAnimation = GameObject.Find("TextDisplayer_Grimora(Clone)").GetComponent<TextDisplayer>().textAnimation;
						GameObject.Find("TextDisplayer").GetComponent<TextDisplayer>().textMesh = GameObject.Find("TextDisplayer_Grimora(Clone)").GetComponent<TextDisplayer>().textMesh;
						GameObject.Find("TextDisplayer").GetComponent<TextDisplayer>().textShadow = GameObject.Find("TextDisplayer_Grimora(Clone)").GetComponent<TextDisplayer>().textShadow;
						GameObject.Find("TextDisplayer").GetComponent<TextDisplayer>().triangleImage = GameObject.Find("TextDisplayer_Grimora(Clone)").GetComponent<TextDisplayer>().triangleImage;
						GameObject.Find("TextDisplayer").GetComponent<TextDisplayer>().alternateSpeakerStyles = GameObject.Find("TextDisplayer_Grimora(Clone)").GetComponent<TextDisplayer>().alternateSpeakerStyles;
						AnimatingSprite spritet = GameObject.Find("LeshyHeadAnim").GetComponentInChildren<AnimatingSprite>();
						for (int z = 0; z < spritet.textureFrames.Count; z++)
						{
							spritet.textureFrames[z] = (Resources.Load("art\\assets3d\\characters\\grimora\\Eyes") as Texture2D);
						}
					}

				}
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
				NewCard.Add("ara_Necromancer", "Necromancer", 2, 1, metaCategories, CardComplexity.Vanilla, CardTemple.Nature, description: "The vicious Necromancer, nothing dies once.", bonesCost: 3, abilities: abilities, defaultTex: tex);
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
				NewCard.Add("ara_Pets", "Pharaohs Pets", 3, 1, metaCategories, CardComplexity.Vanilla, CardTemple.Nature, description: "The Undying Pets of the Pharao, enchanting.", bonesCost: 5, appearanceBehaviour: appearanceBehaviour, abilities: abilities, defaultTex: tex);
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


				NewCard.Add("SkeletonMage", "Skelemagus", 1, 4, metaCategories, CardComplexity.Simple, CardTemple.Nature, description: "The Skelemagus, they have learned the Ancient Spell of Death.", energyCost: 4, abilities: abilities, defaultTex: tex, decals: decals);
			}

			private void AddAra_Snapper()
			{
				List<CardMetaCategory> metaCategories = new List<CardMetaCategory>();
				metaCategories.Add(CardMetaCategory.ChoiceNode);
				metaCategories.Add(CardMetaCategory.TraderOffer);

				byte[] imgBytes = System.IO.File.ReadAllBytes(Path.Combine(this.Info.Location.Replace("GrimoraMod.dll", ""), "Artwork/Snapper.png"));
				Texture2D tex = new Texture2D(2, 2);
				tex.LoadImage(imgBytes);
				NewCard.Add("Snapper", "Bone Snapper", 1, 6, metaCategories, CardComplexity.Vanilla, CardTemple.Nature, description: "Snap snap your Bones are gone.", bonesCost: 5, defaultTex: tex);
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
				NewCard.Add("SporeDigger", "Sporedigger", 0, 3, metaCategories, CardComplexity.Simple, CardTemple.Nature, description: "The Spore Sigger, an excellent digger.", bonesCost: 1, appearanceBehaviour: appearanceBehaviour, abilities: abilities, defaultTex: tex);
			}

			private void AddAra_Wolf()
			{
				List<CardMetaCategory> metaCategories = new List<CardMetaCategory>();
				metaCategories.Add(CardMetaCategory.ChoiceNode);
				metaCategories.Add(CardMetaCategory.TraderOffer);

				byte[] imgBytes = System.IO.File.ReadAllBytes(Path.Combine(this.Info.Location.Replace("GrimoraMod.dll", ""), "Artwork/Wolf.png"));
				Texture2D tex = new Texture2D(2, 2);
				tex.LoadImage(imgBytes);
				NewCard.Add("Wolf", "Undead Wolf", 3, 2, metaCategories, CardComplexity.Vanilla, CardTemple.Nature, description: "A diseased Wolf, but the Pack stays Strong.", bonesCost: 7, defaultTex: tex);
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
				NewCard.Add("ZombGeck", "Zomb-Geck", 2, 1, metaCategories, CardComplexity.Vanilla, CardTemple.Nature, description: "Da Geeckkkkk is deaddd nooooooooo.", abilities: abilities, defaultTex: tex, appearanceBehaviour: appearanceBehaviour);
			}

			private void AddAra_Zombie()
			{
				List<CardMetaCategory> metaCategories = new List<CardMetaCategory>();
				metaCategories.Add(CardMetaCategory.ChoiceNode);
				metaCategories.Add(CardMetaCategory.TraderOffer);

				byte[] imgBytes = System.IO.File.ReadAllBytes(Path.Combine(this.Info.Location.Replace("GrimoraMod.dll", ""), "Artwork/Zombie.png"));
				Texture2D tex = new Texture2D(2, 2);
				tex.LoadImage(imgBytes);
				NewCard.Add("Zombie", "Zombie", 1, 1, metaCategories, CardComplexity.Simple, CardTemple.Nature, description: "The humble Zombie, a respected member of the Army.", bonesCost: 2, defaultTex: tex);
			}

			private void RemoveAll()
			{
				List<string> cards = new List<string> {"placeholder", "Adder", "Alpha", "Amalgam", "Ant", "AntQueen", "Bee", "Beaver", "Beehive", "Bloodhound", "Bullfrog", "Cat", "Cockroach", "Daus", "Elk", "ElkCub", "FieldMouse", "Geck", "Grizzly", "JerseyDevil",
			"Kingfisher", "Magpie", "Mantis", "MantisGod", "Mole", "MoleMan", "Moose", "Mothman_Stage1", "Opossum", "Otter", "Ouroboros", "Porcupine", "Pronghorn", "RatKing", "Raven", "RavenEgg", "Shark", "Skink", "Skunk", "Snapper",
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













			///complicated stuff ahead, most if not all of this is made by Kopie




			private void Update()
			{
				try
				{
					b++;
					if (b != 0)
					{
						foreach (var light in GameObject.FindObjectsOfType<Light>())
						{
							light.color = new Color(0.1702f, 0.8019f, 0.644f, 1);
						}

						b = 0;
					}
				}
				catch (NullReferenceException)
				{
				}

			}





			[HarmonyPatch(typeof(SpecialNodeHandler), "StartSpecialNodeSequence")]
			public class MapGenerator_patch2
			{
				public static bool Prefix(ref SpecialNodeHandler __instance, SpecialNodeData nodeData)
				{
					if (nodeData is CustomNode1)
					{
						if (__instance.gameObject.GetComponent<MapGenerator_patch4.MapGenerator_patch3.sacrifice_trade>() == null)
						{
							__instance.gameObject.AddComponent<MapGenerator_patch4.MapGenerator_patch3.sacrifice_trade>();
						}

						__instance.StartCoroutine(
							__instance.gameObject.GetComponent<MapGenerator_patch4.MapGenerator_patch3.sacrifice_trade>()
								.startsacrifice_trade(nodeData as CustomNode1));
						return false; // This prevents the rest of the thing from running.
					}

					return true;
				}

				[HarmonyPatch(typeof(SpecialNodeHandler), "StartSpecialNodeSequence")]
				public class MapGenerator_patch4
				{
					public static bool Prefix(ref SpecialNodeHandler __instance, SpecialNodeData nodeData)
					{
						if (nodeData is CustomNode3)
						{
							if (__instance.gameObject.GetComponent<electricstool>() == null)
							{
								__instance.gameObject.AddComponent<electricstool>();
							}

							__instance.StartCoroutine(
								__instance.gameObject.GetComponent<electricstool>()
									.sequencer(nodeData as CustomNode3));
							return false; // This prevents the rest of the thing from running.
						}

						return true;
					}

					[HarmonyPatch(typeof(SpecialNodeHandler), "StartSpecialNodeSequence")]
					public class MapGenerator_patch3
					{
						public static bool Prefix(ref SpecialNodeHandler __instance, SpecialNodeData nodeData)
						{
							if (nodeData is CustomNode2)
							{
								if (__instance.gameObject.GetComponent<cardshop>() == null)
								{
									__instance.gameObject.AddComponent<cardshop>();
								}

								__instance.StartCoroutine(
									__instance.gameObject.GetComponent<cardshop>()
										.sequencer(nodeData as CustomNode2));
								return false; // This prevents the rest of the thing from running.
							}

							return true;
						}




						[HarmonyPatch(typeof(MapDataReader), "SpawnAndPlaceElement")]
						public class MapGenerator_patch5
						{
							public static void Postfix(ref GameObject __result, MapElementData data)
							{
								if ((data as CustomNode1) != null && (data as CustomNode1).name == "sacrifice_trade")
								{
									AnimatingSprite sprite = __result.GetComponentInChildren<AnimatingSprite>();
									for (int i = 0; i < sprite.textureFrames.Count; i++)
									{
										Texture2D blank = new Texture2D(4, 4);
										blank.LoadImage(File.ReadAllBytes(Path.Combine(Paths.BepInExRootPath,
											"plugins\\abilitiesfolder\\" + "trade_node_1" + ".png")));
										Texture2D resized = new Texture2D(4, 4);
										resized = blank;

										sprite.textureFrames[i] = resized;
									}



									sprite.IterateFrame();
								}
							}
						}


						[HarmonyPatch(typeof(MapDataReader), "SpawnAndPlaceElement")]
						public class MapGenerator_patch6
						{
							public static void Postfix(ref GameObject __result, MapElementData data)
							{
								if ((data as CustomNode3) != null && (data as CustomNode3).name == "electricstool")
								{
									AnimatingSprite sprite = __result.GetComponentInChildren<AnimatingSprite>();
									for (int i = 0; i < sprite.textureFrames.Count; i++)
									{
										Texture2D blank = new Texture2D(4, 4);
										blank.LoadImage(File.ReadAllBytes(Path.Combine(Paths.BepInExRootPath,
											"plugins\\abilitiesfolder\\" + "stool_node_1" + ".png")));
										Texture2D resized = new Texture2D(4, 4);
										resized = blank;

										sprite.textureFrames[i] = resized;
									}



									sprite.IterateFrame();
								}
							}
						}

						[HarmonyPatch(typeof(MapDataReader), "SpawnAndPlaceElement")]
						public class MapGenerator_patch4
						{
							public static void Postfix(ref GameObject __result, MapElementData data)
							{
								if ((data as CustomNode2) != null && (data as CustomNode2).name == "cardshop")
								{
									AnimatingSprite sprite = __result.GetComponentInChildren<AnimatingSprite>();
									for (int i = 0; i < sprite.textureFrames.Count; i++)
									{
										Texture2D blank = new Texture2D(4, 4);
										blank.LoadImage(File.ReadAllBytes(Path.Combine(Paths.BepInExRootPath,
											"plugins\\abilitiesfolder\\" + "trade_shop_1" + ".png")));
										Texture2D resized = new Texture2D(4, 4);
										resized = blank;

										sprite.textureFrames[i] = resized;
									}



									sprite.IterateFrame();
								}
							}
						}


						Texture2D duplicateTexture(Texture2D source)
						{
							RenderTexture renderTex = RenderTexture.GetTemporary(
								source.width,
								source.height,
								0,
								RenderTextureFormat.Default,
								RenderTextureReadWrite.Linear);

							Graphics.Blit(source, renderTex);
							RenderTexture previous = RenderTexture.active;
							RenderTexture.active = renderTex;
							Texture2D readableText = new Texture2D(source.width, source.height);
							readableText.ReadPixels(new Rect(0, 0, renderTex.width, renderTex.height), 0, 0);
							readableText.Apply();
							RenderTexture.active = previous;
							RenderTexture.ReleaseTemporary(renderTex);
							return readableText;
						}

						[HarmonyPatch(typeof(MapGenerator), "CreateNode")]
						public class MapGenerator_patch1
						{
							public static bool Prefix(ref NodeData __result, int x, int y, List<NodeData> nodesInRow,
								List<NodeData> previousNodes, int mapLength)
							{
								NodeData nodeData = null;
								if (y == 0)
								{
									nodeData = new NodeData();
								}
								else if (y % 3 == 0)
								{
									if (y + 1 == mapLength)
									{
										nodeData = new BossBattleNodeData();
										(nodeData as BossBattleNodeData).bossType =
											RunState.CurrentMapRegion.bosses[
												Random.Range(0, RunState.CurrentMapRegion.bosses.Count)];
										(nodeData as BossBattleNodeData).specialBattleId =
											BossBattleSequencer.GetSequencerIdForBoss((nodeData as BossBattleNodeData)
												.bossType);
									}
									else if (nodesInRow.Exists((NodeData n) => n is CardBattleNodeData))
									{
										nodeData = new TotemBattleNodeData();
									}
									else
									{
										nodeData = new CardBattleNodeData();
									}

									(nodeData as CardBattleNodeData).difficulty =
										SaveManager.SaveFile.currentRun.regionTier * 6 + (y + 1) / 3 - 1;
								}
								else if (y % 3 == 1)
								{
									float num = (RunState.CurrentRegionTier > 1) ? 0.66f : 0.33f;
									if (Random.value < num)
									{
										nodeData = MapGenerator.ChooseSpecialNodeFromPossibilities(new List<NodeData>
									{
										new BuyPeltsNodeData(),
										new CustomNode2(),
										new TradePeltsNodeData(),
										new DeckTrialNodeData(),
										new BoulderChoiceNodeData()
									}, y, previousNodes, nodesInRow);
									}

									if (nodeData == null)
									{
										nodeData = new CardChoicesNodeData();
										if (nodesInRow.Exists((NodeData n) =>
											n is CardChoicesNodeData &&
											(n as CardChoicesNodeData).choicesType == CardChoicesType.Random))
										{
											List<CardChoicesType> list = new List<CardChoicesType>();
											if (!nodesInRow.Exists((NodeData n) =>
												n is CardChoicesNodeData &&
												(n as CardChoicesNodeData).choicesType == CardChoicesType.Cost))
											{
												list.Add(CardChoicesType.Cost);
											}

											if (!nodesInRow.Exists((NodeData n) =>
												n is CardChoicesNodeData && (n as CardChoicesNodeData).choicesType ==
												CardChoicesType.Tribe) && MapGenerator.TribeBasedChoiceUnlocked)
											{
												list.Add(CardChoicesType.Tribe);
											}

											if (!nodesInRow.Exists((NodeData n) =>
												n is CardChoicesNodeData && (n as CardChoicesNodeData).choicesType ==
												CardChoicesType.Deathcard))
											{
												if (!previousNodes.Exists((NodeData n) =>
														n is CardChoicesNodeData &&
														(n as CardChoicesNodeData).choicesType ==
														CardChoicesType.Deathcard) &&
													SaveManager.SaveFile.GetChoosableDeathcardMods().Count >= 9)
												{
													list.Add(CardChoicesType.Deathcard);
												}
											}

											if (list.Count > 0)
											{
												(nodeData as CardChoicesNodeData).choicesType =
													list[Random.Range(0, list.Count)];
											}
										}
									}
								}
								else
								{
									nodeData = MapGenerator.ChooseSpecialNodeFromPossibilities(new List<NodeData>
								{
									//new CardMergeNodeData(),
									new CustomNode3(),
									new GainConsumablesNodeData(),
									new CustomNode1(),

									new BuildTotemNodeData(),
									new DuplicateMergeNodeData(),
									new CardRemoveNodeData(),
									new CardStatBoostNodeData()
								}, y, previousNodes, nodesInRow);
								}

								if (nodeData == null)
								{
									nodeData = new CardChoicesNodeData();
								}

								nodeData.gridX = x;
								nodeData.gridY = y;
								nodeData.id = MapGenerator.GetNewID();
								__result = nodeData;

								return false;
							}
						}








						public class sacrifice_trade : CardChoicesSequencer
						{
							private List<SelectableCard> cardchoices;

							public int i = 0;

							public int cd = 0;

							public int selectingoptions = 3;

							public List<SelectableCard> cardpicked = new List<SelectableCard>();

							public List<SelectableCard> cardpickedfromdeck = new List<SelectableCard>();

							public IEnumerator startsacrifice_trade(CustomNode1 tradeCardsData)
							{

								while (selectingoptions != 0)
								{
									selectingoptions--;
									Singleton<ViewManager>.Instance.SwitchToView(View.ChoicesCloser, false, true);
									yield return new WaitForSeconds(Time.deltaTime);
									LeshyAnimationController.Instance.PutOnMask(LeshyAnimationController.Mask.Doctor,
										false);
									yield return new WaitForSeconds(Time.deltaTime);
									GameObject gameObject =
										Instantiate<GameObject>(
											Singleton<SelectableCardArray>.Instance.selectableCardPrefab);
									yield return new WaitForSeconds(Time.deltaTime);
									gameObject.transform.SetParent(base.transform);
									yield return new WaitForSeconds(Time.deltaTime);
									SelectableCard component = gameObject.GetComponent<SelectableCard>();
									yield return new WaitForSeconds(Time.deltaTime);
									component.Initialize(CardLoader.GetRandomChoosableCard(Environment.TickCount),
										new Action<SelectableCard>(Singleton<SelectableCardArray>.Instance.OnCardSelected),
										null,
										false,
										new Action<SelectableCard>(Singleton<SelectableCardArray>.Instance
											.OnCardInspected));
									yield return new WaitForSeconds(Time.deltaTime);
									component.SetEnabled(false);
									yield return new WaitForSeconds(Time.deltaTime);
									Singleton<SelectableCardArray>.Instance.displayedCards.Add(component);
									yield return new WaitForSeconds(Time.deltaTime);
									Singleton<SelectableCardArray>.Instance.TweenInCard(component.transform,
										new Vector3(-1.5f, 5.01f, 0f), 0, false);
									yield return new WaitForSeconds(Time.deltaTime);
									component.Anim.PlayQuickRiffleSound();
									yield return new WaitForSeconds(Time.deltaTime);
									component.Initialize(CardLoader.GetRandomChoosableCard(Environment.TickCount),
										new Action<SelectableCard>(sacrificetradeaction),
										new Action<SelectableCard>(sacrificetradeactionflip), true, null);
									yield return new WaitForSeconds(Time.deltaTime);
									component.GetComponent<Collider>().enabled = true;
									yield return new WaitForSeconds(Time.deltaTime * 17);
									component.transform.position = new Vector3(component.transform.position.x + (1.5f * i),
										component.transform.position.y, -2.6f);
									cardpicked.Add(component);
									yield return new WaitForSeconds(Time.deltaTime);
									i++;
								}

								//yield break;
							}



							public void sacrificetradeactionflip(SelectableCard component)
							{
								if (component != null)
								{
									//Singleton<CardPile>.Instance.AddToPile(component.transform);
									component.Flipped = false;
								}
							}

							public IEnumerator tradesequencer()
							{
								yield return new WaitForSeconds(0.5f);
								foreach (var cardindeck in SaveManager.saveFile.currentRun.playerDeck.Cards)
								{

									yield return new WaitForSeconds(Time.deltaTime);
									yield return new WaitForSeconds(Time.deltaTime);
									GameObject gameObject =
										Instantiate<GameObject>(
											Singleton<SelectableCardArray>.Instance.selectableCardPrefab);
									yield return new WaitForSeconds(Time.deltaTime);
									gameObject.transform.SetParent(base.transform);
									yield return new WaitForSeconds(Time.deltaTime);
									SelectableCard component = gameObject.GetComponent<SelectableCard>();
									yield return new WaitForSeconds(Time.deltaTime);
									component.Initialize(cardindeck,
										new Action<SelectableCard>(Singleton<SelectableCardArray>.Instance.OnCardSelected),
										null,
										false,
										new Action<SelectableCard>(Singleton<SelectableCardArray>.Instance
											.OnCardInspected));
									yield return new WaitForSeconds(Time.deltaTime);
									component.SetEnabled(false);
									yield return new WaitForSeconds(Time.deltaTime);
									Singleton<SelectableCardArray>.Instance.displayedCards.Add(component);
									yield return new WaitForSeconds(Time.deltaTime * 2);
									Singleton<SelectableCardArray>.Instance.TweenInCard(component.transform,
										new Vector3(-1.5f, 5.01f, 0f), 0, false);
									yield return new WaitForSeconds(Time.deltaTime);
									component.Anim.PlayQuickRiffleSound();
									yield return new WaitForSeconds(Time.deltaTime);
									component.Initialize(cardindeck,
										new Action<SelectableCard>(removeselectablecardfromdeck),
										null, false, null);
									yield return new WaitForSeconds(Time.deltaTime);
									component.GetComponent<Collider>().enabled = true;
									yield return new WaitForSeconds(Time.deltaTime * 19);
									component.transform.position = new Vector3(
										component.transform.position.x + (1.2f * cd) - 1f,
										component.transform.position.y, -2.6f);
									cardpickedfromdeck.Add(component);
									yield return new WaitForSeconds(Time.deltaTime);
									cd++;
								}

							}

							public void removeselectablecardfromdeck(SelectableCard component)
							{
								component.Anim.PlayDeathAnimation();
								StartCoroutine(removeselectablecardfromdeckie(component));

							}


							public IEnumerator removeselectablecardfromdeckie(SelectableCard component)
							{
								SaveManager.saveFile.currentRun.playerDeck.RemoveCard(component.Info);
								SaveManager.SaveToFile();
								component.ExitBoard(0.3f, new Vector3(-1f, -1f, 6f));
								yield return new WaitForSeconds(1.0f);
								Singleton<ViewManager>.Instance.SwitchToView(View.Default, false, true);
								StartCoroutine(LeshyAnimationController.Instance.TakeOffMask());
								if (Singleton<GameFlowManager>.Instance != null)
								{
									Singleton<GameFlowManager>.Instance.TransitionToGameState(GameState.Map, null);
								}

								foreach (var VARIABLE in cardpickedfromdeck)
								{
									VARIABLE.ExitBoard(0.3f, new Vector3(-1f, -1f, 6f));
								}
							}


							public void sacrificetradeaction(SelectableCard component)
							{
								if (component != null)
								{
									if (component.Flipped == false)
									{
										//Singleton<CardPile>.Instance.AddToPile(component.transform);
										SaveManager.SaveFile.CurrentDeck.AddCard(component.Info);
										component.Anim.PlayDeathAnimation();
										Singleton<ViewManager>.Instance.SwitchToView(View.Default, false, true);
										//StartCoroutine(LeshyAnimationController.Instance.TakeOffMask());
										//if (Singleton<GameFlowManager>.Instance != null)
										//{
										//	Singleton<GameFlowManager>.Instance.TransitionToGameState(GameState.Map, null);
										//}
										foreach (var VARIABLE in cardpicked)
										{
											VARIABLE.ExitBoard(0.3f, new Vector3(-1f, -1f, 6f));
										}

										StartCoroutine(tradesequencer());







									}

								}
							}
						}

						public class cardshop : CardChoicesSequencer
						{

							public List<SelectableCard> created = new List<SelectableCard>();

							public IEnumerator sequencer(CustomNode2 tradeCardsData)
							{
								{
									{
										GameObject.FindObjectOfType<CurrencyBowl>()
											.MoveIntoPlace(new Vector3(2.5f, 5.19f, 0.72f), Vector3.zero,
												Tween.EaseInOutStrong);
										GameObject.FindObjectOfType<CurrencyBowl>().Show();

										var dinfo = CardLoader.GetCardByName("Squirrel");
										dinfo.displayedName = "The Small Offering";
										Texture2D blank = new Texture2D(4, 4);
										blank.LoadImage(File.ReadAllBytes(Path.Combine(Paths.BepInExRootPath,
											"plugins\\abilitiesfolder\\" + "trade_shop_card" + ".png")));
										Texture2D resized = new Texture2D(4, 4);
										resized = blank;
										dinfo.hideAttackAndHealth = true;
										dinfo.portraitTex = Sprite.Create(resized, new Rect(0.0f, 0.0f, resized.width, resized.height), new Vector2(0.5f, 0.6f), 100.0f);
										Singleton<ViewManager>.Instance.SwitchToView(View.BoardCentered, false, true);
										yield return new WaitForSeconds(0.5f);
										yield return new WaitForSeconds(Time.deltaTime);
										LeshyAnimationController.Instance.PutOnMask(LeshyAnimationController.Mask.Trapper,
											false);
										yield return new WaitForSeconds(Time.deltaTime);
										GameObject gameObject =
											Instantiate<GameObject>(Singleton<SelectableCardArray>.Instance
												.selectableCardPrefab);
										yield return new WaitForSeconds(Time.deltaTime);
										gameObject.transform.SetParent(base.transform);
										yield return new WaitForSeconds(Time.deltaTime);
										SelectableCard component = gameObject.GetComponent<SelectableCard>();
										yield return new WaitForSeconds(Time.deltaTime);
										component.Initialize(dinfo,
											new Action<SelectableCard>(Singleton<SelectableCardArray>.Instance
												.OnCardSelected),
											null,
											false,
											new Action<SelectableCard>(Singleton<SelectableCardArray>.Instance
												.OnCardInspected));
										yield return new WaitForSeconds(Time.deltaTime);
										component.SetEnabled(false);
										yield return new WaitForSeconds(Time.deltaTime);
										Singleton<SelectableCardArray>.Instance.displayedCards.Add(component);
										yield return new WaitForSeconds(Time.deltaTime);
										Singleton<SelectableCardArray>.Instance.TweenInCard(component.transform,
											new Vector3(-1.5f - 1, 5.01f, -0.32f - 0.3f), 0, false);
										yield return new WaitForSeconds(Time.deltaTime);
										component.Anim.PlayQuickRiffleSound();
										yield return new WaitForSeconds(Time.deltaTime);
										component.Initialize(dinfo,
											new Action<SelectableCard>(action),
											null, false, null);
										yield return new WaitForSeconds(Time.deltaTime);
										component.GetComponent<Collider>().enabled = true;
										created.Add(component);
										AddPricetagToCardd(component, 4, 0.25f);
										yield return new WaitForSeconds(Time.deltaTime);
									}
									{
										var dinfo = CardLoader.GetCardByName("Squirrel");
										dinfo.displayedName = "The Big Offering";
										Texture2D blank = new Texture2D(4, 4);
										blank.LoadImage(File.ReadAllBytes(Path.Combine(Paths.BepInExRootPath,
											"plugins\\abilitiesfolder\\" + "trade_shop_card" + ".png")));
										Texture2D resized = new Texture2D(4, 4);
										resized = blank;
										dinfo.hideAttackAndHealth = true;
										dinfo.portraitTex = Sprite.Create(resized, new Rect(0.0f, 0.0f, resized.width, resized.height), new Vector2(0.5f, 0.6f), 100.0f);
										Singleton<ViewManager>.Instance.SwitchToView(View.BoardCentered, false, true);
										yield return new WaitForSeconds(0.5f);
										yield return new WaitForSeconds(Time.deltaTime);
										LeshyAnimationController.Instance.PutOnMask(LeshyAnimationController.Mask.Trapper,
											false);
										yield return new WaitForSeconds(Time.deltaTime);
										GameObject gameObject =
											Instantiate<GameObject>(Singleton<SelectableCardArray>.Instance
												.selectableCardPrefab);
										yield return new WaitForSeconds(Time.deltaTime);
										gameObject.transform.SetParent(base.transform);
										yield return new WaitForSeconds(Time.deltaTime);
										SelectableCard component = gameObject.GetComponent<SelectableCard>();
										yield return new WaitForSeconds(Time.deltaTime);
										component.Initialize(dinfo,
											new Action<SelectableCard>(Singleton<SelectableCardArray>.Instance
												.OnCardSelected),
											null,
											false,
											new Action<SelectableCard>(Singleton<SelectableCardArray>.Instance
												.OnCardInspected));
										yield return new WaitForSeconds(Time.deltaTime);
										component.SetEnabled(false);
										yield return new WaitForSeconds(Time.deltaTime);
										Singleton<SelectableCardArray>.Instance.displayedCards.Add(component);
										yield return new WaitForSeconds(Time.deltaTime);
										Singleton<SelectableCardArray>.Instance.TweenInCard(component.transform,
											new Vector3(1.0f - 1, 5.01f, -0.32f - 0.3f), 0, false);
										yield return new WaitForSeconds(Time.deltaTime);
										component.Anim.PlayQuickRiffleSound();
										yield return new WaitForSeconds(Time.deltaTime);
										component.Initialize(dinfo,
											new Action<SelectableCard>(action),
											null, false, null);
										yield return new WaitForSeconds(Time.deltaTime);
										component.GetComponent<Collider>().enabled = true;
										created.Add(component);
										AddPricetagToCardd(component, 12, 0.25f);
										yield return new WaitForSeconds(Time.deltaTime);

									}
									{
										var dinfo = CardLoader.GetCardByName("Squirrel");
										dinfo.displayedName = "Exit";
										Singleton<ViewManager>.Instance.SwitchToView(View.BoardCentered, false, true);
										yield return new WaitForSeconds(0.5f);
										yield return new WaitForSeconds(Time.deltaTime);
										LeshyAnimationController.Instance.PutOnMask(LeshyAnimationController.Mask.Trapper,
											false);
										yield return new WaitForSeconds(Time.deltaTime);
										GameObject gameObject =
											Instantiate<GameObject>(Singleton<SelectableCardArray>.Instance
												.selectableCardPrefab);
										yield return new WaitForSeconds(Time.deltaTime);
										gameObject.transform.SetParent(base.transform);
										yield return new WaitForSeconds(Time.deltaTime);
										SelectableCard component = gameObject.GetComponent<SelectableCard>();
										yield return new WaitForSeconds(Time.deltaTime);
										component.Initialize(dinfo,
											new Action<SelectableCard>(Singleton<SelectableCardArray>.Instance
												.OnCardSelected),
											null,
											false,
											new Action<SelectableCard>(Singleton<SelectableCardArray>.Instance
												.OnCardInspected));
										yield return new WaitForSeconds(Time.deltaTime);
										component.SetEnabled(false);
										yield return new WaitForSeconds(Time.deltaTime);
										Singleton<SelectableCardArray>.Instance.displayedCards.Add(component);
										yield return new WaitForSeconds(Time.deltaTime);
										Singleton<SelectableCardArray>.Instance.TweenInCard(component.transform,
											new Vector3(3.5f - 1, 5.01f, -0.32f - 0.3f), 0, false);
										yield return new WaitForSeconds(Time.deltaTime);
										component.Anim.PlayQuickRiffleSound();
										yield return new WaitForSeconds(Time.deltaTime);
										component.Initialize(dinfo,
											new Action<SelectableCard>(action),
											null, false, null);
										yield return new WaitForSeconds(Time.deltaTime);
										component.GetComponent<Collider>().enabled = true;
										created.Add(component);
										yield return new WaitForSeconds(Time.deltaTime);

									}
								}


							}


							public void AddPricetagToCardd(SelectableCard card, int priceIndex, float tweenDelay)
							{
								GameObject gameObject =
									Instantiate<GameObject>(Singleton<BuyPeltsSequencer>.Instance.pricetagPrefab);
								gameObject.transform.SetParent(card.transform);
								gameObject.transform.localPosition =
									new Vector3(-0.4f + Random.value * 0.1f, 0.95f, -0.03f);
								gameObject.transform.localEulerAngles = new Vector3(-90f, -90f, 90f);
								gameObject.name = "pricetag";
								gameObject.GetComponentInChildren<Renderer>().material.mainTexture =
									Singleton<BuyPeltsSequencer>.Instance.pricetagTextures[priceIndex];
								Tween.LocalRotation(gameObject.transform,
									new Vector3(-80f + Random.value * -20f, -90f, 90f), 0.25f, tweenDelay, Tween.EaseOut,
									Tween.LoopType.None, null, null, true);
							}

							public void action(SelectableCard component)
							{

								if (component.Info.displayedName == "The Small Offering")
								{
									Log.LogInfo("testsmall");
									if (RunState.Run.currency >= 4)
									{

										List<Rigidbody> list = GameObject.FindObjectOfType<CurrencyBowl>().TakeWeights(4);

										foreach (Rigidbody rigidbody in list)
										{
											Log.LogInfo(rigidbody);
											float num3 = (float)list.IndexOf(rigidbody) * 0.05f;
											Tween.Position(rigidbody.transform,
												rigidbody.transform.position + Vector3.up * 0.5f, 0.075f, num3,
												Tween.EaseIn, Tween.LoopType.None, null, null, true);
											Tween.Position(rigidbody.transform, new Vector3(0f, 5.5f, 4f), 0.3f,
												0.125f + num3, Tween.EaseOut, Tween.LoopType.None, null, null, true);
											Destroy(rigidbody.gameObject, 0.5f);
										}

										SaveManager.saveFile.currentRun.playerDeck.AddCard(
											CardLoader.AllData.FindAll(info =>
												!info.metaCategories.Contains(CardMetaCategory.Rare) &&
												info.temple == CardTemple.Nature)[
												SeededRandom.Range(0,
													CardLoader.AllData.FindAll(info =>
														!info.metaCategories.Contains(CardMetaCategory.Rare) &&
														info.temple == CardTemple.Nature).Count - 1,
													Environment.TickCount)]);
										RunState.Run.currency -= 4;
										SaveManager.SaveToFile();
									}

								}
								else
								{
									if (component.Info.displayedName == "The Big Offering")
									{
										Log.LogInfo("testbig");


										if (SaveManager.saveFile.currentRun.currency >= 12)
										{
											RunState.Run.currency -= 12;
											List<Rigidbody> list = GameObject.FindObjectOfType<CurrencyBowl>()
												.TakeWeights(12);
											foreach (Rigidbody rigidbody in list)
											{
												float num3 = (float)list.IndexOf(rigidbody) * 0.05f;
												Tween.Position(rigidbody.transform,
													rigidbody.transform.position + Vector3.up * 0.5f, 0.075f, num3,
													Tween.EaseIn, Tween.LoopType.None, null, null, true);
												Tween.Position(rigidbody.transform, new Vector3(0f, 5.5f, 4f), 0.3f,
													0.125f + num3, Tween.EaseOut, Tween.LoopType.None, null, null, true);
												Destroy(rigidbody.gameObject, 0.5f);
											}

											SaveManager.saveFile.currentRun.playerDeck.AddCard(
												CardLoader.AllData.FindAll(info =>
													info.metaCategories.Contains(CardMetaCategory.Rare) &&
													info.temple == CardTemple.Nature)[
													SeededRandom.Range(0,
														CardLoader.AllData.FindAll(info =>
															info.metaCategories.Contains(CardMetaCategory.Rare) &&
															info.temple == CardTemple.Nature).Count - 1,
														Environment.TickCount)]);
											SaveManager.SaveToFile();
										}
									}
									else
									{
										if (component.Info.displayedName == "Exit")
										{
											StartCoroutine(TextDisplayer.Instance.ShowUntilInput("Leaving so soon!"));

											if (Singleton<GameFlowManager>.Instance != null)
											{
												Singleton<GameFlowManager>.Instance.TransitionToGameState(GameState.Map,
													null);
												foreach (var VARIABLE in created)
												{
													VARIABLE.Anim.PlayDeathAnimation();
													VARIABLE.ExitBoard(0.3f, new Vector3(-1f, -1f, 6f));
												}

												GameObject.FindObjectOfType<CurrencyBowl>().MoveAway(new Vector3(0, 0, 0));
												GameObject.FindObjectOfType<CurrencyBowl>().Hide();
												StartCoroutine(LeshyAnimationController.Instance.TakeOffMask());
											}
										}
									}
								}
							}
						}
					}
				}





				public class electricstool : CardChoicesSequencer
				{


					public int i = 0;

					public int cd = 0;

					public int selectingoptions = 3;

					public List<SelectableCard> cardpicked = new List<SelectableCard>();

					public List<SelectableCard> cardpickedfromdeck = new List<SelectableCard>();

					public List<SelectableCard> created = new List<SelectableCard>();

					public ConfirmStoneButton confirmStone { get; set; }


					public void removeselectablecardfromdeck(SelectableCard component)
					{
						//component.Anim.PlayDeathAnimation();
						StartCoroutine(removeselectablecardfromdeckie(component));

					}

					public void cardpickingupscropt(SelectableCard component)
					{
						component.ExitBoard(0.3f, new Vector3(-1f, -1f, 6f));
						Singleton<ViewManager>.Instance.SwitchToView(View.Default, false, true);
						StartCoroutine(LeshyAnimationController.Instance.TakeOffMask());
						confirmStone.Exit();
						if (Singleton<GameFlowManager>.Instance != null)
						{
							Singleton<GameFlowManager>.Instance.TransitionToGameState(GameState.Map, null);
						}
					}

					public IEnumerator removeselectablecardfromdeckie(SelectableCard component)
					{
						var amountdefault = 4;
						//SaveManager.saveFile.currentRun.playerDeck.RemoveCard(component.Info);
						//SaveManager.SaveToFile();
						//component.ExitBoard(0.3f, new Vector3(-1f, -1f, 6f));
						cardpickedfromdeck.Remove(component);

						Tween.Position(component.transform,
							new Vector3(component.transform.position.x, component.transform.position.y,
								component.transform.position.z + 1.2f), 0.15f, 0.1f);
						yield return new WaitForSeconds(0.2f);
						//Singleton<ViewManager>.Instance.SwitchToView(View.Default, false, true);
						//StartCoroutine(LeshyAnimationController.Instance.TakeOffMask());
						//if (Singleton<GameFlowManager>.Instance != null)
						//{
						//	Singleton<GameFlowManager>.Instance.TransitionToGameState(GameState.Map, null);
						//}
						ConfirmStoneButton.Instantiate(Resources.Load("prefabs\\specialnodesequences\\ConfirmStoneButton"))
							.name = "TESTSTONE";
						//GameObject.Find("TESTSTONE").AddComponent<ConfirmStoneButton>();
						GameObject.Find("TESTSTONE").AddComponent<BoxCollider>().size = new Vector3(1.4f, 0.2f, 2.1f);
						confirmStone = GameObject.Find("TESTSTONE").GetComponentInChildren<ConfirmStoneButton>();
						foreach (var VARIABLE in cardpickedfromdeck)
						{
							VARIABLE.ExitBoard(0.3f, new Vector3(-1f, -1f, 6f));
						}

						component.Initialize(component.Info, cardpickingupscropt, null);
						this.confirmStone.Enter();
						yield return this.confirmStone.WaitUntilConfirmation();
						if (amountdefault - component.Info.ModAbilities.Count >= 0)
						{
							{
								this.confirmStone.Unpress();
								component.Anim.PlayTransformAnimation();
								CardInfo cardinfo = CardLoader.AllData.FindAll(info =>
									info.name == "Magpie" || info.name == "RatKing" || info.name == "Elk" ||
									info.name == "Adder" || info.name == "Sparrow" || info.name == "Bloodhound" ||
									info.name == "Otter" || info.name == "Maggots" || info.name == "Skink" ||
									info.name == "Shieldbot" || info.name == "Bullfrog" || info.name == "Mantis" ||
									info.name == "MantisFod" || info.name == "Moose" || info.name == "Porcupine" ||
									info.name == "WolfCub" || info.name == "SentryBot" || info.name == "Necromancer")[
									SeededRandom.Range(0, 17, Environment.TickCount)];
								yield return new WaitForSeconds(0.03f);
								CardModificationInfo cardModificationInfo = new CardModificationInfo(cardinfo);
								cardModificationInfo.fromCardMerge = true;
								RunState.Run.playerDeck.ModifyCard(component.Info, cardModificationInfo);
								component.SetInfo(component.Info);

								if (amountdefault - component.Info.ModAbilities.Count == 0)
								{
									confirmStone.Disable();
								}
							}
						}

						yield return this.confirmStone.WaitUntilConfirmation();
						if (amountdefault - component.Info.ModAbilities.Count >= 0)
						{
							if (SeededRandom.Value(SaveManager.SaveFile.GetCurrentRandomSeed()) > 1f - 1 * 0.225f)
							{
								component.Anim.PlayDeathAnimation();
								component.ExitBoard(0.0f, new Vector3(-1f, -1f, 6f));
								RunState.Run.playerDeck.RemoveCard(component.Info);
								Singleton<ViewManager>.Instance.SwitchToView(View.Default, false, true);
								StartCoroutine(LeshyAnimationController.Instance.TakeOffMask());
								confirmStone.Exit();
								if (Singleton<GameFlowManager>.Instance != null)
								{
									Singleton<GameFlowManager>.Instance.TransitionToGameState(GameState.Map, null);
								}

								StartCoroutine(TextDisplayer.Instance.ShowUntilInput("Haha, Fool! You lost your Card to the Chair."));

							}
							else
							{
								this.confirmStone.Unpress();
								component.Anim.PlayTransformAnimation();
								CardInfo cardinfo = CardLoader.AllData.FindAll(info =>
									info.name == "Magpie" || info.name == "RatKing" || info.name == "Elk" ||
									info.name == "Adder" || info.name == "Sparrow" || info.name == "Bloodhound" ||
									info.name == "Otter" || info.name == "Maggots" || info.name == "Skink" ||
									info.name == "Shieldbot" || info.name == "Bullfrog" || info.name == "Mantis" ||
									info.name == "MantisGod" || info.name == "Moose" || info.name == "Porcupine" ||
									info.name == "WolfCub" || info.name == "SentryBot" || info.name == "Necromancer")[
									SeededRandom.Range(0, 17, Environment.TickCount)];
								yield return new WaitForSeconds(0.03f);
								CardModificationInfo cardModificationInfo = new CardModificationInfo(cardinfo);
								cardModificationInfo.fromCardMerge = true;
								RunState.Run.playerDeck.ModifyCard(component.Info, cardModificationInfo);
								component.SetInfo(component.Info);

								if (amountdefault - component.Info.ModAbilities.Count == 0)
								{
									confirmStone.Disable();
								}
							}
						}
						yield return this.confirmStone.WaitUntilConfirmation();
						if (amountdefault - component.Info.ModAbilities.Count >= 0)
						{
							if (SeededRandom.Value(SaveManager.SaveFile.GetCurrentRandomSeed()) > 1f - 2 * 0.225f)
							{
								component.Anim.PlayDeathAnimation();
								component.ExitBoard(0.0f, new Vector3(-1f, -1f, 6f));
								RunState.Run.playerDeck.RemoveCard(component.Info);
								Singleton<ViewManager>.Instance.SwitchToView(View.Default, false, true);
								StartCoroutine(LeshyAnimationController.Instance.TakeOffMask());
								confirmStone.Exit();
								if (Singleton<GameFlowManager>.Instance != null)
								{
									Singleton<GameFlowManager>.Instance.TransitionToGameState(GameState.Map, null);
								}

								StartCoroutine(TextDisplayer.Instance.ShowUntilInput("Haha, Fool! You lost your Card to the Chair."));

							}
							else
							{
								this.confirmStone.Unpress();
								component.Anim.PlayTransformAnimation();
								CardInfo cardinfo = CardLoader.AllData.FindAll(info =>
									info.name == "Magpie" || info.name == "RatKing" || info.name == "Elk" ||
									info.name == "Adder" || info.name == "Sparrow" || info.name == "Bloodhound" ||
									info.name == "Otter" || info.name == "Maggots" || info.name == "Skink" ||
									info.name == "Shieldbot" || info.name == "Bullfrog" || info.name == "Mantis" ||
									info.name == "MantisGod" || info.name == "Moose" || info.name == "Porcupine" ||
									info.name == "WolfCub" || info.name == "SentryBot" || info.name == "Necromancer")[
									SeededRandom.Range(0, 17, Environment.TickCount)];
								yield return new WaitForSeconds(0.03f);
								CardModificationInfo cardModificationInfo = new CardModificationInfo(cardinfo);
								cardModificationInfo.fromCardMerge = true;
								RunState.Run.playerDeck.ModifyCard(component.Info, cardModificationInfo);
								component.SetInfo(component.Info);

								if (amountdefault - component.Info.ModAbilities.Count == 0)
								{
									confirmStone.Disable();
								}
							}
						}
						yield return this.confirmStone.WaitUntilConfirmation();
						if (amountdefault - component.Info.ModAbilities.Count >= 0)
						{
							if (SeededRandom.Value(SaveManager.SaveFile.GetCurrentRandomSeed()) > 1f - 3 * 0.225f)
							{
								component.Anim.PlayDeathAnimation();
								component.ExitBoard(0.0f, new Vector3(-1f, -1f, 6f));
								RunState.Run.playerDeck.RemoveCard(component.Info);
								Singleton<ViewManager>.Instance.SwitchToView(View.Default, false, true);
								StartCoroutine(LeshyAnimationController.Instance.TakeOffMask());
								confirmStone.Exit();
								if (Singleton<GameFlowManager>.Instance != null)
								{
									Singleton<GameFlowManager>.Instance.TransitionToGameState(GameState.Map, null);
								}

								StartCoroutine(TextDisplayer.Instance.ShowUntilInput("Haha, Fool! You lost your Card to the Chair."));

							}
							else
							{
								this.confirmStone.Unpress();
								component.Anim.PlayTransformAnimation();
								CardInfo cardinfo = CardLoader.AllData.FindAll(info =>
									info.name == "Magpie" || info.name == "RatKing" || info.name == "Elk" ||
									info.name == "Adder" || info.name == "Sparrow" || info.name == "Bloodhound" ||
									info.name == "Otter" || info.name == "Maggots" || info.name == "Skink" ||
									info.name == "Shieldbot" || info.name == "Bullfrog" || info.name == "Mantis" ||
									info.name == "MantisGod" || info.name == "Moose" || info.name == "Porcupine" ||
									info.name == "WolfCub" || info.name == "SentryBot" || info.name == "Necromancer")[
									SeededRandom.Range(0, 17, Environment.TickCount)];
								yield return new WaitForSeconds(0.03f);
								CardModificationInfo cardModificationInfo = new CardModificationInfo(cardinfo);
								cardModificationInfo.fromCardMerge = true;
								RunState.Run.playerDeck.ModifyCard(component.Info, cardModificationInfo);
								component.SetInfo(component.Info);

								if (amountdefault - component.Info.ModAbilities.Count == 0)
								{
									confirmStone.Disable();
								}
							}
						}

					}



					public IEnumerator sequencer(CustomNode3 tradeCardsData)
					{
						foreach (var cardindeck in SaveManager.saveFile.currentRun.playerDeck.Cards)
						{

							yield return new WaitForSeconds(Time.deltaTime);
							yield return new WaitForSeconds(Time.deltaTime);
							GameObject gameObject =
								Instantiate<GameObject>(
									Singleton<SelectableCardArray>.Instance.selectableCardPrefab);
							yield return new WaitForSeconds(Time.deltaTime);
							gameObject.transform.SetParent(base.transform);
							yield return new WaitForSeconds(Time.deltaTime);
							SelectableCard component = gameObject.GetComponent<SelectableCard>();
							yield return new WaitForSeconds(Time.deltaTime);
							component.Initialize(cardindeck,
								new Action<SelectableCard>(Singleton<SelectableCardArray>.Instance.OnCardSelected),
								null,
								false,
								new Action<SelectableCard>(Singleton<SelectableCardArray>.Instance
									.OnCardInspected));
							yield return new WaitForSeconds(Time.deltaTime);
							component.SetEnabled(false);
							yield return new WaitForSeconds(Time.deltaTime);
							Singleton<SelectableCardArray>.Instance.displayedCards.Add(component);
							yield return new WaitForSeconds(Time.deltaTime * 2);
							Singleton<SelectableCardArray>.Instance.TweenInCard(component.transform,
								new Vector3(-1.3f + (1.3f * cd) - 1f - (cd / 6 * 7.8f), 5.31f, -1.6f + (cd / 6) * 2), 0, false);
							yield return new WaitForSeconds(Time.deltaTime);

							component.Anim.PlayQuickRiffleSound();
							yield return new WaitForSeconds(Time.deltaTime);
							component.Initialize(cardindeck,
								new Action<SelectableCard>(removeselectablecardfromdeck),
								null, false, null);
							yield return new WaitForSeconds(Time.deltaTime);
							component.GetComponent<Collider>().enabled = true;
							yield return new WaitForSeconds(Time.deltaTime * 19);
							cardpickedfromdeck.Add(component);
							yield return new WaitForSeconds(Time.deltaTime);
							cd++;
						}
						Log.LogInfo("electricstool");
						yield break;
					}
				}


				public class ZombieVirusItem : ConsumableItem
				{
					// Token: 0x06000F7B RID: 3963 RVA: 0x00036847 File Offset: 0x00034A47
					public override IEnumerator ActivateSequence()
					{
						base.PlayExitAnimation();
						yield return new WaitForSeconds(0.25f);
						Singleton<TurnManager>.Instance.Opponent.SkipNextTurn = true;
						Singleton<TextDisplayer>.Instance.StartCoroutine(Singleton<TextDisplayer>.Instance.ShowUntilInput("I'll test this item.", -0.65f, 0.4f, Emotion.Neutral, TextDisplayer.LetterAnimation.Jitter, DialogueEvent.Speaker.Single, null, true));
						yield break;
					}
				}







				// Token: 0x04000A61 RID: 2657
				[Header("Trade Cards")]
				[SerializeField]
				private Transform selectedCardPosMarker;

				// Token: 0x04000A62 RID: 2658
				[SerializeField] private SelectableCardArray cardArray;

			}
		}





		public class CustomNode1 : SpecialNodeData
		{
			public string name = "sacrifice_trade";

			public CardChoicesType choicesType { get; set; }

			// Token: 0x04000C65 RID: 3173
			public List<CardChoice> overrideChoices { get; set; }

			// Token: 0x04000C66 RID: 3174
			public bool gemifyChoices { get; set; }

			//public CustomNode1 tradeCardsData { get; set; }


			// Token: 0x1700021F RID: 543
			// (get) Token: 0x060011C7 RID: 4551 RVA: 0x0003E26D File Offset: 0x0003C46D
			public override string PrefabPath
			{
				get { return "Prefabs/Map/MapNodesPart1/MapNode_Empty"; }
			}

			// Token: 0x17000220 RID: 544
			// (get) Token: 0x060011C8 RID: 4552 RVA: 0x0003E274 File Offset: 0x0003C474
			//public override List<NodeData.SelectionCondition> GenerationPrerequisiteConditions
			//{
			//	get
			//	{
			//		return new List<NodeData.SelectionCondition>
			//		{
			//			new NodeData.PreviousNodesContent(typeof(CustomNode1), false),
			//			new NodeData.StoryEventCompleted(StoryEvent.ArchivistDefeated),
			//			new NodeData.WithinRegionIndexRange(0, 0),
			//			new NodeData.PastRunsCompleted(3),
			//			new NodeData.WithinGridYRange(2, int.MaxValue)
			//		};
			//	}
			//}
		}

		public class CustomNode2 : SpecialNodeData
		{
			public string name = "cardshop";

			public CardChoicesType choicesType { get; set; }

			// Token: 0x04000C65 RID: 3173
			public List<CardChoice> overrideChoices { get; set; }

			// Token: 0x04000C66 RID: 3174
			public bool gemifyChoices { get; set; }

			//public CustomNode1 tradeCardsData { get; set; }


			// Token: 0x1700021F RID: 543
			// (get) Token: 0x060011C7 RID: 4551 RVA: 0x0003E26D File Offset: 0x0003C46D
			public override string PrefabPath
			{
				get { return "Prefabs/Map/MapNodesPart1/MapNode_Empty"; }
			}

			public override List<NodeData.SelectionCondition> GenerationPrerequisiteConditions
			{
				get
				{
					return new List<NodeData.SelectionCondition>
				{
					new NodeData.PreviousNodesContent(typeof(BuyPeltsNodeData), false),
					new NodeData.PreviousNodesContent(typeof(CardBattleNodeData), true),
				};
				}
			}
		}

		public class CustomNode3 : SpecialNodeData
		{
			public string name = "electricstool";

			public CardChoicesType choicesType { get; set; }

			// Token: 0x04000C65 RID: 3173
			public List<CardChoice> overrideChoices { get; set; }

			// Token: 0x04000C66 RID: 3174
			public bool gemifyChoices { get; set; }

			//public CustomNode1 tradeCardsData { get; set; }


			// Token: 0x1700021F RID: 543
			// (get) Token: 0x060011C7 RID: 4551 RVA: 0x0003E26D File Offset: 0x0003C46D
			public override string PrefabPath
			{
				get { return "Prefabs/Map/MapNodesPart1/MapNode_Empty"; }
			}

			// Token: 0x17000220 RID: 544
			// (get) Token: 0x060011C8 RID: 4552 RVA: 0x0003E274 File Offset: 0x0003C474
			//public override List<NodeData.SelectionCondition> GenerationPrerequisiteConditions
			//{
			//	get
			//	{
			//		return new List<NodeData.SelectionCondition>
			//		{
			//			new NodeData.PreviousNodesContent(typeof(CustomNode1), false),
			//			new NodeData.StoryEventCompleted(StoryEvent.ArchivistDefeated),
			//			new NodeData.WithinRegionIndexRange(0, 0),
			//			new NodeData.PastRunsCompleted(3),
			//			new NodeData.WithinGridYRange(2, int.MaxValue)
			//		};
			//	}
			//}
		}








	}
   
