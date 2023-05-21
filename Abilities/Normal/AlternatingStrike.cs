using System.Collections;
using DiskCardGame;
using InscryptionAPI.Card;
using InscryptionAPI.Helpers.Extensions;
using InscryptionAPI.Triggers;
using Sirenix.Utilities;

namespace GrimoraMod;

public class AlternatingStrike : AbilityBehaviour, IGetOpposingSlots
{
	public static Ability ability;
	public override Ability Ability => ability;

	public bool isAttackingLeft = true;

	public override bool RespondsToSlotTargetedForAttack(CardSlot slot, PlayableCard attacker) => attacker && attacker == Card;

	public override IEnumerator OnSlotTargetedForAttack(CardSlot slot, PlayableCard attacker)
	{
		// if in far left slot, adj slot left will be null
		CardSlot slotToAttack = Card.OpposingSlot().GetAdjacent(isAttackingLeft);

		if (slotToAttack.SafeIsUnityNull())
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

	public bool RemoveDefaultAttackSlot() => true;

	public bool RespondsToGetOpposingSlots() => Card.LacksAbility(AreaOfEffectStrike.ability);

	public List<CardSlot> GetOpposingSlots(List<CardSlot> originalSlots, List<CardSlot> otherAddedSlots)
	{
		CardSlot slotToAttack = Card.OpposingSlot().GetAdjacent(isAttackingLeft);
		if (!slotToAttack)
		{
			slotToAttack = Card.OpposingSlot().GetAdjacent(!isAttackingLeft);
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

		AbilityBuilder<AlternatingStrike>.Builder
		 .FlipIconIfOnOpponentSide()
		 .SetRulebookDescription(rulebookDescription)
		 .SetPowerLevel(0)
		 .Build();
	}
}
