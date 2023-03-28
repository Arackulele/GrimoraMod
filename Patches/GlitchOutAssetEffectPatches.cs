using DiskCardGame;
using HarmonyLib;
using UnityEngine;

namespace GrimoraMod;

[HarmonyPatch(typeof(GlitchOutAssetEffect))]
public class GlitchOutAssetEffectPatches
{

	[HarmonyPrefix, HarmonyPatch(nameof(GlitchOutAssetEffect.ShowDeletionInUI))]
	public static bool DoNotShowWindowIfDisabled(Transform parent)
	{
		return GrimoraSaveUtil.IsNotGrimoraModRun || GrimoraGameFlowManager.Instance.CurrentGameState != GameState.CardBattle;
	}
	
}
