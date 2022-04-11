using DiskCardGame;
using InscryptionAPI.Card;

namespace GrimoraMod;

public class BuffCrewMates : ExtendedAbilityBehaviour
{
	public static Ability ability;

	public override Ability Ability => ability;

	public override bool ProvidesPassiveAttackBuff => true;

	public override int[] GetPassiveAttackBuffs()
	{
		int[] arrForSkeletonsToBuff = ConfigHelper.HasIncreaseSlotsMod ? new[] { 0, 0, 0, 0, 0 } : new[] { 0, 0, 0, 0 };
		var skeletonSlotIndexes = BoardManager.Instance
		 .GetSlots(!Card.OpponentCard)
		 .Where(slot => slot.CardInSlotIs(GrimoraPlugin.NameSkeleton))
		 .Select(slot => slot.Index)
		 .ToList();

		foreach (var slotIndex in skeletonSlotIndexes)
		{
			arrForSkeletonsToBuff[slotIndex] = 1;
		}

		return arrForSkeletonsToBuff;
	}
}

public partial class GrimoraPlugin
{
	public void Add_Ability_BuffCrewMates()
	{
		const string rulebookDescription =
			"[creature] empowers each Skeleton on the owner's side of the board, providing a +1 buff their power.";

		AbilityBuilder<BuffCrewMates>.Builder
		 .SetRulebookDescription(rulebookDescription)
		 .SetRulebookName("Sea Shanty")
		 .Build();
	}
}
