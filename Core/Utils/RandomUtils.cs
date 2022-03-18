using DiskCardGame;
using static GrimoraMod.GrimoraPlugin;

namespace GrimoraMod;

public static class RandomUtils
{
	private const int MaxChoices = 3;

	public static List<CardChoice> GenerateRandomChoicesOfCategory(
		IEnumerable<CardInfo> cards,
		CardMetaCategory category = CardMetaCategory.NUM_CATEGORIES
	)
	{
		List<CardChoice> cardChoices = new List<CardChoice>();

		var randomizedChoices = new List<CardInfo>(cards)
			.FindAll(
				info => info.name.StartsWith($"{GUID}_")
				        && (category == CardMetaCategory.NUM_CATEGORIES || info.metaCategories.Contains(category))
			)
			.Select(card => new CardChoice { CardInfo = card })
			.ToList();

		while (cardChoices.Count < MaxChoices)
		{
			var choice = randomizedChoices.GetRandomItem();
			if (cardChoices.Exists(_ => _.info.name.Equals(choice.info.name)))
			{
				randomizedChoices.Remove(choice);
				Log.LogDebug($"[GenerateChoices] Removing [{choice.info.name}] as it already exists");
			}
			else
			{
				cardChoices.Add(choice);
				Log.LogDebug($"[GenerateChoices] Adding random card choice [{choice.info.name}]");
			}
		}

		return cardChoices;
	}

	public static int GenerateRandomSeed()
	{
		return SaveManager.SaveFile.GetCurrentRandomSeed() + UnityEngine.Random.RandomRangeInt(1, 666);
	}
}
