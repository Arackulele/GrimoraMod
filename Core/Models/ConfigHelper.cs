using BepInEx;
using BepInEx.Configuration;
using static GrimoraMod.GrimoraPlugin;

namespace GrimoraMod;

public class ConfigHelper
{
	private static ConfigHelper m_Instance;

	public static ConfigHelper Instance => m_Instance ??= new ConfigHelper();

	public readonly ConfigFile GrimoraConfigFile = new(
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

	public ConfigEntry<int> CurrentChessboardIndex;

	private ConfigEntry<bool> KayceeFirstBossDead;

	public static bool IsKayceeDead => Instance.KayceeFirstBossDead.Value;

	private ConfigEntry<bool> SawyerSecondBossDead;

	public static bool isSawyerDead => Instance.SawyerSecondBossDead.Value;

	private ConfigEntry<bool> RoyalThirdBossDead;

	public static bool isRoyalDead => Instance.RoyalThirdBossDead.Value;

	private ConfigEntry<bool> GrimoraBossDead;

	public static bool isGrimoraDead => Instance.GrimoraBossDead.Value;

	private ConfigEntry<bool> DeveloperMode;

	public static bool IsDevModeEnabled => Instance.DeveloperMode.Value;


	public ConfigEntry<string> CurrentRemovedPieces;

	public static List<string> RemovedPiecesList
		=> Instance.CurrentRemovedPieces.Value.Split(',').Distinct().ToList();

	public void BindConfig()
	{
		CurrentChessboardIndex
			= GrimoraConfigFile.Bind(PluginName, "Current chessboard layout index", 0);

		KayceeFirstBossDead
			= GrimoraConfigFile.Bind(PluginName, "Kaycee defeated?", false);

		SawyerSecondBossDead
			= GrimoraConfigFile.Bind(PluginName, "Sawyer defeated?", false);

		RoyalThirdBossDead
			= GrimoraConfigFile.Bind(PluginName, "Royal defeated?", false);

		GrimoraBossDead
			= GrimoraConfigFile.Bind(PluginName, "Grimora defeated?", false);

		CurrentRemovedPieces = GrimoraConfigFile.Bind(
			PluginName,
			"Current Removed Pieces",
			StaticDefaultRemovedPiecesList,
			new ConfigDescription("Contains all the current removed pieces." +
			                      "\nDo not alter this list unless you know what you are doing!")
		);

		DeveloperMode = GrimoraConfigFile.Bind(
			PluginName,
			"Enable Developer Mode",
			false,
			new ConfigDescription("Does not generate blocker pieces. Chests fill first row, enemy pieces fill first column.")
		);

		var list = CurrentRemovedPieces.Value.Split(',').ToList();
		// this is so that for whatever reason the game map gets added to the removal list,
		//	this will automatically remove those entries
		if (list.Contains("ChessboardGameMap"))
		{
			list.RemoveAll(piece => piece.Equals("ChessboardGameMap"));
		}

		CurrentRemovedPieces.Value = string.Join(",", list.Distinct());
	}

	public void ResetConfig()
	{
		Log.LogWarning($"Resetting Grimora Mod config");
		KayceeFirstBossDead.Value = false;
		SawyerSecondBossDead.Value = false;
		RoyalThirdBossDead.Value = false;
		GrimoraBossDead.Value = false;
		CurrentRemovedPieces.Value = StaticDefaultRemovedPiecesList;
		CurrentChessboardIndex.Value = 0;
	}

	public static void AddPieceToRemovedPiecesConfig(string pieceName)
	{
		Instance.CurrentRemovedPieces.Value += "," + pieceName + ",";
	}

	public int BonesToAdd
	{
		get
		{
			int bonesToAdd = 0;
			if (IsKayceeDead)
			{
				bonesToAdd += 2;
			}

			if (isSawyerDead)
			{
				bonesToAdd += 3;
			}

			if (isRoyalDead)
			{
				bonesToAdd += 5;
			}

			return bonesToAdd;
		}
	}

	public void SetBossDefeatedInConfig(BaseBossExt bossOpponent)
	{
		switch (bossOpponent)
		{
			case KayceeBossOpponent:
				ConfigHelper.Instance.KayceeFirstBossDead.Value = true;
				break;
			case SawyerBossOpponent:
				ConfigHelper.Instance.SawyerSecondBossDead.Value = true;
				break;
			case RoyalBossOpponentExt:
				ConfigHelper.Instance.RoyalThirdBossDead.Value = true;
				break;
			case GrimoraBossOpponentExt:
				ConfigHelper.Instance.GrimoraBossDead.Value = true;
				break;
		}

		var bossPiece = ChessboardMapExt.Instance.BossPiece;
		ChessboardMapExt.Instance.BossDefeated = true;
		AddPieceToRemovedPiecesConfig(bossPiece.name);
		Log.LogDebug($"[BossDefeatedSequence][PostFix] Boss {GetType()} defeated.");
	}
}
