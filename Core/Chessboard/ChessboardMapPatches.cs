using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DiskCardGame;
using HarmonyLib;
using UnityEngine;

namespace GrimoraMod.Chessboard
{
	[HarmonyPatch(typeof(ChessboardMap))]
	public class ChessboardMapPatches
	{
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

			public static bool FirstRun = true;
			private static int instDefeatedBossCount = 0;

			public static void SetupGamePieces(ChessboardMap __instance)
			{
				// var defaulttombstone = (GameObject)Resources.Load("prefabs\\map\\chessboardmap\\Chessboard_Tombstone_3");
				// var enemypiece = (GameObject)Resources.Load("prefabs\\map\\chessboardmap\\ChessboardEnemyPiece");
				// var bosspiece = (GameObject)Resources.Load("prefabs\\map\\chessboardmap\\BossFigurine");
				// var chestpiece = (GameObject)Resources.Load("prefabs\\map\\chessboardmap\\ChessboardChestPiece");

				//StoryEventsData.EraseEvent(StoryEvent.PlayerDeletedArchivistFile);			// Begin game
				//StoryEventsData.SetEventCompleted(StoryEvent.FactoryConveyorBeltMoved); // Kaycee defeated
				//StoryEventsData.EraseEvent(StoryEvent.FactoryCuckooClockAppeared);			// Doggy defeated
				//StoryEventsData.EraseEvent(StoryEvent.Part3PurchasedHoloBrush);					// Royal defeated

				ResetChessboard(__instance);

				if (StoryEventsData.EventCompleted(StoryEvent.Part3PurchasedHoloBrush))
				{
					//tptohereforzone4
					GrimoraPlugin.Log.LogDebug($"Royal defeated");
					// ResetChessboard(__instance);

					CreateBossPiece(__instance, "GrimoraBoss", 6, 2);

					#region Blockers

					CreateBlockerPiece(__instance, 1, 0);
					CreateBlockerPiece(__instance, 1, 2);
					CreateBlockerPiece(__instance, 1, 3);
					CreateBlockerPiece(__instance, 1, 4);
					CreateBlockerPiece(__instance, 1, 6);
					CreateBlockerPiece(__instance, 1, 7);
					CreateBlockerPiece(__instance, 3, 0);
					CreateBlockerPiece(__instance, 4, 0);
					CreateBlockerPiece(__instance, 6, 0);
					CreateBlockerPiece(__instance, 6, 1);
					CreateBlockerPiece(__instance, 6, 3);
					CreateBlockerPiece(__instance, 6, 4);
					CreateBlockerPiece(__instance, 6, 6);
					CreateBlockerPiece(__instance, 6, 7);

					#endregion

					#region ChestPieces

					CreateChestPiece(__instance, 5, 1);
					CreateChestPiece(__instance, 5, 6);

					#endregion

					#region EnemyPieces

					CreateEnemyPiece(__instance, 1, 0);
					CreateEnemyPiece(__instance, 3, 5);

					#endregion
				}
				else if (StoryEventsData.EventCompleted(StoryEvent.FactoryCuckooClockAppeared))
				{
					GrimoraPlugin.Log.LogDebug($"Doggy defeated");

					CreateBossPiece(__instance, "RoyalBoss", 6, 2);

					#region Blockers

					CreateBlockerPiece(__instance, 0, 2);
					CreateBlockerPiece(__instance, 0, 6);
					CreateBlockerPiece(__instance, 1, 2);
					CreateBlockerPiece(__instance, 1, 4);
					CreateBlockerPiece(__instance, 1, 6);
					CreateBlockerPiece(__instance, 2, 2);
					CreateBlockerPiece(__instance, 2, 6);
					CreateBlockerPiece(__instance, 4, 0);
					CreateBlockerPiece(__instance, 4, 2);
					CreateBlockerPiece(__instance, 5, 0);
					CreateBlockerPiece(__instance, 5, 4);
					CreateBlockerPiece(__instance, 5, 6);
					CreateBlockerPiece(__instance, 6, 0);
					CreateBlockerPiece(__instance, 6, 4);
					CreateBlockerPiece(__instance, 7, 0);
					CreateBlockerPiece(__instance, 7, 4);

					#endregion

					#region ChestPieces

					CreateChestPiece(__instance, 0, 6);
					CreateChestPiece(__instance, 5, 3);
					CreateChestPiece(__instance, 7, 7);

					#endregion

					#region EnemyPieces

					CreateEnemyPiece(__instance, 0, 1);
					CreateEnemyPiece(__instance, 3, 5);
					CreateEnemyPiece(__instance, 4, 2);
					CreateEnemyPiece(__instance, 7, 2);

					#endregion
				}
				else if (StoryEventsData.EventCompleted(StoryEvent.FactoryConveyorBeltMoved))
				{
					//tptohereforzone2
					GrimoraPlugin.Log.LogDebug($"Kaycee defeated");

					// ResetChessboard(__instance);

					CreateBossPiece(__instance, "DoggyBoss", 5, 1);

					#region Blockers

					CreateBlockerPiece(__instance, 1, 2);
					CreateBlockerPiece(__instance, 1, 4);
					CreateBlockerPiece(__instance, 1, 5);
					CreateBlockerPiece(__instance, 1, 6);
					CreateBlockerPiece(__instance, 2, 1);
					CreateBlockerPiece(__instance, 2, 4);
					CreateBlockerPiece(__instance, 2, 6);
					CreateBlockerPiece(__instance, 4, 1);
					CreateBlockerPiece(__instance, 5, 0);
					CreateBlockerPiece(__instance, 5, 6);
					CreateBlockerPiece(__instance, 6, 1);
					CreateBlockerPiece(__instance, 6, 3);
					CreateBlockerPiece(__instance, 6, 4);
					CreateBlockerPiece(__instance, 6, 5);
					CreateBlockerPiece(__instance, 6, 6);
					CreateBlockerPiece(__instance, 7, 4);

					#endregion

					#region ChestPieces

					CreateChestPiece(__instance, 0, 6);
					CreateChestPiece(__instance, 3, 7);
					CreateChestPiece(__instance, 5, 2);

					#endregion

					#region EnemyPieces

					CreateEnemyPiece(__instance, 0, 1);
					CreateEnemyPiece(__instance, 3, 5);
					CreateEnemyPiece(__instance, 4, 2);
					CreateEnemyPiece(__instance, 5, 3);
					CreateEnemyPiece(__instance, 7, 2);

					#endregion

					// StoryEventsData.EraseEvent(StoryEvent.PlayerDeletedArchivistFile);
					// StoryEventsData.EraseEvent(StoryEvent.FactoryCuckooClockAppeared);
					// StoryEventsData.EraseEvent(StoryEvent.Part3PurchasedHoloBrush);
				}
			}

