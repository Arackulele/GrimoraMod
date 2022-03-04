using System.Collections;
using DiskCardGame;
using Pixelplacement;
using Pixelplacement.TweenSystem;
using UnityEngine;
using static GrimoraMod.GrimoraPlugin;

namespace GrimoraMod;

public class GrimoraModRoyalBossSequencer : GrimoraModBossBattleSequencer
{
	private readonly RandomEx _rng = new();

	private GameObject _gameTable = GameObject.Find("GameTable");

	private const float DurationTableSway = 3.5f;

	private readonly Vector3 _tableSwayLeft = new(0, 0, 10);
	private readonly Vector3 _tableSwayRight = new(0, 0, -10);

	private int boardSwayCounter = 0;

	public override Opponent.Type BossType => BaseBossExt.RoyalOpponent;

	public override EncounterData BuildCustomEncounter(CardBattleNodeData nodeData)
	{
		return new EncounterData()
		{
			opponentType = BossType
		};
	}

	public override bool RespondsToUpkeep(bool playerUpkeep)
	{
		return playerUpkeep;
	}

	public override IEnumerator OnUpkeep(bool playerUpkeep)
	{
		var activePlayerCards = BoardManager.Instance.GetPlayerCards();
		if (activePlayerCards.IsNotEmpty() && _rng.NextBoolean())
		{
			var playableCard = activePlayerCards[UnityEngine.Random.Range(0, activePlayerCards.Count)];
			ViewManager.Instance.SwitchToView(View.Board);
			yield return new WaitForSeconds(0.25f);
			yield return TextDisplayer.Instance.ShowUntilInput(
				$"YARRRR, I WILL ENJOY THE KABOOM OF {playableCard.Info.displayedName.BrightRed()}",
				1f,
				0.5f,
				Emotion.Anger
			);
			if (!playableCard.TemporaryMods.Exists(mod => mod.abilities.Contains(LitFuse.ability)))
			{
				playableCard.AddTemporaryMod(new CardModificationInfo(LitFuse.ability));
			}
		}

		if (++boardSwayCounter >= 2)
		{
			yield return TextDisplayer.Instance.ShowUntilInput(
				$"Seven seas for the table!"
			);

			ViewManager.Instance.SwitchToView(View.Default);
			ViewManager.Instance.Controller.LockState = ViewLockState.Locked;
			bool moveLeft = boardSwayCounter % 2 == 0;
			// z axis for table movement
			_gameTable
				.GetComponent<Animator>()
				.Play(
					moveLeft
						? "sway_left"
						: "sway_right"
				);
			
			var allCardsOnBoard = BoardManager.Instance.AllSlotsCopy
				.Where(slot => slot.Card is not null)
				.Select(slot => slot.Card)
				.ToList();
			foreach (var playableCard in allCardsOnBoard)
			{
				StartCoroutine(DoStrafe(playableCard, moveLeft));
			}

			ViewManager.Instance.Controller.LockState = ViewLockState.Unlocked;
		}
	}

	protected virtual IEnumerator DoStrafe(PlayableCard playableCard, bool movingLeft)
	{
		Log.LogInfo($"[DoStrafe] starting strafe for card {playableCard.GetNameAndSlot()}");
		// check left slot, if null, then play animation of falling of the board and then destroy
		CardSlot toLeftSlot = BoardManager.Instance.GetAdjacent(playableCard.Slot, true);
		CardSlot toRightSlot = BoardManager.Instance.GetAdjacent(playableCard.Slot, false);
		bool canMoveLeft = toLeftSlot != null && toLeftSlot.Card == null;
		bool canMoveRight = toRightSlot != null && toRightSlot.Card == null;

		CardSlot destination = movingLeft
			? toLeftSlot
			: toRightSlot;
		bool destinationValid = destination is not null
		                        && (movingLeft
			                        ? canMoveLeft
			                        : canMoveRight);
		Log.LogInfo(
			$"[DoStrafe] {playableCard.GetNameAndSlot()} Destination [{destination?.name}] DestValid [{destinationValid}]"
		);
		yield return StartCoroutine(MoveToSlot(playableCard, destination, destinationValid, movingLeft));
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

		Vector3 positionCopy = playableCard.transform.localPosition;
		if (destination != null && destinationValid)
		{
			Log.LogInfo(
				$"[MoveToSlot] Card [{playableCard.GetNameAndSlot()}] will be moved to slot [{destination.name}]!"
			);

			yield return BoardManager.Instance.AssignCardToSlot(playableCard, destination, DurationTableSway);
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
			float leftOrRightX = movingLeft
				? positionCopy.x - 6
				: positionCopy.x + 6;
			Log.LogInfo($"[MoveToSlot] Card [{playableCard.GetNameAndSlot()}] is about to fucking die me hearty!");
			TweenBase slidingCard = Tween.LocalPosition(
				playableCard.transform,
				new Vector3(leftOrRightX, positionCopy.y, positionCopy.z),
				DurationTableSway,
				0,
				Tween.EaseIn
			);
			yield return new WaitForSeconds(0.15f);
			CustomCoroutine.WaitOnConditionThenExecute(
				() => slidingCard.Status == Tween.TweenStatus.Finished,
				() =>
				{
					Log.LogInfo($"[MoveToSlot] Killing card playableCard [{playableCard.GetNameAndSlot()}]");
					StartCoroutine(playableCard.Die(false));
				}
			);
		}
	}
}
