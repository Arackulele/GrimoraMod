global using Color = UnityEngine.Color;
global using UnityObject = UnityEngine.Object;
global using UnityRandom = UnityEngine.Random;
using System.Collections;
using System.Diagnostics;
using System.Reflection;
using BepInEx;
using BepInEx.Bootstrap;
using BepInEx.Logging;
using DiskCardGame;
using GrimoraMod.Consumables;
using HarmonyLib;
using InscryptionAPI;
using InscryptionAPI.Card;
using InscryptionAPI.Helpers;
using InscryptionAPI.Guid;
using InscryptionAPI.Helpers.Extensions;
using InscryptionAPI.Items;
using JetBrains.Annotations;
using Sirenix.Utilities;
using UnityEngine;
using Infiniscryption.Achievements;
using static InscryptionAPI.Card.AbilityManager;
using static DiskCardGame.ConceptNode;
using GBC;
using GrimoraMod.Core.Consumables.Secret;

namespace GrimoraMod;


[BepInDependency(InscryptionAPIPlugin.ModGUID)]
[BepInDependency("community.inscryption.patch")]
[BepInDependency("zorro.inscryption.infiniscryption.achievements")]
[BepInPlugin(GUID, Name, Version)]
public partial class GrimoraPlugin : BaseUnityPlugin
{
	public const string GUID = "arackulele.inscryption.grimoramod";
	public const string Name = "GrimoraMod";
	private const string Version = "3.3.2";

	internal static ManualLogSource Log;

	private static Harmony _harmony;

	public static List<GameObject> AllPrefabs =new List<GameObject>();
	public static List<Material> AllMats;
	public static List<RuntimeAnimatorController> AllControllers;
	public static List<Sprite> AllSprites;
	public static List<AudioClip> AllSounds;
	public static List<Texture> AllAbilitiesTextures;
	public static List<Mesh> AllMesh;
	public static List<Font> AllFonts;

	// Gets populated in CardBuilder.Build()
	public static List<CardInfo> AllGrimoraModCards = new();
	public static List<string> AllGrimoraModCardsNoGuid = new();
	public static List<CardInfo> AllPlayableGrimoraModCards = new();
	public static List<ConsumableItemData> ObtainableGrimoraItems = new();
	public static List<ConsumableItemData> GrimoraItemsSecret = new();


	public static readonly CardMetaCategory GrimoraChoiceNode = GuidManager.GetEnumValue<CardMetaCategory>(GUID, "GrimoraModChoiceNode");
	public static readonly AbilityMetaCategory ElectricChairLevel1 = GuidManager.GetEnumValue<AbilityMetaCategory>(GUID, "ElectricChairLevel1");
	public static readonly AbilityMetaCategory ElectricChairLevel2 = GuidManager.GetEnumValue<AbilityMetaCategory>(GUID, "ElectricChairLevel2");
	public static readonly AbilityMetaCategory ElectricChairLevel3 = GuidManager.GetEnumValue<AbilityMetaCategory>(GUID, "ElectricChairLevel3");

	//Defeat Kaycee and unfreeze at least 5 of your own cards
	internal static Achievement GrimReminder { get; private set; }
	//Defeat Sawyers phase 2 without killing the hellhound
	internal static Achievement CowardsEnd { get; private set; }
	//Destroy Royals ship with a single attack
	internal static Achievement SeasonOfStorms { get; private set; }
	//Defeat Grimora without defeating the bonelord 
	internal static Achievement TheBlackBirdTheDarkSlope { get; private set; }
	//Have 30 or more bones at the end of a turn
	internal static Achievement BoneSaw { get; private set; }
	//Beat skullstorm
	internal static Achievement PileOfSkulls { get; private set; }
	//Beat grimora
	internal static Achievement DanceOfDoom { get; private set; }
	//Reach 6 Souls by turn 3
	internal static Achievement TheSpiritsWay { get; private set; }
	//Use the dead hand item when out of cards
	internal static Achievement WailOfTheDamned { get; private set; }
	//Get an ourobones to deal 6 damage
	internal static Achievement SomethingEnds { get; private set; }
	//Sacrifice an obol with 5 sigils to the bone lord
	internal static Achievement CullTheWeak { get; private set; }
	//Beat Hell Mode
	internal static Achievement GatewayToDis { get; private set; }

	public static bool Initialized { get; set; } 

