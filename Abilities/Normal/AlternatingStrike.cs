using System.Collections;
using DiskCardGame;
using InscryptionAPI.Card;

namespace GrimoraMod;

public class AlternatingStrike : ExtendedAbilityBehaviour
{
	public static Ability ability;
	public override Ability Ability => ability;

	public bool isAttackingLeft = true;

	public override bool RespondsToSlotTargetedForAttack(CardSlot slot, PlayableCard attacker)
	{
		return attacker && attacker == Card;
	}

	public override IEnumerator OnSlotTargetedForAttack(CardSlot slot, PlayableCard attacker)
	{
		// if in far left slot, adj slot left will be null
		CardSlot slotToAttack = BoardManager.Instance.GetAdjacent(Card.Slot.opposingSlot, isAttackingLeft);

		if (slotToAttack.IsNull())
		{
			isAttackingLeft = !isAttackingLeft;
			// if in far left slot and attacked right last, need to keep attack to the right slot
		}

		yield return base.OnSlotTargetedForAttack(slot, attacker);
	}

	public override bool RespondsToAttackEnded() => true;

	public override IEnumerator OnAttackEnded()
	{
		isAttackingLeft = !isAttackingLeft;
		yield return base.OnAttackEnded();
	}

	public override bool RemoveDefaultAttackSlot() => true;

	public override bool RespondsToGetOpposingSlots()
	{
		return !Card.HasAbility(AreaOfEffectStrike.ability);
	}

	public override List<CardSlot> GetOpposingSlots(List<CardSlot> originalSlots, List<CardSlot> otherAddedSlots)
	{
		CardSlot slotToAttack = BoardManager.Instance.GetAdjacent(Card.Slot, isAttackingLeft);
		if (!slotToAttack)
		{
			slotToAttack = BoardManager.Instance.GetAdjacent(Card.Slot, !isAttackingLeft);
		}

		return new List<CardSlot> { slotToAttack };
	}
}

public partial class GrimoraPlugin
{
	public void Add_Ability_AlternatingStrike()
	{
		const string rulebookDescription =
			"[creature] alternates between striking the opposing space to the left and right from it.";

		ApiUtils.CreateAbility<AlternatingStrike>(rulebookDescription);
	}
}