			private static void ResetChessboard(ChessboardMap __instance)
			{
				GrimoraPlugin.Log.LogDebug(
					$"Resetting chess board, current existing pieces [{string.Join(", ", __instance.pieces.Select(p => p.name))}]");
				foreach (var existingChessPiece in __instance.pieces.FindAll(_ => true))
				{
					GrimoraPlugin.Log.LogDebug($"--> Removing piece from list [{existingChessPiece.name}]");
					__instance.pieces.Remove(existingChessPiece);

					GrimoraPlugin.Log.LogDebug($"--> Adding piece to RemovedPieces save data [{existingChessPiece.saveId}]");
					GrimoraSaveData.Data.removedPieces.Add(existingChessPiece.saveId);
					// existingChessPiece.MapNode.OccupyingPiece = null;

					// ChessboardNavGrid.instance.zones[existingChessPiece.gridXPos, existingChessPiece.gridYPos].GetComponent<ChessboardMapNode>()
					// 	.OccupyingPiece = null;

					GrimoraPlugin.Log.LogDebug(
						$"--> UnityEngine.Object.Destroying piece x[{existingChessPiece.gridXPos}] y[{existingChessPiece.gridYPos}]");
					UnityEngine.Object.Destroy(existingChessPiece);
				}

				// GrimoraPlugin.Log.LogDebug($"-> Setting instance pieces to clone");
				// __instance.pieces = piecesClone;
				
				GrimoraPlugin.Log.LogDebug($"-> Setting all navgrid zones to active");
				foreach (var zone in ChessboardNavGrid.instance.zones)
				{
					zone.gameObject.GetComponent<ChessboardMapNode>().gameObject.SetActive(true);
				}
			}

