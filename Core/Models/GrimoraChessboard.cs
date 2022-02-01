using DiskCardGame;
using HarmonyLib;
using UnityEngine;
using static GrimoraMod.GrimoraPlugin;

namespace GrimoraMod;

public class GrimoraChessboard
{
	#region Prefabs

	public const string PrefabPath = "Prefabs/Map/ChessboardMap";

	public static Mesh MeshFilterBlockerIceBlock => AllAssets[8] as Mesh;
	public static Mesh MeshFilterBlockerBones => AllAssets[5] as Mesh;
	public static Mesh MeshFilterBlockerBarrels => AllAssets[2] as Mesh;

	public static ChessboardBlockerPiece PrefabTombstone =>
		ResourceBank.Get<ChessboardBlockerPiece>($"{PrefabPath}/Chessboard_Tombstone_1");

	public static ChessboardEnemyPiece PrefabEnemyPiece =>
		ResourceBank.Get<ChessboardEnemyPiece>($"{PrefabPath}/ChessboardEnemyPiece");

	public static ChessboardEnemyPiece PrefabBossPiece =>
		ResourceBank.Get<ChessboardEnemyPiece>($"{PrefabPath}/BossFigurine");

	public static ChessboardChestPiece PrefabChestPiece =>
		ResourceBank.Get<ChessboardChestPiece>($"{PrefabPath}/ChessboardChestPiece");

	public static ChessboardBoneyardPiece PrefabBoneyardPiece = CreateCustomPrefabPiece<ChessboardBoneyardPiece>();

	public static ChessboardCardRemovePiece PrefabCardRemovePiece = CreateCustomPrefabPiece<ChessboardCardRemovePiece>();

	public static ChessboardElectricChairPiece PrefabElectricChairPiece
		= CreateCustomPrefabPiece<ChessboardElectricChairPiece>();

	public static ChessboardGoatEyePiece PrefabGoatEyePiece = CreateCustomPrefabPiece<ChessboardGoatEyePiece>();

	// TODO: Refactor scaledValue and ResourceBank call into Dictionary tu?
	public static T CreateCustomPrefabPiece<T>() where T : ChessboardPiece
	{
		GameObject gameObj = null;
		float scaledValue = 1.25f;

		if (typeof(T) == typeof(ChessboardBoneyardPiece))
		{
			gameObj = ResourceBank.Get<GameObject>($"Resources/Art/Assets3D/PlayerAvatar/gravedigger/GravediggerFin");
		}
		else if (typeof(T) == typeof(ChessboardCardRemovePiece))
		{
			gameObj = ResourceBank.Get<GameObject>($"Prefabs/SpecialNodeSequences/SkinningKnife");
			scaledValue = 0.25f;
		}
		else if (typeof(T) == typeof(ChessboardElectricChairPiece))
		{
			gameObj = ResourceBank.Get<GameObject>($"Resources/Art/Assets3D/MiscLowPolySpace/SM_Bld_Bridge_Chair_Captain_01");
			scaledValue = 0.5f;
		}
		else if (typeof(T) == typeof(ChessboardGoatEyePiece))
		{
			gameObj = ResourceBank.Get<GameObject>($"Prefabs/SpecialNodeSequences/EyeBall");
			Material goatEyeMat = ResourceBank.Get<Material>("Resources/Art/Materials/Eyeball_Goat");
			gameObj.GetComponent<MeshRenderer>().material = goatEyeMat;
			gameObj.GetComponent<MeshRenderer>().sharedMaterial = goatEyeMat;
			scaledValue = 0.4f;
		}

		gameObj.transform.localScale = new Vector3(scaledValue, scaledValue, scaledValue);
		Vector3 vLocalPosition = gameObj.transform.localPosition;
		gameObj.transform.localPosition = new Vector3(vLocalPosition.x, 1.4f, vLocalPosition.z);

		T piece = gameObj.AddComponent<T>();
		piece.anim = PrefabChestPiece.anim;
		return piece;
	}

	#endregion

	public readonly int indexInList;
	public readonly List<ChessNode> BlockerNodes;
	public readonly ChessNode BossNode;
	public readonly List<ChessNode> ChestNodes;
	public readonly List<ChessNode> CardRemovalNodes;
	public readonly List<ChessNode> EnemyNodes;
	public readonly List<ChessNode> OpenPathNodes;
	public readonly ChessNode PlayerNode;

	private static readonly Dictionary<Type, ChessboardPiece> PiecePrefabByType = new()
	{
		{ typeof(ChessboardBlockerPiece), PrefabTombstone },
		{ typeof(ChessboardBoneyardPiece), PrefabBoneyardPiece },
		{ typeof(ChessboardCardRemovePiece), PrefabCardRemovePiece },
		{ typeof(ChessboardChestPiece), PrefabChestPiece },
		{ typeof(ChessboardElectricChairPiece), PrefabElectricChairPiece },
		{ typeof(ChessboardEnemyPiece), PrefabEnemyPiece },
		{ typeof(ChessboardGoatEyePiece), PrefabGoatEyePiece },
	};

