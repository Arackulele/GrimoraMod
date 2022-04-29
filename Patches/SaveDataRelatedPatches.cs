using DiskCardGame;
using HarmonyLib;
using InscryptionAPI.Ascension;
using InscryptionAPI.Saves;
using static GrimoraMod.GrimoraPlugin;

namespace GrimoraMod;

[HarmonyPatch]
public class SaveDataRelatedPatches
{
	public const string AscensionSaveKey = "CopyOfGrimoraAscensionSave";
	public const string RegularSaveKey = "CopyOfGrimoraSave";
	public const string IsGrimoraRunKey = "IsGrimoraRun";

	public static string SaveKey
	{
		get
		{
			if (SceneLoader.ActiveSceneName == "Ascension_Configure")
				return AscensionSaveKey;

			if (SceneLoader.ActiveSceneName == SceneLoader.StartSceneName)
				return RegularSaveKey;

			return SaveFile.IsAscension ? AscensionSaveKey : RegularSaveKey;
		}
	}

	public static bool IsNotGrimoraRun => !IsGrimoraRun;

	public static bool IsGrimoraRun
	{
		get
		{
			string activeSceneName = SceneLoader.ActiveSceneName.ToLowerInvariant();
			// Log.LogDebug($"[IsGrimoraRun] Active scene name is [{activeSceneName}] Screen State [{ScreenManagement.ScreenState}]");
			if (activeSceneName.Contains("grimora") || ScreenManagement.ScreenState == CardTemple.Undead)
				return true;

			return AscensionSaveData.Data is { currentRun.playerLives: > 0 } && ModdedSaveManager.SaveData.GetValueAsBoolean(GUID, IsGrimoraRunKey);
		}
		set => ModdedSaveManager.SaveData.SetValue(GUID, IsGrimoraRunKey, value);
	}

	public static void EnsureRegularSave()
	{
		// The only way there is not a copy of the regular save is because you went straight to a grimora ascension run
		// after installing the mod. This means that the current grimoraSaveData is your actual Grimora save data
		// We don't want to lose that.
		if (ModdedSaveManager.SaveData.GetValue(GUID, RegularSaveKey) == default)
		{
			Log.LogInfo($"[GrimoraMod.EnsureRegularSave] Saving {RegularSaveKey} to json format...");
			ModdedSaveManager.SaveData.SetValue(GUID, RegularSaveKey, FileUtils.ToCompressedJSON(GrimoraSaveData.Data));
		}
	}

	public static void EnsureGrimoraSaved()
	{
		if (SaveFile.IsAscension)
		{
			// Check to see if there is a grimora save data yet
			EnsureRegularSave();
		}
	}

	[HarmonyPostfix, HarmonyPatch(typeof(RunState), nameof(RunState.Run), MethodType.Getter)]
	public static void RunIsNullForGrimora(ref RunState __result)
	{
		if (IsGrimoraRun)
		{
			if (__result == null)
			{
				Log.LogWarning($"[RunState.Run.Getter] RunState was null, creating new one");
			}
			__result ??= new RunState();
		}
	}

	[HarmonyPatch(typeof(SaveManager))]
	public class SaveManagerPatches
	{
		// Okay, I recognize that this is all kind of crazy.

		// Here's the problem: the game wants your Botopia save data to be in a specific place
		// I don't want you to lose your original Part 3 save. Plus, **I** really don't want to lose that save either!
		// Why? Because I want to be able to leave Kaycee's Mod and go explore original Botopia so I can
		// check out how it behaves, etc.

		// So what we do is we actually keep two Part3Save copies alive in the ModdedSaveFile, and we swap in whichever
		// one is necessary based on context (see the patch for LoadFromFile)

		// But whenever the file is saved, only the original part 3 save data gets saved in the normal spot
		// This fixes issues that arise when people unload the Grimora KCM mod.

		[HarmonyPrefix, HarmonyPatch(nameof(SaveManager.SaveToFile))]
		public static void Prefix(ref GrimoraSaveData __state)
		{
			// What this does is save a copy of the current part 3 save data somewhere else
			// The idea is that when you play part 3, every time you save we keep a copy of that data
			// And whenever you play ascension part 3, same thing.
			//
			// That way, if you switch over to the other type of part 3, we can load the last time this happened.
			// And whenever creating a new ascension part 3 run, we check to see if there is a copy of part 3 save yet
			// If not, we will end up creating one

			string compressedString = FileUtils.ToCompressedJSON(SaveManager.SaveFile.grimoraData);
			Log.LogInfo($"[GrimoraMod.SaveManager] Saving {SaveKey}, compressed string [{compressedString}]");
			ModdedSaveManager.SaveData.SetValue(GUID, SaveKey, compressedString);

			// Then, right before we actually save the data, we swap back in the original part3 data
			__state = SaveManager.SaveFile.grimoraData;

			EnsureGrimoraSaved();
			string originalGrimoraString = ModdedSaveManager.SaveData.GetValue(GUID, RegularSaveKey);
			GrimoraSaveData originalPart3Data = FileUtils.FromCompressedJSON<GrimoraSaveData>(originalGrimoraString);
			SaveManager.SaveFile.grimoraData = originalPart3Data;

			// SEE BELOW FOR WHAT HAPPENS NEXT: \/ \/ \/ 
		}

