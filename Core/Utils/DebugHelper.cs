using DiskCardGame;
using HarmonyLib;
using InscryptionAPI.Card;
using Sirenix.Utilities;
using UnityEngine;
using static GrimoraMod.GrimoraPlugin;

namespace GrimoraMod;

public class DebugHelper : ManagedBehaviour
{
	private bool IsInBattle => GrimoraGameFlowManager.Instance.CurrentGameState == GameState.CardBattle && !TurnManager.Instance.GameEnding;

	private const int DefaultToggleHeight = 20;

	private const int DefaultToggleWidth = 200;

	private const int DefaultGridWidth = 300;

	private readonly int _togglePositionsFromRightSideOfScreen = Screen.width - DefaultToggleWidth;

	private readonly int _togglePositionsTwoFromRightSideOfScreen = Screen.width - 400;

	private const int _togglePositionsFromLeftSideOfScreen = 20;

	private static string[] _allGrimoraCardNames;

	private static string[] _allGrimoraCustomCardNames;

	private static readonly Rect RectCardListArea = new(Screen.width - 420, 140, 400, Screen.height - 150);

	public bool StartAtTwinGiants;

	public bool StartAtBonelord;

	private bool _toggleEncounterMenu;

	private bool _toggleSpawnCardInOpponentSlot1;

	private bool _toggleSpawnCardInOpponentSlot2;

	private bool _toggleSpawnCardInOpponentSlot3;

	private bool _toggleSpawnCardInOpponentSlot4;

	private bool _toggleSpawnCardInAllOpponentSlots;

	private bool _toggleDebugBaseModCardsDeck;

	private bool _toggleDebugBaseModCardsHand;

	private bool _toggleDebugCustomCardsDeck;

	private bool _toggleDebugCustomCardsHand;

	private bool _togglePlayerHandModel;

	private const int DebugToolsHeight = 20;

	private bool _toggleDebugTools;

	private readonly string[] _btnDebugToolsInBattle =
	{
		"Win Round", "Lose Round",
		"Add Energy", "Add Bones",
		"Kill Opponent Cards"
	};

	private readonly string[] _btnDebugToolsOutOfBattle =
	{
		"Clear Deck", "Reset Removed Pieces"
	};

	private const int DebugChestsHeight = 210;

	private bool _toggleDebugChests;

	private readonly string[] _btnChests =
	{
		"Card Choice", "Rare Card Choice"
	};

	private const int DebugEncountersHeight = 260;

	private bool _toggleEncounters;

	private readonly List<EncounterBlueprintData> _encounters = new();

	private string[] _encounterNames;

	private bool _toggleCombatBell;

	private bool _toggleHammer;

	private List<CardInfo> AllGrimoraCustomCards { get; set; } = new();

	public void SetupEncounterData()
	{
		CustomCoroutine.WaitOnConditionThenExecute(
			() => !GameFlowManager.Instance.Transitioning,
			() =>
			{
				Log.LogDebug($"[DebugHelper] Setting up encounter buttons");
				_encounters.AddRange(Resources.LoadAll<EncounterBlueprintData>("Data/EncounterBlueprints/").Where(ebd => ebd.name.StartsWith("Grimora")));
				List<EncounterBlueprintData> largeList = BlueprintUtils.RegionWithBlueprints.Values.SelectMany(ebd => ebd.ToList())
				 .Concat(ChessboardMapExt.Instance.CustomBlueprints.Values)
				 .ToList();
				foreach (var lst in largeList)
				{
					Log.LogDebug($"Adding blueprint [{lst.name}] to _encounters");
					_encounters.Add(lst);
				}

				_encounterNames = _encounters.Select(ebd => ebd.name).ToArray();

				_allGrimoraCardNames = AllGrimoraModCards.Select(card => card.name.Replace($"{GUID}_", "")).ToArray();

				AllGrimoraCustomCards.AddRange(
					CardManager.AllCardsCopy.FindAll(
						info => info.name.StartsWith($"{GUID}_")
						     && !AllGrimoraModCards.Exists(modInfo => modInfo.name == info.name)
					));
				if (_allGrimoraCustomCardNames == null)
				{
					_allGrimoraCustomCardNames = new string[AllGrimoraCustomCards.Count + 1];
				}
				foreach (var customCard in AllGrimoraCustomCards)
				{
					_allGrimoraCustomCardNames.AddToArray(customCard.name.Replace($"{GUID}_", ""));
				}
			});
	}

