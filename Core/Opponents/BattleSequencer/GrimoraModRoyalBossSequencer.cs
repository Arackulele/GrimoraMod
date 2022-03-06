using System.Collections;
using DiskCardGame;
using Pixelplacement;
using Sirenix.Utilities;
using UnityEngine;
using static GrimoraMod.GrimoraPlugin;

namespace GrimoraMod;

public class GrimoraModRoyalBossSequencer : GrimoraModBossBattleSequencer
{
	private readonly RandomEx _rng = new();

	private GameObject GameTable => GameObject.Find("GameTable");

	public const float DurationTableSway = 3.5f;

	public int boardSwayCounter = 0;
	public bool boardSwayedLeftLast = false;

	public override Opponent.Type BossType => BaseBossExt.RoyalOpponent;

	private void PlayTableSway()
	{
		GameTable
			.GetComponent<Animator>()
			.Play(
				!boardSwayedLeftLast
					? "sway_left"
					: "sway_right",
				0,
				0f
			);

		boardSwayedLeftLast = !boardSwayedLeftLast;
	}

	public override EncounterData BuildCustomEncounter(CardBattleNodeData nodeData)
	{
		return new EncounterData()
		{
			opponentType = BossType
		};
	}

	public override IEnumerator OpponentCombatEnd()
	{
		var activePlayerCards = BoardManager.Instance.GetPlayerCards(pCard => !pCard.FaceDown);
		if (activePlayerCards.IsNullOrEmpty())
		{
			yield break;
		}

		if (_rng.NextBoolean())
		{
			var playableCard = activePlayerCards[UnityEngine.Random.Range(0, activePlayerCards.Count)];
			ViewManager.Instance.SwitchToView(View.Board);
			yield return new WaitForSeconds(0.25f);
			yield return TextDisplayer.Instance.ShowUntilInput(
				$"YARRRR, I WILL ENJOY THE KABOOM OF {playableCard.Info.displayedName.BrightRed()}"
			);
			playableCard.AddTemporaryMod(new CardModificationInfo(LitFuse.ability));
		}

		if (++boardSwayCounter >= 2)
		{
			boardSwayCounter = 0;
			yield return TextDisplayer.Instance.ShowUntilInput(
				$"Seven seas for the table!"
			);

			ViewManager.Instance.SwitchToView(View.Default, lockAfter: true);
			// z axis for table movement

			PlayTableSway();

			var allCardsOnBoard = BoardManager.Instance.AllSlotsCopy
				.Where(slot => slot.Card is not null && !slot.CardIsNotNullAndHasAbility(SeaLegs.ability))
				.Select(slot => slot.Card)
				.ToList();

			// Log.LogInfo($"[TableSway] List of cards [{allCardsOnBoard.Join(converter: card => card.GetNameAndSlot())}]");

			if (!boardSwayedLeftLast)
			{
				// the reason for this is so that the cards are executed right to left and not left to right.
				// if they're executed left to right like how swaying left will is,
				//	it will kill cards that have cards next to them 
				allCardsOnBoard.Reverse();
			}

			foreach (var playableCard in allCardsOnBoard)
			{
				yield return StartCoroutine(DoStrafe(playableCard, boardSwayedLeftLast));
			}

			ViewManager.Instance.SetViewUnlocked();
		}
	}

	private bool SlotHasSpace(CardSlot slot, bool toLeft)
	{
		CardSlot adjacent = BoardManager.Instance.GetAdjacent(slot, toLeft);
		if (adjacent == null)
		{
			Log.LogInfo(
				$"[TableSway.SlotHasSpace] Adjacent slot [{slot.name}] does not have an adjacent slot to the {(toLeft ? "left" : "right")}"
			);
			// if beyond far left slot or far right slot, slot does not have space
			// play death animation?
			return false;
		}

		if (adjacent.Card == null)
		{
			// if the slot is valid but no card, slot does have space
			Log.LogInfo(
				$"[TableSway.SlotHasSpace] Adjacent slot [{slot.name}] is not null but does not have a card to the {(toLeft ? "left" : "right")}"
			);
			return true;
		}

		// if the slot is not null and the slot is occupied, check the adjacent slot of that card
		Log.LogInfo(
			$"[TableSway.SlotHasSpace] Checking {(toLeft ? "left" : "right")} adjacent slot of card [{adjacent.Card.InfoName()}]"
		);
		return SlotHasSpace(adjacent, toLeft);
	}

