using System.Collections;
using DiskCardGame;
using HarmonyLib;
using InscryptionAPI.Card;
using UnityEngine;
using static GrimoraMod.GrimoraPlugin;

namespace GrimoraMod;

public partial class GrimoraPlugin
{
	public const string NameGiant = $"{GUID}_Giant";

	private void Add_Card_Giant()
	{
		CardBuilder.Builder
			.SetAsNormalCard()
			.SetAbilities(Ability.QuadrupleBones, Ability.SplitStrike)
			.SetBaseAttackAndHealth(2, 7)
			.SetBoneCost(15)
			.SetNames(NameGiant, "Giant")
			.SetTraits(Trait.Giant, Trait.Uncuttable)
			.SetDescription("TRULY A SIGHT TO BEHOLD.")
			.Build()
			;
	}
}

[HarmonyPatch]
public class ModifyLocalPositionsOfTableObjects
{
	[HarmonyPostfix, HarmonyPatch(typeof(BoardManager3D), nameof(BoardManager3D.TransitionAndResolveCreatedCard))]
	public static IEnumerator ChangeScaleOfMoonCardToFitAcrossAllSlots(
		IEnumerator enumerator,
		PlayableCard card,
		CardSlot slot,
		float transitionLength,
		bool resolveTriggers = true
	)
	{
		if (GrimoraSaveUtil.isGrimora
		    && card.HasTrait(Trait.Giant)
		    && card.HasSpecialAbility(GrimoraGiant.FullSpecial.Id))
		{
			bool isBonelord = card.InfoName().Equals(NameBonelord);
			// Card -> RotatingParent (child zero) -> TombstoneParent -> Cardbase_StatsLayer
			Transform rotatingParent = card.transform.GetChild(0);

			float xValPosition = -0.7f;
			float xValScale = 2.1f;
			if (ConfigHelper.HasIncreaseSlotsMod && isBonelord)
			{
				xValPosition = -1.4f;
				xValScale = 3.3f;
			}

			rotatingParent.localPosition = new Vector3(xValPosition, 1.05f, 0);
			rotatingParent.localScale = new Vector3(xValScale, 2.1f, 1);
		}

		yield return enumerator;
	}
}
