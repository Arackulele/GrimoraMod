using System.Collections;
using DiskCardGame;

namespace GrimoraMod;

public abstract class StrikeAdjacentSlots : AbilityBehaviour
{
	protected abstract Ability strikeAdjacentAbility { get; }
	
	public int damageDoneToPlayer = 0;

	public override bool RespondsToSlotTargetedForAttack(CardSlot slot, PlayableCard attacker)
	{
		// check if the attacking card is this card
		if (attacker.IsNotNull() && attacker.Slot == Card.Slot && slot.Card.IsNull())
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
}
