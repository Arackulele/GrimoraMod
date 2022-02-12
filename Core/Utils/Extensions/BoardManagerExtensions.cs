using DiskCardGame;

namespace GrimoraMod;

public static class BoardManagerExtensions
{

	public static List<CardSlot> GetQueueSlots(this BoardManager manager)
	{
		var queuedSlots = TurnManager.Instance.Opponent.QueuedSlots;
		return manager
			.GetSlots(false)
			.FindAll(x => x.Card == null && !queuedSlots.Contains(x)); 
	}

}
