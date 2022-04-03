using DiskCardGame;
using HarmonyLib;
using UnityEngine;
using UnityEngine.Rendering;

namespace GrimoraMod;

[HarmonyPatch(typeof(GravestoneRenderStatsLayer))]
public class GravestoneRenderStatsLayerPatches
{
	private static readonly CardStatIcons CardStatIcons = ResourceBank.Get<CardStatIcons>(
		"Prefabs/Cards/CardSurfaceInteraction/CardStatIcons_Invisible"
	);

	private static readonly GameObject EnergyCellsLeft = AssetUtils.GetPrefab<GameObject>("EnergyCells_Left");
	private static readonly GameObject EnergyCellsRight = AssetUtils.GetPrefab<GameObject>("EnergyCells_Right");
	private static readonly int SrcBlend = Shader.PropertyToID("_SrcBlend");
	private static readonly int DstBlend = Shader.PropertyToID("_DstBlend");
	private static readonly int ZWrite = Shader.PropertyToID("_ZWrite");

	[HarmonyPostfix, HarmonyPatch(nameof(GravestoneRenderStatsLayer.RenderCard))]
	public static void PrefixChangeEmissionColorBasedOnModSingletonId(
		ref GravestoneRenderStatsLayer __instance,
		ref CardRenderInfo info
	)
	{
		PlayableCard playableCard = __instance.PlayableCard;
		SelectableCard selectableCard = __instance.GetComponentInParent<SelectableCard>();
		if (playableCard && playableCard.HasBeenElectricChaired() || selectableCard && selectableCard.Info.HasBeenElectricChaired())
		{
			__instance.SetEmissionColor(GameColors.Instance.blue);
		}

		HandleImbuedAbility(__instance, playableCard, selectableCard);
	}

	private static void HandleElectricChair(
		GravestoneRenderStatsLayer statsLayer,
		PlayableCard playableCard,
		SelectableCard selectableCard
	)
	{
		if (playableCard && playableCard.HasBeenElectricChaired() 
		 || selectableCard && selectableCard.Info.HasBeenElectricChaired())
		{
			statsLayer.SetEmissionColor(GameColors.Instance.blue);
		}
	}

	private static void HandleImbuedAbility(
		GravestoneRenderStatsLayer statsLayer, 
		PlayableCard playableCard, 
		SelectableCard selectableCard
		)
	{
		if (playableCard && playableCard.HasAbility(Imbued.ability)
		 || selectableCard && selectableCard.Info.HasAbility(Imbued.ability))
		{
			if (playableCard && playableCard.Attack == 0)
			{
				statsLayer.SetEmissionColor(GrimoraColors.AlphaZeroBlack);
			}
			else if (selectableCard && selectableCard.Info.Attack == 0)
			{
				statsLayer.SetEmissionColor(GrimoraColors.AlphaZeroBlack);
			}
		}
	}

	[HarmonyPrefix, HarmonyPatch(nameof(GravestoneRenderStatsLayer.RenderCard))]
	public static void PrefixAddStatIcons(ref GravestoneRenderStatsLayer __instance, CardRenderInfo info)
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
		if (__instance.transform.childCount == 0)
		{
			int energyCost = info.energyCost;
			if (energyCost > 0)
			{
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
				material.SetInt(SrcBlend, (int)BlendMode.One);
				material.SetInt(DstBlend, (int)BlendMode.Zero);
				material.SetInt(ZWrite, 1);
				material.EnableKeyword("_ALPHATEST_ON");
				material.DisableKeyword("_ALPHABLEND_ON");
				material.DisableKeyword("_ALPHAPREMULTIPLY_ON");
				material.renderQueue = 2450;
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
					material.SetInt(SrcBlend, (int)BlendMode.One);
					material.SetInt(DstBlend, (int)BlendMode.Zero);
					material.SetInt(ZWrite, 1);
					material.EnableKeyword("_ALPHATEST_ON");
					material.DisableKeyword("_ALPHABLEND_ON");
					material.DisableKeyword("_ALPHAPREMULTIPLY_ON");
					material.renderQueue = 2450;
				}
			}
		}
	}
}
