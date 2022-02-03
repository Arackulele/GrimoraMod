using DiskCardGame;
using HarmonyLib;
using Sirenix.Utilities;
using UnityEngine;
using static GrimoraMod.GrimoraPlugin;

namespace GrimoraMod;

public class PrefabPieceHelper : ManagedBehaviour
{
	private readonly Dictionary<int, Func<GameObject>> _bossByIndex = new()
	{
		{ 0, () => AllPrefabAssets.Single(pb => pb.name.Equals("Blocker_Kaycee")) },
		{ 1, () => AllPrefabAssets.Single(pb => pb.name.Equals("Blocker_Sawyer")) },
		{ 2, () => AllPrefabAssets.Single(pb => pb.name.Equals("Blocker_Royal")) },
		{ 3, () => AllPrefabAssets.Single(pb => pb.name.Equals("Blocker_Grimora")) },
	};
	
	public const string PathPrefabChessboardMap = "Prefabs/Map/ChessboardMap";
	public const string PathPrefabSpecialNodes = "Prefabs/SpecialNodeSequences";
	public const string PathPrefabArt3D = "Art/Assets3D";

	internal ChessboardBlockerPieceExt PrefabBlockerPiece;

	internal readonly ChessboardEnemyPiece PrefabEnemyPiece;

	internal readonly ChessboardEnemyPiece PrefabBossPiece;

	internal readonly ChessboardChestPiece PrefabChestPiece;

	internal readonly ChessboardBoneyardPiece PrefabBoneyardPiece;

	internal readonly ChessboardCardRemovePiece PrefabCardRemovePiece;

	internal readonly ChessboardElectricChairPiece PrefabElectricChairPiece;

	internal readonly ChessboardGoatEyePiece PrefabGoatEyePiece;

	public readonly Dictionary<Type, Tuple<float, GameObject, Func<ChessboardPiece>>> PieceSetupByType;

	public PrefabPieceHelper()
	{
		PrefabBossPiece = ResourceBank.Get<ChessboardEnemyPiece>($"{PathPrefabChessboardMap}/BossFigurine");
		PrefabChestPiece = ResourceBank.Get<ChessboardChestPiece>($"{PathPrefabChessboardMap}/ChessboardChestPiece");
		PrefabEnemyPiece = ResourceBank.Get<ChessboardEnemyPiece>($"{PathPrefabChessboardMap}/ChessboardEnemyPiece");

		PieceSetupByType = BuildDictionary();
		PrefabBlockerPiece = CreateCustomPrefabPiece<ChessboardBlockerPieceExt>();
		PrefabBoneyardPiece = CreateCustomPrefabPiece<ChessboardBoneyardPiece>();
		PrefabCardRemovePiece = CreateCustomPrefabPiece<ChessboardCardRemovePiece>();
		PrefabElectricChairPiece = CreateCustomPrefabPiece<ChessboardElectricChairPiece>();
		PrefabGoatEyePiece = CreateCustomPrefabPiece<ChessboardGoatEyePiece>();
	}

	public GameObject GetActiveRegionBlockerPiece()
	{
		Log.LogDebug($"[GetActiveRegionBlockerPiece] Getting active region blocker piece");
		int bossesDead = ConfigHelper.Instance.BossesDefeated;
		GameObject blocker = _bossByIndex.GetValueSafe(bossesDead).Invoke();
		// the reason for doing this is because the materials are massive if in our own asset bundle, 5MB+ total
		// so lets just use the already existing material in the game
		if (bossesDead == 2)
		{
			// barrel
			blocker.GetComponent<MeshRenderer>().material =
				ResourceBank.Get<Material>($"{PathPrefabArt3D}/nodesequences/woodenbox/WoodenBox_Wood");
		}
		else if (bossesDead == 3)
		{
			// low-poly skull
			blocker.GetComponent<MeshRenderer>().material =
				ResourceBank.Get<Material>($"{PathPrefabArt3D}/misc/AncientRuins/AncientRuins_StonePath");
		}

		return blocker;
	}

	public void ChangeBlockerPieceForRegion()
	{
		Log.LogDebug($"[ChangeBlockerPieceForRegion] Changing prefab for blocker piece");
		PrefabBlockerPiece = CreateCustomPrefabPiece<ChessboardBlockerPieceExt>();
	}

