using System.Collections;
using DiskCardGame;
using Unity.Cloud.UserReporting.Plugin.SimpleJson;
using UnityEngine;
using static GrimoraMod.GrimoraPlugin;

namespace GrimoraMod;

public class ChessboardMapExt : GameMap
{
	[SerializeField] internal NavigationGrid navGrid;

	[SerializeField] internal List<ChessboardPiece> pieces;

	private bool _toggleCardsLeftInDeck = false;
	private bool _btnDeckView = false;
	private bool _btnGetUp = false;

	public bool HasNotPlayedDialogueOnce =>
		ConfigHelper.Instance.HammerDialogueOption == 1 && hasNotPlayedAllHammerDialogue < 3;

	public int hasNotPlayedAllHammerDialogue = 0;

	public GrimoraChessboard ActiveChessboard { get; set; }

	private List<GrimoraChessboard> _chessboards;

	public new static ChessboardMapExt Instance => GameMap.Instance as ChessboardMapExt;

	public void SetAnimActiveIfInactive()
	{
		GameObject anim = gameObject.transform.GetChild(0).gameObject;
		if (!anim.activeInHierarchy)
		{
			anim.SetActive(true);
		}
	}

	public ChessboardEnemyPiece BossPiece => ActiveChessboard.BossPiece;

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

	public void LoadData()
	{
		if (_chessboards == null)
		{
			string jsonString = File.ReadAllText(
				FileUtils.FindFileInPluginDir(
					ConfigHelper.Instance.isDevModeEnabled
						? "GrimoraChessboardDevMode.json"
						: "GrimoraChessboardsStatic.json"
				)
			);

			_chessboards = ParseJson(
				SimpleJson.DeserializeObject<List<List<List<int>>>>(jsonString)
			);
		}
	}

	private static List<GrimoraChessboard> ParseJson(IEnumerable<List<List<int>>> chessboardsFromJson)
	{
		return chessboardsFromJson.Select((board, idx) => new GrimoraChessboard(board, idx)).ToList();
	}

	private void Awake()
	{
		ViewManager instance = ViewManager.Instance;
		instance.ViewChanged = (Action<View, View>)Delegate
			.Combine(instance.ViewChanged, new Action<View, View>(OnViewChanged));

		gameObject.AddComponent<DebugHelper>();
	}

	public static string[] CardsLeftInDeck => CardDrawPiles3D
		.Instance
		.Deck
		.cards
		.OrderBy(info => info.name)
		.Select(_ => _.name.Replace("GrimoraMod_", ""))
		.ToArray();

	private void OnGUI()
	{
		if (GrimoraGameFlowManager.Instance.CurrentGameState != GameState.FirstPerson3D)
		{
			_btnDeckView = GUI.Button(
				new Rect(0, 0, 100, 50),
				"Deck View"
			);

			_btnGetUp = GUI.Button(
				new Rect(200, 0, 100, 50),
				"Get Up"
			);

			if (ViewManager.Instance.Controller.LockState == ViewLockState.Unlocked)
			{
				if (_btnDeckView)
				{
					switch (ViewManager.Instance.CurrentView)
					{
						case View.MapDeckReview:
							ViewManager.Instance.SwitchToView(
								SpecialNodeHandler.Instance.cardChoiceSequencer.transform.childCount > 1
									? View.Choices
									: View.MapDefault
							);

							break;
						case View.Choices:
						case View.MapDefault:
							ViewManager.Instance.SwitchToView(View.MapDeckReview);
							break;
					}
				}
				else if (_btnGetUp && GrimoraGameFlowManager.Instance.CurrentGameState == GameState.Map)
				{
					Log.LogDebug($"Transitioning to first person");
					GrimoraGameFlowManager.Instance.TransitionToFirstPerson();
				}
			}
		}
		else
		{
			_btnDeckView = false;
			_btnGetUp = false;
		}

		var resetRunBtn = GUI.Button(
			new Rect(100, 0, 100, 50),
			"Reset Run"
		);


		if (TurnManager.Instance.Opponent is not null
		    && CardDrawPiles3D.Instance.Deck is not null)
		{
			_toggleCardsLeftInDeck = GUI.Toggle(
				new Rect(
					20,
					(ConfigHelper.Instance.isDevModeEnabled
						? 360
						: 60),
					150,
					15
				),
				_toggleCardsLeftInDeck,
				"Cards Left in Deck"
			);
		}

		if (resetRunBtn)
		{
			ConfigHelper.Instance.ResetRun();
		}

		if (ConfigHelper.Instance.EnableCardsLeftInDeckView && _toggleCardsLeftInDeck)
		{
			GUI.SelectionGrid(
				new Rect(25, 350, 150, CardDrawPiles3D.Instance.Deck.cards.Count * 25f),
				-1,
				CardsLeftInDeck,
				1
			);
		}
	}

