using System.Collections;
using APIPlugin;
using DiskCardGame;
using UnityEngine;

namespace GrimoraMod;

public class BoneLordsReign : AbilityBehaviour
{
	public static Ability ability;

	public override Ability Ability => ability;

	public override bool RespondsToResolveOnBoard()
	{
		return true;
	}

	public override IEnumerator OnResolveOnBoard()
	{
		var activePlayerCards = BoardManager.Instance.GetPlayerCards();
		GrimoraPlugin.Log.LogDebug($"[BoneLord] Player cards [{activePlayerCards.Count}]");
		if (activePlayerCards.IsNotEmpty())
		{
			yield return PreSuccessfulTriggerSequence();
			ViewManager.Instance.SwitchToView(View.Board);
			yield return TextDisplayer.Instance.ShowUntilInput(
				"DID YOU REALLY THINK THE BONE LORD WOULD LET YOU OFF THAT EASILY?!"
			);
			foreach (var playableCard in activePlayerCards)
			{
				playableCard.Anim.StrongNegationEffect();
				int attack = playableCard.Attack == 0 ? 0 : 1 - playableCard.Attack;
				CardModificationInfo mod = new CardModificationInfo(attack, 1 - playableCard.Health);
				playableCard.AddTemporaryMod(mod);
				playableCard.Anim.PlayTransformAnimation();
				yield return new WaitForSeconds(0.1f);
			}
		}
	}

	public static NewAbility Create()
	{
		const string rulebookDescription =
			"Whenever [creature] gets played, all enemies attack and health is set to 1.";

		return ApiUtils.CreateAbility<BoneLordsReign>(rulebookDescription, "Bone Lord's Reign");
	}
}