	private void Awake()
	{
		Log = Logger;

		Log.LogInfo("[GrimoraPlugin] Loading patches...");
		_harmony = Harmony.CreateAndPatchAll(Assembly.GetExecutingAssembly(), GUID);
		Log.LogInfo("[GrimoraPlugin] Done patches...");

		ConfigHelper.Instance.BindConfig();

		LoadAssetsSync();

		StartCoroutine(LoadAssetsAsync());

		LoadAbilitiesAndCards();
		
		LoadExpansionCards();
		
		
		StarterDecks.RegisterStarterDecks();
		
		ChallengeManagement.UpdateGrimoraChallenges();

		//StartCoroutine(LoadMetaCategories());

		AddAchievements();

		Initialized = true;

		LoadItems();

		AbilityManager.ModifyAbilityList += delegate (List<FullAbility> abilities)
		{
			foreach (FullAbility ability in abilities)
			{
				if (ElectricChairLever.AbilitiesSaferRisk.Contains(ability.Info.ability)) { ability.Info.AddMetaCategories(ElectricChairLevel1); }
				if (ElectricChairLever.AbilitiesMinorRisk.Contains(ability.Info.ability)) { ability.Info.AddMetaCategories(ElectricChairLevel2); }
				if (ElectricChairLever.AbilitiesMajorRisk.Contains(ability.Info.ability)) { ability.Info.AddMetaCategories(ElectricChairLevel3); }
			}
			return abilities;
		};

		Log.LogInfo("[GrimoraPlugin] Initialized");
	}




	private void Update()
	{


		/*
		if (Input.GetKeyDown(KeyCode.Alpha1))
		{
			CustomCoroutine.Instance.StartCoroutine(LifeManager.Instance.ShowDamageSequence(1, 1, false));
		}

		if (Input.GetKeyDown(KeyCode.Alpha2))
		{
			AchievementManager.Unlock(GrimReminder);
			AchievementManager.Unlock(CowardsEnd);
			AchievementManager.Unlock(SeasonOfStorms);
			AchievementManager.Unlock(TheBlackBirdTheDarkSlope);
			AchievementManager.Unlock(BoneSaw);
			AchievementManager.Unlock(TheSpiritsWay);
			AchievementManager.Unlock(PileOfSkulls);
			AchievementManager.Unlock(DanceOfDoom);
			AchievementManager.Unlock(WailOfTheDamned);
			AchievementManager.Unlock(SomethingEnds);
			AchievementManager.Unlock(CullTheWeak);
			AchievementManager.Unlock(GatewayToDis);
		}*/

	}



	internal static List<GameObject> NewObjects = new List<GameObject>();
	private void LoadAssetsSync()
	{

		AllAbilitiesTextures = AssetUtils.LoadAssetBundle<Texture>("grimoramod_abilities");
		AllControllers = AssetUtils.LoadAssetBundle<RuntimeAnimatorController>("grimoramod_controller");
		AllMats = AssetUtils.LoadAssetBundle<Material>("grimoramod_mats");
		AllMesh = AssetUtils.LoadAssetBundle<Mesh>("grimoramod_mesh");

		AllFonts = AssetUtils.LoadAssetBundle<Font>("grimoramod_fonts");

		AllSounds = AssetUtils.LoadAssetBundle<AudioClip>("grimoramod_sounds");
		AllSprites = AssetUtils.LoadAssetBundle<Sprite>("grimoramod_sprites");
		var kopieSprites = AssetUtils.LoadAssetBundle<Sprite>("grimora_kopiebunde");

		var kopiePrefabs = AssetUtils.LoadAssetBundle<GameObject>("grimora_kopiebunde");

		var grimora_new_prefabs = AssetUtils.LoadAssetBundle<GameObject>("grimoramod_prefab_new");

		var grimora_new = AssetUtils.LoadAssetBundle<Sprite>("grimoramod_new");

		//var grimora_newmodels = AssetUtils.LoadAssetBundle<GameObject>("grimoramod_new");

		AllSprites.AddRange(grimora_new);
		AllSprites.AddRange(kopieSprites);

		AllPrefabs.AddRange(grimora_new_prefabs);
		NewObjects.AddRange(kopiePrefabs);
		NewObjects.AddRange(grimora_new_prefabs);
	}

	private IEnumerator LoadAssetsAsync()
	{
		Log.LogInfo($"Loading asset bundles");

		yield return StartCoroutine(AssetUtils.LoadAssetBundleAsync<GameObject>("grimoramod_prefabs"));


	}

