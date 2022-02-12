using System.Collections;
using APIPlugin;
using DiskCardGame;
using static GrimoraMod.GrimoraPlugin;

namespace GrimoraMod;

public class GrimoraGiant : SpecialCardBehaviour
{
	public static readonly NewSpecialAbility NewSpecialAbility = Create();

	public static NewSpecialAbility Create()
	{
		var sId = SpecialAbilityIdentifier.GetID(GUID, "!GRIMORA_GIANT");

		return new NewSpecialAbility(typeof(GrimoraGiant), sId);
	}

	public override bool RespondsToResolveOnBoard()
	{
		return true;
	}

	public override IEnumerator OnResolveOnBoard()
	{
		BoardManager.Instance.OpponentSlotsCopy[base.PlayableCard.Slot.Index - 1].Card = base.PlayableCard;
		BoardManager.Instance.OpponentSlotsCopy[base.PlayableCard.Slot.Index].Card = base.PlayableCard;
		yield break;
	}

	public override bool RespondsToDie(bool wasSacrifice, PlayableCard killer)
	{
		return true;
	}

	public override IEnumerator OnDie(bool wasSacrifice, PlayableCard killer)
	{
		BoardManager.Instance.OpponentSlotsCopy[base.PlayableCard.Slot.Index - 1].Card = null;
		BoardManager.Instance.OpponentSlotsCopy[base.PlayableCard.Slot.Index].Card = null;
		yield break;
	}
}
