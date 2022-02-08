﻿using DiskCardGame;
using HarmonyLib;

namespace GrimoraMod;

[HarmonyPatch(typeof(SpecialNodeHandler), nameof(SpecialNodeHandler.StartSpecialNodeSequence))]
public class SpecialNodeHandlerPatches
{
	[HarmonyPrefix]
	public static bool CastToGrimoraCardRemoveSequencer(SpecialNodeHandler __instance, ref SpecialNodeData nodeData)
	{
		if (!SaveManager.SaveFile.IsGrimora)
		{
			return true;
		}

		if (nodeData is CardRemoveNodeData)
		{
			// We have to cast it, otherwise it tries to call the base version of it
			__instance.StartCoroutine(((GrimoraCardRemoveSequencer)__instance.cardRemoveSequencer).RemoveSequence());
			return false;

		}

		if (nodeData is CardStatBoostNodeData)
		{
			__instance.StartCoroutine(((BoneyardBurialSequencer)__instance.cardStatBoostSequencer).BurialSequence());
			return false;
		}

		return true;
	}
}