	// private IEnumerator HotReloadMenuCardAdd()
	// {
	// 	if (ConfigHelper.Instance.isHotReloadEnabled && SceneManager.GetActiveScene().name.Equals("Start"))
	// 	{
	// 		if (MenuController.Instance.cardRow.Find("MenuCard_Grimora").SafeIsUnityNull())
	// 		{
	// 			Log.LogDebug($"Hot reload menu button creation");
	// 			MenuController.Instance.cards.Add(MenuControllerPatches.CreateButton(MenuController.Instance));
	// 		}
	// 	}
	// }

	private IEnumerator Start()
	{

		if (AllPrefabs.IsNullOrEmpty())
		{
			Log.LogWarning($"Prefabs are still being loaded, waiting until they have finished.");
			yield return new WaitUntil(() => !AllPrefabs.IsNullOrEmpty());
		}
	}
	
	private void LoadAbilities()
	{
		// Since cards are split, abilities will be added like this to save some lines
		AccessTools.GetDeclaredMethods(typeof(GrimoraPlugin))
		 .Where(mi => mi.Name.StartsWith("Add_Ability"))
		 .ForEach(mi => mi.Invoke(this, null));
	}

	private void LoadAbilitiesAndCards()
	{
		Stopwatch stopwatch = Stopwatch.StartNew();

		LoadAbilities();
		// Manually adding so splitting cards is easier for future use
		Log.LogDebug($"[LoadAbilitiesAndCards] Loading cards...");

		#region Normal

		Add_Card_Ashes();											// Bt Y#0895
		Add_Card_Avalanche();                 // Anne Bean
		Add_Card_Banshee();                   // vanilla
		Add_Card_Bonehound();                 // vanilla
		Add_Card_Bonelord();                  // Ryan S. Art
		Add_Card_Bonepile();                  // Bt Y#0895
		Add_Card_BonePrince();                // Cevin2006™ (◕‿◕)#7971
		Add_Card_BooHag();                    // Bt Y#0895
		Add_Card_Catacomb();                  // Bt Y#0895
		Add_Card_Centurion();                 // Bt Y#0895
		Add_Card_Dalgyal();                   // Bt Y#0895
		Add_Card_DanseMacabre();              // Bt Y#0895
		Add_Card_Deadeye();                   // Bt Y#0895
		Add_Card_DeadHand();                  // Bt Y#0895
		Add_Card_DeadTree();                  // Bt Y#0895
		Add_Card_Doll();                      // Bt Y#0895
		Add_Card_Draugr();                    // Bt Y#0895
		Add_Card_DrownedSoul();               // Bt Y#0895
		Add_Card_Family();                    // Catboy Stinkbug#4099
		Add_Card_Flames();                    // Cevin2006™ (◕‿◕)#7971
		Add_Card_ForgottenMan();              // Anne Bean
		Add_Card_Franknstein();               // vanilla
		Add_Card_GhostShip();                 // Cevin2006™ (◕‿◕)#7971
		Add_Card_Giant();                     // Bt Y#0895
		Add_Card_TwinGiant_Ephialtes();       // Bt Y#0895
		Add_Card_TwinGiant_Otis();            // Bt Y#0895
		Add_Card_Glacier_Stage1();            // Bt Y#0895
		Add_Card_Glacier_Stage2_FrostGiant(); // Bt Y#0895
		Add_Card_Gravebard();                 // Bt Y#0895
		Add_Card_Gravedigger();               // vanilla
		Add_Card_Hellhand();                  // Bt Y#0895
		Add_Card_HellHound();                 // Cevin2006™ (◕‿◕)#7971

		//Hellhound halloween
		Add_Card_CandyMonster();
		Add_Card_CandyBucket();

		Add_Card_HauntedMirror();							// Bt Y#0895
		Add_Card_Jikininki();                 // Bt Y#0895
		Add_Card_Kennel();                    // Bt Y#0895
		Add_Card_Manananggal();               // Bt Y#0895
		Add_Card_Mummy();                     // Bt Y#0895
		Add_Card_Obol();                      // Bt Y#0895
		Add_Card_PirateExploding();           // Lich underling#7678, then Ara
		Add_Card_PirateFirstMateSnag();       // Bt Y#0895
		Add_Card_PiratePrivateer();						// Bt Y#0895
		Add_Card_PirateSwashbuckler();        // Bt Y#0895
		Add_Card_PlagueDoctor();              // Cevin2006™ (◕‿◕)#7971
		Add_Card_Poltergeist();               // Cevin2006™ (◕‿◕)#7971
		Add_Card_Project();                   // Bt Y#0895
		Add_Card_Revenant();                  // vanilla
		Add_Card_RotTail();                   // Bt Y#0895
		Add_Card_Sarcophagus();               // Bt Y#0895
		Add_Card_Shipwreck();                 // Bt Y#0895
		Add_Card_Shipwreck_Dams();            // Anne Bean
		Add_Card_Skelemagus();                // Cevin2006™ (◕‿◕)#7971
		Add_Card_Sluagh();                    // Bt Y#0895
		Add_Card_Spectrabbit();               // Bt Y#0895
		Add_Card_Spectre();                   // Bt Y#0895
		Add_Card_Summoner();                  // Cevin2006™ (◕‿◕)#7971
		Add_Card_TamperedCoffin();            // Bt Y#0895
		Add_Card_TombRobber();                // LavaErrorDoggo#1564
		Add_Card_Urn();                       // Bt Y#0895
		Add_Card_Vampire();                   // gabe
		Add_Card_Vellum();                    // Bt Y#0895
		Add_Card_VengefulSpirit();            // Cevin2006™ (◕‿◕)#7971
		Add_Card_WardingPresence();           // Bt Y#0895
		Add_Card_Wechuge();                   // Bt Y#0895
		Add_Card_WillOTheWisp();              // Bt Y#0895
		Add_Card_Zombie();                    // Bt Y#0895

		#endregion

		#region Rares

		Add_Card_Animator();                        // Bt Y#0895
		Add_Card_Apparition();                      // Bt Y#0895
		Add_Card_BonelordsHorn();                   // Cevin2006™ (◕‿◕)#7971
		Add_Card_CalaveraCatrina();                 // Bt Y#0895
		Add_Card_PirateDavyJones();                 // Bt Y#0895
		Add_Card_DavyJonesLocker();                 // Bt Y#0895
		Add_Card_DeadManWalking();										// Bt Y#0895
		Add_Card_DeadPets();                        // Bt Y#0895
		Add_Card_DeathKnell();                      // Bt Y#0895
		Add_Card_DeathKnellBell();                  // Bt Y#0895
		Add_Card_Dybbuk();                          // Bt Y#0895
		Add_Card_Gashadokuro_Stage1_MassGrave();    // Bt Y#0895
		Add_Card_Gashadokuro_Stage2_RisingHunger(); // Bt Y#0895
		Add_Card_Gashadokuro_Stage3();              // Bt Y#0895
		Add_Card_GhostShipRoyal();                  // Cevin2006™ (◕‿◕)#7971
		Add_Card_HeadlessHorseman();                // Cevin2006™ (◕‿◕)#7971
		Add_Card_Hydra();                           // Cevin2006™ (◕‿◕)#7971
		Add_Card_Necromancer();                     // Bt Y#0895
		Add_Card_Nixie();														// Bt Y#0895
		Add_Card_Ourobones();												// Anne Bean?
		Add_Card_PirateCaptainYellowbeard();        // Bt Y#0895
		Add_Card_Ripper();                          // Bt Y#0895
		Add_Card_ScreamingSkull();                  // Bt Y#0895
		Add_Card_Silbon();                          // Bt Y#0895
		Add_Card_Sporedigger();                     // LavaErrorDoggo#1564
		Add_Card_StarvedMan();                      // ~=Lost Soul=~
		Add_Card_Writher();                         // Bt Y#0895
		Add_Card_WritherTail();                     // Bt Y#0895

		#endregion

		#region Egypt
		
		Add_Card_Boneless();                        // Bt Y#0895
		Add_Card_Eidolon();                         // Bt Y#0895
		Add_Card_Boneclaw();                        //gold#2445
		Add_Card_EgyptMummy();                      //gold#2445


		#endregion

		Add_Card_Obelisk();         //keks307#7315
		Add_Card_Voodoo_Doll();
		Add_Card_DisturbedGrave();   // Bt Y#0895
		Add_Card_DisturbedGraveNonTerrain(); // Bt Y#0895

		AllGrimoraModCards.Sort((info1, info2) => string.Compare(info1.name, info2.name, StringComparison.Ordinal));
		AllPlayableGrimoraModCards = AllGrimoraModCards.Where(info => info.metaCategories.Any()).ToList();

		// change just the artwork of Starvation
		CardInfo card = CardManager.BaseGameCards.CardByName("Starvation");
		card.portraitTex = AllSprites.Find(sp => sp.name.Equals("Starvation"));
		card.portraitTex.RegisterEmissionForSprite(AllSprites.Find(sp => sp.name.Equals("Starvation_emission")));

		AddDebugCards();

		stopwatch.Stop();
		Log.LogDebug($"[LoadAbilitiesAndCards] Finished loading all abilities and cards in [{stopwatch.ElapsedMilliseconds}]ms");
	}
	