	public IEnumerator CompleteRegionSequence()
	{
		ViewManager.Instance.Controller.SwitchToControlMode(ViewController.ControlMode.Map);
		ViewManager.Instance.Controller.LockState = ViewLockState.Locked;

		SaveManager.SaveToFile();

		BossDefeated = false;

		ChangingRegion = true;

		ViewManager.Instance.Controller.LockState = ViewLockState.Locked;

		ViewManager.Instance.SwitchToView(View.MapDefault);

		RunState.CurrentMapRegion.FadeInAmbientAudio();

		MapNodeManager.Instance.SetAllNodesInteractable(false);

		// Log.LogDebug($"[CompleteRegionSequence] Looping audio");
		AudioController.Instance.SetLoopAndPlay("finalegrimora_ambience");
		AudioController.Instance.SetLoopVolumeImmediate(0f);
		AudioController.Instance.FadeInLoop(1f, 1f);

		ClearBoardForChangingRegion();

		SetAllNodesActive();

		// this will call Unrolling and Showing the player Marker
		yield return GameMap.Instance.ShowMapSequence();

		ViewManager.Instance.Controller.LockState = ViewLockState.Unlocked;

		ChangingRegion = false;
		Log.LogDebug($"[CompleteRegionSequence] No longer ChangingRegion");
	}

	private void ClearBoardForChangingRegion()
	{
		Log.LogDebug($"[CompleteRegionSequence] Clearing and destroying pieces");
		pieces.RemoveAll(
			delegate(ChessboardPiece piece)
			{
				// piece.gameObject.SetActive(false);
				piece.MapNode.OccupyingPiece = null;
				Destroy(piece.gameObject);

				return true;
			}
		);

		// GrimoraPlugin.Log.LogDebug($"[CompleteRegionSequence] Clearing removedPiecesConfig");
		ConfigHelper.Instance.ResetRemovedPieces();
	}

	public override IEnumerator UnrollingSequence(float unrollSpeed)
	{
		TableRuleBook.Instance.SetOnBoard(false);

		pieces.ForEach(delegate(ChessboardPiece x) { x.gameObject.SetActive(false); });

		UpdateVisuals();

		// base.mapAnim.speed = 1f;
		mapAnim.Play("enter", 0, 0f);

		dynamicElementsParent.gameObject.SetActive(true);

		// for checking which nodes are active/inactive
		if (ConfigHelper.Instance.isDevModeEnabled) RenameMapNodesWithGridCoords();

		// if the boss piece exists in the removed pieces,
		// this means the game didn't complete clearing the board for changing the region
		if (ConfigHelper.Instance.RemovedPieces.Exists(piece => piece.Contains("BossPiece")))
		{
			ClearBoardForChangingRegion();
		}

		UpdateActiveChessboard();

		ActiveChessboard.SetupBoard();

		yield return HandleActivatingChessPieces();

		ActiveChessboard.UpdatePlayerMarkerPosition(ChangingRegion);

		if (!DialogueEventsData.EventIsPlayed("FinaleGrimoraMapShown"))
		{
			yield return TextDisplayer.Instance.PlayDialogueEvent(
				"FinaleGrimoraMapShown",
				TextDisplayer.MessageAdvanceMode.Input
			);
		}

		StoryEventsData.SetEventCompleted(StoryEvent.GrimoraReachedTable);

		SaveManager.SaveToFile();
	}

	private void UpdateActiveChessboard()
	{
		int currentChessboardIndex = ConfigHelper.Instance.CurrentChessboardIndex;
		Log.LogDebug($"[HandleChessboardSetup] Before setting chess board idx [{currentChessboardIndex}]");

		if (ChangingRegion)
		{
			if (++currentChessboardIndex >= 4)
			{
				currentChessboardIndex = 0;
			}

			ConfigHelper.Instance.CurrentChessboardIndex = currentChessboardIndex;
			Log.LogDebug($"[HandleChessboardSetup] -> Setting new chessboard idx [{currentChessboardIndex}]");
			ActiveChessboard = Chessboards[currentChessboardIndex];

			ActiveChessboard.SetSavePositions();
		}

		Log.LogDebug($"[HandleChessboardSetup] Chessboard [{ActiveChessboard}] Chessboards [{Chessboards.Count}]");
		ActiveChessboard ??= Chessboards[currentChessboardIndex];
	}


	private static void SetAllNodesActive()
	{
		foreach (var zone in ChessboardNavGrid.instance.zones)
		{
			zone.gameObject.SetActive(true);
			// UnityExplorer.ExplorerCore.Log(zone.GetComponent<ChessboardMapNode>().isActiveAndEnabled);
		}
	}

