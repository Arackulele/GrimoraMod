using System;
using System.Collections;
using System.Collections.Generic;
using DiskCardGame;
using Pixelplacement;
using Sirenix.Utilities;
using UnityEngine;

namespace GrimoraMod;

public class GrimoraGainConsumableSequencer : GainConsumablesSequencer
{

	static GameObject PharaoChest;
	public static void CreateSequencerInScene()
	{
		if (SpecialNodeHandler.Instance.SafeIsUnityNull() || SpecialNodeHandler.Instance.gainConsumablesSequencer)
		{
			return;
		}

		GameObject cardRemoveSequencerObj = Instantiate(
			ResourceBank.Get<GameObject>("Prefabs/SpecialNodeSequences/GainConsumablesSequencer"),
			SpecialNodeHandler.Instance.transform
		);

		PharaoChest = GameObject.Instantiate(GrimoraPlugin.kopieGameObjects.Find(g => g.name.Contains("PharaoChestPrefab")));
		cardRemoveSequencerObj.name = "CardRemoveSequencer_Grimora";

		var oldSequence = cardRemoveSequencerObj.GetComponent<GainConsumablesSequencer>();


		var gainConsumableSequence = cardRemoveSequencerObj.AddComponent<GrimoraGainConsumableSequencer>();



		gainConsumableSequence.fullConsumablesReward = CardLoader.GetCardByName(GrimoraPlugin.NameTombRobber);
		gainConsumableSequence.backpack = oldSequence.backpack;
		gainConsumableSequence.rat = oldSequence.rat;
		gainConsumableSequence.slots = oldSequence.slots;
		gainConsumableSequence.slotsGamepadControl = oldSequence.slotsGamepadControl;
		PharaoChest.transform.parent = gainConsumableSequence.backpack.transform;

		gainConsumableSequence.ratCard = Instantiate(
			AssetConstants.GrimoraSelectableCard,
			oldSequence.ratCard.transform.parent
		).GetComponent<SelectableCard>();
		gainConsumableSequence.ratCard.transform.localRotation = Quaternion.identity;
		gainConsumableSequence.ratCard.transform.localPosition = Vector3.zero;
		
		
		Destroy(oldSequence.ratCard.gameObject);
		Destroy(oldSequence);

		SpecialNodeHandler.Instance.gainConsumablesSequencer = gainConsumableSequence;
	}
	
	public new IEnumerator ReplenishConsumables(GainConsumablesNodeData nodeData)
	{


		Singleton<ViewManager>.Instance.SwitchToView(View.Default, false, true);
		yield return new WaitForEndOfFrame();
		//Singleton<ExplorableAreaManager>.Instance.SetHandLightRange(17f, 0.25f);
		//Singleton<ExplorableAreaManager>.Instance.SetHangingLightRange(7f, 0.25f);
		backpack.SetActive(true);
		GameObject.Find("BackpackModel").transform.position = new Vector3(0, -1000f, 0);
		PharaoChest.SetActive(true);
		yield return new WaitForSeconds(0.25f);
		if (Singleton<GameFlowManager>.Instance != null)
		{
			yield return new WaitUntil(() => !Singleton<GameFlowManager>.Instance.Transitioning);
		}
		ChallengeActivationUI.TryShowActivation(AscensionChallenge.LessConsumables);
		if (RunState.Run.consumables.Count == RunState.Run.MaxConsumables)
		{
			yield return FullConsumablesSequence();
		}
		else if (!ProgressionData.LearnedMechanic(MechanicsConcept.GainConsumables))
		{
			yield return TutorialGainConsumables();
			ProgressionData.SetMechanicLearned(MechanicsConcept.GainConsumables);
		}
		else
		{
			yield return RegularGainConsumables(nodeData);
			ProgressionData.SetMechanicLearned(MechanicsConcept.ChooseConsumables);
		}
		//Singleton<ExplorableAreaManager>.Instance.ResetHandLightRange(0.25f);
		//Singleton<ExplorableAreaManager>.Instance.ResetHangingLightRange(0.25f);
		backpack.GetComponentInChildren<Animator>().SetTrigger("exit");
		CustomCoroutine.WaitThenExecute(0.25f, delegate
		{
			backpack.SetActive(false);
			PharaoChest.SetActive(false);
		});
		if (Singleton<GameFlowManager>.Instance != null)
		{
			Singleton<GameFlowManager>.Instance.TransitionToGameState(GameState.Map);
		}
	}

