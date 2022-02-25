using System.Collections;
using DiskCardGame;
using HarmonyLib;
using UnityEngine;

namespace GrimoraMod;

[HarmonyPatch(typeof(IceCube))]
public class IceCubePatches
{
	[HarmonyPostfix, HarmonyPatch(nameof(IceCube.OnDie))]
	public static IEnumerator PostfixUseInfoObjectAndNotJustName(
		IEnumerator enumerator, IceCube __instance, bool wasSacrifice, PlayableCard killer
	)
	{
		if (GrimoraSaveUtil.isNotGrimora)
		{
			yield return enumerator;
			yield break;
		}

		yield return __instance.PreSuccessfulTriggerSequence();
		yield return new WaitForSeconds(0.3f);
		CardInfo creatureToSpawn = "Opossum".GetCardInfo();
		if (__instance.Card.Info.iceCubeParams != null && __instance.Card.Info.iceCubeParams.creatureWithin != null)
		{
			creatureToSpawn = __instance.Card.Info.iceCubeParams.creatureWithin;
		}

		CardSlot slot = __instance.Card.Slot;
		SkinCrawler skinCrawler = SkinCrawler.GetSkinCrawlerFromCard(__instance.Card);
		yield return BoardManager.Instance.CreateCardInSlot(creatureToSpawn, slot, 0.15f);
		if (skinCrawler is not null)
		{
			GrimoraPlugin.Log.LogDebug($"[IceCube] SkinCrawler exists in card [{__instance.Card}] ");
			yield return skinCrawler.AssignSkinCrawlerCardToHost(slot);
		}
		yield return __instance.LearnAbility(0.5f);
	}
}