	private void OnGUI()
	{
		SetupGrimoraFight();

		_toggleDebugTools = GUI.Toggle(
			new Rect(_togglePositionsFromLeftSideOfScreen, DebugToolsHeight, DefaultToggleWidth, DefaultToggleHeight),
			_toggleDebugTools,
			"Debug Tools"
		);

		if (GrimoraGameFlowManager.Instance.CurrentGameState != GameState.FirstPerson3D)
		{
			SetupOnChessboardHelpers();

			if (IsInBattle)
			{
				SetupInBattleHelpers();

				((BoardManager3D)BoardManager.Instance).Bell.gameObject.SetActive(!_toggleCombatBell);

				GrimoraItemsManagerExt.Instance.hammerSlot.gameObject.SetActive(!_toggleHammer);

				((PlayerHand3D)PlayerHand.Instance).anim.transform.Find("HandModel_Male").gameObject.SetActive(!_togglePlayerHandModel);
			}
		}

		HandleDebugTools();

		HandleChestPieces();

		HandleEncounters();

		HandleSpawningAndAddingCards();
	}

	private void HandleDebugTools()
	{
		if (_toggleDebugTools)
		{
			string[] toolNames = IsInBattle ? _btnDebugToolsInBattle : _btnDebugToolsOutOfBattle;
			int selectedButton = GUI.SelectionGrid(
				new Rect(_togglePositionsFromLeftSideOfScreen, DebugToolsHeight + 20, DefaultGridWidth, 90),
				-1,
				toolNames,
				2
			);

			if (selectedButton >= 0)
			{
				// Log.LogDebug($"[OnGUI] Calling button [{selectedButton}]");
				string selectedBtn = toolNames[selectedButton];
				switch (selectedBtn)
				{
					case "Win Round":
						StartCoroutine(
							LifeManager.Instance.ShowDamageSequence(10, 1, false)
						);
						break;
					case "Lose Round":
						StartCoroutine(
							LifeManager.Instance.ShowDamageSequence(10, 1, true)
						);
						break;
					case "Add Bones":
						StartCoroutine(ResourcesManager.Instance.AddBones(25));
						break;
					case "Add Energy":
						StartCoroutine(ResourcesManager.Instance.AddMaxEnergy(1));
						StartCoroutine(ResourcesManager.Instance.AddEnergy(1));
						break;
					case "Clear Deck":
						GrimoraSaveUtil.ClearDeck();
						SaveManager.SaveToFile();
						break;
					case "Reset Removed Pieces":
						ConfigHelper.Instance.ResetRemovedPieces();
						break;
					case "Kill Opponent Cards":
						foreach (var opponentCard in BoardManager.Instance.GetOpponentCards())
						{
							StartCoroutine(opponentCard.Die(false));
						}
						break;
				}
			}
		}
	}

	private void SetupOnChessboardHelpers()
	{
		_toggleDebugChests = GUI.Toggle(
			new Rect(_togglePositionsFromLeftSideOfScreen, DebugChestsHeight, DefaultToggleWidth, DefaultToggleHeight),
			_toggleDebugChests,
			"Debug Chests"
		);

		_toggleEncounters = GUI.Toggle(
			new Rect(_togglePositionsFromLeftSideOfScreen, DebugEncountersHeight, DefaultToggleWidth, DefaultToggleHeight),
			_toggleEncounters,
			"Debug Encounters"
		);

		_toggleDebugBaseModCardsDeck = GUI.Toggle(
			new Rect(_togglePositionsFromRightSideOfScreen, 40, DefaultToggleWidth, DefaultToggleHeight),
			_toggleDebugBaseModCardsDeck,
			"Base Mod Cards To Deck"
		);

		_toggleDebugCustomCardsDeck = GUI.Toggle(
			new Rect(_togglePositionsFromRightSideOfScreen, 80, DefaultToggleWidth, DefaultToggleHeight),
			_toggleDebugCustomCardsDeck,
			"Custom Mod Cards To Deck"
		);
	}

