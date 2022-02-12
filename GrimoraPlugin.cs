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
	private const string Version = "2.5.3";

	internal static ManualLogSource Log;

	private static Harmony _harmony;

	public static List<GameObject> AllPrefabs;
	public static List<Material> AllMats;
	public static List<RuntimeAnimatorController> AllControllers;
	public static List<Sprite> AllSprites;
	public static List<Texture> AllAbilityTextures;

	// Gets populated in CardBuilder.Build()
	public static List<CardInfo> AllGrimoraModCards = new();

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
		Add_VengefulSpirit();
		Add_SkeletonArmy();
		Add_Skelemagus();
		Add_SporeDigger();
		Add_Summoner();
		Add_TombRobber();
		Add_Wendigo();
		Add_Wyvern();
		Add_ZombieGeck();
		Add_Zombie();

		#endregion

		ResizeArtworkForVanillaBoneCards();

		if (ConfigHelper.Instance.isHotReloadEnabled)
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
		AllAbilityTextures = null;
		AllMats = null;
		AllPrefabs = null;
		AllSprites = null;
		AllGrimoraModCards = new List<CardInfo>();
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
			CardInfo cardInfo = cardName.GetCardInfo();
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

		Log.LogDebug($"Loading assets into static vars");
		AllAbilityTextures = AssetUtils.LoadAssetBundle<Texture>("grimoramod_abilities");

		AllMats = AssetUtils.LoadAssetBundle<Material>("grimoramod_mats");

		AllPrefabs = AssetUtils.LoadAssetBundle<GameObject>("grimoramod_prefabs");

		AllSprites = AssetUtils.LoadAssetBundle<Sprite>("grimoramod_sprites");
		
		AllControllers = AssetUtils.LoadAssetBundle<RuntimeAnimatorController>("grimoramod_controller");
	}
}
