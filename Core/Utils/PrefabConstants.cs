using DiskCardGame;
using UnityEngine;
using static GrimoraMod.GrimoraPlugin;

namespace GrimoraMod;

public static class PrefabConstants
{
	public const string PathChessboardMap = "Prefabs/Map/ChessboardMap";
	public const string PathSpecialNodes = "Prefabs/SpecialNodeSequences";
	public const string PathArt3D = "Art/Assets3D";

	public static GameObject GraveDiggerFigurine 
		=> ResourceBank.Get<GameObject>($"{PathArt3D}/PlayerAvatar/gravedigger/GravediggerFin");

	public static ChessboardEnemyPiece BossPiece =>
		ResourceBank.Get<ChessboardEnemyPiece>($"{PathChessboardMap}/BossFigurine");

	public static ChessboardChestPiece ChestPiece =>
		ResourceBank.Get<ChessboardChestPiece>($"{PathChessboardMap}/ChessboardChestPiece");

	public static ChessboardEnemyPiece EnemyPiece =>
		ResourceBank.Get<ChessboardEnemyPiece>($"{PathChessboardMap}/ChessboardEnemyPiece");

	public static GameObject GrimoraSelectableCard =>
		ResourceBank.Get<GameObject>("Prefabs/Cards/SelectableCard_Grimora");

	public static GameObject GrimoraPlayableCard =>
		ResourceBank.Get<GameObject>("Prefabs/Cards/PlayableCard_Grimora");

	public static GameObject GrimoraCardBack =>
		ResourceBank.Get<GameObject>("Prefabs/Cards/CardBack_Grimora");

	public static GameObject EyeBall => ResourceBank.Get<GameObject>($"{PathSpecialNodes}/EyeBall");
	
	public static Material GoatEyeMat => ResourceBank.Get<Material>($"Art/Materials/Eyeball_Goat");

	public static GameObject SkinningKnife => ResourceBank.Get<GameObject>($"{PathSpecialNodes}/SkinningKnife");

	public static GameObject Tombstone3 =>
		ResourceBank.Get<GameObject>($"{PathChessboardMap}/Chessboard_Tombstone_3");

	public static GameObject CardStatBoostSequencer
	{
		get
		{
			Log.LogDebug($"Getting CardStatBoostSequencer");
			return ResourceBank.Get<GameObject>($"{PathSpecialNodes}/CardStatBoostSequencer");
		}
	}

	public static GameObject BoneyardGrave
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

	public static GameObject ElectricChair => AllPrefabs.Single(go => go.name.Equals("SpecialNode_ElectricChair"));

	public static Material WoodenBoxMaterial
	{
		get
		{
			Log.LogDebug($"Getting WoodenBoxMaterial");
			return ResourceBank.Get<Material>($"{PathArt3D}/nodesequences/woodenbox/WoodenBox_Wood");
		}
	}

	public static Material AncientStonesMaterial
	{
		get
		{
			Log.LogDebug($"Getting AncientStonesMaterial");
			return ResourceBank.Get<Material>($"{PathArt3D}/misc/AncientRuins/AncientRuins_StonePath");
		}
	}
}
