using DiskCardGame;
using HarmonyLib;
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
				new Tuple<Func<GameObject>, Func<List<ChessNode>>>(() => PrefabConstants.GraveDiggerFigurine, GetBoneyardNodes)
			},
			{
				typeof(ChessboardCardRemovePiece),
				new Tuple<Func<GameObject>, Func<List<ChessNode>>>(() => PrefabConstants.SkinningKnife, GetCardRemovalNodes)
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
				new Tuple<Func<GameObject>, Func<List<ChessNode>>>(() => PrefabConstants.EyeBall, GetGoatEyeNodes)
			}
		};
	}

	private readonly Dictionary<int, Func<GameObject>> _bossByIndex = new()
	{
		{ 0, () => AllPrefabs.Single(pb => pb.name.Equals("Blocker_Kaycee")) },
		{ 1, () => AllPrefabs.Single(pb => pb.name.Equals("Blocker_Sawyer")) },
		{ 2, () => AllPrefabs.Single(pb => pb.name.Equals("Blocker_Royal")) },
		{ 3, () => AllPrefabs.Single(pb => pb.name.Equals("Blocker_Grimora")) },
	};

	private readonly Dictionary<string, Opponent.Type> _bossBySpecialId = new()
	{
		{ KayceeBossOpponent.SpecialId, BaseBossExt.KayceeOpponent },
		{ SawyerBossOpponent.SpecialId, BaseBossExt.SawyerOpponent },
		{ RoyalBossOpponentExt.SpecialId, BaseBossExt.RoyalOpponent },
		{ GrimoraBossOpponentExt.SpecialId, BaseBossExt.GrimoraOpponent }
	};

	public GameObject GetActiveRegionBlockerPiece()
	{
		Log.LogDebug($"[GetActiveRegionBlockerPiece] Getting active region blocker piece [{AllPrefabs.Length}]");
		int bossesDead = ConfigHelper.Instance.BossesDefeated;
		Log.LogDebug($"[GetActiveRegionBlockerPiece] Bosses dead [{bossesDead}]");
		GameObject blocker = _bossByIndex.GetValueSafe(bossesDead).Invoke();
		Log.LogDebug($"[GetActiveRegionBlockerPiece] Blocker [{blocker}]");
		blocker.GetComponentInChildren<MeshRenderer>().material = bossesDead switch
		{
			// the reason for doing this is because the materials are massive if in our own asset bundle, 5MB+ total
			// so lets just use the already existing material in the game
			2 => PrefabConstants.WoodenBoxMaterial,
			3 => PrefabConstants.AncientStonesMaterial,
			_ => blocker.GetComponentInChildren<MeshRenderer>().material
		};

		Log.LogDebug($"[GetActiveRegionBlockerPiece] Returning blocker");
		return blocker;
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
		switch (ConfigHelper.Instance.BossesDefeated)
		{
			case 1:
				Log.LogDebug($"[GetBossSpecialIdForRegion] Kaycee defeated");
				return SawyerBossOpponent.SpecialId;
			case 2:
				Log.LogDebug($"[GetBossSpecialIdForRegion] Sawyer defeated");
				return RoyalBossOpponentExt.SpecialId;
			case 3:
				Log.LogDebug($"[GetBossSpecialIdForRegion] Royal defeated");
				return GrimoraBossOpponentExt.SpecialId;
			default:
				Log.LogDebug($"[GetBossSpecialIdForRegion] No bosses defeated yet, creating Kaycee");
				return KayceeBossOpponent.SpecialId;
		}
	}

	#endregion

	public void SetupBoard()
	{
		Log.LogDebug($"[SetupBoard]");
		PlaceBossPiece(GetBossSpecialIdForRegion());
		PlacePieces<ChessboardBlockerPieceExt>();
		PlacePieces<ChessboardBoneyardPiece>();
		PlacePieces<ChessboardCardRemovePiece>();
		PlacePieces<ChessboardChestPiece>();
		PlacePieces<ChessboardElectricChairPiece>();
		PlacePieces<ChessboardEnemyPiece>();
		PlacePieces<ChessboardGoatEyePiece>();
	}

	public void UpdatePlayerMarkerPosition(bool changingRegion)
	{
		int x = GrimoraSaveData.Data.gridX;
		int y = GrimoraSaveData.Data.gridY;

		Log.LogDebug($"[HandlePlayerMarkerPosition] " +
		             $"Player Marker name [{PlayerMarker.Instance.name}] " +
		             $"x{x}y{y} coords");

		var occupyingPiece = GetPieceAtSpace(x, y);

		bool isPlayerOccupied = occupyingPiece is not null && PlayerMarker.Instance.name == occupyingPiece.name;

		Log.LogDebug($"[HandlePlayerMarkerPosition] isPlayerOccupied? [{isPlayerOccupied}]");

		if (changingRegion || !StoryEventsData.EventCompleted(StoryEvent.GrimoraReachedTable))
		{
			// the PlayerNode will be different since this is now a different chessboard
			x = GetPlayerNode().GridX;
			y = GetPlayerNode().GridY;
		}

		MapNodeManager.Instance.ActiveNode = ChessboardNavGrid.instance.zones[x, y].GetComponent<MapNode>();
		Log.LogDebug($"[SetupGamePieces] MapNodeManager ActiveNode is x[{x}]y[{y}]");

		Log.LogDebug($"[SetupGamePieces] SetPlayerAdjacentNodesActive");
		ChessboardNavGrid.instance.SetPlayerAdjacentNodesActive();

		Log.LogDebug($"[SetupGamePieces] Setting player position to active node");
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

	private EncounterBlueprintData GetBlueprint()
	{
		// Log.LogDebug($"[GetBlueprint] ActiveBoss [{ActiveBossType}]");
		var blueprints = BlueprintUtils.RegionWithBlueprints[ActiveBossType];
		return blueprints[UnityEngine.Random.RandomRangeInt(0, blueprints.Count)];
	}

	#endregion

	#region PlacingPieces

	public ChessboardEnemyPiece PlaceBossPiece(string bossName, int x = -1, int y = -1)
	{
		int newX = x == -1 ? BossNode.GridX : x;
		int newY = x == -1 ? BossNode.GridY : y;
		return CreateChessPiece<ChessboardEnemyPiece>(
			PrefabConstants.BossPiece.gameObject,
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

	public List<T> PlacePieces<T>() where T : ChessboardPiece
	{
		if (!_nodesByPieceType.TryGetValue(typeof(T), out Tuple<Func<GameObject>, Func<List<ChessNode>>> tuple))
		{
			throw new Exception($"Unable to find piece of type [{typeof(T)}] in _nodesByPieceType!");
		}

		List<ChessNode> nodes = tuple.Item2.Invoke();

		return nodes.Select(node => PlacePiece<T>(node.GridX, node.GridY)).ToList();
	}

	#endregion

	#region CreatePieces

	private T CreateChessPiece<T>(
		GameObject prefab,
		int x, int y,
		string id = "",
		SpecialNodeData specialNodeData = null) where T : ChessboardPiece
	{
		string coordName = $"x[{x}]y[{y}]";

		if (GetPieceAtSpace(x, y) is not null)
		{
			Log.LogDebug($"[CreateChessPiece<{typeof(T).Name}>] Skipping x{x}y{y}");
			return GetPieceAtSpace(x, y) as T;
		}

		GameObject pieceObj = Object.Instantiate(prefab, ChessboardMapExt.Instance.dynamicElementsParent);

		// ChessboardEnemyPiece => EnemyPiece_x[]y[]

		switch (pieceObj.GetComponent<T>())
		{
			case ChessboardEnemyPiece enemyPiece:
			{
				enemyPiece.GoalPosX = x;
				enemyPiece.GoalPosX = y;
				if (prefab.name.Contains("Boss"))
				{
					Log.LogDebug($"[CreateChessPiece] Setting ActiveBossType to [{id}]");
					ActiveBossType = _bossBySpecialId.GetValueSafe(id);
					enemyPiece.blueprint = BlueprintUtils.BossInitialBlueprints[id];
				}
				else
				{
					id = "GrimoraModBattleSequencer";
					enemyPiece.blueprint = GetBlueprint();
				}

				enemyPiece.specialEncounterId = id;
				break;
			}
			default:
				if (pieceObj.GetComponent<T>() is null)
				{
					Log.LogDebug($"[CreateChessPiece] Adding type [{typeof(T).Name}] to [{prefab}]");
					pieceObj.AddComponent<T>();
				}

				if (typeof(T) == typeof(ChessboardGoatEyePiece))
				{
					pieceObj.GetComponent<MeshRenderer>().material = PrefabConstants.GoatEyeMat;
					pieceObj.GetComponent<MeshRenderer>().sharedMaterial = PrefabConstants.GoatEyeMat;
					pieceObj.GetComponent<Rigidbody>().useGravity = false;
				}

				break;
		}

		ChessboardPiece piece = pieceObj.GetComponent<T>();

		piece.gridXPos = x;
		piece.gridYPos = y;

		if (specialNodeData is not null)
		{
			piece.NodeData = specialNodeData;
		}

		piece.anim ??= PrefabConstants.EnemyPiece.anim;

		string nameTemp = typeof(T).Name.Replace("Chessboard", "") + "_" + coordName;
		if (prefab.name.Contains("boss"))
		{
			nameTemp = nameTemp.Replace("Enemy", "Boss");
		}

		piece.name = nameTemp;

		// Log.LogDebug($"[CreateChessPiece] {piece.name}");
		ChessboardMapExt.Instance.pieces.Add(piece);
		// ChessboardNavGrid.instance.zones[x, y].GetComponent<ChessboardMapNode>().OccupyingPiece = piece;
		return (T)piece;
	}

	#endregion
}
