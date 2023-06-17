using System.Collections;
using System.Text;
using DiskCardGame;
using InscryptionAPI.Card;
using Pixelplacement;
using Pixelplacement.TweenSystem;
using Sirenix.Utilities;
using UnityEngine;

namespace GrimoraMod;

public static class CardRelatedExtension
{
	
	public static Animator GetCustomArm(this CardAnimationController controller)
	{
		return GrimoraSaveUtil.IsGrimoraModRun ? ((GraveControllerExt)controller).GetCustomArm() : null;
	}

	public static void PlaySpecificAttackAnimation(
		this CardAnimationController controller,
		string animToPlay,
		bool attackPlayer,
		CardSlot targetSlot,
		Action impactCallback
	)
	{
		((GraveControllerExt)controller).PlaySpecificAttackAnimation(animToPlay, attackPlayer, targetSlot, impactCallback);
	}
	
	public static string GetNameAndSlot(this PlayableCard playableCard)
	{
		StringBuilder printedNameAndSlot = new StringBuilder(playableCard.Info.DisplayedNameEnglish);
		if (playableCard.Slot)
		{
			printedNameAndSlot.Append($" Slot {playableCard.Slot.Index}");
		}

		return printedNameAndSlot.ToString();
	}

	public static bool IsGrimoraGiant(this PlayableCard playableCard)
	{
		return playableCard.HasSpecialAbility(GrimoraGiant.FullSpecial.Id)
		    || playableCard.TemporaryMods.Exists(mod => mod.specialAbilities.Contains(GrimoraGiant.FullSpecial.Id));
	}

	/// <summary>
	/// Create a basic CardBlueprint based from the name of the card.
	/// </summary>
	/// <param name="cardName">Name of the card</param>
	/// <returns>The CardBlueprint containing the card.</returns>
	public static EncounterBlueprintData.CardBlueprint CreateCardBlueprint(this string cardName)
	{
		return cardName.GetCardInfo().CreateBlueprint();
	}

	public static CardInfo GetCardInfo(this string self)
	{

		return CardManager.AllCardsCopy.Single(info => info.name == self);
	}

	public static string InfoName(this Card card)
	{
		return card.Info.name;
	}

	public static T GetRandomItem<T>(this List<T> self)
	{
		return self[UnityRandom.Range(0, self.Count)];
	}

	public static bool IsNotEmpty<T>(this List<T> self)
	{
		return !self.IsNullOrEmpty();
	}
	
	public static List<Ability> AllAbilities(this PlayableCard playableCard)
	{
		return AbilitiesUtil
		 .GetAbilitiesFromMods(playableCard.TemporaryMods)
		 .Concat(playableCard.Info.Abilities)
		 .ToList();
	}

	public static bool HasAnyAbilities(this PlayableCard playableCard, params Ability[] abilities)
	{
		return playableCard.Info.Abilities.Any(abilities.Contains);
	}

	public static bool HasBeenElectricChaired(this CardInfo cardInfo)
	{
		return cardInfo.Mods.Exists(mod => mod.singletonId == ElectricChairSequencer.ModSingletonId);
	}

	public static bool HasBeenGraveDug(this CardInfo cardInfo)
	{
		return cardInfo.Mods.Exists(mod => mod.singletonId == BoneyardBurialSequencer.ModSingletonId);
	}

	public static bool HasBeenBonelorded(this CardInfo cardInfo)
	{
		return cardInfo.Mods.Exists(mod => mod.singletonId == GrimoraCardRemoveSequencer.ModSingletonId);
	}

	public static void RemoveAbilityFromThisCard(
		this PlayableCard playableCard,
		CardModificationInfo modInfo,
		Action callback = null
	)
	{
		CardInfo cardInfoClone = playableCard.Info.Clone() as CardInfo;
		cardInfoClone.Mods.Add(modInfo);
		playableCard.SetInfo(cardInfoClone);
		callback?.Invoke();
	}

	public static IEnumerator DieCustom(
		this PlayableCard playableCard,
		bool wasSacrifice,
		PlayableCard killer = null,
		bool playSound = true,
		float royalTableSwayValue = 0f
	)
	{
		if (playableCard.NotDead())
		{
			Animator customArmPrefab = playableCard.Anim.GetCustomArm();
			playableCard.Dead = true;
			CardSlot slotBeforeDeath = playableCard.Slot;
			if (playableCard.Info && playableCard.Info.name.ToLower().Contains("squirrel"))
			{
				AscensionStatsData.TryIncrementStat(AscensionStat.Type.SquirrelsKilled);
			}

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

			if (!GrimoraSaveUtil.IsNotGrimoraModRun) { 
			if (playableCard.LacksAbility(Ability.QuadrupleBones) && playableCard.LacksAbility(Boneless.ability) && playableCard.LacksTrait(Trait.Terrain) && slotBeforeDeath.IsPlayerSlot)
			{
				yield return ResourcesManager.Instance.AddBones(1, slotBeforeDeath);
			}
			}
			else if (playableCard.LacksAbility(Ability.QuadrupleBones) && playableCard.LacksAbility(Boneless.ability) && slotBeforeDeath.IsPlayerSlot)
			{
				yield return ResourcesManager.Instance.AddBones(1, slotBeforeDeath);
			}

			if (playableCard.TriggerHandler.RespondsToTrigger(Trigger.Die, wasSacrifice, killer))
			{
				yield return playableCard.TriggerHandler.OnTrigger(Trigger.Die, wasSacrifice, killer);
			}

			if (customArmPrefab)
			{
				UnityObject.Destroy(customArmPrefab.gameObject);
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

				if (royalTableSwayValue == -7f) {

					TweenBase slidingCard = Tween.LocalPosition(
		playableCard.transform,
		new Vector3(positionCopy.x, 5f, positionCopy.z),
		GrimoraModRoyalBossSequencer.DurationTableSway,
		0,
		Tween.EaseIn,
		completeCallback: () => playableCard.Anim.PlayDeathAnimation(playSound && !wasSacrifice)
	);

				}

				else { 
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
}