			[HarmonyPostfix, HarmonyPatch(typeof(ChessboardMap), "UnrollingSequence")]
			public static IEnumerator Postfix(IEnumerator enumerator, ChessboardMap __instance, float unrollSpeed)
			{
				if (!SaveManager.SaveFile.IsGrimora)
				{
					yield return enumerator;
					yield break;
				}

				if (FirstRun)
				{
					GrimoraPlugin.Log.LogDebug(
						"POSTFIX HAS RUN DONT IGNORE THIS THIS IS NOT AN ERROR IT IS A GOOD THING IF YOU ARE READING THIS HAVE A NICE DAY");

					var nonEnemyPieces = __instance.pieces.FindAll(_ => true);

					// GrimoraPlugin.Log.LogDebug($"[ChessboardMap.UnrollingSequence][Prefix] " +
					//                            $"Non-enemy pieces [{string.Join(", ", nonEnemyPieces.Select(piece => piece.gameObject.name))}]");
					GrimoraPlugin.Log.LogDebug($"-> Initial pieces are there, resetting initial chessboard");
					foreach (var existingPiece in nonEnemyPieces)
					{
						GrimoraPlugin.Log.LogDebug($"--> UnityEngine.Object.Destroying [{existingPiece.gameObject}]");
						UnityEngine.Object.Destroy(existingPiece.gameObject);
						GrimoraSaveData.Data.removedPieces.Add(existingPiece.saveId);
						// __instance.pieces.Remove(existingPiece);
					}
				}

				StoryEventsData.SetEventCompleted(StoryEvent.GrimoraReachedTable);

				if (StoryEventsData.EventCompleted(StoryEvent.FactoryConveyorBeltMoved))
				{
					GrimoraPlugin.Log.LogDebug($"First boss defeated");
					SetupGamePieces(__instance);
				}
				else
				{
					if (FirstRun)
					{
						FirstRun = false;

						GrimoraPlugin.Log.LogDebug($"No bosses defeated, setting initial board.");

						ResetChessboard(__instance);

						// boss
						CreateBossPiece(__instance, "KayceeBoss", 0, 0);

						#region ChestPieces

						CreateChestPiece(__instance, 3, 5);
						CreateChestPiece(__instance, 3, 7);
						CreateChestPiece(__instance, 6, 6);
						CreateChestPiece(__instance, 7, 0);

						#endregion

						#region CreatingEnemyPieces

						CreateEnemyPiece(__instance, 0, 2);
						CreateEnemyPiece(__instance, 2, 7);
						CreateEnemyPiece(__instance, 3, 4);

						#endregion

						#region Blockers

						CreateBlockerPiece(__instance, 0, 5);
						CreateBlockerPiece(__instance, 1, 2);
						CreateBlockerPiece(__instance, 1, 5);
						CreateBlockerPiece(__instance, 2, 0);
						CreateBlockerPiece(__instance, 2, 2);
						CreateBlockerPiece(__instance, 2, 5);
						CreateBlockerPiece(__instance, 2, 6);
						CreateBlockerPiece(__instance, 4, 2);
						CreateBlockerPiece(__instance, 4, 3);
						CreateBlockerPiece(__instance, 5, 4);
						CreateBlockerPiece(__instance, 6, 0);
						CreateBlockerPiece(__instance, 6, 4);
						CreateBlockerPiece(__instance, 7, 4);

						#endregion
					}
				}

				GrimoraPlugin.Log.LogDebug($"Setting each piece game object active to false");
				__instance.pieces.ForEach(delegate(ChessboardPiece x) { x.gameObject.SetActive(value: false); });
				GrimoraPlugin.Log.LogDebug($"Playing map anim enter");
				__instance.mapAnim.Play("enter", 0, 0f);
				yield return new WaitForSeconds(0.25f);
				GrimoraPlugin.Log.LogDebug($"Setting dynamicElements [{__instance.dynamicElementsParent}] to active");
				__instance.dynamicElementsParent.gameObject.SetActive(value: true);
				MapNodeManager.Instance.ActiveNode = __instance
					.navGrid
					.zones[GrimoraSaveData.Data.gridX, GrimoraSaveData.Data.gridY].GetComponent<MapNode>();
				GrimoraPlugin.Log.LogDebug($"MapNodeManager ActiveNode is [{MapNodeManager.Instance.ActiveNode.name}]");
				
				Singleton<TableVisualEffectsManager>.Instance.SetFogPlaneShown(shown: true);
				Singleton<CameraEffects>.Instance.SetFogEnabled(fogEnabled: true);
				Singleton<CameraEffects>.Instance.SetFogAlpha(0f);
				Singleton<CameraEffects>.Instance.TweenFogAlpha(0.6f, 1f);
				
				GrimoraPlugin.Log.LogDebug($"Setting SetPlayerAdjacentNodesActive");
				ChessboardNavGrid.instance.SetPlayerAdjacentNodesActive();

				var activeNode = MapNodeManager.Instance.ActiveNode;
				GrimoraPlugin.Log.LogDebug($"Setting PlayerMaker to [{activeNode.transform.position}]");
				PlayerMarker.Instance.transform.position = activeNode.transform.position;
				__instance.pieces.ForEach(delegate(ChessboardPiece x)
				{
					GrimoraPlugin.Log.LogDebug($"Starting UpdateSaveState and Hide for piece [{x.name}]");
					x.UpdateSaveState();
					x.Hide(immediate: true);
				});

				yield return new WaitForSeconds(0.05f);
				foreach (var piece in __instance.pieces.Where(piece => piece.gameObject.activeInHierarchy))
				{
					GrimoraPlugin.Log.LogDebug($"Piece [{piece.name}] saveid[{piece.saveId}] is active in hierarchy, calling Show method");
					piece.Show();
					yield return new WaitForSeconds(0.025f);
				}

				if (!DialogueEventsData.EventIsPlayed("FinaleGrimoraMapShown"))
				{
					yield return new WaitForSeconds(0.5f);
					yield return Singleton<TextDisplayer>.Instance.PlayDialogueEvent("FinaleGrimoraMapShown",
						TextDisplayer.MessageAdvanceMode.Input);
				}


				// foreach (var piece in FindObjectsOfType(typeof(ChessboardPiece)))
				// {
				// 	var piece = (ChessboardPiece)piece;
				// 	// piece.gameObject.transform.parent = __instance.dynamicElementsParent;
				// 	// GrimoraPlugin.Log.LogDebug($"Set [{piece}] parent to [{__instance.dynamicElementsParent}]");
				//
				// 	// __instance.pieces.ForEach(delegate(ChessboardPiece x)
				// 	// {
				// 	// 	GrimoraPlugin.Log.LogDebug($"Hiding [{x}]");
				// 	// 	x.UpdateSaveState();
				// 	// 	x.Hide(true);
				// 	// });
				// }
			}
			
