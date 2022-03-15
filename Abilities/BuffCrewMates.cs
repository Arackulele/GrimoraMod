using DiskCardGame;
using HarmonyLib;
using InscryptionAPI.Card;

namespace GrimoraMod;

public class BuffCrewMates : AbilityBehaviour
{
	public static Ability ability;

	public override Ability Ability => ability;

	public static AbilityManager.FullAbility Create()
	{
		const string rulebookDescription =
			"[creature] empowers each Skeleton on the owner's side of the board, providing a +1 buff their power.";

		return ApiUtils.CreateAbility<BuffCrewMates>(rulebookDescription, "Sea Shanty");
	}
}

[HarmonyPatch(typeof(PlayableCard))]
public class BuffCrewMatesPatch
{
	[HarmonyPostfix, HarmonyPatch(nameof(PlayableCard.GetPassiveAttackBuffs))]
	public static void Postfix(PlayableCard __instance, ref int __result)
	{
		if (__instance.IsNotNull() && __instance.OnBoard)
		{
			if (__instance.InfoName() == GrimoraPlugin.NameSkeleton)
			{
				__result += BoardManager.Instance.GetSlots(!__instance.OpponentCard)
					.Count(slot => slot.CardIsNotNullAndHasAbility(BuffCrewMates.ability));
			}
		}
	}
}
