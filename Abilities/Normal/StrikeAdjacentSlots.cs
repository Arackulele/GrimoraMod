using System.Collections;
using DiskCardGame;
using InscryptionAPI.Card;
using InscryptionAPI.Helpers.Extensions;
using InscryptionAPI.Triggers;
using Sirenix.Utilities;

namespace GrimoraMod;

public abstract class StrikeAdjacentSlots : AbilityBehaviour, IGetOpposingSlots
{
	protected abstract Ability StrikeAdjacentAbility { get; }

	public int damageDoneToPlayer = 0;

	public override bool RespondsToSlotTargetedForAttack(CardSlot slot, PlayableCard attacker)
	{
		// check if the attacking card is this card
		if (attacker && attacker.Slot == Card.Slot && slot.Card.SafeIsUnityNull())
		{
			if (attacker.Slot.IsPlayerSlot)
			{
				// if the attacker slot is the player, return if the targeted slot is also the player slot 
				return slot.IsPlayerSlot;
			}

			// check if slot being attacked is the opponent slot if the attacking slot is the opponent
			return slot.IsOpponentSlot();
		}

		return false;
	}
	/* Duplicate Code in an API Update
	public override IEnumerator OnSlotTargetedForAttack(CardSlot slot, PlayableCard attacker)
	{
		damageDoneToPlayer += Card.Attack;
		yield break;
	}
	*/
	public override bool RespondsToUpkeep(bool playerUpkeep) => damageDoneToPlayer > 0;

	public override IEnumerator OnUpkeep(bool playerUpkeep)
	{
		damageDoneToPlayer = 0;
		yield break;
	}

	public bool RemoveDefaultAttackSlot() => true;

	public bool RespondsToGetOpposingSlots() => true;

	public List<CardSlot> GetOpposingSlots(List<CardSlot> originalSlots, List<CardSlot> otherAddedSlots)
	{
		var toLeftSlot = Card.Slot.GetAdjacent(true);
		var toRightSlot = Card.Slot.GetAdjacent(false);
		var opposingSlot = Card.OpposingSlot();

		bool hasInvertedStrike = Card.HasAbility(InvertedStrike.ability);
		bool hasAlternatingStrike = Card.HasAbility(AlternatingStrike.ability);

		List<CardSlot> slotsToTarget = new List<CardSlot>();

		if (StrikeAdjacentAbility != Raider.ability)
		{
			slotsToTarget.AddRange(opposingSlot.GetAdjacentSlots());
			slotsToTarget.Add(opposingSlot);
		}

		if (toLeftSlot && (StrikeAdjacentAbility != Raider.ability
		                || toLeftSlot.Card.SafeIsUnityNull()
		                || toLeftSlot.Card.LacksAbility(Raider.ability)))
		{
			slotsToTarget.Insert(0, toLeftSlot);
		}

		// insert at end
		if (toRightSlot && (StrikeAdjacentAbility != Raider.ability
		                 || toRightSlot.Card.SafeIsUnityNull()
		                 || toRightSlot.Card.LacksAbility(Raider.ability)))
		{
			slotsToTarget.Insert(slotsToTarget.Count, toRightSlot);
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
