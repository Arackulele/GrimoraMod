using DiskCardGame;

namespace GrimoraMod;

public static class BoardManagerExtensions
{
	private static readonly Predicate<CardSlot> FindOccupiedQueueSlots
		= slot => TurnManager.Instance.Opponent.queuedCards.Find(pCard => pCard.QueuedSlot == slot);

	public static List<CardSlot> GetQueueSlots(this BoardManager manager)
	{
		List<CardSlot> opponentSlotsCopy = manager.OpponentSlotsCopy;
		opponentSlotsCopy.RemoveAll(FindOccupiedQueueSlots);
		return opponentSlotsCopy;
	}

	public static List<PlayableCard> GetPlayerCards(
		this BoardManager manager, Predicate<PlayableCard> filterOnPredicate = null
	)
	{
		return manager
			.PlayerSlotsCopy
			.Where(slot => slot.Card != null && (filterOnPredicate is null || filterOnPredicate.Invoke(slot.Card)))
			.Select(slot => slot.Card)
			.ToList();
	}

	public static List<PlayableCard> GetOpponentCards(
		this BoardManager manager, Predicate<PlayableCard> filterOnPredicate = null
	)
	{
		return manager
			.OpponentSlotsCopy
			.Where(slot => slot.Card != null && (filterOnPredicate is null || filterOnPredicate.Invoke(slot.Card)))
			.Select(slot => slot.Card)
			.ToList();
	}
}
