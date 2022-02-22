using System.Diagnostics.CodeAnalysis;
using DiskCardGame;
using UnityEngine;
using static GrimoraMod.GrimoraPlugin;

namespace GrimoraMod;

[SuppressMessage("ReSharper", "InconsistentNaming")]
public static class AssetConstants
{
	public const string PathChessboardMap = "Prefabs/Map/ChessboardMap";
	public const string PathSpecialNodes = "Prefabs/SpecialNodeSequences";
	public const string PathArt3D = "Art/Assets3D";


	#region Cards

	public static readonly GameObject GrimoraCardBack =
		ResourceBank.Get<GameObject>("Prefabs/Cards/CardBack_Grimora");

	public static readonly GameObject GrimoraPlayableCard =
		ResourceBank.Get<GameObject>("Prefabs/Cards/PlayableCard_Grimora");

	public static readonly GameObject GrimoraSelectableCard =
		ResourceBank.Get<GameObject>("Prefabs/Cards/SelectableCard_Grimora");

	#endregion


	#region Controllers

	public static readonly RuntimeAnimatorController GraveStoneController
		= AssetUtils.GetPrefab<RuntimeAnimatorController>("GravestoneCardAnim - Copy");

	public static readonly RuntimeAnimatorController SkeletonArmController
		= AssetUtils.GetPrefab<RuntimeAnimatorController>("SkeletonAttackAnim");

	#endregion


	#region BossPieces

	public static readonly GameObject BossPieceGrimora
		= ResourceBank.Get<GameObject>("Prefabs/Opponents/Grimora/GrimoraAnim");

	public static readonly GameObject BossPieceRoyal
		= ResourceBank.Get<GameObject>($"{PathChessboardMap}/BossFigurine");

	public static readonly GameObject BossPieceKaycee = AssetUtils.GetPrefab<GameObject>("KayceeFigurine");

	public static readonly GameObject BossPieceSawyer = AssetUtils.GetPrefab<GameObject>("SawyerFigurine");

	#endregion


	#region ChessPieces

	public static readonly ChessboardChestPiece ChestPiece =
		ResourceBank.Get<ChessboardChestPiece>($"{PathChessboardMap}/ChessboardChestPiece");

	public static readonly ChessboardEnemyPiece EnemyPiece =
		ResourceBank.Get<ChessboardEnemyPiece>($"{PathChessboardMap}/ChessboardEnemyPiece");

	public static readonly GameObject CardRemovalFigurine
		= AssetUtils.GetPrefab<GameObject>("ChessboardPiece_CardRemove");

	public static readonly GameObject GoatEyeFigurine = AssetUtils.GetPrefab<GameObject>("ChessboardPiece_GoatEye");

	#endregion


	#region Boneyard

	public static readonly GameObject BoneyardGrave = AssetUtils.GetPrefab<GameObject>("BoneyardBurialGrave");

	public static readonly Material BoneyardSelectionSlot = AssetUtils.GetPrefab<Material>("Boneyard_SelectionSlot");

	public static readonly Material BoneyardConfirmButton =
		AssetUtils.GetPrefab<Material>("Boneyard_ConfirmButton_Shovel");

	public static readonly GameObject BoneyardFigurine = AssetUtils.GetPrefab<GameObject>("ChessboardPiece_Boneyard");

	public static readonly GameObject Tombstone3 =
		ResourceBank.Get<GameObject>($"{PathChessboardMap}/Chessboard_Tombstone_3");

	#endregion


	#region ElectricChair

	public static readonly GameObject ElectricChair = AssetUtils.GetPrefab<GameObject>("ChessboardPiece_ElectricChair");

	public static readonly Material ElectricChairSelectionSlot =
		AssetUtils.GetPrefab<Material>("ElectricChair_SelectionSlot");

	public static readonly GameObject ElectricChairForSelectionSlot = AssetUtils.GetPrefab<GameObject>("ElectricChairV2");

	#endregion


	#region Menu

	public static readonly Sprite MenuCardGrimora = AssetUtils.GetPrefab<Sprite>("MenuCard");

	public static readonly Sprite TitleSprite = AssetUtils.GetPrefab<Sprite>("menutext_grimora_mod");

	#endregion


	#region Skulls

	public static readonly GameObject BossSkullKaycee = AssetUtils.GetPrefab<GameObject>("KayceeBossSkull");

	public static readonly GameObject BossSkullSawyer = AssetUtils.GetPrefab<GameObject>("SawyerBossSkull");

	#endregion


	public static readonly GameObject GrimoraFirstPersonHammer =
		AssetUtils.GetPrefab<GameObject>("FirstPersonGrimoraHammer");

	public static readonly GameObject GrimoraHammer = AssetUtils.GetPrefab<GameObject>("GrimoraHammer");

	public static readonly GameObject CardStatBoostSequencer =
		ResourceBank.Get<GameObject>($"{PathSpecialNodes}/CardStatBoostSequencer");

	public static readonly Material WoodenBoxMaterial =
		ResourceBank.Get<Material>($"{PathArt3D}/nodesequences/woodenbox/WoodenBox_Wood");

	public static readonly Material AncientStonesMaterial =
		ResourceBank.Get<Material>($"{PathArt3D}/misc/AncientRuins/AncientRuins_StonePath");
}
