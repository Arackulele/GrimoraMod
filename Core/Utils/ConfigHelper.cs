using BepInEx;
using BepInEx.Configuration;
using DiskCardGame;
using HarmonyLib;
using InscryptionAPI.Card;
using static GrimoraMod.GrimoraPlugin;

namespace GrimoraMod;

public class ConfigHelper
{
	private static readonly List<StoryEvent> StoryEventsToBeCompleteBeforeStarting = new()
	{
		StoryEvent.BasicTutorialCompleted, StoryEvent.TutorialRunCompleted, StoryEvent.BonesTutorialCompleted,
		StoryEvent.TutorialRun2Completed, StoryEvent.TutorialRun3Completed
	};
	
	public const string P03ModGuid = "zorro.inscryption.infiniscryption.p03kayceerun";

	public const string PackManagerGuid = "zorro.inscryption.infiniscryption.packmanager";

	private static ConfigHelper m_Instance;
	public static ConfigHelper Instance => m_Instance ??= new ConfigHelper();

	public static bool HasIncreaseSlotsMod => Harmony.HasAnyPatches("julianperge.inscryption.act1.increaseCardSlots");
	public static bool HasP03Mod => Harmony.HasAnyPatches(P03ModGuid);
	
	private readonly ConfigFile GrimoraConfigFile = new(
		Path.Combine(Paths.ConfigPath, "grimora_mod_config.cfg"),
		true
	);

	public const string DefaultRemovedPieces =
		"BossFigurine,"
	+ "ChessboardChestPiece,"
	+ "EnemyPiece_Skelemagus,EnemyPiece_Gravedigger,"
	+ "Tombstone_North1,"
	+ "Tombstone_Wall1,Tombstone_Wall2,Tombstone_Wall3,Tombstone_Wall4,Tombstone_Wall5,"
	+ "Tombstone_South1,Tombstone_South2,Tombstone_South3,";

	private ConfigEntry<bool> _configEndlessMode;

	public bool IsEndlessModeEnabled => _configEndlessMode.Value;

	private ConfigEntry<bool> _configCardsLeftInDeck;
	public bool EnableCardsLeftInDeckView => _configCardsLeftInDeck.Value;

	private ConfigEntry<bool> _configMotionSickness;
	public bool DisableMotionSicknessEffects => _configMotionSickness.Value;

	private ConfigEntry<bool> _configDeveloperMode;

	public bool IsDevModeEnabled => _configDeveloperMode.Value;

	private ConfigEntry<bool> _configHotReloadEnabled;

	public bool IsHotReloadEnabled => _configHotReloadEnabled.Value;

	private ConfigEntry<int> _configEncounterBlueprintType;

	public BlueprintTypeForEncounter EncounterBlueprintType => (BlueprintTypeForEncounter)Enum.GetValues(typeof(BlueprintTypeForEncounter)).GetValue(_configEncounterBlueprintType.Value);


	private ConfigEntry<int> _configInputConfig;
	public int InputType => _configInputConfig.Value;

	private ConfigEntry<int> _configElectricChairBurnRateType;
	public int ElectricChairBurnRateType => _configElectricChairBurnRateType.Value;

	private ConfigEntry<int> _configHardSavedValues;
	public int ConfigHardSave
	{
		get => _configHardSavedValues.Value;
		set => _configHardSavedValues.Value = value;
	}

	public void SetSkullStormDefeated()
	{

		//this is just a sequence of numbers i got by generating a random number sequence with the seed set as 'bonelord'
		ConfigHardSave = 44731;

	}

