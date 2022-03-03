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

		if (++boardSwayCounter == 2)
		{
			yield return TextDisplayer.Instance.ShowUntilInput(
				$"Seven seas for the table!"
			);

			Vector3 positionCopy = BoardManager3D.Instance.transform.position;

			// z axis for table movement
			Log.LogDebug($"Rotating left downward");
			Tween.Rotation(
				_gameTable.transform,
				new Vector3(0, 0, 20),
				10f,
				1f,
				startCallback: () =>
				{
					Quaternion currentRotation = GrimoraAnimationController.Instance.transform.localRotation;
					Tween.LocalRotation(
						GrimoraAnimationController.Instance.transform,
						new Vector3(0, 180, currentRotation.z + 20),
						7,
						1
					);
				},
				completeCallback: () =>
				{
					Tween.Rotation(_gameTable.transform, new Vector3(0, 0, 0), 7, 0);
				}
			);

			foreach (var playableCard in BoardManager.Instance.GetPlayerCards())
			{
				yield return DoStrafe(playableCard, true);
			}
		}
	}

	protected virtual IEnumerator DoStrafe(PlayableCard playableCard, bool movingLeft)
	{
		Log.LogWarning($"[DoStrafe] starting strafe for card {playableCard.GetNameAndSlot()}");
		// check left slot, if null, then play animation of falling of the board and then destroy
		CardSlot toLeftSlot = BoardManager.Instance.GetAdjacent(playableCard.Slot, true);
		CardSlot toRightSlot = BoardManager.Instance.GetAdjacent(playableCard.Slot, false);
		bool canMoveLeft = toLeftSlot != null && toLeftSlot.Card == null;
		bool canMoveRight = toRightSlot != null && toRightSlot.Card == null;

		CardSlot destination = movingLeft
			? toLeftSlot
			: toRightSlot;
		bool destinationValid = movingLeft
			? canMoveLeft
			: canMoveRight;
		yield return MoveToSlot(playableCard, destination, destinationValid, movingLeft);
	}

	protected IEnumerator MoveToSlot(
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
			Log.LogWarning(
				$"[MoveToSlot] Card [{playableCard.GetNameAndSlot()}] will be moved to slot [{destination.name}]!"
			);

			Tween.LocalPosition(playableCard.transform, destination.transform.localPosition, 3, 0);
			
			Log.LogWarning(
				$"[MoveToSlot] Starting assign card to slot"
			);
			yield return BoardManager.Instance.AssignCardToSlot(playableCard, destination, 3);
			yield return new WaitForSeconds(0.25f);
		}
		else
		{
			Vector3 positionCopy = playableCard.transform.localPosition;
			Log.LogWarning($"[MoveToSlot] Card [{playableCard.GetNameAndSlot()}] is about to fucking die me hearty!");
			TweenBase slidingCard = Tween.LocalPosition(
				playableCard.transform,
				new Vector3(positionCopy.x - 6, positionCopy.y, positionCopy.z),
				9f,
				0f,
				Tween.EaseInStrong
			);
			yield return new WaitForSeconds(0.15f);
			yield return new WaitUntil(() => slidingCard.Status == Tween.TweenStatus.Finished);
			yield return playableCard.Die(false);
		}
	}
}
