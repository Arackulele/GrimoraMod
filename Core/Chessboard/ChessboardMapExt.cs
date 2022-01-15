using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DiskCardGame;
using Unity.Cloud.UserReporting.Plugin.SimpleJson;
using UnityEngine;
using Resources = GrimoraMod.Properties.Resources;

// This class is literally just meant to be able to view the deck review sequencer
namespace GrimoraMod
{
	public class ChessboardMapExt : ChessboardMap
	{
		private GrimoraChessboard activeChessboard;
		private List<ChessboardPiece> activePieces;

		private ChessboardEnemyPiece bossPiece;

		private string[] buttonNames =
		{
			"Win Round", "Lose Round", "Deck View",
			"Place Chest"
		};

		private List<GrimoraChessboard> chessboards;
		private int currentChessboardIndex;
		private List<string> removedPieces;

		private bool toggleEncounterMenu;
		public new static ChessboardMapExt Instance => GameMap.Instance as ChessboardMapExt;

		public List<string> RemovedPieces => GrimoraPlugin.ConfigCurrentRemovedPieces.Value.Split(',').Distinct().ToList();

		public ChessboardEnemyPiece BossPiece
		{
			get
			{
				if (bossPiece != null)
				{
					return bossPiece;
				}

				foreach (var piece in pieces.Where(piece => piece.name.Contains("Boss")))
				{
					bossPiece = piece as ChessboardEnemyPiece;
				}

				return bossPiece;
			}
		}

		private bool ChangingRegion { get; set; }

		public bool BossDefeated { get; protected internal set; }

		private List<GrimoraChessboard> Chessboards
		{
			get
			{
				LoadData();
				return chessboards;
			}
		}

		private void Start()
		{
			// GrimoraPlugin.Log.LogDebug($"[MapExt] Setting on view changed");
			ViewManager instance = ViewManager.Instance;
			instance.ViewChanged = (Action<View, View>)Delegate
				.Combine(instance.ViewChanged, new Action<View, View>(OnViewChanged));
		}

		private void OnGUI()
		{
			toggleEncounterMenu = GUI.Toggle(
				new Rect(20, 100, 200, 20),
				toggleEncounterMenu,
				"Debug Tools"
			);

			if (!toggleEncounterMenu) return;

			int selectedButton = GUI.SelectionGrid(
				new Rect(25, 150, 300, 100),
				-1,
				buttonNames,
				2
			);

			if (selectedButton >= 0)
			{
				GrimoraPlugin.Log.LogDebug($"[OnGUI] Calling button [{selectedButton}]");
				switch (buttonNames[selectedButton])
				{
					case "Win Round":
						LifeManager.Instance.StartCoroutine(
							LifeManager.Instance.ShowDamageSequence(10, 1, false)
						);
						break;
					case "Lose Round":
						LifeManager.Instance.StartCoroutine(
							LifeManager.Instance.ShowDamageSequence(10, 1, true)
						);
						break;
					case "Place Chest":
						ChessboardMapExt.Instance.activeChessboard.PlaceChestPiece(0, 0);
						break;
					case "Deck View":
						GrimoraPlugin.Log.LogDebug($"[OnGUI] is deck view [{selectedButton}]");
						switch (ViewManager.Instance.CurrentView)
						{
							case View.MapDeckReview:
								ViewManager.Instance.SwitchToView(View.MapDefault);
								break;
							case View.MapDefault:
								ViewManager.Instance.SwitchToView(View.MapDeckReview);
								break;
						}

						break;
				}
			}
		}

		public void LoadData()
		{
			if (chessboards == null)
			{
				GrimoraPlugin.Log.LogDebug($"[ChessboardMapExt] Loading json boards");
				string jsonString = Encoding.UTF8.GetString(Resources.GrimoraChessboardsStatic);

				chessboards = ParseJson(
					SimpleJson.DeserializeObject<List<List<List<int>>>>(jsonString)
				);
			}
		}

		private static List<GrimoraChessboard> ParseJson(IEnumerable<List<List<int>>> chessboardsFromJson)
		{
			return chessboardsFromJson.Select(board => new GrimoraChessboard(board)).ToList();
		}

