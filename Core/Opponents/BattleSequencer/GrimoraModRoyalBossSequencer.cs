using System.Collections;
using DiskCardGame;
using InscryptionAPI.Card;
using InscryptionAPI.Encounters;
using InscryptionAPI.Helpers.Extensions;
using Pixelplacement;
using Sirenix.Utilities;
using UnityEngine;
using static GrimoraMod.GrimoraPlugin;

namespace GrimoraMod;

public class GrimoraModRoyalBossSequencer : GrimoraModBossBattleSequencer
{
	public new static readonly SpecialSequenceManager.FullSpecialSequencer FullSequencer = SpecialSequenceManager.Add(
		GUID,
		nameof(GrimoraModRoyalBossSequencer),
		typeof(GrimoraModRoyalBossSequencer)
	);

	private readonly RandomEx _rng = new();

	private Animator _gameTableAnimator;

	public const float DurationTableSway = 3.5f;

	public int boardSwayCounter = 0;
	public bool boardSwayedLeftLast;

	public override Opponent.Type BossType => RoyalBossOpponentExt.FullOpponent.Id;

	private void Start()
	{
		_gameTableAnimator = GameObject.Find("GameTable").GetComponent<Animator>();
	}

	private void PlayTableSway()
	{
		_gameTableAnimator.Play(!boardSwayedLeftLast ? "sway_left" : "sway_right", 0, 0f);

		boardSwayedLeftLast = !boardSwayedLeftLast;
	}

	private List<PlayableCard> GetValidCardsForLitFuse()
	{
		var cardsHavingFewerThan5Abilities = BoardManager.Instance
		 .GetPlayerCards(pCard =>
			                 !pCard.FaceDown
			              && pCard.InfoName() != NamePirateSwashbuckler
			              && pCard.AllAbilities().Count < 5
			);

		return cardsHavingFewerThan5Abilities;
	}

	private IEnumerator ApplyLitFuseToPlayerCard(PlayableCard playerCard)
	{
		ViewManager.Instance.SwitchToView(View.Board);
		yield return new WaitForSeconds(0.25f);
		yield return TextDisplayer.Instance.ShowUntilInput(
			$"YARRRR, I WILL ENJOY THE KABOOM OF {playerCard.Info.displayedName.BrightRed()}"
		);
		playerCard.AddTemporaryMod(new CardModificationInfo(LitFuse.ability));
	}

	public override IEnumerator OpponentCombatEnd()
	{
		if (AscensionSaveData.Data.ChallengeIsActive(ChallengeManagement.ThreePhaseGhouls) && TurnManager.Instance.Opponent.NumLives == 1)
		{
			if (cannonTargetSlots.Count > 0)
			{
				yield return FireCannonsSequence();
			}
			yield return ChooseWaveTargetsSequence();

		}
		else { 
			var validCards = GetValidCardsForLitFuse();
		if (validCards.IsNullOrEmpty())
		{
			yield break;
		}

		if (_rng.NextBoolean() && _rng.NextBoolean())
		{
			yield return ApplyLitFuseToPlayerCard(validCards.GetRandomItem());
		}
		}
	}

	private List<CardSlot> cannonTargetSlots = new List<CardSlot>();

	private List<GameObject> targetIcons = new List<GameObject>();

	private bool playFireDialogue;

	private bool playPirateImmuneDialogue;

	private bool playedCannonTargetDialogue;

	private GameObject targetIconPrefab;

