using System.Collections;
using APIPlugin;
using DiskCardGame;
using static GrimoraMod.GrimoraPlugin;

namespace GrimoraMod;

public class GrimoraGiant : SpecialCardBehaviour
{
	public static SpecialTriggeredAbility SpecialTriggeredAbility;

	public static NewSpecialAbility Create()
	{
		var sId = SpecialAbilityIdentifier.GetID(GUID, "!GRIMORA_GIANT");

		var ability = new NewSpecialAbility(typeof(GrimoraGiant), sId);
		SpecialTriggeredAbility = ability.specialTriggeredAbility;
		return ability;
	}

	public override bool RespondsToResolveOnBoard()
	{
		return true;
	}

	public override IEnumerator OnResolveOnBoard()
	{
		int slotsToSet = 2;
		if (ConfigHelper.HasIncreaseSlotsMod && Card.InfoName().Equals(NameBonelord))
		{
			slotsToSet = 3;
		}

		for (int i = 0; i < slotsToSet; i++)
		{
			BoardManager.Instance.OpponentSlotsCopy[PlayableCard.Slot.Index - i].Card = PlayableCard;
		}

		yield break;
	}

	public override bool RespondsToDie(bool wasSacrifice, PlayableCard killer)
	{
		return true;
	}

	public override IEnumerator OnDie(bool wasSacrifice, PlayableCard killer)
	{
		int slotsToSet = 2;
		if (ConfigHelper.HasIncreaseSlotsMod && Card.InfoName().Equals(NameBonelord))
		{
			slotsToSet = 3;
		}

		for (int i = 0; i < slotsToSet; i++)
		{
			BoardManager.Instance.OpponentSlotsCopy[PlayableCard.Slot.Index - i].Card = null;
		}

		yield break;
	}
}
