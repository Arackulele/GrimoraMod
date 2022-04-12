using System.Collections;
using DiskCardGame;
using UnityEngine;

namespace GrimoraMod;

public class BloodGuzzler : AbilityBehaviour
{
	public static Ability ability;
	
	public override Ability Ability => ability;

	private CardModificationInfo _modInfo;

	private void Start()
	{
		_modInfo = new CardModificationInfo
		{
			nonCopyable = true,
			singletonId = "grimoramod_BloodGuzzler"
		};
		Card.AddTemporaryMod(_modInfo);
	}

	public override bool RespondsToDealDamage(int amount, PlayableCard target)
	{
		return amount > 0;
	}

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
		Texture icon = AbilitiesUtil.LoadAbilityIcon(Ability.BloodGuzzler.ToString());
		AbilityBuilder<BloodGuzzler>.Builder
		 .SetRulebookDescription(abilityInfo.rulebookDescription)
		 .SetIcon(icon)
		 .Build();
	}
}
