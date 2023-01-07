using System.Collections;
using DiskCardGame;
using HarmonyLib;
using InscryptionAPI.Helpers.Extensions;
using UnityEngine;
using static GrimoraMod.GrimoraPlugin;

namespace GrimoraMod;

[HarmonyPatch(typeof(IceCube))]
public class IceCubePatches
{
	[HarmonyPostfix, HarmonyPatch(nameof(IceCube.OnDie))]
	public static IEnumerator PostfixUseInfoObjectAndNotJustName(
		IEnumerator enumerator,
		IceCube __instance,
		bool wasSacrifice,
		PlayableCard killer
	)
	{
		if (GrimoraSaveUtil.IsNotGrimoraModRun)
		{
			yield return enumerator;
			yield break;
		}

		yield return __instance.PreSuccessfulTriggerSequence();
		yield return new WaitForSeconds(0.3f);
		CardInfo creatureToSpawn = "Skeleton".GetCardInfo();
		if (__instance.Card.Info.iceCubeParams.IsNotNull() && __instance.Card.Info.iceCubeParams.creatureWithin)
		{
			creatureToSpawn = __instance.Card.Info.iceCubeParams.creatureWithin;
		}

		yield return __instance.Card.Slot.CreateCardInSlot(creatureToSpawn, 0.15f);
		yield return __instance.LearnAbility(0.5f);
	}
}
