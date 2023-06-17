using DiskCardGame;
using HarmonyLib;
using Sirenix.Utilities;
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
	public static void PrefixChangeEmissionColorBasedOnModSingletonId(GravestoneRenderStatsLayer __instance, ref CardRenderInfo info)
	{

		//Emission Colors in render order
		//Has Imbued? Does not glow until it has 1 attack
		//Has gotten Bonelord Buff, is bonelord or name is bonelordshorn > Red
		//Has gotten Boneyard Buff, or is Hellhound > Lime
		//Has gotten Electric Chair Buff > Blue

		if (info.baseInfo.HasBeenBonelorded() || info.baseInfo.name == GrimoraPlugin.NameBonelord || info.baseInfo.name == GrimoraPlugin.NameBoneLordsHorn || info.baseInfo.name == GrimoraPlugin.NameSpectre)
		{
			__instance.SetEmissionColor(GameColors.Instance.glowRed);
		}
		else if (info.baseInfo.HasBeenGraveDug() || info.baseInfo.name == GrimoraPlugin.NameHellHound)
		{
			__instance.SetEmissionColor(GameColors.Instance.darkLimeGreen);
		}
		else if (info.baseInfo.HasBeenElectricChaired())
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
		if (__instance && __instance.transform.parent.Find("CardStatIcons_Invisible").SafeIsUnityNull())
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
