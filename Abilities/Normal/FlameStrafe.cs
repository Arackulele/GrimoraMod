using System.Collections;
using DiskCardGame;
using static GrimoraMod.GrimoraPlugin;

namespace GrimoraMod;

public class FlameStrafe : Strafe
{
	public static Ability ability;

	public override Ability Ability => ability;

	public override IEnumerator PostSuccessfulMoveSequence(CardSlot oldSlot)
	{
		if (oldSlot.Card.IsNull())
		{
			yield return BoardManager.Instance.CreateCardInSlot(NameFlames.GetCardInfo(), oldSlot);
		}
	}
}

public partial class GrimoraPlugin
{
	public void Add_Ability_FlameStrafe()
	{
		const string rulebookDescription =
			"Whenever [creature] moves, it leaves a trail of Embers. The warmth of the Embers shall enlighten nearby cards.";

		ApiUtils.CreateAbility<FlameStrafe>(rulebookDescription);
	}
}