	private void SetupInBattleHelpers()
	{
		_toggleCombatBell = GUI.Toggle(
			new Rect(Screen.width - 600, 30, DefaultToggleWidth, DefaultToggleHeight),
			_toggleCombatBell,
			"Disable Combat Bell"
		);

		_toggleHammer = GUI.Toggle(
			new Rect(Screen.width - 600, 50, DefaultToggleWidth, DefaultToggleHeight),
			_toggleHammer,
			"Disable Hammer"
		);
		
		_togglePlayerHandModel = GUI.Toggle(
			new Rect(Screen.width - 600, 70, DefaultToggleWidth, DefaultToggleHeight),
			_togglePlayerHandModel,
			"Disable Hand Model"
		);

		_toggleDebugBaseModCardsHand = GUI.Toggle(
			new Rect(_togglePositionsFromRightSideOfScreen, 20, DefaultToggleWidth, DefaultToggleHeight),
			_toggleDebugBaseModCardsHand,
			"Base Mod Cards To Hand"
		);

		_toggleDebugCustomCardsHand = GUI.Toggle(
			new Rect(_togglePositionsFromRightSideOfScreen, 60, DefaultToggleWidth, DefaultToggleHeight),
			_toggleDebugCustomCardsHand,
			"Custom Mod Cards To Hand"
		);

		_toggleSpawnCardInOpponentSlot1 = GUI.Toggle(
			new Rect(_togglePositionsTwoFromRightSideOfScreen, 20, DefaultToggleWidth, DefaultToggleHeight),
			_toggleSpawnCardInOpponentSlot1,
			"Spawn Opponent Slot 1"
		);

		_toggleSpawnCardInOpponentSlot2 = GUI.Toggle(
			new Rect(_togglePositionsTwoFromRightSideOfScreen, 40, DefaultToggleWidth, DefaultToggleHeight),
			_toggleSpawnCardInOpponentSlot2,
			"Spawn Opponent Slot 2"
		);

		_toggleSpawnCardInOpponentSlot3 = GUI.Toggle(
			new Rect(_togglePositionsTwoFromRightSideOfScreen, 60, DefaultToggleWidth, DefaultToggleHeight),
			_toggleSpawnCardInOpponentSlot3,
			"Spawn Opponent Slot 3"
		);

		_toggleSpawnCardInOpponentSlot4 = GUI.Toggle(
			new Rect(_togglePositionsTwoFromRightSideOfScreen, 80, DefaultToggleWidth, DefaultToggleHeight),
			_toggleSpawnCardInOpponentSlot4,
			"Spawn Opponent Slot 4"
		);

		_toggleSpawnCardInAllOpponentSlots = GUI.Toggle(
			new Rect(_togglePositionsTwoFromRightSideOfScreen, 100, DefaultToggleWidth, DefaultToggleHeight),
			_toggleSpawnCardInAllOpponentSlots,
			"Spawn All Opponent Slots"
		);
	}

	private void SetupGrimoraFight()
	{
		if (ConfigHelper.Instance.BossesDefeated == 3)
		{
			StartAtTwinGiants = GUI.Toggle(
				new Rect(Screen.width / 3f, 40, DefaultToggleWidth, DefaultToggleHeight),
				StartAtTwinGiants,
				"Start at Twin Giants"
			);

			StartAtBonelord = GUI.Toggle(
				new Rect(Screen.width / 3f + 200, 40, DefaultToggleWidth, DefaultToggleHeight),
				StartAtBonelord,
				"Start at Bonelord"
			);
		}
	}

