using DiskCardGame;
using HarmonyLib;
using UnityEngine;

namespace GrimoraMod;

[HarmonyPatch(typeof(GravestoneRenderStatsLayer))]
public class GravestoneRenderStatsLayerPatches
{
	private static readonly CardStatIcons CardStatIcons = ResourceBank.Get<CardStatIcons>(
		"Prefabs/Cards/CardSurfaceInteraction/CardStatIcons_Invisible"
	);

	private static readonly GameObject EnergyCellsLeft = AssetUtils.GetPrefab<GameObject>("EnergyCells_Left");
	private static readonly GameObject EnergyCellsRight = AssetUtils.GetPrefab<GameObject>("EnergyCells_Right");

	[HarmonyPostfix, HarmonyPatch(nameof(GravestoneRenderStatsLayer.RenderCard))]
	public static void PrefixChangeEmissionColorBasedOnModSingletonId(
		GravestoneRenderStatsLayer __instance,
		ref CardRenderInfo info
	)
	{
		if (info.baseInfo.HasBeenElectricChaired())
		{
			__instance.SetEmissionColor(GameColors.Instance.blue);
		}

		if (info.baseInfo.HasAbility(Imbued.ability) && info.attack == 0)
		{
			__instance.SetEmissionColor(GrimoraColors.AlphaZeroBlack);
		}
	}

	[HarmonyPrefix, HarmonyPatch(nameof(GravestoneRenderStatsLayer.RenderCard))]
	public static void PrefixAddStatIcons(GravestoneRenderStatsLayer __instance, CardRenderInfo info)
	{
		if (__instance.transform.parent.Find("CardStatIcons_Invisible").IsNull())
		{
			CardStatIcons statIcons = UnityObject.Instantiate(
				CardStatIcons,
				__instance.transform.parent
			);
			statIcons.name = "CardStatIcons_Invisible";
			statIcons.transform.localPosition = new Vector3(-0.11f, -0.72f, 0f);

			if (__instance.PlayableCard)
			{
				__instance.PlayableCard.statIcons = statIcons;
			}
			else if (__instance.GetComponentInParent<SelectableCard>())
			{
				__instance.GetComponentInParent<SelectableCard>().statIcons = statIcons;
			}
		}
	}

	[HarmonyPostfix, HarmonyPatch(nameof(GravestoneRenderStatsLayer.RenderCard))]
	public static void AddEnergyCellsToCards(GravestoneRenderStatsLayer __instance, CardRenderInfo info)
	{
		// RenderStatsLayer in the prefab has zero children
		if (info is { energyCost: > 0 } && __instance && __instance.transform.childCount == 0)
		{
			int energyCost = info.energyCost;
			MeshRenderer energyCellsLeft = UnityObject.Instantiate(
					EnergyCellsLeft,
					__instance.gameObject.transform
				)
			 .GetComponent<MeshRenderer>();

			MeshRenderer energyCellsRight = null;
			if (energyCost > 3)
			{
				energyCellsRight = UnityObject.Instantiate(
						EnergyCellsRight,
						__instance.gameObject.transform
					)
				 .GetComponent<MeshRenderer>();
			}

			UpdateEnergyCost(energyCost, energyCellsLeft, energyCellsRight);
		}
	}

	private static void UpdateEnergyCost(int energyCost, Renderer energyCellsLeft, Renderer energyCellsRight = null)
	{
		int energyCellsLeftLength = energyCellsLeft.materials.Length;
		for (int i = 0; i < energyCellsLeftLength; i++)
		{
			if (i < energyCost)
			{
				energyCellsLeft.materials[energyCellsLeftLength - i - 1].color = GrimoraColors.GrimoraEnergyCost;
			}
			else
			{
				Material material = energyCellsLeft.materials[energyCellsLeftLength - i - 1];
				material.ChangeRenderMode(UnityObjectExtensions.BlendMode.Cutout);
			}
		}

		if (energyCellsRight)
		{
			int energyCellsRightLength = energyCellsRight.materials.Length;
			for (int i = 0; i < energyCellsRightLength; i++)
			{
				if (i < energyCost - 3)
				{
					energyCellsRight.materials[energyCellsRightLength - i - 1].color = GrimoraColors.GrimoraEnergyCost;
				}
				else
				{
					Material material = energyCellsRight.materials[energyCellsRightLength - i - 1];
					material.ChangeRenderMode(UnityObjectExtensions.BlendMode.Cutout);
				}
			}
		}
	}
}