	private void LoadExpansionCards()
	{
		//these are needed for Boss Fights, only obtainable when expansion is enabled

		Add_Card_Flameskull();       // Bt Y#0895
		Add_Card_EmberSpirit();    // Cevin2006™ (◕‿◕)#7971

		//extra cards dll doesnt actually do anything, as its probably more convenient for us to have everything in the main project
		if (Chainloader.PluginInfos.ContainsKey("arackulele.inscryption._grimoramodextracards"))
		{
			Log.LogInfo($"Detected expansion mod, adding a new set of cards!");

			#region Normal

			Add_Card_RandomCard();

			Add_Card_Bigbones();					// Bt Y#0895
			Add_Card_BloodySack();				// Bt Y#0895
			Add_Card_CompoundFracture();	// Bt Y#0895
			Add_Card_Crossbones();				// Bt Y#0895
			Add_Card_FesteringWretch();  // Bt Y#0895
			Add_Card_GratefulDead();          // Bt Y#0895
			Add_Card_Haltia();           // Bt Y#0895
			Add_Card_IceCube();          // Bt Y#0895
			Add_Card_LaLlorona();       // Bt Y#0895
			Add_Card_Moroi();           // Bt Y#0895
			Add_Card_PiratePolly();     // Bt Y#0895
			Add_Card_PossessedArmour(); // Bt Y#0895
			Add_Card_Rot();             // Bt Y#0895
			Add_Card_SkeletonArmy();    // Cevin2006™ (◕‿◕)#7971
			Add_Card_Warthr();          // Bt Y#0895
			Add_Card_Wight();           // Bt Y#0895

			#endregion

			#region Rares

			Add_Card_BalBal();         // Bt Y#0895
			Add_Card_BoneCollective(); // Bt Y#0895
			Add_Card_Fylgja();         // Bt Y#0895
			Add_Card_GraveCarver();    // Bt Y#0895
			Add_Card_Nosferat();       // Bt Y#0895
			Add_Card_Extoplasm();         // Cevin2006™ (◕‿◕)#7971
			Add_Card_SlingersSoul();
				#endregion
			}
	}

