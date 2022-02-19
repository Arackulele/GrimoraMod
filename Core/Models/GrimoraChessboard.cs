using DiskCardGame;
using HarmonyLib;
using Sirenix.Utilities;
using UnityEngine;
using static GrimoraMod.GrimoraPlugin;

namespace GrimoraMod;

public class GrimoraChessboard
{
	private readonly Dictionary<System.Type, Tuple<Func<GameObject>, Func<List<ChessNode>>>> _nodesByPieceType;

	private Dictionary<Type, Tuple<Func<GameObject>, Func<List<ChessNode>>>> BuildDictionary()
	{
		return new Dictionary<Type, Tuple<Func<GameObject>, Func<List<ChessNode>>>>
		{
			{
				typeof(ChessboardBlockerPieceExt),
				new Tuple<Func<GameObject>, Func<List<ChessNode>>>(GetActiveRegionBlockerPiece, GetBlockerNodes)
			},
			{
				typeof(ChessboardBoneyardPiece),
				new Tuple<Func<GameObject>, Func<List<ChessNode>>>(() => PrefabConstants.BoneyardFigurine, GetBoneyardNodes)
			},
			{
				typeof(ChessboardCardRemovePiece),
				new Tuple<Func<GameObject>, Func<List<ChessNode>>>(() => PrefabConstants.CardRemovalFigurine,
					GetCardRemovalNodes)
			},
			{
				typeof(ChessboardChestPiece),
				new Tuple<Func<GameObject>, Func<List<ChessNode>>>(() => PrefabConstants.ChestPiece.gameObject, GetChestNodes)
			},
			{
				typeof(ChessboardElectricChairPiece),
				new Tuple<Func<GameObject>, Func<List<ChessNode>>>(() => PrefabConstants.ElectricChair, GetElectricChairNodes)
			},
			{
				typeof(ChessboardEnemyPiece),
				new Tuple<Func<GameObject>, Func<List<ChessNode>>>(() => PrefabConstants.EnemyPiece.gameObject, GetEnemyNodes)
			},
			{
				typeof(ChessboardGoatEyePiece),
				new Tuple<Func<GameObject>, Func<List<ChessNode>>>(() => PrefabConstants.GoatEyeFigurine, GetGoatEyeNodes)
			}
		};
	}

	private readonly Dictionary<int, Func<GameObject>> _bossByIndex = new()
	{
		{ 0, () => AssetUtils.GetPrefab<GameObject>("Blocker_Kaycee") },
		{ 1, () => AssetUtils.GetPrefab<GameObject>("Blocker_Sawyer") },
		{ 2, () => AssetUtils.GetPrefab<GameObject>("Blocker_Royal") },
		{ 3, () => AssetUtils.GetPrefab<GameObject>("Blocker_Grimora") },
	};


	public GameObject GetActiveRegionBlockerPiece()
	{
		int bossesDead = ConfigHelper.Instance.BossesDefeated;
		GameObject blockerPrefab = _bossByIndex.GetValueSafe(bossesDead).Invoke();
		blockerPrefab.GetComponentInChildren<MeshRenderer>().material = bossesDead switch
		{
			// the reason for doing this is because the materials are massive if in our own asset bundle, 5MB+ total
			// so lets just use the already existing material in the game
			2 => PrefabConstants.WoodenBoxMaterial,
			3 => PrefabConstants.AncientStonesMaterial,
			_ => blockerPrefab.GetComponentInChildren<MeshRenderer>().material
		};

		return blockerPrefab;
	}

	public readonly int indexInList;
	public readonly ChessNode BossNode;

	protected internal ChessboardEnemyPiece BossPiece =>
		GetPieceAtSpace(BossNode.GridX, BossNode.GridY) as ChessboardEnemyPiece;

	public readonly List<ChessRow> Rows;


	public Opponent.Type ActiveBossType;


	public GrimoraChessboard(IEnumerable<List<int>> board, int indexInList)
	{
		Rows = board.Select((boardList, idx) => new ChessRow(boardList, idx)).ToList();
		BossNode = GetBossNode();
		this.indexInList = indexInList;
		_nodesByPieceType = BuildDictionary();
	}

	#region Getters

	public List<ChessNode> GetOpenPathNodes()
	{
		return Rows.SelectMany(row => row.GetNodesOfType(0)).ToList();
	}

	private List<ChessNode> GetBlockerNodes()
	{
		return Rows.SelectMany(row => row.GetNodesOfType(1)).ToList();
	}

	private List<ChessNode> GetChestNodes()
	{
		return Rows.SelectMany(row => row.GetNodesOfType(2)).ToList();
	}