			private static bool IsSpaceNotOccupied(int x, int y)
			{
				var occupyingPiece = ChessboardNavGrid.instance.zones[x, y].GetComponent<ChessboardMapNode>().OccupyingPiece;
				return occupyingPiece is null;
			}

			private static void CreateBossPiece(ChessboardMap __instance, string id, int x, int y)
			{
				if (IsSpaceNotOccupied(x, y))
				{
					GrimoraPlugin.Log.LogDebug($"Creating boss piece");
					ChessboardEnemyPiece bossPiece = Object.Instantiate(PrefabBossPiece, __instance.dynamicElementsParent);
					bossPiece.specialEncounterId = id;
					bossPiece.GoalPosX = x;
					bossPiece.GoalPosY = y;
					bossPiece.gridXPos = x;
					bossPiece.gridYPos = y;
					bossPiece.blueprint = GetBlueprint(id);
					bossPiece.saveId = bossPiece.gridXPos * 10 + bossPiece.gridYPos * 1000;
					GrimoraPlugin.Log.LogDebug($"bossPiece [{bossPiece.name}] save ID [{bossPiece.saveId}]");
					__instance.pieces.Add(bossPiece);
					ChessboardNavGrid.instance.zones[x, y].GetComponent<ChessboardMapNode>().OccupyingPiece = bossPiece;
				}
			}

