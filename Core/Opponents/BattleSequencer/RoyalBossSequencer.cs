using System.Collections;
using DiskCardGame;
using UnityEngine;
using static GrimoraMod.GrimoraPlugin;

namespace GrimoraMod;

public class RoyalBossSequencer : GrimoraModBossBattleSequencer
{
	public override Opponent.Type BossType => BaseBossExt.RoyalOpponent;

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

	public override IEnumerator OnUpkeep(bool playerUpkeep)
	{
		RandomEx rnd = new RandomEx();
		var playerSlotsWithCards = CardSlotUtils.GetPlayerSlotsWithCards();
		if (playerSlotsWithCards.Count > 0 && rnd.NextBoolean())
		{
			var playableCard = playerSlotsWithCards[UnityEngine.Random.Range(0, playerSlotsWithCards.Count)].Card;
			Log.LogDebug($"[{GetType()}] About to assign ExplodeOnDeath to [{playableCard.Info.name}]");
			playableCard.AddTemporaryMod(new CardModificationInfo(Ability.ExplodeOnDeath));
			yield return playableCard.TakeDamage(1, null);
			playableCard.Anim.StrongNegationEffect();
			yield return new WaitForSeconds(1f);
		}

		yield break;
	}
}
