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


	#region Controllers/CustomAnims

	public static RuntimeAnimatorController GraveStoneController
		=> AssetUtils.GetPrefab<RuntimeAnimatorController>("GravestoneCardAnim - Copy");

	public static RuntimeAnimatorController SkeletonArmController
		=> AssetUtils.GetPrefab<RuntimeAnimatorController>("SkeletonAttackAnim");

	public static GameObject CustomSkeletonArmBase => AssetUtils.GetPrefab<GameObject>("Skeleton2ArmsAttacks");
	
	public static GameObject CustomSkeletonArmGiants => AssetUtils.GetPrefab<GameObject>("SkeletonArms_Giants");
	
	public static GameObject CustomSkeletonArmSentry => AssetUtils.GetPrefab<GameObject>("Grimora_Sentry");

	#endregion


	#region BossPieces

	public static readonly GameObject BossPieceGrimora
		= ResourceBank.Get<GameObject>("Prefabs/Opponents/Grimora/GrimoraAnim");

	public static readonly GameObject BossPieceRoyal
		= ResourceBank.Get<GameObject>($"{PathChessboardMap}/BossFigurine");

	public static GameObject BossPieceKaycee => AssetUtils.GetPrefab<GameObject>("KayceeFigurine");

	public static GameObject BossPieceSawyer => AssetUtils.GetPrefab<GameObject>("SawyerFigurine");

	#endregion


	#region ChessPieces

	public static readonly ChessboardChestPiece ChestPiece =
		ResourceBank.Get<ChessboardChestPiece>($"{PathChessboardMap}/ChessboardChestPiece");

	public static readonly ChessboardEnemyPiece EnemyPiece =
		ResourceBank.Get<ChessboardEnemyPiece>($"{PathChessboardMap}/ChessboardEnemyPiece");

	public static GameObject CardRemovalFigurine => AssetUtils.GetPrefab<GameObject>("ChessboardPiece_CardRemove");
	
	public static GameObject GoatEyeFigurine => GrimoraPlugin.kopieGameObjects.Find(g => g.name.Contains("AnkhGuardPrefab"));

	public static GameObject GainConsumable => GrimoraPlugin.kopieGameObjects.Find(g => g.name.Contains("SarcophagusPrefab"));

	public static GameObject GhostBottle => kopieGameObjects.Find(g => g.name.Contains("soul"));

	#endregion


	#region Boneyard

	public static GameObject BoneyardGrave => GrimoraPlugin.kopieGameObjects.Find(g => g.name.Contains("BoneyardBurialGraveNe"));


	public static Material OldBoneyardSlot => AssetUtils.GetPrefab<Material>("Boneyard_SelectionSlot");
	public static Material EnergyGraveSelectionSlot => AssetUtils.GetPrefab<Material>("energygrave");

	public static Material BrittleGraveSelectionSlot => AssetUtils.GetPrefab<Material>("gravebrittle");

	public static Material HaunterGraveSelectionSlot => AssetUtils.GetPrefab<Material>("gravehaunter");

	public static Material PrisonerGraveSelectionSlot => AssetUtils.GetPrefab<Material>("graveprisoner");

	public static Material StarveGraveSelectionSlot => AssetUtils.GetPrefab<Material>("gravestarve");

	public static Material BoneyardConfirmButton =>
		AssetUtils.GetPrefab<Material>("Boneyard_ConfirmButton_Shovel");

	public static GameObject BoneyardFigurine => AssetUtils.GetPrefab<GameObject>("ChessboardPiece_Boneyard");

	public static GameObject Tombstone3 =>
		ResourceBank.Get<GameObject>($"{PathChessboardMap}/Chessboard_Tombstone_3");

	#endregion


	#region ElectricChair

	public static GameObject ElectricChairFigurine => AssetUtils.GetPrefab<GameObject>("ChessboardPiece_ElectricChair");

	public static Material ElectricChairSelectionSlot =>
		AssetUtils.GetPrefab<Material>("ElectricChair_SelectionSlot");

	public static GameObject ElectricChair => AssetUtils.GetPrefab<GameObject>("Electric_Chair");

	#endregion


	#region Menu

	public static Sprite MenuCardGrimora => AssetUtils.GetPrefab<Sprite>("MenuCardNew");



	public static Sprite TitleSprite => AssetUtils.GetPrefab<Sprite>("TitleLogo");

	#endregion


	#region Skulls

	public static GameObject BossSkullKaycee => kopieGameObjects.Find(g => g.name.Contains("KayceeBossSkull"));

	public static GameObject BossSkullSawyer => AssetUtils.GetPrefab<GameObject>("SawyerBossSkull");

	public static readonly GameObject BossSkullRoyal =
		ResourceBank.Get<GameObject>("Prefabs/Opponents/Grimora/RoyalBossSkull");

	#endregion


	public static GameObject GrimoraFirstPersonHammer =>
		AssetUtils.GetPrefab<GameObject>("FirstPersonGrimoraHammer");

	public static GameObject GrimoraHammer => AssetUtils.GetPrefab<GameObject>("GrimoraHammer");

	public static readonly GameObject CardStatBoostSequencer =
		ResourceBank.Get<GameObject>($"{PathSpecialNodes}/CardStatBoostSequencer");

	public static readonly Material WoodenBoxMaterial =
		ResourceBank.Get<Material>($"{PathArt3D}/nodesequences/woodenbox/WoodenBox_Wood");

	public static readonly Material AncientStonesMaterial =
		ResourceBank.Get<Material>($"{PathArt3D}/misc/AncientRuins/AncientRuins_StonePath");
}