	public static GameObject HandModel;

	public static GameObject FemurModel;

	private void LoadItems()
	{

		GameObject UrnModel = GameObject.Instantiate(NewObjects.Find(g => g.name.Contains("UrnPrefab")));

		GameObject HornModel = GameObject.Instantiate(NewObjects.Find(g => g.name.Contains("BoneLordsHornPrefab")));

		HandModel = GameObject.Instantiate(NewObjects.Find(g => g.name.Contains("HandPrefab")));

		GameObject TrowelModel = GameObject.Instantiate(NewObjects.Find(g => g.name.Contains("TrowelPrefab")));

		GameObject EmbalmModel = GameObject.Instantiate(NewObjects.Find(g => g.name.Contains("Embalming_Fluid_Prefab")));

		GameObject ShipBottlePrefab = GameObject.Instantiate(NewObjects.Find(g => g.name.Contains("ShipBottlePrefab")));

		GameObject QuillPrefab = GameObject.Instantiate(NewObjects.Find(g => g.name.Contains("QuillPrefab")));

		GameObject MalletPrefab = GameObject.Instantiate(NewObjects.Find(g => g.name.Contains("MalletPrefab")));

		GameObject ChiselPrefab = GameObject.Instantiate(NewObjects.Find(g => g.name.Contains("Chisel_Prefab")));

		GameObject Piggy1 = GameObject.Instantiate(NewObjects.Find(g => g.name.Contains("piggyphase1Prefab")));

		GameObject Piggy2 = GameObject.Instantiate(NewObjects.Find(g => g.name.Contains("piggyphase2Prefab")));

		GameObject Piggy3 = GameObject.Instantiate(NewObjects.Find(g => g.name.Contains("piggyphase3Prefab")));

		FemurModel = GameObject.Instantiate(NewObjects.Find(g => g.name.Contains("FemurPrefab")));

		GrimoraCardInABottle.CreateModel();
		ObtainableGrimoraItems.Add(GrimoraCardInABottle.NewCardBottleItem(NameRevenant));
		ObtainableGrimoraItems.Add(GrimoraCardInABottle.NewCardBottleItem(NameBonepile));

		ObtainableGrimoraItems.Add(EmbalmingFluid.NewEmbalmingFluid(EmbalmModel));

		ObtainableGrimoraItems.Add(ShipBottle.NewShipBottle(ShipBottlePrefab));

		ObtainableGrimoraItems.Add(GrimoraUrn.NewGrimoraUrn(UrnModel));

		ObtainableGrimoraItems.Add(DeadHandItem.NewDeadHand(HandModel));

		ObtainableGrimoraItems.Add(BoneHorn.NewBoneHorn(HornModel));

		ObtainableGrimoraItems.Add(Trowel.NewTrowel(TrowelModel));

		ObtainableGrimoraItems.Add(Mallet.NewMallet(MalletPrefab));

		ObtainableGrimoraItems.Add(Quill.NewQuill(QuillPrefab));

		GrimoraItemsSecret.Add(BoneLordsFemur.NewBoneLordsFemur(FemurModel));

		GrimoraItemsSecret.Add(SliveredBank.NewSliveredBank(Piggy1));
		GrimoraItemsSecret.Add(SliveredBank2.NewSliveredBank(Piggy2));
		GrimoraItemsSecret.Add(SliveredBank3.NewSliveredBank(Piggy3));

		GrimoraItemsSecret.Add(GravecarversChisel.NewGravecarversChisel(ChiselPrefab));

	}



