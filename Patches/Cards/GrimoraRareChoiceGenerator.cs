using System.Collections;
using DiskCardGame;
using HarmonyLib;
using Pixelplacement;
using UnityEngine;

namespace GrimoraMod;

public class GrimoraRareChoiceGenerator : CardChoiceGenerator
{
	public override List<CardChoice> GenerateChoices(CardChoicesNodeData data, int randomSeed)
	{
		return RandomUtils.GenerateRandomChoicesOfCategory(CardLoader.allData, randomSeed, CardMetaCategory.Rare);
	}
}

[HarmonyPatch(typeof(RareCardChoicesSequencer), nameof(RareCardChoicesSequencer.ChooseRareCard))]
public class RareCardChoicesSequencerPatch
{
	[HarmonyPostfix]
	public static IEnumerator ChangeDeckListRunState(IEnumerator enumerator, RareCardChoicesSequencer __instance)
	{
		if (GrimoraSaveUtil.isNotGrimora)
		{
			yield return enumerator;
			yield break;
		}

		ViewManager.Instance.SetViewLocked();
		ViewManager.Instance.SwitchToView(View.Default);
		yield return new WaitForSeconds(1f);

		TableRuleBook.Instance.SetOnBoard(true);
		__instance.deckPile.gameObject.SetActive(true);
		__instance.StartCoroutine(__instance.deckPile.SpawnCards(GrimoraSaveUtil.DeckList.Count));
		Vector3 vector = new Vector3(__instance.box.position.x, __instance.box.position.y, 0.5f);
		Vector3 startPos = new Vector3(vector.x, vector.y, 9f);
		__instance.box.position = startPos;
		__instance.box.gameObject.SetActive(true);
		AudioController.Instance.PlaySound3D("woodbox_slide", MixerGroup.TableObjectsSFX, vector);
		Tween.Position(__instance.box, vector, 0.3f, 0f, Tween.EaseOut);
		yield return new WaitForSeconds(0.3f);
		yield return new WaitForSeconds(0.5f);

		yield return TextDisplayer.Instance.PlayDialogueEvent("RareCardsIntro", TextDisplayer.MessageAdvanceMode.Input);
		__instance.selectableCards = __instance.SpawnCards(3, __instance.box.transform, new Vector3(-1.55f, 0.2f, 0f));

		List<CardChoice> list = __instance.choiceGenerator.GenerateChoices(SaveManager.SaveFile.GetCurrentRandomSeed());
		for (int i = 0; i < __instance.selectableCards.Count; i++)
		{
			__instance.selectableCards[i].gameObject.SetActive(true);
			__instance.selectableCards[i].ChoiceInfo = list[i];
			__instance.selectableCards[i].Initialize(
				list[i].CardInfo,
				__instance.OnRewardChosen,
				__instance.OnCardFlipped,
				true,
				__instance.OnCardInspected
			);
			__instance.selectableCards[i].SetEnabled(false);
			__instance.selectableCards[i].SetFaceDown(true, true);
		}

		__instance.box.GetComponentInChildren<Animator>().Play("open", 0, 0f);
		AudioController.Instance.PlaySound3D("woodbox_open", MixerGroup.TableObjectsSFX, __instance.box.transform.position);
		yield return new WaitForSeconds(2f);

		ViewManager.Instance.SwitchToView(View.OpponentQueueCentered);
		InteractionCursor.Instance.InteractionDisabled = false;
		__instance.SetCollidersEnabled(true);
		__instance.gamepadGrid.enabled = true;
		__instance.chosenReward = null;
		yield return new WaitUntil(() => __instance.chosenReward.IsNotNull());
		__instance.chosenReward.transform.parent = null;
		RuleBookController.Instance.SetShown(false);
		__instance.gamepadGrid.enabled = false;
		__instance.deckPile.MoveCardToPile(__instance.chosenReward, true, 0.25f, 1f);
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
