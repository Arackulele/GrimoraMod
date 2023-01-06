using System.Collections;
using DiskCardGame;
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

		gainConsumableSequence.backpack = oldSequence.backpack;
		gainConsumableSequence.rat = oldSequence.rat;
		gainConsumableSequence.ratCard = oldSequence.ratCard;
		gainConsumableSequence.fullConsumablesReward = oldSequence.fullConsumablesReward;

		Destroy(oldSequence);

		SpecialNodeHandler.Instance.gainConsumablesSequencer = gainConsumableSequence;
	}
}
