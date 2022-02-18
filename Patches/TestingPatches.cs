using System.Collections;
using DiskCardGame;
using HarmonyLib;
using Pixelplacement;
using UnityEngine;
using static GrimoraMod.GrimoraPlugin;

namespace GrimoraMod;

[HarmonyPatch]
public class TestingPatches
{
	[HarmonyPrefix, HarmonyPatch(typeof(CardLoader), nameof(CardLoader.GetCardByName))]
	public static bool PrefixAddExceptionForCardLoaderGetCardByName(CardLoader __instance, string name,
		ref CardInfo __result)
	{
		if (GrimoraSaveUtil.isNotGrimora)
		{
			return true;
		}

		try
		{
			__result = CardLoader.Clone(ScriptableObjectLoader<CardInfo>.AllData.Find(x => x.name == name));
		}
		catch (Exception e)
		{
			Log.LogWarning($"Unable to GetCardByName with name [{name}]! " +
			               $"Current card pool count [{CardLoader.allData?.Count}]\nException [{e.Message}]");
			throw;
		}

		return false;
	}
}
