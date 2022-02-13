using System.Collections;
using APIPlugin;
using DiskCardGame;
using static GrimoraMod.GrimoraPlugin;

namespace GrimoraMod;

public class Erratic : Strafe
{
	public static Ability ability;
	public override Ability Ability => ability;

	private readonly RandomEx _rng = new();

	public override IEnumerator DoStrafe(CardSlot toLeft, CardSlot toRight)
	{
		bool toLeftIsValid = toLeft is not null && toLeft.Card is null;
		bool toRightIsValid = toRight is not null && toRight.Card is null;
		if (!toLeftIsValid)
		{
			movingLeft = false;
		}
		else if (!toRightIsValid)
		{
			movingLeft = true;
		}
		else
		{
			// means that both adj-slots are valid for moving to
			movingLeft = _rng.NextBoolean();
		}

		CardSlot destination = movingLeft ? toLeft : toRight;
		yield return StartCoroutine(MoveToSlot(destination, movingLeft ? toLeftIsValid : toRightIsValid));
		if (destination != null)
		{
			Log.LogDebug($"[Erratic] Moving from slot [{base.Card.Slot.Index}] to slot [{destination.Index}]");
			yield return PreSuccessfulTriggerSequence();
			yield return LearnAbility();
		}
	}


	public static NewAbility Create()
	{
		const string rulebookDescription = "At the end of the owner's turn, [creature] will move in a random direction.";

		return ApiUtils.CreateAbility<Erratic>(rulebookDescription);
	}
}
