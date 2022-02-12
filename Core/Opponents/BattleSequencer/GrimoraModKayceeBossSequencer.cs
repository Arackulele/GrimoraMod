using System.Collections;
using DiskCardGame;
using Sirenix.Utilities;
using UnityEngine;

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
	private int _iceBreakCounter = 0;

	public override IEnumerator OnUpkeep(bool playerUpkeep)
	{
		var playerCardsWithAttacks
			= CardSlotUtils.GetPlayerSlotsWithCards()
				.Select(slot => slot.Card)
				.Where(card => card.Attack > 0)
				.ToList();

		_freezeCounter++;
		if (!playerCardsWithAttacks.IsNullOrEmpty())
		{
			if (_freezeCounter >= 3)
			{
				ViewManager.Instance.SwitchToView(View.BossCloseup);
				yield return TextDisplayer.Instance.ShowUntilInput(
					"It's time for your cards attack to freeze! CHILLED TO THE BONE!"
				);
				ViewManager.Instance.SwitchToView(View.Board);
				foreach (var card in playerCardsWithAttacks)
				{
					int attack = card.Attack == 0 ? 0 : 1 - card.Attack;
					card.Anim.StrongNegationEffect();
					card.Anim.StrongNegationEffect();
					card.AddTemporaryMod(new CardModificationInfo(attack, 0));
					card.Anim.StrongNegationEffect();
					card.Anim.StrongNegationEffect();
					yield return new WaitForSeconds(0.05f);

					_freezeCounter = 0;
				}
			}
		}

		var opponentCards = CardSlotUtils.GetOpponentSlotsWithCards();
		var draugrCards = opponentCards.FindAll(slot => slot.Card.name.Equals(GrimoraPlugin.NameDraugr));
		if (++_iceBreakCounter == 2 && draugrCards.Count >= 2)
		{
			ViewManager.Instance.SwitchToView(View.Board);
			yield return TextDisplayer.Instance.ShowUntilInput("ALL THIS ICE IS TAKING UP TOO MUCH SPACE!");
			foreach (var card in draugrCards.Select(slot => slot.Card))
			{
				yield return card.Die(false);
				yield return new WaitForSeconds(0.1f);
			}
		}

		ViewManager.Instance.SwitchToView(View.Default);
	}
}
