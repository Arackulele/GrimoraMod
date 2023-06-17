using System.Collections;
using DiskCardGame;
using InscryptionAPI.Card;
using InscryptionAPI.Helpers.Extensions;
using UnityEngine;

namespace GrimoraMod;

public class Slasher : AbilityBehaviour
{
	public static Ability ability;
	public override Ability Ability => ability;

	public override bool RespondsToDealDamage(int amount, PlayableCard target) => Card.NotDead() && amount > 0;

	public override IEnumerator OnDealDamage(int amount, PlayableCard target)
	{
		yield return PreSuccessfulTriggerSequence();


		 CardSlot left = target.Slot.GetAdjacent(true);
		CardSlot right = target.Slot.GetAdjacent(false);

		if (left != null && left.Card != null)
		{
			//This uses Take damage instead of other strike sigils which directly modify the attack behavior, because a slash is not an additional hit but instead a side effect, attacker is null so this doesnt trigger itself over and over again
			if (left.Card.Health > amount) yield return left.Card.TakeDamage(amount, null);
			else yield return left.Card.Die(Card);

		}
		else if (right != null && right.Card != null)
		{
			if (right.Card.Health > amount) yield return right.Card.TakeDamage(amount, null);
			else yield return right.Card.Die(Card);

		}
		else Card.Anim.StrongNegationEffect();

		left = null;
		right = null;

		yield return LearnAbility(0.1f);
	}

}

public partial class GrimoraPlugin
{
	public void Add_Ability_Slasher()
	{
			const string rulebookDescription =
				"When [creature] hits an opposing card, one of the targets adjacent allies will take damage as well ";

		AbilityBuilder<Slasher>.Builder
		 .SetRulebookDescription(rulebookDescription)
		 .Build();
	}
}
