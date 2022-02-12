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
		var playerSlotsWithCards = CardSlotUtils.GetPlayerSlotsWithCards();
		if (!playerSlotsWithCards.IsNullOrEmpty())
		{
			yield return base.PreSuccessfulTriggerSequence();
			ViewManager.Instance.SwitchToView(View.Board);
			yield return TextDisplayer.Instance.ShowUntilInput(
				"DID YOU REALLY THINK THE BONE LORD WOULD LET YOU OFF THAT EASILY?!"
			);
			foreach (var cardSlot in playerSlotsWithCards)
			{
				cardSlot.Card.Anim.StrongNegationEffect();
				cardSlot.Card.AddTemporaryMod(new CardModificationInfo(1 - Card.Attack, 0));
				cardSlot.Card.Anim.StrongNegationEffect();
				yield return new WaitForSeconds(0.1f);
			}
		}
	}

	public static NewAbility Create()
	{
		const string rulebookDescription =
			"Whenever [creature] gets played, all enemies Power is set to 1. " +
			"When the Bone Lord appears, every Creature will fall.";

		return ApiUtils.CreateAbility<BoneLordsReign>(
			rulebookDescription, "Bone Lord's Reign"
		);
	}
}
