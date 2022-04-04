using DiskCardGame;
using InscryptionAPI.Card;
using Sirenix.Utilities;
using UnityEngine;
using static GrimoraMod.GrimoraPlugin;

namespace GrimoraMod;

public class DebugHelper : ManagedBehaviour
{
	private static string[] _allGrimoraCardNames;

	private static string[] _allGrimoraCustomCardNames;

	private static readonly Rect RectCardListArea = new(Screen.width - 420, 180, 400, Screen.height - 200);

	private bool _toggleEncounterMenu;

	private bool _toggleSpawnCardInOpponentSlot1;

	private bool _toggleSpawnCardInOpponentSlot2;

	private bool _toggleSpawnCardInOpponentSlot3;

	private bool _toggleSpawnCardInOpponentSlot4;

	private bool _toggleDebugBaseModCardsDeck;

	private bool _toggleDebugBaseModCardsHand;

	private bool _toggleDebugCustomCardsDeck;

	private bool _toggleDebugCustomCardsHand;

	private bool _toggleDebugTools;

	private readonly string[] _btnTools =
	{
		"Win Round", "Lose Round", "Add Energy",
		"Clear Deck", "Add Bones", "Reset Removed Pieces"
	};

	private bool _toggleDebugChests;

	private readonly string[] _btnChests =
	{
		"Card Choice", "Rare Card Choice"
	};

	private bool _toggleDebug;

	private readonly string[] _btnDebug =
	{
		"Win Round", "Lose Round"
	};

	private bool _toggleEncounters;

	private readonly List<EncounterBlueprintData> _encounters = new();

	private string[] _encounterNames;

	private void Start()
	{
		_encounters.AddRange(Resources.LoadAll<EncounterBlueprintData>("Data/EncounterBlueprints/").Where(ebd => ebd.name.StartsWith("Grimora")));
		foreach (var lst in BlueprintUtils.RegionWithBlueprints.Values)
		{
			_encounters.AddRange(lst);
		}
		
		_encounterNames = _encounters.Select(ebd => ebd.name).ToArray();

		_allGrimoraCardNames = AllGrimoraModCards.Select(card => card.name.Replace($"{GUID}_", "")).ToArray();

		_allGrimoraCustomCardNames
			= CardManager.AllCardsCopy
			 .FindAll(
					info => info.name.StartsWith($"{GUID}_")
					     && !AllGrimoraModCards.Exists(modInfo => modInfo.name == info.name)
				)
			 .Select(info => info.name.Replace($"{GUID}_", ""))
			 .ToArray();
	}

