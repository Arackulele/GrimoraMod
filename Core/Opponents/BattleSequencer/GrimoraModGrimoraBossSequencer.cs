using System.Collections;
using DiskCardGame;
using InscryptionAPI.Encounters;
using UnityEngine;
using static GrimoraMod.GrimoraPlugin;

namespace GrimoraMod;

public class GrimoraModGrimoraBossSequencer : GrimoraModBossBattleSequencer
{
	public static readonly SpecialSequenceManager.FullSpecialSequencer FullSequencer = SpecialSequenceManager.Add(
		GUID,
		nameof(GrimoraModGrimoraBossSequencer),
		typeof(GrimoraModGrimoraBossSequencer)
	);

	private readonly RandomEx _rng = new();

	private bool hasPlayedArmyDialogue = false;

	private bool playedDialogueDeathTouch;

	private bool playedDialoguePossessive;

	public override Opponent.Type BossType => GrimoraBossOpponentExt.FullOpponent.Id;

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

			yield return new WaitForSeconds(0.5f);

			if (ConfigHelper.Instance.IsEndlessModeEnabled)
			{
				Log.LogInfo($"Player won against Grimora! Now to win again, endlessly...");
				yield return TextDisplayer.Instance.ShowUntilInput("Wonderful! I am pleasantly surprised by your triumph against me!");
				yield return TextDisplayer.Instance.ShowUntilInput("...You wish to continue? Endlessly? Splendid!");
				yield return TextDisplayer.Instance.ShowUntilInput("Please, take a card of your choosing.");
			}
			else
			{
				Log.LogInfo($"Player won against Grimora! Resetting run...");
				ConfigHelper.Instance.ResetRun();
				FinaleDeletionWindowManager.instance.mainWindow.gameObject.SetActive(true);
				yield return ((GrimoraGameFlowManager)GameFlowManager.Instance).EndSceneSequence();
			}
		}
		else
		{
			yield return base.GameEnd(false);
		}
	}

	private bool SlotContainsTwinGiant(CardSlot cardSlot)
	{
		return cardSlot.CardIsNotNullAndHasSpecialAbility(GrimoraGiant.FullSpecial.Id) && cardSlot.CardInSlotIs(NameGiant);
	}

	public override IEnumerator OpponentUpkeep()
	{
		if (!playedDialogueDeathTouch
		 && BoardManager.Instance.PlayerSlotsCopy.Exists(x => x.CardIsNotNullAndHasAbility(Ability.Deathtouch))
		 && BoardManager.Instance.OpponentSlotsCopy.Exists(SlotContainsTwinGiant)
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
		      && BoardManager.Instance.PlayerSlotsCopy.Exists(x => x.CardIsNotNullAndHasAbility(Possessive.ability))
		      && BoardManager.Instance.OpponentSlotsCopy.Exists(slot => slot.CardInSlotIs(NameBonelord))
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

		List<CardSlot> opponentQueuedSlots = BoardManager.Instance.GetQueueSlots();
		if (card.InfoName() == NameGiant)
		{
			yield return EnrageLastTwinGiant(card);
		}
		else if (opponentQueuedSlots.IsNotEmpty() && _willReanimateCardThatDied)
		{
			ViewManager.Instance.SwitchToView(View.BossCloseup);
			yield return TextDisplayer.Instance.PlayDialogueEvent(
				"GrimoraBossReanimate1",
				TextDisplayer.MessageAdvanceMode.Input
			);

			CardSlot slot = opponentQueuedSlots[UnityRandom.Range(0, opponentQueuedSlots.Count)];
			yield return TurnManager.Instance.Opponent.QueueCard(card.Info, slot);
			_willReanimateCardThatDied = false;
			yield return new WaitForSeconds(0.5f);
		}
		else { _willReanimateCardThatDied = true; }
	}

	private IEnumerator EnrageLastTwinGiant(PlayableCard playableCard)
	{
		CardSlot remainingGiantSlot = BoardManager.Instance.OpponentSlotsCopy
		 .Find(slot => slot.Card && playableCard.Slot != slot && slot.Card.InfoName() == NameGiant);
		
		if (remainingGiantSlot)
		{
			ViewManager.Instance.SwitchToView(View.OpponentQueue);
			PlayableCard lastGiant = remainingGiantSlot.Card;
			yield return TextDisplayer.Instance.ShowUntilInput(
				$"Oh dear, you've made {lastGiant.Info.displayedName.Red()} quite angry."
			);
			CardModificationInfo modInfo = new CardModificationInfo
			{
				abilities = new List<Ability> { GiantStrikeEnraged.ability },
				attackAdjustment = 1,
				negateAbilities = new List<Ability> { GiantStrike.ability }
			};
			lastGiant.Anim.PlayTransformAnimation();
			lastGiant.AddTemporaryMod(modInfo);
			yield return new WaitForSeconds(0.1f);
			lastGiant.StatsLayer.SetEmissionColor(GameColors.Instance.red);

			yield return new WaitForSeconds(0.5f);
		}
	}
}