	private IEnumerator ChooseWaveTargetsSequence()
	{
		Singleton<ViewManager>.Instance.Controller.LockState = ViewLockState.Locked;
		yield return new WaitForSeconds(0.3f);
		int seed = SaveManager.SaveFile.GetCurrentRandomSeed() + Singleton<TurnManager>.Instance.TurnNumber;
		if (targetIconPrefab == null)
		{
			targetIconPrefab = ResourceBank.Get<GameObject>("Prefabs/Cards/SpecificCardModels/CannonTargetIcon");
		}
		List<CardSlot> opponentSlotsCopy = Singleton<BoardManager>.Instance.OpponentSlotsCopy;
		opponentSlotsCopy.RemoveAll((CardSlot x) => cannonTargetSlots.Contains(x));
		List<CardSlot> playerSlotsCopy = Singleton<BoardManager>.Instance.PlayerSlotsCopy;
		playerSlotsCopy.RemoveAll((CardSlot x) => cannonTargetSlots.Contains(x));
		cannonTargetSlots.Clear();
		cannonTargetSlots.Add(opponentSlotsCopy[SeededRandom.Range(0, opponentSlotsCopy.Count, seed++)]);
		cannonTargetSlots.Add(playerSlotsCopy[SeededRandom.Range(0, playerSlotsCopy.Count, seed)]);
		Singleton<ViewManager>.Instance.SwitchToView(View.Board, immediate: false, lockAfter: true);
		yield return new WaitForSeconds(0.25f);
		foreach (CardSlot slot in cannonTargetSlots)
		{
			yield return new WaitForSeconds(0.05f);
			GameObject gameObject = UnityEngine.Object.Instantiate(targetIconPrefab, slot.transform);
			gameObject.transform.localPosition = new Vector3(0f, 0.25f, 0f);
			gameObject.transform.localRotation = Quaternion.identity;
			targetIcons.Add(gameObject);
		}
		yield return new WaitForSeconds(0.3f);
		if (!playedCannonTargetDialogue)
		{
			yield return TextDisplayer.Instance.ShowUntilInput($"BOIL ME ORANGES, THERES A-WAVES COMIN!");
			playedCannonTargetDialogue = true;
		}
		yield return new WaitForSeconds(0.3f);
	}

	private IEnumerator FireCannonsSequence()
	{
		bool firedLeftSide = false;
		bool firedRightSide = false;
		for (int i = 0; i < cannonTargetSlots.Count; i++)
		{
			Singleton<ViewManager>.Instance.SwitchToView(View.Default, immediate: false, lockAfter: true);
			yield return new WaitForSeconds(0.25f);
			CardSlot slot = cannonTargetSlots[i];
			if (!(slot.Card != null) || slot.Card.Dead)
			{
				continue;
			}
			if (i == 0)
			{
				playFireDialogue = !playFireDialogue;
				if (playFireDialogue)
				{
					yield return TextDisplayer.Instance.ShowUntilInput($"THESE WAVES WILL PUSH YE CARD OFF THE BOARD, BACK TO YER HAND!");
				}
				firedRightSide = true;
			}
			else
			{
				firedLeftSide = true;
			}
			yield return new WaitForSeconds(0.5f);
			Singleton<ViewManager>.Instance.SwitchToView(View.Board);

			if (!slot.Card.AllAbilities().Contains(Anchored.ability))
			{
				CardInfo cloneInfo = slot.Card.Info.Clone() as CardInfo;
				cloneInfo.Mods = new(slot.Card.Info.Mods);
				ViewManager.Instance.SetViewUnlocked();

				yield return CardSpawner.Instance.SpawnCardToHand(cloneInfo);
				yield return slot.Card.DieCustom(
				false,
				royalTableSwayValue: -7f
			);
				yield return new WaitForSeconds(0.8f);
				Singleton<ViewManager>.Instance.SwitchToView(View.Hand);
			}
			else
			{
				if (playPirateImmuneDialogue)
				{
					yield return TextDisplayer.Instance.ShowUntilInput($"THIS PIRATE CANNOT BE TAKEN BACK UP BY THE WAVES! BUT EVEN THE ZESTIEST OF PIRATES WILL BE SHAKEN UP!");
					playPirateImmuneDialogue = false;
				}
				slot.Card.TakeDamage(1, null);
				slot.Card.Anim.StrongNegationEffect();
			}
		}
		CleanupTargetIcons();
	}