	private IEnumerator HandleActivatingChessPieces()
	{
		var removedList = ConfigHelper.Instance.RemovedPieces;

		// pieces will contain the pieces just placed
		var activePieces = pieces
			.Where(p => !removedList.Contains(p.name))
			.ToList();

		pieces.RemoveAll(
			delegate(ChessboardPiece piece)
			{
				bool toRemove = false;
				if (activePieces.Contains(piece))
				{
					piece.gameObject.SetActive(true);
				}
				else
				{
					piece.gameObject.SetActive(false);
					piece.MapNode.OccupyingPiece = null;
					toRemove = true;
				}

				piece.Hide(true);
				return toRemove;
			}
		);

		yield return new WaitForSeconds(0.05f);

		yield return ShowPiecesThatAreActive();
	}

	private IEnumerator ShowPiecesThatAreActive()
	{
		foreach (var piece in pieces.Where(piece => piece.gameObject.activeInHierarchy))
		{
			piece.Show();
			yield return new WaitForSeconds(0.02f);
		}
	}

	private static void UpdateVisuals()
	{
		TableVisualEffectsManager.Instance.SetFogPlaneShown(true);
		CameraEffects.Instance.SetFogEnabled(true);
		CameraEffects.Instance.SetFogAlpha(0f);
		CameraEffects.Instance.TweenFogAlpha(0.6f, 1f);

		TableVisualEffectsManager.Instance.SetDustParticlesActive(!RunState.CurrentMapRegion.dustParticlesDisabled);
	}

	private void OnViewChanged(View newView, View oldView)
	{
		switch (oldView)
		{
			case View.Choices when newView == View.MapDeckReview:
			case View.MapDefault when newView == View.MapDeckReview:
			{
				if (MapNodeManager.Instance != null)
				{
					MapNodeManager.Instance.SetAllNodesInteractable(false);
				}

				DeckReviewSequencer.Instance.SetDeckReviewShown(true, transform, DefaultPosition);
				break;
			}
			case View.MapDeckReview when newView == View.Choices:
			case View.MapDeckReview when newView == View.MapDefault:
			{
				DeckReviewSequencer.Instance.SetDeckReviewShown(false, transform, DefaultPosition);
				if (MapNodeManager.Instance != null)
				{
					ChessboardNavGrid.instance.SetPlayerAdjacentNodesActive();
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
			    StringComparison.OrdinalIgnoreCase
		    )
		   )
		{
			var zones = ChessboardNavGrid.instance.zones;
			for (var i = 0; i < zones.GetLength(0); i++)
			{
				for (var i1 = 0; i1 < zones.GetLength(1); i1++)
				{
					var obj = ChessboardNavGrid.instance.zones[i, i1].GetComponent<ChessboardMapNode>();
					obj.name = $"ChessboardMapNode_x{i}y{i1}";
				}
			}
		}
	}

	public override IEnumerator RerollingSequence()
	{
		foreach (var piece in pieces.Where(piece => piece.gameObject.activeInHierarchy))
		{
			piece.Hide();
		}

		PlayerMarker.Instance.Hide();
		CameraEffects.Instance.TweenFogAlpha(0f, 0.15f);
		TableVisualEffectsManager.Instance.SetFogPlaneShown(shown: false);
		CameraEffects.Instance.SetFogEnabled(fogEnabled: false);
		mapAnim.Play("exit", 0, 0f);
		dynamicElementsParent.gameObject.SetActive(value: false);
		yield break;
	}

	public override void OnHideMapImmediate()
	{
		mapAnim.Play("exit", 0, 1f);
	}

	public static void ChangeChessboardToExtendedClass()
	{
		ChessboardMapExt ext = ChessboardMap.Instance.gameObject.GetComponent<ChessboardMapExt>();

		if (ext is null)
		{
			ChessboardMap boardComp = ChessboardMap.Instance.gameObject.GetComponent<ChessboardMap>();
			boardComp.pieces.Clear();

			ext = ChessboardMap.Instance.gameObject.AddComponent<ChessboardMapExt>();

			ext.dynamicElementsParent = boardComp.dynamicElementsParent;
			ext.mapAnim = boardComp.mapAnim;
			ext.navGrid = boardComp.navGrid;
			ext.pieces = new List<ChessboardPiece>();
			ext.defaultPosition = boardComp.defaultPosition;

			Destroy(boardComp);
		}

		var initialStartingPieces = FindObjectsOfType<ChessboardPiece>();

		foreach (var piece in initialStartingPieces)
		{
			piece.MapNode.OccupyingPiece = null;
			piece.gameObject.SetActive(false);
			Destroy(piece.gameObject);
		}
	}
}