	private List<ChessNode> GetEnemyNodes()
	{
		return Rows.SelectMany(row => row.GetNodesOfType(3)).ToList();
	}

	private ChessNode GetBossNode()
	{
		return Rows.SelectMany(row => row.GetNodesOfType(4)).Single();
	}

	private List<ChessNode> GetCardRemovalNodes()
	{
		return Rows.SelectMany(row => row.GetNodesOfType(5)).ToList();
	}

	private List<ChessNode> GetBoneyardNodes()
	{
		return Rows.SelectMany(row => row.GetNodesOfType(6)).ToList();
	}

	private List<ChessNode> GetElectricChairNodes()
	{
		return Rows.SelectMany(row => row.GetNodesOfType(7)).ToList();
	}

	private List<ChessNode> GetGoatEyeNodes()
	{
		return Rows.SelectMany(row => row.GetNodesOfType(8)).ToList();
	}

	public ChessNode GetPlayerNode()
	{
		return Rows.SelectMany(row => row.GetNodesOfType(9)).Single();
	}

	public static string GetBossSpecialIdForRegion()
	{
		return BaseBossExt.OpponentTupleBySpecialId.ElementAt(ConfigHelper.Instance.BossesDefeated).Key;
	}

	#endregion

	public void SetupBoard()
	{
		PlaceBossPiece();
		PlacePieces<ChessboardBlockerPieceExt>();
		PlacePieces<ChessboardBoneyardPiece>();
		PlacePieces<ChessboardCardRemovePiece>();
		PlacePieces<ChessboardChestPiece>();
		PlacePieces<ChessboardElectricChairPiece>();
		PlacePieces<ChessboardEnemyPiece>("GrimoraModBattleSequencer");
		PlacePieces<ChessboardGoatEyePiece>();
	}

	public void UpdatePlayerMarkerPosition(bool changingRegion)
	{
		int x = GrimoraSaveData.Data.gridX;
		int y = GrimoraSaveData.Data.gridY;

		ChessboardMapNode nodeAtSpace = GetNodeAtSpace(x, y);

		bool pieceAtSpaceIsNotPlayer = nodeAtSpace.OccupyingPiece is not null
		                               && nodeAtSpace.OccupyingPiece.GetType() != typeof(PlayerMarker)
		                               || !nodeAtSpace.isActiveAndEnabled;

		if (changingRegion || !StoryEventsData.EventCompleted(StoryEvent.GrimoraReachedTable) || pieceAtSpaceIsNotPlayer)
		{
			// the PlayerNode will be different since this is now a different chessboard
			x = GetPlayerNode().GridX;
			y = GetPlayerNode().GridY;
			GrimoraSaveData.Data.gridX = x;
			GrimoraSaveData.Data.gridY = y;
			Log.LogDebug($"[UpdatePlayerMarkerPosition] New x{x}y{y} coords");
		}

		MapNodeManager.Instance.ActiveNode = ChessboardNavGrid.instance.zones[x, y].GetComponent<MapNode>();

		ChessboardNavGrid.instance.SetPlayerAdjacentNodesActive();

		PlayerMarker.Instance.transform.position = MapNodeManager.Instance.ActiveNode.transform.position;
	}

	public void SetSavePositions()
	{
		// set the updated position to spawn the player in
		GrimoraSaveData.Data.gridX = GetPlayerNode().GridX;
		GrimoraSaveData.Data.gridY = GetPlayerNode().GridY;
	}

	#region HelperMethods

	public static ChessboardMapNode GetNodeAtSpace(int x, int y)
	{
		return ChessboardNavGrid.instance.zones[x, y].GetComponent<ChessboardMapNode>();
	}

	public static ChessboardPiece GetPieceAtSpace(int x, int y)
	{
		// Log.LogDebug($"[GetPieceAtSpace] Getting piece at space x{x}y{y}");
		return GetNodeAtSpace(x, y).OccupyingPiece;
	}

	#endregion

	#region PlacingPieces

	public ChessboardEnemyPiece PlaceBossPiece(string bossName = "", int x = -1, int y = -1)
	{
		if (bossName.IsNullOrWhitespace())
		{
			bossName = GetBossSpecialIdForRegion();
		}

		GameObject prefabToUse = BaseBossExt.OpponentTupleBySpecialId[bossName].Item3;
		int newX = x == -1 ? BossNode.GridX : x;
		int newY = x == -1 ? BossNode.GridY : y;
		return CreateChessPiece<ChessboardEnemyPiece>(
			prefabToUse,
			newX,
			newY,
			bossName
		);
	}

