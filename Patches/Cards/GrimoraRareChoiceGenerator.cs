using System.Collections;
using DiskCardGame;
using HarmonyLib;
using InscryptionAPI.Card;
using Pixelplacement;
using UnityEngine;

namespace GrimoraMod;

public class GrimoraRareChoiceGenerator : Part1RareChoiceGenerator
{
	public override List<CardChoice> GenerateChoices(CardChoicesNodeData data, int randomSeed)
	{
		return RandomUtils.GenerateRandomChoicesForRare(CardManager.AllCardsCopy);
	}
}

[HarmonyPatch(typeof(RareCardChoicesSequencer))]
public class RareCardChoicesSequencerPatch
{
	[HarmonyPostfix, HarmonyPatch(nameof(RareCardChoicesSequencer.ChooseRareCard))]
	public static IEnumerator ChangeDeckListRunState(IEnumerator enumerator, RareCardChoicesSequencer __instance)
	{
		if (GrimoraSaveUtil.IsNotGrimoraModRun)
		{
			yield return enumerator;
			yield break;
		}

		ViewManager.Instance.SetViewLocked();
		ViewManager.Instance.SwitchToView(View.Default);
		yield return new WaitForSeconds(1f);

		TableRuleBook.Instance.SetOnBoard(true);
		__instance.deckPile.gameObject.SetActive(true);
		__instance.StartCoroutine(__instance.deckPile.SpawnCards(RunState.Run.playerDeck.Cards.Count));
		Vector3 vector = new Vector3(__instance.box.position.x, __instance.box.position.y, 0.5f);
		Vector3 startPos = new Vector3(vector.x, vector.y, 9f);
		__instance.box.position = startPos;
		__instance.box.gameObject.SetActive(true);
		AudioController.Instance.PlaySound3D("woodbox_slide", MixerGroup.TableObjectsSFX, vector);
		Tween.Position(__instance.box, vector, 0.3f, 0f, Tween.EaseOut);
		yield return new WaitForSeconds(0.8f);

		
		__instance.selectableCards = __instance.SpawnCards(3, __instance.box.transform, new Vector3(-1.55f, 0.2f, 0f));

		List<CardChoice> list = !AscensionSaveData.Data.ChallengeIsActive(AscensionChallenge.NoBossRares)
			                        ? __instance.rareChoiceGenerator.GenerateChoices(SaveManager.SaveFile.GetCurrentRandomSeed())
			                        : __instance.choiceGenerator.GenerateChoices(new CardChoicesNodeData(), SaveManager.SaveFile.GetCurrentRandomSeed());
		for (int i = 0; i < __instance.selectableCards.Count; i++)
		{
			var selectableCard = __instance.selectableCards[i];
			selectableCard.gameObject.SetActive(true);
			selectableCard.ChoiceInfo = list[i];
			selectableCard.Initialize(
				list[i].CardInfo,
				__instance.OnRewardChosen,
				__instance.OnCardFlipped,
				true,
				__instance.OnCardInspected
			);
			selectableCard.SetEnabled(false);
			selectableCard.SetFaceDown(true, true);
			SpecialCardBehaviour[] components = selectableCard.GetComponents<SpecialCardBehaviour>();
			foreach (var specialBehaviour in components)
			{
				specialBehaviour.OnShownForCardChoiceNode();
			}
		}

		__instance.box.GetComponentInChildren<Animator>().Play("open", 0, 0f);
		AudioController.Instance.PlaySound3D("woodbox_open", MixerGroup.TableObjectsSFX, __instance.box.transform.position);
		yield return new WaitForSeconds(2f);

		ViewManager.Instance.SwitchToView(__instance.choicesView);
		
		InteractionCursor.Instance.InteractionDisabled = false;
		__instance.SetCollidersEnabled(true);
		__instance.gamepadGrid.enabled = true;
		__instance.EnableViewDeck(__instance.viewControlMode, __instance.basePosition);
		__instance.chosenReward = null;
		yield return new WaitUntil(() => __instance.chosenReward);
		__instance.DisableViewDeck();
		__instance.chosenReward.transform.parent = null;
		RuleBookController.Instance.SetShown(false);
		__instance.gamepadGrid.enabled = false;
		__instance.deckPile.MoveCardToPile(__instance.chosenReward, true, 0.25f, 1.15f);
		__instance.AddChosenCardToDeck();
		__instance.CleanupMushrooms();
		yield return new WaitForSeconds(0.5f);

		ViewManager.Instance.SwitchToView(View.Default);
		yield return new WaitForSeconds(0.5f);

		__instance.box.GetComponentInChildren<Animator>().Play("close", 0, 0f);
		AudioController.Instance.PlaySound3D(
			"woodbox_close",
			MixerGroup.TableObjectsSFX,
			__instance.box.transform.position
		);
		yield return new WaitForSeconds(0.6f);

		__instance.CleanUpCards(false);
		Tween.Position(__instance.box, startPos, 0.3f, 0f, Tween.EaseInOut);
		yield return new WaitForSeconds(0.3f);

		__instance.box.gameObject.SetActive(false);
		yield return __instance.StartCoroutine(__instance.deckPile.DestroyCards());
		__instance.deckPile.gameObject.SetActive(false);

		GameFlowManager.Instance.TransitionToGameState(GameState.Map);
	}
}
