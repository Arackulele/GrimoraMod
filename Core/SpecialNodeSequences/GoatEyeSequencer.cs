using System.Collections;
using System.Diagnostics.CodeAnalysis;
using DiskCardGame;
using HarmonyLib;
using InscryptionAPI.Card;
using Pixelplacement;
using Sirenix.Utilities;
using UnityEngine;
using static GrimoraMod.GrimoraPlugin;

namespace GrimoraMod;

[SuppressMessage("ReSharper", "InconsistentNaming")]
public class GoatEyeSequencer : ManagedBehaviour
{
	public static GoatEyeSequencer Instance => FindObjectOfType<GoatEyeSequencer>();


	[SerializeField]
	private ConfirmStoneButton confirmStone;


	public IEnumerator StartSequence()
	{

		confirmStone.Enter();

		yield return confirmStone.WaitUntilConfirmation();

		yield break;
	}

	public static void CreateSequencerInScene()
	{
		if (SpecialNodeHandler.Instance.SafeIsUnityNull())
		{
			return;
		}

		GameObject cardStatObj = Instantiate(
				ResourceBank.Get<GameObject>("Prefabs/SpecialNodeSequences/CardRemoveSequencer"),
	SpecialNodeHandler.Instance.transform
	);

		cardStatObj.name = "GoatEyeSequencer_Grimora";

		var newSequencer = cardStatObj.AddComponent<ElectricChairSequencer>();


	}



}

