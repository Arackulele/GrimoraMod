using DiskCardGame;
using HarmonyLib;
using InscryptionAPI.Card;
using Sirenix.Utilities;
using UnityEngine;

namespace GrimoraMod;

[HarmonyPatch(typeof(GravestoneRenderStatsLayer))]
public class GravestoneRenderStatsLayerPatches
{
	private static readonly CardStatIcons CardStatIcons = ResourceBank.Get<CardStatIcons>(
		"Prefabs/Cards/CardSurfaceInteraction/CardStatIcons_Invisible"
	);

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
		else if (info.baseInfo.HasBeenGraveDug() || info.baseInfo.name == GrimoraPlugin.NameHellHound || info.baseInfo.name == GrimoraPlugin.NameCandyMonster)
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


	[HarmonyPatch(typeof(GravestoneRenderStatsLayer), nameof(GravestoneRenderStatsLayer.RenderCard))]
	public static class MoxColoredEmissionsPatch
	{
		[HarmonyPostfix]
		public static void PrefixRessourceManager(GravestoneRenderStatsLayer __instance, ref CardRenderInfo info)
		{
			GrimoraPlugin.Log.LogInfo("adding RessourceManager to Card");

			GameObject ResourceManager;
			if (__instance.gameObject.transform.Find("ResourceManager") == null) ResourceManager = UnityObject.Instantiate(GrimoraPlugin.NewObjects.Find(g => g.name.Contains("GraveStoneRessource")), __instance.gameObject.transform);
			else ResourceManager = __instance.gameObject.transform.Find("ResourceManager").gameObject;
			ResourceManager.name = "ResourceManager";

			GrimoraPlugin.Log.LogInfo(" RessourceManager added to Card");


			GameObject EnergyManager = ResourceManager.transform.Find("EnergyManager").gameObject;

			GameObject EnergyCell1 = EnergyManager.transform.Find("Energy1").gameObject;
			GameObject EnergyCell2 = EnergyManager.transform.Find("Energy2").gameObject;
			GameObject EnergyCell3 = EnergyManager.transform.Find("Energy3").gameObject;
			GameObject EnergyCell4 = EnergyManager.transform.Find("Energy4").gameObject;
			GameObject EnergyCell5 = EnergyManager.transform.Find("Energy5").gameObject;
			GameObject EnergyCell6 = EnergyManager.transform.Find("Energy6").gameObject;

			GrimoraPlugin.Log.LogInfo(" Energy children found");

			GameObject MoxManager = ResourceManager.transform.Find("MoxManager").gameObject;

			GameObject OrangeGem = MoxManager.transform.Find("GemDiamond").gameObject;
			GameObject BlueGem = MoxManager.transform.Find("GemSapphire").gameObject;
			GameObject GreenGem = MoxManager.transform.Find("GemEmerald").gameObject;

			GrimoraPlugin.Log.LogInfo(" Gem children found");

			GameObject BloodManager = ResourceManager.transform.Find("BloodManager").gameObject;

			GameObject Blood1 = BloodManager.transform.Find("Blood1").gameObject;
			GameObject Blood2 = BloodManager.transform.Find("Blood2").gameObject;
			GameObject Blood3 = BloodManager.transform.Find("Blood3").gameObject;
			GameObject Blood4 = BloodManager.transform.Find("Blood4").gameObject;
			GrimoraPlugin.Log.LogInfo(" Blood children found");

			GrimoraPlugin.Log.LogInfo(" disabling everything");
			foreach (Transform transform in EnergyManager.transform)
			{
				transform.gameObject.SetActive(false);
			}
			foreach (Transform transform in MoxManager.transform)
			{
				transform.gameObject.SetActive(false);
			}
			foreach (Transform transform in BloodManager.transform)
			{
				transform.gameObject.SetActive(false);
			}
			GrimoraPlugin.Log.LogInfo("everything disabled");

			UpdateEnergyCost(EnergyManager, info.energyCost, info, __instance);
			UpdateBloodCost(BloodManager, info.baseInfo.BloodCost, info, __instance);
			UpdateMoxCost(MoxManager, info.baseInfo.GemsCost, info, __instance);


			//Mox Colored Emissions

			if (info.baseInfo.Abilities.Contains(Ability.GainGemOrange))
			{
				__instance.SetEmissionColor(GameColors.Instance.orange);
				info.baseInfo.AddTraits(Trait.Gem);
				__instance.Update();
			}
			else if (info.baseInfo.Abilities.Contains(Ability.GainGemBlue))
			{
				__instance.SetEmissionColor(GameColors.Instance.blue);
				info.baseInfo.AddTraits(Trait.Gem);
				__instance.Update();

			}
			else if (info.baseInfo.Abilities.Contains(Ability.GainGemGreen))
			{
				__instance.SetEmissionColor(GameColors.Instance.limeGreen);
				info.baseInfo.AddTraits(Trait.Gem);
				__instance.Update();

			}

		}