	private new IEnumerator RegularGainConsumables(GainConsumablesNodeData nodeData)
	{
		bool tutorialMessageShown = false;
		int randomSeed = SaveManager.SaveFile.GetCurrentRandomSeed();
		while (RunState.Run.consumables.Count < RunState.Run.MaxConsumables)
		{
			yield return new WaitForSeconds(0.1f);
			SelectableItemSlot selectedSlot = null;
			List<ConsumableItemData> data = GenerateItemChoices(nodeData, randomSeed);
			randomSeed += 100;
			foreach (SelectableItemSlot slot in slots)
			{
				ConsumableItemData data2 = data[slots.IndexOf(slot)];
				slot.gameObject.SetActive(true);
				slot.CreateItem(data2);
				SelectableItemSlot selectableItemSlot = slot;
				selectableItemSlot.CursorSelectStarted = (Action<MainInputInteractable>)Delegate.Combine(selectableItemSlot.CursorSelectStarted, (Action<MainInputInteractable>)delegate(MainInputInteractable i)
				{
					selectedSlot = i as SelectableItemSlot;
				});
				SelectableItemSlot selectableItemSlot2 = slot;
				selectableItemSlot2.CursorEntered = (Action<MainInputInteractable>)Delegate.Combine(selectableItemSlot2.CursorEntered, (Action<MainInputInteractable>)delegate(MainInputInteractable i)
				{
					Singleton<OpponentAnimationController>.Instance.SetLookTarget(i.transform, Vector3.up * 2f);
				});
				AlternateInputInteractable component = slot.GetComponent<AlternateInputInteractable>();
				component.AlternateSelectStarted = (Action<AlternateInputInteractable>)Delegate.Combine(component.AlternateSelectStarted, (Action<AlternateInputInteractable>)delegate
				{
					Singleton<RuleBookController>.Instance.OpenToItemPage(slot.Item.Data.name, true);
				});
				yield return new WaitForSeconds(0.1f);
				if (!ProgressionData.IntroducedConsumable(slot.Item.Data))
				{
					yield return new WaitForSeconds(0.1f);
					yield return LearnItemSequence(slot.Item);
					ProgressionData.SetConsumableIntroduced(slot.Item.Data);
				}
			}
			if (!tutorialMessageShown && !ProgressionData.LearnedMechanic(MechanicsConcept.ChooseConsumables))
			{
				StartCoroutine(Singleton<TextDisplayer>.Instance.ShowThenClear("Choose one.", 4f));
				tutorialMessageShown = true;
			}
			SetSlotCollidersEnabled(true);
			yield return new WaitUntil(() => selectedSlot != null);
			Singleton<RuleBookController>.Instance.SetShown(false);
			RunState.Run.consumables.Add(selectedSlot.Item.Data.name);
			DisableSlotsAndExitItems(selectedSlot);
			yield return new WaitForSeconds(0.2f);
			selectedSlot.Item.PlayExitAnimation();
			yield return new WaitForSeconds(0.1f);
			Singleton<ItemsManager>.Instance.UpdateItems();
			foreach (SelectableItemSlot slot2 in slots)
			{
				slot2.ClearDelegates();
				slot2.GetComponent<AlternateInputInteractable>().ClearDelegates();
			}
			SetSlotsActive(false);
			Singleton<OpponentAnimationController>.Instance.ClearLookTarget();
			foreach (SelectableItemSlot slot3 in slots)
			{
				UnityEngine.Object.Destroy(slot3.Item.gameObject);
			}
		}
		if (!ProgressionData.LearnedMechanic(MechanicsConcept.ChooseConsumables))
		{
			yield return new WaitForSeconds(0.4f);
			yield return Singleton<TextDisplayer>.Instance.ShowUntilInput("Feeling overburdened enough with a full 3 items, you carried on.", -2.5f, 0.5f, Emotion.Neutral, TextDisplayer.LetterAnimation.WavyJitter);
		}
	}

	private new IEnumerator TutorialGainConsumables()
	{
		Singleton<ItemsManager>.Instance.SetSlotsAtEdge(false, true);
		List<ConsumableItemData> list = new List<ConsumableItemData>
		{
			ItemsUtil.GetConsumableByName("SquirrelBottle"),
			ItemsUtil.GetConsumableByName("SquirrelBottle"),
			ItemsUtil.GetConsumableByName("Pliers")
		};
		SelectableItemSlot middleSlot = slots[1];
		middleSlot.gameObject.SetActive(true);
		foreach (ConsumableItemData item in list)
		{
			if (!ProgressionData.IntroducedConsumable(item))
			{
				middleSlot.CreateItem(item);
				yield return new WaitForSeconds(0.15f);
				yield return new WaitForSeconds(0.1f);
				yield return StartCoroutine(LearnItemSequence(middleSlot.Item));
				ProgressionData.SetConsumableIntroduced(middleSlot.Item.Data);
				middleSlot.Item.PlayExitAnimation();
				yield return new WaitForSeconds(0.25f);
				if (middleSlot.Item != null)
				{
					UnityEngine.Object.Destroy(middleSlot.Item.gameObject);
				}
			}
			else
			{
				yield return Singleton<TextDisplayer>.Instance.ShowUntilInput("And have a second...", 0f, 0f);
			}
			RunState.Run.consumables.Add(item.name);
			Singleton<ItemsManager>.Instance.UpdateItems();
			yield return new WaitForSeconds(0.75f);
		}
		yield return Singleton<TextDisplayer>.Instance.ShowUntilInput("Three is as much as you can carry.", -0.5f, 0.45f);
	}