	public T PlacePiece<T>(int x, int y, string id = "", SpecialNodeData specialNodeData = null) where T : ChessboardPiece
	{
		return CreateChessPiece<T>(
			_nodesByPieceType.GetValueSafe(typeof(T)).Item1.Invoke(),
			x, y,
			id,
			specialNodeData
		);
	}

	public List<T> PlacePieces<T>(string specialEncounterId = "") where T : ChessboardPiece
	{
		if (!_nodesByPieceType.TryGetValue(typeof(T), out Tuple<Func<GameObject>, Func<List<ChessNode>>> tuple))
		{
			throw new Exception($"Unable to find piece of type [{typeof(T)}] in _nodesByPieceType!");
		}

		List<ChessNode> nodes = tuple.Item2.Invoke();

		return nodes.Select(node => PlacePiece<T>(node.GridX, node.GridY, specialEncounterId)).ToList();
	}

	#endregion

	#region CreatePieces

	private T CreateChessPiece<T>(
		GameObject prefab,
		int x, int y,
		string specialEncounterId = "",
		SpecialNodeData specialNodeData = null) where T : ChessboardPiece
	{
		string coordName = $"x[{x}]y[{y}]";

		ChessboardPiece piece = ChessboardMapExt.Instance.pieces.Find(p => p.gridXPos == x && p.gridYPos == y);

		if (piece is not null)
		{
			// Log.LogDebug($"[CreateChessPiece<{typeof(T).Name}>] Skipping x{x}y{y}");
			return piece.GetComponent<T>();
		}

		piece = HandlePieceSetup<T>(prefab, specialEncounterId);

		if (piece.anim is null && piece.transform.Find("Anim") is not null)
		{
			piece.anim = piece.transform.Find("Anim").GetComponent<Animator>();
		}

		if (typeof(T) == typeof(ChessboardBlockerPieceExt))
		{
			piece.anim = PrefabConstants.EnemyPiece.anim;
		}

		piece.gridXPos = x;
		piece.gridYPos = y;

		if (specialNodeData is not null)
		{
			piece.NodeData = specialNodeData;
		}

		piece.name = CreateNameOfPiece<T>(specialEncounterId, coordName);

		ChessboardMapExt.Instance.pieces.Add(piece);
		return (T)piece;
	}

	private static string CreateNameOfPiece<T>(string specialEncounterId, string coordName) where T : ChessboardPiece
	{
		string nameTemp = typeof(T).Name.Replace("Chessboard", "") + "_" + coordName;
		if (specialEncounterId.Contains("Boss"))
		{
			nameTemp = nameTemp.Replace("Enemy", "Boss");
		}

		return nameTemp;
	}

	private T HandlePieceSetup<T>(GameObject prefab, string specialEncounterId = "") where T : ChessboardPiece
	{
		GameObject pieceObj = Object.Instantiate(prefab, ChessboardMapExt.Instance.dynamicElementsParent);

		if (pieceObj.GetComponent<T>() is null)
		{
			pieceObj.AddComponent<T>();
		}

		switch (pieceObj.GetComponent<T>())
		{
			case ChessboardEnemyPiece enemyPiece:
			{
				if (specialEncounterId.Contains("Boss"))
				{
					ActiveBossType = BaseBossExt.OpponentTupleBySpecialId.GetValueSafe(specialEncounterId).Item1;
					enemyPiece.blueprint = BaseBossExt.OpponentTupleBySpecialId[specialEncounterId].Item4;
					int bossesDefeated = ConfigHelper.Instance.BossesDefeated;
					switch (bossesDefeated)
					{
						case 3:
							// have to set the scale since the Grimora anim prefab is much larger
							enemyPiece.transform.localScale = new Vector3(0.4f, 0.4f, 0.4f);
							enemyPiece.transform.Rotate(new Vector3(0, 215, 0));
							break;
						default:
							enemyPiece.transform.localRotation = Quaternion.Euler(0, 90, 0);
							break;
					}
				}
				else
				{
					enemyPiece.blueprint = GetBlueprint();
				}

				enemyPiece.specialEncounterId = specialEncounterId;
				break;
			}
		}

		return pieceObj.GetComponent<T>();
	}

	private static EncounterBlueprintData GetBlueprint()
	{
		var blueprints
			= BlueprintUtils.RegionWithBlueprints[ChessboardMapExt.Instance.ActiveChessboard.ActiveBossType];
		return blueprints[UnityEngine.Random.RandomRangeInt(0, blueprints.Count)];
	}

	#endregion
}
