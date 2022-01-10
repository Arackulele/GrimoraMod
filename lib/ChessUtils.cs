using System;
using System.Collections.Generic;
using System.Linq;
using DiskCardGame;
using HarmonyLib;
using UnityEngine;
using Object = System.Object;

namespace GrimoraMod
{
	public static class ChessUtils
	{
		#region Prefabs

		public const string PrefabPath = "Prefabs/Map/ChessboardMap";

		public static List<ChessboardBlockerPiece> PrefabTombstones = new()
		{
			ResourceBank.Get<ChessboardBlockerPiece>($"{PrefabPath}/Chessboard_Tombstone_1"),
			ResourceBank.Get<ChessboardBlockerPiece>($"{PrefabPath}/Chessboard_Tombstone_2"),
			ResourceBank.Get<ChessboardBlockerPiece>($"{PrefabPath}/Chessboard_Tombstone_3")
		};

		public static ChessboardEnemyPiece PrefabEnemyPiece =>
			ResourceBank.Get<ChessboardEnemyPiece>($"{PrefabPath}/ChessboardEnemyPiece");

		public static ChessboardEnemyPiece PrefabBossPiece =>
			ResourceBank.Get<ChessboardEnemyPiece>($"{PrefabPath}/BossFigurine");

		public static ChessboardChestPiece PrefabChestPiece =>
			ResourceBank.Get<ChessboardChestPiece>($"{PrefabPath}/ChessboardChestPiece");

		#endregion


		#region PiecesHelperMethods

		private static ChessboardPiece GetPieceAtSpace(int x, int y)
		{
			return ChessboardNavGrid.instance.zones[x, y].GetComponent<ChessboardMapNode>().OccupyingPiece;
		}

		private static EncounterBlueprintData GetBlueprint(string bossType = "")
		{
			List<Opponent.Type> opponentTypes = BlueprintUtils.RegionWithBlueprints.Keys.ToList();

			var randomType = string.IsNullOrEmpty(bossType)
				? opponentTypes[UnityEngine.Random.RandomRangeInt(0, opponentTypes.Count)]
				: BaseBossExt.BossTypesByString.GetValueSafe(bossType);

			var blueprints = BlueprintUtils.RegionWithBlueprints[randomType];
			return blueprints[UnityEngine.Random.RandomRangeInt(0, blueprints.Count)];
		}

		private static ChessboardBlockerPiece GetRandomBlockerPiece()
		{
			return ResourceBank.Get<ChessboardBlockerPiece>($"{PrefabPath}/Chessboard_Tombstone_3");
		}

		#endregion


		public static List<string> StaticCurrentList;

		public static void RenameMapNodesWithGridCoords()
		{
			GrimoraPlugin.Log.LogDebug($"ChessboardMap.UnrollingSequence] Renaming all map nodes");

			var zones = ChessboardNavGrid.instance.zones;
			for (var i = 0; i < zones.GetLength(0); i++)
			{
				for (var i1 = 0; i1 < zones.GetLength(1); i1++)
				{
					var obj = ChessboardNavGrid.instance.zones[i, i1].GetComponent<ChessboardMapNode>();
					obj.name = $"ChessboardMapNode_x[{i}]y[{i1}]";
				}
			}
		}

		public static void PrintCurrentList()
		{
			GrimoraPlugin.Log.LogDebug($"Current static list [{string.Join(", ", StaticCurrentList)}]");
		}

		public static void AddPieceToRemovedPiecesConfig(string name)
		{
			GrimoraPlugin.ConfigCurrentRemovedPieces.Value += "," + name;
		}

		#region CreatePieces

		public static void CreateBossPiece(ChessboardMap map, string id, int x, int y)
		{
			CreateBaseEnemyPiece(map, PrefabBossPiece, x, y, id);
		}