	private new IEnumerator FullConsumablesSequence()
	{
		yield return new WaitForSeconds(1f);
		yield return Singleton<TextDisplayer>.Instance.PlayDialogueEvent("GainConsumablesFull", TextDisplayer.MessageAdvanceMode.Input, TextDisplayer.EventIntersectMode.Wait, new string[1] { RunState.Run.MaxConsumables.ToString() });
		yield return new WaitForSeconds(0.5f);
		rat.SetActive(true);
		yield return new WaitUntil(() => ratCard.gameObject.activeInHierarchy);
		bool cardGrabbed = false;
		ratCard.SetInfo(fullConsumablesReward);
		ratCard.ClearDelegates();
		SelectableCard selectableCard = ratCard;
		selectableCard.CursorSelectEnded = (Action<MainInputInteractable>)Delegate.Combine(selectableCard.CursorSelectEnded, (Action<MainInputInteractable>)delegate
		{
			cardGrabbed = true;
		});
		yield return new WaitUntil(() => cardGrabbed);
		Singleton<RuleBookController>.Instance.SetShown(false);
		Singleton<TableRuleBook>.Instance.SetOnBoard(false);
		rat.GetComponentInChildren<Animator>().SetTrigger("exit");
		yield return new WaitForEndOfFrame();
		GameObject cardObj = UnityEngine.Object.Instantiate(ratCard.gameObject);
		cardObj.SetActive(true);
		cardObj.transform.parent = null;
		cardObj.transform.position = ratCard.transform.position;
		cardObj.transform.rotation = ratCard.transform.rotation;
		cardObj.transform.localScale = Vector3.one;
		SelectableCard component = cardObj.GetComponent<SelectableCard>();
		component.SetInfo(fullConsumablesReward);
		component.SetInteractionEnabled(false);
		string text = fullConsumablesReward.description;
		if (ProgressionData.LearnedCard(fullConsumablesReward))
		{
			text = string.Format(Localization.Translate("A [c:bR]{0}[c:]... He's only here for the riches."), fullConsumablesReward.DisplayedNameLocalized);
		}
		yield return LearnObjectSequence(cardObj.transform, 1f, new Vector3(20f, 0f, 0f), text);
		Tween.Position(cardObj.transform, cardObj.transform.position + Vector3.up * 2f + Vector3.forward * 0.5f, 0.1f, 0f, null, Tween.LoopType.None, null, delegate
		{
			UnityEngine.Object.Destroy(cardObj);
		});
		yield return new WaitForSeconds(0.5f);
		rat.SetActive(false);
		RunState.Run.playerDeck.AddCard(fullConsumablesReward);
	}

	private new IEnumerator LearnItemSequence(Item item)
	{
		item.Anim.enabled = false;
		item.PlayPickUpSound();
		float heightOffset = 1f - item.Data.modelHeight;
		string description = (item.Data as ConsumableItemData).description;
		yield return LearnObjectSequence(item.transform, heightOffset, new Vector3(-3.2f, 26f, -3f), description);
		Tween.LocalPosition(item.transform, Vector3.zero, 0.1f, 0f, Tween.EaseInOut, Tween.LoopType.None, null, delegate
		{
			item.Anim.enabled = true;
			item.PlayPlacedSound();
		});
		Tween.LocalRotation(item.transform, Vector3.zero, 0.1f, 0f, Tween.EaseInOut);
	}

	private new IEnumerator LearnObjectSequence(Transform obj, float heightOffset, Vector3 baseRotation, string text)
	{
		Tween.Position(obj.transform, new Vector3(0f, 5.7f + heightOffset, -4.25f), 0.1f, 0f, Tween.EaseInOut);
		Tween.Rotation(obj.transform, baseRotation, 0.1f, 0f, Tween.EaseInOut);
		Tween.Rotate(obj.transform, new Vector3(1f, 5f, 3f), Space.World, 3f, 0.1f, Tween.EaseInOut, Tween.LoopType.PingPong);
		yield return Singleton<TextDisplayer>.Instance.ShowUntilInput(text);
	}

	private new List<ConsumableItemData> GenerateItemChoices(GainConsumablesNodeData nodeData, int randomSeed)
	{
		List<ConsumableItemData> unlockedConsumablesForRegion = ItemsUtil.GetUnlockedConsumablesForRegion(RunState.CurrentMapRegion);
		unlockedConsumablesForRegion.RemoveAll((a) => a.rulebookCategory != AbilityMetaCategory.GrimoraRulebook);
		
		List<ConsumableItemData> list = new List<ConsumableItemData>();
		
		while (list.Count < 3)
		{
			if (unlockedConsumablesForRegion.Count == 0)
			{
				list.Add(ItemsUtil.GetConsumableByName("SquirrelBottle"));
				continue;
			}
			int powerLevelRoll = SeededRandom.Range(1, 6, randomSeed++);
			List<ConsumableItemData> list2 = unlockedConsumablesForRegion.FindAll((ConsumableItemData x) => x.powerLevel <= powerLevelRoll);
			if (list2.Count > 0)
			{
				ConsumableItemData consumableItemData = list2[SeededRandom.Range(0, list2.Count, randomSeed++)];
				unlockedConsumablesForRegion.Remove(consumableItemData);
				if (consumableItemData != null)
				{
					list.Add(consumableItemData);
				}
			}
		}
		
		return list;
	}
}
