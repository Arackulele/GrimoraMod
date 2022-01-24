using System.Collections;
using APIPlugin;
using DiskCardGame;
using UnityEngine;

namespace GrimoraMod;

public class BoneLordsReign : AbilityBehaviour
{
	public static Ability ability;

	public override Ability Ability => ability;

	public bool playedOnBoard = false;

	public override bool RespondsToResolveOnBoard()
	{
		return true;
	}

	public override IEnumerator OnResolveOnBoard()
	{
		var playerSlotsWithCards = CardSlotUtils.GetPlayerSlotsWithCards();
		if (playerSlotsWithCards.Count > 0 && !playedOnBoard)
		{
			yield return base.PreSuccessfulTriggerSequence();
			ViewManager.Instance.SwitchToView(View.Board);
			yield return TextDisplayer.Instance.ShowUntilInput(
				"DID YOU REALLY THINK THE BONELORD WOULD LET YOU OFF THAT EASILY?!"
			);
			foreach (var cardSlot in playerSlotsWithCards)
			{
				cardSlot.Card.Anim.StrongNegationEffect();
				cardSlot.Card.AddTemporaryMod(new CardModificationInfo(1 - Card.Attack, 0));
				cardSlot.Card.Anim.StrongNegationEffect();
				yield return new WaitForSeconds(0.1f);
			}

			playedOnBoard = true;
		}
	}

	public static NewAbility CreateBoneLordsReign()
	{
		const string rulebookDescription =
			"Whenever [creature] gets played, all enemies Power is set to 1. " +
			"When the Bone Lord appears, every Creature will fall.";

		return ApiUtils.CreateAbility<BoneLordsReign>(
			GrimoraPlugin.AllSpriteAssets.Single(spr => spr.name == "BoneLordsReign").texture,
			nameof(BoneLordsReign),
			rulebookDescription,
			5
		);
	}
}
