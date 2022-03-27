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
			 .AddComponent<Animator>();
			skeletonArm2Attacks.name = "Skeleton2ArmsAttacks";
			skeletonArm2Attacks.runtimeAnimatorController = AssetConstants.SkeletonArmController;
			skeletonArm2Attacks.gameObject.AddComponent<AnimMethods>();
			skeletonArm2Attacks.gameObject.SetActive(false);
		}
	}
}
