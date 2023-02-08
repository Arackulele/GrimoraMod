using System.Collections;
using DiskCardGame;
using InscryptionAPI.Card;
using UnityEngine;

namespace GrimoraMod;

public class ActivatedDealDamageGrimora : ActivatedDealDamage
{
	public const int ENERGY_COST = 1;

	public const string RulebookName = "Soul Shot";

	public static Ability ability;

	public override Ability Ability => ability;

	public override int EnergyCost => ENERGY_COST;

	public override IEnumerator Activate()
	{
		CardSlot opposingSlot = Card.OpposingSlot();
		bool impactFrameReached = false;
		Card.Anim.PlaySpecificAttackAnimation(
			"attack_sentry",
			false,
			opposingSlot,
			delegate { impactFrameReached = true; }
		);
		yield return new WaitUntil(() => impactFrameReached);
		yield return opposingSlot.Card.TakeDamage(1, Card);
		yield return new WaitForSeconds(0.25f);
	}
}

public partial class GrimoraPlugin
{
	public void Add_Ability_ActivatedDealDamageGrimora()
	{
		const string rulebookDescriptionEnglish = "Pay 1 Energy to deal 1 damage to the creature across from [creature].";
		const string rulebookDescriptionChinese = "消耗1点能量，可对[creature]对面的造物造成1点伤害。";
		string rulebookDescription = Localization.CurrentLanguage == Language.ChineseSimplified ? rulebookDescriptionChinese : rulebookDescriptionEnglish;

		AbilityBuilder<ActivatedDealDamageGrimora>.Builder
		 .SetRulebookDescription(rulebookDescription)
		 .SetRulebookName(ActivatedDealDamageGrimora.RulebookName)
		 .Build();
	}
}
