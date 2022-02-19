using DiskCardGame;
using UnityEngine;
using static GrimoraMod.GrimoraPlugin;

namespace GrimoraMod;

public static class PrefabConstants
{
	public const string PathChessboardMap = "Prefabs/Map/ChessboardMap";
	public const string PathSpecialNodes = "Prefabs/SpecialNodeSequences";
	public const string PathArt3D = "Art/Assets3D";


	#region Cards

	public static GameObject GrimoraCardBack =
		ResourceBank.Get<GameObject>("Prefabs/Cards/CardBack_Grimora");

	public static GameObject GrimoraPlayableCard =
		ResourceBank.Get<GameObject>("Prefabs/Cards/PlayableCard_Grimora");

	public static GameObject GrimoraSelectableCard =
		ResourceBank.Get<GameObject>("Prefabs/Cards/SelectableCard_Grimora");

	#endregion


	#region BossPieces

	public static readonly GameObject BossPieceGrimora
		= ResourceBank.Get<GameObject>("Prefabs/Opponents/Grimora/GrimoraAnim");

	public static readonly GameObject BossPieceRoyal
		= ResourceBank.Get<GameObject>($"{PathChessboardMap}/BossFigurine");

	public static readonly GameObject BossPieceKaycee
		= AssetUtils.GetPrefab<GameObject>("ChessboardPiece_KayceeFigurine");

	public static readonly GameObject BossPieceSawyer
		= AssetUtils.GetPrefab<GameObject>("ChessboardPiece_SawyerFigurine");

	#endregion


	#region ChessPieces

	public static ChessboardChestPiece ChestPiece =
		ResourceBank.Get<ChessboardChestPiece>($"{PathChessboardMap}/ChessboardChestPiece");

	public static ChessboardEnemyPiece EnemyPiece =
		ResourceBank.Get<ChessboardEnemyPiece>($"{PathChessboardMap}/ChessboardEnemyPiece");

	public static GameObject CardRemovalFigurine = AssetUtils.GetPrefab<GameObject>("ChessboardPiece_CardRemove");


	public static GameObject GoatEyeFigurine = AssetUtils.GetPrefab<GameObject>("ChessboardPiece_GoatEye");

	#endregion


	#region Boneyard

	public static GameObject BoneyardGrave = AssetUtils.GetPrefab<GameObject>("BoneyardBurialGrave");

	public static GameObject BoneyardFigurine = AssetUtils.GetPrefab<GameObject>("ChessboardPiece_Boneyard");

	public static GameObject Tombstone3 =
		ResourceBank.Get<GameObject>($"{PathChessboardMap}/Chessboard_Tombstone_3");

	#endregion


	#region ElectricChair

	public static GameObject ElectricChair = AssetUtils.GetPrefab<GameObject>("ChessboardPiece_ElectricChair");

	public static GameObject ElectricChairForSelectionSlot = AssetUtils.GetPrefab<GameObject>("ElectricChairV2");

	#endregion
	

	public static GameObject CardStatBoostSequencer =
		ResourceBank.Get<GameObject>($"{PathSpecialNodes}/CardStatBoostSequencer");
	
	public static Material WoodenBoxMaterial =
		ResourceBank.Get<Material>($"{PathArt3D}/nodesequences/woodenbox/WoodenBox_Wood");

	public static Material AncientStonesMaterial =
		ResourceBank.Get<Material>($"{PathArt3D}/misc/AncientRuins/AncientRuins_StonePath");
}
