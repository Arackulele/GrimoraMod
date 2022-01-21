using DiskCardGame;
using HarmonyLib;
using static GrimoraMod.GrimoraPlugin;

namespace GrimoraMod;

[HarmonyPatch(typeof(GrimoraSaveData))]
public class GrimoraSaveDataPatches
{
	private static readonly List<CardInfo> DefaultCardInfos = new()
	{
		CardLoader.GetCardByName(NameBonepile),
		CardLoader.GetCardByName(NameGraveDigger),
		CardLoader.GetCardByName(NameGraveDigger),
		CardLoader.GetCardByName(NameFranknstein),
		CardLoader.GetCardByName(NameZombie)
	};

	[HarmonyPrefix, HarmonyPatch(nameof(GrimoraSaveData.Initialize))]
	public static bool PrefixChangeSetupOfGrimoraSaveData(ref GrimoraSaveData __instance)
	{
		__instance.gridX = 0;
		__instance.gridY = 7;
		__instance.removedPieces = new List<int>();
		__instance.deck = new DeckInfo();
		__instance.deck.Cards.Clear();
		foreach (var cardInfo in DefaultCardInfos)
		{
			__instance.deck.AddCard(cardInfo);
		}

		return false;
	}
}
