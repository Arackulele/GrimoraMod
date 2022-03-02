using System.Collections;
using APIPlugin;
using DiskCardGame;
using static GrimoraMod.GrimoraPlugin;

namespace GrimoraMod;

public class AlternatingStrike : AbilityBehaviour
{
	public static Ability ability;
	public override Ability Ability => ability;

	public bool isAttackingLeft = true;

	public override bool RespondsToSlotTargetedForAttack(CardSlot slot, PlayableCard attacker)
	{
		return attacker == base.Card;
	}

	public override IEnumerator OnSlotTargetedForAttack(CardSlot slot, PlayableCard attacker)
	{
		// if in far left slot, adj slot left will be null
		CardSlot slotToAttack = BoardManager.Instance.GetAdjacent(Card.Slot.opposingSlot, isAttackingLeft);

		if (slotToAttack is null)
		{
			isAttackingLeft = !isAttackingLeft;
			// if in far left slot and attacked right last, need to keep attack to the right slot
		}

		yield return base.OnSlotTargetedForAttack(slot, attacker);
	}

	public override bool RespondsToAttackEnded()
	{
		return true;
	}

	public override IEnumerator OnAttackEnded()
	{
		isAttackingLeft = !isAttackingLeft;
		yield return base.OnAttackEnded();
	}

	public static NewAbility Create()
	{
		const string rulebookDescription =
			"[creature] alternates between striking the opposing space to the left and right from it.";

		return ApiUtils.CreateAbility<AlternatingStrike>(rulebookDescription);
	}
}
