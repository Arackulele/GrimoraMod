using DiskCardGame;
using Sirenix.Utilities;

namespace GrimoraMod;

public static class CardRelatedExtension
{

	public static bool CardHasAbility(this CardSlot cardSlot, Ability ability)
	{
		return cardSlot.Card is not null && cardSlot.Card.HasAbility(ability);
	}
	
	public static bool CardHasSpecialAbility(this CardSlot cardSlot, SpecialTriggeredAbility ability)
	{
		return cardSlot.Card is not null && cardSlot.Card.Info.specialAbilities.Contains(ability);
	}

	public static bool CardInSlotIs(this CardSlot cardSlot, string cardName)
	{
		return cardSlot.Card is not null && cardSlot.Card.InfoName().Equals(cardName);
	}
	
	public static string InfoName(this Card card)
	{
		return card.Info.name;
	}

	public static T GetRandomItem<T>(this List<T> self)
	{
		return self.Randomize().ToList()[SeededRandom.Range(0, self.Count, RandomUtils.GenerateRandomSeed())];
	}

	public static bool IsNotEmpty<T>(this List<T> self)
	{
		return !self.IsNullOrEmpty();
	}
}
