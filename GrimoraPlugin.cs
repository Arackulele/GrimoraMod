global using Object = UnityEngine.Object;
using System.Reflection;
using BepInEx;
using BepInEx.Logging;
using DiskCardGame;
using HarmonyLib;
using Sirenix.Utilities;
using UnityEngine;

namespace GrimoraMod;

[BepInDependency("cyantist.inscryption.api")]
[BepInPlugin(GUID, Name, Version)]
public partial class GrimoraPlugin : BaseUnityPlugin
{
	public const string GUID = "arackulele.inscryption.grimoramod";
	public const string Name = "GrimoraMod";
	private const string Version = "2.7.6";

	internal static ManualLogSource Log;

	private static Harmony _harmony;

	public static List<GameObject> AllPrefabs;
	public static List<Material> AllMats;
	public static List<RuntimeAnimatorController> AllControllers;
	public static List<Sprite> AllSprites;
	public static List<Texture> AllAbilityTextures;
	public static List<AudioClip> AllSounds;

	// Gets populated in CardBuilder.Build()
	public static List<CardInfo> AllGrimoraModCards = new();

	private void Awake()
	{
		Log = Logger;

		_harmony = Harmony.CreateAndPatchAll(Assembly.GetExecutingAssembly(), GUID);

		ConfigHelper.Instance.BindConfig();

		LoadAssets();

		LoadAbilities();

		LoadCards();

		if (ConfigHelper.Instance.isHotReloadEnabled)
		{
			Transform cardRow = MenuController.Instance.cardRow;
			if (cardRow.Find("MenuCard_Grimora") is null)
			{
				MenuController.Instance.cards.Add(MenuControllerPatches.CreateButton(MenuController.Instance));
			}
		}

		ConfigHelper.Instance.HandleHotReloadAfter();
	}

	private void LoadAbilities()
	{
		ActivatedDrawSkeletonGrimora.Create();
		ActivatedEnergyDrawWyvern.Create();
		AlternatingStrike.Create();
		AreaOfEffectStrike.Create();
		BoneLordsReign.Create();
		CreateArmyOfSkeletons.Create();
		CreateKnells.Create();
		Erratic.Create();
		FlameStrafe.Create();
		GiantStrike.Create();
		GiantStrikeEnraged.Create();
		GrimoraRandomAbility.Create();
		HookLineAndSinker.Create();
		InvertedStrike.Create();
		LitFuse.Create();
		Possessive.Create();
		SkinCrawler.Create();
		SpiritBearer.Create();

		#region Special

		GainAttackBones.Create();
		GrimoraGiant.Create();
		LammergeierAttack.Create();

		#endregion
	}

