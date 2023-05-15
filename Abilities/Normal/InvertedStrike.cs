using DiskCardGame;
using InscryptionAPI.Card;
using InscryptionAPI.Triggers;
using UnityEngine;

namespace GrimoraMod;

public class InvertedStrike : AbilityBehaviour, IGetOpposingSlots
{
	public static Ability ability;
	public override Ability Ability => ability;

	public bool RemoveDefaultAttackSlot() => true;

	public bool RespondsToGetOpposingSlots() => Card.LacksAbility(AreaOfEffectStrike.ability);

	public List<CardSlot> GetOpposingSlots(List<CardSlot> originalSlots, List<CardSlot> otherAddedSlots)
	{
		int slotIdx = Card.Slot.Index;
		int totalSlotCount = BoardManager.Instance.PlayerSlotsCopy.Count;
		List<CardSlot> opposingSlots = BoardManager.Instance.GetSlots(Card.OpponentCard);
		List<CardSlot> slotsToTarget = new List<CardSlot>();
		if (Card.HasTriStrike())
		{
			// 0 1 2 3 - indexes
			// 1 O 3 2
			// C O O O
			// cardIdx == 0
			// 4 - 1 - cardIdx == 3
			// 4 - 2 - cardIdx == 2 then abs

			// 0 1 2 3 - indexes
			// 0 3 2 1
			// O C O O
			// 4 - cardIdx == 3
			// 4 - 1 - cardIdx == 2
			// 4 - 2 - cardIdx == 1

			// 0 1 2 3 - indexes
			// 1 2 3 0 - expected attack pattern
			// O O C O
			// 4 - 2 - cardIdx == 0
			// 4 - 1 - cardIdx == 1
			// 4 - cardIdx == 2

			// 0 1 2 3 - indexes
			// 2 3 0 1
			// O O O C
			// cardIdx == 3
			// 4 - 1 - cardIdx == 0
			// 4 - 2 - cardIdx == -1 then abs
			if (slotIdx == 1)
			{
				slotsToTarget.Add(opposingSlots[totalSlotCount - slotIdx]);               // 3 if card idx is 3
				slotsToTarget.Add(opposingSlots[totalSlotCount - 1 - slotIdx]);           // 0 if card idx is 3
				slotsToTarget.Add(opposingSlots[Math.Abs(totalSlotCount - 2 - slotIdx)]); // 1 if card idx is 3
			}
			else if (slotIdx == 2)
			{
				slotsToTarget.Add(opposingSlots[totalSlotCount - 2 - slotIdx]);       // 3 if card idx is 3
				slotsToTarget.Add(opposingSlots[totalSlotCount - 1 - slotIdx]);       // 0 if card idx is 3
				slotsToTarget.Add(opposingSlots[Math.Abs(totalSlotCount - slotIdx)]); // 1 if card idx is 3
			}
			else
			{
				slotsToTarget.Add(opposingSlots[slotIdx]);                                // 3 if card idx is 3
				slotsToTarget.Add(opposingSlots[totalSlotCount - 1 - slotIdx]);           // 0 if card idx is 3
				slotsToTarget.Add(opposingSlots[Math.Abs(totalSlotCount - 2 - slotIdx)]); // 1 if card idx is 3
			}
		}
		else
		{
			slotsToTarget.Add(opposingSlots[totalSlotCount - 1 - slotIdx]); // index 0
		}

		return slotsToTarget;
	}
}

public partial class GrimoraPlugin
{
	public void Add_Ability_InvertedStrike()
	{
		const string rulebookDescription =
			"[creature] will strike the opposing slot as if the board was flipped. "
		+ "A card in the far left slot will attack the opposing far right slot.";

		AbilityBuilder<InvertedStrike>.Builder
		 .SetRulebookDescription(rulebookDescription)
		 .SetPixelIcon(AssetUtils.GetPrefab<Sprite>("invertedstrike2"))
		 .FlipIconIfOnOpponentSide()
		 .Build();
	}
}
