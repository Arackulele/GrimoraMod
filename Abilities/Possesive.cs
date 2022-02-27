using System.Collections;
using APIPlugin;
using DiskCardGame;

namespace GrimoraMod;

public class Possessive : AbilityBehaviour
{
	public static readonly NewAbility NewAbility = Create();

	public static Ability ability;
	public override Ability Ability => ability;

	public override bool RespondsToSlotTargetedForAttack(CardSlot slot, PlayableCard attacker)
	{
		return attacker.Slot == Card.Slot.opposingSlot;
	}

	public override IEnumerator OnSlotTargetedForAttack(CardSlot slot, PlayableCard attacker)
	{
		attacker.Anim.StrongNegationEffect();
		yield break;
	}

	public static NewAbility Create()
	{
		const string rulebookDescription =
			"[creature] cannot be attacked from the opposing slot. " +
			"The opposing slot instead attacks one of its adjacent friendly slots if possible.";

		return ApiUtils.CreateAbility<Possessive>(rulebookDescription);
	}
}
