using System.Collections;
using APIPlugin;
using DiskCardGame;
using HarmonyLib;
using Pixelplacement;
using Pixelplacement.TweenSystem;
using UnityEngine;
using static GrimoraMod.GrimoraPlugin;

namespace GrimoraMod;

public class SkinCrawler : AbilityBehaviour
{
	public static readonly NewAbility NewAbility = Create();

	public static Ability ability;

	public override Ability Ability => ability;

	private CardSlot _slotHidingUnderCard = null;

	private bool cardDoesNotHaveSkinCrawlerChild = false;

	public static SkinCrawler GetSkinCrawlerFromCard(PlayableCard playableCard)
	{
		for (int i = 0; i < playableCard.transform.childCount; i++)
		{
			SkinCrawler skinCrawler = playableCard.transform.GetChild(i).GetComponent<SkinCrawler>();
			if (skinCrawler is not null)
			{
				// now we get the card itself to add to the list
				return skinCrawler;
			}
		}

		return null;
	}

	public static SkinCrawler GetSkinCrawlerFromSlot(CardSlot slot)
	{
		if (slot is not null && slot.Card is not null)
		{
			return GetSkinCrawlerFromCard(slot.Card);
		}

		return null;
	}

	private CardSlot GetValidHostSlot()
	{
		Log.LogDebug($"[SkinCrawler] Checking if adj slots from [{Card.Slot}] are not null");
		CardSlot slotToPick = null;
		CardSlot centerSlot = null;
		if (_slotHidingUnderCard is not null)
		{
			centerSlot = Card.OpponentCard
				? BoardManager.Instance.OpponentSlotsCopy[_slotHidingUnderCard.Index]
				: BoardManager.Instance.PlayerSlotsCopy[_slotHidingUnderCard.Index];
		}

		Log.LogDebug($"[SkinCrawler] Slot for SkinCrawler [{Card.Slot}]");
		CardSlot toLeftSlot = BoardManager.Instance.GetAdjacent(Card.Slot, true);
		CardSlot toRightSlot = BoardManager.Instance.GetAdjacent(Card.Slot, false);

		if (toLeftSlot is not null && toLeftSlot.Card is not null)
		{
			slotToPick = toLeftSlot;
			Log.LogDebug($"[SkinCrawler] LeftSlot is not null, has card [{slotToPick.Card.InfoName()}]");
		}
		else if (toRightSlot is not null && toRightSlot.Card is not null)
		{
			slotToPick = toRightSlot;
			Log.LogDebug($"[SkinCrawler] RightSlot is not null, has card [{slotToPick.Card.InfoName()}]");
		}
		else if (centerSlot is not null && centerSlot.Card is not null)
		{
			slotToPick = centerSlot;
			Log.LogDebug($"[SkinCrawler] Center is not null, has card [{slotToPick.Card.InfoName()}]");
		}

		if (GetSkinCrawlerFromSlot(slotToPick) is not null)
		{
			return null;
		}

		return slotToPick;
	}

	public IEnumerator AssignSkinCrawlerCardToHost(CardSlot slotToPick)
	{
		if (slotToPick is not null)
		{
			PlayableCard cardToPick = slotToPick.Card;
			CardSlot cardSlotToPick = slotToPick.Card.Slot;

			ViewManager.Instance.SwitchToView(View.Board, lockAfter: true);

			yield return new WaitForSeconds(0.4f);
			// to the left and up, like something is being added under it
			Vector3 vector = new Vector3(0f, 0.25f, 0f);
			// do to card that will be hiding Boo Hag
			Tween.Position(cardToPick.transform, cardSlotToPick.transform.position + vector, 0.1f, 0f, Tween.EaseInOut);

			Vector3 cardRot = cardToPick.transform.rotation.eulerAngles;

			// rotate on z-axis, as if you rotated your hand holding the card counter-clockwise
			// Tween.Rotate(toRightTransform, new Vector3(0f, 0f, 25f), Space.World, 0.1f, 0f, Tween.EaseInOut);

			// Vector3 positionFurtherAwayFromBaseCard = toRightSlotTransform.position + Vector3.forward * 8f;
			// set starting position 
			// base.Card.transform.position = positionFurtherAwayFromBaseCard;

			// move pack from current position to the baseCardSlotPosition
			// Log.LogDebug($"[SkinCrawler] moving BooHag to [{toRightSlotTransform.position}]");
			TweenBase tweenMoveIntoCardSlot = Tween.Position(
				Card.transform,
				cardSlotToPick.transform.position,
				0.4f,
				0f,
				Tween.EaseOut
			);

			while (tweenMoveIntoCardSlot.Status is not Tween.TweenStatus.Finished)
			{
				// Log.LogDebug($"[SkinCrawler] playing negation effect in loop");
				cardToPick.Anim.StrongNegationEffect();
				yield return new WaitForSeconds(0.1f);
			}

			TweenBase tweenMoveUpward = Tween.Position(
				Card.transform,
				cardSlotToPick.transform.position + new Vector3(0f, 0f, 0.31f),
				0.2f,
				0f,
				Tween.EaseOut
			);
			yield return new WaitForSeconds(0.1f);

			// rotate base card with it's original rotation values so that it lays flat on the board again
			// Log.LogDebug($"[SkinCrawler] rotating [{toRightSlot.Card.Info.name}]");
			Tween.Rotation(cardToPick.transform, cardRot, 0.1f, 0f, Tween.EaseInOut);

			// offset the card to be a little higher
			// Log.LogDebug($"[SkinCrawler] Setting height offset");
			cardToPick.SlotHeightOffset = 0.13f;

			// reassign the card to the slot
			Log.LogDebug($"[SkinCrawler] Assigning [{cardToPick.Info.name}] to slot");
			yield return BoardManager.Instance.AssignCardToSlot(cardToPick, cardSlotToPick);

			cardToPick.AddTemporaryMod(new CardModificationInfo() { attackAdjustment = 1, healthAdjustment = 1 });

			yield return new WaitUntil(
				() => !Tween.activeTweens.Exists(t => t.targetInstanceID == cardToPick.transform.GetInstanceID())
			);

			Log.LogDebug($"[SkinCrawler] Setting Boo Hag as child of card");
			transform.SetParent(cardToPick.transform);
			Log.LogDebug($"[SkinCrawler] Setting Boo Hag slot [{Card.Slot.Index}] to null");
			// Card.UnassignFromSlot();
			BoardManager.Instance.playerSlots[Card.Slot.Index].Card = null;
			Log.LogDebug($"[SkinCrawler] Setting Boo Hag slot to [{cardSlotToPick}]");
			_slotHidingUnderCard = cardSlotToPick;
		}

		yield return new WaitForSeconds(0.25f);
		ViewManager.Instance.SwitchToView(View.Default);
		ViewManager.Instance.Controller.LockState = ViewLockState.Unlocked;
	}

