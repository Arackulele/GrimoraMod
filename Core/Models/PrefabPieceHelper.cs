using DiskCardGame;
using HarmonyLib;
using Sirenix.Utilities;
using UnityEngine;
using static GrimoraMod.GrimoraPlugin;

namespace GrimoraMod;

public class PrefabPieceHelper
{
	public const string PrefabPath = "Prefabs/Map/ChessboardMap";
	public const string PrefabPathSpecialNodes = "Prefabs/SpecialNodeSequences";
	public const string PrefabPathArt3D = "Art/Assets3D";

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
		PrefabBossPiece = ResourceBank.Get<ChessboardEnemyPiece>($"{PrefabPath}/BossFigurine");
		PrefabChestPiece = ResourceBank.Get<ChessboardChestPiece>($"{PrefabPath}/ChessboardChestPiece");
		PrefabEnemyPiece = ResourceBank.Get<ChessboardEnemyPiece>($"{PrefabPath}/ChessboardEnemyPiece");

		PieceSetupByType = BuildDictionary();
		PrefabBlockerPiece = CreateCustomPrefabPiece<ChessboardBlockerPiece>();
		PrefabBoneyardPiece = CreateCustomPrefabPiece<ChessboardBoneyardPiece>();
		PrefabCardRemovePiece = CreateCustomPrefabPiece<ChessboardCardRemovePiece>();
		PrefabElectricChairPiece = CreateCustomPrefabPiece<ChessboardElectricChairPiece>();
		PrefabGoatEyePiece = CreateCustomPrefabPiece<ChessboardGoatEyePiece>();
	}

	public static GameObject GetActiveRegionBlockerPiece()
	{
		int bossesDead = ConfigHelper.Instance.BossesDefeated;
		Opponent.Type oppType = BaseBossExt.BossByIndex.GetValueSafe(bossesDead).Item1; 
		Log.LogDebug($"[GetActiveRegionBlockerPiece] Getting region piece, bosses defeated [{bossesDead}] Opp {oppType}");
		return oppType switch
			{
				BaseBossExt.SawyerOpponent => AllPrefabAssets.Single(pb => pb.name.Contains("Sawyer")),
				BaseBossExt.RoyalOpponent => AllPrefabAssets.Single(pb => pb.name.Contains("Royal")),
				_ => AllPrefabAssets.Single(pb => pb.name.Contains("Kaycee")),
			};
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
					() => ResourceBank.Get<GameObject>($"{PrefabPathArt3D}/PlayerAvatar/gravedigger/GravediggerFin"),
					() => PrefabBoneyardPiece
				)
			},
			{
				typeof(ChessboardBossPiece),
				new Tuple<float, Func<GameObject>, Func<ChessboardPiece>>(
					1f,
					() => ResourceBank.Get<ChessboardEnemyPiece>($"{PrefabPath}/BossFigurine").gameObject,
					() => PrefabBossPiece
				)
			},
			{
				typeof(ChessboardCardRemovePiece), new Tuple<float, Func<GameObject>, Func<ChessboardPiece>>(
					0.25f,
					() => ResourceBank.Get<GameObject>($"{PrefabPathSpecialNodes}/SkinningKnife"),
					() => PrefabCardRemovePiece
				)
			},
			{
				typeof(ChessboardChestPiece), new Tuple<float, Func<GameObject>, Func<ChessboardPiece>>(
					1f,
					() => ResourceBank.Get<ChessboardChestPiece>($"{PrefabPath}/ChessboardChestPiece").gameObject,
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
					() => ResourceBank.Get<ChessboardEnemyPiece>($"{PrefabPath}/ChessboardEnemyPiece").gameObject,
					() => PrefabEnemyPiece
				)
			},
			{
				typeof(ChessboardGoatEyePiece), new Tuple<float, Func<GameObject>, Func<ChessboardPiece>>(
					0.4f,
					() => ResourceBank.Get<GameObject>($"{PrefabPathSpecialNodes}/EyeBall"),
					() => PrefabGoatEyePiece
				)
			},
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
