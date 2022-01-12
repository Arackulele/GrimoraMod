#define SIMPLE_JSON_NO_LINQ_EXPRESSION

using System.Collections.Generic;
using System.IO;
using System.Linq;
using BepInEx;
using DiskCardGame;
using HarmonyLib;
using Unity.Cloud.UserReporting.Plugin.SimpleJson;
using UnityEngine;

namespace GrimoraMod
{
	
	public static class ChessPieceUtils
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
				? opponentTypes[Random.RandomRangeInt(0, opponentTypes.Count)]
				: BaseBossExt.BossTypesByString.GetValueSafe(bossType);

			var blueprints = BlueprintUtils.RegionWithBlueprints[randomType];
			return blueprints[Random.RandomRangeInt(0, blueprints.Count)];
		}

		private static ChessboardBlockerPiece GetRandomBlockerPiece()
		{
			return PrefabTombstones[UnityEngine.Random.RandomRangeInt(0, PrefabTombstones.Count)];
		}

		#endregion

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

		private static void CreateBaseEnemyPiece(ChessboardMap map, ChessboardPiece prefab, int x, int y, string id = "")
		{
			// GrimoraPlugin.Log.LogDebug($"Space is not occupied, attempting to create enemy piece at x [{x}] y [{y}]");
			CreateChessPiece(map, prefab, x, y, id);
		}

		public static void CreateBlockerPiece(ChessboardMap map, int x, int y)
		{
			// GrimoraPlugin.Log.LogDebug($"Attempting to create blocker piece at x [{x}] y [{y}]");
			CreateChessPiece(map, GetRandomBlockerPiece(), x, y);
		}

		private static void CreateChessPiece(ChessboardMap map, ChessboardPiece prefab, int x, int y, string id = "")
		{
			string coordName = $"x[{x}]y[{y}]";
			
			ChessboardPiece piece = GetPieceAtSpace(x, y);
			
			if (ChessboardMapExt.Instance.RemovedPieces.Exists(c => piece is not null && c == piece.name))
			{
				GrimoraPlugin.Log.LogDebug($"-> Skipping [{coordName}] as it already exists. Setting map code to active.");
				piece.MapNode.SetActive(true);
			}
			else
			{

				if (piece is null)
				{
					piece = UnityEngine.Object.Instantiate(prefab, map.dynamicElementsParent);
					piece.gridXPos = x;
					piece.gridYPos = y;
					
					int saveId = x * 10 + y * 1000;
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

					GrimoraPlugin.Log.LogDebug($"[CreatingPiece] {piece.name}");
					map.pieces.Add(piece);
				}
			}
		}

		#endregion
		
	}
}