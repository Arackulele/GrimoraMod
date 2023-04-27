using System.Collections;
using DiskCardGame;
using GrimoraMod.Extensions;
using InscryptionAPI.Card;
using InscryptionAPI.Helpers.Extensions;
using UnityEngine;

namespace GrimoraMod;

public class Puppeteer : AbilityBehaviour
{
	public const string ModSingletonId = "GrimoraMod_Puppeteer";

	private bool CardHasBeenPuppeteered(PlayableCard playableCard)
	{
		return playableCard.TemporaryMods.Exists(mod => mod.singletonId == ModSingletonId);
	}

	private static readonly CardModificationInfo NegateBrittleMod = new()
	{
		negateAbilities = new List<Ability> { Ability.Brittle },
		singletonId = ModSingletonId
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
		    && (otherCard.OpponentCard && Card.OpponentCard || otherCard.IsPlayerCard() && Card.IsPlayerCard())
		    && otherCard.HasAbility(Ability.Brittle);
	}

	public override IEnumerator OnOtherCardResolve(PlayableCard otherCard)
	{
		yield return RemoveBrittle(otherCard);
	}

	private IEnumerator RemoveBrittleFromCards()
	{
		List<PlayableCard> cardsWithBrittle = BoardManager.Instance
		 .GetSlots(Card.IsPlayerCard()).GetCards(pCard => pCard.HasAbility(Ability.Brittle));

		foreach (var card in cardsWithBrittle)
		{
			yield return RemoveBrittle(card);
		}
	}

	public override bool RespondsToDie(bool wasSacrifice, PlayableCard killer) => true;

	public override IEnumerator OnDie(bool wasSacrifice, PlayableCard killer)
	{
		List<PlayableCard> cardsThatHadBrittle = BoardManager.Instance
		 .GetSlots(Card.IsPlayerCard())
		 .GetCards(CardHasBeenPuppeteered);

		foreach (var card in cardsThatHadBrittle)
		{
			yield return AddBrittleBack(card);
		}
	}

	private IEnumerator AddBrittleBack(PlayableCard playableCard)
	{
		playableCard.Anim.PlayTransformAnimation();
		yield return new WaitForSeconds(0.25f);
		playableCard.RemoveTemporaryMod(NegateBrittleMod);
		playableCard.TriggerHandler.AddAbility(Ability.Brittle);
	}

	private IEnumerator RemoveBrittle(PlayableCard playableCard)
	{
		playableCard.Anim.PlayTransformAnimation();
		yield return new WaitForSeconds(0.25f);
		playableCard.AddTemporaryMod(NegateBrittleMod);
	}
}

public partial class GrimoraPlugin
{
	public void Add_Ability_Puppeteer()
	{
		const string rulebookDescription =
			"Cards on the owner's side of the field are unaffected by Brittle.";

		AbilityBuilder<Puppeteer>.Builder
		 .SetRulebookDescription(rulebookDescription)
		 .SetPixelIcon(AssetUtils.GetPrefab<Sprite>("puppet_pixel"))
		 .Build();
	}
}