	private void LoadCards()
	{
		#region Normal

		Add_ArmoredZombie();            // Ara
		Add_Banshee();                  // vanilla
		Add_BonePrince();               // Cevin2006™ (◕‿◕)#7971
		Add_Bonehound();                // vanilla
		Add_Bonelord();                 // Ryan S Art
		Add_Bonepile();                 // Bt Y#0895
		Add_BooHag();                   // Bt Y#0895
		Add_Catacomb();                 // Bt Y#0895
		Add_Centurion();                // Bt Y#0895
		Add_CompoundFracture();         // Bt Y#0895
		Add_CorpseMaggots();            // vanilla
		Add_Dalgyal();                  // Bt Y#0895
		Add_DanseMacabre();             // Bt Y#0895
		Add_DeadHand();                 // Ara?
		Add_Deadeye();                  // Bt Y#0895
		Add_Draugr();                   // Bt Y#0895
		Add_DrownedSoul();              // LavaErrorDoggo#1564
		Add_ExplodingPirate();          // Lich underling#7678, then Ara
		Add_Family();                   // Catboy Stinkbug#4099
		Add_FesteringWretch();          // Bt Y#0895
		Add_Flames();                   // Ara
		Add_Franknstein();              // vanilla
		Add_GhostShip();                // Cevin2006™ (◕‿◕)#7971
		Add_Giant();                    // Bt Y#0895
		Add_GraveDigger();              // vanilla
		Add_HellHound();                // Cevin2006™ (◕‿◕)#7971
		Add_Hellhand();                 // Bt Y#0895 
		Add_Manananggal();              // Bt Y#0895
		Add_MudWorm();                  // LavaErrorDoggo#1564
		Add_Mummy();                    // Bt Y#0895
		Add_Obol();                     // Bt Y#0895
		Add_PlagueDoctor();             // Cevin2006™ (◕‿◕)#7971
		Add_Poltergeist();              // Cevin2006™ (◕‿◕)#7971
		Add_Project();                  // Bt Y#0895
		Add_PirateCaptainYellowbeard(); // Bt Y#0895
		Add_PirateFirstMateSnag();      // Bt Y#0895
		Add_PiratePrivateer();          // Bt Y#0895
		Add_PirateSwashbuckler();       // Bt Y#0895
		Add_Revenant();                 // vanilla
		Add_Sarcophagus();              // Bt Y#0895
		Add_Skelemagus();               // Cevin2006™ (◕‿◕)#7971
		Add_SkeletonArmy();             // LavaErrorDoggo#1564 ?
		Add_Summoner();                 // Ara
		Add_TombRobber();               // LavaErrorDoggo#1564
		Add_Vellum();                   // Bt Y#0895
		Add_VengefulSpirit();           // Cevin2006™ (◕‿◕)#7971
		Add_Wendigo();                  // Cevin2006™ (◕‿◕)#7971
		Add_WillOTheWisp();             // Bt Y#0895
		Add_Zombie();                   // Bt Y#0895

		#endregion


		#region Rares

		Add_Amoeba();           // vanilla
		Add_BonelordsHorn();    // Cevin2006™ (◕‿◕)#7971
		Add_DeadPets();         // LavaErrorDoggo#1564
		Add_DeathKnell();       // Bt Y#0895
		Add_DeathKnellBell();   // Bt Y#0895
		Add_Dybbuk();           // Bt Y#0895
		Add_EmberSpirit();      // Cevin2006™ (◕‿◕)#7971
		Add_HeadlessHorseman(); // Cevin2006™ (◕‿◕)#7971
		Add_Hydra();            // Cevin2006™ (◕‿◕)#7971
		Add_Necromancer();      // Bt Y#0895
		Add_Ripper();           // Bt Y#0895
		Add_ScreamingSkull();   // Bt Y#0895
		Add_Silbon();           // Bt Y#0895
		Add_SporeDigger();      // LavaErrorDoggo#1564
		Add_Wyvern();           // Cevin2006™ (◕‿◕)#7971
		Add_ZombieGeck();       // LavaErrorDoggo#1564 ?

		#endregion

		AllGrimoraModCards.Sort((info, cardInfo) => string.Compare(info.name, cardInfo.name, StringComparison.Ordinal));
	}

	private void OnDestroy()
	{
		AllAbilityTextures = null;
		AllControllers = null;
		AllMats = null;
		AllPrefabs = null;
		AllSprites = null;
		AllSounds = null;
		AllGrimoraModCards = new List<CardInfo>();
		ConfigHelper.Instance.HandleHotReloadBefore();
		SkinCrawler.SlotsThatHaveCrawlersHidingUnderCards = new List<CardSlot>();
		Resources.UnloadUnusedAssets();
		_harmony?.UnpatchSelf();
		GrimoraModBattleSequencer.ActiveEnemyPiece = null;

		Destroy(ChessboardMapExt.Instance);
		Destroy(DeckReviewSequencer.Instance);
		Destroy(ResourceDrone.Instance);
		Destroy(FindObjectOfType<BoneyardBurialSequencer>());
		Destroy(FindObjectOfType<DebugHelper>());
		Destroy(FindObjectOfType<GrimoraModBattleSequencer>());
		Destroy(FindObjectOfType<GrimoraModBossBattleSequencer>());
		Destroy(FindObjectOfType<GrimoraCardRemoveSequencer>());
		Destroy(FindObjectOfType<BoonIconInteractable>());
		Destroy(FindObjectOfType<GrimoraRareChoiceGenerator>());
		Destroy(FindObjectOfType<SpecialNodeHandler>());
		FindObjectsOfType<ChessboardPiece>().ForEach(_ => Destroy(_.gameObject));
		GC.Collect();
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

	private static void LoadAssets()
	{
		Log.LogInfo($"Loading asset bundles");
		AllAbilityTextures = AssetUtils.LoadAssetBundle<Texture>("grimoramod_abilities");

		AllMats = AssetUtils.LoadAssetBundle<Material>("grimoramod_mats");

		AllPrefabs = AssetUtils.LoadAssetBundle<GameObject>("grimoramod_prefabs");

		AllSprites = AssetUtils.LoadAssetBundle<Sprite>("grimoramod_sprites");

		AllControllers = AssetUtils.LoadAssetBundle<RuntimeAnimatorController>("grimoramod_controller");

		AllSounds = AssetUtils.LoadAssetBundle<AudioClip>("grimoramod_sounds");
	}
}
