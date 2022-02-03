using APIPlugin;
using BepInEx;
using BepInEx.Configuration;
using DiskCardGame;
using Sirenix.Utilities;
using UnityEngine;
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

	private ConfigEntry<int> _configBossesDefeated;
	public int BossesDefeated => _configBossesDefeated.Value;

	public bool isKayceeDead => BossesDefeated == 1;

	public bool isSawyerDead => BossesDefeated == 2;

	public bool isRoyalDead => BossesDefeated == 3;

	public bool isGrimoraDead => BossesDefeated == 4;

	private ConfigEntry<bool> _configDeveloperMode;

	public bool isDevModeEnabled => _configDeveloperMode.Value;

	private ConfigEntry<bool> _configHotReloadEnabled;

	public bool isHotReloadEnabled => _configHotReloadEnabled.Value;

	protected internal ConfigEntry<string> _configCurrentRemovedPieces;

	public List<string> RemovedPieces => _configCurrentRemovedPieces.Value.Split(',').Distinct().ToList();

	internal void BindConfig()
	{
		Log.LogDebug($"Binding config");

		_configCurrentChessboardIndex
			= GrimoraConfigFile.Bind(PluginName, "Current chessboard layout index", 0);

		_configBossesDefeated
			= GrimoraConfigFile.Bind(PluginName, "Number of bosses defeated", 0);

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

		_configHotReloadEnabled = GrimoraConfigFile.Bind(
			PluginName,
			"Enable Hot Reload",
			false,
			new ConfigDescription(
				"If the dll is placed in BepInEx/scripts, this will allow running certain commands that should only ever be ran to re-add abilities/cards back in the game correctly.")
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

		HandleHotReloadBefore();
	}

	public static void ResetRun()
	{
		Log.LogDebug($"[ResetRun] Resetting run");

		ResetConfig();
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
		_configBossesDefeated.Value = 0;
		_configCurrentRemovedPieces.Value = StaticDefaultRemovedPiecesList;
		_configCurrentChessboardIndex.Value = 0;
	}

	private void ResetConfigDataIfGrimoraHasNotReachedTable()
	{
		if (!StoryEventsData.EventCompleted(StoryEvent.GrimoraReachedTable))
		{
			Log.LogWarning($"Grimora has not reached the table yet, resetting values to false again.");
			ResetConfig();
		}
	}

	public void AddPieceToRemovedPiecesConfig(string pieceName)
	{
		_configCurrentRemovedPieces.Value += "," + pieceName + ",";
	}

	public void SetBossDefeatedInConfig(BaseBossExt boss)
	{
		_configBossesDefeated.Value = boss switch
		{
			KayceeBossOpponent => 1,
			SawyerBossOpponent => 2,
			RoyalBossOpponentExt => 3,
			GrimoraBossOpponentExt => 4
		};

		var bossPiece = ChessboardMapExt.Instance.BossPiece;
		ChessboardMapExt.Instance.BossDefeated = true;
		AddPieceToRemovedPiecesConfig(bossPiece.name);
		Log.LogDebug($"[SetBossDefeatedInConfig] Boss {bossPiece} defeated.");
	}
}
