using System.Collections;
using DiskCardGame;
using InscryptionAPI.Card;
using UnityEngine;

namespace GrimoraMod;

public class BloodGuzzler : AbilityBehaviour
{
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
		AbilityInfo abilityInfo = AbilitiesUtil.GetInfo(Ability.BloodGuzzler);
		AbilityBuilder<BloodGuzzler>.Builder
		 .SetIcon(AbilitiesUtil.LoadAbilityIcon(Ability.BloodGuzzler.ToString()))
		 .SetRulebookDescription(abilityInfo.rulebookDescription)
		 .Build();
	}
}