	private void AddAchievements()
	{

		


		ModdedAchievementManager.AchievementGroup groupId = ModdedAchievementManager.NewGroup(
		GUID,
		"Grimora Mod Achievements",
		AssetUtils.GetPrefab<Texture>("achievement_lock").ToTexture2D()
		).ID;


		GrimReminder = ModdedAchievementManager.New(
		GUID,       // plugin guid
		"Grim Reminder",  // Achievement name
		"Defeat Kaycee and thaw at least 5 of your cards.",
		false,
		groupId,
		AssetUtils.GetPrefab<Texture>("achievement_kc").ToTexture2D()
		).ID;

		CowardsEnd = ModdedAchievementManager.New(
		GUID,       // plugin guid
		"Cowards End",  // Achievement name
		"Defeat Sawyers second phase without hurting the hellhound.",
		false,
		groupId,
		AssetUtils.GetPrefab<Texture>("achievement_sawyer").ToTexture2D()
		).ID;

		

		SeasonOfStorms = ModdedAchievementManager.New(
		GUID,       // plugin guid
		"Season Of Storms",  // Achievement name
		"Defeat Royal and have no cards walk the plank.",
		false,
		groupId,
		AssetUtils.GetPrefab<Texture>("achievement_royal").ToTexture2D()
		).ID;

		TheBlackBirdTheDarkSlope = ModdedAchievementManager.New(
		GUID,       // plugin guid
		"The Black Bird//",  // Achievement name
		"The Dark Slope. Defeat [redacted] without defeating [redacted].",
		false,
		groupId,
		AssetUtils.GetPrefab<Texture>("achievement_lord").ToTexture2D()
		).ID;

		BoneSaw = ModdedAchievementManager.New(
		GUID,       // plugin guid
		"Bonesaw",  // Achievement name
		"End a turn with at least 30 bones.",
		false,
		groupId,
		AssetUtils.GetPrefab<Texture>("achievement_saw").ToTexture2D()
		).ID;

		PileOfSkulls = ModdedAchievementManager.New(
		GUID,       // plugin guid
		"Pile Of Skulls",  // Achievement name
		"Win with all challenges enabled and no antichallenges enabled.",
		false,
		groupId,
		AssetUtils.GetPrefab<Texture>("achievement_pit").ToTexture2D()
		).ID;

		DanceOfDoom = ModdedAchievementManager.New(
		GUID,       // plugin guid
		"Dance of Doom",  // Achievement name
		"Witness the end of everything.",
		false,
		groupId,
		AssetUtils.GetPrefab<Texture>("achievement_dance").ToTexture2D()
		).ID;

		TheSpiritsWay = ModdedAchievementManager.New(
		GUID,       // plugin guid
		"The Spirits Way",  // Achievement name
		"Reach 6 Souls by turn 3.",
		false,
		groupId,
		AssetUtils.GetPrefab<Texture>("achievement_ghost").ToTexture2D()
		).ID;

		WailOfTheDamned = ModdedAchievementManager.New(
		GUID,       // plugin guid
		"Wail Of The Damned",  // Achievement name
		"Use the dead hand item when your deck is empty.",
		false,
		groupId,
		AssetUtils.GetPrefab<Texture>("achievement_hand").ToTexture2D()
		).ID;

		SomethingEnds = ModdedAchievementManager.New(
		GUID,       // plugin guid
		"Something Ends...",  // Achievement name
		"...Something Begins. Get an ourobones to deal 6 damage.",
		false,
		groupId,
		AssetUtils.GetPrefab<Texture>("achievement_ouro").ToTexture2D()
		).ID;

		CullTheWeak = ModdedAchievementManager.New(
		GUID,       // plugin guid
		"Cull the weak",  // Achievement name
		"Sacrifice an Obol with 5 sigils to the Bonelord.",
		false,
		groupId,
		AssetUtils.GetPrefab<Texture>("achievement_cull").ToTexture2D()
		).ID;

		GatewayToDis = ModdedAchievementManager.New(
		GUID,       // plugin guid
		"Gateway to Dis",  // Achievement name
		"Win while Hell mode is active.",
		false,
		groupId,
		AssetUtils.GetPrefab<Texture>("achievement_hell").ToTexture2D()
		).ID;

	}

