using DiskCardGame;

namespace GrimoraMod;

public static class GrimoraSaveUtil
{
	public static void ClearDeck()
	{
		DeckInfo.Cards.Clear();
		DeckInfo.cardIds.Clear();
	}

	public static bool IsGrimora => SaveManager.SaveFile.IsGrimora || SaveDataRelatedPatches.IsGrimoraRun;

	public static bool IsNotGrimora => !SaveManager.SaveFile.IsGrimora || SaveDataRelatedPatches.IsNotGrimoraRun;

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
	
	public static void ModifyCard(CardInfo cardToModify, CardModificationInfo modInfo)
	{
		DeckInfo.ModifyCard(cardToModify, modInfo);
	}
}
