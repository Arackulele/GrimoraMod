using System.Collections;
using DiskCardGame;
using Sirenix.Utilities;
using UnityEngine;

namespace GrimoraMod;

public class KayceeBossSequencer : GrimoraModBossBattleSequencer
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

	public int freezeCounter = 0;

	public override IEnumerator OnUpkeep(bool playerUpkeep)
	{
		var playerCardsWithAttacks
			= CardSlotUtils.GetPlayerSlotsWithCards()
				.Select(slot => slot.Card)
				.Where(card => card.Attack > 0)
				.ToList();

		freezeCounter++;
		if (!playerCardsWithAttacks.IsNullOrEmpty())
		{
			if (freezeCounter >= 3)
			{
				StartCoroutine(TextDisplayer.Instance.ShowUntilInput("Freeze!"));
				foreach (var card in playerCardsWithAttacks)
				{
					card.Anim.StrongNegationEffect();
					card.Anim.StrongNegationEffect();
					card.AddTemporaryMod(new CardModificationInfo(1 - card.Attack, 0));
					card.Anim.StrongNegationEffect();
					card.Anim.StrongNegationEffect();
					yield return new WaitForSeconds(0.05f);

					freezeCounter = 0;
				}
			}
		}
	}
}
