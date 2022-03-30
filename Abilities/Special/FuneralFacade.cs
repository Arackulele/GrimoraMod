using System.Collections;
using DiskCardGame;
using InscryptionAPI.Card;
using UnityEngine;

namespace GrimoraMod;

public class FuneralFacade : SpecialCardBehaviour
{
	public static SpecialTriggeredAbilityManager.FullSpecialTriggeredAbility FullSpecial;

	public override bool RespondsToOtherCardDie(PlayableCard card,
	                                            CardSlot deathSlot,
	                                            bool fromCombat,
	                                            PlayableCard killer)
	{
		return card && killer == Card;
	}

	public override IEnumerator OnOtherCardDie(PlayableCard card,
	                                           CardSlot deathSlot,
	                                           bool fromCombat,
	                                           PlayableCard killer)
	{
		if (card.InfoName() != GrimoraPlugin.NameTamperedCoffin)
		{
			Card.Anim.LightNegationEffect();
			yield return BoardManager.Instance.CreateCardInSlot(GrimoraPlugin.NameTamperedCoffin.GetCardInfo(), deathSlot);
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
