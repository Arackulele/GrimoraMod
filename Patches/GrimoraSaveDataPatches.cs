using DiskCardGame;
using HarmonyLib;
using InscryptionAPI.Card;
using static GrimoraMod.GrimoraPlugin;

namespace GrimoraMod;

[HarmonyPatch(typeof(GrimoraSaveData))]
public class GrimoraSaveDataPatches
{
	[HarmonyPrefix, HarmonyPatch(nameof(GrimoraSaveData.Initialize))]
	public static bool PrefixChangeSetupOfGrimoraSaveData(ref GrimoraSaveData __instance)
	{
		__instance.gridX = 0;
		__instance.gridY = 7;
		__instance.removedPieces = new List<int>();
		__instance.deck = new DeckInfo();
		__instance.deck.Cards.Clear();

		if (CardManager.AllCardsCopy.Any(info => info.name.StartsWith($"{GUID}_")))
		{
			Log.LogDebug($"[GrimoraSaveData.Initialize] All data");
			List<CardInfo> defaultCardInfos = new()
			{
				NameBonepile.GetCardInfo(),
				NameGravedigger.GetCardInfo(),
				NameGravedigger.GetCardInfo(),
				NameFranknstein.GetCardInfo(),
				NameZombie.GetCardInfo()
			};

			foreach (var cardInfo in defaultCardInfos)
			{
				__instance.deck.AddCard(cardInfo);
			}
		}
		else
		{
			Log.LogDebug($"[GrimoraSaveData.Initialize] All data.IsNull()");
			__instance.deck.AddCard("Gravedigger".GetCardInfo());
			__instance.deck.AddCard("Gravedigger".GetCardInfo());
			__instance.deck.AddCard("Gravedigger".GetCardInfo());

			__instance.deck.AddCard("FrankNStein".GetCardInfo());
			__instance.deck.AddCard("FrankNStein".GetCardInfo());
		}

		return false;
	}
}
