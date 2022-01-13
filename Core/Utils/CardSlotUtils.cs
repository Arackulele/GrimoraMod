using System.Collections.Generic;
using DiskCardGame;

namespace GrimoraMod
{
	public class CardSlotUtils
	{
		public static List<CardSlot> GetPlayerSlotsWithCards()
		{
			return BoardManager.Instance.PlayerSlotsCopy.FindAll(slot => slot.Card != null);
		}
	}
}