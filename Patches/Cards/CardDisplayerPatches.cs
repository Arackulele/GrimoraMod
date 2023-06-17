using DiskCardGame;
using HarmonyLib;
using UnityEngine;

namespace GrimoraMod;

[HarmonyPatch(typeof(CardDisplayer))]
public class CardDisplayerPatches
{
	[HarmonyPrefix, HarmonyPatch(nameof(CardDisplayer.SetTextColors))]
	public static bool PrefixChangeRenderColors(ref CardDisplayer __instance,
	                                            CardRenderInfo renderInfo,
	                                            PlayableCard playableCard)
	{
		// I check for GBCCameras because it is an object that only appears while act 2 cards are being rendered in any way
		//to account for KCmod deck screen
		if (GrimoraSaveUtil.IsNotGrimoraModRun || GameObject.Find("GBCCameras") != null )
		{
			return true;
		}


		_ = playableCard ? playableCard.MaxHealth : renderInfo.baseInfo.Health;

		__instance.SetHealthTextColor(
			renderInfo.health >= renderInfo.baseInfo.Health
				? GameColors.Instance.glowSeafoam
				: GameColors.Instance.darkLimeGreen
		);

		__instance.SetAttackTextColor(GameColors.Instance.glowSeafoam);
		__instance.SetNameTextColor(GameColors.Instance.glowSeafoam);

		return false;
	}
}

[HarmonyPatch(typeof(GravestoneCardDisplayer))]
public class GravestoneCardDisplayerPatches
{
	[HarmonyPostfix, HarmonyPatch(nameof(GravestoneCardDisplayer.DisplayInfo))]
	public static void CorrectRenderBoneCost(GravestoneCardDisplayer __instance, CardRenderInfo renderInfo, PlayableCard playableCard)
	{
		string bonesCost = Mathf.Max(0, renderInfo.baseInfo.BonesCost).ToString();
		__instance.costShadow.text = bonesCost;
		__instance.costText.text = bonesCost;
	}
}
