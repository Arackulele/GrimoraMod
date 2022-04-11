using System.Collections;
using DiskCardGame;
using UnityEngine;

namespace GrimoraMod;

public class Imbued : AbilityBehaviour
{
	public const string ModIdImbued = "grimoramod_Imbued";

	public static Ability ability;

	public override Ability Ability => ability;

	private CardModificationInfo GetImbuedMod(PlayableCard playableCard)
	{
		return playableCard.TemporaryMods.Find(mod => mod.singletonId == ModIdImbued);
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
		CardModificationInfo imbuedMod = GetImbuedMod(Card);
		if (imbuedMod != null)
		{
			Card.Anim.StrongNegationEffect();
			imbuedMod.attackAdjustment += 1;
			Card.OnStatsChanged();
		}
		else
		{
			Card.AddTemporaryMod(new CardModificationInfo(1, 0)
			{
				singletonId = ModIdImbued
			});
			Card.Anim.PlayTransformAnimation();
			Card.StatsLayer.SetEmissionColor(GrimoraColors.DefaultEmission);
		}
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
