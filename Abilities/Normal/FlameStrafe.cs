using System.Collections;
using DiskCardGame;
using InscryptionAPI.Card;
using static GrimoraMod.GrimoraPlugin;

namespace GrimoraMod;

public class FlameStrafe : Strafe
{
	public static Ability ability;

	public override Ability Ability => ability;

	public override IEnumerator PostSuccessfulMoveSequence(CardSlot cardSlot)
	{
		if (cardSlot.Card.IsNull())
		{
			yield return BoardManager.Instance.CreateCardInSlot(NameFlames.GetCardInfo(), cardSlot);
		}
	}

	public static AbilityManager.FullAbility Create()
	{
		const string rulebookDescription =
			"Whenever [creature] moves, it leaves a trail of Embers. "
			+ "The warmth of the Embers shall enlighten nearby cards.";

		return ApiUtils.CreateAbility<FlameStrafe>(rulebookDescription);
	}
}
