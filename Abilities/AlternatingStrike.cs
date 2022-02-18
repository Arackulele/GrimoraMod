using System.Collections;
using APIPlugin;
using DiskCardGame;
using HarmonyLib;
using static GrimoraMod.GrimoraPlugin;

namespace GrimoraMod;

public class AlternatingStrike : AbilityBehaviour
{
	public static Ability ability;
	public override Ability Ability => ability;

	public bool isAttackingLeft = true;

	public override bool RespondsToSlotTargetedForAttack(CardSlot slot, PlayableCard attacker)
	{
		return attacker == base.Card;
	}

	public override IEnumerator OnSlotTargetedForAttack(CardSlot slot, PlayableCard attacker)
	{
		// if in far left slot, adj slot left will be null
		CardSlot slotToAttack = BoardManager.Instance.GetAdjacent(base.Card.Slot.opposingSlot, isAttackingLeft);

		if (slotToAttack is null)
		{
			isAttackingLeft = !isAttackingLeft;
			// if in far left slot and attacked right last, need to keep attack to the right slot
			Log.LogDebug($"[AlternatingStrike.Patch]" +
			             $" SlotToAttack is null. Changing [{isAttackingLeft}] to [{!isAttackingLeft}]");
		}

		yield return base.OnSlotTargetedForAttack(slot, attacker);
	}

	public override bool RespondsToAttackEnded()
	{
		return true;
	}

	public override IEnumerator OnAttackEnded()
	{
		isAttackingLeft = !isAttackingLeft;
		yield return base.OnAttackEnded();
	}

	public static NewAbility Create()
	{
		const string rulebookDescription =
			"[creature] alternates between striking the opposing space to the left and right from it.";

		return ApiUtils.CreateAbility<AlternatingStrike>(rulebookDescription);
	}
}

[HarmonyPatch]
public class PatchesForAlternatingStrike
{
	[HarmonyPostfix, HarmonyPatch(typeof(PlayableCard), nameof(PlayableCard.GetOpposingSlots))]
	public static void AlternatingStrikeGetOpposingSlotsPatch(PlayableCard __instance, ref List<CardSlot> __result)
	{
		if (__instance.HasAbility(AlternatingStrike.ability))
		{
			__result.Clear();
			bool isAttackingLeft = __instance.GetComponent<AlternatingStrike>().isAttackingLeft;
			CardSlot slotToAttack = BoardManager.Instance.GetAdjacent(__instance.Slot.opposingSlot, isAttackingLeft);
			if (slotToAttack is null)
			{
				// if in far left slot, adj slot left will be null
				// if in far left slot and attacked right last, need to keep attack to the right slot
				Log.LogDebug($"[AlternatingStrike.Patch]" +
				             $" SlotToAttack is null. Changing [{isAttackingLeft}] to [{!isAttackingLeft}]");
				slotToAttack = BoardManager.Instance.GetAdjacent(__instance.Slot.opposingSlot, !isAttackingLeft);
			}

			__result.Add(slotToAttack);
			// __instance.GetComponent<AlternatingStrike>().isAttackingLeft = isAttackingLeft;
			Log.LogDebug($"[AlternatingStrike.Patch] Attacking slot [{slotToAttack.Index}]");
		}
	}
}
