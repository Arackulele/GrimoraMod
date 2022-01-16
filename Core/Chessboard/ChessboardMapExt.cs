using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DiskCardGame;
using Unity.Cloud.UserReporting.Plugin.SimpleJson;
using UnityEngine;
using static GrimoraMod.GrimoraPlugin;
using Resources = GrimoraMod.Properties.Resources;

// This class is literally just meant to be able to view the deck review sequencer
namespace GrimoraMod
{
	public class ChessboardMapExt : ChessboardMap
	{
		private GrimoraChessboard _activeChessboard;

		private string[] buttonNames =
		{
			"Win Round", "Lose Round", "Deck View",
			"Place Chest"
		};

		private List<GrimoraChessboard> _chessboards;
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

		public ChessboardEnemyPiece BossPiece => _activeChessboard.BossPiece;

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
				return _chessboards;
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
						Instance._activeChessboard.PlaceChestPiece(0, 0);
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
			if (_chessboards == null)
			{
				GrimoraPlugin.Log.LogDebug($"[ChessboardMapExt] Loading json boards");
				string jsonString = Encoding.UTF8.GetString(Resources.GrimoraChessboardsStatic);

				_chessboards = ParseJson(
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
			StoryEventsData.SetEventCompleted(StoryEvent.GrimoraReachedTable);

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

			UpdateActiveChessboard();

			_activeChessboard.SetupBoard();

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

		private void UpdateActiveChessboard()
		{
			int currentChessboardIndex = ConfigCurrentChessboardIndex.Value;
			Log.LogDebug($"[HandleChessboardSetup] Before setting chess board idx [{currentChessboardIndex}]");

			if (ChangingRegion)
			{
				if (currentChessboardIndex++ == 4)
				{
					currentChessboardIndex = 0;
				}

				ConfigCurrentChessboardIndex.Value = currentChessboardIndex;
				Log.LogDebug($"[HandleChessboardSetup] -> Setting new chessboard idx [{currentChessboardIndex}]");
				_activeChessboard = Chessboards[currentChessboardIndex];

				_activeChessboard.SetSavePositions();
			}

			Log.LogDebug($"[HandleChessboardSetup] Chessboard [{_activeChessboard}] Chessboards [{Chessboards.Count}]");
			_activeChessboard ??= Chessboards[currentChessboardIndex];
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

			GrimoraPlugin.Log.LogDebug($"[HandlePlayerMarkerPosition] " +
			                           $"Player Marker name [{PlayerMarker.Instance.name}] " +
			                           $"x{x}y{y} coords");
			
			var occupyingPiece = GetMapNodeFromXY(x, y).OccupyingPiece;
			
			bool isPlayerOccupied = occupyingPiece is not null && PlayerMarker.Instance.name == occupyingPiece.name;

			GrimoraPlugin.Log.LogDebug($"[HandlePlayerMarkerPosition] isPlayerOccupied? [{isPlayerOccupied}]");

			if (ChangingRegion)
			{
				x = activeChessboard.GetPlayerNode().GridX;
				y = activeChessboard.GetPlayerNode().GridY;
			}
			
			MapNodeManager.Instance.ActiveNode = navGrid.zones[x, y].GetComponent<MapNode>();
			GrimoraPlugin.Log.LogDebug($"[SetupGamePieces] MapNodeManager ActiveNode is x[{x}]y[{y}]");

			GrimoraPlugin.Log.LogDebug($"[SetupGamePieces] SetPlayerAdjacentNodesActive");
			ChessboardNavGrid.instance.SetPlayerAdjacentNodesActive();

			GrimoraPlugin.Log.LogDebug($"[SetupGamePieces] Setting player position to active node");
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