using DiskCardGame;
using UnityEngine;
using static GrimoraMod.GrimoraPlugin;

namespace GrimoraMod;

public class DebugHelper : ManagedBehaviour
{
	private bool _toggleDebugBosses = false;

	private readonly string[] _btnBosses =
	{
		"Kaycee", "Sawyer", "Royal", "Grimora"
	};

	private bool _toggleDebugChests = false;

	private readonly string[] _btnChests =
	{
		"Card Remove", "Rare Card"
	};

	private bool _toggleDebugTools = false;

	private readonly string[] _btnTools =
	{
		"Win Round", "Lose Round"
	};

	private void OnGUI()
	{
		if (ConfigHelper.IsDevModeEnabled)
		{
			_toggleDebugBosses = GUI.Toggle(
				new Rect(20, 60, 200, 20),
				_toggleDebugBosses,
				"Debug-Bosses"
			);

			_toggleDebugChests = GUI.Toggle(
				new Rect(20, 120, 200, 20),
				_toggleDebugChests,
				"Debug-Chests"
			);

			_toggleDebugTools = GUI.Toggle(
				new Rect(20, 180, 200, 20),
				_toggleDebugTools,
				"Debug Tools"
			);

			if (_toggleDebugBosses)
			{
				int selectedBtnBosses = GUI.SelectionGrid(
					new Rect(25, 80, 400, 40),
					-1,
					_btnBosses,
					2
				);

				if (selectedBtnBosses >= 0)
				{
					string bossName = _btnBosses[selectedBtnBosses] switch
					{
						"Kaycee" => KayceeBossOpponent.SpecialId,
						"Sawyer" => SawyerBossOpponent.SpecialId,
						"Royal" => RoyalBossOpponentExt.SpecialId,
						"Grimora" => GrimoraBossOpponentExt.SpecialId
					};

					ChessboardMapExt.Instance.ActiveChessboard.PlaceBossPieceDev(bossName, 7, 4);

					Log.LogDebug($"Finished placing boss piece [{bossName}]");
				}
			}

			if (_toggleDebugChests)
			{
				int selectedBtnChests = GUI.SelectionGrid(
					new Rect(25, 140, 200, 20),
					-1,
					_btnChests,
					2
				);

				if (selectedBtnChests >= 0)
				{
					SpecialNodeData nodeType = _btnChests[selectedBtnChests] switch
					{
						"Card Remove" => new CardRemoveNodeData(),
						"Rare Card" => new ChooseRareCardNodeData(),
						_ => new CardChoicesNodeData()
					};

					var activeChests = UnityEngine.Object.FindObjectsOfType<ChessboardChestPiece>();
					if (activeChests.Length == 0)
					{
						ConfigHelper.Instance.CurrentRemovedPieces.Value = "";
						for (int i = 0; i < 8; i++)
						{
							// GrimoraChessboard.GetNodeAtSpace(i, 0).OccupyingPiece = null;
							ChessboardMapExt.Instance.ActiveChessboard.PlaceChestPiece(i, 0, nodeType);
						}

						Log.LogDebug($"Finished placing [{nodeType.GetType()}] [{activeChests.Length}] chests");
					}
					else
					{
						foreach (var chest in activeChests)
						{
							chest.NodeData = nodeType;
						}

						Log.LogDebug($"Finished assigning [{nodeType.GetType()}] to [{activeChests.Length}] chests");
					}
				}
			}

			if (_toggleDebugTools)
			{
				int selectedBtnTools = GUI.SelectionGrid(
					new Rect(25, 200, 200, 20),
					-1,
					_btnTools,
					2
				);

				if (selectedBtnTools >= 0)
				{
					// Log.LogDebug($"[OnGUI] Calling button [{selectedButton}]");
					switch (_btnTools[selectedBtnTools])
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
					}
				}
			}
		}
	}
}
