using APIPlugin;
using DiskCardGame;
using HarmonyLib;

namespace GrimoraMod;

public class BuffCrewMates : AbilityBehaviour
{
	public static Ability ability;

	public override Ability Ability => ability;

	public static NewAbility Create()
	{
		const string rulebookDescription = "[creature] empowers each Skeleton, providing a +1 buff their power.";

		return ApiUtils.CreateAbility<BuffCrewMates>(rulebookDescription);
	}
}

[HarmonyPatch(typeof(PlayableCard))]
public class BuffCrewMatesPatch
{
	[HarmonyPostfix, HarmonyPatch(nameof(PlayableCard.GetPassiveAttackBuffs))]
	public static void Postfix(PlayableCard __instance, ref int __result)
	{
		if (__instance.OnBoard)
		{
			if (__instance.InfoName() == GrimoraPlugin.NameSkeleton)
			{
				GrimoraPlugin.Log.LogDebug($"[CrewMates] Result before [{__result}]");
				__result += BoardManager.Instance.GetSlots(!__instance.OpponentCard)
					.Count(slot => slot.CardHasAbility(BuffCrewMates.ability));
				GrimoraPlugin.Log.LogDebug($"[CrewMates] Result after [{__result}]");
			}
		}
	}
}
