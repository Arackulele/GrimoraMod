using DiskCardGame;
using InscryptionAPI.Card;

namespace GrimoraMod;

public class InvertedStrike : ExtendedAbilityBehaviour
{
	public static Ability ability;
	public override Ability Ability => ability;

	public override bool RemoveDefaultAttackSlot() => true;

	public override bool RespondsToGetOpposingSlots()
	{
		return !Card.HasAbility(AreaOfEffectStrike.ability);
	}

	public override List<CardSlot> GetOpposingSlots(List<CardSlot> originalSlots, List<CardSlot> otherAddedSlots)
	{
		int totalSlotCount = BoardManager.Instance.PlayerSlotsCopy.Count;
		List<CardSlot> opposingSlots = BoardManager.Instance.GetSlots(Card.OpponentCard);
		List<CardSlot> slotsToTarget = new List<CardSlot>();
		for (int i = 1; i <= originalSlots.Count; i++)
		{
			// first iter == 4 - 1 - 0 == opposing slot 3
			// second iter == 4 - 2 - 0 == opposing slot 2
			// third iter == 4 - 1 - 0 == opposing slot 1
			slotsToTarget.Add(opposingSlots[totalSlotCount - i - Card.Slot.Index]);
		}

		return slotsToTarget;
	}

	public static AbilityManager.FullAbility Create()
	{
		const string rulebookDescription =
			"[creature] will strike the opposing slot as if the board was flipped. "
			+ "A card in the far left slot will attack the opposing far right slot.";

		return ApiUtils.CreateAbility<InvertedStrike>(rulebookDescription, flipYIfOpponent: true);
	}
}
