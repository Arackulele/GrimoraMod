using DiskCardGame;
using HarmonyLib;
using InscryptionAPI.Card;
using InscryptionAPI.Helpers.Extensions;
using InscryptionAPI.Triggers;

namespace GrimoraMod;

public class ChaosStrike : AbilityBehaviour, IGetOpposingSlots
{
	public const string RulebookName = "Chaos Strike";

	public static Ability ability;

	public override Ability Ability => ability;

	public bool RemoveDefaultAttackSlot() => true;

	public bool RespondsToGetOpposingSlots() => true;

	public List<CardSlot> GetOpposingSlots(List<CardSlot> originalSlots, List<CardSlot> otherAddedSlots)
	{
		CardSlot opposingSlot = Card.OpposingSlot();
		List<CardSlot> opposingSlotAndAdj = new List<CardSlot>(opposingSlot.GetAdjacentSlots(true)) { opposingSlot };
		List<CardSlot> slotsToTarget = new List<CardSlot>();

		for (int i = 0; i < 3; i++)
		{
			slotsToTarget.Add(opposingSlotAndAdj.GetRandomItem());
		}

		GrimoraPlugin.Log.LogInfo($"[{GetType().Name}] Opposing slots is now [{slotsToTarget.Join(slot => slot.Index.ToString())}]");
		return slotsToTarget;
	}
}

public partial class GrimoraPlugin
{
	public void Add_Ability_ChaosStrike()
	{
		const string rulebookDescriptionEnglish = "[creature] will strike the opposing slots to the left, right, and center of it randomly, up to 3 times.";
		const string rulebookDescriptionChinese = "[creature]会随机攻击正对面的左右两侧和中间位置，共3次。";
		string rulebookDescription = Localization.CurrentLanguage == Language.ChineseSimplified ? rulebookDescriptionChinese : rulebookDescriptionEnglish;

		AbilityBuilder<ChaosStrike>.Builder
		 .FlipIconIfOnOpponentSide()
		 .SetRulebookDescription(rulebookDescription)
		 .SetRulebookName(ChaosStrike.RulebookName)
		 .Build();
	}
}
