using System.Collections;
using DiskCardGame;
using Unity.Cloud.UserReporting.Plugin.SimpleJson;
using UnityEngine;
using static GrimoraMod.GrimoraPlugin;

// This class is literally just meant to be able to view the deck review sequencer
namespace GrimoraMod;

public class ChessboardMapExt : ChessboardMap
{
	internal PrefabPieceHelper PrefabPieceHelper;

	public List<ChessboardPiece> ActivePieces => pieces;

	public GrimoraChessboard ActiveChessboard { get; set; }

	private List<GrimoraChessboard> _chessboards;

	public new static ChessboardMapExt Instance => GameMap.Instance as ChessboardMapExt;

	public void SetAnimActiveIfInactive()
	{
		GameObject anim = Instance.gameObject.transform.GetChild(0).gameObject;
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
			Log.LogDebug($"[ChessboardMapExt] Loading json boards");
			string jsonString = File.ReadAllText(FileUtils.FindFileInPluginDir("GrimoraChessboardsStatic.json"));

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
		PrefabPieceHelper = new PrefabPieceHelper();

		Log.LogDebug($"[MapExt] Setting on view changed");
		ViewManager instance = ViewManager.Instance;
		instance.ViewChanged = (Action<View, View>)Delegate
			.Combine(instance.ViewChanged, new Action<View, View>(OnViewChanged));

		Log.LogDebug($"[MapExt] Adding debug helper");
		gameObject.AddComponent<DebugHelper>();
	}

	private void OnGUI()
	{
		var deckViewBtn = GUI.Button(
			new Rect(0, 0, 100, 50),
			"Deck View"
		);

		var deckResetBtn = GUI.Button(
			new Rect(100, 0, 100, 50),
			"Reset Deck"
		);

		var resetRunBtn = GUI.Button(
			new Rect(200, 0, 100, 50),
			"Reset Run"
		);

		if (deckViewBtn)
		{
			switch (ViewManager.Instance.CurrentView)
			{
				case View.MapDeckReview:
					ViewManager.Instance.SwitchToView(View.MapDefault);
					break;
				case View.MapDefault:
					ViewManager.Instance.SwitchToView(View.MapDeckReview);
					break;
			}
		}
		else if (resetRunBtn)
		{
			ConfigHelper.ResetRun();
		}
		else if (deckResetBtn)
		{
			ConfigHelper.ResetDeck();
		}
	}

	public IEnumerator CompleteRegionSequence()
	{
		PrefabPieceHelper.ChangeBlockerPieceForRegion();
		ViewManager.Instance.Controller.SwitchToControlMode(ViewController.ControlMode.Map);
		ViewManager.Instance.Controller.LockState = ViewLockState.Locked;

		SaveManager.SaveToFile();

		Instance.BossDefeated = false;

		Log.LogDebug($"[CompleteRegionSequence] Starting CompleteRegionSequence");
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

	private static void ClearBoardForChangingRegion()
	{
		Log.LogDebug($"[CompleteRegionSequence] Clearing and destroying pieces");
		Instance.pieces.RemoveAll(delegate(ChessboardPiece piece)
		{
			// piece.gameObject.SetActive(false);
			piece.MapNode.OccupyingPiece = null;
			Destroy(piece.gameObject);

			return true;
		});

		// GrimoraPlugin.Log.LogDebug($"[CompleteRegionSequence] Clearing removedPiecesConfig");
		ConfigHelper.Instance._configCurrentRemovedPieces.Value = "";
	}

	public override IEnumerator UnrollingSequence(float unrollSpeed)
	{
		TableRuleBook.Instance.SetOnBoard(false);

		Log.LogDebug($"[UnrollingSequence] Setting each piece game object active to false");
		Instance.pieces.ForEach(delegate(ChessboardPiece x) { x.gameObject.SetActive(false); });
		// yield return new WaitForSeconds(0.5f);

		UpdateVisuals();

		// Log.LogDebug($"[ChessboardMap.UnrollingSequence] Playing map anim enter");
		// base.mapAnim.speed = 1f;
		mapAnim.Play("enter", 0, 0f);
		yield return new WaitForSeconds(0.25f);

		// todo: play sound?

		// base.mapAnim.speed = unrollSpeed;
		// yield return new WaitForSeconds(0.15f);

		// Log.LogDebug($"[UnrollingSequence] Setting dynamicElements [{dynamicElementsParent}] to active");
		dynamicElementsParent.gameObject.SetActive(true);

		// for checking which nodes are active/inactive
		if (ConfigHelper.Instance.isDevModeEnabled) RenameMapNodesWithGridCoords();

		// if the boss piece exists in the removed pieces,
		// this means the game didn't complete clearing the board for changing the region
		if (ConfigHelper.Instance.RemovedPieces.Exists(piece => piece.Contains("Boss")))
		{
			ClearBoardForChangingRegion();
		}

		UpdateActiveChessboard();

		ActiveChessboard.SetupBoard();

		yield return HandleActivatingChessPieces();

		ActiveChessboard.UpdatePlayerMarkerPosition(ChangingRegion);

		if (!DialogueEventsData.EventIsPlayed("FinaleGrimoraMapShown"))
		{
			yield return new WaitForSeconds(0.5f);
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
		// GrimoraPlugin.Log.LogDebug($"[SetAllNodesActive] setting all chess nodes active");
		foreach (var zone in ChessboardNavGrid.instance.zones)
		{
			zone.gameObject.SetActive(true);
			// UnityExplorer.ExplorerCore.Log(zone.GetComponent<ChessboardMapNode>().isActiveAndEnabled);
		}
	}

	private IEnumerator HandleActivatingChessPieces()
	{
		// GrimoraPlugin.Log.LogDebug($"[HandleActivatingChessPieces] active pieces before setting if active " +
		//                            $"[{string.Join(",", activePieces.Select(_ => _.name))}]");

		var removedList = ConfigHelper.Instance.RemovedPieces;

		// pieces will contain the pieces just placed
		var activePieces = Instance.pieces
			.Where(p => !removedList.Contains(p.name))
			.ToList();

		Instance.pieces.RemoveAll(delegate(ChessboardPiece piece)
		{
			bool toRemove = false;
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
				toRemove = true;
				// GrimoraPlugin.Log.LogDebug($"[HandleSaveStatesForPieces] -> is node active and enabled? [{piece.MapNode.isActiveAndEnabled}]]");
			}

			piece.Hide(true);
			return toRemove;
		});

		// GrimoraPlugin.Log.LogDebug("[HandleSaveStatesForPieces] Finished UpdatingSaveStates of pieces");

		yield return new WaitForSeconds(0.05f);

		yield return ShowPiecesThatAreActive();
	}

	private IEnumerator ShowPiecesThatAreActive()
	{
		foreach (var piece in Instance.pieces.Where(piece => piece.gameObject.activeInHierarchy))
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
					// Log.LogDebug($"[OnViewChanged] PlayerMarker transform {PlayerMarker.Instance.transform}");
					MapNodeManager.Instance.FindAndSetActiveNodeInteractable();
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
