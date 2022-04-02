using System.Collections;
using DiskCardGame;
using InscryptionAPI.Card;
using static GrimoraMod.GrimoraPlugin;

namespace GrimoraMod;

public class GrimoraGiant : SpecialCardBehaviour
{
	public static SpecialTriggeredAbilityManager.FullSpecialTriggeredAbility FullSpecial;

	public override bool RespondsToResolveOnBoard() => true;

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

	public override bool RespondsToDie(bool wasSacrifice, PlayableCard killer) => true;

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

public partial class GrimoraPlugin
{
	public static void Add_Ability_GrimoraGiant()
	{
		ApiUtils.CreateSpecialAbility<GrimoraGiant>("!GRIMORA_GIANT");
	}
}
