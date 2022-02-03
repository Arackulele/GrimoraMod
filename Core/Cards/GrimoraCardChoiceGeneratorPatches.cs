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

		var randomizedChoices = CardLoader.allData
			.FindAll(info => info.name.StartsWith("ara_") && info.metaCategories.Contains(CardMetaCategory.ChoiceNode))
			.Select(card => new CardChoice { CardInfo = card })
			.ToArray()
			.Randomize()
			.ToList();

		GrimoraPlugin.Log.LogDebug($"[GenerateChoices] random cards are " +
		                           $"{string.Join(",", randomizedChoices.Select(cc => cc.info.name))}");

		while (cardsToAdd.Count < 3)
		{
			var choice = randomizedChoices[SeededRandom.Range(0, randomizedChoices.Count, randomSeed++)];
			while (cardsToAdd.Contains(choice))
			{
				choice = randomizedChoices[SeededRandom.Range(0, randomizedChoices.Count, randomSeed++)];
			}

			cardsToAdd.Add(choice);
		}

		__result = cardsToAdd;
		return false;
	}
}
