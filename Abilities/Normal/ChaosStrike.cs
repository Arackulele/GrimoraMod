using DiskCardGame;
using HarmonyLib;
using InscryptionAPI.Card;

namespace GrimoraMod;

public class ChaosStrike : ExtendedAbilityBehaviour
{
	public static Ability ability;

	public override Ability Ability => ability;

	public override bool RemoveDefaultAttackSlot() => true;

	public override bool RespondsToGetOpposingSlots() => true;

	public override List<CardSlot> GetOpposingSlots(List<CardSlot> originalSlots, List<CardSlot> otherAddedSlots)
	{
		List<CardSlot> slotsToTarget = new List<CardSlot>(BoardManager.Instance.GetAdjacentSlots(Card.Slot.opposingSlot))
			{ Card.Slot.opposingSlot };

		slotsToTarget = slotsToTarget.Randomize().ToList();

		GrimoraPlugin.Log.LogInfo($"[{GetType().Name}] Opposing slots is now [{slotsToTarget.Join(slot => slot.Index.ToString())}]");
		return slotsToTarget;
	}
}

public partial class GrimoraPlugin
{
	public void Add_Ability_ChaosStrike()
	{
		const string rulebookDescription = "[creature] will strike each opposing space to the left, right, and center of it, randomly.";

		ApiUtils.CreateAbility<ChaosStrike>(rulebookDescription, flipYIfOpponent: true);
	}
}
