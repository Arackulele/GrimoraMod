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
}