	public override bool RespondsToOtherCardPreDeath(CardSlot deathSlot, bool fromCombat, PlayableCard killer)
	{
		// boo hag in slot 1
		// boo hag under card in slot 2
		// draugr dies in slot 2
		// 
		SkinCrawler crawler = GetSkinCrawlerFromSlot(deathSlot);
		return crawler is null
		       && BoardManager.Instance.GetAdjacentSlots(Card.Slot)
			       .Exists(slot => slot is not null && slot == deathSlot);
	}

	public override IEnumerator OnOtherCardPreDeath(CardSlot deathSlot, bool fromCombat, PlayableCard killer)
	{
		Log.LogDebug($"[Crawler.OnOtherCardPreDeath] Setting cardDoesNotHaveSkinCrawlerChild to true");
		cardDoesNotHaveSkinCrawlerChild = true;
		yield break;
	}

	public override bool RespondsToOtherCardAssignedToSlot(PlayableCard otherCard)
	{
		Log.LogDebug(
			$"[Crawler.RespondsToOtherCardAssignedToSlot]"
			+ $" This {Card.GetNameAndSlot()} OtherCard {otherCard.GetNameAndSlot()}"
		);
		return cardDoesNotHaveSkinCrawlerChild
		       && otherCard.Slot != Card.Slot
		       && BoardManager.Instance.GetAdjacentSlots(Card.Slot)
			       .Exists(slot => slot is not null && slot.Card == otherCard);
	}

	public override IEnumerator OnOtherCardAssignedToSlot(PlayableCard otherCard)
	{
		cardDoesNotHaveSkinCrawlerChild = false;
		Log.LogDebug($"[Crawler.OnOtherCardAssignedToSlot] Will now buff [{otherCard.InfoName()}]");
		yield return AssignSkinCrawlerCardToHost(otherCard.Slot);
	}


	public override bool RespondsToResolveOnBoard()
	{
		return true;
	}

	public override IEnumerator OnResolveOnBoard()
	{
		yield return AssignSkinCrawlerCardToHost(GetValidHostSlot());
	}

	public override bool RespondsToOtherCardDie(
		PlayableCard card,
		CardSlot deathSlot,
		bool fromCombat,
		PlayableCard killer
	)
	{
		Log.LogDebug($"[Crawler.RespondsToOtherCardDie] Card [{card.InfoName()}] has died in slot [{deathSlot}]");
		SkinCrawler crawler = GetSkinCrawlerFromCard(card);
		return crawler is not null && crawler.GetComponent<PlayableCard>() == Card;
	}

	public override IEnumerator OnOtherCardDie(
		PlayableCard card,
		CardSlot deathSlot,
		bool fromCombat,
		PlayableCard killer
	)
	{
		Log.LogDebug($"[SkinCrawler.OnOtherCardDie] Resolving [{Card}] to deathSlot [{deathSlot.Index}]");
		CardInfo infoCopy = Card.Info;
		Log.LogDebug($"[SkinCrawler.OnOtherCardDie] Destroying existing card to create a new one");
		Object.Destroy(Card.gameObject);
		Log.LogDebug($"[SkinCrawler.OnOtherCardDie] Creating new card");
		yield return BoardManager.Instance.CreateCardInSlot(infoCopy, deathSlot, 0);
	}

	public static NewAbility Create()
	{
		const string rulebookDescription =
			"When one of your creatures is placed in an adjacent space to [creature], "
			+ "[creature] will hide under it providing a +1 buff. Cards on the left take priority.";

		return ApiUtils.CreateAbility<SkinCrawler>(rulebookDescription);
	}
}

[HarmonyPatch(typeof(BoardManager))]
public class SkinCrawlerPatches
{
	[HarmonyPostfix, HarmonyPatch(nameof(BoardManager.CardsOnBoard), MethodType.Getter)]
	public static void Postfix(ref List<PlayableCard> __result)
	{
		List<PlayableCard> newList = new List<PlayableCard>(__result);
		foreach (var card in __result)
		{
			SkinCrawler skinCrawler = SkinCrawler.GetSkinCrawlerFromSlot(card.Slot);
			if (skinCrawler is not null)
			{
				// now we get the card itself to add to the list
				Log.LogDebug(
					$"[CardsOnBoard] Card [{card.InfoName()}] at slot [{card.slot.Index}] has a child with skin crawler"
				);
				newList.Add(skinCrawler.GetComponent<PlayableCard>());
			}
		}

		__result = newList;
	}
}
