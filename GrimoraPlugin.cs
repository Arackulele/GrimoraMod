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
[BepInPlugin(PluginGuid, PluginName, PluginVersion)]
public partial class GrimoraPlugin : BaseUnityPlugin
{
	public const string PluginGuid = "arackulele.inscryption.grimoramod";
	public const string PluginName = "GrimoraMod";
	private const string PluginVersion = "2.4.1";

	internal static ManualLogSource Log;

	private static Harmony _harmony;

	public static AssetBundle BundlePrefab;
	public static UnityEngine.Object[] AllAssets;
	public static UnityEngine.Sprite[] AllSpriteAssets;
	public static UnityEngine.Texture[] AllAbilityAssets;


	private static readonly List<StoryEvent> StoryEventsToBeCompleteBeforeStarting = new()
	{
		StoryEvent.BasicTutorialCompleted, StoryEvent.TutorialRunCompleted, StoryEvent.BonesTutorialCompleted,
		StoryEvent.TutorialRun2Completed, StoryEvent.TutorialRun3Completed
	};


	private void Awake()
	{
		Log = base.Logger;

		_harmony = Harmony.CreateAndPatchAll(Assembly.GetExecutingAssembly(), PluginGuid);

		LoadAssets();

		ConfigHelper.Instance.BindConfig();

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

		AddAra_ArmoredZombie();
		AddAra_Bonepile(); // Bt Y#0895		
		AddAra_BonePrince();
		AddAra_Bonelord();
		AddAra_BonelordsHorn();
		AddAra_BooHag(); // Bt Y#0895
		AddAra_DanseMacabre(); // Bt Y#0895
		AddAra_DeadHand();
		AddAra_DeadPets();
		AddAra_Draugr();
		AddAra_DrownedSoul();
		AddAra_Dybbuk(); // Bt Y#0895
		AddAra_Ember_spirit();
		AddAra_Family();
		AddAra_Flames();
		AddAra_Franknstein();
		AddAra_Giant(); // Bt Y#0895
		AddAra_GhostShip();
		AddAra_GraveDigger();
		AddAra_HeadlessHorseman();
		AddAra_Hydra();
		AddAra_Mummy();
		AddAra_Necromancer();
		AddAra_Obol(); // Bt Y#0895
		AddAra_PlagueDoctor();
		AddAra_Poltergeist();
		AddAra_Project(); // Bt Y#0895
		AddAra_Revenant();
		AddAra_RingWorm();
		AddAra_Ripper(); // Bt Y#0895
		AddAra_Sarcophagus();
		AddAra_Silbon(); // Bt Y#0895
		AddAra_ScreamingSkull(); // Bt Y#0895
		AddAra_Skelemancer();
		AddAra_SkeletonArmy();
		AddAra_SkeletonMage();
		AddAra_SporeDigger();
		AddAra_Summoner();
		AddAra_TombRobber();
		AddAra_Wendigo();
		AddAra_Wyvern();
		AddAra_ZombieGeck();
		AddAra_Zombie();

		#endregion

		ResizeArtworkForVanillaBoneCards();
	}

	private void OnDestroy()
	{
		_harmony?.UnpatchSelf();
	}

	private static void ResizeArtworkForVanillaBoneCards()
	{
		List<string> cardsToResizeArtwork = new List<string>
		{
			"Amoeba", "Maggots"
		};

		foreach (var cardName in cardsToResizeArtwork)
		{
			CardInfo cardInfo = CardLoader.Clone(CardLoader.GetCardByName(cardName));
			CardBuilder builder = CardBuilder.Builder
				.SetAsNormalCard()
				.SetAbilities(cardInfo.abilities)
				.SetBaseAttackAndHealth(cardInfo.baseAttack, cardInfo.baseHealth)
				.SetBoneCost(cardInfo.bonesCost)
				.SetDescription(cardInfo.description)
				.SetNames("ara_" + cardInfo.name, cardInfo.displayedName)
				.SetTribes(cardInfo.tribes);

			if (cardName == "Amoeba")
			{
				builder.SetAsRareCard();
			}

			NewCard.Add(builder.Build());
		}
	}


	private static void LoadAssets()
	{
		Log.LogDebug($"Loading asset bundles");

		AssetBundle abilityBundle = AssetBundle.LoadFromFile(FileUtils.FindFileInPluginDir("grimoramod_abilities"));
		AssetBundle blockerBundle = AssetBundle.LoadFromFile(FileUtils.FindFileInPluginDir("GrimoraMod_Prefabs_Blockers"));
		AssetBundle spritesBundle = AssetBundle.LoadFromFile(FileUtils.FindFileInPluginDir("grimoramod_sprites"));
		// BundlePrefab = AssetBundle.LoadFromFile(FileUtils.FindFileInPluginDir("grimoramod_prefabs"));

		// BundlePrefab = AssetBundle.LoadFromFile(FileUtils.FindFileInPluginDir("prefab-testing"));
		// Log.LogDebug($"{string.Join(",", BundlePrefab.GetAllAssetNames())}");

		AllAssets = blockerBundle.LoadAllAssets();
		blockerBundle.Unload(false);

		AllAbilityAssets = abilityBundle.LoadAllAssets<Texture>();
		abilityBundle.Unload(false);

		AllSpriteAssets = spritesBundle.LoadAllAssets<Sprite>();
		spritesBundle.Unload(false);

		// Log.LogDebug($"Abilities textures loaded {string.Join(",", AllAbilityAssets.Select(_ => _.name))}");
		// Log.LogDebug($"Sprites loaded {string.Join(",", AllSpriteAssets.Select(spr => spr.name))}");
		// try
		// {
		// }
		// catch (Exception e)
		// {
		// 	Log.LogWarning($"Asset bundles already exist");
		// }
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
