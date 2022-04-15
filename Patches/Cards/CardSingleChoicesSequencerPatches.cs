using System.Collections;
using DiskCardGame;
using HarmonyLib;
using Pixelplacement;
using UnityEngine;

namespace GrimoraMod;

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
		// TODO: this is an exact copy, minus the commented out code at (choice.CardInfo) block
		if (SaveManager.saveFile.IsGrimora)
		{
			CardChoicesNodeData choicesData = nodeData as CardChoicesNodeData;
			if (StoryEventsData.EventCompleted(StoryEvent.CloverFound) && __state.rerollInteractable != null)
			{
				if (!AscensionSaveData.Data.ChallengeIsActive(AscensionChallenge.NoClover))
				{
					__state.rerollInteractable.gameObject.SetActive(true);
					__state.rerollInteractable.SetEnabled(false);
					CustomCoroutine.WaitThenExecute(1f, delegate
					{
						__state.rerollInteractable.SetEnabled(true);
					});
				}
				ChallengeActivationUI.TryShowActivation(AscensionChallenge.NoClover);
				if (AscensionSaveData.Data.ChallengeIsActive(AscensionChallenge.NoClover) && !DialogueEventsData.EventIsPlayed("ChallengeNoClover"))
				{
					yield return new WaitForSeconds(1f);
					yield return TextDisplayer.Instance.PlayDialogueEvent("ChallengeNoClover", TextDisplayer.MessageAdvanceMode.Input);
				}
			}

			__state.chosenReward = null;
			int randomSeed = SaveManager.SaveFile.GetCurrentRandomSeed();
			while (__state.chosenReward.IsNull())
			{
				List<CardChoice> choices;
				if (choicesData.overrideChoices != null)
				{
					choices = choicesData.overrideChoices;
				}
				else
				{
					choices = __state.choiceGenerator.GenerateChoices(choicesData, randomSeed);
					randomSeed *= 2;
				}

				float x = (float)((choices.Count - 1) * 0.5 * -1.5);
				__state.selectableCards = __state.SpawnCards(
					choices.Count,
					__state.transform,
					new Vector3(x, 5.01f, 0.0f)
				);

				for (int i = 0; i < choices.Count; ++i)
				{
					CardChoice choice = choices[i];
					SelectableCard card = __state.selectableCards[i];
					card.gameObject.SetActive(true);
					card.SetParticlesEnabled(true);
					card.SetEnabled(false);
					card.ChoiceInfo = choice;
					if (choice.CardInfo)
					{
						card.Initialize(
							choice.CardInfo,
							__state.OnRewardChosen,
							__state.OnCardFlipped,
							true,
							__state.OnCardInspected
						);
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

					card.SetFaceDown(true, true);
					Vector3 position = card.transform.position;
					card.transform.position = position
					                          + Vector3.forward * 5f
					                          + new Vector3(-0.5f + UnityRandom.value * 1f, 0f, 0f);
					Tween.Position(card.transform, position, 0.3f, 0.0f, Tween.EaseInOut);
					Tween.Rotate(
						card.transform,
						new Vector3(0f, 0f, UnityRandom.value * 1.5f),
						Space.Self,
						0.4f,
						0f,
						Tween.EaseOut
					);
					yield return new WaitForSeconds(0.2f);
					ParticleSystem componentInChildren = card.GetComponentInChildren<ParticleSystem>();
					if (componentInChildren)
					{
						var emission = componentInChildren.emission;
						emission.rateOverTime = 0f;
					}
				}

				yield return new WaitForSeconds(0.2f);

				__state.SetCollidersEnabled(true);
				__state.choicesRerolled = false;
				__state.EnableViewDeck(__state.viewControlMode, __state.basePosition);
				yield return new WaitUntil(() => __state.chosenReward || __state.choicesRerolled);
				__state.DisableViewDeck();
				__state.CleanUpCards();
			}

			yield return __state.AddCardToDeckAndCleanUp(__state.chosenReward);
		}
		else
		{
			yield return enumerator;
		}
	}
}
