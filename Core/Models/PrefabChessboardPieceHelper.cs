using DiskCardGame;
using HarmonyLib;
using UnityEngine;
using static GrimoraMod.GrimoraPlugin;

namespace GrimoraMod;

public class PrefabChessboardPieceHelper : ManagedBehaviour
{
	private readonly Dictionary<int, Func<GameObject>> _bossByIndex = new()
	{
		{ 0, () => AllPrefabs.Single(pb => pb.name.Equals("Blocker_Kaycee")) },
		{ 1, () => AllPrefabs.Single(pb => pb.name.Equals("Blocker_Sawyer")) },
		{ 2, () => AllPrefabs.Single(pb => pb.name.Equals("Blocker_Royal")) },
		{ 3, () => AllPrefabs.Single(pb => pb.name.Equals("Blocker_Grimora")) },
	};
	
	internal ChessboardBlockerPieceExt PrefabBlockerPiece;

	internal readonly ChessboardEnemyPiece PrefabEnemyPiece;

	internal readonly ChessboardEnemyPiece PrefabBossPiece;

	internal readonly ChessboardChestPiece PrefabChestPiece;

	internal readonly ChessboardBoneyardPiece PrefabBoneyardPiece;

	internal readonly ChessboardCardRemovePiece PrefabCardRemovePiece;

	internal readonly ChessboardElectricChairPiece PrefabElectricChairPiece;

	internal readonly ChessboardGoatEyePiece PrefabGoatEyePiece;

	public readonly Dictionary<Type, Tuple<float, GameObject, Func<ChessboardPiece>>> PieceSetupByType;

	public PrefabChessboardPieceHelper()
	{
		PrefabBossPiece = PrefabConstants.Instance.BossPiece;
		PrefabChestPiece = PrefabConstants.Instance.ChestPiece;
		PrefabEnemyPiece = PrefabConstants.Instance.EnemyPiece;

		PieceSetupByType = BuildDictionary();
		PrefabBlockerPiece = CreateCustomPrefabPiece<ChessboardBlockerPieceExt>();
		PrefabBoneyardPiece = CreateCustomPrefabPiece<ChessboardBoneyardPiece>();
		Log.LogDebug($"PrefabBoneyardPiece [{PrefabBoneyardPiece.GetHashCode()}]");
		PrefabCardRemovePiece = CreateCustomPrefabPiece<ChessboardCardRemovePiece>();
		PrefabElectricChairPiece = CreateCustomPrefabPiece<ChessboardElectricChairPiece>();
		PrefabGoatEyePiece = CreateCustomPrefabPiece<ChessboardGoatEyePiece>();
	}

	public GameObject GetActiveRegionBlockerPiece()
	{
		Log.LogDebug($"[GetActiveRegionBlockerPiece] Getting active region blocker piece [{AllPrefabs.Length}]");
		int bossesDead = ConfigHelper.Instance.BossesDefeated;
		Log.LogDebug($"[GetActiveRegionBlockerPiece] Bosses dead [{bossesDead}]");
		GameObject blocker = _bossByIndex.GetValueSafe(bossesDead).Invoke();
		Log.LogDebug($"[GetActiveRegionBlockerPiece] Blocker [{blocker}] Changing material [{blocker.GetComponentInChildren<MeshRenderer>()}]");
		blocker.GetComponentInChildren<MeshRenderer>().material = bossesDead switch
		{
			// the reason for doing this is because the materials are massive if in our own asset bundle, 5MB+ total
			// so lets just use the already existing material in the game
			2 => PrefabConstants.Instance.WoodenBoxMaterial,
			3 => PrefabConstants.Instance.AncientStonesMaterial,
			_ => blocker.GetComponentInChildren<MeshRenderer>().material
		};
		
		Log.LogDebug($"[GetActiveRegionBlockerPiece] Returning blocker");
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
					PrefabConstants.Instance.GraveDiggerFigurine.gameObject,
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
					PrefabConstants.Instance.ChestPiece.gameObject,
					() => PrefabChestPiece
				)
			},
			{
				typeof(ChessboardElectricChairPiece), new Tuple<float, GameObject, Func<ChessboardPiece>>(
					18f,
					AllPrefabs.Single(go => go.name.Equals("SpecialNode_ElectricChair")),
					() => PrefabElectricChairPiece
				)
			},
			{
				typeof(ChessboardEnemyPiece), new Tuple<float, GameObject, Func<ChessboardPiece>>(
					1f,
					PrefabConstants.Instance.EnemyPiece.gameObject,
					() => PrefabEnemyPiece
				)
			},
			{
				typeof(ChessboardGoatEyePiece), new Tuple<float, GameObject, Func<ChessboardPiece>>(
					0.4f,
					PrefabConstants.Instance.GoatEye,
					() => PrefabGoatEyePiece
				)
			}
		};
	}

	private T CreateCustomPrefabPiece<T>() where T : ChessboardPiece
	{
		Log.LogDebug($"[CreateCustomPrefabPiece] Creating custom piece [{typeof(T)}]");
		if (!PieceSetupByType.TryGetValue(typeof(T), out Tuple<float, GameObject, Func<ChessboardPiece>> tuple))
		{
			throw new Exception($"[CreateCustomPrefabPiece] Failed to create [{typeof(T)}] as it does not exist in the dictionary!");
		}

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
}
