using System.Collections;
using DiskCardGame;
using InscryptionAPI.Card;
using static GrimoraMod.GrimoraPlugin;

namespace GrimoraMod;

public class GrimoraGiant : SpecialCardBehaviour
{
	public static SpecialTriggeredAbilityManager.FullSpecialTriggeredAbility FullAbility;

	public static SpecialTriggeredAbilityManager.FullSpecialTriggeredAbility Create()
	{
		FullAbility = SpecialTriggeredAbilityManager.Add(GUID, "!GRIMORA_GIANT", typeof(GrimoraGiant));
		return FullAbility;
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
			BoardManager.Instance.opponentSlots[PlayableCard.Slot.Index - i].Card = PlayableCard;
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
			BoardManager.Instance.opponentSlots[PlayableCard.Slot.Index - i].Card = null;
		}

		yield break;
	}
}
