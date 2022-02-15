using System.Collections;
using DiskCardGame;
using Sirenix.Utilities;
using UnityEngine;
using static GrimoraMod.GrimoraPlugin;

namespace GrimoraMod;

public class GrimoraModRoyalBossSequencer : GrimoraModBossBattleSequencer
{
	private readonly RandomEx _rng = new();

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
		var activePlayerCards = BoardManager.Instance.GetPlayerCards();
		if (!activePlayerCards.IsNullOrEmpty() && _rng.NextBoolean())
		{
			var playableCard = activePlayerCards[UnityEngine.Random.Range(0, activePlayerCards.Count)];
			Log.LogDebug($"[{GetType()}] About to assign ExplodeOnDeath to [{playableCard.Info.name}]");
			ViewManager.Instance.SwitchToView(View.Board);
			yield return new WaitForSeconds(0.25f);
			yield return TextDisplayer.Instance.ShowUntilInput(
				$"YARRRR, I WILL ENJOY THE KABOOM OF [c:bR]{playableCard.Info.displayedName}[c:]", 1f, 0.5f, Emotion.Anger
			);
			if (!playableCard.TemporaryMods.Exists(mod => mod.abilities.Contains(Ability.ExplodeOnDeath)))
			{
				playableCard.AddTemporaryMod(new CardModificationInfo(Ability.ExplodeOnDeath));
			}

			playableCard.Anim.StrongNegationEffect();
			yield return new WaitForSeconds(0.25f);
			yield return playableCard.TakeDamage(1, null);
			yield return new WaitForSeconds(0.5f);
		}
	}
}
