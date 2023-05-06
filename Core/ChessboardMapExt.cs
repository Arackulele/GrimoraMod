using System.Collections;
using System.Reflection;
using System.Runtime.Serialization.Json;
using System.Text;
using DiskCardGame;
using GrimoraMod.Saving;
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

	internal DebugHelper debugHelper;

	private bool _toggleCardsLeftInDeck;

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

	public bool ChangingRegion { get; set; }

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

	
	
	
	
	private List<GrimoraChessboard> _kayceechessboards;


	private List<GrimoraChessboard> KayceeChessboards 
	{
		get
		{
			LoadData();
			return _kayceechessboards;
		}
	}
	private List<GrimoraChessboard> _sawyerchessboards;


	private List<GrimoraChessboard> SawyerChessboards 
	{
		get
		{
			LoadData();
			return _sawyerchessboards;
		}
	}
	private List<GrimoraChessboard> _royalchessboards;


	private List<GrimoraChessboard> RoyalChessboards 
	{
		get
		{
			LoadData();
			return _royalchessboards;
		}
	}
	private List<GrimoraChessboard> _grimorachessboards;


	private List<GrimoraChessboard> GrimoraChessboards 
	{
		get
		{
			LoadData();
			return _grimorachessboards;
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
		if (_kayceechessboards == null)
		{
			_kayceechessboards = LoadChessboardsFromFile("maps_kaycee.json");
		}
		if (_sawyerchessboards == null)
		{
			_sawyerchessboards = LoadChessboardsFromFile("maps_sawyer.json");
		}
		if (_royalchessboards == null)
		{
			_royalchessboards = LoadChessboardsFromFile("maps_royal.json");
		}
		if (_grimorachessboards == null)
		{
			_grimorachessboards = LoadChessboardsFromFile("maps_grimora.json");
		}
		
		
		if (_chessboards == null)
		{
			_chessboards = new List<GrimoraChessboard>();
			_chessboards.AddRange(KayceeChessboards);
			_chessboards.AddRange(SawyerChessboards);
			_chessboards.AddRange(RoyalChessboards );
			_chessboards.AddRange(GrimoraChessboards);
		}

		if (_customBlueprints == null)
		{
			_customBlueprints = new Dictionary<EncounterJson, EncounterBlueprintData>();
			string[] encounters = Directory.GetFiles(
					Assembly.GetExecutingAssembly().Location.Replace("GrimoraMod.dll", string.Empty), "GrimoraMod_Encounter*", SearchOption.AllDirectories)
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
			if(debugHelper)
			{
				debugHelper.SetupEncounterData();
			}
		}
	}

	private static List<GrimoraChessboard> LoadChessboardsFromFile(string fileName)
	{
		string jsonString = File.ReadAllText(FileUtils.FindFileInPluginDir(fileName));
		
		List<List<List<char>>> chessboardsFromJson = null;
		try
		{
			chessboardsFromJson = SimpleJson.DeserializeObject<List<List<List<char>>>>(jsonString);
		}
		catch (Exception)
		{
			chessboardsFromJson = ConvertMapToCorrectFormat(jsonString, fileName);
		}

		List<GrimoraChessboard> chessboards = new List<GrimoraChessboard>();
		for (int i = 0; i < chessboardsFromJson.Count; i++)
		{
			GrimoraChessboard chessboard = new GrimoraChessboard(chessboardsFromJson[i]);
			chessboards.Add(chessboard);
		}
		
		Debug.Log(fileName + " maps parsed");
		return chessboards;
	}

	private static List<List<List<char>>> ConvertMapToCorrectFormat(string jsonString, string fileName)
	{
		Log.LogError($"Failed to load map {fileName}. Map does not use strings!");
		List<List<List<int>>> chessboardsAsNumbers = null;
		try
		{
			chessboardsAsNumbers = SimpleJson.DeserializeObject<List<List<List<int>>>>(jsonString);
		}
		catch (Exception exception)
		{
			Log.LogError($"Failed to correct map {fileName}. Is there a something wrong with the JSON?");
			Log.LogError(exception);
		}
		
		List<List<List<char>>> chessboardsFromJson = new List<List<List<char>>>();
		foreach (List<List<int>> data in chessboardsAsNumbers)
		{
			List<List<char>> column = new List<List<char>>();
			foreach (List<int> row in data)
			{
				List<char> convertedRow = row.Select(a=>a.ToString()[0]).ToList();
				column.Add(convertedRow);
			}
			chessboardsFromJson.Add(column);
		}

		return chessboardsFromJson;
	}

	public static string[] CardsLeftInDeck => CardDrawPiles3D
	 .Instance
	 .Deck
	 .cards
	 .OrderBy(info => info.name)
	 .Select(info => info.name.Replace($"{GUID}_", string.Empty))
	 .ToArray();

	private void Awake()
	{
		ViewManager instance = ViewManager.Instance;
		instance.ViewChanged += OnViewChanged;

		if (ConfigHelper.Instance.IsDevModeEnabled && debugHelper.SafeIsUnityNull())
		{
			debugHelper = gameObject.AddComponent<DebugHelper>();
		}
	}

	private void Start()
	{
		EnableCandlesIfTheyAreDisabled();

		if (FinaleDeletionWindowManager.instance && FinaleDeletionWindowManager.instance.mainWindow.isActiveAndEnabled)
		{
			FinaleDeletionWindowManager.instance.mainWindow.gameObject.SetActive(false);
		}

		if (ConfigHelper.Instance.IsDevModeEnabled)
		{
			// for checking which nodes are active/inactive
			RenameMapNodesWithGridCoords();
		}

		if (!CryptManager.Instance.HandLight.gameObject.activeInHierarchy)
		{
			CryptManager.Instance.HandLight.gameObject.SetActive(true);
		}
		CryptManager.Instance.ResetHandLightRange(0.25f);
	}

	private void OnGUI()
	{
		if (GrimoraGameFlowManager.Instance.CurrentGameState == GameState.CardBattle)
		{
			_toggleCardsLeftInDeck = GUI.Toggle(
				new Rect(
					(ConfigHelper.Instance.IsDevModeEnabled ? 400 : 20),
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
						(ConfigHelper.Instance.IsDevModeEnabled ? 400 : 25),
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

		ChangingRegion = true;

		ViewManager.Instance.SetViewLocked();

		ViewManager.Instance.SwitchToView(View.MapDefault);

		RunState.CurrentMapRegion.FadeInAmbientAudio();

		MapNodeManager.Instance.SetAllNodesInteractable(false);

		AudioController.Instance.SetLoopAndPlay("finalegrimora_ambience");
		AudioController.Instance.SetLoopVolumeImmediate(0f);
		AudioController.Instance.FadeInLoop(1f, 1f);

		ClearBoardForChangingRegion();
		GrimoraRunState.CurrentRun.regionTier++;

		SetAllNodesActive();

		// this will call Unrolling and Showing the player Marker
		yield return GameMap.Instance.ShowMapSequence();

		ViewManager.Instance.SetViewUnlocked();

		ChangingRegion = false;
		Log.LogInfo($"[CompleteRegionSequence] No longer ChangingRegion");
	}

	public void ClearBoardForChangingRegion()
	{
		pieces.RemoveAll(
			delegate(ChessboardPiece piece)
			{
				piece.MapNode.OccupyingPiece = null;
				Destroy(piece.gameObject);
				return true;
			}
		);

		ActiveChessboard = null;
		GrimoraRunState.CurrentRun.CurrentChessboard = null;
		GrimoraRunState.CurrentRun.PiecesRemovedFromBoard.Clear();
	}

	private void EnableCandlesIfTheyAreDisabled()
	{
		Transform tableCandles = CryptManager.Instance.gameObject.transform.Find("Furniture/TableCandles");
		for (int i = 0; i < tableCandles.childCount; i++)
		{
			tableCandles.GetChild(i).gameObject.SetActive(true);
		}
	}

	private static void CheckLights()
	{

		if (GameObject.Find("BoardLight") != null)
		{
			if (GrimoraRunState.CurrentRun.regionTier == 1)
			{
				GameObject.Find("BoardLight").GetComponent<Light>().color = new Color(0.5240266f, 0.5660378f, 0.2429691f);
				GameObject.Find("BoardLight_Cards").GetComponent<Light>().color = new Color(0.5240266f, 0.5660378f, 0.2429691f);
			}

			if (GrimoraRunState.CurrentRun.regionTier == 2)
			{
				GameObject.Find("BoardLight").GetComponent<Light>().color = new Color(0.13333f, 0.7451f, 0.8549f);
				GameObject.Find("BoardLight_Cards").GetComponent<Light>().color = new Color(0.13333f, 0.7451f, 0.8549f);
			}

		if (GrimoraRunState.CurrentRun.regionTier == 3)
				{ 
				GameObject.Find("BoardLight").GetComponent<Light>().color = new Color(0.46275f, 0.32549f, 0.65098f);
				GameObject.Find("BoardLight_Cards").GetComponent<Light>().color = new Color(0.46275f, 0.32549f, 0.65098f);
			}
		}

		Debug.Log("Changed board light color");

	}

	public override IEnumerator UnrollingSequence(float unrollSpeed)
	{

		CheckLights();

		Component interact = GameObject.Find("StinkbugInteractable").GetComponent<BoxCollider>();

		Destroy(interact);
		GameObject.Find("StinkbugInteractable").transform.position = new Vector3(4.5487f, GameObject.Find("StinkbugInteractable").transform.position.y , - 1.51f);

		InteractionCursor.Instance.InteractionDisabled = true;

		TableRuleBook.Instance.SetOnBoard(false);

		pieces.ForEach(delegate(ChessboardPiece x) { x.gameObject.SetActive(false); });

		UpdateVisuals();

		// base.mapAnim.speed = 1f;
		mapAnim.Play("enter", 0, 0f);

		dynamicElementsParent.gameObject.SetActive(true);

		// if the boss piece exists in the removed pieces,
		// this means the game didn't complete clearing the board for changing the region
		if (GrimoraRunState.CurrentRun.PiecesRemovedFromBoard.Exists(piece => piece.Contains("BossPiece")))
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

		SaveManager.SaveToFile();
		InteractionCursor.Instance.InteractionDisabled = false;

		CheckLights();
		Log.LogDebug($"Finished unrolling chessboard");
	}


	public GrimoraChessboard GenerateChessboard(int region)
	{
		switch (region)
		{
			case 0: //kaycee
			{
				return KayceeChessboards.GetRandomItem();
			}
			case 1: //sawyer
			{
				return SawyerChessboards.GetRandomItem();
			}
			case 2: //royal
			{
				return RoyalChessboards.GetRandomItem();
			}
			case 3: //grimora
			{
				return GrimoraChessboards.GetRandomItem();
			}
			default:
				//kaycee
				return KayceeChessboards.GetRandomItem();
		}
	}


	private void UpdateActiveChessboard()
	{
		if (ActiveChessboard == null)
		{
			if (GrimoraRunState.CurrentRun.CurrentChessboard == null)
			{
				Log.LogDebug($"[UpdateActiveChessboard] Generating new chessboard");
				GrimoraChessboard generateChessboard = GenerateChessboard(GrimoraRunState.CurrentRun.regionTier);
				ActiveChessboard = generateChessboard;

				GrimoraRunState.CurrentRun.SetCurrentChessboard(generateChessboard);
			}
			else
			{
				Log.LogDebug($"[UpdateActiveChessboard] Loading chessboard from save data");
				List<List<char>> currentRunCurrentChessboard = GrimoraRunState.CurrentRun.CurrentChessboard;
				ActiveChessboard = new GrimoraChessboard(currentRunCurrentChessboard);
			}
		}
		
		Log.LogDebug($"[UpdateActiveChessboard] Chessboard [{ActiveChessboard}]");
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
		var removedList = GrimoraRunState.CurrentRun.PiecesRemovedFromBoard;

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
		CheckLights();
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

		if (ext.SafeIsUnityNull())
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
		Log.LogDebug($"[ChangeStartDeckIfNotAlreadyChanged] Checking if deck needs reset");
		try
		{
			List<CardInfo> grimoraDeck = RunState.Run.playerDeck.Cards;

			int graveDiggerCount = grimoraDeck.Count(info => info.name == "Gravedigger");
			int frankNSteinCount = grimoraDeck.Count(info => info.name == "FrankNStein");
			if (grimoraDeck.Count == 5 && graveDiggerCount == 3 && frankNSteinCount == 2)
			{
				Log.LogWarning($"[ChangeStartDeckIfNotAlreadyChanged] --> Starter deck needs reset");
				GrimoraSaveData.Data.Initialize();
			}
		}
		catch (Exception e)
		{
			Log.LogWarning($"[ChangingDeck] Had trouble retrieving deck list! Resetting deck. Current card Ids: [{RunState.Run.playerDeck.cardIds.Join()}]");
			GrimoraSaveData.Data.Initialize();
		}

		Log.LogDebug($"[ChangeStartDeckIfNotAlreadyChanged] -> Finished");
	}
}
