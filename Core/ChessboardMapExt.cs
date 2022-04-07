using System.Collections;
using System.Reflection;
using System.Runtime.Serialization.Json;
using System.Text;
using DiskCardGame;
using HarmonyLib;
using Sirenix.Utilities;
using Unity.Cloud.UserReporting.Plugin.SimpleJson;
using UnityEngine;
using static GrimoraMod.GrimoraPlugin;

namespace GrimoraMod;

public class ChessboardMapExt : GameMap
{
	[SerializeField] internal NavigationGrid navGrid;

	[SerializeField] internal List<ChessboardPiece> pieces;

	internal DebugHelper _debugHelper;

	public bool StartAtTwinGiants;

	public bool StartAtBonelord;

	private bool _toggleCardsLeftInDeck = false;

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
	
	public GrimoraChessboard ActiveChessboard { get; set; }
	
	private List<GrimoraChessboard> _chessboards;

	private List<GrimoraChessboard> Chessboards
	{
		get
		{
			LoadData();
			return _chessboards;
		}
	}

	public Dictionary<EncounterJson, EncounterBlueprintData> CustomBlueprintsRegions { get; set; } = new();
	public Dictionary<EncounterJson, EncounterBlueprintData> CustomBlueprintsBosses { get; set; } = new();
	
	private Dictionary<EncounterJson, EncounterBlueprintData> _customBlueprints;
	