	public void CleanupTargetIcons()
	{
		targetIcons.ForEach(delegate (GameObject x)
		{
			if (x != null)
			{
				CleanUpTargetIcon(x);
			}
		});
		targetIcons.Clear();
	}

	private void CleanUpTargetIcon(GameObject icon)
	{
		Tween.LocalScale(icon.transform, Vector3.zero, 0.1f, 0f, Tween.EaseIn, Tween.LoopType.None, null, delegate
		{
			UnityEngine.Object.Destroy(icon);
		});
	}



	public override IEnumerator PlayerUpkeep()
	{
		if (AscensionSaveData.Data.ChallengeIsActive(ChallengeManagement.ThreePhaseGhouls) && TurnManager.Instance.Opponent.NumLives == 1)
		{
		}
		else
		{
			if (boardSwayCounter++ >= 2)
			{
				yield return StartBoardSway();
			}
		}

		yield return base.PlayerUpkeep();
	}

	private IEnumerator StartBoardSway()
	{
		boardSwayCounter = 0;
		yield return TextDisplayer.Instance.ShowUntilInput("Seven seas for the table!");

		ViewManager.Instance.SwitchToView(View.Default, lockAfter: true);
		// z axis for table movement

		PlayTableSway();

		var allCardsOnBoard = BoardManager.Instance.AllSlotsCopy
		 .Where(slot => slot.Card && slot.Card.LacksAbility(Anchored.ability) && slot.Card.LacksAbility(Ability.Flying))
		 .Select(slot => slot.Card)
		 .ToList();

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

		yield return new WaitForSeconds(3f);
		ViewManager.Instance.SetViewUnlocked();
	}

	private bool SlotHasSpace(CardSlot slot, bool toLeft)
	{
		CardSlot adjacent = slot.GetAdjacent(toLeft);
		if (adjacent.SafeIsUnityNull())
		{
			Log.LogInfo($"[TableSway.SlotHasSpace] Adjacent slot [{slot.name}] does not have an adjacent slot to the {(toLeft ? "left" : "right")}");
			// if beyond far left slot or far right slot, slot does not have space
			// play death animation?
			return false;
		}

		if (adjacent.Card.SafeIsUnityNull())
		{
			// if the slot is valid but no card, slot does have space
			Log.LogInfo($"[TableSway.SlotHasSpace] Adjacent slot [{slot.name}] but does not have a card to the {(toLeft ? "left" : "right")}");
			return true;
		}

		// if the slot and the slot is occupied, check the adjacent slot of that card
		Log.LogInfo($"[TableSway.SlotHasSpace] Checking {(toLeft ? "left" : "right")} adjacent slot of card [{adjacent.Card.GetNameAndSlot()}]");
		return adjacent.Card.LacksAbility(Anchored.ability) && adjacent.Card.LacksAbility(Ability.Flying) && SlotHasSpace(adjacent, toLeft);
	}

	protected virtual IEnumerator DoStrafe(PlayableCard playableCard, bool movingLeft)
	{
		Log.LogInfo($"[TableSway.DoStrafe] Starting strafe for card {playableCard.GetNameAndSlot()} Moving left? [{movingLeft}]");

		CardSlot toLeft = playableCard.Slot.GetAdjacent(true);
		CardSlot toRight = playableCard.Slot.GetAdjacent(false);
		Log.LogInfo($"[TableSway.DoStrafe] Card {playableCard.GetNameAndSlot()} Checking adjacent slots to left [{toLeft?.name}] to right [{toRight?.name}]");

		bool toLeftIsNotOccupied = SlotHasSpace(playableCard.Slot, true);
		bool toRightIsNotOccupied = SlotHasSpace(playableCard.Slot, false);
		Log.LogInfo($"[TableSway.DoStrafe] Card {playableCard.GetNameAndSlot()} toLeftIsNotOccupied? [{toLeftIsNotOccupied}] toRightIsNotOccupied [{toRightIsNotOccupied}]");

		CardSlot destination = movingLeft ? toLeft : toRight;
		bool destinationValid = movingLeft ? toLeftIsNotOccupied : toRightIsNotOccupied;

		yield return MoveToSlot(playableCard, destination, destinationValid, movingLeft);
	}

