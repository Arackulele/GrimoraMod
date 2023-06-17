using System.Collections;
using DiskCardGame;
using GrimoraMod.Saving;
using HarmonyLib;
using InscryptionAPI.Card;
using InscryptionAPI.Helpers.Extensions;
using Sirenix.Utilities;
using UnityEngine;
using UnityEngine.UI;
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

	private Toggle _toggleSpawnCardInOpponentSlot1;

	private Toggle _toggleSpawnCardInOpponentSlot2;

	private Toggle _toggleSpawnCardInOpponentSlot3;

	private Toggle _toggleSpawnCardInOpponentSlot4;

	private Toggle _toggleSpawnCardInAllOpponentSlots;

	private Toggle _toggleDebugBaseModCardsDeck;

	private Toggle _toggleDebugBaseModCardsHand;

	private Toggle _toggleDebugCustomCardsDeck;

	private Toggle _toggleDebugCustomCardsHand;

	private bool _togglePlayerHandModel;

	private const int DebugToolsHeight = 20;

	private bool _toggleDebugTools;

	private readonly string[] _btnDebugToolsInBattle =
	{
		"Win Round", "Lose Round",
		"Add Energy", "Add Bones",
		"Kill Opponent Cards",
		"Draw Main Deck", "Draw Side Deck"
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

	private ToggleGroup _toggleGroupsParent;

	private ToggleGroup _toggleGroupsAddingCards;

	private ToggleGroup _toggleGroupsSpawningCards;

	private GameObject _handModel;

	private GameObject _combatBell;

	private GameObject _hammer;

	private Toggle CreateOpponentSlotToggle(string toggleName)
	{
		Toggle toggle = CreateToggle(toggleName, _toggleGroupsSpawningCards);
		toggle.onValueChanged.AddListener(delegate(bool isOn)
		{
			_toggleGroupsSpawningCards.NotifyToggleOn(toggle);
			_toggleGroupsAddingCards.SetAllTogglesOff();
			toggle.isOn = isOn;
		});
		return toggle;
	}

	private Toggle CreateAddingCardsToggle(string toggleName)
	{
		Toggle toggle = CreateToggle(toggleName, _toggleGroupsAddingCards);
		toggle.onValueChanged.AddListener(delegate(bool isOn)
		{
			_toggleGroupsAddingCards.NotifyToggleOn(toggle);
			_toggleGroupsSpawningCards.SetAllTogglesOff();
			toggle.isOn = isOn;
		});
		return toggle;
	}

	private Toggle CreateToggle(string toggleName, ToggleGroup parent)
	{
		Toggle toggle = new GameObject(toggleName).AddComponent<Toggle>();
		toggle.transform.SetParent(parent.transform);
		parent.RegisterToggle(toggle);
		return toggle;
	}

	private void Start()
	{
		_toggleGroupsParent = new GameObject("ToggleGroups").AddComponent<ToggleGroup>();
		_toggleGroupsParent.transform.SetParent(UIManager.Instance.gameObject.transform);

		_toggleGroupsAddingCards = new GameObject("AddingCards").AddComponent<ToggleGroup>();
		_toggleGroupsAddingCards.transform.SetParent(_toggleGroupsParent.transform);
		_toggleGroupsAddingCards.allowSwitchOff = true;

		_toggleGroupsSpawningCards = new GameObject("Spawning Cards").AddComponent<ToggleGroup>();
		_toggleGroupsSpawningCards.transform.SetParent(_toggleGroupsParent.transform);
		_toggleGroupsSpawningCards.allowSwitchOff = true;

		_toggleSpawnCardInOpponentSlot1 = CreateOpponentSlotToggle("_toggleSpawnCardInOpponentSlot0");
		_toggleSpawnCardInOpponentSlot2 = CreateOpponentSlotToggle("_toggleSpawnCardInOpponentSlot1");
		_toggleSpawnCardInOpponentSlot3 = CreateOpponentSlotToggle("_toggleSpawnCardInOpponentSlot2");
		_toggleSpawnCardInOpponentSlot4 = CreateOpponentSlotToggle("_toggleSpawnCardInOpponentSlot3");
		_toggleSpawnCardInAllOpponentSlots = CreateOpponentSlotToggle("_toggleSpawnCardInAllOpponentSlots");

		_toggleDebugBaseModCardsDeck = CreateAddingCardsToggle("_toggleDebugBaseModCardsDeck");
		_toggleDebugBaseModCardsHand = CreateAddingCardsToggle("_toggleDebugBaseModCardsHand");
		_toggleDebugCustomCardsDeck = CreateAddingCardsToggle("_toggleDebugCustomCardsDeck");
		_toggleDebugCustomCardsHand = CreateAddingCardsToggle("_toggleDebugCustomCardsHand");
	}

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

				_allGrimoraCardNames = AllGrimoraModCards.Select(info => info.DisplayedNameEnglish).ToArray();

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
					_allGrimoraCustomCardNames.AddToArray(customCard.name.Replace($"{GUID}_", string.Empty));
				}
			});
	}

	private void OnGUI()
	{
		SetupGrimoraFight();

		if (GrimoraGameFlowManager.Instance.CurrentGameState == GameState.FirstPerson3D)
		{
			return;
		}

		SetupGlobalHelpers();

		if (IsInBattle)
		{
			_combatBell ??= ((BoardManager3D)BoardManager.Instance).Bell.gameObject;
			_handModel ??= ((PlayerHand3D)PlayerHand.Instance).anim.transform.Find("HandModel_Male").gameObject;
			_hammer ??= GrimoraItemsManagerExt.Instance.HammerSlot.gameObject;

			SetupInBattleHelpers();
		}
		else
		{
			_toggleDebugChests = GUI.Toggle(
				new Rect(_togglePositionsFromLeftSideOfScreen, DebugChestsHeight, DefaultToggleWidth, DefaultToggleHeight),
				_toggleDebugChests,
				"Debug Chests"
			);

			_toggleDebugBaseModCardsDeck.isOn = GUI.Toggle(
				new Rect(_togglePositionsFromRightSideOfScreen, 40, DefaultToggleWidth, DefaultToggleHeight),
				_toggleDebugBaseModCardsDeck.isOn,
				"Base Mod Cards To Deck"
			);

			_toggleDebugCustomCardsDeck.isOn = GUI.Toggle(
				new Rect(_togglePositionsFromRightSideOfScreen, 80, DefaultToggleWidth, DefaultToggleHeight),
				_toggleDebugCustomCardsDeck.isOn,
				"Custom Mod Cards To Deck"
			);

			if (_toggleDebugBaseModCardsDeck.isOn || _toggleDebugCustomCardsDeck.isOn)
			{
				int selectedButton = GUI.SelectionGrid(
					RectCardListArea,
					-1,
					_toggleDebugBaseModCardsDeck.isOn ? _allGrimoraCardNames : _allGrimoraCustomCardNames,
					3
				);

				if (selectedButton >= 0)
				{
					RunState.Run.playerDeck.AddCard((_toggleDebugBaseModCardsDeck.isOn ? AllGrimoraModCards : AllGrimoraCustomCards)[selectedButton]);
				}
			}

			HandleChestPieces();
		}

		HandleDebugTools();

		HandleEncounters();

		HandleSpawningCards();
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
					{
						StartCoroutine(LifeManager.Instance.ShowDamageSequence(10, 1, false));
						break;
					}
					case "Lose Round":
					{
						StartCoroutine(LifeManager.Instance.ShowDamageSequence(10, 1, true));
						break;
					}
					case "Add Bones":
					{
						StartCoroutine(ResourcesManager.Instance.AddBones(25));
						break;
					}
					case "Add Energy":
					{
						StartCoroutine(ResourcesManager.Instance.AddMaxEnergy(1));
						StartCoroutine(ResourcesManager.Instance.AddEnergy(1));
						break;
					}
					case "Clear Deck":
					{
						RunState.Run.playerDeck.Cards.Clear();
						RunState.Run.playerDeck.cardIds.Clear();
						SaveManager.SaveToFile();
						break;
					}
					case "Reset Removed Pieces":
					{
						GrimoraRunState.CurrentRun.PiecesRemovedFromBoard.Clear();
						ChessboardMapExt.Instance.ActiveChessboard.SetupBoard(true);
						break;
					}
					case "Kill Opponent Cards":
					{
						foreach (var opponentCard in BoardManager.Instance.GetOpponentCards())
						{
							StartCoroutine(opponentCard.Die(false));
						}

						break;
					}
					case "Draw Main Deck":
					{
						CardDrawPiles3D.Instance.Pile.Draw();
						StartCoroutine(CardDrawPiles3D.Instance.DrawCardFromDeck());
						break;
					}
					case "Draw Side Deck":
					{
						CardDrawPiles3D.Instance.SidePile.Draw();
						StartCoroutine(CardDrawPiles3D.Instance.DrawFromSidePile());
						break;
					}
				}
			}
		}
	}

	private void SetupGlobalHelpers()
	{
		_toggleDebugTools = GUI.Toggle(
			new Rect(_togglePositionsFromLeftSideOfScreen, DebugToolsHeight, DefaultToggleWidth, DefaultToggleHeight),
			_toggleDebugTools,
			"Debug Tools"
		);

		_toggleEncounters = GUI.Toggle(
			new Rect(_togglePositionsFromLeftSideOfScreen, DebugEncountersHeight, DefaultToggleWidth, DefaultToggleHeight),
			_toggleEncounters,
			"Debug Encounters"
		);
	}

	private void SetupInBattleHelpers()
	{
		_combatBell.SetActive(!_toggleCombatBell);

		_hammer.SetActive(!_toggleHammer);

		_handModel.SetActive(!_togglePlayerHandModel);

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

		_toggleDebugBaseModCardsHand.isOn = GUI.Toggle(
			new Rect(_togglePositionsFromRightSideOfScreen, 20, DefaultToggleWidth, DefaultToggleHeight),
			_toggleDebugBaseModCardsHand.isOn,
			"Base Mod Cards To Hand"
		);

		_toggleDebugCustomCardsHand.isOn = GUI.Toggle(
			new Rect(_togglePositionsFromRightSideOfScreen, 60, DefaultToggleWidth, DefaultToggleHeight),
			_toggleDebugCustomCardsHand.isOn,
			"Custom Mod Cards To Hand"
		);

		_toggleSpawnCardInOpponentSlot1.isOn = GUI.Toggle(
			new Rect(_togglePositionsTwoFromRightSideOfScreen, 20, DefaultToggleWidth, DefaultToggleHeight),
			_toggleSpawnCardInOpponentSlot1.isOn,
			"Spawn Opponent Slot 1"
		);

		_toggleSpawnCardInOpponentSlot2.isOn = GUI.Toggle(
			new Rect(_togglePositionsTwoFromRightSideOfScreen, 40, DefaultToggleWidth, DefaultToggleHeight),
			_toggleSpawnCardInOpponentSlot2.isOn,
			"Spawn Opponent Slot 2"
		);

		_toggleSpawnCardInOpponentSlot3.isOn = GUI.Toggle(
			new Rect(_togglePositionsTwoFromRightSideOfScreen, 60, DefaultToggleWidth, DefaultToggleHeight),
			_toggleSpawnCardInOpponentSlot3.isOn,
			"Spawn Opponent Slot 3"
		);

		_toggleSpawnCardInOpponentSlot4.isOn = GUI.Toggle(
			new Rect(_togglePositionsTwoFromRightSideOfScreen, 80, DefaultToggleWidth, DefaultToggleHeight),
			_toggleSpawnCardInOpponentSlot4.isOn,
			"Spawn Opponent Slot 4"
		);

		_toggleSpawnCardInAllOpponentSlots.isOn = GUI.Toggle(
			new Rect(_togglePositionsTwoFromRightSideOfScreen, 100, DefaultToggleWidth, DefaultToggleHeight),
			_toggleSpawnCardInAllOpponentSlots.isOn,
			"Spawn All Opponent Slots"
		);

		if (_toggleDebugBaseModCardsHand.isOn || _toggleDebugCustomCardsHand.isOn)
		{
			int selectedButton = GUI.SelectionGrid(
				RectCardListArea,
				-1,
				_toggleDebugBaseModCardsHand.isOn ? _allGrimoraCardNames : _allGrimoraCustomCardNames,
				3
			);

			if (selectedButton >= 0)
			{
				StartCoroutine((_toggleDebugBaseModCardsHand.isOn ? AllGrimoraModCards : AllGrimoraCustomCards)[selectedButton].SpawnInHand());
			}
		}
	}

	private void SetupGrimoraFight()
	{
		if (GrimoraRunState.CurrentRun.regionTier == 3)
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

	private void HandleSpawningCards()
	{
		if (_toggleGroupsSpawningCards.AnyTogglesOn())
		{
			int selectedButton = GUI.SelectionGrid(
				RectCardListArea,
				-1,
				_allGrimoraCardNames,
				3
			);

			if (selectedButton >= 0)
			{
				CardInfo cardToSpawn = AllGrimoraModCards[selectedButton];
				var activeToggle = _toggleGroupsSpawningCards.ActiveToggles().First();
				StartCoroutine(int.TryParse(activeToggle.name.Substring(activeToggle.name.Length - 1), out int num)
					               ? BoardManager.Instance.OpponentSlotsCopy[num].CreateCardInSlot(cardToSpawn)
					               : SpawnCardAtSlots(BoardManager.Instance.OpponentSlotsCopy, cardToSpawn)
				);
			}
		}
	}

	private IEnumerator SpawnCardAtSlots(List<CardSlot> slots, CardInfo cardToSpawn)
	{
		foreach (var slot in slots)
		{
			yield return slot.CreateCardInSlot(cardToSpawn);
			yield return new WaitForSeconds(0.1f);
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
				SpecialNodeData specialNode = selectedButton == 0 ? new CardChoicesNodeData() : new ChooseRareCardNodeData();

				ChessboardChestPiece[] chests = FindObjectsOfType<ChessboardChestPiece>();

				if (chests.IsNullOrEmpty())
				{
					// TODO:
					/*var copy = ConfigHelper.Instance.RemovedPieces;
					copy.RemoveAll(piece => piece.Contains("Chest"));
					ConfigHelper.Instance.RemovedPieces = copy;
					ChessboardMapExt.Instance.ActiveChessboard.PlacePieces<ChessboardChestPiece>(specialNodeData: specialNode);*/
				}
				else
				{
					foreach (var chest in chests)
					{
						chest.NodeData = specialNode;
					}

					Log.LogDebug($"[DebugHelper] Set [{chests.Length}] to rare chests.");
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
				string scrubbedName = encounter.name.Replace("P1", string.Empty).Replace("P2", string.Empty);

				CardBattleNodeData node = new CardBattleNodeData
				{
					blueprint = encounter,
					specialBattleId = GrimoraModBattleSequencer.FullSequencer.Id
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
					// Log.LogDebug($"Setting NumLives to 1");
					opponent.NumLives = 1;
					// Log.LogDebug($"Game is not over, making opponent surrender");
					// opponent.SurrenderImmediate();
					Log.LogDebug($"Playing LifeLostSequence for [{opponent.GetType()}]");
					StartCoroutine(LifeManager.Instance.ShowDamageSequence(50, 1, false));
				}

				Log.LogDebug($"Transitioning to encounter [{encounter.name}]");
				CustomCoroutine.WaitOnConditionThenExecute(
					() => GameMap.Instance && GrimoraGameFlowManager.Instance.CurrentGameState == GameState.Map && !GrimoraGameFlowManager.Instance.Transitioning,
					delegate
					{
						Log.LogDebug($"-> No longer in transitioning state, transitioning to new encounter");
						GameFlowManager.Instance.TransitionToGameState(GameState.CardBattle, node);
					});
			}
		}
	}
}
