using BepInEx;
using BepInEx.Configuration;
using DiskCardGame;
using static GrimoraMod.GrimoraPlugin;

namespace GrimoraMod;

public class ConfigHelper
{
	private static ConfigHelper m_Instance;
	public static ConfigHelper Instance => m_Instance ??= new ConfigHelper();

	private readonly ConfigFile GrimoraConfigFile = new(
		Path.Combine(Paths.ConfigPath, "grimora_mod_config.cfg"),
		true
	);

	public const string StaticDefaultRemovedPiecesList =
		"BossFigurine," +
		"ChessboardChestPiece," +
		"EnemyPiece_Skelemagus,EnemyPiece_Gravedigger," +
		"Tombstone_North1," +
		"Tombstone_Wall1,Tombstone_Wall2,Tombstone_Wall3,Tombstone_Wall4,Tombstone_Wall5," +
		"Tombstone_South1,Tombstone_South2,Tombstone_South3,";

	private ConfigEntry<int> _configCurrentChessboardIndex;

	public int CurrentChessboardIndex
	{
		get => _configCurrentChessboardIndex.Value;
		set => _configCurrentChessboardIndex.Value = value;
	}

	private ConfigEntry<bool> _configKayceeFirstBossDead;

	public bool isKayceeDead => _configKayceeFirstBossDead.Value;

	private ConfigEntry<bool> _configSawyerSecondBossDead;

	public bool isSawyerDead => _configSawyerSecondBossDead.Value;

	private ConfigEntry<bool> _configRoyalThirdBossDead;

	public bool isRoyalDead => _configRoyalThirdBossDead.Value;

	private ConfigEntry<bool> _configGrimoraBossDead;

	public bool isGrimoraDead => _configGrimoraBossDead.Value;

	private ConfigEntry<bool> _configDeveloperMode;

	public bool isDevModeEnabled => _configDeveloperMode.Value;

	protected internal ConfigEntry<string> _configCurrentRemovedPieces;

	public List<string> RemovedPieces => _configCurrentRemovedPieces.Value.Split(',').Distinct().ToList();

	internal void BindConfig()
	{
		_configCurrentChessboardIndex
			= GrimoraConfigFile.Bind(PluginName, "Current chessboard layout index", 0);

		_configKayceeFirstBossDead
			= GrimoraConfigFile.Bind(PluginName, "Kaycee defeated?", false);

		_configSawyerSecondBossDead
			= GrimoraConfigFile.Bind(PluginName, "Sawyer defeated?", false);

		_configRoyalThirdBossDead
			= GrimoraConfigFile.Bind(PluginName, "Royal defeated?", false);

		_configGrimoraBossDead
			= GrimoraConfigFile.Bind(PluginName, "Grimora defeated?", false);

		_configCurrentRemovedPieces = GrimoraConfigFile.Bind(
			PluginName,
			"Current Removed Pieces",
			StaticDefaultRemovedPiecesList,
			new ConfigDescription("Contains all the current removed pieces." +
			                      "\nDo not alter this list unless you know what you are doing!")
		);

		_configDeveloperMode = GrimoraConfigFile.Bind(
			PluginName,
			"Enable Developer Mode",
			false,
			new ConfigDescription("Does not generate blocker pieces. Chests fill first row, enemy pieces fill first column.")
		);

		var list = _configCurrentRemovedPieces.Value.Split(',').ToList();
		// this is so that for whatever reason the game map gets added to the removal list,
		//	this will automatically remove those entries
		if (list.Contains("ChessboardGameMap"))
		{
			list.RemoveAll(piece => piece.Equals("ChessboardGameMap"));
		}

		_configCurrentRemovedPieces.Value = string.Join(",", list.Distinct());

		GrimoraConfigFile.SaveOnConfigSet = true;

		ResetConfigDataIfGrimoraHasNotReachedTable();
	}

	public static void ResetRun()
	{
		Log.LogDebug($"[ResetRun] Resetting run");

		Instance.ResetConfig();
		ResetDeck();
		StoryEventsData.EraseEvent(StoryEvent.GrimoraReachedTable);
		SaveManager.SaveToFile();

		LoadingScreenManager.LoadScene("finale_grimora");
	}

	public static void ResetDeck()
	{
		Log.LogWarning($"Resetting Grimora Deck Data");
		GrimoraSaveData.Data.Initialize();
	}

	internal void ResetConfig()
	{
		Log.LogWarning($"Resetting Grimora Mod config");
		_configKayceeFirstBossDead.Value = false;
		_configSawyerSecondBossDead.Value = false;
		_configRoyalThirdBossDead.Value = false;
		_configGrimoraBossDead.Value = false;
		_configCurrentRemovedPieces.Value = StaticDefaultRemovedPiecesList;
		_configCurrentChessboardIndex.Value = 0;
	}

	private static void ResetConfigDataIfGrimoraHasNotReachedTable()
	{
		if (!StoryEventsData.EventCompleted(StoryEvent.GrimoraReachedTable))
		{
			Log.LogWarning($"Grimora has not reached the table yet, resetting values to false again.");
			Instance.ResetConfig();
		}
	}

	public void AddPieceToRemovedPiecesConfig(string pieceName)
	{
		_configCurrentRemovedPieces.Value += "," + pieceName + ",";
	}
	
	public void SetBossDefeatedInConfig(BaseBossExt boss)
	{
		switch (boss)
		{
			case KayceeBossOpponent:
				_configKayceeFirstBossDead.Value = true;
				break;
			case SawyerBossOpponent:
				_configSawyerSecondBossDead.Value = true;
				break;
			case RoyalBossOpponentExt:
				_configRoyalThirdBossDead.Value = true;
				break;
			case GrimoraBossOpponentExt:
				_configGrimoraBossDead.Value = true;
				break;
		}

		var bossPiece = ChessboardMapExt.Instance.BossPiece;
		ChessboardMapExt.Instance.BossDefeated = true;
		Instance.AddPieceToRemovedPiecesConfig(bossPiece.name);
		Log.LogDebug($"[SetBossDefeatedInConfig] Boss {bossPiece} defeated.");
	}
}
