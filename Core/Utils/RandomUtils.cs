using DiskCardGame;
using static GrimoraMod.GrimoraPlugin;

namespace GrimoraMod;

public static class RandomUtils
{
	private static int MAX_CHOICES = 3;

	public static List<CardChoice> GenerateRandomChoicesOfCategory(
		int seed,
		CardMetaCategory category = CardMetaCategory.ChoiceNode
	)
	{
		List<CardChoice> cardChoices = new List<CardChoice>();

		var randomizedChoices = CardLoader.allData
			.FindAll(info => info.name.StartsWith("ara_") && info.metaCategories.Contains(category))
			.Select(card => new CardChoice { CardInfo = card })
			.ToArray()
			.Randomize()
			.ToList();

		while (cardChoices.Count < MAX_CHOICES)
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
