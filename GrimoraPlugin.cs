using APIPlugin;
using BepInEx;
using BepInEx.Logging;
using DiskCardGame;
using HarmonyLib;
using UnityEngine;

namespace GrimoraMod;

[BepInDependency("cyantist.inscryption.api")]
[BepInPlugin(PluginGuid, PluginName, PluginVersion)]
public partial class GrimoraPlugin : BaseUnityPlugin
{
	public const string PluginGuid = "arackulele.inscryption.grimoramod";
	public const string PluginName = "GrimoraMod";
	private const string PluginVersion = "2.4.0";

	internal static ManualLogSource Log;

	private static Harmony _harmony;

	public static UnityEngine.Object[] AllAssets;
	public static UnityEngine.Sprite[] AllSpriteAssets;


	private static readonly List<StoryEvent> StoryEventsToBeCompleteBeforeStarting = new()
	{
		StoryEvent.BasicTutorialCompleted, StoryEvent.TutorialRunCompleted, StoryEvent.BonesTutorialCompleted,
		StoryEvent.TutorialRun2Completed, StoryEvent.TutorialRun3Completed
	};


	private void Awake()
	{
		Log = base.Logger;

		LoadAssets();

		ResetConfigDataIfGrimoraHasNotReachedTable();

		UnlockAllNecessaryEventsToPlay();

		_harmony = new Harmony(PluginGuid);
		_harmony.PatchAll();

		#region AddingAbilities

		FlameStrafe.CreateFlameStrafe();

		#endregion

		#region AddingCards

		AddAra_Bonepile();
		AddAra_BonePrince();
		AddAra_Bonelord();
		AddAra_BonelordsHorn();
		AddAra_BoneSerpent();
		AddAra_CrazedMantis();
		AddAra_DeadHand();
		AddAra_DeadPets();
		AddAra_Draugr();
		AddAra_DrownedSoul();
		AddAra_Ember_spirit();
		AddAra_Family();
		AddAra_Flames();
		AddAra_Franknstein();
		AddAra_GhostShip();
		AddAra_GraveDigger();
		AddAra_HeadlessHorseman();
		AddAra_Hydra();
		AddAra_Mummy();
		AddAra_Necromancer();
		AddAra_Obol();
		AddAra_Poltergeist();
		AddAra_Revenant();
		AddAra_RingWorm();
		AddAra_Sarcophagus();
		AddAra_Skelemancer();
		AddAra_Skelemaniac();
		AddAra_SkeletonArmy();
		AddAra_SkeletonMage();
		AddAra_BoneSnapper();
		AddAra_SporeDigger();
		AddAra_TombRobber();
		AddAra_UndeadWolf();
		AddAra_Wendigo();
		AddAra_Wyvern();
		AddAra_ZombieGeck();
		AddAra_Zombie();

		#endregion

		ResizeArtworkForVanillaBoneCards();

		ConfigHelper.Instance.BindConfig();
		ConfigHelper.Instance.GrimoraConfigFile.SaveOnConfigSet = true;

		// ChangePackRat();
		// ChangeSquirrel();
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

	private void OnDestroy()
	{
		_harmony?.UnpatchSelf();
	}


	private static void LoadAssets()
	{
		Log.LogDebug($"Loading asset bundles");
		string blockersFile = FileUtils.FindFileInPluginDir("GrimoraMod_Prefabs_Blockers");
		string spritesFile = FileUtils.FindFileInPluginDir("grimoramod_sprites");

		AssetBundle blockerBundle = AssetBundle.LoadFromFile(blockersFile);
		AssetBundle spritesBundle = AssetBundle.LoadFromFile(spritesFile);
		// Log.LogDebug($"Sprites bundle {string.Join(",", spritesBundle.GetAllAssetNames())}");

		AllAssets = blockerBundle.LoadAllAssets();
		AllSpriteAssets = spritesBundle.LoadAllAssets<Sprite>();
		// Log.LogDebug($"Sprites loaded {string.Join(",", AllSpriteAssets.Select(spr => spr.name))}");
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

	private static void ResetConfigDataIfGrimoraHasNotReachedTable()
	{
		if (!StoryEventsData.EventCompleted(StoryEvent.GrimoraReachedTable))
		{
			Log.LogWarning($"Grimora has not reached the table yet, resetting values to false again.");
			ConfigHelper.Instance.ResetConfig();
		}
	}


	public static void ResetDeck()
	{
		Log.LogWarning($"Resetting Grimora Deck Data");
		GrimoraSaveData.Data.Initialize();
	}
}