	private void HandleSpawningAndAddingCards()
	{
		if (_toggleDebugBaseModCardsHand
		 && !_toggleDebugCustomCardsHand
		 && !_toggleDebugBaseModCardsDeck
		 && !_toggleDebugCustomCardsDeck
		 && !_toggleSpawnCardInOpponentSlot1
		 && !_toggleSpawnCardInOpponentSlot2
		 && !_toggleSpawnCardInOpponentSlot3
		 && !_toggleSpawnCardInOpponentSlot4
		 && !_toggleSpawnCardInAllOpponentSlots
		)
		{
			int selectedButton = GUI.SelectionGrid(
				RectCardListArea,
				-1,
				_allGrimoraCardNames,
				3
			);

			if (selectedButton >= 0)
			{
				StartCoroutine(
					CardSpawner.Instance.SpawnCardToHand(($"{GUID}_" + _allGrimoraCardNames[selectedButton]).GetCardInfo())
				);
			}
		}

		if (_toggleDebugCustomCardsHand
		 && !_toggleDebugBaseModCardsHand
		 && !_toggleDebugBaseModCardsDeck
		 && !_toggleDebugCustomCardsDeck
		 && !_toggleSpawnCardInOpponentSlot1
		 && !_toggleSpawnCardInOpponentSlot2
		 && !_toggleSpawnCardInOpponentSlot3
		 && !_toggleSpawnCardInOpponentSlot4
		 && !_toggleSpawnCardInAllOpponentSlots
		)
		{
			int selectedButton = GUI.SelectionGrid(
				RectCardListArea,
				-1,
				_allGrimoraCustomCardNames,
				3
			);

			if (selectedButton >= 0)
			{
				StartCoroutine(
					CardSpawner.Instance.SpawnCardToHand(
						($"{GUID}_" + _allGrimoraCustomCardNames[selectedButton]).GetCardInfo()
					)
				);
			}
		}

		if (_toggleDebugBaseModCardsDeck
		 && !_toggleDebugCustomCardsDeck
		 && !_toggleDebugCustomCardsHand
		 && !_toggleDebugBaseModCardsHand
		 && !_toggleSpawnCardInOpponentSlot1
		 && !_toggleSpawnCardInOpponentSlot2
		 && !_toggleSpawnCardInOpponentSlot3
		 && !_toggleSpawnCardInOpponentSlot4
		 && !_toggleSpawnCardInAllOpponentSlots
		)
		{
			int selectedButton = GUI.SelectionGrid(
				RectCardListArea,
				-1,
				_allGrimoraCardNames,
				3
			);

			if (selectedButton >= 0)
			{
				GrimoraSaveUtil.AddCard(($"{GUID}_" + _allGrimoraCardNames[selectedButton]).GetCardInfo());
			}
		}

		if (_toggleDebugCustomCardsDeck
		 && !_toggleDebugBaseModCardsDeck
		 && !_toggleDebugCustomCardsHand
		 && !_toggleDebugBaseModCardsHand
		 && !_toggleSpawnCardInOpponentSlot1
		 && !_toggleSpawnCardInOpponentSlot2
		 && !_toggleSpawnCardInOpponentSlot3
		 && !_toggleSpawnCardInOpponentSlot4
		 && !_toggleSpawnCardInAllOpponentSlots
		)
		{
			int selectedButton = GUI.SelectionGrid(
				RectCardListArea,
				-1,
				_allGrimoraCustomCardNames,
				3
			);

			if (selectedButton >= 0)
			{
				GrimoraSaveUtil.AddCard(($"{GUID}_" + _allGrimoraCustomCardNames[selectedButton]).GetCardInfo());
			}
		}

		if (_toggleSpawnCardInOpponentSlot1
		 && !_toggleDebugCustomCardsDeck
		 && !_toggleDebugBaseModCardsDeck
		 && !_toggleDebugCustomCardsHand
		 && !_toggleDebugBaseModCardsHand
		 && !_toggleSpawnCardInOpponentSlot2
		 && !_toggleSpawnCardInOpponentSlot3
		 && !_toggleSpawnCardInOpponentSlot4
		 && !_toggleSpawnCardInAllOpponentSlots
		)
		{
			int selectedButton = GUI.SelectionGrid(
				RectCardListArea,
				-1,
				_allGrimoraCardNames,
				3
			);

			if (selectedButton >= 0)
			{
				StartCoroutine(
					BoardManager.Instance.CreateCardInSlot(
						AllGrimoraModCards[selectedButton],
						BoardManager.Instance.OpponentSlotsCopy[0]
					)
				);
			}
		}

		if (_toggleSpawnCardInOpponentSlot2
		 && !_toggleDebugCustomCardsDeck
		 && !_toggleDebugBaseModCardsDeck
		 && !_toggleDebugCustomCardsHand
		 && !_toggleDebugBaseModCardsHand
		 && !_toggleSpawnCardInOpponentSlot1
		 && !_toggleSpawnCardInOpponentSlot3
		 && !_toggleSpawnCardInOpponentSlot4
		 && !_toggleSpawnCardInAllOpponentSlots
		)
		{
			int selectedButton = GUI.SelectionGrid(
				RectCardListArea,
				-1,
				_allGrimoraCardNames,
				3
			);

			if (selectedButton >= 0)
			{
				StartCoroutine(
					BoardManager.Instance.CreateCardInSlot(
						AllGrimoraModCards[selectedButton],
						BoardManager.Instance.OpponentSlotsCopy[1]
					)
				);
			}
		}

		if (_toggleSpawnCardInOpponentSlot3
		 && !_toggleDebugCustomCardsDeck
		 && !_toggleDebugBaseModCardsDeck
		 && !_toggleDebugCustomCardsHand
		 && !_toggleDebugBaseModCardsHand
		 && !_toggleSpawnCardInOpponentSlot1
		 && !_toggleSpawnCardInOpponentSlot2
		 && !_toggleSpawnCardInOpponentSlot4
		 && !_toggleSpawnCardInAllOpponentSlots
		)
		{
			int selectedButton = GUI.SelectionGrid(
				RectCardListArea,
				-1,
				_allGrimoraCardNames,
				3
			);

			if (selectedButton >= 0)
			{
				StartCoroutine(
					BoardManager.Instance.CreateCardInSlot(
						AllGrimoraModCards[selectedButton],
						BoardManager.Instance.OpponentSlotsCopy[2]
					)
				);
			}
		}

		if (_toggleSpawnCardInOpponentSlot4
		 && !_toggleDebugCustomCardsDeck
		 && !_toggleDebugBaseModCardsDeck
		 && !_toggleDebugCustomCardsHand
		 && !_toggleDebugBaseModCardsHand
		 && !_toggleSpawnCardInOpponentSlot1
		 && !_toggleSpawnCardInOpponentSlot2
		 && !_toggleSpawnCardInOpponentSlot3
		 && !_toggleSpawnCardInAllOpponentSlots
		)
		{
			int selectedButton = GUI.SelectionGrid(
				RectCardListArea,
				-1,
				_allGrimoraCardNames,
				3
			);

			if (selectedButton >= 0)
			{
				StartCoroutine(
					BoardManager.Instance.CreateCardInSlot(
						AllGrimoraModCards[selectedButton],
						BoardManager.Instance.OpponentSlotsCopy[3]
					)
				);
			}
		}

		if (_toggleSpawnCardInAllOpponentSlots
		 && !_toggleDebugCustomCardsDeck
		 && !_toggleDebugBaseModCardsDeck
		 && !_toggleDebugCustomCardsHand
		 && !_toggleDebugBaseModCardsHand
		 && !_toggleSpawnCardInOpponentSlot1
		 && !_toggleSpawnCardInOpponentSlot2
		 && !_toggleSpawnCardInOpponentSlot3
		 && !_toggleSpawnCardInOpponentSlot4
		)
		{
			int selectedButton = GUI.SelectionGrid(
				RectCardListArea,
				-1,
				_allGrimoraCardNames,
				3
			);

			if (selectedButton >= 0)
			{
				foreach (var cardSlot in BoardManager.Instance.OpponentSlotsCopy)
				{
					StartCoroutine(
						BoardManager.Instance.CreateCardInSlot(
							AllGrimoraModCards[selectedButton],
							cardSlot
						)
					);
				}
			}
		}
	}

