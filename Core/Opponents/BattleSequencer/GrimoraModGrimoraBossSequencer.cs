using System.Collections;
using DiskCardGame;
using InscryptionAPI.Encounters;
using UnityEngine;
using static GrimoraMod.GrimoraPlugin;

namespace GrimoraMod;

public class GrimoraModGrimoraBossSequencer : GrimoraModBossBattleSequencer
{
	public static readonly SpecialSequenceManager.FullSpecialSequencer FullSequencer = SpecialSequenceManager.Add(
		GrimoraPlugin.GUID,
		nameof(GrimoraModGrimoraBossSequencer),
		typeof(GrimoraModGrimoraBossSequencer)
	);

	private readonly RandomEx _rng = new();

	private bool hasPlayedArmyDialogue = false;

	private bool playedDialogueDeathTouch;

	private bool playedDialoguePossessive;

	public override IEnumerator GameEnd(bool playerWon)
	{
		if (playerWon)
		{
			if (!DialogueEventsData.EventIsPlayed("FinaleGrimoraBattleWon"))
			{
				Log.LogDebug($"FinaleGrimoraBattleWon has not played yet, playing now.");

				ViewManager.Instance.SetViewLocked();
				yield return new WaitForSeconds(0.5f);
				yield return TextDisplayer.Instance.PlayDialogueEvent(
					"FinaleGrimoraBattleWon",
					TextDisplayer.MessageAdvanceMode.Input
				);
			}

			if (!ConfigHelper.Instance.isEndlessModeEnabled)
			{
				yield return new WaitForSeconds(0.5f);
				Log.LogInfo($"Player won against Grimora! Resetting run...");
				ConfigHelper.Instance.ResetRun();
			}
		}
		else
		{
			yield return base.GameEnd(false);
		}
	}

	private bool SlotContainsTwinGiant(CardSlot cardSlot)
	{
		return cardSlot.CardIsNotNullAndHasSpecialAbility(GrimoraGiant.FullAbility.Id) && cardSlot.CardInSlotIs(NameGiant);
	}

	public override IEnumerator OpponentUpkeep()
	{
		if (!playedDialogueDeathTouch
		    && BoardManager.Instance.GetSlots(true).Exists(x => x.CardIsNotNullAndHasAbility(Ability.Deathtouch))
		    && BoardManager.Instance.GetSlots(false).Exists(SlotContainsTwinGiant)
		   )
		{
			yield return new WaitForSeconds(0.5f);
			yield return TextDisplayer.Instance.ShowUntilInput(
				"DEATH TOUCH WON'T HELP YOU HERE DEAR."
				+ "\nI MADE THESE GIANTS SPECIAL, IMMUNE TO QUITE A FEW DIFFERENT TRICKS!"
			);
			playedDialogueDeathTouch = true;
		}
		else if (!playedDialoguePossessive
		         && BoardManager.Instance.GetSlots(true).Exists(x => x.CardIsNotNullAndHasAbility(Possessive.ability))
		         && BoardManager.Instance.GetSlots(false).Exists(slot => slot.CardInSlotIs(NameBonelord))
		        )
		{
			yield return new WaitForSeconds(0.5f);
			yield return TextDisplayer.Instance.ShowUntilInput("THE BONE LORD CANNOT BE POSSESSED!");
			playedDialoguePossessive = true;
		}
	}

	public override IEnumerator PlayerUpkeep()
	{
		if (!DialogueEventsData.EventIsPlayed("FinaleGrimoraBattleStart"))
		{
			yield return new WaitForSeconds(0.5f);
			yield return TextDisplayer.Instance.PlayDialogueEvent(
				"FinaleGrimoraBattleStart",
				TextDisplayer.MessageAdvanceMode.Input
			);
		}
	}

	public override bool RespondsToOtherCardDie(
		PlayableCard card,
		CardSlot deathSlot,
		bool fromCombat,
		PlayableCard killer
	)
	{
		bool isPhaseOne = !card.OpponentCard && TurnManager.Instance.Opponent.NumLives == 3;
		bool giantDied = card.Info.HasTrait(Trait.Giant) && card.InfoName() == NameGiant;
		if (giantDied)
		{
			Log.LogDebug($"[GrimoraBoss] Giant died [{card.GetNameAndSlot()}]");
		}

		return isPhaseOne || giantDied;
	}

	private bool _willReanimateCardThatDied = false;

	public override IEnumerator OnOtherCardDie(
		PlayableCard card,
		CardSlot deathSlot,
		bool fromCombat,
		PlayableCard killer
	)
	{
		CardSlot remainingGiantSlot = BoardManager.Instance.OpponentSlotsCopy
			.Find(slot => slot.Card.IsNotNull() && card.Slot != slot && slot.Card.InfoName() == NameGiant);
		List<CardSlot> opponentQueuedSlots = BoardManager.Instance.GetQueueSlots();
		if (card.InfoName() == NameGiant)
		{
			if (remainingGiantSlot.IsNotNull())
			{
				PlayableCard lastGiant = remainingGiantSlot.Card;
				yield return TextDisplayer.Instance.ShowUntilInput(
					$"Oh dear, you've made {lastGiant.Info.displayedName.Red()} quite angry."
				);
				ViewManager.Instance.SwitchToView(View.Board);
				CardModificationInfo modInfo = new CardModificationInfo
				{
					abilities = new List<Ability> { GiantStrikeEnraged.ability },
					attackAdjustment = 1
				};
				lastGiant.Anim.PlayTransformAnimation();
				lastGiant.Info.RemoveBaseAbility(GiantStrike.ability);
				lastGiant.AddTemporaryMod(modInfo);
				yield return new WaitForSeconds(0.5f);
			}
		}
		else if (opponentQueuedSlots.IsNotEmpty() && _willReanimateCardThatDied)
		{
			ViewManager.Instance.SwitchToView(View.BossCloseup);
			yield return TextDisplayer.Instance.PlayDialogueEvent(
				"GrimoraBossReanimate1",
				TextDisplayer.MessageAdvanceMode.Input
			);

			CardSlot slot = opponentQueuedSlots[UnityEngine.Random.Range(0, opponentQueuedSlots.Count)];
			yield return TurnManager.Instance.Opponent.QueueCard(card.Info, slot);
			_willReanimateCardThatDied = false;
			yield return new WaitForSeconds(0.5f);
		}
		else { _willReanimateCardThatDied = true; }
	}
}
