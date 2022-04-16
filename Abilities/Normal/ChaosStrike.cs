using DiskCardGame;
using HarmonyLib;
using InscryptionAPI.Card;
using InscryptionAPI.Helpers.Extensions;
using InscryptionAPI.Triggers;

namespace GrimoraMod;

public class ChaosStrike : AbilityBehaviour, IGetOpposingSlots
{
	public static Ability ability;

	public override Ability Ability => ability;

	public bool RemoveDefaultAttackSlot() => true;

	public bool RespondsToGetOpposingSlots() => true;

	public List<CardSlot> GetOpposingSlots(List<CardSlot> originalSlots, List<CardSlot> otherAddedSlots)
	{
		List<CardSlot> slotsToTarget = new List<CardSlot>(Card.Slot.opposingSlot.GetAdjacentSlots())
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

		AbilityBuilder<ChaosStrike>.Builder
		 .FlipIconIfOnOpponentSide()
		 .SetRulebookDescription(rulebookDescription)
		 .Build();
	}
}
