using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DiskCardGame;
using HarmonyLib;
using UnityEngine;

namespace GrimoraMod
{
	[HarmonyPatch(typeof(ChessboardMap))]
	public class ChessboardMapPatches
	{
		public static bool isTransitioningFromBoss = false;
		

		public static void SetupGamePieces(ChessboardMap map)
		{
			//StoryEventsData.EraseEvent(StoryEvent.PlayerDeletedArchivistFile);			// Begin game
			//StoryEventsData.SetEventCompleted(StoryEvent.FactoryConveyorBeltMoved); // Kaycee defeated
			//StoryEventsData.EraseEvent(StoryEvent.FactoryCuckooClockAppeared);			// Doggy defeated
			//StoryEventsData.EraseEvent(StoryEvent.Part3PurchasedHoloBrush);					// Royal defeated
			
			if (isTransitioningFromBoss)
			{
				GrimoraPlugin.Log.LogDebug($"Transitioning from boss, resetting board");
				isTransitioningFromBoss = false;
				// GrimoraPlugin.ConfigCurrentRemovedPieces.Value = "";
				ResetChessboard(map);
				GrimoraPlugin.ConfigCurrentRemovedPieces.Value = "";
			}
			
			ChessUtils.StaticCurrentList = GrimoraPlugin.ConfigCurrentRemovedPieces.Value.Split(',').ToList();

			ChessUtils.PrintCurrentList();
			
			if (GrimoraPlugin.ConfigRoyalThirdBossDead.Value)
			{
				//tptohereforzone4
				GrimoraPlugin.Log.LogDebug($"Royal defeated");
				// ResetChessboard(__instance);

				ChessUtils.CreateBossPiece(map, "GrimoraBoss", 6, 2);

				#region Blockers

				ChessUtils.CreateBlockerPiece(map, 1, 0);
				// CreateBlockerPiece(map,1, 2);
				// CreateBlockerPiece(map,1, 3);
				// CreateBlockerPiece(map,1, 4);
				// CreateBlockerPiece(map,1, 6);
				// CreateBlockerPiece(map,1, 7);
				// CreateBlockerPiece(map,3, 0);
				// CreateBlockerPiece(map,4, 0);
				// CreateBlockerPiece(map,6, 0);
				// CreateBlockerPiece(map,6, 1);
				// CreateBlockerPiece(map,6, 3);
				// CreateBlockerPiece(map,6, 4);
				// CreateBlockerPiece(map,6, 6);
				ChessUtils.CreateBlockerPiece(map, 6, 7);

				#endregion

				#region ChestPieces

				ChessUtils.CreateChestPiece(map, 5, 1);
				// CreateChestPiece(map, 5, 6);

				#endregion

				#region EnemyPieces

				// CreateEnemyPiece(map, 1, 0);
				ChessUtils.CreateEnemyPiece(map, 3, 5);

				#endregion
			}
			else if (GrimoraPlugin.ConfigDoggySecondBossDead.Value)
			{
				GrimoraPlugin.Log.LogDebug($"Doggy defeated");

				ChessUtils.CreateBossPiece(map, "RoyalBoss", 6, 2);

				#region Blockers

				// CreateBlockerPiece(map,0, 2);
				// CreateBlockerPiece(map,0, 6);
				// CreateBlockerPiece(map,1, 2);
				// CreateBlockerPiece(map,1, 4);
				// CreateBlockerPiece(map,1, 6);
				// CreateBlockerPiece(map,2, 2);
				// CreateBlockerPiece(map,2, 6);
				// CreateBlockerPiece(map,4, 0);
				// CreateBlockerPiece(map,4, 2);
				// CreateBlockerPiece(map,5, 0);
				// CreateBlockerPiece(map,5, 4);
				// CreateBlockerPiece(map,5, 6);
				// CreateBlockerPiece(map,6, 0);
				// CreateBlockerPiece(map,6, 4);
				ChessUtils.CreateBlockerPiece(map, 7, 0);
				ChessUtils.CreateBlockerPiece(map, 7, 4);

				#endregion

				#region ChestPieces

				// CreateChestPiece(map, 0, 6);
				ChessUtils.CreateChestPiece(map, 5, 3);
				// CreateChestPiece(map, 7, 7);

				#endregion

				#region EnemyPieces

				// CreateEnemyPiece(map, 0, 1);
				// CreateEnemyPiece(map, 3, 5);
				// CreateEnemyPiece(map, 4, 2);
				ChessUtils.CreateEnemyPiece(map, 7, 2);

				#endregion
			}
			else if (GrimoraPlugin.ConfigKayceeFirstBossDead.Value)
			{
				//tptohereforzone2
				GrimoraPlugin.Log.LogDebug($"Kaycee defeated");

				// ResetChessboard(__instance);

				ChessUtils.CreateBossPiece(map, "DoggyBoss", 5, 1);

				#region Blockers

				// CreateBlockerPiece( 1, 2);
				// CreateBlockerPiece( 1, 4);
				// CreateBlockerPiece( 1, 5);
				// CreateBlockerPiece( 1, 6);
				// CreateBlockerPiece( 2, 1);
				// CreateBlockerPiece( 2, 4);
				// CreateBlockerPiece( 2, 6);
				// CreateBlockerPiece( 4, 1);
				// CreateBlockerPiece( 5, 0);
				// CreateBlockerPiece( 5, 6);
				// CreateBlockerPiece( 6, 1);
				// CreateBlockerPiece( 6, 3);
				// CreateBlockerPiece( 6, 4);
				// CreateBlockerPiece( 6, 5);
				// CreateBlockerPiece( 6, 6);
				ChessUtils.CreateBlockerPiece(map, 7, 4);

				#endregion

				#region ChestPieces

				ChessUtils.CreateChestPiece(map, 0, 6);
				// CreateChestPiece(__instance, 3, 7);
				// CreateChestPiece(__instance, 5, 2);

				#endregion

				#region EnemyPieces

				ChessUtils.CreateEnemyPiece(map, 0, 1);
				// CreateEnemyPiece(__instance, 3, 5);
				// CreateEnemyPiece(__instance, 4, 2);
				// CreateEnemyPiece(__instance, 5, 3);
				// CreateEnemyPiece(__instance, 7, 2);

				#endregion
			}
			else
			{
				GrimoraPlugin.Log.LogDebug($"No bosses defeated yet, creating Kaycee");
				
				ChessUtils.CreateBossPiece(map, "KayceeBoss", 0, 7);

				#region ChestPieces

				ChessUtils.CreateChestPiece(map, 3, 5);
				// CreateChestPiece(__instance, 3, 7);
				// CreateChestPiece(__instance, 6, 6);
				// CreateChestPiece(__instance, 7, 0);

				#endregion

				#region CreatingEnemyPieces

				for (int i = 0; i < 8; i++)
				{
					ChessUtils.CreateEnemyPiece(map, 5, i);
					
				// CreateEnemyPiece(__instance, 0, 2);
				// CreateEnemyPiece(__instance, 2, 7);
				}

				#endregion

				#region Blockers

				ChessUtils.CreateBlockerPiece(map, 0, 5);
				// CreateBlockerPiece(1, 2);
				// CreateBlockerPiece(1, 5);
				// CreateBlockerPiece(2, 0);
				// CreateBlockerPiece(2, 2);
				// CreateBlockerPiece(2, 5);
				// CreateBlockerPiece(2, 6);
				// CreateBlockerPiece(4, 2);
				// CreateBlockerPiece(4, 3);
				// CreateBlockerPiece(5, 4);
				// CreateBlockerPiece(6, 0);
				// CreateBlockerPiece(6, 4);
				// CreateBlockerPiece(7, 4);

				#endregion
			}
		}

