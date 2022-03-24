﻿using System.Collections;
using DiskCardGame;
using InscryptionAPI.Card;
using UnityEngine;

namespace GrimoraMod;

public class BoneThief : AbilityBehaviour
{
	public static Ability ability;

	public override Ability Ability => ability;

	public static AbilityManager.FullAbility Create()
	{
		const string rulebookDescription = "When [creature] kills another card, gain 1 bone.";

		return ApiUtils.CreateAbility<BoneThief>(rulebookDescription);
	}

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
		yield return ResourcesManager.Instance.AddBones(1, deathSlot);
		yield return LearnAbility();
		yield return new WaitForSeconds(0.25f);
	}
}