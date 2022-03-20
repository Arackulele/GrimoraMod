using System.Collections;
using DiskCardGame;
using InscryptionAPI.Card;
using UnityEngine;

namespace GrimoraMod;

public class Puppeteer : AbilityBehaviour
{
	public static Ability ability;
	
	public override Ability Ability => ability;

	private static readonly CardModificationInfo NegateBrittleMod = new()
	{
		negateAbilities = new List<Ability> { Ability.Brittle }
	};

	public override bool RespondsToResolveOnBoard()
	{
		return true;
	}

	public override IEnumerator OnResolveOnBoard()
	{
		yield return RemoveBrittleFromCards();
	}

	public override bool RespondsToOtherCardResolve(PlayableCard otherCard)
	{
		return otherCard && otherCard.Slot.IsPlayerSlot && otherCard.HasAbility(Ability.Brittle);
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
		playableCard.RemoveAbilityFromThisCard(NegateBrittleMod);
		yield return new WaitForSeconds(0.25f);
		playableCard.Anim.PlayTransformAnimation();
	}

	public static AbilityManager.FullAbility Create()
	{
		const string rulebookDescription =
			"Cards on the owner's side of the field are unaffected by Brittle.";

		return ApiUtils.CreateAbility<Puppeteer>(rulebookDescription);
	}
}
