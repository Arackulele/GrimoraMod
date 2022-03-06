using DiskCardGame;

namespace GrimoraMod;

public static class GrimoraSaveUtil
{
	public static void ClearDeck()
	{
		DeckList.ForEach(card => GrimoraSaveData.Data.deck.RemoveCard(card));
	}

	public static bool isGrimora => SaveManager.SaveFile.IsGrimora;

	public static bool isNotGrimora => !SaveManager.SaveFile.IsGrimora;

	public static DeckInfo DeckInfo => GrimoraSaveData.Data.deck;

	public static List<CardInfo> DeckList => DeckInfo.Cards;

	public static List<CardInfo> DeckListCopy => new(DeckList);

	public static void RemoveCard(CardInfo cardToRemove)
	{
		DeckInfo.RemoveCard(cardToRemove);
	}
	
	public static void AddCard(CardInfo cardToAdd)
	{
		DeckInfo.AddCard(cardToAdd);
	}
}
