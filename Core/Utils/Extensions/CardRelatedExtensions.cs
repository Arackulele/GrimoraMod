using System.Collections;
using DiskCardGame;
using Pixelplacement;
using Pixelplacement.TweenSystem;
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
		if (playableCard.Slot.IsNotNull())
		{
			printedNameAndSlot += $" Slot [{playableCard.Slot.Index}]";
		}

		return printedNameAndSlot;
	}

	public static bool CardIsNotNullAndHasAbility(this CardSlot cardSlot, Ability ability)
	{
		return cardSlot.Card.IsNotNull() && cardSlot.Card.HasAbility(ability);
	}

	public static bool CardDoesNotHaveAbility(this CardSlot cardSlot, Ability ability)
	{
		return cardSlot.Card.IsNotNull() && !cardSlot.CardIsNotNullAndHasAbility(ability);
	}


	public static bool CardIsNotNullAndHasSpecialAbility(this CardSlot cardSlot, SpecialTriggeredAbility ability)
	{
		return cardSlot.Card.IsNotNull() && cardSlot.Card.Info.SpecialAbilities.Contains(ability);
	}

	public static bool CardDoesNotHaveSpecialAbility(this CardSlot cardSlot, SpecialTriggeredAbility ability)
	{
		return cardSlot.Card is null || !cardSlot.CardIsNotNullAndHasSpecialAbility(ability);
	}

	public static bool CardInSlotIs(this CardSlot cardSlot, string cardName)
	{
		return cardSlot.Card.IsNotNull() && cardSlot.Card.InfoName().Equals(cardName);
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

	public static void RemoveAbilityFromThisCard(this PlayableCard playableCard, CardModificationInfo modInfo)
	{
		CardInfo cardInfoClone = playableCard.Info.Clone() as CardInfo;
		cardInfoClone.Mods.Add(modInfo);
		playableCard.SetInfo(cardInfoClone);
	}

	public static CardInfo DeepCopy(this CardInfo cardInfo)
	{
		CardInfo deepCopy = ScriptableObject.CreateInstance<CardInfo>();
		deepCopy.abilities = cardInfo.abilities;
		deepCopy.alternatePortrait = cardInfo.alternatePortrait;
		deepCopy.animatedPortrait = cardInfo.animatedPortrait;
		deepCopy.appearanceBehaviour = cardInfo.appearanceBehaviour;
		deepCopy.baseAttack = cardInfo.baseAttack;
		deepCopy.baseHealth = cardInfo.baseHealth;
		deepCopy.bonesCost = cardInfo.bonesCost;
		deepCopy.boon = cardInfo.boon;
		deepCopy.cardComplexity = cardInfo.cardComplexity;
		deepCopy.cost = cardInfo.cost;
		deepCopy.decals = cardInfo.decals;
		deepCopy.defaultEvolutionName = cardInfo.defaultEvolutionName;
		deepCopy.description = cardInfo.description;
		deepCopy.displayedName = cardInfo.displayedName;
		deepCopy.energyCost = cardInfo.energyCost;
		deepCopy.evolveParams = cardInfo.evolveParams;
		deepCopy.flipPortraitForStrafe = cardInfo.flipPortraitForStrafe;
		deepCopy.gemsCost = cardInfo.gemsCost;
		deepCopy.get_decals = cardInfo.get_decals;
		deepCopy.hideAttackAndHealth = cardInfo.hideAttackAndHealth;
		deepCopy.holoPortraitPrefab = cardInfo.holoPortraitPrefab;
		deepCopy.iceCubeParams = cardInfo.iceCubeParams;
		deepCopy.metaCategories = cardInfo.metaCategories;
		deepCopy.mods = cardInfo.mods;
		deepCopy.onePerDeck = cardInfo.onePerDeck;
		deepCopy.pixelPortrait = cardInfo.pixelPortrait;
		deepCopy.portraitTex = cardInfo.portraitTex;
		deepCopy.specialAbilities = cardInfo.specialAbilities;
		deepCopy.specialStatIcon = cardInfo.specialStatIcon;
		deepCopy.tailParams = cardInfo.tailParams;
		deepCopy.temple = cardInfo.temple;
		deepCopy.temporaryDecals = cardInfo.temporaryDecals;
		deepCopy.titleGraphic = cardInfo.titleGraphic;
		deepCopy.traits = cardInfo.traits;
		deepCopy.tribes = cardInfo.tribes;

		return deepCopy;
	}

	public static void AddTempModGrimora(this PlayableCard playableCard, CardModificationInfo mod)
	{
		playableCard.Info.Mods.Add(mod);
		playableCard.Anim.PlayTransformAnimation();
		playableCard.RenderCard();
		playableCard.Info.Mods.Remove(mod);
		playableCard.AddTemporaryMod(mod);
	}

	public static IEnumerator DieCustom(
		this PlayableCard playableCard,
		bool wasSacrifice,
		PlayableCard killer = null,
		bool playSound = true,
		float royalTableSwayValue = 0f
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

			if (!playableCard.HasAbility(Ability.QuadrupleBones) && slotBeforeDeath.IsPlayerSlot)
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
