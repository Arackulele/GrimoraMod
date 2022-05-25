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
using HarmonyLib;
using InscryptionAPI;
using InscryptionAPI.Card;
using InscryptionAPI.Helpers;
using Sirenix.Utilities;
using UnityEngine;

namespace GrimoraMod;

[BepInDependency(InscryptionAPIPlugin.ModGUID)]
[BepInPlugin(GUID, Name, Version)]
public partial class GrimoraPlugin : BaseUnityPlugin
{
	public const string GUID = "arackulele.inscryption.grimoramod";
	public const string Name = "GrimoraMod";
	private const string Version = "2.8.81";

	internal static ManualLogSource Log;

	private static Harmony _harmony;

	public static List<GameObject> AllPrefabs;
	public static List<Material> AllMats;
	public static List<RuntimeAnimatorController> AllControllers;
	public static List<Sprite> AllSprites;
	public static List<AudioClip> AllSounds;
	public static List<Texture> AllAbilitiesTextures;
	public static List<Mesh> AllMesh;

	// Gets populated in CardBuilder.Build()
	public static List<CardInfo> AllGrimoraModCards = new();
	public static List<string> AllGrimoraModCardsNoGuid = new();
	public static List<CardInfo> AllPlayableGrimoraModCards = new();
	
	public static bool Initialized { get; set; } 

	private void Awake()
	{
		Log = Logger;

		_harmony = Harmony.CreateAndPatchAll(Assembly.GetExecutingAssembly(), GUID);

		ConfigHelper.Instance.BindConfig();

		LoadAssetsSync();

		StartCoroutine(LoadAssetsAsync());
		
		LoadAbilitiesAndCards();
		
		LoadExpansionCards();
		
		StarterDecks.RegisterStarterDecks();
		
		ChallengeManagement.UpdateGrimoraChallenges();

		Initialized = true;
	}



#if DEBUG
	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.Alpha1))
		{
			CustomCoroutine.Instance.StartCoroutine(LifeManager.Instance.ShowDamageSequence(1, 1, false));
		}
	}
#endif

	private void LoadAssetsSync()
	{
		AllAbilitiesTextures = AssetUtils.LoadAssetBundle<Texture>("grimoramod_abilities");
		AllControllers = AssetUtils.LoadAssetBundle<RuntimeAnimatorController>("grimoramod_controller");
		AllMats = AssetUtils.LoadAssetBundle<Material>("grimoramod_mats");
		AllSounds = AssetUtils.LoadAssetBundle<AudioClip>("grimoramod_sounds");
		AllSprites = AssetUtils.LoadAssetBundle<Sprite>("grimoramod_sprites");
		var kopie = AssetUtils.LoadAssetBundle<Sprite>("grimora_kopiebunde");
		#if DEBUG
		foreach (var s in kopie)
		{
			Log.LogInfo($"Sprite added by Kopie {s}");
		}
		#endif

		AllSprites.AddRange(kopie);
		AllMesh = AssetUtils.LoadAssetBundle<Mesh>("grimoramod_mesh");
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
		Add_Card_Jikininki();                 // Bt Y#0895
		Add_Card_Kennel();                    // Bt Y#0895
		Add_Card_Manananggal();               // Bt Y#0895
		Add_Card_Mummy();                     // Bt Y#0895
		Add_Card_Obol();                      // Bt Y#0895
		Add_Card_PirateExploding();           // Lich underling#7678, then Ara
		Add_Card_PirateFirstMateSnag();       // Bt Y#0895
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
		Add_Card_Summoner();                  // Cevin2006™ (◕‿◕)#7971
		Add_Card_TamperedCoffin();            // Bt Y#0895
		Add_Card_TombRobber();                // LavaErrorDoggo#1564
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
		//extra cards dll doesnt actually do anything, as its probably more convenient for us to have everything in the main project
		if (Chainloader.PluginInfos.ContainsKey("arackulele.inscryption._grimoramodextracards"))
		{
			Log.LogInfo($"Detected expansion mod, adding a new set of cards!");

			#region Normal

			Add_Card_BloodySack();       // Bt Y#0895
			Add_Card_CompoundFracture(); // Bt Y#0895
			Add_Card_DisturbedGrave();   // Bt Y#0895
			Add_Card_FesteringWretch();  // Bt Y#0895
			Add_Card_Flameskull();       // Bt Y#0895
			Add_Card_Haltia();           // Bt Y#0895
			Add_Card_HauntedMirror();    // Bt Y#0895
			// Add_Card_IceCube();          // Bt Y#0895
			Add_Card_LaLlorona();       // Bt Y#0895
			Add_Card_Moroi();           // Bt Y#0895
			Add_Card_Nixie();           // Bt Y#0895
			Add_Card_PiratePolly();     // Bt Y#0895
			Add_Card_PiratePrivateer(); // Bt Y#0895
			Add_Card_PossessedArmour(); // Bt Y#0895
			Add_Card_Rot();             // Bt Y#0895
			Add_Card_SkeletonArmy();    // Cevin2006™ (◕‿◕)#7971
			Add_Card_Warthr();          // Bt Y#0895
			Add_Card_Wight();           // Bt Y#0895

			#endregion

			#region Rares

			Add_Card_BalBal();         // Bt Y#0895
			Add_Card_BoneCollective(); // Bt Y#0895
			Add_Card_EmberSpirit();    // Cevin2006™ (◕‿◕)#7971
			Add_Card_Fylgja();         // Bt Y#0895
			Add_Card_Nosferat();       // Bt Y#0895
			Add_Card_Wyvern();         // Cevin2006™ (◕‿◕)#7971

			#endregion
		}
	}

	private void AddDebugCards()
	{
		CardInfo trapInfo = "Trap".GetCardInfo();
		CardBuilder.Builder
		 .SetAbilities(Ability.BoneDigger, Ability.SteelTrap, Haunter.ability)
		 .SetBaseAttackAndHealth(0, 1)
		 .SetNames($"{GUID}_!TRAP", "!TEST Trap", trapInfo.portraitTex)
		 .Build();

		CardBuilder.Builder
		 .SetAbilities(Ability.DeathShield)
		 .SetBaseAttackAndHealth(0, 99)
		 .SetNames($"{GUID}_!BLOCKER", "!TEST Blocker", trapInfo.portraitTex)
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
		ConfigHelper.Instance.HandleHotReloadBefore();
		Resources.UnloadUnusedAssets();
		GrimoraModBattleSequencer.ActiveEnemyPiece = null;
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
}