	protected internal ChessboardEnemyPiece BossPiece =>
		GetPieceAtSpace(BossNode.GridX, BossNode.GridY) as ChessboardEnemyPiece;

	public readonly List<ChessRow> Rows;

	public Opponent.Type ActiveBossType;

	public GrimoraChessboard(IEnumerable<List<int>> board, int indexInList)
	{
		this.Rows = board.Select((boardList, idx) => new ChessRow(boardList, idx)).ToList();
		this.BlockerNodes = GetBlockerNodes();
		this.BossNode = GetBossNode();
		this.ChestNodes = GetChestNodes();
		this.CardRemovalNodes = GetCardRemovalNodes();
		this.EnemyNodes = GetEnemyNodes();
		this.OpenPathNodes = GetOpenPathNodes();
		this.PlayerNode = GetPlayerNode();
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

	public ChessNode GetPlayerNode()
	{
		return Rows.SelectMany(row => row.GetNodesOfType(9)).Single();
	}

	public void SetupBoard()
	{
		if (ConfigHelper.Instance.isRoyalDead)
		{
			Log.LogDebug($"[SetupGamePieces] Royal defeated");
			PlaceBossPiece(GrimoraBossOpponentExt.SpecialId);
		}
		else if (ConfigHelper.Instance.isSawyerDead)
		{
			Log.LogDebug($"[SetupGamePieces] Sawyer defeated");
			PlaceBossPiece(RoyalBossOpponentExt.SpecialId);
		}
		else if (ConfigHelper.Instance.isKayceeDead)
		{
			Log.LogDebug($"[SetupGamePieces] Kaycee defeated");
			PlaceBossPiece(SawyerBossOpponent.SpecialId);
		}
		else
		{
			Log.LogDebug($"[SetupGamePieces] No bosses defeated yet, creating Kaycee");
			PlaceBossPiece(KayceeBossOpponent.SpecialId);
		}

		if (ConfigHelper.Instance.isDevModeEnabled)
		{
			for (int i = 0; i < 8; i++)
			{
				PlacePiece<ChessboardChestPiece>(i, 0, specialNodeData: new CardChoicesNodeData());
				PlacePiece<ChessboardEnemyPiece>(0, i);
				PlacePiece<ChessboardCardRemovePiece>(7, i);
			}
		}
		else
		{
			PlacePieces<ChessboardEnemyPiece>();
			PlacePieces<ChessboardBlockerPiece>();
			PlacePieces<ChessboardCardRemovePiece>();
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

	public ChessboardEnemyPiece PlaceBossPiece(string bossName)
	{
		return CreateChessPiece<ChessboardEnemyPiece>(PrefabBossPiece, BossNode.GridX, BossNode.GridY, bossName);
	}

	public T PlacePiece<T>(int x, int y, string id = "", SpecialNodeData specialNodeData = null) where T : ChessboardPiece
	{
		PiecePrefabByType.TryGetValue(typeof(T), out ChessboardPiece prefabToUse);

		return CreateChessPiece<T>(prefabToUse, x, y, id, specialNodeData);
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
		piece.saveId = x * 10 + y * 1000;

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
			case ChessboardBlockerPiece blockerPiece:
			{
				Mesh blockerMesh = GetActiveRegionBlockerMesh();
				foreach (var meshFilter in blockerPiece.GetComponentsInChildren<MeshFilter>())
				{
					GameObject meshFilerObj = meshFilter.gameObject;
					if (meshFilerObj.name != "Base")
					{
						UnityEngine.Object.Destroy(meshFilter);
					}
					else
					{
						// meshFilter59.mesh = (Pluginz.allAssets[2] as Mesh);
						// .material.mainTexture = (Pluginz.allAssets[3] as Texture2D);
						// .sharedMaterial.mainTexture = (Pluginz.allAssets[3] as Texture2D);

						meshFilter.mesh = blockerMesh;
						// meshObj.GetComponent<MeshRenderer>().material.mainTexture = blockerMesh as Texture2D;
						// meshObj.GetComponent<MeshRenderer>().sharedMaterial.mainTexture = allAssets[3] as Texture2D;
						meshFilerObj.transform.localScale = new Vector3(0.08f, 0.08f, 0.08f);
						meshFilerObj.transform.localPosition = new Vector3(0f, -0.0209f, 0f);
					}
				}

				break;
			}
			case ChessboardChestPiece chestPiece:
			{
				if (specialNodeData is not null)
				{
					chestPiece.NodeData = specialNodeData;
				}

				break;
			}
		}

		piece.name = nameTemp;

		// Log.LogDebug($"[CreatingPiece] {piece.name}");
		ChessboardMapExt.Instance.pieces.Add(piece);
		ChessboardNavGrid.instance.zones[x, y].GetComponent<ChessboardMapNode>().OccupyingPiece = piece;
		return piece as T;
	}

	#endregion
}
