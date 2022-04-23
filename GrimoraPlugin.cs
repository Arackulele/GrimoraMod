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
	private const string Version = "2.8.7";

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

	private void Awake()
	{
		Log = Logger;

		_harmony = Harmony.CreateAndPatchAll(Assembly.GetExecutingAssembly(), GUID);

		ConfigHelper.Instance.BindConfig();

		AllAbilitiesTextures = AssetUtils.LoadAssetBundle<Texture>("grimoramod_abilities");
		AllControllers = AssetUtils.LoadAssetBundle<RuntimeAnimatorController>("grimoramod_controller");
		AllMats = AssetUtils.LoadAssetBundle<Material>("grimoramod_mats");
		AllSounds = AssetUtils.LoadAssetBundle<AudioClip>("grimoramod_sounds");
		AllSprites = AssetUtils.LoadAssetBundle<Sprite>("grimoramod_sprites");
		AllMesh = AssetUtils.LoadAssetBundle<Mesh>("grimoramod_mesh");

		StartCoroutine(LoadAssetsAsync());
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
		LoadAbilitiesAndCards();

		if (AllPrefabs.IsNullOrEmpty())
		{
			yield return new WaitUntil(() => !AllPrefabs.IsNullOrEmpty());
		}

		//extra cards dll doesnt actually do anything, as its probably more convenient for us to have everything in the main project
		if (Chainloader.PluginInfos.ContainsKey("arackulele.inscryption._grimoramodextracards"))
				{
			Log.LogDebug($"Detected expansion mod");
			Add_Card_BalBal();
			Add_Card_BloodySack();
			Add_Card_BoneCollective();
			Add_Card_CompoundFracture();
			Add_Card_DisturbedGrave();
			Add_Card_EmberSpirit();
			Add_Card_FesteringWretch();
			Add_Card_Flameskull();
			Add_Card_Haltia();
			Add_Card_HauntedMirror();
			Add_Card_IceCube();
			Add_Card_LaLlorona();
			Add_Card_Moroi();
			Add_Card_Nixie();
			Add_Card_Nosferat();
			Add_Card_PiratePolly();
			Add_Card_PiratePrivateer();
			Add_Card_PossessedArmour();
			Add_Card_Rot();
			Add_Card_Warthr();

			Add_Card_Fylgja();
			Add_Card_Wyvern();
			Add_Card_Wight();
			Add_Card_SkeletonArmy();
				}
		}

	private void LoadAbilitiesAndCards()
	{
		Stopwatch stopwatch = Stopwatch.StartNew();
		Log.LogDebug($"[LoadAbilitiesAndCards] Loading cards...");

		// Sort and add all abilities, since cards are split its faster to just add them manually than to save a few lines of code with a blacklist (also its nice for organization)

		var allAddMethods = AccessTools.GetDeclaredMethods(typeof(GrimoraPlugin))
		 .Where(mi => mi.Name.StartsWith("Add_Ability"))
		 .ToList();
		allAddMethods.Sort((mi, mi2) => string.Compare(mi.Name, mi2.Name, StringComparison.Ordinal));
		allAddMethods.ForEach(mi => mi.Invoke(this, null));

		//Add Cards, this was automatic but i made it manual so i can more easily split the cards
		//Will add the individual Art Credits we had once later here, nice to have that organized so we know who did what
		#region Cards

		Add_Card_Animator();
		Add_Card_Apparition();
		Add_Card_Banshee();
		Add_Card_Bonehound();
		Add_Card_Bonelord();
		Add_Card_BonelordsHorn();
		Add_Card_Bonepile();
		Add_Card_BonePrince();
		Add_Card_BooHag();
		Add_Card_CalaveraCatrina();
		Add_Card_Catacomb();
		Add_Card_Centurion();
		Add_Card_Dalgyal();
		Add_Card_DanseMacabre();
		Add_Card_Deadeye();
		Add_Card_DeadHand();
		Add_Card_DeadPets();
		Add_Card_DeadTree();
		Add_Card_DeathKnell();
		Add_Card_Doll();
		Add_Card_Draugr();
		Add_Card_DrownedSoul();
		Add_Card_Dybbuk();
		Add_Card_Family();
		Add_Card_Flames();
		Add_Card_ForgottenMan();
		Add_Card_Franknstein();
		Add_Card_Gashadokuro_Stage1_MassGrave();
		Add_Card_Gashadokuro_Stage2_RisingHunger();
		Add_Card_Gashadokuro_Stage3();
		Add_Card_GhostShip();
		Add_Card_GhostShipRoyal();
		Add_Card_Giant();
		Add_Card_Glacier_Stage1();
		Add_Card_Glacier_Stage2_FrostGiant();
		Add_Card_Gravebard();
		Add_Card_Gravedigger();
		Add_Card_Haltia();
		Add_Card_HeadlessHorseman();
		Add_Card_Hellhand();
		Add_Card_HellHound();
		Add_Card_Hydra();
		Add_Card_Jikininki();
		Add_Card_Kennel();
		Add_Card_Manananggal();
		Add_Card_Mummy();
		Add_Card_Necromancer();
		Add_Card_Obol();
		Add_Card_PirateCaptainYellowbeard();
		Add_Card_PirateExploding();
		Add_Card_PirateFirstMateSnag();
		Add_Card_PirateSwashbuckler();
		Add_Card_PlagueDoctor();
		Add_Card_Poltergeist();
		Add_Card_Project();
		Add_Card_Revenant();
		Add_Card_Ripper();
		Add_Card_RotTail();
		Add_Card_Sarcophagus();
		Add_Card_ScreamingSkull();
		Add_Card_Shipwreck();
		Add_Card_Shipwreck_Dams();
		Add_Card_Silbon();

		Add_Card_Skelemagus();
		Add_Card_Sluagh();
		Add_Card_Spectrabbit();
		Add_Card_Sporedigger();
		Add_Card_StarvedMan();
		Add_Card_Summoner();
		Add_Card_TamperedCoffin();
		Add_Card_TombRobber();
		Add_Card_Vampire();
		Add_Card_Vellum();
		Add_Card_VengefulSpirit();
		Add_Card_WardingPresence();
		Add_Card_Wechuge();
		Add_Card_WillOTheWisp();
		Add_Card_Writher();
		Add_Card_WritherTail();
		Add_Card_Zombie();
				#endregion
				// Log.LogInfo($"Adding [{allAddMethods.Count(mi => mi.Name.Contains("_Card_"))}] cards.");

				AllGrimoraModCards.Sort((info, cardInfo) => string.Compare(info.name, cardInfo.name, StringComparison.Ordinal));
		AllPlayableGrimoraModCards = AllGrimoraModCards.Where(info => info.metaCategories.Any()).ToList();

		// change just the artwork of Starvation
		CardInfo card = CardManager.BaseGameCards.CardByName("Starvation");
		card.portraitTex = AllSprites.Single(sp => sp.name.Equals("Starvation"));
		card.portraitTex.RegisterEmissionForSprite(AllSprites.Single(sp => sp.name.Equals("Starvation_emission")));

		CardBuilder.Builder
		 .SetAbilities(Ability.BoneDigger, Ability.SteelTrap, Haunter.ability)
		 .SetBaseAttackAndHealth(0, 1)
		 .SetNames($"{GUID}_!TRAP", "!TEST Trap", "Trap".GetCardInfo().portraitTex)
		 .Build();

		CardBuilder.Builder
		 .SetAbilities(Ability.DeathShield)
		 .SetBaseAttackAndHealth(0, 99)
		 .SetNames($"{GUID}_!BLOCKER", "!TEST Blocker", "Trap".GetCardInfo().portraitTex)
		 .Build();
		stopwatch.Stop();
		Log.LogDebug($"[LoadAbilitiesAndCards] Finished loading all abilities and cards in [{stopwatch.ElapsedMilliseconds}]ms");
	}

	private void OnDestroy()
	{
		AllAbilitiesTextures = null;
		AllControllers = null;
		AllMats = null;
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

	private IEnumerator LoadAssetsAsync()
	{
		Log.LogInfo($"Loading asset bundles");

		yield return StartCoroutine(AssetUtils.LoadAssetBundleAsync<GameObject>("grimoramod_prefabs"));
	}
}