		public static void UpdateEnergyCost(GameObject gb, int energyCost, CardRenderInfo info, GravestoneRenderStatsLayer layer)
		{
			foreach (Transform transform in gb.transform)
			{
				transform.gameObject.SetActive(false);
			}
			GrimoraPlugin.Log.LogInfo(" checking for Energy availability");
			if (energyCost >= 1 && layer)
			{
				GrimoraPlugin.Log.LogInfo("Calculating Energy Stuff");
				GrimoraPlugin.Log.LogInfo("energy: " + energyCost);

				GrimoraPlugin.Log.LogInfo("adding Energy");
				if (energyCost >= 1)
				{
					gb.transform.Find("Energy1").gameObject.SetActive(true);
				}
				if (energyCost >= 2)
				{
					gb.transform.Find("Energy2").gameObject.SetActive(true);
				}
				if (energyCost >= 3)
				{
					gb.transform.Find("Energy3").gameObject.SetActive(true);
				}
				if (energyCost >= 4)
				{
					gb.transform.Find("Energy4").gameObject.SetActive(true);
				}
				if (energyCost >= 5)
				{
					gb.transform.Find("Energy5").gameObject.SetActive(true);
				}
				if (energyCost > 5)
				{
					gb.transform.Find("Energy6").gameObject.SetActive(true);
				}
			}
		}
		private static void UpdateBloodCost(GameObject gb, int bloodCost, CardRenderInfo info, GravestoneRenderStatsLayer layer)
		{
			foreach (Transform transform in gb.transform)
			{
				transform.gameObject.SetActive(false);
			}
			GrimoraPlugin.Log.LogInfo(" checking for Blood availability");
			if (info.baseInfo.cost >= 1 && layer)
			{
				GrimoraPlugin.Log.LogInfo("Calculating Blood Stuff");
				GrimoraPlugin.Log.LogInfo("energy: " + bloodCost);

				GrimoraPlugin.Log.LogInfo("adding Blood");
				if (bloodCost >= 1)
				{
					gb.transform.Find("Blood1").gameObject.SetActive(true);
				}
				if (bloodCost >= 2)
				{
					gb.transform.Find("Blood2").gameObject.SetActive(true);
				}
				if (bloodCost >= 3)
				{
					gb.transform.Find("Blood3").gameObject.SetActive(true);
				}
				if (bloodCost > 3)
				{
					gb.transform.Find("Blood4").gameObject.SetActive(true);
				}
			}
		}
		private static void UpdateMoxCost(GameObject gb, List<GemType> gemCost, CardRenderInfo info, GravestoneRenderStatsLayer layer)
		{
			foreach (Transform transform in gb.transform)
			{
				transform.gameObject.SetActive(false);
			}
			GrimoraPlugin.Log.LogInfo(" checking for Gem availability");
			if (info.baseInfo.gemsCost.Contains(GemType.Orange) || info.baseInfo.gemsCost.Contains(GemType.Blue) || info.baseInfo.gemsCost.Contains(GemType.Green) && layer)
			{
				GrimoraPlugin.Log.LogInfo("Calculating Gem Stuff");
				foreach (GemType gem in gemCost)
				{
					GrimoraPlugin.Log.LogInfo("Gem:" + gem);
				}

				GrimoraPlugin.Log.LogInfo("adding Gems");
				if (gemCost.Contains(GemType.Orange))
				{
					gb.transform.Find("GemDiamond").gameObject.SetActive(true);
				}
				if (gemCost.Contains(GemType.Blue))
				{
					gb.transform.Find("GemSapphire").gameObject.SetActive(true);
				}
				if (gemCost.Contains(GemType.Green))
				{
					gb.transform.Find("GemEmerald").gameObject.SetActive(true);
				}
			}
		}
	}
}
