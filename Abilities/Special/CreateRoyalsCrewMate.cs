using System.Collections;
using DiskCardGame;
using InscryptionAPI.Card;
using InscryptionAPI.Helpers.Extensions;
using Sirenix.Utilities;
using UnityEngine;
using static GrimoraMod.GrimoraPlugin;

namespace GrimoraMod;

public class CreateRoyalsCrewMate : SpecialCardBehaviour
{
	public static SpecialTriggeredAbilityManager.FullSpecialTriggeredAbility FullSpecial;

	private int _timeToSpawnCounter = 0;

	private CardInfo _swashBuckler;

	private void Start()
	{
		_swashBuckler = NamePirateSwashbuckler.GetCardInfo();
	}

	private CardSlot GetCardSlotForSwashbuckler()
	{
		CardSlot slotToSpawnIn = null;
		var playerOpenSlots = BoardManager.Instance.GetPlayerOpenSlots();

		if (!base.PlayableCard.Slot.IsOpponentSlot()) BoardManager.Instance.GetOpponentOpenSlots();

		if (playerOpenSlots.Any())
		{
			slotToSpawnIn = playerOpenSlots.FirstOrDefault(slot => slot.opposingSlot.Card.SafeIsUnityNull() || slot.opposingSlot.Card.Attack == 0);
			Log.LogInfo($"[Swashbuckler] Slot to spawn in [{slotToSpawnIn}]");
			if (slotToSpawnIn.SafeIsUnityNull())
			{
				slotToSpawnIn = playerOpenSlots.GetRandomItem();
				Log.LogInfo($"[Swashbuckler] -> First choice is null, now spawning in [{slotToSpawnIn}]");
			}
		}

		return slotToSpawnIn;
	}

	public override bool RespondsToResolveOnBoard() => true;

	public override IEnumerator OnResolveOnBoard()
	{
		var slotToSpawnIn = GetCardSlotForSwashbuckler();
		if (slotToSpawnIn)
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
		if (_timeToSpawnCounter >= 2 && slotToSpawnIn)
		{
			_timeToSpawnCounter = 0;
			yield return SpawnSwashbuckler(slotToSpawnIn);
		}
	}

	private IEnumerator SpawnSwashbuckler(CardSlot playerOpenSlot)
	{
		ViewManager.Instance.SwitchToView(View.Board, lockAfter: true);
		yield return TextDisplayer.Instance.ShowThenClear($"PREPARE TO BE BOARDED!", 1.25f);
		yield return new WaitForSeconds(0.2f);
		yield return playerOpenSlot.CreateCardInSlot(_swashBuckler);

		ViewManager.Instance.SetViewUnlocked();
	}
}

public partial class GrimoraPlugin
{
	public static void Add_Ability_CreateRoyalsCrewMate()
	{
		ApiUtils.CreateSpecialAbility<CreateRoyalsCrewMate>("!GRIMORA_ROYALS_SHIP");
	}
}
