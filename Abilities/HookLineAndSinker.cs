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
		return killer is not null;
	}

	public override IEnumerator OnDie(bool wasSacrifice, PlayableCard killer)
	{
		ViewManager.Instance.SwitchToView(View.Default);
		yield return new WaitForSeconds(0.2f);
		AudioController.Instance.PlaySound3D(
			"angler_use_hook",
			MixerGroup.TableObjectsSFX,
			killer.transform.position
		);
		// LeshyAnimationController.Instance.RightArm.SetTrigger("angler_pullhook");
		yield return new WaitForSeconds(0.75f);

		if (killer.Slot.opposingSlot.Card is not null)
		{
			yield return TurnManager.Instance.Opponent.ReturnCardToQueue(killer.Slot.opposingSlot.Card, 0.25f);
		}

		if (killer.Status != null)
		{
			killer.Status.anglerHooked = true;
		}

		killer.SetIsOpponentCard();
		killer.transform.eulerAngles += new Vector3(0f, 0f, -180f);
		CardSlot cardSlot = killer.Slot;
		yield return BoardManager.Instance.AssignCardToSlot(cardSlot.Card, cardSlot.opposingSlot, 0.25f);
		yield return new WaitForSeconds(0.25f);
		yield return new WaitForSeconds(0.4f);
		// LeshyAnimationController.Instance.RightArm.ResetPosition(0.35f);
	}

	public static NewAbility Create()
	{
		const string rulebookDescription =
			"When [creature] perishes, the creature in the opposing slot is dragged onto your side of the board.";

		return ApiUtils.CreateAbility<HookLineAndSinker>(rulebookDescription);
	}
}
