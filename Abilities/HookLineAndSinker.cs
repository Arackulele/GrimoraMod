using System.Collections;
using APIPlugin;
using DiskCardGame;
using UnityEngine;

namespace GrimoraMod;

public class HookLineAndSinker : AbilityBehaviour
{
	public static Ability ability;

	public override Ability Ability => ability;

	public override bool RespondsToDie(bool wasSacrifice, PlayableCard killer)
	{
		return Card.Slot.opposingSlot.Card is not null
		       && !Card.Slot.opposingSlot.CardIsNotNullAndHasSpecialAbility(GrimoraGiant.SpecialTriggeredAbility);
	}

	public override IEnumerator OnDie(bool wasSacrifice, PlayableCard killer)
	{
		AudioController.Instance.PlaySound3D(
			"angler_use_hook",
			MixerGroup.TableObjectsSFX,
			killer.transform.position,
			1f,
			0.1f
		);
		yield return new WaitForSeconds(0.51f);

		CardSlot opposingSlot = Card.Slot.opposingSlot;
		PlayableCard targetCard = opposingSlot.Card;
		targetCard.SetIsOpponentCard(!opposingSlot.IsPlayerSlot);
		yield return BoardManager.Instance.AssignCardToSlot(targetCard, Card.Slot, 0.33f);
		if (targetCard.FaceDown)
		{
			targetCard.SetFaceDown(false);
			targetCard.UpdateFaceUpOnBoardEffects();
		}
		yield return new WaitForSeconds(0.66f);
	}

	public static NewAbility Create()
	{
		const string rulebookDescription =
			"When [creature] perishes, the creature in the opposing slot is dragged onto the owner's side of the board.";

		return ApiUtils.CreateAbility<HookLineAndSinker>(rulebookDescription);
	}
}
