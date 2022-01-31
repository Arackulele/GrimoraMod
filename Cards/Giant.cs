using System.Collections;
using APIPlugin;
using DiskCardGame;
using HarmonyLib;
using UnityEngine;

namespace GrimoraMod;

public partial class GrimoraPlugin
{
	public const string NameGiant = "ara_Giant";

	private void AddAra_Giant()
	{
		var sbIds = GrimoraGiant.Create();
		
		NewCard.Add(CardBuilder.Builder
			.SetAsNormalCard()
			.SetAbilities(Ability.QuadrupleBones, Ability.SplitStrike)
			.SetBaseAttackAndHealth(2, 7)
			.SetBoneCost(15)
			.SetNames(NameGiant, "Giant")
			.SetTraits(Trait.Giant)
			// .SetDescription("A vicious pile of bones. You can have it...")
			.Build()
			, specialAbilitiesIdsParam: new List<SpecialAbilityIdentifier> { sbIds.id }
		);
	}
}

public class GrimoraGiant : SpecialCardBehaviour
{
	public static NewSpecialAbility Create()
	{
		var sId = SpecialAbilityIdentifier.GetID(GrimoraPlugin.PluginGuid, "!GRIMORA_GIANT");

		return new NewSpecialAbility(typeof(GrimoraGiant), sId);
	}

	public override bool RespondsToResolveOnBoard()
	{
		return true;
	}

	public override IEnumerator OnResolveOnBoard()
	{
		BoardManager.Instance.OpponentSlotsCopy[base.PlayableCard.Slot.Index - 1].Card = base.PlayableCard;
		BoardManager.Instance.OpponentSlotsCopy[base.PlayableCard.Slot.Index].Card = base.PlayableCard;
		yield break;
	}

	public override bool RespondsToDie(bool wasSacrifice, PlayableCard killer)
	{
		return true;
	}

	public override IEnumerator OnDie(bool wasSacrifice, PlayableCard killer)
	{
		BoardManager.Instance.OpponentSlotsCopy[base.PlayableCard.Slot.Index - 1].Card = null;
		BoardManager.Instance.OpponentSlotsCopy[base.PlayableCard.Slot.Index].Card = null;
		yield break;
	}
}

[HarmonyPatch]
public class ModifyLocalPositionsOfTableObjects
{
	[HarmonyPostfix,
	 HarmonyPatch(typeof(BoardManager3D), nameof(BoardManager3D.TransitionAndResolveCreatedCard))
	]
	public static IEnumerator ChangeScaleOfMoonCardToFitAcrossAllSlots(
		IEnumerator enumerator, PlayableCard card, CardSlot slot,
		float transitionLength, bool resolveTriggers = true)
	{
		if (SaveManager.SaveFile.IsGrimora && card.Info.HasTrait(Trait.Giant))
		{
			GrimoraPlugin.Log.LogDebug($"Setting new scaling and position of [{card.Info.name}]");
			// Card -> RotatingParent -> TombstoneParent -> Cardbase_StatsLayer
			// UnityEngine.GameObject cardBase = __result.transform.GetChild(0).GetChild(0).GetChild(0).gameObject;

			card.transform.GetChild(0).localPosition = new Vector3(-0.7f, 1.25f, 0f);
			// GrimoraPlugin.Log.LogDebug($"Successfully set new localPosition for the giant");

			card.GetComponentInChildren<GravestoneRenderStatsLayer>()
					.transform.localScale = new UnityEngine.Vector3(1.4f, 1.25f, 0.2f);
			// GrimoraPlugin.Log.LogDebug($"Successfully set new scaling for the giant");
		}

		yield return enumerator;
	}
}
