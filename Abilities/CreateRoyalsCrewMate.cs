using System.Collections;
using DiskCardGame;
using InscryptionAPI.Card;
using UnityEngine;
using static GrimoraMod.GrimoraPlugin;

namespace GrimoraMod;

public class CreateRoyalsCrewMate : SpecialCardBehaviour
{
	public static SpecialTriggeredAbilityManager.FullSpecialTriggeredAbility FullSpecial;

	private int _timeToSpawnCounter = 0;

	private readonly CardInfo _swashBuckler = NamePirateSwashbuckler.GetCardInfo();

	private CardSlot GetCardSlotForSwashbuckler()
	{
		CardSlot slotToSpawnIn = null;
		var playerOpenSlots = BoardManager.Instance.GetPlayerOpenSlots();
		if (playerOpenSlots.IsNotEmpty())
		{
			slotToSpawnIn =
				playerOpenSlots.FirstOrDefault(slot => slot.opposingSlot.Card.IsNull() || slot.opposingSlot.Card.Attack == 0);
			if (slotToSpawnIn.IsNull())
			{
				slotToSpawnIn = playerOpenSlots.GetRandomItem();
			}
		}

		return slotToSpawnIn;
	}

	public override bool RespondsToResolveOnBoard() => true;

	public override IEnumerator OnResolveOnBoard()
	{
		var slotToSpawnIn = GetCardSlotForSwashbuckler();
		if (slotToSpawnIn.IsNotNull())
		{
			yield return SpawnSwashbuckler(slotToSpawnIn);
		}
	}

	public override bool RespondsToUpkeep(bool playerUpkeep)
	{
		return !playerUpkeep;
	}

	public override IEnumerator OnUpkeep(bool playerUpkeep)
	{
		_timeToSpawnCounter++;
		var slotToSpawnIn = GetCardSlotForSwashbuckler();
		if (_timeToSpawnCounter >= 2 && slotToSpawnIn.IsNotNull())
		{
			_timeToSpawnCounter = 0;
			yield return SpawnSwashbuckler(slotToSpawnIn);
		}
	}

	private IEnumerator SpawnSwashbuckler(CardSlot playerOpenSlot)
	{
		ViewManager.Instance.SwitchToView(View.Board, lockAfter: true);
		yield return TextDisplayer.Instance.ShowThenClear(
			$"PREPARE TO BE BOARDED!",
			1.25f
		);
		yield return new WaitForSeconds(0.2f);

		yield return BoardManager.Instance.CreateCardInSlot(
			_swashBuckler,
			playerOpenSlot
		);

		ViewManager.Instance.SetViewUnlocked();
	}

	public static SpecialTriggeredAbilityManager.FullSpecialTriggeredAbility Create()
	{
		FullSpecial = SpecialTriggeredAbilityManager.Add(GUID, "!GRIMORA_ROYALS_SHIP", typeof(CreateRoyalsCrewMate));
		return FullSpecial;
	}
}
