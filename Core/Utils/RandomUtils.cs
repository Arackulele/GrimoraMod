using DiskCardGame;
using static GrimoraMod.GrimoraPlugin;

namespace GrimoraMod;

public static class RandomUtils
{
	private const int MaxChoices = 3;

	public static List<CardChoice> GenerateRandomChoicesOfCategory(
		IEnumerable<CardInfo> cards
	)
	{
		List<CardChoice> cardChoices = new List<CardChoice>();

		var randomizedChoices = new List<CardInfo>(cards)
			.FindAll(
				info => info.name.StartsWith($"{GUID}_")
								&& !info.metaCategories.Contains(CardMetaCategory.Rare)
								&& info.metaCategories.Contains(CardMetaCategory.ChoiceNode)
														|| info.metaCategories.Contains(GrimoraChoiceNode)
								&& !info.metaCategories.Contains(CardMetaCategory.Rare)
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


	public static List<CardChoice> GenerateRandomChoicesForRare(
	IEnumerable<CardInfo> cards
)
	{
		List<CardChoice> cardChoices = new List<CardChoice>();

		var randomizedChoices = new List<CardInfo>(cards)
			.FindAll(
				info => info.name.StartsWith($"{GUID}_")
								&& info.metaCategories.Contains(CardMetaCategory.Rare)
								|| info.metaCategories.Contains(GrimoraChoiceNode)
								&& info.metaCategories.Contains(CardMetaCategory.Rare)
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
}