		internal void BindConfig()
	{
		Log.LogDebug($"Binding config");

		_configCardsLeftInDeck = GrimoraConfigFile.Bind(
			Name,
			"Enable showing list of cards left in deck during battles",
			true,
			new ConfigDescription("This option will allow you to see what cards are left in your deck.")
		);

		_configMotionSickness = GrimoraConfigFile.Bind(
	Name,
	"Disable Motion Sickness Inducing effects",
	false,
	new ConfigDescription("This option will disable all effects that might cause motion sickness and other visually jarring effects.")
);

		_configDeveloperMode = GrimoraConfigFile.Bind(
			Name,
			"Enable Developer Mode",
			false,
			new ConfigDescription("Does not generate blocker pieces. Chests fill first row, enemy pieces fill first column.")
		);

		_configHotReloadEnabled = GrimoraConfigFile.Bind(
			Name,
			"Enable Hot Reload",
			false,
			new ConfigDescription(
				"If the dll is placed in BepInEx/scripts, this will allow running certain commands that should only ever be ran to re-add abilities/cards back in the game correctly."
			)
		);

		_configEndlessMode = GrimoraConfigFile.Bind(
			Name,
			"Enable Endless Mode",
			false,
			new ConfigDescription("For players who want to continue playing with their deck after defeating Grimora.")
		);

		_configEncounterBlueprintType = GrimoraConfigFile.Bind(
			Name,
			"Encounter Blueprints",
			0,
			new ConfigDescription(
				"0 = Default. Use the mod's internal blueprint system."
			+ "\n1 = Randomized. Encounters are completely randomized using the mod's internal card pool."
			+ "\n2 = Custom. Encounters are from made and used from the JSON files."
			+ "\n3 = Mixed. Encounters are from both default list and custom list."
			)
		);

		_configInputConfig = GrimoraConfigFile.Bind(
			Name,
			"Input Movement Type",
			0,
			"0 = W for viewing deck, S for getting up from the table."
		+ "\n1 = Up arrow for viewing deck, down arrow for getting up from the table."
		);

		_configElectricChairBurnRateType = GrimoraConfigFile.Bind(
			Name,
			"Electric Chair Burn Rate",
			0,
			"0 = Default. Flat 50% burn rate for each lever option on second shock."
		+ "\n1 = Base 30% chance to burn plus 0%, 10%, or 20% for low, medium, high, respectively. Meaning, if the first shock is high then the second is high, the chance for the card to be destroyed is 70%."
		+ "\n2 = Low: 12.5%, Medium: 17.5%, High 30%. Meaning, if the first shock is high, then the second one is also high, the chance for the card to be destroyed is 60%."
		+ "\n3 = Low: 12.5%, Medium: 20%, High 27.5%. Meaning, if the first shock is high, then the second one is also high, the chance for the card to be destroyed is 55%."
		);

		_configHardSavedValues = GrimoraConfigFile.Bind(
		Name,
		"Hard Saved Values",
		00000,
		new ConfigDescription(
			"Dont change this value, it is used by the game to store certain events."
		)
);

		GrimoraConfigFile.SaveOnConfigSet = true;
	}

	public void HandleHotReloadBefore()
	{
		if (!_configHotReloadEnabled.Value)
		{
			return;
		}

		if (CardLoader.allData.IsNotEmpty())
		{
			CardManager.AllCardsCopy.RemoveAll(info => info.name.StartsWith($"{GUID}_"));
			int removedCardLoader = CardLoader.allData.RemoveAll(info => info.name.StartsWith($"{GUID}_"));
			Log.LogDebug($"All data. Removed [{removedCardLoader}] CardLoader");
		}

		if (AbilitiesUtil.allData.IsNotEmpty())
		{
			SpecialTriggeredAbilityManager.AllSpecialTriggers.Clear();
			int removed = AbilitiesUtil.allData.RemoveAll(
				info => AbilityManager.AllAbilities.Exists(na => na.Info.rulebookName == info.rulebookName)
			);
			AbilityManager.AllAbilities.Clear();
			Log.LogDebug($"All data Removed [{removed}] AbilitiesUtil");
		}
	}

	private static void UnlockAllNecessaryEventsToPlay()
	{
		if (!StoryEventsToBeCompleteBeforeStarting.TrueForAll(StoryEventsData.EventCompleted))
		{
			Log.LogWarning($"You haven't completed a required event... Starting unlock process");
			StoryEventsToBeCompleteBeforeStarting.ForEach(evt => StoryEventsData.SetEventCompleted(evt));
			try
			{
				ProgressionData.UnlockAll();
			}
			catch (Exception e)
			{
				Log.LogError(
					"Failed to unlock all necessary mechanics with [ProgressionData.UnlockAll]. "
				+ "There's something wrong with your save file or your computer reading data/files. "
				+ "This should not throw an exception and I have no idea how to fix this for you."
				+ "If the combat bell doesn't show up, restart your game."
				);
			}

			SaveManager.SaveToFile();
		}
	}
}