		public IEnumerator CompleteRegionSequence()
		{
			// GrimoraPlugin.Log.LogDebug($"[CompleteRegionSequence] Starting CompleteRegionSequence");
			ChangingRegion = true;

			ViewManager.Instance.Controller.LockState = ViewLockState.Locked;
			yield return new WaitForSeconds(0.8f);

			// yield return TextDisplayer.Instance.PlayDialogueEvent("RegionNext", TextDisplayer.MessageAdvanceMode.Input);

			ViewManager.Instance.SwitchToView(View.MapDefault);
			yield return new WaitForSeconds(0.25f);

			RunState.CurrentMapRegion.FadeInAmbientAudio();


			MapNodeManager.Instance.SetAllNodesInteractable(false);
			// yield return ShowMapSequence(0.25f);

			// yield return TextDisplayer.Instance.PlayDialogueEvent("Region" + RunState.CurrentMapRegion.name, TextDisplayer.MessageAdvanceMode.Input);

			// GrimoraPlugin.Log.LogDebug($"[CompleteRegionSequence] Looping audio");
			AudioController.Instance.SetLoopAndPlay("finalegrimora_ambience");
			AudioController.Instance.SetLoopVolumeImmediate(0f);
			AudioController.Instance.FadeInLoop(1f, 1f);

			// GrimoraPlugin.Log.LogDebug($"[CompleteRegionSequence] HandleChessboardSetup called");
			Instance.HandleChessboardSetup();

			// GrimoraPlugin.Log.LogDebug($"[CompleteRegionSequence] Clearing and destroying pieces");
			Instance.pieces.RemoveAll(delegate(ChessboardPiece piece)
			{
				// piece.gameObject.SetActive(false);
				piece.MapNode.OccupyingPiece = null;
				Destroy(piece.gameObject);

				return true;
			});
			// GrimoraPlugin.Log.LogDebug($"[CompleteRegionSequence] Clearing activePieces list");
			Instance.activePieces.Clear();
			// GrimoraPlugin.Log.LogDebug($"[CompleteRegionSequence] Clearing removedPiecesConfig");
			GrimoraPlugin.ConfigCurrentRemovedPieces.Value = "";

			// MapNodeManager.Instance.FindAndSetActiveNodeInteractable();

			ViewManager.Instance.Controller.LockState = ViewLockState.Unlocked;

			// GrimoraPlugin.Log.LogDebug($"[CompleteRegionSequence] No longer ChangingRegion");

			SetAllNodesActive();
			ChangingRegion = false;
		}

		public override IEnumerator UnrollingSequence(float unrollSpeed)
		{
			// StoryEventsData.SetEventCompleted(StoryEvent.GrimoraReachedTable, true);

			TableRuleBook.Instance.SetOnBoard(false);

			GrimoraPlugin.Log.LogDebug($"[ChessboardMap.UnrollingSequence] Setting each piece game object active to false");
			pieces.ForEach(delegate(ChessboardPiece x) { x.gameObject.SetActive(false); });
			// yield return new WaitForSeconds(0.5f);

			UpdateVisuals();

			GrimoraPlugin.Log.LogDebug($"[ChessboardMap.UnrollingSequence] Playing map anim enter");
			// base.mapAnim.speed = 1f;
			mapAnim.Play("enter", 0, 0f);
			yield return new WaitForSeconds(0.25f);

			// todo: play sound?

			// base.mapAnim.speed = unrollSpeed;
			// yield return new WaitForSeconds(0.15f);

			// GrimoraPlugin.Log.LogDebug($"[ChessboardMap.UnrollingSequence] Setting dynamicElements [{__instance.dynamicElementsParent}] to active");
			dynamicElementsParent.gameObject.SetActive(true);

			// for checking which nodes are active/inactive
			RenameMapNodesWithGridCoords();

			SetupGamePieces();

			yield return HandleActivatingChessPieces();

			HandlePlayerMarkerPosition();

			if (!DialogueEventsData.EventIsPlayed("FinaleGrimoraMapShown"))
			{
				yield return new WaitForSeconds(0.5f);
				yield return TextDisplayer.Instance.PlayDialogueEvent("FinaleGrimoraMapShown",
					TextDisplayer.MessageAdvanceMode.Input);
			}

			MapNodeManager.Instance.FindAndSetActiveNodeInteractable();

			SaveManager.SaveToFile();
		}

		public void SetupGamePieces()
		{
			GrimoraPlugin.Log.LogDebug($"[SetupGamePieces] Setting up game pieces");

			HandleChessboardSetup();

			if (GrimoraPlugin.ConfigRoyalThirdBossDead.Value)
			{
				GrimoraPlugin.Log.LogDebug($"[SetupGamePieces] Royal defeated");
				activeChessboard.PlaceBossPiece("GrimoraBoss");
				activeChessboard.ActiveBossType = BaseBossExt.GrimoraOpponent;
			}
			else if (GrimoraPlugin.ConfigDoggySecondBossDead.Value)
			{
				GrimoraPlugin.Log.LogDebug($"[SetupGamePieces] Doggy defeated");
				activeChessboard.PlaceBossPiece("RoyalBoss");
				activeChessboard.ActiveBossType = BaseBossExt.RoyalOpponent;
			}
			else if (GrimoraPlugin.ConfigKayceeFirstBossDead.Value)
			{
				GrimoraPlugin.Log.LogDebug($"[SetupGamePieces] Kaycee defeated");
				activeChessboard.PlaceBossPiece("DoggyBoss");
				activeChessboard.ActiveBossType = BaseBossExt.DoggyOpponent;
			}
			else
			{
				GrimoraPlugin.Log.LogDebug($"[SetupGamePieces] No bosses defeated yet, creating Kaycee");
				activeChessboard.PlaceBossPiece("KayceeBoss");
				activeChessboard.ActiveBossType = BaseBossExt.KayceeOpponent;
			}

			activeChessboard.PlaceBlockerPieces();
			activeChessboard.PlaceChestPieces();
			activeChessboard.PlaceEnemyPieces();

			// GrimoraPlugin.Log.LogDebug($"[SetupGamePieces] Finished setting up game pieces." +
			//                            $" Current active list before {PiecesDelimited}");

			var removedList = RemovedPieces;

			GrimoraPlugin.Log.LogDebug($"[SetupGamePieces] " +
			                           $" Current removed list before {GrimoraPlugin.ConfigCurrentRemovedPieces.Value}");

			activePieces = pieces
				.Where(p => !removedList.Contains(p.name))
				.ToList();
		}