	private void HandleChestPieces()
	{
		if (_toggleDebugChests)
		{
			int selectedButton = GUI.SelectionGrid(
				new Rect(_togglePositionsFromLeftSideOfScreen, DebugChestsHeight + 20, DefaultGridWidth, 40),
				-1,
				_btnChests,
				2
			);

			if (selectedButton >= 0)
			{
				SpecialNodeData specialNode = new CardChoicesNodeData();
				switch (_btnChests[selectedButton])
				{
					case "Card Remove":
						specialNode = new CardRemoveNodeData();
						break;
					case "Rare Card Choice":
						specialNode = new ChooseRareCardNodeData();
						break;
				}

				ChessboardChestPiece[] chests = FindObjectsOfType<ChessboardChestPiece>();

				if (chests.IsNullOrEmpty())
				{
					for (int i = 0; i < 8; i++)
					{
						var copy = ConfigHelper.Instance.RemovedPieces;
						copy.RemoveAll(piece => piece.Contains("Chest"));
						ConfigHelper.Instance.RemovedPieces = copy;
						ChessboardMapExt.Instance
						 .ActiveChessboard
						 .PlacePiece<ChessboardChestPiece>(i, 0, specialNodeData:specialNode);
					}
				}
				else
				{
					foreach (var chest in chests)
					{
						chest.NodeData = specialNode;
					}
				}
			}
		}
	}

