using System.Collections;
using APIPlugin;
using DiskCardGame;
using Sirenix.Utilities;
using UnityEngine;

namespace GrimoraMod;

public class BoneLordsReign : AbilityBehaviour
{
	public static Ability ability;

	public override Ability Ability => ability;

	public override bool RespondsToPlayFromHand()
	{
		return true;
	}

	public override IEnumerator OnPlayFromHand()
	{
		var activePlayerCards = BoardManager.Instance.GetPlayerCards();
		if (activePlayerCards.IsNotEmpty())
		{
			yield return PreSuccessfulTriggerSequence();
			ViewManager.Instance.SwitchToView(View.Board);
			yield return TextDisplayer.Instance.ShowUntilInput(
				"DID YOU REALLY THINK THE BONE LORD WOULD LET YOU OFF THAT EASILY?!"
			);
			foreach (var cardSlot in activePlayerCards)
			{
				cardSlot.Anim.StrongNegationEffect();
				cardSlot.AddTemporaryMod(new CardModificationInfo(1 - Card.Attack, 0));
				cardSlot.Anim.StrongNegationEffect();
				yield return new WaitForSeconds(0.1f);
			}
		}
	}

	public static NewAbility Create()
	{
		const string rulebookDescription =
			"Whenever [creature] gets played, all enemies attack and health is set to 1.";

		return ApiUtils.CreateAbility<BoneLordsReign>(
			rulebookDescription, "Bone Lord's Reign"
		);
	}
}
