using DiskCardGame;
using HarmonyLib;

namespace GrimoraMod;

[HarmonyPatch(typeof(SpecialNodeHandler))]
public class SpecialNodeHandlerPatches
{
	[HarmonyPrefix, HarmonyPatch(nameof(SpecialNodeHandler.StartSpecialNodeSequence))]
	public static bool CastToGrimoraCardRemoveSequencer(SpecialNodeHandler __instance, ref SpecialNodeData nodeData)
	{
		if (GrimoraSaveUtil.IsNotGrimoraModRun)
		{
			return true;
		}

		if (nodeData is GrimoraCardRemoveNodeData)
		{
			// We have to cast it, otherwise it tries to call the base version of it
			__instance.StartCoroutine(((GrimoraCardRemoveSequencer)__instance.cardRemoveSequencer).RemoveSequence());
			return false;
		}

		if (nodeData is GrimoraGainConsumableNodeData)
		{
			// We have to cast it, otherwise it tries to call the base version of it
			__instance.StartCoroutine(((GrimoraGainConsumableSequencer)__instance.gainConsumablesSequencer).ReplenishConsumables(null));
			return false;
		}

		if (nodeData is BoneyardBurialNodeData)
		{
			__instance.StartCoroutine(((BoneyardBurialSequencer)__instance.cardStatBoostSequencer).BurialSequence());
			return false;
		}

		if (nodeData is ElectricChairNodeData)
		{
			__instance.StartCoroutine(ElectricChairSequencer.Instance.StartSequence());
			return false;
		}

		if (nodeData is GoatEyeNodeData)
		{
			__instance.StartCoroutine(GoatEyeSequencer.Instance.StartSequence());
			return false;
		}


		if (nodeData is GravebardCampNodeData)
		{
			__instance.StartCoroutine(GravebardCampSequencer.Instance.StartSequence());
			return false;
		}

		if (nodeData is CardMergeNodeData)
		{
			__instance.StartCoroutine(GrimoraCardMergeSequencer.Instance.StartSequence());
			return false;
		}

		return true;
	}
}
