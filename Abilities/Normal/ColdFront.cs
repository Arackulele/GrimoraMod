using System.Collections;
using DiskCardGame;
using InscryptionAPI.Card;
using UnityEngine;

namespace GrimoraMod;

public class ColdFront : AbilityBehaviour
{
	public static Ability ability;

	public override Ability Ability => ability;

	public override bool RespondsToDie(bool wasSacrifice, PlayableCard killer)
	{
		return true;
	}

	public override IEnumerator OnDie(bool wasSacrifice, PlayableCard killer)
	{
		PlayableCard opposingSlotCard = Card.Slot.opposingSlot.Card;
		if (opposingSlotCard)
		{
			var frozenAway = GrimoraModKayceeBossSequencer.CreateModForFreeze(opposingSlotCard);
			opposingSlotCard.RemoveAbilityFromThisCard(frozenAway);
			opposingSlotCard.Anim.PlayTransformAnimation();
			yield return new WaitForSeconds(0.25f);
		}
	}
}

public partial class GrimoraPlugin
{
	public void Add_Abiltiy_ColdFront()
	{
		const string rulebookDescription = "When [creature] perishes, the card opposing it is Frozen Away.";

		ApiUtils.CreateAbility<ColdFront>(rulebookDescription);
	}
}
