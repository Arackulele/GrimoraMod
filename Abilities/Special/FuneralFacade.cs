using System.Collections;
using DiskCardGame;
using InscryptionAPI.Card;
using InscryptionAPI.Helpers.Extensions;
using UnityEngine;

namespace GrimoraMod;

public class FuneralFacade : SpecialCardBehaviour
{
	public static SpecialTriggeredAbilityManager.FullSpecialTriggeredAbility FullSpecial;

	private CardInfo _tamperedCoffin;

	private void Start()
	{
		_tamperedCoffin = GrimoraPlugin.NameTamperedCoffin.GetCardInfo();
	}

	public override bool RespondsToOtherCardDie(
		PlayableCard card,
		CardSlot deathSlot,
		bool fromCombat,
		PlayableCard killer
	)
	{
		return card && killer == Card && card.InfoName() != _tamperedCoffin.name;
	}

	public override IEnumerator OnOtherCardDie(
		PlayableCard card,
		CardSlot deathSlot,
		bool fromCombat,
		PlayableCard killer
	)
	{
		Card.Anim.LightNegationEffect();
		yield return deathSlot.CreateCardInSlot(_tamperedCoffin);
		yield return new WaitForSeconds(0.25f);
	}
}

public partial class GrimoraPlugin
{
	public static void Add_Ability_FuneralFacade()
	{
		ApiUtils.CreateSpecialAbility<FuneralFacade>();
	}
}