		private void AddDebugCards()
	{
		CardInfo trapInfo = "Trap".GetCardInfo();
		CardBuilder.Builder
		 .SetAbilities(Burning.ability)
		 .SetBaseAttackAndHealth(1, 1)
		 .SetNames($"{GUID}_!TRAP", "!TEST Trap", trapInfo.portraitTex)
		 .Build();

		CardBuilder.Builder
		 .SetAbilities(Haunter.ability, Ability.IceCube, Ability.LatchBrittle)
		 .SetBaseAttackAndHealth(1, 2)
		 .SetNames($"{GUID}_!BLOCKER","I am going to", trapInfo.portraitTex)
		 .Build();
	}

	private void OnDestroy()
	{
		AllAbilitiesTextures = null;
		AllControllers = null;
		AllMats = null;
		AllMesh = null;
		AllPrefabs = null;
		AllSprites = null;
		AllSounds = null;
		AllGrimoraModCards = new List<CardInfo>();
		AllGrimoraModCardsNoGuid = new List<string>();
		ObtainableGrimoraItems = new List<ConsumableItemData>();
		GrimoraItemsSecret = new List<ConsumableItemData>();
		ConfigHelper.Instance.HandleHotReloadBefore();
		Resources.UnloadUnusedAssets();
		GrimoraModBattleSequencer.ActiveEnemyPiece = null;
		AnkhGuardCombatSequencer.ActiveEnemyPiece = null;
		_harmony?.UnpatchSelf();
	}

	public static void SpawnParticlesOnCard(PlayableCard target, Texture2D tex, bool reduceY = false)
	{
		GravestoneCardAnimationController anim = target.Anim as GravestoneCardAnimationController;
		GameObject gameObject = Instantiate<ParticleSystem>(anim.deathParticles).gameObject;
		ParticleSystem particle = gameObject.GetComponent<ParticleSystem>();
		particle.startColor = Color.white;
		particle.GetComponent<ParticleSystemRenderer>().material =
			new Material(particle.GetComponent<ParticleSystemRenderer>().material) { mainTexture = tex };

		ParticleSystem.MainModule mainMod = particle.main;
		mainMod.startColor = new ParticleSystem.MinMaxGradient(Color.white);
		gameObject.gameObject.SetActive(true);
		gameObject.transform.position = anim.deathParticles.transform.position;
		gameObject.transform.localScale = anim.deathParticles.transform.localScale;
		gameObject.transform.rotation = anim.deathParticles.transform.rotation;
		if (reduceY)
		{
			particle.transform.position = new Vector3(
				particle.transform.position.x,
				particle.transform.position.y - 0.1f,
				particle.transform.position.z
			);
		}

		Destroy(gameObject, 6f);
	}

	static bool addedvoices = false;

