global using UnityObject = UnityEngine.Object;
using System.Collections;
using System.Reflection;
using BepInEx;
using BepInEx.Logging;
using DiskCardGame;
using HarmonyLib;
using InscryptionAPI.Card;
using InscryptionAPI.Helpers;
using UnityEngine;

namespace GrimoraMod;

[BepInDependency(InscryptionAPI.InscryptionAPIPlugin.ModGUID)]
[BepInPlugin(GUID, Name, Version)]
public partial class GrimoraPlugin : BaseUnityPlugin
{
	public const string GUID = "arackulele.inscryption.grimoramod";
	public const string Name = "GrimoraMod";
	private const string Version = "2.8.3";

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

		AllSprites = AssetUtils.LoadAssetBundle<Sprite>("grimoramod_sprites");
	}

	// private IEnumerator HotReloadMenuCardAdd()
	// {
	// 	if (ConfigHelper.Instance.isHotReloadEnabled && SceneManager.GetActiveScene().name.Equals("Start"))
	// 	{
	// 		if (MenuController.Instance.cardRow.Find("MenuCard_Grimora").IsNull())
	// 		{
	// 			Log.LogDebug($"Hot reload menu button creation");
	// 			MenuController.Instance.cards.Add(MenuControllerPatches.CreateButton(MenuController.Instance));
	// 		}
	// 	}
	// }

	private IEnumerator Start()
	{
		yield return LoadEverything();

		// yield return new WaitUntil(() => FindObjectOfType<StartScreenThemeSetter>());
		// StartScreenPatches.SetBackgroundToGrimoraTheme(FindObjectOfType<StartScreenThemeSetter>());
	}

	private IEnumerator LoadEverything()
	{
		yield return LoadAssetsAsync();

		LoadAbilities();

		LoadCards();
	}

	private void LoadAbilities()
	{
		Log.LogDebug($"Loading abilities");

		Anchored.Create();                     // Blind, the Bound Demon
		ActivatedDrawSkeletonGrimora.Create(); // Blind, the Bound Demon
		ActivatedEnergyDrawWyvern.Create();    // Blind, the Bound Demon
		AlternatingStrike.Create();            // Bt Y#0895
		AreaOfEffectStrike.Create();           // Bt Y#0895
		BonelordsReign.Create();               // Ara
		BuffCrewMates.Create();                // Blind, the Bound Demon
		CreateArmyOfSkeletons.Create();        // Blind, the Bound Demon
		Erratic.Create();                      // Bt Y#0895
		FlameStrafe.Create();                  // Cevin
		GiantStrike.Create();                  // Blind, the Bound Demon
		GiantStrikeEnraged.Create();           // Blind, the Bound Demon
		GrimoraRandomAbility.Create();         // vanilla
		Haunter.Create();                      // vanilla
		HookLineAndSinker.Create();            // Bt Y#0895
		InvertedStrike.Create();               // Bt Y#0895
		LitFuse.Create();
		Possessive.Create();   // Bt Y#0895
		Raider.Create();       // Blind, the Bound Demon
		SkinCrawler.Create();  // Bt Y#0895
		SpiritBearer.Create(); // Bt Y#0895

		#region Special

		CreateRoyalsCrewMate.Create();
		GainAttackBones.Create(); // vanilla
		GrimoraGiant.Create();
		LammergeierAttack.Create(); // vanilla

		#endregion
	}

	private void LoadCards()
	{
		Log.LogDebug($"Loading cards");

		CardBuilder.Builder
			.SetAbilities(Ability.SteelTrap)
			.SetBaseAttackAndHealth(0, 1)
			.SetNames($"{GUID}_TrapTest", "Trap Test", "Trap".GetCardInfo().portraitTex)
			.Build();

		#region Normal

		Add_Banshee();                  // vanilla
		Add_BonePrince();               // Cevin2006™ (◕‿◕)#7971
		Add_Bonehound();                // vanilla
		Add_Bonelord();                 // Ryan S Art
		Add_Bonepile();                 // Bt Y#0895
		Add_BooHag();                   // Bt Y#0895
		Add_Catacomb();                 // Bt Y#0895
		Add_Centurion();                // Bt Y#0895
		Add_CompoundFracture();         // Bt Y#0895
		Add_Dalgyal();                  // Bt Y#0895
		Add_DanseMacabre();             // Bt Y#0895
		Add_DeadHand();                 // Ara?
		Add_Deadeye();                  // Bt Y#0895
		Add_Draugr();                   // Bt Y#0895
		Add_DrownedSoul();              // Bt Y#0895
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
		Add_WillOTheWisp();             // Bt Y#0895
		Add_Zombie();                   // Bt Y#0895

		#endregion


		#region Rares

		Add_Apparition();           // vanilla
		Add_BonelordsHorn();    // Cevin2006™ (◕‿◕)#7971
		Add_DeadPets();         // LavaErrorDoggo#1564
		Add_DeathKnell();       // Bt Y#0895
		Add_DeathKnellBell();   // Bt Y#0895
		Add_Dybbuk();           // Bt Y#0895
		Add_EmberSpirit();      // Cevin2006™ (◕‿◕)#7971
		Add_GhostShipRoyal();   // Cevin2006™ (◕‿◕)#7971
		Add_HeadlessHorseman(); // Cevin2006™ (◕‿◕)#7971
		Add_Hydra();            // Cevin2006™ (◕‿◕)#7971
		Add_Necromancer();      // Bt Y#0895
		Add_Ripper();           // Bt Y#0895
		Add_ScreamingSkull();   // Bt Y#0895
		Add_Silbon();           // Bt Y#0895
		Add_SporeDigger();      // LavaErrorDoggo#1564
		Add_Wyvern();           // Cevin2006™ (◕‿◕)#7971

		#endregion

		AllGrimoraModCards.Sort((info, cardInfo) => string.Compare(info.name, cardInfo.name, StringComparison.Ordinal));
		
		// change just the artwork of Starvation
		CardInfo card = CardManager.BaseGameCards.CardByName("Starvation");
		card.portraitTex = AllSprites.Single(sp => sp.name.Equals("Starvation"));
		card.portraitTex.RegisterEmissionForSprite(AllSprites.Single(sp => sp.name.Equals("Starvation_emission")));
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
		GrimoraModBattleSequencer.ActiveEnemyPiece = null;
		_harmony?.UnpatchSelf();
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

	private IEnumerator LoadAssetsAsync()
	{
		Log.LogInfo($"Loading asset bundles");

		yield return StartCoroutine(AssetUtils.LoadAssetBundleAsync<GameObject>("grimoramod_prefabs"));

		yield return StartCoroutine(AssetUtils.LoadAssetBundleAsync<AudioClip>("grimoramod_sounds"));

		yield return AssetUtils.LoadAssetBundleAsync<RuntimeAnimatorController>("grimoramod_controller");

		yield return AssetUtils.LoadAssetBundleAsync<Material>("grimoramod_mats");

		yield return AssetUtils.LoadAssetBundleAsync<Texture>("grimoramod_abilities");
	}
}
