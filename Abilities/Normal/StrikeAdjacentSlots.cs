using System.Collections;
using DiskCardGame;
using InscryptionAPI.Card;

namespace GrimoraMod;

public abstract class StrikeAdjacentSlots : ExtendedAbilityBehaviour
{
	protected abstract Ability StrikeAdjacentAbility { get; }

	public int damageDoneToPlayer = 0;

	public override bool RespondsToSlotTargetedForAttack(CardSlot slot, PlayableCard attacker)
	{
		// check if the attacking card is this card
		if (attacker && attacker.Slot == Card.Slot && slot.Card.IsNull())
		{
			if (attacker.Slot.IsPlayerSlot)
			{
				// if the attacker slot is the player, return if the targeted slot is also the player slot 
				return slot.IsPlayerSlot;
			}

			// check if slot being attacked is the opponent slot if the attacking slot is the opponent
			return !slot.IsPlayerSlot;
		}

		return false;
	}

	public override IEnumerator OnSlotTargetedForAttack(CardSlot slot, PlayableCard attacker)
	{
		damageDoneToPlayer += Card.Attack;
		yield break;
	}

	public override bool RespondsToUpkeep(bool playerUpkeep)
	{
		return damageDoneToPlayer > 0;
	}

	public override IEnumerator OnUpkeep(bool playerUpkeep)
	{
		damageDoneToPlayer = 0;
		yield break;
	}

	public override bool RemoveDefaultAttackSlot() => true;

	public override bool RespondsToGetOpposingSlots() => true;

	public override List<CardSlot> GetOpposingSlots(List<CardSlot> originalSlots, List<CardSlot> otherAddedSlots)
	{
		var toLeftSlot = BoardManager.Instance.GetAdjacent(Card.Slot, true);
		var toRightSlot = BoardManager.Instance.GetAdjacent(Card.Slot, false);

		bool hasInvertedStrike = Card.HasAbility(InvertedStrike.ability);
		bool hasAlternatingStrike = Card.HasAbility(AlternatingStrike.ability);

		List<CardSlot> slotsToTarget = new List<CardSlot>();

		if (StrikeAdjacentAbility != Raider.ability)
		{
			slotsToTarget.AddRange(BoardManager.Instance.GetAdjacentSlots(Card.Slot.opposingSlot));
			slotsToTarget.Add(Card.Slot.opposingSlot);
		}

		if (toLeftSlot)
		{
			if (StrikeAdjacentAbility != Raider.ability
			    || toLeftSlot.Card.IsNull()
			    || !toLeftSlot.Card.HasAbility(Raider.ability))
			{
				slotsToTarget.Insert(0, toLeftSlot);
			}
		}

		// insert at end
		if (toRightSlot)
		{
			if (StrikeAdjacentAbility != Raider.ability
			    || toRightSlot.Card.IsNull()
			    || !toRightSlot.Card.HasAbility(Raider.ability))
			{
				slotsToTarget.Insert(slotsToTarget.Count, toRightSlot);
			}
		}

		if (hasInvertedStrike)
		{
			slotsToTarget.Reverse();
		}

		if (hasAlternatingStrike && StrikeAdjacentAbility == AreaOfEffectStrike.ability)
		{
			List<CardSlot> alternatedResult = new List<CardSlot>()
			{
				slotsToTarget[4], // right adj
				slotsToTarget[0], // left adj
				slotsToTarget[3], // right opposing
				slotsToTarget[1], // left opposing
				slotsToTarget[2], // center
			};

			slotsToTarget = alternatedResult;
		}

		slotsToTarget.Sort((slot1, slot2) => slot1.Index - slot2.Index);
		return slotsToTarget;
	}
}
