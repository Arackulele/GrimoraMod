﻿using DiskCardGame;
using HarmonyLib;
using UnityEngine;
using static GrimoraMod.GrimoraPlugin;

namespace GrimoraMod;

public class GrimoraChessboard
{

	public readonly int indexInList;
	public readonly ChessNode BossNode;

	protected internal ChessboardEnemyPiece BossPiece =>
		GetPieceAtSpace(BossNode.GridX, BossNode.GridY) as ChessboardEnemyPiece;

	public readonly List<ChessRow> Rows;


	public Opponent.Type ActiveBossType;


	public GrimoraChessboard(IEnumerable<List<int>> board, int indexInList)
	{
		this.Rows = board.Select((boardList, idx) => new ChessRow(boardList, idx)).ToList();
		this.BossNode = GetBossNode();
		this.indexInList = indexInList;
	}

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

	public void SetupBoard()
	{
		PlaceBossPiece(GetBossSpecialIdForRegion());
		PlacePieces<ChessboardBlockerPiece>();
		PlacePieces<ChessboardBoneyardPiece>();
		PlacePieces<ChessboardCardRemovePiece>();
		PlacePieces<ChessboardChestPiece>();
		PlacePieces<ChessboardElectricChairPiece>();
		PlacePieces<ChessboardEnemyPiece>();
		PlacePieces<ChessboardGoatEyePiece>();
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

	public ChessboardEnemyPiece PlaceBossPieceDev(int x, int y, string bossName)
	{
		return PlacePiece<ChessboardEnemyPiece>(x, y, bossName);
	}

	public ChessboardBossPiece PlaceBossPiece(string bossName)
	{
		return CreateChessPiece<ChessboardBossPiece>(
			ChessboardMapExt.Instance.PrefabPieceHelper.PrefabBossPiece, BossNode.GridX, BossNode.GridY, bossName
		);
	}

	public T PlacePiece<T>(int x, int y, string id = "", SpecialNodeData specialNodeData = null) where T : ChessboardPiece
	{
		// out ChessboardPiece prefabToUse
		ChessboardMapExt.Instance.PrefabPieceHelper
			.PieceSetupByType
			.TryGetValue(typeof(T), out Tuple<float, Func<GameObject>, Func<ChessboardPiece>> tuple);

		return CreateChessPiece<T>(tuple.Item3.Invoke(), x, y, id, specialNodeData);
	}

	public List<T> PlacePieces<T>() where T : ChessboardPiece
	{
		Type type = typeof(T);
		List<ChessNode> nodes = BlockerNodes;
		if (type == typeof(ChessboardEnemyPiece))
		{
			nodes = EnemyNodes;
		}
		else if (type == typeof(ChessboardChestPiece))
		{
			nodes = ChestNodes;
		}
		else if (type == typeof(ChessboardCardRemovePiece))
		{
			nodes = CardRemovalNodes;
		}

		return nodes.Select(node => PlacePiece<T>(node.GridX, node.GridY)).ToList();
	}

	#endregion

	#region CreatePieces

	public Mesh GetActiveRegionBlockerMesh()
	{
		Mesh meshObj = ActiveBossType switch
		{
			BaseBossExt.SawyerOpponent => MeshFilterBlockerBones,
			BaseBossExt.RoyalOpponent => MeshFilterBlockerBarrels,
			_ => MeshFilterBlockerIceBlock
		};

		return meshObj;
	}

	private T CreateChessPiece<T>(
		ChessboardPiece prefab,
		int x, int y,
		string id = "",
		SpecialNodeData specialNodeData = null) where T : ChessboardPiece
	{
		string coordName = $"x[{x}]y[{y}]";

		ChessboardPiece piece = GetPieceAtSpace(x, y);

		if (piece is not null)
		{
			Log.LogDebug($"[CreateChessPiece<{typeof(T).Name}>] Skipping x{x}y{y}, {piece.name} already exists");
			return piece as T;
		}

		piece = UnityEngine.Object.Instantiate(prefab, ChessboardMapExt.Instance.dynamicElementsParent);
		piece.gridXPos = x;
		piece.gridYPos = y;

		// ChessboardEnemyPiece => EnemyPiece_x[]y[]
		string nameTemp = piece.GetType().Name.Replace("Chessboard", "") + "_" + coordName;

		switch (piece)
		{
			case ChessboardEnemyPiece enemyPiece:
			{
				enemyPiece.GoalPosX = x;
				enemyPiece.GoalPosX = y;

				enemyPiece.specialEncounterId = id;

				if (!id.Equals("GrimoraModBattleSequencer"))
				{
					nameTemp = nameTemp.Replace("Enemy", "Boss");
					ActiveBossType = BaseBossExt.BossTypesByString.GetValueSafe(id);
					Log.LogDebug($"[CreateChessPiece] id is not null, setting ActiveBossType to [{id}]");
					enemyPiece.blueprint = BlueprintUtils.BossInitialBlueprints[id];
				}
				else
				{
					// Log.LogDebug($"[CreateChessPiece] id is null, getting blueprint");
					enemyPiece.blueprint = GetBlueprint();
				}

				break;
			}
		}

		if (specialNodeData is not null)
		{
			piece.NodeData = specialNodeData;
		}

		piece.name = nameTemp;

		// Log.LogDebug($"[CreatingPiece] {piece.name}");
		ChessboardMapExt.Instance.pieces.Add(piece);
		// ChessboardNavGrid.instance.zones[x, y].GetComponent<ChessboardMapNode>().OccupyingPiece = piece;
		return piece as T;
	}

	#endregion
}
