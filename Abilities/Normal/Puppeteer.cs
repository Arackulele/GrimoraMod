using System.Collections;
using DiskCardGame;
using UnityEngine;

namespace GrimoraMod;

public class Puppeteer : AbilityBehaviour
{
	private static readonly CardModificationInfo NegateBrittleMod = new()
	{
		negateAbilities = new List<Ability> { Ability.Brittle },
		singletonId = "grimoramod_puppeteer"
	};

	public static Ability ability;

	public override Ability Ability => ability;

	public override bool RespondsToResolveOnBoard() => true;

	public override IEnumerator OnResolveOnBoard()
	{
		yield return RemoveBrittleFromCards();
	}

	public override bool RespondsToOtherCardResolve(PlayableCard otherCard)
	{
		return otherCard
		    && (otherCard.OpponentCard && Card.OpponentCard || !otherCard.OpponentCard && !Card.OpponentCard)
		    && otherCard.HasAbility(Ability.Brittle);
	}

	public override IEnumerator OnOtherCardResolve(PlayableCard otherCard)
	{
		yield return RemoveBrittle(otherCard);
	}

	private IEnumerator RemoveBrittleFromCards()
	{
		List<PlayableCard> cardsWithBrittle = BoardManager.Instance.GetSlots(!Card.OpponentCard)
		 .Where(slot => slot.Card && slot.Card.HasAbility(Ability.Brittle))
		 .Select(slot => slot.Card)
		 .ToList();

		foreach (var card in cardsWithBrittle)
		{
			yield return RemoveBrittle(card);
		}
	}

	private IEnumerator RemoveBrittle(PlayableCard playableCard)
	{
		playableCard.Anim.PlayTransformAnimation();
		yield return new WaitForSeconds(0.25f);
		playableCard.RemoveAbilityFromThisCard(NegateBrittleMod);
	}

	public override bool RespondsToOtherCardPreDeath(CardSlot deathSlot, bool fromCombat, PlayableCard killer)
	{
		return deathSlot.IsPlayerSlot 
		    && deathSlot.Card 
		    && deathSlot.Card != Card 
		    && deathSlot.Card.TemporaryMods.Exists(mod => mod.singletonId == "grimoramod_puppeteer");
	}

	public override IEnumerator OnOtherCardPreDeath(CardSlot deathSlot, bool fromCombat, PlayableCard killer)
	{
		yield return AddBrittleBack(deathSlot.Card);
	}

	public override bool RespondsToDie(bool wasSacrifice, PlayableCard killer) => true;

	public override IEnumerator OnDie(bool wasSacrifice, PlayableCard killer)
	{
		List<PlayableCard> cardsWithThatNoLongerHaveBrittle = BoardManager.Instance.GetSlots(!Card.OpponentCard)
		 .Where(slot => slot.Card && slot.Card.Info.Mods.Exists(mod => mod.singletonId == "grimoramod_puppeteer"))
		 .Select(slot => slot.Card)
		 .ToList();

		foreach (var card in cardsWithThatNoLongerHaveBrittle)
		{
			yield return AddBrittleBack(card);
		}
	}

	private IEnumerator AddBrittleBack(PlayableCard playableCard)
	{
		playableCard.Anim.PlayTransformAnimation();
		yield return new WaitForSeconds(0.25f);
		CardInfo cardInfoClone = playableCard.Info.Clone() as CardInfo;
		cardInfoClone.Mods.Remove(NegateBrittleMod);
		playableCard.SetInfo(cardInfoClone);
	}
}

public partial class GrimoraPlugin
{
	public void Add_Ability_Puppeteer()
	{
		const string rulebookDescription =
			"Cards on the owner's side of the field are unaffected by Brittle.";

		ApiUtils.CreateAbility<Puppeteer>(rulebookDescription);
	}
}
