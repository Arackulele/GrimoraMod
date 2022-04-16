using System.Collections;
using DiskCardGame;
using InscryptionAPI.Card;
using InscryptionAPI.Helpers.Extensions;
using UnityEngine;

namespace GrimoraMod;

public class FuneralFacade : SpecialCardBehaviour
{
	public static SpecialTriggeredAbilityManager.FullSpecialTriggeredAbility FullSpecial;

	public override bool RespondsToOtherCardDie(
		PlayableCard card,
		CardSlot deathSlot,
		bool fromCombat,
		PlayableCard killer
	)
	{
		return card && killer == Card;
	}

	public override IEnumerator OnOtherCardDie(
		PlayableCard card,
		CardSlot deathSlot,
		bool fromCombat,
		PlayableCard killer
	)
	{
		if (card.InfoName() != GrimoraPlugin.NameTamperedCoffin)
		{
			Card.Anim.LightNegationEffect();
			yield return deathSlot.CreateCardInSlot(GrimoraPlugin.NameTamperedCoffin.GetCardInfo());
			yield return new WaitForSeconds(0.25f);
		}
	}
}

public partial class GrimoraPlugin
{
	public static void Add_Ability_FuneralFacade()
	{
		ApiUtils.CreateSpecialAbility<FuneralFacade>();
	}
}
