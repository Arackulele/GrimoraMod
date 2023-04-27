using DiskCardGame;
using InscryptionAPI.Helpers.Extensions;

namespace GrimoraMod.Extensions;

public static class CardSlotExt
{
	public static List<CardSlot> OpenSlots(this List<CardSlot> cards)
	{
		List<CardSlot> openSlots = new List<CardSlot>();
		foreach (var cardSlot in cards)
		{
			if (cardSlot.Card is null)
			{
				openSlots.Add(cardSlot);
			}
		}

		return openSlots;
	}
	
	
	public static List<PlayableCard> GetCards(this List<CardSlot> slot)
	{
		List<PlayableCard> cards = new List<PlayableCard>();
		foreach (var cardSlot in slot)
		{
			if (cardSlot.Card != null)
			{
				cards.Add(cardSlot.Card);
			}
		}

		return cards;
	}
	
	
	public static List<PlayableCard> GetCards(this List<CardSlot> slot, Func<PlayableCard, bool> func)
	{
		List<PlayableCard> cards = new List<PlayableCard>();
		foreach (var cardSlot in slot)
		{
			if (cardSlot.Card != null&& func(cardSlot.Card)  )
			{
				cards.Add(cardSlot.Card);
			}
		}

		return cards;
	}
}