		private static bool FirstRun = true;
		private static bool ChessboardAlreadyRenamed = false;

		[HarmonyPrefix, HarmonyPatch(typeof(ChessboardMap), nameof(ChessboardMap.UnrollingSequence))]
		public static void SetStatePrefix(ref ChessboardMap __instance, out ChessboardMap __state)
		{
			__state = __instance;

			if (__state.pieces.Any(p => p.name.Contains("Tombstone")))
			{
				GrimoraPlugin.Log.LogDebug($"Destroying starting chess pieces");
				FirstRun = false;
				__state.pieces.RemoveAll(p =>
				{
					if (GrimoraPlugin.InitialBoardPiecesToRemove.Contains(p.saveId))
					{
						p.MapNode.OccupyingPiece = null;
						p.gameObject.SetActive(false);
						UnityEngine.Object.Destroy(p.gameObject);

						return true;
					}

					return false;
				});
				SaveManager.SaveToFile();
			}
		}

		[HarmonyPostfix, HarmonyPatch(typeof(ChessboardMap), nameof(ChessboardMap.UnrollingSequence))]
		public static IEnumerator Postfix(IEnumerator enumerator, ChessboardMap __instance, float unrollSpeed)
		{
			if (!SaveManager.SaveFile.IsGrimora)
			{
				yield return enumerator;
				yield break;
			}

			GrimoraPlugin.Log.LogDebug($"[ChessboardMap.UnrollingSequence] Setting up game pieces");
			SetupGamePieces(__instance);
			// yield return new WaitForSeconds(1f);
			GrimoraPlugin.Log.LogDebug(
				$"[ChessboardMap.UnrollingSequence] -> Finished setting up game pieces");

			GrimoraPlugin.Log.LogDebug($"[ChessboardMap.UnrollingSequence] " +
			                           $"Current removed pieces [{string.Join(", ", GrimoraPlugin.ConfigCurrentRemovedPieces.Value)}]" +
			                           $"\n Current instance pieces [{string.Join(", ", __instance.pieces.Select(p => p.name))}]"
			);

			StoryEventsData.SetEventCompleted(StoryEvent.GrimoraReachedTable);

			GrimoraPlugin.Log.LogDebug($"[ChessboardMap.UnrollingSequence] Setting each piece game object active to false");
			__instance.pieces.ForEach(delegate(ChessboardPiece x) { x.gameObject.SetActive(value: false); });
			// yield return new WaitForSeconds(0.5f);

			GrimoraPlugin.Log.LogDebug($"[ChessboardMap.UnrollingSequence] Playing map anim enter");
			__instance.mapAnim.Play("enter", 0, 0f);
			yield return new WaitForSeconds(0.25f);

			if (!ChessboardAlreadyRenamed)
			{
				ChessboardAlreadyRenamed = true;
				ChessUtils.RenameMapNodesWithGridCoords();
			}

			// yield return new WaitForSeconds(0.75f);

			GrimoraPlugin.Log.LogDebug(
				$"[ChessboardMap.UnrollingSequence] Setting dynamicElements [{__instance.dynamicElementsParent}] to active");
			__instance.dynamicElementsParent.gameObject.SetActive(value: true);
			// yield return new WaitForSeconds(0.5f);

			MapNodeManager.Instance.ActiveNode = __instance
				.navGrid
				.zones[GrimoraSaveData.Data.gridX, GrimoraSaveData.Data.gridY].GetComponent<MapNode>();
			GrimoraPlugin.Log.LogDebug(
				$"[ChessboardMap.UnrollingSequence] MapNodeManager ActiveNode is [{MapNodeManager.Instance.ActiveNode.name}]");

			TableVisualEffectsManager.Instance.SetFogPlaneShown(shown: true);
			CameraEffects.Instance.SetFogEnabled(fogEnabled: true);
			CameraEffects.Instance.SetFogAlpha(0f);
			CameraEffects.Instance.TweenFogAlpha(0.6f, 1f);
			// yield return new WaitForSeconds(2f);
			
			GrimoraPlugin.Log.LogDebug($"[ChessboardMap.UnrollingSequence] Setting SetPlayerAdjacentNodesActive");
			ChessboardNavGrid.instance.SetPlayerAdjacentNodesActive();

			var position = MapNodeManager.Instance.ActiveNode.transform.position;
			GrimoraPlugin.Log.LogDebug(
				$"[ChessboardMap.UnrollingSequence] Setting PlayerMaker to [{position}]");
			PlayerMarker.Instance.transform.position = position;
			// yield return new WaitForSeconds(1f);
			
			GrimoraPlugin.Log.LogDebug($"[ChessboardMap.UnrollingSequence] " +
			                           $"Current pieces list [{string.Join(", ", __instance.pieces.Select(p => p.name))}]");
			__instance.pieces.ForEach(delegate(ChessboardPiece x)
			{
				x.UpdateSaveState();
				x.Hide(immediate: true);
			});
			GrimoraPlugin.Log.LogDebug("[ChessboardMap.UnrollingSequence] Finished UpdatingSaveStates of pieces");
			yield return new WaitForSeconds(0.05f);
			
			foreach (var piece in __instance.pieces.Where(piece => piece.gameObject.activeInHierarchy))
			{
				// GrimoraPlugin.Log.LogDebug($"-> Piece [{piece.name}] saveId [{piece.saveId}] is active in hierarchy, calling Show method");
				piece.Show();
				yield return new WaitForSeconds(0.025f);
			}

			GrimoraPlugin.Log.LogDebug("[ChessboardMap.UnrollingSequence] Finished showing all active pieces");
			// yield return new WaitForSeconds(1f);

			if (!DialogueEventsData.EventIsPlayed("FinaleGrimoraMapShown"))
			{
				yield return new WaitForSeconds(0.5f);
				yield return TextDisplayer.Instance.PlayDialogueEvent("FinaleGrimoraMapShown",
					TextDisplayer.MessageAdvanceMode.Input);
			}

			SaveManager.SaveToFile();
		}
		