		private void HandleChessboardSetup()
		{
			// GrimoraPlugin.Log.LogDebug($"[HandleChessboardSetup] Before setting chess board idx [{currentChessboardIndex}]");
			currentChessboardIndex = GrimoraPlugin.ConfigCurrentChessboardIndex.Value;
			// GrimoraPlugin.Log.LogDebug($"[HandleChessboardSetup] After setting chess board idx [{currentChessboardIndex}]");

			if (ChangingRegion)
			{
				if (currentChessboardIndex == 4)
				{
					currentChessboardIndex = -1;
				}

				GrimoraPlugin.ConfigCurrentChessboardIndex.Value = ++currentChessboardIndex;
				// GrimoraPlugin.Log.LogDebug($"[HandleChessboardSetup] -> Setting new chessboard idx [{currentChessboardIndex}]");
				activeChessboard = Chessboards[currentChessboardIndex];

				// set the updated position to spawn the player in
				GrimoraSaveData.Data.gridX = activeChessboard.GetPlayerNode().GridX;
				GrimoraSaveData.Data.gridY = activeChessboard.GetPlayerNode().GridY;
			}

			// GrimoraPlugin.Log.LogDebug($"[HandleChessboardSetup] ActiveChessboard [{activeChessboard}] Chessboards size [{Chessboards.Count}]");
			activeChessboard ??= Chessboards[currentChessboardIndex];
		}

		public void AddPieceToRemovedPiecesConfig(string pieceName)
		{
			GrimoraPlugin.ConfigCurrentRemovedPieces.Value += "," + pieceName + ",";
		}

		private static void SetAllNodesActive()
		{
			// GrimoraPlugin.Log.LogDebug($"[SetAllNodesActive] setting all chess nodes active");
			foreach (var zone in ChessboardNavGrid.instance.zones)
			{
				zone.gameObject.SetActive(true);
				// UnityExplorer.ExplorerCore.Log(zone.GetComponent<ChessboardMapNode>().isActiveAndEnabled);
			}
		}

		private void HandlePlayerMarkerPosition()
		{
			int x = GrimoraSaveData.Data.gridX;
			int y = GrimoraSaveData.Data.gridY;

			var occupyingPiece = GetMapNodeFromXY(x, y).OccupyingPiece;
			bool isPlayerOccupied = occupyingPiece != null && PlayerMarker.Instance.name == occupyingPiece.name;

			// GrimoraPlugin.Log.LogDebug($"[HandlePlayerMarkerPosition] isPlayerOccupied? [{isPlayerOccupied}]");

			if (ChangingRegion || occupyingPiece != null && !isPlayerOccupied)
			{
				// GrimoraPlugin.Log.LogDebug($"[HandlePlayerMarkerPosition] Is boss transition or current active node has an already occupying piece");
				var allOpenPathNodes = activeChessboard.GetOpenPathNodes();

				// GrimoraPlugin.Log.LogDebug($"[HandlePlayerMarkerPosition] AllOpenNodes count [{allOpenPathNodes.Count}]");

				for (var i = allOpenPathNodes.Count - 1; i >= 0; i--)
				{
					x = allOpenPathNodes[i].GridX;
					y = allOpenPathNodes[i].GridY;

					// GrimoraPlugin.Log.LogDebug($"[HandlePlayerMarkerPosition] index [{i}] OpenPathNode is x[{x}]y[{y}]");

					occupyingPiece = GetMapNodeFromXY(x, y).OccupyingPiece;

					if (occupyingPiece is null)
					{
						break;
					}
				}
			}

			MapNodeManager.Instance.ActiveNode = navGrid.zones[x, y].GetComponent<MapNode>();
			// GrimoraPlugin.Log.LogDebug($"[SetupGamePieces] MapNodeManager ActiveNode is x[{x}]y[{y}]");

			// GrimoraPlugin.Log.LogDebug($"[SetupGamePieces] SetPlayerAdjacentNodesActive");
			ChessboardNavGrid.instance.SetPlayerAdjacentNodesActive();

			// GrimoraPlugin.Log.LogDebug($"[SetupGamePieces] Setting player position to active node");
			PlayerMarker.Instance.transform.position = MapNodeManager.Instance.ActiveNode.transform.position;
		}

