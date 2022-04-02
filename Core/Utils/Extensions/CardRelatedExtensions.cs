using System.Collections;
using DiskCardGame;
using InscryptionAPI.Card;
using Pixelplacement;
using Pixelplacement.TweenSystem;
using Sirenix.Utilities;
using UnityEngine;

namespace GrimoraMod;

public static class CardRelatedExtension
{
	private static readonly int Hover    = Animator.StringToHash("hover");
	private static readonly int Hovering = Animator.StringToHash("hovering");

	public static string GetNameAndSlot(this PlayableCard playableCard)
	{
		string printedNameAndSlot = $"[{playableCard.Info.displayedName}]";
		if (playableCard.Slot)
		{
			printedNameAndSlot += $" Slot [{playableCard.Slot.Index}]";
		}

		return printedNameAndSlot;
	}

	/// <summary>
	/// Create a basic CardBlueprint based off the CardInfo object.
	/// </summary>
	/// <param name="cardInfo">CardInfo to access</param>
	/// <returns>The same card info so a chain can continue</returns>
	public static EncounterBlueprintData.CardBlueprint CreateBlueprint(this CardInfo cardInfo)
	{
		return new EncounterBlueprintData.CardBlueprint
		{
			card = cardInfo
		};
	}

	/// <summary>
	/// Create a basic CardBlueprint based from the name of the card.
	/// </summary>
	/// <param name="cardName">Name of the card</param>
	/// <returns>The CardBlueprint containing the card.</returns>
	public static EncounterBlueprintData.CardBlueprint CreateCardBlueprint(this string cardName)
	{
		return CreateBlueprint(cardName.GetCardInfo());
	}

	public static CardInfo GetCardInfo(this string self)
	{
		return CardManager.AllCardsCopy.Single(info => info.name == self);
	}

	public static bool CardIsNotNullAndHasAbility(this CardSlot cardSlot, Ability ability)
	{
		return cardSlot.Card && cardSlot.Card.HasAbility(ability);
	}

	public static bool CardIsNotNullAndHasSpecialAbility(this CardSlot cardSlot, SpecialTriggeredAbility ability)
	{
		return cardSlot.Card && cardSlot.Card.Info.SpecialAbilities.Contains(ability);
	}

	public static bool CardInSlotIs(this CardSlot cardSlot, string cardName)
	{
		return cardSlot.Card && cardSlot.Card.InfoName().Equals(cardName);
	}

	public static string InfoName(this Card card)
	{
		return card.Info.name;
	}

	public static T GetRandomItem<T>(this List<T> self)
	{
		return self[UnityEngine.Random.Range(0, self.Count)];
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

	public static bool HasSpecialAbility(this PlayableCard playableCard, SpecialTriggeredAbility ability)
	{
		return playableCard.Info.SpecialAbilities.Contains(ability);
	}

	public static bool HasAnyAbilities(this PlayableCard playableCard, params Ability[] abilities)
	{
		return playableCard.Info.Abilities.Any(abilities.Contains);
	}

	public static bool HasBeenElectricChaired(this PlayableCard playableCard)
	{
		return playableCard.Info.Mods.Exists(mod => mod.singletonId == "GrimoraMod_ElectricChaired");
	}

	public static bool HasBeenElectricChaired(this CardInfo cardInfo)
	{
		return cardInfo.Mods.Exists(mod => mod.singletonId == "GrimoraMod_ElectricChaired");
	}

	public static void RemoveAbilityFromThisCard(
		this PlayableCard    playableCard,
		CardModificationInfo modInfo,
		Action               callback = null
	)
	{
		CardInfo cardInfoClone = playableCard.Info.Clone() as CardInfo;
		cardInfoClone.Mods.Add(modInfo);
		playableCard.SetInfo(cardInfoClone);
		callback?.Invoke();
	}

	public static IEnumerator DieCustom(
		this PlayableCard playableCard,
		bool              wasSacrifice,
		PlayableCard      killer              = null,
		bool              playSound           = true,
		float             royalTableSwayValue = 0f
	)
	{
		if (!playableCard.Dead)
		{
			playableCard.Dead = true;
			CardSlot slotBeforeDeath = playableCard.Slot;
			if (playableCard.TriggerHandler.RespondsToTrigger(Trigger.PreDeathAnimation, wasSacrifice))
			{
				yield return playableCard.TriggerHandler.OnTrigger(Trigger.PreDeathAnimation, wasSacrifice);
			}

			yield return GlobalTriggerHandler.Instance.TriggerCardsOnBoard(
				Trigger.OtherCardPreDeath,
				false,
				slotBeforeDeath,
				!wasSacrifice,
				killer
			);
			playableCard.Anim.SetShielded(false);
			yield return playableCard.Anim.ClearLatchAbility();
			if (royalTableSwayValue == 0f)
			{
				if (playableCard.HasAbility(Ability.PermaDeath))
				{
					playableCard.Anim.PlayPermaDeathAnimation(playSound && !wasSacrifice);
					yield return new WaitForSeconds(1.25f);
				}
				else if (playableCard.InOpponentQueue)
				{
					playableCard.Anim.PlayQueuedDeathAnimation(playSound && !wasSacrifice);
				}
				else
				{
					playableCard.Anim.PlayDeathAnimation(playSound && !wasSacrifice);
				}
			}

			if ((!playableCard.HasAbility(Ability.QuadrupleBones)
			  || !playableCard.HasAbility(Boneless.ability)) && slotBeforeDeath.IsPlayerSlot)
			{
				yield return ResourcesManager.Instance.AddBones(1, slotBeforeDeath);
			}

			if (playableCard.TriggerHandler.RespondsToTrigger(Trigger.Die, wasSacrifice, killer))
			{
				yield return playableCard.TriggerHandler.OnTrigger(Trigger.Die, wasSacrifice, killer);
			}

			yield return GlobalTriggerHandler.Instance.TriggerAll(
				Trigger.OtherCardDie,
				false,
				playableCard,
				slotBeforeDeath,
				!wasSacrifice,
				killer
			);
			playableCard.UnassignFromSlot();
			playableCard.StartCoroutine(playableCard.DestroyWhenStackIsClear());
			if (royalTableSwayValue != 0f)
			{
				GrimoraPlugin.Log.LogInfo($"[DieCustom] Waiting until tween is finished to play death animation");
				Vector3 positionCopy = playableCard.transform.localPosition;
				TweenBase slidingCard = Tween.LocalPosition(
					playableCard.transform,
					new Vector3(royalTableSwayValue, positionCopy.y, positionCopy.z),
					GrimoraModRoyalBossSequencer.DurationTableSway,
					0,
					Tween.EaseIn,
					completeCallback: () => playableCard.Anim.PlayDeathAnimation(playSound && !wasSacrifice)
				);
			}
		}
	}
}
