﻿using System.Collections;
using DiskCardGame;
using UnityEngine;

namespace GrimoraMod;

public class BoneThief : AbilityBehaviour
{
	public static Ability ability;

	public override Ability Ability => ability;

	public override bool RespondsToOtherCardDie(
		PlayableCard card,
		CardSlot deathSlot,
		bool fromCombat,
		PlayableCard killer
	)
	{
		if (Card.OnBoard)
		{
			return killer == Card && card.OpponentCard;
		}

		return false;
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
		const string rulebookDescription = "When [creature] kills another creature, gain 2 bones.";

		ApiUtils.CreateAbility<BoneThief>(rulebookDescription);
	}
}