		private static void ResetChessboard(ChessboardMap __instance)
		{
			GrimoraPlugin.Log.LogDebug(
				$"Resetting chess board, current existing pieces [{string.Join(", ", __instance.pieces.Select(p => p.name))}]");
			foreach (var existingChessPiece in __instance.pieces.FindAll(_ => true))
			{
				GrimoraPlugin.Log.LogDebug($"-> Adding piece to removed pieces [{existingChessPiece.saveId}]");
				// ChessUtils.AddPieceToRemovedPiecesConfig(existingChessPiece.name);
				__instance.pieces.Remove(existingChessPiece);
				// GrimoraPlugin.Log.LogDebug($"--> Setting Nav Grid x,y occupying piece to null");
				// ChessboardNavGrid.instance
				// 	.zones[existingChessPiece.gridXPos, existingChessPiece.gridYPos]
				// 	.GetComponent<ChessboardMapNode>()
				// 	.OccupyingPiece = null;

				GrimoraPlugin.Log.LogDebug(
					$"--> Destroying piece x[{existingChessPiece.gridXPos}] y[{existingChessPiece.gridYPos}]");
				UnityEngine.Object.Destroy(existingChessPiece);
			}

			// GrimoraSaveData.Data.removedPieces = GrimoraSaveData.Data.removedPieces.Distinct().ToList();


			SaveManager.SaveToFile();
		}




	}
}