	public static void ChangeDialogueSpeaker(string character)
	{
		if (addedvoices == false) {
			AudioController.Instance.SFX.Add(AllSounds.Find(g => g.name.Contains("smallcologistvoice_calm#1")));
			AudioController.Instance.SFX.Add(AllSounds.Find(g => g.name.Contains("smallcologistvoice_calm#2")));
			AudioController.Instance.SFX.Add(AllSounds.Find(g => g.name.Contains("smallcologistvoice_calm#3")));

			AudioController.Instance.SFX.Add(AllSounds.Find(g => g.name.Contains("kayceevoice_calm#1")));
			AudioController.Instance.SFX.Add(AllSounds.Find(g => g.name.Contains("kayceevoice_calm#2")));
			AudioController.Instance.SFX.Add(AllSounds.Find(g => g.name.Contains("kayceevoice_calm#3")));
			AudioController.Instance.SFX.Add(AllSounds.Find(g => g.name.Contains("sawyervoice_calm#1")));
			AudioController.Instance.SFX.Add(AllSounds.Find(g => g.name.Contains("sawyervoice_calm#2")));
			AudioController.Instance.SFX.Add(AllSounds.Find(g => g.name.Contains("sawyervoice_calm#3")));

			AudioController.Instance.SFX.Add(AllSounds.Find(g => g.name.Contains("grimmycologistvoice_calm#1")));
			AudioController.Instance.SFX.Add(AllSounds.Find(g => g.name.Contains("grimmycologistvoice_calm#2")));
			AudioController.Instance.SFX.Add(AllSounds.Find(g => g.name.Contains("grimmycologistvoice_calm#3")));
			AudioController.Instance.SFX.Add(AllSounds.Find(g => g.name.Contains("grimmycologistvoice_suprise#1")));

			addedvoices = true;
		}

		TextDisplayer.SpeakerTextStyle current;


		switch (character)
		{
			default:
			case "grimora":
				current = TextDisplayer.Instance.defaultStyle;
				break;

			case "kaycee":
				current = new TextDisplayer.SpeakerTextStyle()
				{
					color = GameColors.instance.brightBlue,
					font = AllFonts.Find(g => g.name.Contains("FronzyFreeTrial-mLVlP")),
					voiceSoundIdPrefix = "kaycee",
					voiceSoundVolume = 1.2f,
					triangleSprite = AssetUtils.GetPrefab<Sprite>("white_triangle_tech"),

				};
				break;

			case "sawyer":
				current = new TextDisplayer.SpeakerTextStyle()
				{
					color = GameColors.instance.brownOrange,
					font = AllFonts.Find(g => g.name.Contains("zai_ConsulPolishTypewriter")),
					voiceSoundIdPrefix = "sawyer",
					voiceSoundVolume = 1.4f,
					triangleSprite = AssetUtils.GetPrefab<Sprite>("white_triangle_tech"),

				};
				break;

			case "royal":
				current = new TextDisplayer.SpeakerTextStyle()
				{
					color = GameColors.instance.blue,
					font = AllFonts.Find(g => g.name.Contains("Pieces_of_Eight")),
					voiceSoundIdPrefix = "pirateskull",
					triangleSprite = AssetUtils.GetPrefab<Sprite>("white_triangle_tech"),
				};
				break;

			case "mycologist":
				current = new TextDisplayer.SpeakerTextStyle()
				{
					color = GameColors.instance.brightLimeGreen,
					font = ResourceBank.Get<Font>("fonts/3d scene fonts/VICIOUSHUNGER"),
					voiceSoundIdPrefix = "grimmycologist",
					voiceSoundVolume = 1.4f,
					fontSizeChange = 8,
					triangleSprite = AssetUtils.GetPrefab<Sprite>("white_triangle_tech"),
				};
				break;

			case "mycologistside":
				current = new TextDisplayer.SpeakerTextStyle()
				{
					color = GameColors.instance.limeGreen,
					font = ResourceBank.Get<Font>("fonts/3d scene fonts/VICIOUSHUNGER"),
					voiceSoundIdPrefix = "smallcologist",
					voiceSoundVolume = 1.4f,
					triangleSprite = AssetUtils.GetPrefab<Sprite>("white_triangle_tech"),
				};
				break;

			case "bonelord":
				current = new TextDisplayer.SpeakerTextStyle()
				{
					color = GameColors.instance.brightRed,
					font = ResourceBank.Get<Font>("fonts/3d scene fonts/VICIOUSHUNGER"),
					voiceSoundIdPrefix = "bonelord",
					triangleSprite = AssetUtils.GetPrefab<Sprite>("white_triangle_tech"),
				};
				break;
		}

		TextDisplayer.Instance.alternateSpeakerStyles = new List<TextDisplayer.SpeakerTextStyle> { current };
		TextDisplayer.Instance.SetTextStyle(current);


	}

}
