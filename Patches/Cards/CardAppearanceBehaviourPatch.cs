using DiskCardGame;
using HarmonyLib;
using UnityEngine;
using static GrimoraMod.GrimoraPlugin;

namespace GrimoraMod;

[HarmonyPatch]
public class CardAppearanceBehaviourPatch
{
	public static readonly Material GravestoneGold = AssetUtils.GetPrefab<Material>("GravestoneCardBack_Rare");

	public static readonly Material GravestoneSlate = AssetUtils.GetPrefab<Material>("Gravestone_Gold");

	public static readonly Material GravestoneTerrain = AssetUtils.GetPrefab<Material>("GravestoneTerrain");

	public static readonly Material GravestoneFrozen = AssetUtils.GetPrefab<Material>("GravestoneFrozen");

	[HarmonyPrefix, HarmonyPatch(typeof(RareCardBackground), nameof(RareCardBackground.ApplyAppearance))]
	public static bool CorrectBehaviourForGrimora(ref RareCardBackground __instance)
	{
		if (GrimoraSaveUtil.IsNotGrimoraModRun)
		{
			return true;
		}

		var renderer = __instance.Card.GetComponentInChildren<GravestoneRenderStatsLayer>();
		if (renderer != null)
		{
			renderer.Material.SetAlbedoTexture(GravestoneGold.mainTexture);
		}

		return false;
	}

	[HarmonyPrefix, HarmonyPatch(typeof(TerrainBackground), nameof(TerrainBackground.ApplyAppearance))]
	public static bool CorrectTerrainBehaviourForGrimora(ref TerrainBackground __instance)
	{
		if (GrimoraSaveUtil.IsNotGrimoraModRun)
		{
			return true;
		}

		var renderer = __instance.Card.GetComponentInChildren<GravestoneRenderStatsLayer>();
		if (renderer != null)
		{
			renderer.Material.SetAlbedoTexture(GravestoneTerrain.mainTexture);
		}

		return false;
	}

	[HarmonyPrefix, HarmonyPatch(typeof(GoldEmission), nameof(GoldEmission.ApplyAppearance))]
	public static bool CorrectIceBehaviourForGrimora(ref GoldEmission __instance)
	{
		if (GrimoraSaveUtil.IsNotGrimoraModRun)
		{
			return true;
		}

		var renderer = __instance.Card.GetComponentInChildren<GravestoneRenderStatsLayer>();
		if (renderer != null)
		{
			renderer.Material.SetAlbedoTexture(GravestoneFrozen.mainTexture);
		}

		return false;
	}

	[HarmonyPrefix, HarmonyPatch(typeof(HologramPortrait), nameof(HologramPortrait.ApplyAppearance))]
	public static bool CorrectIceBehaviourForGrimora(ref HologramPortrait __instance)
	{
		if (GrimoraSaveUtil.IsNotGrimoraModRun)
		{
			return true;
		}

		var renderer = __instance.Card.GetComponentInChildren<GravestoneRenderStatsLayer>();
		if (renderer != null)
		{
			renderer.Material.SetAlbedoTexture(GravestoneSlate.mainTexture);
		}

		return false;
	}
}