		[HarmonyPostfix, HarmonyPatch(nameof(SaveManager.SaveToFile))]
		public static void Postfix(GrimoraSaveData __state)
		{
			// Now that we've saved the file, we swap back whatever we had before
			SaveManager.SaveFile.grimoraData = __state;
		}

		[HarmonyPrefix, HarmonyPatch(nameof(SaveManager.TestSaveFileCorrupted))]
		public static void RepairMissingGrimoraData(SaveFile file)
		{
			if (file.grimoraData == null)
			{
				Log.LogWarning($"[SaveManager.TestSaveFileCorrupted] GrimoraSaveData is null, re-initializing");
				file.grimoraData = new GrimoraSaveData();
				file.grimoraData.Initialize();
			}
		}

		[HarmonyPostfix, HarmonyPatch(nameof(SaveManager.LoadFromFile))]
		[HarmonyAfter(InscryptionAPI.InscryptionAPIPlugin.ModGUID)]
		public static void LoadGrimoraAscensionSaveData()
		{
			string saveKey = SaveKey;
			string grimoraData = ModdedSaveManager.SaveData.GetValue(GUID, SaveKey);
			GrimoraSaveData data = FileUtils.FromCompressedJSON<GrimoraSaveData>(grimoraData);
			Log.LogDebug($"[SaveManager.LoadFromFile] Save key is [{saveKey}] GrimoraData [{grimoraData}] Data [{data}]");

			if (data == null)
			{
				Log.LogWarning($"[SaveManager.LoadFromFile] GrimoraSaveData is default/null, re-initializing.");
				data = new GrimoraSaveData();
				data.Initialize();
			}

			SaveManager.SaveFile.grimoraData = data;
		}
	}

	[HarmonyPatch(typeof(AscensionSaveData))]
	public class AscensionSaveDataPatches
	{
		[HarmonyPostfix, HarmonyPatch(nameof(AscensionSaveData.NewRun))]
		public static void InitializeGrimoraSave(ref AscensionSaveData __instance, List<CardInfo> starterDeck)
		{
			if (IsGrimoraRun)
			{
				if (__instance.currentStarterDeck.Equals("Vanilla", StringComparison.InvariantCultureIgnoreCase))
				{
					Log.LogDebug($"[AscensionSaveData.NewRun] Changing current starter deck to default");
					__instance.currentStarterDeck = StarterDecks.DefaultStarterDeck;
				}

				GrimoraSaveData data = new GrimoraSaveData();
				data.Initialize();
				SaveManager.SaveFile.grimoraData = data;
			}

			Log.LogInfo($"[AscensionSaveData.NewRun] CurrentRun [{__instance.currentRun}] player lives [{__instance.currentRun?.playerLives}]");
		}

		[HarmonyPrefix, HarmonyPatch(nameof(AscensionSaveData.EndRun))]
		public static void ClearGrimoraSaveOnEndRun(ref AscensionSaveData __instance)
		{
			if(__instance.currentRun.IsNotNull())
			{
				Log.LogInfo($"[AscensionSaveData.EndRun.Prefix] Clearing grimoraData");
				SaveManager.SaveFile.grimoraData = null;
				ModdedSaveManager.SaveData.SetValue(GUID, AscensionSaveKey, default(string));
			}
			
			Log.LogInfo($"[AscensionSaveData.EndRun.Prefix] CurrentRun [{__instance.currentRun}]");
		}
		
		[HarmonyPostfix, HarmonyPatch(nameof(AscensionSaveData.EndRun))]
		public static void CheckCurrentRun(ref AscensionSaveData __instance)
		{
			Log.LogInfo($"[AscensionSaveData.EndRun.Postfix] CurrentRun [{__instance.currentRun}]");
		}
	}

