using DiskCardGame;
using HarmonyLib;

namespace GrimoraMod;

[HarmonyPatch(typeof(GrimoraCardChoiceGenerator))]
public class PlaceAllActOneChoicesToGrimora
{
	[HarmonyPrefix, HarmonyPatch(nameof(GrimoraCardChoiceGenerator.GenerateChoices))]
	public static bool Prefix(ref List<CardChoice> __result, ref int randomSeed)
	{
		var cardsToAdd = new List<CardChoice>();

		var randomizedChoices = CardLoader
			.AllData
			.FindAll(info => info.metaCategories.Contains(CardMetaCategory.ChoiceNode)
			                 && info.temple == CardTemple.Nature
			)
			.Select(card => new CardChoice { CardInfo = card })
			.ToArray()
			.Randomize()
			.ToList();

		GrimoraPlugin.Log.LogDebug($"[GenerateChoices] random cards are " +
		                           $"{string.Join(",", randomizedChoices.Select(cc => cc.info.name))}");

		while (cardsToAdd.Count < 3)
		{
			cardsToAdd.Add(
				randomizedChoices[SeededRandom.Range(0, randomizedChoices.Count, randomSeed++)]
			);
		}

		__result = cardsToAdd;
		return false;
	}
}
