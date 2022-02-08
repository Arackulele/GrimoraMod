global using Object = UnityEngine.Object;
using System.Reflection;
using APIPlugin;
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
	private const string Version = "2.5.0";

	internal static ManualLogSource Log;

	private static Harmony _harmony;

	public static GameObject[] AllPrefabs;
	public static Sprite[] AllSprites;
	public static Texture[] AllAbilityTextures;
	public static Material[] AllMats;


	private static readonly List<StoryEvent> StoryEventsToBeCompleteBeforeStarting = new()
	{
		StoryEvent.BasicTutorialCompleted, StoryEvent.TutorialRunCompleted, StoryEvent.BonesTutorialCompleted,
		StoryEvent.TutorialRun2Completed, StoryEvent.TutorialRun3Completed
	};


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
				particle.transform.position.z);
		}

		Destroy(gameObject, 6f);
	}

	private void Awake()
	{
		Log = Logger;

		_harmony = Harmony.CreateAndPatchAll(Assembly.GetExecutingAssembly(), GUID);

		ConfigHelper.Instance.BindConfig();

		LoadAssets();

		UnlockAllNecessaryEventsToPlay();

		#region AddingAbilities

		AlternatingStrike.Create(); // Bt Y#0895
		AreaOfEffectStrike.Create(); // Bt Y#0895
		Erratic.Create(); // Bt Y#0895
		InvertedStrike.Create(); // Bt Y#0895
		Possessive.Create(); // Bt Y#0895
		SkinCrawler.Create(); // Bt Y#0895

		BoneLordsReign.Create();
		FlameStrafe.Create();
		PayBonesForSkeleton.Create();
		PayEnergyForWyvern.Create();

		#endregion

		#region AddingCards

		Add_ArmoredZombie();
		Add_Bonepile(); // Bt Y#0895		
		Add_BonePrince();
		Add_Bonelord();
		Add_BonelordsHorn();
		Add_BooHag(); // Bt Y#0895
		Add_DanseMacabre(); // Bt Y#0895
		Add_DeadHand();
		Add_DeadPets();
		Add_Draugr();
		Add_DrownedSoul();
		Add_Dybbuk(); // Bt Y#0895
		Add_Ember_spirit();
		Add_Family();
		Add_Flames();
		Add_Franknstein();
		Add_Giant(); // Bt Y#0895
		Add_GhostShip();
		Add_GraveDigger();
		Add_HeadlessHorseman();
		Add_Hydra();
		Add_Mummy();
		Add_Necromancer();
		Add_Obol(); // Bt Y#0895
		Add_PlagueDoctor();
		Add_Poltergeist();
		Add_Project(); // Bt Y#0895
		Add_Revenant();
		Add_RingWorm();
		Add_Ripper(); // Bt Y#0895
		Add_Sarcophagus();
		Add_Silbon(); // Bt Y#0895
		Add_ScreamingSkull(); // Bt Y#0895
		Add_Skelemancer();
		Add_SkeletonArmy();
		Add_SkeletonMage();
		Add_SporeDigger();
		Add_Summoner();
		Add_TombRobber();
		Add_Wendigo();
		Add_Wyvern();
		Add_ZombieGeck();
		Add_Zombie();

		#endregion

		ResizeArtworkForVanillaBoneCards();
		
		if(ConfigHelper.Instance.isHotReloadEnabled)
		{
			GameObject cardRow = GameObject.Find("CardRow");
			if (cardRow is not null && cardRow.transform.Find("MenuCard_Grimora") is null)
			{
				StartScreenThemeSetterPatches.AddGrimoraModMenuCardButton(FindObjectOfType<StartScreenThemeSetter>());
			}
		}

		ConfigHelper.Instance.HandleHotReloadAfter();
	}

	private void OnDestroy()
	{
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
	}

	private static void ResizeArtworkForVanillaBoneCards()
	{
		List<string> cardsToResizeArtwork = new List<string>
		{
			"Amoeba", "Maggots"
		};

		foreach (var cardName in cardsToResizeArtwork)
		{
			CardInfo cardInfo = CardLoader.GetCardByName(cardName);
			CardBuilder builder = CardBuilder.Builder
				.SetAsNormalCard()
				.SetAbilities(cardInfo.abilities.ToArray())
				.SetBaseAttackAndHealth(cardInfo.baseAttack, cardInfo.baseHealth)
				.SetBoneCost(cardInfo.bonesCost)
				.SetDescription(cardInfo.description)
				.SetNames("GrimoraMod_" + cardInfo.name, cardInfo.displayedName)
				.SetTribes(cardInfo.tribes.ToArray());

			if (cardName == "Amoeba")
			{
				builder.SetAsRareCard();
			}

			NewCard.Add(builder.Build());
		}
	}


	private static void LoadAssets()
	{
		FileUtils.CheckIfDirectoriesNeededExist();

		Log.LogDebug($"Loading asset bundles");

		AssetBundle abilityBundle = AssetBundle.LoadFromFile(FileUtils.FindFileInPluginDir("grimoramod_abilities"));
		AssetBundle matsBundle = AssetBundle.LoadFromFile(FileUtils.FindFileInPluginDir("grimoramod_mats"));
		AssetBundle spritesBundle = AssetBundle.LoadFromFile(FileUtils.FindFileInPluginDir("grimoramod_sprites"));
		AssetBundle prefabsBundle = AssetBundle.LoadFromFile(FileUtils.FindFileInPluginDir("grimoramod_prefabs"));

		
		// BundlePrefab = AssetBundle.LoadFromFile(FileUtils.FindFileInPluginDir("prefab-testing"));

		Log.LogDebug($"Loading assets into static vars");
		AllAbilityTextures = abilityBundle.LoadAllAssets<Texture>();
		abilityBundle.Unload(false);

		AllMats = matsBundle.LoadAllAssets<Material>();
		matsBundle.Unload(false);
		
		AllPrefabs = prefabsBundle.LoadAllAssets<GameObject>();
		Log.LogDebug($"{string.Join(",", AllPrefabs.Select(_ => _.name))}");
		prefabsBundle.Unload(false);

		AllSprites = spritesBundle.LoadAllAssets<Sprite>();
		spritesBundle.Unload(false);
	}

	private static void UnlockAllNecessaryEventsToPlay()
	{
		if (!StoryEventsToBeCompleteBeforeStarting.TrueForAll(StoryEventsData.EventCompleted))
		{
			Log.LogWarning($"You haven't completed a required event... Starting unlock process");
			StoryEventsToBeCompleteBeforeStarting.ForEach(evt => StoryEventsData.SetEventCompleted(evt));
			ProgressionData.UnlockAll();
			SaveManager.SaveToFile();
		}
	}
}
