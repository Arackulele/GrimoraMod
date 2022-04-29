using DiskCardGame;
using HarmonyLib;
using InscryptionAPI.Card;
using Sirenix.Utilities;
using UnityEngine;

namespace GrimoraMod;

[HarmonyPatch(typeof(Card))]
public class BaseCardPatches
{
	[HarmonyPrefix, HarmonyPatch(nameof(Card.SetCardbackSubmerged))]
	public static bool SetCardbackSubmergedFixWhenDeadPatch(Card __instance)
	{
		if (GrimoraSaveUtil.isNotGrimora)
		{
			return true;
		}

		if ((__instance as PlayableCard).NotDead())
		{
			__instance.SetCardback(ResourceBank.Get<Texture>("Art/Cards/card_back_submerge"));
		}
		else
		{
			GrimoraPlugin.Log.LogInfo($"[Card.SetCardbackSubmerged] Card [{__instance.name}] IS dead. Not setting cardback.");
		}
		
		return false;
	}


	[HarmonyPostfix, HarmonyPatch(nameof(Card.SetInfo))]
	public static void AddNewController(Card __instance, CardInfo info)
	{
		if (GrimoraSaveUtil.isNotGrimora)
		{
			return;
		}
		
		if (__instance.GetComponentInParent<SelectableCard>().SafeIsUnityNull() && __instance.GetComponent<GraveControllerExt>().SafeIsUnityNull())
		{
			var oldController = __instance.GetComponent<GravestoneCardAnimationController>();
			GraveControllerExt.SetupNewController(oldController);
		}
	}
}
