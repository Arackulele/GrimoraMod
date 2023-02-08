using System.Collections;
using DiskCardGame;
using UnityEngine;

namespace GrimoraMod;

public class BoneThief : AbilityBehaviour
{
	public const string RulebookName = "Bone Thief";

	public static Ability ability;

	public override Ability Ability => ability;

	public override bool RespondsToOtherCardDie(
		PlayableCard card,
		CardSlot deathSlot,
		bool fromCombat,
		PlayableCard killer
	)
	{
		return Card.OnBoard && killer == Card && card.OpponentCard;
	}

	public override IEnumerator OnOtherCardDie(
		PlayableCard card,
		CardSlot deathSlot,
		bool fromCombat,
		PlayableCard killer
	)
	{
		yield return PreSuccessfulTriggerSequence();
		Card.Anim.LightNegationEffect();
		yield return ResourcesManager.Instance.AddBones(2, deathSlot);
		yield return LearnAbility();
		yield return new WaitForSeconds(0.25f);
	}
}

public partial class GrimoraPlugin
{
	public void Add_Ability_BoneThief()
	{
		const string rulebookDescriptionEnglish = "When [creature] kills another creature, gain 2 bones.";
		const string rulebookDescriptionChinese = "当[creature]击杀其他造物时，获得2根骨头。";
		string rulebookDescription = Localization.CurrentLanguage == Language.ChineseSimplified ? rulebookDescriptionChinese : rulebookDescriptionEnglish;

		AbilityBuilder<BoneThief>.Builder
		 .SetRulebookDescription(rulebookDescription)
		 .SetRulebookName(BoneThief.RulebookName)
		 .Build();
	}
}
