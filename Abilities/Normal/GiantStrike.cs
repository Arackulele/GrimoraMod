using System.Collections;
using DiskCardGame;
using GrimoraMod.Extensions;
using HarmonyLib;
using InscryptionAPI.Helpers.Extensions;
using InscryptionAPI.Triggers;
using UnityEngine;

namespace GrimoraMod;

public class GiantStrike : AbilityBehaviour, IGetOpposingSlots
{
	public const string ModSingletonId = "GrimoraMod_EnragedGiant";

	public const string RulebookName = "Giant Strike";

	public static Ability ability;

	public override Ability Ability => ability;

	private readonly CardModificationInfo _modEnragedGiant = new(1, 0)
	{
		abilities = new List<Ability> { GiantStrikeEnraged.ability },
		negateAbilities = new List<Ability> { GiantStrike.ability },
		singletonId = ModSingletonId
	};

	public override bool RespondsToOtherCardDie(PlayableCard card, CardSlot deathSlot, bool fromCombat, PlayableCard killer)
	{
		return !Card.TemporaryMods.Exists(mod => mod.singletonId == ModSingletonId)
		    && TurnManager.Instance.Opponent.OpponentType == GrimoraBossOpponentExt.FullOpponent.Id
		    && card != Card
		    && card.InfoName() is GrimoraPlugin.NameGiantOtis or GrimoraPlugin.NameGiantEphialtes
		    && deathSlot.IsOpponentSlot();
	}

	public override IEnumerator OnOtherCardDie(PlayableCard card, CardSlot deathSlot, bool fromCombat, PlayableCard killer)
	{
		ViewManager.Instance.SwitchToView(View.OpponentQueue);
		yield return TextDisplayer.Instance.ShowUntilInput(
			$"Oh dear, you've made {Card.Info.DisplayedNameEnglish.Red()} quite angry."
		);
		Card.AddTemporaryMod(_modEnragedGiant);
		yield return new WaitForSeconds(1);
	}

	public bool RemoveDefaultAttackSlot() => true;

	public bool RespondsToGetOpposingSlots() => true;

	public List<CardSlot> GetTwinGiantOpposingSlots()
	{
		return BoardManager.Instance.PlayerSlotsCopy
		 .Where(slot => slot.opposingSlot.Card == Card)
		 .ToList();
	}

	public List<CardSlot> GetOpposingSlots(List<CardSlot> originalSlots, List<CardSlot> otherAddedSlots)
	{
		// assume giant is in slot indexes 0, 1
		// original slots has opposing slot of index 1
		List<CardSlot> slotsToTarget = new List<CardSlot>(GetTwinGiantOpposingSlots());
		if (ability != GiantStrikeEnraged.ability && slotsToTarget.Exists(slot => slot.Card))
		{
			List<PlayableCard> cards = slotsToTarget.GetCards();
			if (cards.Count == 1)
			{
				PlayableCard onlyCard = cards[0];
				slotsToTarget.Clear();
				slotsToTarget.Add(onlyCard.Slot);
				// single card has health greater than current attack, then attack twice 
				if (onlyCard.Health > Card.Attack)
				{
					slotsToTarget.Add(onlyCard.Slot);
				}
			}
		}

		GrimoraPlugin.Log.LogInfo($"[{GetType().Name}] Opposing slots is now [{slotsToTarget.Join(slot => slot.Index.ToString())}]");
		return slotsToTarget;
	}
}

public partial class GrimoraPlugin
{
	public void Add_Ability_GiantStrike()
	{
		const string rulebookDescriptionEnglish =
			"[creature] will strike each opposing space. "
		+ "If only one creature is in the opposing spaces, this card will strike that creature twice. ";
		const string rulebookDescriptionChinese =
			"[creature]会攻击对面每个位置。"
		+ "如果对面只有一只造物，则会攻击那只造物两次。";
		string rulebookDescription = Localization.CurrentLanguage == Language.ChineseSimplified ? rulebookDescriptionChinese : rulebookDescriptionEnglish;

		AbilityBuilder<GiantStrike>.Builder
		 .FlipIconIfOnOpponentSide()
		 .SetRulebookDescription(rulebookDescription)
		 .SetRulebookName(GiantStrike.RulebookName)
		 .Build();
	}
}