	private IEnumerator MoveToSlot(
		PlayableCard playableCard,
		CardSlot destination,
		bool destinationValid,
		bool movingLeft
	)
	{
		Log.LogInfo($"[TableSway.DoStrafe] Starting MoveToSlot method for {playableCard.GetNameAndSlot()} Destination [{destination?.name}] Destination Valid? [{destinationValid}]");

		if (playableCard.HasAnyAbilities(Ability.Strafe, Ability.StrafePush, Ability.StrafeSwap, Ability.SkeletonStrafe))
		{
			playableCard.RenderInfo.SetAbilityFlipped(
				playableCard.Info.abilities.Find(ab => ab is Ability.Strafe or Ability.StrafePush or Ability.StrafeSwap or Ability.SkeletonStrafe),
				movingLeft
			);
			playableCard.RenderInfo.flippedPortrait = movingLeft && playableCard.Info.flipPortraitForStrafe;
			playableCard.RenderCard();
		}

		if (destination)
		{
			bool destinationSlotCardHasAnchoredOrFlying =
				destination.Card && (destination.Card.HasAnyAbilities(Anchored.ability, Ability.Flying));
			if (destinationSlotCardHasAnchoredOrFlying)
			{
				Log.LogInfo($"[TableSway.MoveToSlot] Card {playableCard.GetNameAndSlot()} Destination card is not null and has anchored or flying.");
				playableCard.Anim.StrongNegationEffect();
				yield return new WaitForSeconds(0.15f);
			}
			else if (destinationValid)
			{
				Log.LogInfo($"[TableSway.MoveToSlot] Card {playableCard.GetNameAndSlot()} will be moved to slot [{destination.name}]");

				SkinCrawlerSlot crawlerSlot = null;
				if (playableCard.Slot.GetComponentInChildren<SkinCrawlerSlot>())
				{
					Log.LogWarning($"[TableSway.MoveToSlot] SkinCrawlerSlot is not null, sliding to new slot");
					crawlerSlot = playableCard.Slot.GetComponentInChildren<SkinCrawlerSlot>();
					var crawlerCard = crawlerSlot.skinCrawlerCard;
					crawlerCard.transform.SetParent(destination.transform);
					crawlerSlot.transform.SetParent(destination.transform);
					crawlerSlot.hidingOnSlot = destination;
					Tween.LocalPosition(
						crawlerCard.transform,
						Vector3.up * (BoardManager3D.Instance.SlotHeightOffset + crawlerCard.SlotHeightOffset) + new Vector3(0f, 0f, 0.31f),
						DurationTableSway + 2,
						0.05f,
						Tween.EaseOut,
						Tween.LoopType.None,
						null,
						delegate { crawlerCard.Anim.PlayRiffleSound(); }
					);
					Tween.Rotation(
						crawlerCard.transform,
						destination.transform.GetChild(0).rotation,
						DurationTableSway + 2.5f,
						0f,
						Tween.EaseOut
					);
				}

				yield return BoardManager.Instance.AssignCardToSlot(
					playableCard,
					destination,
					DurationTableSway + 2
				);
				Tween.LocalRotation(
					playableCard.transform,
					new Vector3(90, 0, 0),
					0,
					0,
					Tween.EaseIn
				);
				if (crawlerSlot)
				{
					Tween.LocalRotation(
						crawlerSlot.skinCrawlerCard.transform,
						new Vector3(90, 0, 0),
						0,
						0,
						Tween.EaseIn
					);
					yield return new WaitForSeconds(3f);
				}

				yield return new WaitForSeconds(0.25f);
			}
		}
		else
		{
			Log.LogInfo($"[TableSway.MoveToSlot] Card {playableCard.GetNameAndSlot()} is about to fucking die me hearty!");
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