		public static void CreateChestPiece(ChessboardMap map, int x, int y)
		{
			// GrimoraPlugin.Log.LogDebug($"Attempting to create chest piece at x [{x}] y [{y}]");
			CreateChessPiece(map, PrefabChestPiece, x, y);
		}

		public static void CreateEnemyPiece(ChessboardMap map, int x, int y)
		{
			// GrimoraPlugin.Log.LogDebug($"Space is not occupied, attempting to create enemy piece at x [{x}] y [{y}]");
			CreateBaseEnemyPiece(map, PrefabEnemyPiece, x, y);
		}

		private static void CreateBaseEnemyPiece(
			ChessboardMap map,
			ChessboardPiece prefab,
			int x, int y,
			string id = ""
		)
		{
			// GrimoraPlugin.Log.LogDebug($"Space is not occupied, attempting to create enemy piece at x [{x}] y [{y}]");
			CreateChessPiece(map, prefab, x, y, id);
		}


		public static void CreateBlockerPiece(ChessboardMap map, int x, int y)
		{
			// GrimoraPlugin.Log.LogDebug($"Attempting to create blocker piece at x [{x}] y [{y}]");

			CreateChessPiece(map, GetRandomBlockerPiece(), x, y);

			// foreach (MeshFilter meshFilter in blocker.GetComponentsInChildren<MeshFilter>())
			// {
			// 	if (meshFilter.gameObject.name != "Base")
			// 	{
			// 		UnityEngine.Object.Destroy(meshFilter.gameObject);
			// 	}
			// 	else
			// 	{
			// 		meshFilter.mesh = GrimoraPlugin.AllAssets[2] as Mesh;
			// 		GameObject meshFilterGameObject = meshFilter.gameObject;
			// 		meshFilterGameObject.GetComponent<MeshRenderer>().material.mainTexture =
			// 			GrimoraPlugin.AllAssets[3] as Texture2D;
			// 		meshFilterGameObject.GetComponent<MeshRenderer>().sharedMaterial.mainTexture =
			// 			GrimoraPlugin.AllAssets[3] as Texture2D;
			// 		meshFilterGameObject.transform.localScale = new Vector3(0.08f, 0.08f, 0.08f);
			// 		meshFilterGameObject.transform.localPosition = new Vector3(0, -0.0209f, 0);
			// 	}
			// }
		}

		private static void CreateChessPiece(
			ChessboardMap map,
			ChessboardPiece prefab,
			int x, int y, string id = ""
		)
		{
			string coordName = $"x[{x}]y[{y}]";

			int saveId = x * 10 + y * 1000;

			if (StaticCurrentList.Exists(pieceName => pieceName.EndsWith(coordName)))
			{
				GrimoraPlugin.Log.LogDebug($"-> Skipping creation of [{coordName}] as it exists in the removed pieces data");
			}
			else
			{
				ChessboardPiece piece = GetPieceAtSpace(x, y);

				if (piece is null)
				{
					piece = UnityEngine.Object.Instantiate(prefab, map.dynamicElementsParent);
					piece.gridXPos = x;
					piece.gridYPos = y;
					piece.saveId = saveId;

					string nameTemp = piece.GetType().Name.Replace("Chessboard", "") + "_" + coordName;

					if (piece is ChessboardEnemyPiece enemyPiece)
					{
						enemyPiece.GoalPosX = x;
						enemyPiece.GoalPosX = y;
						enemyPiece.blueprint = GetBlueprint(id);

						if (prefab.name.Contains("Boss"))
						{
							// GrimoraPlugin.Log.LogDebug($"Prefab piece is boss, setting name");
							enemyPiece.specialEncounterId = id;
							nameTemp = nameTemp.Replace("Enemy", "Boss");
						}
					}

					piece.name = nameTemp;

					GrimoraPlugin.Log.LogDebug(
						$"[ChessboardMap.UnrollingSeq.CreatingPiece] {piece.name} SaveID [{piece.saveId}]");
					map.pieces.Add(piece);
				}
			}
		}

		#endregion
		
	}
}