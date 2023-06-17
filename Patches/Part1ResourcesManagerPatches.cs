using DiskCardGame;
using HarmonyLib;

namespace GrimoraMod;

[HarmonyPatch(typeof(Part1ResourcesManager))]
public class Part1ResourcesManagerPatches
{

	[HarmonyPostfix, HarmonyPatch(nameof(Part1ResourcesManager.ShowAddBones))]
	public static void SetTokensAsChildrenOfBoneTokenArea(Part1ResourcesManager __instance, int amount, CardSlot slot)
	{
		foreach (var boneToken in __instance.boneTokens.Where(boneToken => boneToken.transform.parent != __instance.boneTokenArea))
		{
			boneToken.transform.SetParent(__instance.boneTokenArea);
		}
	}
}
