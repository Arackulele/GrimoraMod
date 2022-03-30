using DiskCardGame;
using HarmonyLib;
using Pixelplacement;
using UnityEngine;

namespace GrimoraMod;

[HarmonyPatch(typeof(CombatPhaseManager))]
public class VisualizeSniperPatches
{
	private static Animator skeletonArmSniper = null;
	private static GameObject sniperIconPrefab = null;
	private static List<GameObject> sniperIcons = new();
	
	[HarmonyPrefix, HarmonyPatch(nameof(CombatPhaseManager.VisualizeStartSniperAbility))]
	public static bool VisualizeStartSniperAbilityGrimora(CardSlot sniperSlot)
	{
		if (GrimoraSaveUtil.isNotGrimora)
		{
			return true;
		}

		skeletonArmSniper = sniperSlot.transform.Find("SkeletonArms_Sentry").GetComponent<Animator>();
		skeletonArmSniper.Play("sniper_hold", 0, 0);
		return false;
	}
	
	[HarmonyPrefix, HarmonyPatch(nameof(CombatPhaseManager.VisualizeAimSniperAbility))]
	public static bool VisualizeAimForGrimoraGrimora(CardSlot sniperSlot, CardSlot targetSlot)
	{
		if (GrimoraSaveUtil.isNotGrimora)
		{
			return true;
		}
		
		Tween.LookAt(skeletonArmSniper.transform, targetSlot.transform.position, Vector3.up, 0.075f, 0f, Tween.EaseInOut);
		return false;
	}
	
	[HarmonyPrefix, HarmonyPatch(nameof(CombatPhaseManager.VisualizeConfirmSniperAbility))]
	public static bool VisualizeConfirmSniperAbilityGrimora(CombatPhaseManager __instance, CardSlot targetSlot)
	{
		if (GrimoraSaveUtil.isNotGrimora)
		{
			return true;
		}
		
		if (sniperIconPrefab == null)
		{
			sniperIconPrefab = ResourceBank.Get<GameObject>("Prefabs/Cards/SpecificCardModels/SniperTargetIcon");
		}
		GameObject gameObject = UnityObject.Instantiate(sniperIconPrefab, targetSlot.transform);
		gameObject.transform.localPosition = new Vector3(0f, 0.25f, 0f);
		gameObject.transform.localRotation = Quaternion.identity;
		sniperIcons.Add(gameObject);
		return false;
	}
	
	[HarmonyPrefix, HarmonyPatch(nameof(CombatPhaseManager.VisualizeClearSniperAbility))]
	public static bool VisualizeClearSniperAbilityGrimora()
	{
		if (GrimoraSaveUtil.isNotGrimora)
		{
			return true;
		}
		
		sniperIcons.ForEach(delegate(GameObject x)
		{
			UnityObject.Destroy(x, 0.1f);
		});
		sniperIcons.Clear();
		return false;
	}
}
