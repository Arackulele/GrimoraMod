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
		return new List<CardSlot>
		{
			BoardManager.Instance.GetSlots(Card.OpponentCard)
				[BoardManager.Instance.PlayerSlotsCopy.Count - 1 - Card.Slot.Index]
		};
	}

	public static AbilityManager.FullAbility Create()
	{
		const string rulebookDescription =
			"[creature] will strike the opposing slot as if the board was flipped. "
			+ "A card in the far left slot will attack the opposing far right slot.";

		return ApiUtils.CreateAbility<InvertedStrike>(rulebookDescription, flipYIfOpponent: true);
	}
}