	// TODO: not sure if a tuple is what I need?
	/// <summary>
	/// Item1: Scaled value		 -> value to set the localscale to
	/// Item3: GameObject			 -> Object to use when creating the custom prefab piece.
	/// Item4: ChessboardPiece -> The prefab to use 
	/// </summary>
	private Dictionary<Type, Tuple<float, GameObject, Func<ChessboardPiece>>> BuildDictionary()
	{
		Log.LogDebug($"[PrefabPieceHelper] Building dictionary");
		return new Dictionary<Type, Tuple<float, GameObject, Func<ChessboardPiece>>>()
		{
			{
				typeof(ChessboardBlockerPieceExt),
				new Tuple<float, GameObject, Func<ChessboardPiece>>(
					0f,
					GetActiveRegionBlockerPiece().gameObject,
					() => PrefabBlockerPiece
				)
			},
			{
				typeof(ChessboardBoneyardPiece),
				new Tuple<float, GameObject, Func<ChessboardPiece>>(
					1.25f,
					ResourceBank.Get<GameObject>($"{PathPrefabArt3D}/PlayerAvatar/gravedigger/GravediggerFin"),
					() => PrefabBoneyardPiece
				)
			},
			{
				typeof(ChessboardCardRemovePiece), new Tuple<float, GameObject, Func<ChessboardPiece>>(
					0.25f,
					ResourceBank.Get<GameObject>($"{PathPrefabSpecialNodes}/SkinningKnife"),
					() => PrefabCardRemovePiece
				)
			},
			{
				typeof(ChessboardChestPiece), new Tuple<float, GameObject, Func<ChessboardPiece>>(
					1f,
					ResourceBank.Get<ChessboardChestPiece>($"{PathPrefabChessboardMap}/ChessboardChestPiece").gameObject,
					() => PrefabChestPiece
				)
			},
			{
				typeof(ChessboardElectricChairPiece), new Tuple<float, GameObject, Func<ChessboardPiece>>(
					18f,
					AllPrefabAssets.Single(go => go.name.Equals("SpecialNode_ElectricChair")),
					() => PrefabElectricChairPiece
				)
			},
			{
				typeof(ChessboardEnemyPiece), new Tuple<float, GameObject, Func<ChessboardPiece>>(
					1f,
					ResourceBank.Get<ChessboardEnemyPiece>($"{PathPrefabChessboardMap}/ChessboardEnemyPiece").gameObject,
					() => PrefabEnemyPiece
				)
			},
			{
				typeof(ChessboardGoatEyePiece), new Tuple<float, GameObject, Func<ChessboardPiece>>(
					0.4f,
					ResourceBank.Get<GameObject>($"{PathPrefabSpecialNodes}/EyeBall"),
					() => PrefabGoatEyePiece
				)
			}
		};
	}

	private T CreateCustomPrefabPiece<T>() where T : ChessboardPiece
	{
		Log.LogDebug($"[CreateCustomPrefabPiece] Creating custom piece [{typeof(T)}]");
		if (PieceSetupByType.TryGetValue(typeof(T), out Tuple<float, GameObject, Func<ChessboardPiece>> tuple))
		{
			GameObject gameObj = tuple.Item2;

			if (typeof(T) == typeof(ChessboardBlockerPieceExt))
			{
				// Log.LogDebug($"[CreateCustomPrefabPiece] Grabbed scale [{scaledValue}] and GO [{gameObj}]");
				gameObj = GetActiveRegionBlockerPiece();
			}
			else if (typeof(T) == typeof(ChessboardGoatEyePiece))
			{
				Material goatEyeMat = ResourceBank.Get<Material>("Art/Materials/Eyeball_Goat");
				gameObj.GetComponent<MeshRenderer>().material = goatEyeMat;
				gameObj.GetComponent<MeshRenderer>().sharedMaterial = goatEyeMat;
				gameObj.GetComponent<Rigidbody>().useGravity = false;
			}

			T piece = gameObj.AddComponent<T>();
			// this is just so any anim that would play doesn't throw an exception
			piece.anim = PrefabEnemyPiece.anim;
			return piece;
		}

		Log.LogError($"[CreateCustomPrefabPiece] Failed to create [{typeof(T)}]");
		return null;
	}
}
