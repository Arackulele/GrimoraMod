using DiskCardGame;
using HarmonyLib;
using UnityEngine;

namespace GrimoraMod;

[HarmonyPatch(typeof(GravestoneRenderStatsLayer))]
public class GravestoneRenderStatsLayerPatches
{
	private static readonly Color GrimoraTextColor = new(0.420f, 1f, 0.63f, 0.25f);
	private static readonly GameObject EnergyCellsLeft = AssetUtils.GetPrefab<GameObject>("EnergyCells_Left");
	private static readonly GameObject EnergyCellsRight = AssetUtils.GetPrefab<GameObject>("EnergyCells_Right");

	[HarmonyPostfix, HarmonyPatch(nameof(GravestoneRenderStatsLayer.RenderCard))]
	public static void AddEnergyCellsToCards(GravestoneRenderStatsLayer __instance, CardRenderInfo info)
	{
		if (__instance.transform.childCount == 0)
		{
			int energyCost = info.energyCost;
			if (energyCost > 0)
			{
				MeshRenderer energyCellsLeft = Object.Instantiate(
					EnergyCellsLeft, __instance.gameObject.transform
				).GetComponent<MeshRenderer>();

				MeshRenderer energyCellsRight = null;
				if (energyCost > 3)
				{
					energyCellsRight = Object.Instantiate(
						EnergyCellsRight, __instance.gameObject.transform
					).GetComponent<MeshRenderer>();
				}

				UpdateEnergyCost(energyCost, energyCellsLeft, energyCellsRight);
			}
		}
	}

	private static void UpdateEnergyCost(int energyCost, Renderer energyCellsLeft, Renderer energyCellsRight)
	{
		int energyCellsLeftLength = energyCellsLeft.materials.Length;
		for (int i = 0; i < energyCellsLeftLength; i++)
		{
			Color value = i < energyCost ? GrimoraTextColor : GrimoraColors.AlphaZeroBlack;
			energyCellsLeft.materials[energyCellsLeftLength - i - 1].color = value;
		}

		if (energyCellsRight is not null)
		{
			int energyCellsRightLength = energyCellsRight.materials.Length;
			for (int i = 0; i < energyCellsRightLength; i++)
			{
				Color value = i < energyCost - 3 ? GrimoraTextColor : GrimoraColors.AlphaZeroBlack;
				GrimoraPlugin.Log.LogDebug($"[UpdateEnergyCost] Setting color [{value}]");
				energyCellsRight.materials[energyCellsRightLength - i - 1].color = value;
			}
		}
	}
}