	public Dictionary<EncounterJson, EncounterBlueprintData> CustomBlueprints
	{
		get
		{
			LoadData();
			return _customBlueprints;
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
		
		if (_customBlueprints == null)
		{
			_customBlueprints = new Dictionary<EncounterJson, EncounterBlueprintData>();
			string[] encounters = Directory.GetFiles(
					Assembly.GetExecutingAssembly().Location.Replace("GrimoraMod.dll", ""), "GrimoraMod_Encounter*", SearchOption.AllDirectories)
			 .Select(File.ReadAllText)
			 .ToArray();

			foreach (var encounterFile in encounters)
			{
				using var ms = new MemoryStream(Encoding.Unicode.GetBytes(encounterFile));
				DataContractJsonSerializer deserial = new DataContractJsonSerializer(typeof(EncounterJsonUtil.EncountersFromJson));
				EncounterJsonUtil.EncountersFromJson encountersFromJson = (EncounterJsonUtil.EncountersFromJson)deserial.ReadObject(ms);
				foreach (var encounterFromJson in encountersFromJson.encounters)
				{
					EncounterBlueprintData blueprint = encounterFromJson.BuildEncounter();
					if (encounterFromJson.blueprintType.Equals("Region", StringComparison.InvariantCultureIgnoreCase))
					{
						CustomBlueprintsRegions.Add(encounterFromJson, blueprint);
					}
					else
					{
						CustomBlueprintsBosses.Add(encounterFromJson, blueprint);
					}
					_customBlueprints.Add(encounterFromJson, blueprint);
				}
			}
			Log.LogDebug($"Final custom blueprint names: [{_customBlueprints.Join(kv => kv.Value.name)}]");
			_debugHelper.SetupEncounterData();
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

		_debugHelper = gameObject.AddComponent<DebugHelper>();
	}

	public static string[] CardsLeftInDeck => CardDrawPiles3D
		.Instance
		.Deck
		.cards
		.OrderBy(info => info.name)
		.Select(_ => _.name.Replace($"{GUID}_", ""))
		.ToArray();

	private void OnGUI()
	{
		if (ConfigHelper.Instance.isDevModeEnabled)
		{
			StartAtTwinGiants = GUI.Toggle(
				new Rect(Screen.width - 200, 180, 200, 20),
				StartAtTwinGiants, 
				"Start at Twin Giants"
			);

			StartAtBonelord = GUI.Toggle(
				new Rect(Screen.width - 200, 200, 200, 20),
				StartAtBonelord, 
				"Start at Bonelord"
			);
		}

		if (GrimoraGameFlowManager.Instance.CurrentGameState == GameState.CardBattle)
		{
			_toggleCardsLeftInDeck = GUI.Toggle(
				new Rect(
					(ConfigHelper.Instance.isDevModeEnabled ? Screen.width - 400 : 20),
					20,
					150,
					15
				),
				_toggleCardsLeftInDeck,
				"Cards Left in Deck"
			);

			if (ConfigHelper.Instance.EnableCardsLeftInDeckView && _toggleCardsLeftInDeck)
			{
				GUI.SelectionGrid(
					new Rect(
						(ConfigHelper.Instance.isDevModeEnabled ? Screen.width - 400 : 25), 
						Screen.height * 0.75f, 
						150, 
						CardDrawPiles3D.Instance.Deck.cards.Count * 25f
						),
					-1,
					CardsLeftInDeck,
					2
				);
			}
		}
	}

	public IEnumerator CompleteRegionSequence()
	{
		ViewManager.Instance.Controller.SwitchToControlMode(ViewController.ControlMode.Map);
		ViewManager.Instance.SetViewLocked();

		SaveManager.SaveToFile();

		BossDefeated = false;

		ChangingRegion = true;

		ViewManager.Instance.SetViewLocked();

		ViewManager.Instance.SwitchToView(View.MapDefault);

		RunState.CurrentMapRegion.FadeInAmbientAudio();

		MapNodeManager.Instance.SetAllNodesInteractable(false);

		AudioController.Instance.SetLoopAndPlay("finalegrimora_ambience");
		AudioController.Instance.SetLoopVolumeImmediate(0f);
		AudioController.Instance.FadeInLoop(1f, 1f);

		ClearBoardForChangingRegion();

		SetAllNodesActive();

		// this will call Unrolling and Showing the player Marker
		yield return GameMap.Instance.ShowMapSequence();

		ViewManager.Instance.SetViewUnlocked();

		ChangingRegion = false;
		Log.LogInfo($"[CompleteRegionSequence] No longer ChangingRegion");
	}

	private void ClearBoardForChangingRegion()
	{
		pieces.RemoveAll(
			delegate(ChessboardPiece piece)
			{
				piece.MapNode.OccupyingPiece = null;
				Destroy(piece.gameObject);
				return true;
			}
		);

		ConfigHelper.Instance.ResetRemovedPieces();
	}

	public override IEnumerator UnrollingSequence(float unrollSpeed)
	{
		if (FinaleDeletionWindowManager.instance)
		{
			Destroy(FinaleDeletionWindowManager.instance.gameObject);
		}
		
		StoryEventsData.SetEventCompleted(StoryEvent.GrimoraReachedTable, true);

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

		ActiveChessboard.SetupBoard(ChangingRegion || pieces.IsNullOrEmpty());

		yield return HandleActivatingChessPieces();

		ActiveChessboard.UpdatePlayerMarkerPosition(ChangingRegion);

		if (!DialogueEventsData.EventIsPlayed("FinaleGrimoraMapShown"))
		{
			yield return TextDisplayer.Instance.PlayDialogueEvent(
				"FinaleGrimoraMapShown",
				TextDisplayer.MessageAdvanceMode.Input
			);
		}

		ChangeStartDeckIfNotAlreadyChanged();

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

		ActiveChessboard ??= Chessboards[currentChessboardIndex];
		Log.LogDebug($"[HandleChessboardSetup] Chessboard [{ActiveChessboard}] Chessboards [{Chessboards.Count}]");
	}


	private static void SetAllNodesActive()
	{
		foreach (var zone in ChessboardNavGrid.instance.zones)
		{
			zone.gameObject.SetActive(true);
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
				if (MapNodeManager.Instance)
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
				if (MapNodeManager.Instance)
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

		if (ext.IsNull())
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

	private static void ChangeStartDeckIfNotAlreadyChanged()
	{
		if (GrimoraSaveUtil.DeckInfo.cardIds.IsNullOrEmpty())
		{
			Log.LogWarning($"Re-initializing player deck as there are no cardIds! This means the deck was never loaded correctly.");
			GrimoraSaveData.Data.Initialize();
		}
		else
		{
			try
			{
				List<CardInfo> grimoraDeck = GrimoraSaveUtil.DeckList;

				int graveDiggerCount = grimoraDeck.Count(info => info.name == "Gravedigger");
				int frankNSteinCount = grimoraDeck.Count(info => info.name == "FrankNStein");
				if (grimoraDeck.Count == 5 && graveDiggerCount == 3 && frankNSteinCount == 2)
				{
					Log.LogWarning($"[ChangeStartDeckIfNotAlreadyChanged] Starter deck needs reset");
					GrimoraSaveData.Data.Initialize();
				}
			}
			catch (Exception e)
			{
				Log.LogWarning($"[ChangingDeck] Had trouble retrieving deck list! Resetting deck. Current card Ids: [{GrimoraSaveUtil.DeckInfo.cardIds.Join()}]");
				GrimoraSaveData.Data.Initialize();
			}
		}
	}
}
