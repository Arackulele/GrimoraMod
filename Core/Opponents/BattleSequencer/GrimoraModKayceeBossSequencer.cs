using System.Collections;
using DiskCardGame;
using UnityEngine;
using static GrimoraMod.GrimoraPlugin;

namespace GrimoraMod;

public class GrimoraModKayceeBossSequencer : GrimoraModBossBattleSequencer
{
	public override Opponent.Type BossType => BaseBossExt.KayceeOpponent;

	public override EncounterData BuildCustomEncounter(CardBattleNodeData nodeData)
	{
		return new EncounterData()
		{
			opponentType = BossType
		};
	}

	public override bool RespondsToUpkeep(bool playerUpkeep)
	{
		return playerUpkeep;
	}

	private int _freezeCounter = 0;

	public override IEnumerator OnUpkeep(bool playerUpkeep)
	{
		var playerCardsWithAttacks
			= BoardManager.Instance.GetPlayerCards(pCard => pCard.Attack > 0);

		_freezeCounter++;
		if (playerCardsWithAttacks.IsNotEmpty())
		{
			if (_freezeCounter >= 3)
			{
				ViewManager.Instance.SwitchToView(View.Board, lockAfter: true);
				yield return TextDisplayer.Instance.ShowUntilInput(
					$"IT'S TIME FOR YOUR CARDS TO FREEZE! {"CHILLED".Blue()} TO THE BONE!"
				);
				foreach (var card in playerCardsWithAttacks)
				{
					int attack = card.Attack == 0 ? 0 : -card.Attack;
					var modInfo = new CardModificationInfo()
					{
						attackAdjustment = attack,
						healthAdjustment = 1 - card.Health,
						abilities = new List<Ability>() { Ability.IceCube }
					};
					card.Info.iceCubeParams = new IceCubeParams() { creatureWithin = card.Info };
					card.AddTemporaryMod(modInfo);
					card.Anim.PlayTransformAnimation();
					yield return new WaitForSeconds(0.05f);
					card.RenderCard();
					_freezeCounter = 0;
				}
			}
		}

		var draugrCards
			= BoardManager.Instance.GetOpponentCards(pCard => pCard.InfoName().Equals(NameDraugr));
		Log.LogDebug($"[KayceeSequencer] Draugr cards found [{draugrCards.GetDelimitedString()}]");
		if (draugrCards.Count >= 2)
		{
			ViewManager.Instance.SwitchToView(View.Board);
			yield return TextDisplayer.Instance.ShowUntilInput(
				$"ALL THIS {"ICE".BrightBlue()} IS TAKING UP TOO MUCH SPACE!"
			);
			foreach (var card in draugrCards)
			{
				yield return card.Die(false);
				yield return new WaitForSeconds(0.1f);
			}
		}

		ViewManager.Instance.SwitchToView(View.Default);
	}
}