	protected virtual IEnumerator DoStrafe(PlayableCard playableCard, bool movingLeft)
	{
		Log.LogInfo(
			$"[TableSway.DoStrafe] Starting strafe for card {playableCard.GetNameAndSlot()} Moving left? [{movingLeft}]"
		);

		CardSlot toLeft = BoardManager.Instance.GetAdjacent(playableCard.Slot, true);
		CardSlot toRight = BoardManager.Instance.GetAdjacent(playableCard.Slot, false);
		Log.LogInfo(
			$"[TableSway.DoStrafe] Card {playableCard.GetNameAndSlot()} Checking adjacent slots to left [{toLeft?.name}] to right [{toRight?.name}]"
		);

		bool toLeftIsNotOccupied = SlotHasSpace(playableCard.Slot, true);
		bool toRightIsNotOccupied = SlotHasSpace(playableCard.Slot, false);
		Log.LogInfo(
			$"[TableSway.DoStrafe] Card {playableCard.GetNameAndSlot()} toLeftIsNotOccupied? [{toLeftIsNotOccupied}] toRightIsNotOccupied [{toRightIsNotOccupied}]"
		);

		CardSlot destination = movingLeft
			? toLeft
			: toRight;
		bool destinationValid = (movingLeft
			? toLeftIsNotOccupied
			: toRightIsNotOccupied);
		if (destination != null && destination.Card != null)
		{
			Log.LogInfo(
				$"[TableSway.DoStrafe] Card {playableCard.GetNameAndSlot()} Destination [{destination?.name}] Destination Valid? [{destinationValid}]"
			);
			yield return RecursivePush(playableCard, destination, movingLeft, null);
		}

		yield return MoveToSlot(playableCard, destination, destinationValid, movingLeft);
	}

	private IEnumerator RecursivePush(PlayableCard playableCard, CardSlot slot, bool toLeft, Action<bool> canMoveResult)
	{
		CardSlot adjacent = BoardManager.Instance.GetAdjacent(slot, toLeft);
		Log.LogInfo(
			$"[TableSway.RecursivePush] Card {playableCard.GetNameAndSlot()} Slot [{slot.name}] {(toLeft ? "left" : "right")} Adjacent Slot [{adjacent?.name}]"
		);
		if (adjacent == null)
		{
			// if beyond far left slot or far right slot, slot does not have space
			// play death animation?
			canMoveResult?.Invoke(false);
			yield break;
		}

		if (adjacent.Card == null)
		{
			// open slot on the board
			Log.LogInfo(
				$"[TableSway.RecursivePush] Assigning [{slot.Card.GetNameAndSlot()}] to adjacent slot [{adjacent.name}]"
			);
			yield return BoardManager.Instance.AssignCardToSlot(slot.Card, adjacent);
			canMoveResult?.Invoke(true);
			yield break;
		}

		bool canMove = false;
		yield return RecursivePush(
			adjacent.Card,
			adjacent,
			toLeft,
			delegate(bool movePossible)
			{
				canMove = movePossible;
			}
		);
		if (canMove)
		{
			Log.LogInfo(
				$"[TableSway.RecursivePush] Card {playableCard.GetNameAndSlot()} can move, moving to [{adjacent.name}]"
			);
			yield return BoardManager.Instance.AssignCardToSlot(slot.Card, adjacent);
		}

		canMoveResult?.Invoke(canMove);
	}

	private IEnumerator MoveToSlot(
		PlayableCard playableCard,
		CardSlot destination,
		bool destinationValid,
		bool movingLeft
	)
	{
		if (playableCard.HasAnyAbilities(Ability.Strafe, Ability.StrafePush))
		{
			playableCard.RenderInfo.SetAbilityFlipped(
				playableCard.Info.abilities.Find(ab => ab is Ability.Strafe or Ability.StrafePush),
				movingLeft
			);
			playableCard.RenderInfo.flippedPortrait = movingLeft && playableCard.Info.flipPortraitForStrafe;
			playableCard.RenderCard();
		}

		if (destination != null && destinationValid)
		{
			Log.LogInfo(
				$"[TableSway.MoveToSlot] Card {playableCard.GetNameAndSlot()} will be moved to slot [{destination.name}]"
			);

			yield return BoardManager.Instance.AssignCardToSlot(playableCard, destination, DurationTableSway + 2);
			Tween.LocalRotation(
				playableCard.transform,
				new Vector3(90, 0, 0),
				0,
				0,
				Tween.EaseIn
			);
			yield return new WaitForSeconds(0.25f);
		}
		else
		{
			if (!playableCard.HasAbility(SeaLegs.ability))
			{
				Log.LogInfo(
					$"[TableSway.MoveToSlot] Card {playableCard.GetNameAndSlot()} is about to fucking die me hearty!"
				);
				Vector3 positionCopy = playableCard.transform.localPosition;
				float leftOrRightX = movingLeft
					? positionCopy.x - 6
					: positionCopy.x + 6;
				yield return playableCard.DieCustom(
					false,
					royalTableSwayValue: leftOrRightX
				);
				yield return new WaitForSeconds(0.15f);
			}
		}
	}
}