	private void HandleEncounters()
	{
		if (_toggleEncounters)
		{
			int selectedButton = GUI.SelectionGrid(
				new Rect(_togglePositionsFromLeftSideOfScreen, DebugEncountersHeight + 20, DefaultGridWidth, Screen.height - 320),
				-1,
				_encounterNames,
				2
			);

			if (selectedButton >= 0)
			{
				EncounterBlueprintData encounter = _encounters[selectedButton];
				// the asset names have P1 or P2 at the end,
				//	so we'll remove it so that we can correctly get a boss if a boss was selected
				string scrubbedName = encounter.name.Replace("P1", "").Replace("P2", "");

				CardBattleNodeData node = new CardBattleNodeData
				{
					blueprint = encounter
				};

				if (Enum.TryParse(scrubbedName, true, out Opponent.Type bossType))
				{
					node = new BossBattleNodeData();
					((BossBattleNodeData)node).specialBattleId = BossBattleSequencer.GetSequencerIdForBoss(bossType);
					((BossBattleNodeData)node).bossType = bossType;
				}

				Opponent opponent = TurnManager.Instance.Opponent;
				if (opponent && !TurnManager.Instance.GameIsOver())
				{
					Log.LogDebug($"Setting NumLives to 1");
					opponent.NumLives = 1;
					Log.LogDebug($"Game is not over, making opponent surrender");
					opponent.SurrenderImmediate();
					Log.LogDebug($"Playing LifeLostSequence for [{opponent.GetType()}]");
					StartCoroutine(opponent.LifeLostSequence());
				}

				Log.LogDebug($"Transitioning to encounter [{encounter.name}]");
				bool canTransitionToFight = GrimoraGameFlowManager.Instance.CurrentGameState == GameState.Map
				                         && !GrimoraGameFlowManager.Instance.Transitioning
				                         && GameMap.Instance;
				CustomCoroutine.WaitOnConditionThenExecute(
					() => canTransitionToFight,
					delegate
					{
						Log.LogDebug($"-> No longer in transitioning state, transitioning to new encounter");
						GameFlowManager.Instance.TransitionToGameState(GameState.CardBattle, node);
					});
			}
		}
	}
}
