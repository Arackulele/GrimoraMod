using System.Collections;
using DiskCardGame;
using UnityEngine;

namespace GrimoraMod;

public class Imbued : AbilityBehaviour
{
	public static Ability ability;

	public override Ability Ability => ability;

	public const string ModIdImbued = "grimoramod_Imbued";

	private void Awake()
	{
		Card.StatsLayer.DisableEmission();
	}

	public override bool RespondsToOtherCardDie(
		PlayableCard card,
		CardSlot deathSlot,
		bool fromCombat,
		PlayableCard killer
	)
	{
		return card && !card.OpponentCard && !card.HasAbility(Ability.Brittle);
	}

	public override IEnumerator OnOtherCardDie(
		PlayableCard card,
		CardSlot deathSlot,
		bool fromCombat,
		PlayableCard killer
	)
	{
		if (Card.TemporaryMods.Exists(mod => mod.singletonId == ModIdImbued))
		{
			var mod = Card.TemporaryMods.Find(mod => mod.singletonId == ModIdImbued);
			mod.attackAdjustment += 1;
		}
		else
		{
			Card.AddTemporaryMod(new CardModificationInfo(1, 0)
			{
				singletonId = ModIdImbued
			});
			Card.Anim.PlayTransformAnimation();
		}

		Card.OnStatsChanged();
		yield return new WaitForSeconds(0.25f);
	}
}

public partial class GrimoraPlugin
{
	public void Add_Ability_Imbued()
	{
		const string rulebookDescription = "When a non-brittle ally card perishes, [creature] gains 1 power.";

		ApiUtils.CreateAbility<Imbued>(rulebookDescription);
	}
}
