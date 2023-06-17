using System.Collections;
using DiskCardGame;
using InscryptionAPI.Card;
using UnityEngine;

namespace GrimoraMod;

public class Possessive : AbilityBehaviour
{
	public const string RulebookName = "Possessive";

	public static Ability ability;
	public override Ability Ability => ability;

	public override bool RespondsToSlotTargetedForAttack(CardSlot slot, PlayableCard attacker)
	{
		return attacker && attacker.Slot == Card.OpposingSlot();
	}

	public override IEnumerator OnSlotTargetedForAttack(CardSlot slot, PlayableCard attacker)
	{
		attacker.Anim.StrongNegationEffect();
		yield break;
	}
}

public partial class GrimoraPlugin
{
	public void Add_Ability_Possessive()
	{
		const string rulebookDescriptionEnglish =
			"[creature] cannot be attacked from the opposing slot. "
		+ "The opposing slot, if possible, instead attacks one of its adjacent friendly cards.";
		const string rulebookDescriptionChinese =
			"[creature]对面的造物不会攻击自身，"
		+ "而是在可能的条件下攻击其相邻的友方造物。";
		string rulebookDescription = Localization.CurrentLanguage == Language.ChineseSimplified ? rulebookDescriptionChinese : rulebookDescriptionEnglish;

		AbilityBuilder<Possessive>.Builder
		 .SetRulebookDescription(rulebookDescription)
		 .SetRulebookName(Possessive.RulebookName)
		 .SetPixelIcon(AssetUtils.GetPrefab<Sprite>("posessive_pixel"))
		 .Build();
	}
}
