using DiskCardGame;
using Sirenix.Utilities;
using UnityEngine;

namespace GrimoraMod;

public static class CardRelatedExtension
{

	private static readonly int Hover = Animator.StringToHash("hover");
	private static readonly int Hovering = Animator.StringToHash("hovering");
	
	public static string GetNameAndSlot(this PlayableCard playableCard)
	{
		string printedNameAndSlot = $"[{playableCard.Info.displayedName}]";
		if (playableCard.Slot is not null)
		{
			printedNameAndSlot += $" Slot [{playableCard.Slot.Index}]";
		}

		return printedNameAndSlot;
	}
	
	public static bool CardHasAbility(this CardSlot cardSlot, Ability ability)
	{
		return cardSlot.Card is not null && cardSlot.Card.HasAbility(ability);
	}
	
	public static bool CardHasSpecialAbility(this CardSlot cardSlot, SpecialTriggeredAbility ability)
	{
		return cardSlot.Card is not null && cardSlot.Card.Info.SpecialAbilities.Contains(ability);
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

	public static void UpdateHoveringForCard(this GravestoneCardAnimationController controller, bool hovering = false)
	{
		bool isHovering = hovering || controller.Anim.GetBool(Hovering);
		if (isHovering)
		{
			controller.Anim.ResetTrigger(Hover);
			controller.Anim.SetTrigger(Hover);
		}

		controller.Anim.SetBool(Hovering, isHovering);
	}

	public static void AddTempModGrimora(this PlayableCard playableCard, CardModificationInfo mod)
	{
		playableCard.Info.Mods.Add(mod);
		playableCard.Anim.PlayTransformAnimation();
		playableCard.RenderCard();
		playableCard.Info.Mods.Remove(mod);
		playableCard.AddTemporaryMod(mod);
	}
}
