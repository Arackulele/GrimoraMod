using DiskCardGame;
using HarmonyLib;
using UnityEngine;

namespace GrimoraMod;

[HarmonyPatch(typeof(CardSpawner))]
public class CardSpawnerPatches
{
	[HarmonyPostfix, HarmonyPatch(nameof(CardSpawner.SpawnPlayableCard))]
	public static void Postfix(CardInfo info, ref PlayableCard __result)
	{
		if (GrimoraSaveUtil.isGrimora && __result.transform.Find("Skeleton2ArmsAttacks").IsNull())
		{
			Animator skeletonArm2Attacks = UnityObject.Instantiate(
					AssetUtils.GetPrefab<GameObject>("Skeleton2ArmsAttacks"),
					__result.transform
				)
			 .GetComponent<Animator>();
			skeletonArm2Attacks.name = "Skeleton2ArmsAttacks";
			skeletonArm2Attacks.gameObject.AddComponent<AnimMethods>();
			skeletonArm2Attacks.gameObject.SetActive(false);

			if (info.HasAbility(Ability.Sniper))
			{
				GrimoraPlugin.Log.LogDebug($"Spawning new sentry prefab for card [{info.displayedName}]");
				Animator skeletonArmSentry = UnityObject.Instantiate(
						AssetUtils.GetPrefab<GameObject>("Grimora_Sentry"),
						__result.transform
					)
				 .GetComponent<Animator>();
				skeletonArmSentry.name = "SkeletonArms_Sentry";
				skeletonArmSentry.gameObject.AddComponent<AnimMethods>();
				skeletonArmSentry.gameObject.SetActive(false);
			}
		}
	}
}
