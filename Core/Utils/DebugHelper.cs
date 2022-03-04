using DiskCardGame;
using Sirenix.Utilities;
using UnityEngine;
using static GrimoraMod.GrimoraPlugin;

namespace GrimoraMod;

public class DebugHelper : ManagedBehaviour
{
	private static readonly string[] AllGrimoraCardNames
		= AllGrimoraModCards.Select(card => card.name.Replace("GrimoraMod_", "")).ToArray();

	private static readonly string[] AllGrimoraCustomCardNames
		= CardLoader.allData
			.FindAll(
				info => info.name.StartsWith("GrimoraMod_") && !AllGrimoraModCards.Exists(modInfo => modInfo.name == info.name)
			)
			.Select(info => info.name.Replace("GrimoraMod_", ""))
			.ToArray();

	private bool _toggleDebugBaseModCardsDeck;

	private bool _toggleDebugBaseModCardsHand;

	private bool _toggleDebugCustomCardsDeck;

	private bool _toggleDebugCustomCardsHand;

	private bool _toggleDebugTools;

	private readonly string[] _btnTools =
	{
		"Win Round", "Lose Round",
		"Clear Deck", "Add Bones", "Reset Removed Pieces"
	};

	private bool _toggleDebugChests;

	private readonly string[] _btnChests =
	{
		"Card Remove", "Card Choice", "Rare Card Choice"
	};

	private bool _toggleDebug;

	private readonly string[] _btnDebug =
	{
		"Win Round", "Lose Round"
	};

	private bool _toggleEnemies;

	private readonly string[] _btnEnemies =
	{
		"Place Enemies"
	};

	private void AddCardsToDeck(int section)
	{
		int startingIndex = section * 15;
		// 3 * 15 == 45
		// total count 59
		// 59 - 45 == 14
		GrimoraSaveUtil.DeckList.AddRange(
			AllGrimoraModCards.GetRange(
				startingIndex,
				Math.Min(15, AllGrimoraModCards.Count - startingIndex)
			)
		);
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

			_toggleEnemies = GUI.Toggle(
				new Rect(20, 300, 120, 20),
				_toggleEnemies,
				"Debug Enemies"
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

		if (_toggleEnemies)
		{
			int selectedButton = GUI.SelectionGrid(
				new Rect(25, 320, 300, 40),
				-1,
				_btnEnemies,
				2
			);

			if (selectedButton >= 0)
			{
				var copy = ConfigHelper.Instance.RemovedPieces;
				copy.RemoveAll(piece => piece.Contains("Enemy"));
				ConfigHelper.Instance.RemovedPieces = copy;
				for (int i = 0; i < 8; i++)
				{
					ChessboardMapExt.Instance.ActiveChessboard.PlacePiece<ChessboardEnemyPiece>(0, i);
				}
			}
		}

		if (_toggleDebugBaseModCardsHand
		    && !_toggleDebugCustomCardsHand
		    && !_toggleDebugBaseModCardsDeck
		    && !_toggleDebugCustomCardsDeck)
		{
			int selectedButton = GUI.SelectionGrid(
				new Rect(Screen.width - 420, 100, 400, Screen.height - 300),
				-1,
				AllGrimoraCardNames,
				3
			);

			if (selectedButton >= 0)
			{
				StartCoroutine(
					CardSpawner.Instance.SpawnCardToHand(("GrimoraMod_" + AllGrimoraCardNames[selectedButton]).GetCardInfo())
				);
			}
		}

		if (_toggleDebugCustomCardsHand
		    && !_toggleDebugBaseModCardsHand
		    && !_toggleDebugBaseModCardsDeck
		    && !_toggleDebugCustomCardsDeck)
		{
			int selectedButton = GUI.SelectionGrid(
				new Rect(Screen.width - 420, 100, 400, Screen.height - 300),
				-1,
				AllGrimoraCustomCardNames,
				3
			);

			if (selectedButton >= 0)
			{
				StartCoroutine(
					CardSpawner.Instance.SpawnCardToHand(
						("GrimoraMod_" + AllGrimoraCustomCardNames[selectedButton]).GetCardInfo()
					)
				);
			}
		}

		if (_toggleDebugBaseModCardsDeck
		    && !_toggleDebugCustomCardsDeck
		    && !_toggleDebugCustomCardsHand
		    && !_toggleDebugBaseModCardsHand)
		{
			int selectedButton = GUI.SelectionGrid(
				new Rect(Screen.width - 420, 100, 400, Screen.height - 300),
				-1,
				AllGrimoraCardNames,
				3
			);

			if (selectedButton >= 0)
			{
				GrimoraSaveUtil.AddCard(("GrimoraMod_" + AllGrimoraCardNames[selectedButton]).GetCardInfo());
			}
		}

		if (_toggleDebugCustomCardsDeck
		    && !_toggleDebugBaseModCardsDeck
		    && !_toggleDebugCustomCardsHand
		    && !_toggleDebugBaseModCardsHand)
		{
			int selectedButton = GUI.SelectionGrid(
				new Rect(Screen.width - 420, 100, 400, Screen.height - 300),
				-1,
				AllGrimoraCustomCardNames,
				3
			);

			if (selectedButton >= 0)
			{
				GrimoraSaveUtil.AddCard(("GrimoraMod_" + AllGrimoraCustomCardNames[selectedButton]).GetCardInfo());
			}
		}
	}
}