		private static ChessboardMapNode GetMapNodeFromXY(int x, int y)
		{
			return ChessboardNavGrid.instance.zones[x, y].GetComponent<ChessboardMapNode>();
		}

		private IEnumerator HandleActivatingChessPieces()
		{
			// GrimoraPlugin.Log.LogDebug($"[HandleActivatingChessPieces] active pieces before setting if active " +
			//                            $"[{string.Join(",", activePieces.Select(_ => _.name))}]");

			pieces.ForEach(delegate(ChessboardPiece piece)
			{
				if (activePieces.Contains(piece))
				{
					// GrimoraPlugin.Log.LogDebug($"[HandleSaveStatesForPieces] Setting active [{piece.name}]");
					piece.gameObject.SetActive(true);
				}
				else
				{
					// GrimoraPlugin.Log.LogDebug($"[HandleSaveStatesForPieces] Setting inactive [{piece.gameObject}]] Node is active? [{piece.MapNode.isActiveAndEnabled}]]");
					piece.gameObject.SetActive(false);
					piece.MapNode.OccupyingPiece = null;
					// GrimoraPlugin.Log.LogDebug($"[HandleSaveStatesForPieces] -> is node active and enabled? [{piece.MapNode.isActiveAndEnabled}]]");
				}

				piece.Hide(true);
			});

			// GrimoraPlugin.Log.LogDebug("[HandleSaveStatesForPieces] Finished UpdatingSaveStates of pieces");

			yield return new WaitForSeconds(0.05f);

			yield return ShowPiecesThatAreActive();
		}

		private IEnumerator ShowPiecesThatAreActive()
		{
			foreach (var piece in pieces.Where(piece => piece.gameObject.activeInHierarchy))
			{
				// GrimoraPlugin.Log.LogDebug($"-> Piece [{piece.name}] saveId [{piece.saveId}] is active in hierarchy, calling Show method");
				piece.Show();
				yield return new WaitForSeconds(0.020f);
			}

			// GrimoraPlugin.Log.LogDebug("[HandleSaveStatesForPieces] Finished showing all active pieces");
		}

		private static void UpdateVisuals()
		{
			// GrimoraPlugin.Log.LogDebug($"[{this.GetType()}] Updating visuals");
			TableVisualEffectsManager.Instance.SetFogPlaneShown(true);
			CameraEffects.Instance.SetFogEnabled(true);
			CameraEffects.Instance.SetFogAlpha(0f);
			CameraEffects.Instance.TweenFogAlpha(0.6f, 1f);

			TableVisualEffectsManager.Instance.SetDustParticlesActive(!RunState.CurrentMapRegion.dustParticlesDisabled);
		}

		private void OnViewChanged(View newView, View oldView)
		{
			// GrimoraPlugin.Log.LogDebug($"[OnViewChanged] OnViewChanged called");
			switch (oldView)
			{
				case View.MapDefault when newView == View.MapDeckReview:
				{
					if (MapNodeManager.Instance != null)
					{
						// GrimoraPlugin.Log.LogDebug($"[OnViewChanged] SetAllNodesInteractable false");
						MapNodeManager.Instance.SetAllNodesInteractable(false);
					}

					DeckReviewSequencer.Instance.SetDeckReviewShown(true, transform, DefaultPosition);
					break;
				}
				case View.MapDeckReview when newView == View.MapDefault:
				{
					DeckReviewSequencer.Instance.SetDeckReviewShown(false, transform, DefaultPosition);
					if (MapNodeManager.Instance != null)
					{
						// GrimoraPlugin.Log.LogDebug($"[OnViewChanged] SetAllNodesInteractable true");
						// MapNodeManager.Instance.SetAllNodesInteractable(true);
						GrimoraPlugin.Log.LogDebug($"[OnViewChanged] PlayerMarker transform {PlayerMarker.Instance.transform}");
						MapNodeManager.Instance.FindAndSetActiveNodeInteractable();
					}

					break;
				}
			}
		}

		public void RenameMapNodesWithGridCoords()
		{
			if (string.Equals(
				navGrid.zones[0, 0].name,
				"ChessBoardMapNode",
				StringComparison.InvariantCultureIgnoreCase)
			)
			{
				// GrimoraPlugin.Log.LogDebug($"ChessboardMap.UnrollingSequence] Renaming all map nodes");

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
		}
	}
}