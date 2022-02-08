using DiskCardGame;
using static GrimoraMod.GrimoraPlugin;

namespace GrimoraMod;

public static class RandomUtils
{
	private const int MaxChoices = 3;

	public static List<CardChoice> GenerateRandomChoicesOfCategory(
		IEnumerable<CardInfo> cards,
		int seed,
		CardMetaCategory category = CardMetaCategory.NUM_CATEGORIES
	)
	{
		List<CardChoice> cardChoices = new List<CardChoice>();

		var randomizedChoices = new List<CardInfo>(cards)
			.FindAll(info => info.name.StartsWith("GrimoraMod_") && (category == CardMetaCategory.NUM_CATEGORIES || info.metaCategories.Contains(category)))
			.Select(card => new CardChoice { CardInfo = card })
			.ToArray()
			.Randomize()
			.ToList();

		while (cardChoices.Count < MaxChoices)
		{
			var choice = randomizedChoices[SeededRandom.Range(0, randomizedChoices.Count, seed++)];
			if (cardChoices.Contains(choice))
			{
				randomizedChoices.Remove(choice);
			}
			else
			{
				cardChoices.Add(choice);
				Log.LogDebug($"[GenerateChoices] Adding random card choice [{choice.info.name}] to opening hand");
			}
		}

		Log.LogDebug($"[GrimoraRareChoiceGenerator] Selected random cards are " +
		             $"{string.Join(",", cardChoices.Select(cc => cc.info.name))}");

		return cardChoices;
	}
}