			private static void CreateChestPiece(ChessboardMap __instance, int x, int y)
			{
				// GrimoraPlugin.Log.LogDebug($"Attempting to create chest piece at x [{x}] y [{y}]");
				if (IsSpaceNotOccupied(x, y))
				{
					ChessboardChestPiece chest = UnityEngine.Object.Instantiate(PrefabChestPiece, __instance.dynamicElementsParent)
						.GetComponent<ChessboardChestPiece>();
					chest.gridXPos = x;
					chest.gridYPos = y;
					var randomValue = Random.Range(0, 4);
					if (randomValue != 2)
					{
						chest.NodeData = new CardChoicesNodeData();
					}
					else
					{
						chest.NodeData = new ChooseRareCardNodeData();
					}

					chest.saveId = chest.gridXPos * 10 + chest.gridYPos * 1000;
					chest.name = $"Chest_x{x}_y{y}";
					GrimoraPlugin.Log.LogDebug($"[CreateChestPiece] Chest [{chest.name}] save ID [{chest.saveId}]");
					__instance.pieces.Add(chest);
					ChessboardNavGrid.instance.zones[x, y].GetComponent<ChessboardMapNode>().OccupyingPiece = chest;
				}
			}

			private static void CreateEnemyPiece(ChessboardMap __instance, int x, int y)
			{
				if (IsSpaceNotOccupied(x, y))
				{
					// GrimoraPlugin.Log.LogDebug($"Space is not occupied, attempting to create enemy piece at x [{x}] y [{y}]");
					ChessboardEnemyPiece enemyPiece = UnityEngine.Object.Instantiate(PrefabEnemyPiece, __instance.dynamicElementsParent);
					enemyPiece.gridXPos = x;
					enemyPiece.gridYPos = y;
					enemyPiece.GoalPosX = x;
					enemyPiece.GoalPosY = y;
					enemyPiece.saveId = enemyPiece.gridXPos * 10 + enemyPiece.gridYPos * 1000;
					enemyPiece.name = $"Enemy_x{x}_y{y}";
					enemyPiece.blueprint = GetBlueprint();
					GrimoraPlugin.Log.LogDebug($"[CreateEnemyPiece] EnemyPiece [{enemyPiece.name}] save ID [{enemyPiece.saveId}]");
					__instance.pieces.Add(enemyPiece);
					ChessboardNavGrid.instance.zones[x, y].GetComponent<ChessboardMapNode>().OccupyingPiece = enemyPiece;
				}
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

			private static void CreateBlockerPiece(ChessboardMap __instance, int x, int y)
			{
				// GrimoraPlugin.Log.LogDebug($"Attempting to create blocker piece at x [{x}] y [{y}]");

				ChessboardBlockerPiece blocker = UnityEngine.Object.Instantiate(GetRandomBlockerPiece(), __instance.dynamicElementsParent);

				foreach (MeshFilter meshFilter in blocker.GetComponentsInChildren<MeshFilter>())
				{
					if (meshFilter.gameObject.name != "Base")
					{
						UnityEngine.Object.Destroy(meshFilter.gameObject);
					}
					else
					{
						meshFilter.mesh = GrimoraPlugin.AllAssets[2] as Mesh;
						GameObject meshFilterGameObject = meshFilter.gameObject;
						meshFilterGameObject.GetComponent<MeshRenderer>().material.mainTexture = GrimoraPlugin.AllAssets[3] as Texture2D;
						meshFilterGameObject.GetComponent<MeshRenderer>().sharedMaterial.mainTexture = GrimoraPlugin.AllAssets[3] as Texture2D;
						meshFilterGameObject.transform.localScale = new Vector3(0.08f, 0.08f, 0.08f);
						meshFilterGameObject.transform.localPosition = new Vector3(0, -0.0209f, 0);
					}
				}

				blocker.gridXPos = x;
				blocker.gridYPos = y;
				blocker.name = $"Blocker_x{x}_y{y}";
				blocker.saveId = blocker.gridXPos * 10 + blocker.gridYPos * 1000;
				GrimoraPlugin.Log.LogDebug($"[CreateBlockerPiece] Blocker [{blocker.name}] save ID [{blocker.saveId}]");
				__instance.pieces.Add(blocker);
				ChessboardNavGrid.instance.zones[x, y].GetComponent<ChessboardMapNode>().OccupyingPiece = blocker;
			}
		}
	}
