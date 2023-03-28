using DiskCardGame;
using HarmonyLib;
using UnityEngine;
using static GrimoraMod.GrimoraPlugin;

namespace GrimoraMod;

[HarmonyPatch(typeof(CardAbilityIcons))]
public class ChangeLogicInCardAbilityIcons
{
	[HarmonyPrefix, HarmonyPatch(nameof(CardAbilityIcons.UpdateAbilityIcons))]
	public static void PrefixTest(
		CardAbilityIcons __instance,
		CardInfo info,
		List<CardModificationInfo> mods,
		PlayableCard playableCard,
		List<Ability> hiddenAbilities
	)
	{
		Log.LogInfo($"[ChangeLogicInCardAbilityIcons] GrimoraSaveUtil.IsNotGrimoraModRun " + GrimoraSaveUtil.IsNotGrimoraModRun);
		if (GrimoraSaveUtil.IsNotGrimoraModRun || TurnManager.Instance.GameEnding)
		{
			return;
		}

		SetupGroupTwo(__instance);

		SetupGroupThree(__instance);

		SetupGroupFour(__instance);
		
		SetupGroupFive(__instance);
	}
	
	private static readonly Vector3 Group2Scale = new Vector3(0.35f, 0.35f, 1);

	private static void SetupGroupTwo(CardAbilityIcons __instance)
	{
		var group2Icons = __instance.defaultIconGroups[1].GetComponentsInChildren<AbilityIconInteractable>();
		var icon1 = group2Icons[0].transform;
		icon1.localPosition = new Vector3(-0.2f, icon1.localPosition.y, icon1.localPosition.z);
		icon1.localScale = Group2Scale;

		var icon2 = group2Icons[1].transform;
		icon2.localPosition = new Vector3(0.2f, icon2.localPosition.y, icon2.localPosition.z);
		icon2.localScale = Group2Scale;
	}

	private static readonly Vector3 Group3Scale = new Vector3(0.325f, 0.325f, 1);

	private static void SetupGroupThree(CardAbilityIcons __instance)
	{
		float startX = -0.3f;
		var group3Icons = __instance.defaultIconGroups[2].GetComponentsInChildren<AbilityIconInteractable>();
		foreach (var icon in group3Icons)
		{
			var iconTransform = icon.transform;
			var localPosition = iconTransform.localPosition;
			iconTransform.localPosition = new Vector3(startX, localPosition.y, localPosition.z);
			startX += 0.3f;
			iconTransform.localScale = Group3Scale;
		}
	}

	private static readonly Vector3 Group4Scale = new Vector3(0.2875f, 0.2875f, 1f);

	private static void SetupGroupFour(CardAbilityIcons __instance)
	{
		var group4Icons = __instance.defaultIconGroups[3].GetComponentsInChildren<AbilityIconInteractable>();
		foreach (var icon in group4Icons)
		{
			icon.transform.localScale = Group4Scale;
		}

		var icon1 = group4Icons[0];
		Vector3 localPos = icon1.transform.localPosition;
		icon1.transform.localPosition = new Vector3(-0.325f, 0.075f, localPos.z);

		var icon2 = group4Icons[1];
		localPos = icon2.transform.localPosition;
		icon2.transform.localPosition = new Vector3(0, 0.075f, localPos.z);

		var icon3 = group4Icons[2];
		localPos = icon3.transform.localPosition;
		icon3.transform.localPosition = new Vector3(0.325f, 0.075f, localPos.z);

		var icon4 = group4Icons[3];
		localPos = icon4.transform.localPosition;
		icon4.transform.localPosition = new Vector3(0, -0.2f, localPos.z);
	}
	
	public static void SetupGroupFive(CardAbilityIcons cardAbilityIcons)
	{
		if (cardAbilityIcons.defaultIconGroups.Count <= 4)
		{
			GameObject group5 = UnityObject.Instantiate(cardAbilityIcons.defaultIconGroups[3], cardAbilityIcons.transform);
			group5.name = "DefaultIcons_5Abilities";
			cardAbilityIcons.defaultIconGroups.Add(group5);
			
			AbilityIconInteractable icon5 = UnityObject.Instantiate(group5.GetComponentInChildren<AbilityIconInteractable>(), group5.transform);
			icon5.name = "Ability_5";

			var group5Icons = group5.GetComponentsInChildren<AbilityIconInteractable>();
			foreach (var icon in group5Icons)
			{
				icon.transform.localScale = Group4Scale;
			}

			var icon1 = group5Icons[0];
			Vector3 localPos = icon1.transform.localPosition;
			icon1.transform.localPosition = new Vector3(-0.325f, 0.075f, localPos.z);

			var icon2 = group5Icons[1];
			localPos = icon2.transform.localPosition;
			icon2.transform.localPosition = new Vector3(0, 0.075f, localPos.z);

			var icon3 = group5Icons[2];
			localPos = icon3.transform.localPosition;
			icon3.transform.localPosition = new Vector3(0.325f, 0.075f, localPos.z);

			var icon4 = group5Icons[3];
			localPos = icon4.transform.localPosition;
			icon4.transform.localPosition = new Vector3(-0.175f, -0.2f, localPos.z);
			
			localPos = icon5.transform.localPosition;
			icon5.transform.localPosition = new Vector3(0.175f, -0.2f, localPos.z);
		}
	}
}
