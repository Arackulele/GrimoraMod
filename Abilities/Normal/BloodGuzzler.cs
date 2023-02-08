using System.Collections;
using DiskCardGame;
using InscryptionAPI.Card;
using UnityEngine;

namespace GrimoraMod;

public class BloodGuzzler : AbilityBehaviour
{
	public const string RulebookName = "Blood Guzzler";

	public const string ModSingletonId = "GrimoraMod_BloodGuzzler";
	
	public static Ability ability;
	
	public override Ability Ability => ability;

	private CardModificationInfo _modInfo;

	private void Start()
	{
		_modInfo = new CardModificationInfo
		{
			nonCopyable = true,
			singletonId = ModSingletonId
		};
		Card.AddTemporaryMod(_modInfo);
	}

	public override bool RespondsToDealDamage(int amount, PlayableCard target) => Card.NotDead() && amount > 0;

	public override IEnumerator OnDealDamage(int amount, PlayableCard target)
	{
		yield return PreSuccessfulTriggerSequence();
		_modInfo.healthAdjustment += amount;
		Card.OnStatsChanged();
		Card.Anim.StrongNegationEffect();
		yield return new WaitForSeconds(0.25f);
		yield return LearnAbility(0.25f);
	}
}

public partial class GrimoraPlugin
{
	public void Add_Ability_BloodGuzzler()
	{
		const string rulebookDescriptionEnglish =
			"When [creature] deals damage, it gains 1 health for each damage dealt.";
		const string rulebookDescriptionChinese =
			"当[creature]造成伤害时，每造成1点伤害获得1点生命。";
        string rulebookDescription = Localization.CurrentLanguage == Language.ChineseSimplified ? rulebookDescriptionChinese : rulebookDescriptionEnglish;

		AbilityBuilder<BloodGuzzler>.Builder
		 .SetIcon(AbilitiesUtil.LoadAbilityIcon(Ability.BloodGuzzler.ToString()))
		 .SetRulebookDescription(rulebookDescription)
		 .SetRulebookName(BloodGuzzler.RulebookName)
		 .Build();
	}
}
