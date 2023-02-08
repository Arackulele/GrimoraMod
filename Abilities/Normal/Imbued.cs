using System.Collections;
using DiskCardGame;
using InscryptionAPI.Card;
using UnityEngine;

namespace GrimoraMod;

public class Imbued : AbilityBehaviour
{
	public const string ModIdImbued = "grimoramod_Imbued";

	public const string RulebookName = "Imbued";

	public static Ability ability;

	public override Ability Ability => ability;

	private CardModificationInfo _modInfo;

	private void Start()
	{
		_modInfo = new CardModificationInfo
		{
			singletonId = ModIdImbued
		};
		Card.AddTemporaryMod(_modInfo);
	}

	public override bool RespondsToOtherCardDie(
		PlayableCard card,
		CardSlot deathSlot,
		bool fromCombat,
		PlayableCard killer
	)
	{
		return card && card != Card && card.IsPlayerCard() && card.LacksAbility(Ability.Brittle);
	}

	public override IEnumerator OnOtherCardDie(
		PlayableCard card,
		CardSlot deathSlot,
		bool fromCombat,
		PlayableCard killer
	)
	{
		Card.Anim.StrongNegationEffect();
		if (_modInfo.attackAdjustment == 0)
		{
			Card.Anim.PlayTransformAnimation();
			Card.StatsLayer.SetEmissionColor(GrimoraColors.DefaultEmission);
		}
		_modInfo.attackAdjustment += 1;
		Card.OnStatsChanged();
		
		yield return new WaitForSeconds(0.25f);
	}
}

public partial class GrimoraPlugin
{
	public void Add_Ability_Imbued()
	{
		const string rulebookDescriptionEnglish = "When a non-brittle ally card perishes, [creature] gains 1 power.";
		const string rulebookDescriptionChinese = "当友方的非脆骨单位阵亡时，[creature]增加1点力量。";
		string rulebookDescription = Localization.CurrentLanguage == Language.ChineseSimplified ? rulebookDescriptionChinese : rulebookDescriptionEnglish;

		AbilityBuilder<Imbued>.Builder
		 .SetRulebookDescription(rulebookDescription)
		 .SetRulebookName(Imbued.RulebookName)
		 .Build();
	}
}