	[HarmonyPrefix, HarmonyPatch(typeof(GrimoraSaveData), nameof(GrimoraSaveData.Initialize))]
	public static bool PrefixChangeSetupOfGrimoraSaveData(ref GrimoraSaveData __instance)
	{
		// ModdedSaveManager.SaveData.SetValue(GUID, SaveKey, default(string));

		EnsureGrimoraSaved();

		__instance.gridX = -1;
		__instance.gridY = -1;
		__instance.removedPieces = new List<int>();
		__instance.deck = new DeckInfo();
		__instance.deck.Cards.Clear();

		if (Initialized)
		{
			Log.LogInfo($"[GrimoraSaveData.Initialize] Has been initialized. Is Ascension? [{SaveFile.IsAscension}] CurrentRun [{AscensionSaveData.Data.currentRun}]");
			if (SaveFile.IsAscension && AscensionSaveData.Data.currentRun.IsNotNull())
			{
				CreateAscensionDeck(__instance);
			}
			else
			{
				Log.LogInfo($"[GrimoraSaveData.Initialize] Creating GrimoraMod vanilla deck");
				List<CardInfo> defaultCardInfos = new()
				{
					NameBonepile.GetCardInfo(),
					NameGravedigger.GetCardInfo(),
					NameGravedigger.GetCardInfo(),
					NameFranknstein.GetCardInfo(),
					NameZombie.GetCardInfo()
				};

				foreach (var cardInfo in defaultCardInfos)
				{
					__instance.deck.AddCard(cardInfo);
				}
			}
		}
		else
		{
			CreateVanillaDeck(__instance);
		}

		return false;
	}

	private static void CreateVanillaDeck(GrimoraSaveData saveData)
	{
		Log.LogDebug($"[GrimoraSaveData.CreateVanillaDeck] Creating original vanilla game deck");
		saveData.deck.AddCard("Gravedigger".GetCardInfo());
		saveData.deck.AddCard("Gravedigger".GetCardInfo());
		saveData.deck.AddCard("Gravedigger".GetCardInfo());
		saveData.deck.AddCard("FrankNStein".GetCardInfo());
		saveData.deck.AddCard("FrankNStein".GetCardInfo());
	}

	private static void CreateAscensionDeck(GrimoraSaveData saveData)
	{
		saveData.deck = new DeckInfo();
		saveData.deck.Cards.Clear();

		Log.LogDebug(
			$"[GrimoraSaveData.CreateAscensionDeck] Creating ascension deck, current starter deck [{AscensionSaveData.Data.currentStarterDeck}] All DeckInfos [{StarterDeckManager.AllDeckInfos.Join(info => info.title)}]");
		StarterDeckInfo deckInfo = StarterDeckManager.AllDeckInfos.Find(deck => AscensionSaveData.Data.currentStarterDeck.EndsWith(deck.title));

		List<CardInfo> starterDeckCards = deckInfo.cards;

		foreach (CardInfo info in starterDeckCards)
		{
			saveData.deck.AddCard(info);
		}

		if (AscensionSaveData.Data.ChallengeIsActive(AscensionChallenge.WeakStarterDeck))
		{
			foreach (CardInfo info in saveData.deck.Cards)
			{
				info.mods ??= new List<CardModificationInfo>();
				info.mods.Add(new CardModificationInfo(Ability.BuffEnemy));
			}
		}

		Log.LogDebug($"[GrimoraSaveData.CreateAscensionDeck] Adding vellum");
		saveData.deck.AddCard(NameVellum.GetCardInfo());
		saveData.deck.AddCard(NameVellum.GetCardInfo());

		/*
		__instance.deck.AddCard(CardLoader.GetCardByName(CustomCards.UNC_TOKEN));
		__instance.deck.AddCard(CardLoader.GetCardByName(CustomCards.UNC_TOKEN));
		__instance.deck.AddCard(CardLoader.GetCardByName(CustomCards.UNC_TOKEN));
		__instance.deck.AddCard(CardLoader.GetCardByName(CustomCards.RARE_DRAFT_TOKEN));
		__instance.deck.AddCard(CardLoader.GetCardByName(CustomCards.RARE_DRAFT_TOKEN));
		__instance.deck.AddCard(CardLoader.GetCardByName(CustomCards.RARE_DRAFT_TOKEN));
		*/
	}

	// This keeps the oil painting puzzle from breaking the game
	[HarmonyPatch(typeof(OilPaintingPuzzle), nameof(OilPaintingPuzzle.GenerateSolution))]
	[HarmonyPrefix]
	public static bool ReplaceGenerateForGrimora(OilPaintingPuzzle __instance, ref List<string> __result)
	{
		if (IsNotGrimoraRun)
		{
			return true;
		}

		Log.LogDebug($"[OilPaintingPuzzle.GenerateSolution] Generating custom Oil Painting...");
		__result = new List<string> { null, null, NameBonelord, NameBonelord };
		return false;
	}
}
