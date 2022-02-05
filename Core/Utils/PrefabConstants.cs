using DiskCardGame;
using UnityEngine;
using static GrimoraMod.GrimoraPlugin;

namespace GrimoraMod;

public class PrefabConstants : Singleton<PrefabConstants>
{
	public const string PathChessboardMap = "Prefabs/Map/ChessboardMap";
	public const string PathSpecialNodes = "Prefabs/SpecialNodeSequences";
	public const string PathArt3D = "Art/Assets3D";

	public GameObject GraveDiggerFigurine
	{
		get
		{
			Log.LogDebug($"Getting GraveDiggerFigurine");
			return ResourceBank.Get<GameObject>($"{PathArt3D}/PlayerAvatar/gravedigger/GravediggerFin");
		}
	}

	public ChessboardEnemyPiece BossPiece =>
		ResourceBank.Get<ChessboardEnemyPiece>($"{PathChessboardMap}/BossFigurine");

	public ChessboardChestPiece ChestPiece =>
		ResourceBank.Get<ChessboardChestPiece>($"{PathChessboardMap}/ChessboardChestPiece");

	public ChessboardEnemyPiece EnemyPiece =>
		ResourceBank.Get<ChessboardEnemyPiece>($"{PathChessboardMap}/ChessboardEnemyPiece");

	public GameObject GrimoraSelectableCard =>
		ResourceBank.Get<GameObject>("Prefabs/Cards/SelectableCard_Grimora");

	public GameObject GrimoraPlayableCard =>
		ResourceBank.Get<GameObject>("Prefabs/Cards/PlayableCard_Grimora");

	public GameObject GrimoraCardBack =>
		ResourceBank.Get<GameObject>("Prefabs/Cards/CardBack_Grimora");

	public GameObject GoatEye = ResourceBank.Get<GameObject>($"{PathSpecialNodes}/EyeBall");

	public GameObject SkinningKnife = ResourceBank.Get<GameObject>($"{PathSpecialNodes}/SkinningKnife");

	public GameObject Tombstone3 =>
		ResourceBank.Get<GameObject>($"{PathChessboardMap}/Chessboard_Tombstone_3");

	public GameObject CardStatBoostSequencer
	{
		get
		{
			Log.LogDebug($"Getting CardStatBoostSequencer");
			return ResourceBank.Get<GameObject>($"{PathSpecialNodes}/CardStatBoostSequencer");
		}
	}

	public GameObject BoneyardGrave
	{
		get
		{
			Log.LogDebug($"Getting BoneyardGrave");
			return AllPrefabs.Single(obj =>
			{
				Log.LogDebug($"Checking against [{obj.name}] is [{obj.name.Equals("BoneyardBurialGrave")}]");
				return obj.name.Equals("BoneyardBurialGrave", StringComparison.OrdinalIgnoreCase);
			});
		}
	}

	public Material WoodenBoxMaterial
	{
		get
		{
			Log.LogDebug($"Getting WoodenBoxMaterial");
			return ResourceBank.Get<Material>($"{PathArt3D}/nodesequences/woodenbox/WoodenBox_Wood");
		}
	}

	public Material AncientStonesMaterial
	{
		get
		{
			Log.LogDebug($"Getting AncientStonesMaterial");
			return ResourceBank.Get<Material>($"{PathArt3D}/misc/AncientRuins/AncientRuins_StonePath");
		}
	}
}
