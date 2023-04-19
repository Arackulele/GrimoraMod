using DiskCardGame;
using GBC;
using GrimoraMod.Saving;
using HarmonyLib;
using InscryptionAPI.Ascension;
using UnityEngine;
using static GrimoraMod.GrimoraPlugin;

namespace GrimoraMod
{


	[HarmonyPatch]
	internal static class StatManagement
	{

		[HarmonyPatch(typeof(AscensionRunEndScreen), nameof(AscensionRunEndScreen.Initialize))]
		[HarmonyPostfix]
		private static void GrimoraInitialize(bool victory, AscensionRunEndScreen __instance)
		{
			if (GrimoraSaveUtil.IsGrimoraModRun)
			{
				__instance.backgroundSpriteRenderer.sprite = (victory ? AssetUtils.GetPrefab<Sprite>("grimgorvictoryfolder") : AssetUtils.GetPrefab<Sprite>("grimgordefeat"));
			}
		}

	}

}
