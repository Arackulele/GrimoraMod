using System.Collections;
using APIPlugin;
using DiskCardGame;
using HarmonyLib;
using static GrimoraMod.GrimoraPlugin;

namespace GrimoraMod;

public class Possessive : AbilityBehaviour
{
	public static Ability ability;
	public override Ability Ability => ability;

	public override bool RespondsToSlotTargetedForAttack(CardSlot slot, PlayableCard attacker)
	{
		return attacker.Slot == base.Card.Slot.opposingSlot;
	}

	public override IEnumerator OnSlotTargetedForAttack(CardSlot slot, PlayableCard attacker)
	{
		attacker.Anim.StrongNegationEffect();
		yield break;
	}

	public static NewAbility Create()
	{
		const string rulebookDescription =
			"[creature] cannot be attacked from the opposing slot. " +
			"The opposing slot instead attacks one of it's adjacent slots if possible.";

		return ApiUtils.CreateAbility<Possessive>(rulebookDescription);
	}
}

[HarmonyPatch]
public class PatchesForPossessive
{
	[HarmonyPostfix, HarmonyPatch(typeof(PlayableCard), nameof(PlayableCard.GetOpposingSlots))]
	public static void PossessiveGetOpposingSlotsPatch(PlayableCard __instance, ref List<CardSlot> __result)
	{
		if (__instance.Slot.opposingSlot.Card is not null
		    && __instance.Slot.opposingSlot.Card.HasAbility(Possessive.ability))
		{
			var adjSlots = BoardManager.Instance
				.GetAdjacentSlots(__instance.Slot)
				.Where(_ => _.Card is not null)
				.ToList();

			__result = new List<CardSlot>();
			if (adjSlots.IsNotEmpty())
			{
				CardSlot slotToTarget = adjSlots[UnityEngine.Random.RandomRangeInt(0, adjSlots.Count)];
				Log.LogDebug($"[OpposingPatches.Possessive] Slot targeted for attack [{slotToTarget.Index}]");
				__result.Add(slotToTarget);
			}
		}
	}
}
