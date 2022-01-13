using System.Collections;
using System.Collections.Generic;
using DiskCardGame;
using HarmonyLib;
using Pixelplacement;
using UnityEngine;

namespace GrimoraMod
{
	[HarmonyPatch(typeof(CardSingleChoicesSequencer))]
	public class CardSingleChoicesSequencerPatches
	{
		[HarmonyPrefix, HarmonyPatch(nameof(CardSingleChoicesSequencer.CardSelectionSequence))]
		public static void Prefix(ref CardSingleChoicesSequencer __instance, out CardSingleChoicesSequencer __state)
		{
			__state = __instance;
		}

		[HarmonyPostfix, HarmonyPatch(nameof(CardSingleChoicesSequencer.CardSelectionSequence))]
		public static IEnumerator Postfix(
			IEnumerator enumerator,
			SpecialNodeData nodeData,
			CardSingleChoicesSequencer __state
		)
		{
			// TODO: this is an exact copy, minus the commented out code at (choice.CardInfo != null) block
			if (SaveManager.saveFile.IsGrimora)
			{
				CardSingleChoicesSequencer choicesSequencer = __state;
				CardChoicesNodeData choicesData = nodeData as CardChoicesNodeData;
				if (StoryEventsData.EventCompleted(StoryEvent.CloverFound) && choicesSequencer.rerollInteractable != null)
				{
					choicesSequencer.rerollInteractable.gameObject.SetActive(true);
					choicesSequencer.rerollInteractable.SetEnabled(false);
					CustomCoroutine.WaitThenExecute(1f, delegate { __state.rerollInteractable.SetEnabled(true); });
				}

				choicesSequencer.chosenReward = null;
				int randomSeed = SaveManager.SaveFile.GetCurrentRandomSeed();
				while (choicesSequencer.chosenReward == null)
				{
					List<CardChoice> choices;
					if (choicesData.overrideChoices != null)
					{
						choices = choicesData.overrideChoices;
					}
					else
					{
						choices = choicesSequencer.choiceGenerator.GenerateChoices(choicesData, randomSeed);
						randomSeed *= 2;
					}

					float x = (float)((choices.Count - 1) * 0.5 * -1.5);
					choicesSequencer.selectableCards = choicesSequencer.SpawnCards(
						choices.Count, choicesSequencer.transform, new Vector3(x, 5.01f, 0.0f)
					);

					for (int i = 0; i < choices.Count; ++i)
					{
						CardChoice choice = choices[i];
						SelectableCard card = choicesSequencer.selectableCards[i];
						card.gameObject.SetActive(true);
						card.SetParticlesEnabled(true);
						card.SetEnabled(false);
						card.ChoiceInfo = choice;
						if (choice.CardInfo != null)
						{
							card.Initialize(choice.CardInfo, choicesSequencer.OnRewardChosen,
								choicesSequencer.OnCardFlipped, true,
								choicesSequencer.OnCardInspected);
							// if (i == 2 && card.Anim is GravestoneCardAnimationController)
							// {
							// 	card.Initialize(cardChoice.CardInfo, null, delegate
							// 	{
							// 		(card.Anim as GravestoneCardAnimationController).PlayGlitchOutAnimation();
							// 		card.SetEnabled(enabled: false);
							// 		card.SetInteractionEnabled(interactionEnabled: false);
							// 	}, startFlipped: true);
							// }
						}
						else if (choice.resourceType != ResourceType.None)
						{
							card.Initialize(choicesSequencer.GetCardbackTexture(choice),
								choicesSequencer.OnRewardChosen,
								choicesSequencer.OnCardFlipped,
								choicesSequencer.OnCardInspected);
						}
						else if (choice.tribe != Tribe.None)
						{
							card.Initialize(choicesSequencer.GetCardbackTexture(choice),
								choicesSequencer.OnRewardChosen,
								choicesSequencer.OnCardFlipped,
								choicesSequencer.OnCardInspected);
						}

						if (choice.isDeathcardChoice)
						{
							card.SetCardback(ResourceBank.Get<Texture>("Art/Cards/card_back_deathcard"));
						}

						card.SetFaceDown(true, true);
						Vector3 position = card.transform.position;
						card.transform.position = card.transform.position + Vector3.forward * 5f + new Vector3(-0.5f + UnityEngine.Random.value * 1f , 0f, 0f);
						Tween.Position(card.transform, position, 0.3f, 0.0f, Tween.EaseInOut);
						Tween.Rotate(card.transform, new Vector3(0f, 0f, Random.value * 1.5f), Space.Self, 0.4f, 0f, Tween.EaseOut);
						yield return new WaitForSeconds(0.2f);
						ParticleSystem componentInChildren = card.GetComponentInChildren<ParticleSystem>();
						if (componentInChildren != null)
						{
							var emmision = componentInChildren.emission;
							emmision.rateOverTime = 0f;
						}
					}

					yield return new WaitForSeconds(0.2f);
					if (choicesData.choicesType == CardChoicesType.Cost && !ProgressionData.LearnedMechanic(MechanicsConcept.CostBasedCardChoice))
					{
						yield return TextDisplayer.Instance.PlayDialogueEvent(
							"TutorialCostBasedChoice",
							TextDisplayer.MessageAdvanceMode.Input
						);
						yield return new WaitForSeconds(0.2f);
					}
					else if (choicesData.choicesType == CardChoicesType.Tribe && !ProgressionData.LearnedMechanic(MechanicsConcept.TribeBasedCardChoice))
					{
						yield return TextDisplayer.Instance.PlayDialogueEvent(
							"TutorialTribeBasedChoice",
							TextDisplayer.MessageAdvanceMode.Input
						);
						yield return new WaitForSeconds(0.2f);
					}
					else if (choicesData.choicesType == CardChoicesType.Deathcard && !ProgressionData.LearnedMechanic(MechanicsConcept.DeathcardCardChoice))
					{
						yield return TextDisplayer.Instance.PlayDialogueEvent(
							"TutorialDeathcardChoice",
							TextDisplayer.MessageAdvanceMode.Input
						);
						yield return new WaitForSeconds(0.2f);
						ProgressionData.SetMechanicLearned(MechanicsConcept.DeathcardCardChoice);
					}

					choicesSequencer.SetCollidersEnabled(true);
					choicesSequencer.choicesRerolled = false;
					choicesSequencer.EnableViewDeck(choicesSequencer.viewControlMode, choicesSequencer.basePosition);
					yield return new WaitUntil(() => __state.chosenReward != null || __state.choicesRerolled);
					choicesSequencer.DisableViewDeck();
					choicesSequencer.CleanUpCards();
				}

				yield return choicesSequencer.AddCardToDeckAndCleanUp(choicesSequencer.chosenReward);
			}
			else
			{
				yield return enumerator;
			}
			
		}
	}
}