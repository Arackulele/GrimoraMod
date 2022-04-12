using DiskCardGame;
using HarmonyLib;
using UnityEngine;

namespace GrimoraMod;

[HarmonyPatch(typeof(CardSpawner))]
public class CardSpawnerPatches
{
	[HarmonyPrefix, HarmonyPatch(nameof(CardSpawner.SpawnPlayableCardWithCopiedMods))]
	public static void CheckForLooseLimb(CardInfo info, PlayableCard copyFrom, ref Ability excludedAbility, ref PlayableCard __result)
	{
		if (GrimoraSaveUtil.isGrimora && excludedAbility == Ability.TailOnHit)
		{
			GrimoraPlugin.Log.LogDebug($"[SpawnPlayableCardWithCopiedMods] Changing TailOnHit ability to Loose Limb");
			excludedAbility = LooseLimb.ability;
		}
	}

	[HarmonyPostfix, HarmonyPatch(nameof(CardSpawner.SpawnPlayableCard))]
	public static void AddCustomAttackPrefabs(CardInfo info, ref PlayableCard __result)
	{
		if (GrimoraSaveUtil.isGrimora)
		{
			if (__result.transform.Find("Skeleton2ArmsAttacks").IsNull())
			{
				Animator skeletonArm2Attacks = UnityObject.Instantiate(
						AssetUtils.GetPrefab<GameObject>("Skeleton2ArmsAttacks"),
						__result.transform
					)
				 .GetComponent<Animator>();
				skeletonArm2Attacks.name = "Skeleton2ArmsAttacks";
				skeletonArm2Attacks.gameObject.AddComponent<AnimMethods>();
				skeletonArm2Attacks.gameObject.SetActive(false);
			}
			
			if (info.HasAbility(Ability.Sniper) && __result.transform.Find("Grimora_Sentry").IsNull())
			{
				GrimoraPlugin.Log.LogDebug($"Spawning new sentry prefab for card [{info.displayedName}]");
				GameObject skeletonArmSentry = UnityObject.Instantiate(
					AssetUtils.GetPrefab<GameObject>("Grimora_Sentry"),
					__result.transform
				);
				skeletonArmSentry.name = "Grimora_Sentry";
				Transform animObj = skeletonArmSentry.transform.GetChild(0);
				animObj.gameObject.AddComponent<AnimMethods>();
				animObj.gameObject.SetActive(false);
			}

			if (__result.Info.Mods.Exists(mod => mod.fromCardMerge))
			{
				GrimoraPlugin.Log.LogDebug($"[SpawnPlayableCard] Card [{info.displayedName}] has FromCardMerge mod, setting fromCardMerge to false");
				__result.Info.Mods.FindAll(mods => mods.fromCardMerge)
				 .ForEach(mod => mod.fromCardMerge = false);
			}
		}
	}
}
