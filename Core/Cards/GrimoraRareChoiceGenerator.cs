using DiskCardGame;

namespace GrimoraMod;

public class GrimoraRareChoiceGenerator : CardChoiceGenerator
{
	private int NUM_CHOICES = 3;

	public override List<CardChoice> GenerateChoices(CardChoicesNodeData data, int randomSeed)
	{
		List<CardChoice> list = new List<CardChoice>();

		var randomizedChoices = CardLoader.allData
			.FindAll(info => info.name.StartsWith("ara_") && info.metaCategories.Contains(CardMetaCategory.Rare))
			.Select(card => new CardChoice { CardInfo = card })
			.ToArray()
			.Randomize()
			.ToList();

		while (list.Count < 3)
		{
			var choice = randomizedChoices[SeededRandom.Range(0, randomizedChoices.Count, randomSeed++)];
			while (list.Contains(choice))
			{
				choice = randomizedChoices[SeededRandom.Range(0, randomizedChoices.Count, randomSeed++)];
			}

			list.Add(choice);
		}

		GrimoraPlugin.Log.LogDebug($"[GrimoraRareChoiceGenerator] Selected random cards are " +
		                           $"{string.Join(",", list.Select(cc => cc.info.name))}");

		return list;
	}
}
