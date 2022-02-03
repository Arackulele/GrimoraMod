using DiskCardGame;
using HarmonyLib;
using Sirenix.Utilities;
using UnityEngine;
using static GrimoraMod.GrimoraPlugin;

namespace GrimoraMod;

public class PrefabPieceHelper
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

	internal ChessboardBlockerPiece PrefabBlockerPiece;

	internal readonly ChessboardEnemyPiece PrefabEnemyPiece;

	internal readonly ChessboardEnemyPiece PrefabBossPiece;

	internal readonly ChessboardChestPiece PrefabChestPiece;

	internal readonly ChessboardBoneyardPiece PrefabBoneyardPiece;

	internal readonly ChessboardCardRemovePiece PrefabCardRemovePiece;

	internal readonly ChessboardElectricChairPiece PrefabElectricChairPiece;

	internal readonly ChessboardGoatEyePiece PrefabGoatEyePiece;

	public readonly Dictionary<Type, Tuple<float, Func<GameObject>, Func<ChessboardPiece>>> PieceSetupByType;

	public PrefabPieceHelper()
	{
		PrefabBossPiece = ResourceBank.Get<ChessboardEnemyPiece>($"{PathPrefabChessboardMap}/BossFigurine");
		PrefabChestPiece = ResourceBank.Get<ChessboardChestPiece>($"{PathPrefabChessboardMap}/ChessboardChestPiece");
		PrefabEnemyPiece = ResourceBank.Get<ChessboardEnemyPiece>($"{PathPrefabChessboardMap}/ChessboardEnemyPiece");

		PieceSetupByType = BuildDictionary();
		PrefabBlockerPiece = CreateCustomPrefabPiece<ChessboardBlockerPiece>();
		PrefabBoneyardPiece = CreateCustomPrefabPiece<ChessboardBoneyardPiece>();
		PrefabCardRemovePiece = CreateCustomPrefabPiece<ChessboardCardRemovePiece>();
		PrefabElectricChairPiece = CreateCustomPrefabPiece<ChessboardElectricChairPiece>();
		PrefabGoatEyePiece = CreateCustomPrefabPiece<ChessboardGoatEyePiece>();
	}

	public GameObject GetActiveRegionBlockerPiece()
	{
		int bossesDead = ConfigHelper.Instance.BossesDefeated;
		GameObject blocker = _bossByIndex.GetValueSafe(bossesDead).Invoke();
		// the reason for doing this is because the materials are massive if in our own asset bundle, 5MB+ total
		// so lets just use the already existing material in the game
		if (bossesDead == 2)
		{
			// barrel
			blocker.GetComponent<MeshRenderer>().material =
				ResourceBank.Get<Material>($"{PathPrefabArt3D}/GameTable/Tombstones/Tombstone_1");
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
		PrefabBlockerPiece = CreateCustomPrefabPiece<ChessboardBlockerPiece>();
	}

	// TODO: not sure if a tuple is what I need?
	/// <summary>
	/// Item1: Scaled value		 -> value to set the localscale to
	/// Item3: GameObject			 -> Object to use when creating the custom prefab piece.
	/// Item4: ChessboardPiece -> The prefab to use 
	/// </summary>
	private Dictionary<Type, Tuple<float, Func<GameObject>, Func<ChessboardPiece>>> BuildDictionary()
	{
		return new Dictionary<Type, Tuple<float, Func<GameObject>, Func<ChessboardPiece>>>()
		{
			{
				typeof(ChessboardBlockerPiece),
				new Tuple<float, Func<GameObject>, Func<ChessboardPiece>>(
					0f,
					GetActiveRegionBlockerPiece,
					() => PrefabBlockerPiece
				)
			},
			{
				typeof(ChessboardBoneyardPiece),
				new Tuple<float, Func<GameObject>, Func<ChessboardPiece>>(
					1.25f,
					() => ResourceBank.Get<GameObject>($"{PathPrefabArt3D}/PlayerAvatar/gravedigger/GravediggerFin"),
					() => PrefabBoneyardPiece
				)
			},
			{
				typeof(ChessboardBossPiece),
				new Tuple<float, Func<GameObject>, Func<ChessboardPiece>>(
					1f,
					() => ResourceBank.Get<ChessboardEnemyPiece>($"{PathPrefabChessboardMap}/BossFigurine").gameObject,
					() => PrefabBossPiece
				)
			},
			{
				typeof(ChessboardCardRemovePiece), new Tuple<float, Func<GameObject>, Func<ChessboardPiece>>(
					0.25f,
					() => ResourceBank.Get<GameObject>($"{PathPrefabSpecialNodes}/SkinningKnife"),
					() => PrefabCardRemovePiece
				)
			},
			{
				typeof(ChessboardChestPiece), new Tuple<float, Func<GameObject>, Func<ChessboardPiece>>(
					1f,
					() => ResourceBank.Get<ChessboardChestPiece>($"{PathPrefabChessboardMap}/ChessboardChestPiece").gameObject,
					() => PrefabChestPiece
				)
			},
			{
				typeof(ChessboardElectricChairPiece), new Tuple<float, Func<GameObject>, Func<ChessboardPiece>>(
					18f,
					() => AllPrefabAssets.Single(go => go.name.Equals("ElectricChairStone")),
					() => PrefabElectricChairPiece
				)
			},
			{
				typeof(ChessboardEnemyPiece), new Tuple<float, Func<GameObject>, Func<ChessboardPiece>>(
					1f,
					() => ResourceBank.Get<ChessboardEnemyPiece>($"{PathPrefabChessboardMap}/ChessboardEnemyPiece").gameObject,
					() => PrefabEnemyPiece
				)
			},
			{
				typeof(ChessboardGoatEyePiece), new Tuple<float, Func<GameObject>, Func<ChessboardPiece>>(
					0.4f,
					() => ResourceBank.Get<GameObject>($"{PathPrefabSpecialNodes}/EyeBall"),
					() => PrefabGoatEyePiece
				)
			}
		};
	}

	internal T CreateCustomPrefabPiece<T>() where T : ChessboardPiece
	{
		Log.LogDebug($"[CreateCustomPrefabPiece] Creating custom piece [{typeof(T)}]");
		if (PieceSetupByType.TryGetValue(typeof(T), out Tuple<float, Func<GameObject>, Func<ChessboardPiece>> tuple))
		{
			Log.LogDebug($"[CreateCustomPrefabPiece] Tuple [{tuple}]");
			float scaledValue = tuple.Item1;
			GameObject gameObj = tuple.Item2.Invoke();

			if (typeof(T) == typeof(ChessboardBlockerPiece))
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

			// Log.LogDebug($"[CreateCustomPrefabPiece] Setting localScale");

			// Vector3 vLocalPosition = gameObj.transform.localPosition;
			// gameObj.transform.localPosition = new Vector3(
			// 	vLocalPosition.x,
			// 	tuple.Item2 < 0.1f ? vLocalPosition.y : tuple.Item2,
			// 	vLocalPosition.z
			// );

			T piece = gameObj.AddComponent<T>();
			// this is just so any anim that would play doesn't throw an exception
			piece.anim = PrefabEnemyPiece.anim;
			return piece;
		}

		Log.LogError($"[CreateCustomPrefabPiece] Failed to create [{typeof(T)}]");
		return null;
	}
}