	private void OnGUI()
	{
		if (!ConfigHelper.Instance.isDevModeEnabled)
		{
			return;
		}

		_toggleDebugTools = GUI.Toggle(
			new Rect(20, 60, 120, 20),
			_toggleDebugTools,
			"Debug Tools"
		);

		if (GrimoraGameFlowManager.Instance.CurrentGameState != GameState.FirstPerson3D)
		{
			_toggleDebugChests = GUI.Toggle(
				new Rect(20, 180, 120, 20),
				_toggleDebugChests,
				"Debug Chests"
			);

			_toggleEncounters = GUI.Toggle(
				new Rect(20, 300, 120, 20),
				_toggleEncounters,
				"Debug Encounters"
			);

			_toggleDebugBaseModCardsHand = GUI.Toggle(
				new Rect(Screen.width - 200, 20, 200, 20),
				_toggleDebugBaseModCardsHand,
				"Base Mod Cards To Hand"
			);

			_toggleDebugBaseModCardsDeck = GUI.Toggle(
				new Rect(Screen.width - 200, 40, 200, 20),
				_toggleDebugBaseModCardsDeck,
				"Base Mod Cards To Deck"
			);

			_toggleDebugCustomCardsHand = GUI.Toggle(
				new Rect(Screen.width - 200, 60, 200, 20),
				_toggleDebugCustomCardsHand,
				"Custom Mod Cards To Hand"
			);

			_toggleDebugCustomCardsDeck = GUI.Toggle(
				new Rect(Screen.width - 200, 80, 200, 20),
				_toggleDebugCustomCardsDeck,
				"Custom Mod Cards To Deck"
			);

			if (GrimoraGameFlowManager.Instance.CurrentGameState == GameState.CardBattle)
			{
				_toggleSpawnCardInOpponentSlot1 = GUI.Toggle(
					new Rect(Screen.width - 200, 100, 200, 20),
					_toggleSpawnCardInOpponentSlot1,
					"Spawn Opponent Slot 1"
				);

				_toggleSpawnCardInOpponentSlot2 = GUI.Toggle(
					new Rect(Screen.width - 200, 120, 200, 20),
					_toggleSpawnCardInOpponentSlot2,
					"Spawn Opponent Slot 2"
				);

				_toggleSpawnCardInOpponentSlot3 = GUI.Toggle(
					new Rect(Screen.width - 200, 140, 200, 20),
					_toggleSpawnCardInOpponentSlot3,
					"Spawn Opponent Slot 3"
				);

				_toggleSpawnCardInOpponentSlot4 = GUI.Toggle(
					new Rect(Screen.width - 200, 160, 200, 20),
					_toggleSpawnCardInOpponentSlot4,
					"Spawn Opponent Slot 4"
				);
			}
		}

		if (_toggleDebugTools)
		{
			int selectedButton = GUI.SelectionGrid(
				new Rect(25, 80, 300, 90),
				-1,
				_btnTools,
				2
			);

			if (selectedButton >= 0)
			{
				// Log.LogDebug($"[OnGUI] Calling button [{selectedButton}]");
				string selectedBtn = _btnTools[selectedButton];
				switch (selectedBtn)
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
					case "Clear Deck":
						GrimoraSaveUtil.ClearDeck();
						SaveManager.SaveToFile();
						break;
					case "Add Bones":
						ResourcesManager.Instance.StartCoroutine(ResourcesManager.Instance.AddBones(25));
						break;
					case "Add Energy":
						ResourcesManager.Instance.StartCoroutine(ResourcesManager.Instance.AddMaxEnergy(1));
						ResourcesManager.Instance.StartCoroutine(ResourcesManager.Instance.AddEnergy(1));
						break;
					case "Reset Removed Pieces":
						ConfigHelper.Instance.ResetRemovedPieces();
						break;
				}
			}
		}

		if (_toggleDebugChests)
		{
			int selectedButton = GUI.SelectionGrid(
				new Rect(25, 200, 300, 40),
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
							.PlacePiece<ChessboardChestPiece>(i, 0, specialNodeData: specialNode);
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

		if (_toggleEncounters)
		{
			int selectedButton = GUI.SelectionGrid(
				new Rect(25, 320, 300, Screen.height - 320),
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

		if (_toggleDebugBaseModCardsHand
		    && !_toggleDebugCustomCardsHand
		    && !_toggleDebugBaseModCardsDeck
		    && !_toggleDebugCustomCardsDeck
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
		    && !_toggleSpawnCardInOpponentSlot4)
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
						($"{GUID}_" + _allGrimoraCardNames[selectedButton]).GetCardInfo(),
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
		    && !_toggleSpawnCardInOpponentSlot4)
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
						($"{GUID}_" + _allGrimoraCardNames[selectedButton]).GetCardInfo(),
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
		    && !_toggleSpawnCardInOpponentSlot4)
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
						($"{GUID}_" + _allGrimoraCardNames[selectedButton]).GetCardInfo(),
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
		    && !_toggleSpawnCardInOpponentSlot3)
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
						($"{GUID}_" + _allGrimoraCardNames[selectedButton]).GetCardInfo(),
						BoardManager.Instance.OpponentSlotsCopy[3]
					)
				);
			}
		}
	}
}
