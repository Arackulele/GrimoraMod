using System.Collections;
using DiskCardGame;
using InscryptionAPI.Card;
using Pixelplacement;
using Sirenix.Utilities;
using UnityEngine;
using static GrimoraMod.GrimoraPlugin;

namespace GrimoraMod;

public class GrimoraGainConsumableSequencer : GainConsumablesSequencer
{
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
		cardRemoveSequencerObj.name = "CardRemoveSequencer_Grimora";

		var oldSequence = cardRemoveSequencerObj.GetComponent<GainConsumablesSequencer>();

		var gainConsumableSequence = cardRemoveSequencerObj.AddComponent<GrimoraGainConsumableSequencer>();

		gainConsumableSequence.fullConsumablesReward = CardLoader.GetCardByName("arackulele.inscryption.grimoramod_Bonepile");
		gainConsumableSequence.backpack = oldSequence.backpack;
		gainConsumableSequence.rat = oldSequence.rat;
		gainConsumableSequence.slots = oldSequence.slots;
		gainConsumableSequence.slotsGamepadControl = oldSequence.slotsGamepadControl;

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
	
	public IEnumerator ReplenishConsumables(GainConsumablesNodeData nodeData)
	{
		Singleton<ViewManager>.Instance.SwitchToView(View.Default, false, true);
		yield return new WaitForEndOfFrame();
		//Singleton<ExplorableAreaManager>.Instance.SetHandLightRange(17f, 0.25f);
		//Singleton<ExplorableAreaManager>.Instance.SetHangingLightRange(7f, 0.25f);
		backpack.SetActive(true);
		yield return new WaitForSeconds(0.25f);
		if (Singleton<GameFlowManager>.Instance != null)
		{
			yield return new WaitUntil(() => !Singleton<GameFlowManager>.Instance.Transitioning);
		}
		ChallengeActivationUI.TryShowActivation(AscensionChallenge.LessConsumables);
		if (GrimoraItemsManagerExt.Instance.SaveDataItemsList.Count == RunState.Run.MaxConsumables)
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
		});
		if (Singleton<GameFlowManager>.Instance != null)
		{
			Singleton<GameFlowManager>.Instance.TransitionToGameState(GameState.Map);
		}
	}
}
