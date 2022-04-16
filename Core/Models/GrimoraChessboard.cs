using DiskCardGame;
using HarmonyLib;
using Sirenix.Utilities;
using UnityEngine;
using static GrimoraMod.GrimoraPlugin;

namespace GrimoraMod;

public class GrimoraChessboard
{
	private readonly Dictionary<Type, Tuple<Func<GameObject>, Func<List<ChessNode>>>> _nodesByPieceType;

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
				new Tuple<Func<GameObject>, Func<List<ChessNode>>>(() => AssetConstants.BoneyardFigurine, GetBoneyardNodes)
			},
			{
				typeof(ChessboardCardRemovePiece),
				new Tuple<Func<GameObject>, Func<List<ChessNode>>>(
					() => AssetConstants.CardRemovalFigurine,
					GetCardRemovalNodes
				)
			},
			{
				typeof(ChessboardChestPiece),
				new Tuple<Func<GameObject>, Func<List<ChessNode>>>(() => AssetConstants.ChestPiece.gameObject, GetChestNodes)
			},
			{
				typeof(ChessboardElectricChairPiece),
				new Tuple<Func<GameObject>, Func<List<ChessNode>>>(() => AssetConstants.ElectricChairFigurine, GetElectricChairNodes)
			},
			{
				typeof(ChessboardEnemyPiece),
				new Tuple<Func<GameObject>, Func<List<ChessNode>>>(() => AssetConstants.EnemyPiece.gameObject, GetEnemyNodes)
			},
			{
				typeof(ChessboardGoatEyePiece),
				new Tuple<Func<GameObject>, Func<List<ChessNode>>>(() => AssetConstants.GoatEyeFigurine, GetGoatEyeNodes)
			}
		};
	}

	private readonly Dictionary<int, Func<GameObject>> _bossByIndex = new()
	{
		{ 0, () => AssetUtils.GetPrefab<GameObject>("Blocker_Kaycee") },
		{ 1, () => AssetUtils.GetPrefab<GameObject>("Sawyer_BlockerPiece") },
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
			2 => AssetConstants.WoodenBoxMaterial,
			3 => AssetConstants.AncientStonesMaterial,
			_ => blockerPrefab.GetComponentInChildren<MeshRenderer>().material
		};

		return blockerPrefab;
	}

	private string _activeBossId;
	public readonly int indexInList;
	public readonly ChessNode BossNode;

	protected internal ChessboardEnemyPiece BossPiece =>
		GetPieceAtSpace(BossNode.GridX, BossNode.GridY) as ChessboardEnemyPiece;

	public readonly List<ChessRow> Rows;

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
		Log.LogDebug($"Getting special id for region");
		return BossHelper.OpponentTupleBySpecialId.ElementAt(ConfigHelper.Instance.BossesDefeated).Key;
	}

	#endregion

	public void SetupBoard(bool placePieces)
	{
		if (placePieces)
		{
			PlaceBossPiece();
			PlacePieces<ChessboardBlockerPieceExt>();
			PlacePieces<ChessboardBoneyardPiece>();
			PlacePieces<ChessboardCardRemovePiece>();
			PlacePieces<ChessboardChestPiece>();
			PlacePieces<ChessboardElectricChairPiece>();
			PlacePieces<ChessboardEnemyPiece>(GrimoraModBattleSequencer.ID);
			PlacePieces<ChessboardGoatEyePiece>();
		}
	}

	public void UpdatePlayerMarkerPosition(bool changingRegion)
	{
		if (GrimoraSaveData.Data.gridX == -1)
		{
			SetSavePositions();
		}
		
		int x = GrimoraSaveData.Data.gridX;
		int y = GrimoraSaveData.Data.gridY;

		ChessboardMapNode nodeAtSpace = GetNodeAtSpace(x, y);

		bool pieceAtSpaceIsNotPlayer = nodeAtSpace.OccupyingPiece
		                               && nodeAtSpace.OccupyingPiece.GetType() != typeof(PlayerMarker)
		                               || !nodeAtSpace.isActiveAndEnabled;

		// this is the final possible spawning condition that I can think of.
		bool hasNotInteractedWithAnyPiece =
			!ConfigHelper.Instance.RemovedPieces.Exists(
				piece => piece.Contains("EnemyPiece_x") || piece.Contains("ChestPiece_x")
			);

		if (changingRegion || pieceAtSpaceIsNotPlayer || hasNotInteractedWithAnyPiece)
		{
			Log.LogDebug(
				$"[UpdatePlayerMarkerPosition] "
				+ $"Changing region? [{changingRegion}] "
				+ $"PieceAtSpaceIsNotPlayer? [{pieceAtSpaceIsNotPlayer}] Piece is [{nodeAtSpace.OccupyingPiece}]"
				+ $"hasNotInteractedWithAnyPiece? [{hasNotInteractedWithAnyPiece}]"
			);
			// the PlayerNode will be different since this is now a different chessboard
			SetSavePositions();
			x = GrimoraSaveData.Data.gridX;
			y = GrimoraSaveData.Data.gridY;
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

	public ChessboardEnemyPiece PlaceBossPiece(string specialSequencerId = "", int x = -1, int y = -1)
	{
		if (specialSequencerId.IsNullOrWhitespace())
		{
			specialSequencerId = GetBossSpecialIdForRegion();
		}

		Log.LogDebug($"Boss name to place piece for [{specialSequencerId}]");
		_activeBossId = specialSequencerId;
		GameObject prefabToUse = BossHelper.OpponentTupleBySpecialId[specialSequencerId].Item2;
		int newX = x == -1 ? BossNode.GridX : x;
		int newY = x == -1 ? BossNode.GridY : y;
		return CreateChessPiece<ChessboardEnemyPiece>(
			prefabToUse,
			newX,
			newY,
			specialSequencerId
		);
	}

	public T PlacePiece<T>(int x, int y, string id = "", SpecialNodeData specialNodeData = null) where T : ChessboardPiece
	{
		return CreateChessPiece<T>(
			_nodesByPieceType.GetValueSafe(typeof(T)).Item1.Invoke(),
			x,
			y,
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
		int x,
		int y,
		string specialEncounterId = "",
		SpecialNodeData specialNodeData = null
	) where T : ChessboardPiece
	{
		string coordName = $"x[{x}]y[{y}]";

		ChessboardPiece piece = ChessboardMapExt.Instance.pieces.Find(p => p.gridXPos == x && p.gridYPos == y);

		if (piece)
		{
			// Log.LogDebug($"[CreateChessPiece<{typeof(T).Name}>] Skipping x{x}y{y}");
			return piece.GetComponent<T>();
		}

		piece = HandlePieceSetup<T>(prefab, specialEncounterId);

		if (piece.anim.IsNull() && piece.transform.Find("Anim"))
		{
			piece.anim = piece.transform.Find("Anim").GetComponent<Animator>();
		}

		if (typeof(T) == typeof(ChessboardBlockerPieceExt))
		{
			piece.anim = AssetConstants.EnemyPiece.anim;
		}

		piece.gridXPos = x;
		piece.gridYPos = y;

		if (specialNodeData != null)
		{
			piece.NodeData = specialNodeData;
		}

		piece.name = CreateNameOfPiece<T>(specialEncounterId, coordName);

		ChessboardMapExt.Instance.pieces.Add(piece);
		return (T)piece;
	}

	private static string CreateNameOfPiece<T>(string specialEncounterId, string coordName) where T : ChessboardPiece
	{
		string nameTemp = typeof(T).Name.Replace("Chessboard", string.Empty) + "_" + coordName;
		if (BossHelper.OpponentTupleBySpecialId.ContainsKey(specialEncounterId))
		{
			nameTemp = nameTemp.Replace("Enemy", "Boss");
		}

		return nameTemp;
	}

	private T HandlePieceSetup<T>(GameObject prefab, string specialEncounterId = "") where T : ChessboardPiece
	{
		GameObject pieceObj = UnityObject.Instantiate(prefab, ChessboardMapExt.Instance.dynamicElementsParent);

		if (pieceObj.GetComponent<T>().IsNull())
		{
			pieceObj.AddComponent<T>();
		}

		switch (pieceObj.GetComponent<T>())
		{
			case ChessboardEnemyPiece enemyPiece:
			{
				if (BossHelper.OpponentTupleBySpecialId.ContainsKey(specialEncounterId))
				{
					// enemyPiece.blueprint = BossHelper.OpponentTupleBySpecialId[specialEncounterId].Item3;
					enemyPiece.blueprint = GetBlueprint(true);
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
							GameObject head = enemyPiece.transform.GetChild(0).Find("Head").gameObject;
							if (head.GetComponent<SineWaveMovement>().IsNull())
							{
								SineWaveMovement wave = head.AddComponent<SineWaveMovement>();
								wave.speed = 1;
								wave.xMagnitude = 0;
								wave.yMagnitude = 0.1f;
								wave.zMagnitude = 0;
							}

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

	private EncounterBlueprintData GetBlueprint(bool isForBoss = false)
	{
		switch (ConfigHelper.Instance.EncounterBlueprintType)
		{
			case BlueprintTypeForEncounter.Randomized:
			{
				return BlueprintUtils.BuildRandomBlueprint();
			}
			case BlueprintTypeForEncounter.CustomOnly:
			{
				if (isForBoss)
				{
					if (ChessboardMapExt.Instance.CustomBlueprintsBosses.Keys.ToList().Exists(json => json.bossName == _activeBossId))
					{
						Log.LogDebug($"[GetBlueprint.ForBoss] -> Active boss id exists in custom blueprints [{_activeBossId}]");
						return ChessboardMapExt.Instance
						 .CustomBlueprintsBosses
						 .Single(kv => kv.Key.bossName == _activeBossId)
						 .Value;
					}
					Log.LogDebug($"[GetBlueprint.ForBoss] -> Active boss id does not exist in custom blueprints, getting base mod blueprints.");
					return BossHelper.OpponentTupleBySpecialId[_activeBossId].Item3; 
				}
				
				if (ChessboardMapExt.Instance.CustomBlueprintsRegions.Keys.ToList().Exists(json => json.bossName == _activeBossId))
				{
					Log.LogDebug($"[GetBlueprint.NormalEnemy] -> Custom blueprints exist for regions, getting [{_activeBossId}] region blueprint.");
					return ChessboardMapExt.Instance.CustomBlueprintsRegions
					 .Where(k => k.Key.bossName == _activeBossId)
					 .Select(k => k.Value)
					 .ToList()
					 .GetRandomItem();
				}

				Log.LogWarning($"Unable to find custom blueprint for boss [{_activeBossId}], defaulting to base mod blueprints");
				return BlueprintUtils.GetRandomBlueprintForRegion();
			}
			case BlueprintTypeForEncounter.Mixed:
			{
				return BlueprintUtils
				 .RegionWithBlueprints
				 .ElementAt(ConfigHelper.Instance.BossesDefeated).Value
				 .Concat(ChessboardMapExt.Instance.CustomBlueprints.Values)
				 .ToList()
				 .GetRandomItem();
			}
			default:
			{
				return isForBoss 
					       ? BossHelper.OpponentTupleBySpecialId[_activeBossId].Item3 
					       : BlueprintUtils.GetRandomBlueprintForRegion();
			}
		}
		
	}

	#endregion
}
