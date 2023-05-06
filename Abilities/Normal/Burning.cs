using System.Collections;
using DiskCardGame;
using InscryptionAPI.Card;
using UnityEngine;
using static GrimoraMod.GrimoraPlugin;

namespace GrimoraMod;

public class Burning : AbilityBehaviour
{
	public static Ability ability;

	public override Ability Ability => ability;


	public static void tryburningcard(Card card,  string FireParticles)
	{
		if (card.transform.Find(FireParticles) == null)
		{
			GameObject cardfire = GameObject.Instantiate(kopieGameObjects.Find(g => g.name.Contains(FireParticles)));
			cardfire.transform.parent = card.transform;
			//need to set the position again because of weird card transforms
			cardfire.transform.localPosition = new Vector3(-0.0836f, 0, 0);
		}
	}


public override bool RespondsToDealDamage(int amount, PlayableCard target) => Card.NotDead() && amount > 0;

	public override bool RespondsToTurnEnd(bool playerTurnEnd)
	{
		return base.Card.OpponentCard != playerTurnEnd;
	}
	public override bool RespondsToTakeDamage(PlayableCard source)
	{
		return true;
	}
	public override bool RespondsToResolveOnBoard()
	{
		return true;
	}

	public override IEnumerator OnResolveOnBoard()
	{
		tryburningcard(Card, "CardFire");
		yield return new WaitForSeconds(0.2f);

		yield break;
	}

	public override IEnumerator OnTakeDamage(PlayableCard source)
	{
		if (source != null)
		{
			if (!source.HasAbility(Ability.MadeOfStone) && !source.HasAbility(Burning.ability))
			{
				yield return base.PreSuccessfulTriggerSequence();
				if (source.AllAbilities().Count() > 4)
				{
					source.Anim.StrongNegationEffect();
					source.TakeDamage(1, Card);
				}
				else
				{ 
				CardModificationInfo fire = new CardModificationInfo();
				fire.abilities.Add(Burning.ability);
				source.AddTemporaryMod(fire);
				tryburningcard(Card, "CardFire");
				source.RenderCard();
				}
			}
		}
		yield break;
	}
	public override IEnumerator OnTurnEnd(bool playerTurnEnd)
	{


		if (!base.Card.HasAbility(Ability.MadeOfStone))
		{
			yield return base.PreSuccessfulTriggerSequence();
			//exploding cards explode instantly
			if (base.Card.HasAbility(Ability.ExplodeOnDeath | LitFuse.ability)) { yield return base.Card.Die(false); }
			//Frozen Cards take double damage
			if (base.Card.HasTrait(Trait.DeathcardCreationNonOption)) yield return base.Card.TakeDamage(2, null);
			else { yield return base.Card.TakeDamage(1, null); }
			yield return base.LearnAbility(0.1f);
		}
		yield break;
	}
}



	public partial class GrimoraPlugin
	{
	public void Add_Ability_Burning()
	{

		const string rulebookDescription = "[creature] takes 1 Damage at the end of your turn, when it gets attacked by another Card, that card gets set on fire.";

			AbilityBuilder<Burning>.Builder
			 .SetRulebookDescription(rulebookDescription)
			 .Build();
		}
}

